using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace engenious.Graphics
{
    /// <summary>
    ///     Attribute to mark a property to be compatible with a specific material type property.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class MaterialTagAttribute : Attribute
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="MaterialTagAttribute"/> class.
        /// </summary>
        /// <param name="attributeName">
        ///     The name of the material type property attribute the property is compatible with.
        /// </param>
        public MaterialTagAttribute(string attributeName)
        {
            AttributeName = attributeName;
        }

        /// <summary>
        ///     Gets the name of the material type property attribute the property is compatible with.
        /// </summary>
        public string AttributeName { get; }
    }
    internal static class TagReflectionCache<T>
    {
        // ReSharper disable once StaticMemberInGenericType
        public static Dictionary<string, PropertyInfo> Properties { get; } = new();
        static TagReflectionCache()
        {
            foreach (var p in typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                foreach(var tagAttribute in p.GetCustomAttributes<MaterialTagAttribute>())
                    Properties.Add(tagAttribute.AttributeName, p);
            }
        }
    }
    internal static class MaterialSetter<TMaterial, TPreset>
        where TMaterial : Material
        where TPreset : MaterialPreset
    {
        public static readonly Action<TMaterial, TPreset> Set;
        public static readonly Func<TMaterial, TPreset> CreatePreset;

        static MaterialSetter()
        {
            var matProps = TagReflectionCache<TMaterial>.Properties;
            var presProps = TagReflectionCache<TPreset>.Properties;

            {
                var matParam = Expression.Parameter(typeof(TMaterial), "material");
                var presParam = Expression.Parameter(typeof(TPreset), "preset");

                var body = Expression.Block();
                foreach (var (attributeName, presPropInfo) in presProps)
                {
                    if (!matProps.TryGetValue(attributeName, out var matPropInfo) ||
                        !presPropInfo.PropertyType.IsAssignableFrom(matPropInfo.PropertyType))
                        continue;

                    var assignExpression = Expression.Assign(Expression.Property(matParam, matPropInfo),
                        Expression.Property(presParam, presPropInfo));

                    body = Expression.Block(body, assignExpression);
                }

                body = Expression.Block(body, Expression.Call(matParam, "Update", null));

                var action = Expression.Lambda<Action<TMaterial, TPreset>>(body, matParam, presParam);
                Set = action.Compile();
            }
            {
                var matParam = Expression.Parameter(typeof(TMaterial), "material");

                var presVar = Expression.Variable(typeof(TMaterial), "preset");
                
                
                Expression body = Expression.Assign(presVar, Expression.New(typeof(TMaterial)));
                foreach (var (attributeName, matPropInfo) in matProps)
                {
                    if (!presProps.TryGetValue(attributeName, out var presPropInfo) ||
                        !matPropInfo.PropertyType.IsAssignableFrom(presPropInfo.PropertyType))
                        continue;

                    var assignExpression = Expression.Assign(Expression.Property(presVar, presPropInfo),
                        Expression.Property(matParam, matPropInfo));

                    body = Expression.Block(body, assignExpression);
                }

                body = Expression.Block(body, presVar);
                var action = Expression.Lambda<Func<TMaterial, TPreset>>(body, matParam);
                CreatePreset = action.Compile();
            }

        }
    }

    /// <summary>
    ///     Base class for all materials.
    /// </summary>
    public abstract class Material
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Material"/> class.
        /// </summary>
        /// <param name="name">The name of the material.</param>
        protected Material(string name)
        {
            Name = name;
        }

        /// <summary>
        ///     Gets the name of the material.
        /// </summary>
        public string Name { get; }

        /// <summary>
        ///     Updates the material properties on GPU side.
        /// </summary>
        public abstract void Update();
    }

    internal static class MaterialRefRegistry<TMaterial> where TMaterial : Material
    {
        public static Dictionary<TMaterial, List<MaterialRef<TMaterial>>> _references = new();

        public static void RegisterReference(MaterialRef<TMaterial> materialRef)
        {
            if (!_references.TryGetValue(materialRef.Material, out var matRefs))
            {
                matRefs = new List<MaterialRef<TMaterial>>();
                _references.Add(materialRef.Material, matRefs);
            }
            matRefs.Add(materialRef);
        }

        public static void UnregisterReference(MaterialRef<TMaterial> materialRef)
        {
            if (_references.TryGetValue(materialRef.Material, out var matRefs))
            {
                matRefs.Remove(materialRef);
            }
        }

        public static void UpdateReferences(TMaterial material)
        {
            if (_references.TryGetValue(material, out var matRefs))
            {
                foreach (var materialRef in matRefs)
                {
                    materialRef.Update?.Invoke(materialRef);
                }
            }
        }
    }

    /// <summary>
    ///     Represents a reference to a <see cref="engenious.Graphics.Material"/>.
    /// </summary>
    /// <typeparam name="TMaterial">The type of the material to reference.</typeparam>
    public sealed class MaterialRef<TMaterial> : IDisposable where TMaterial : Material
    {
        /// <summary>
        ///     Gets the associated material.
        /// </summary>
        public TMaterial Material { get; }

        /// <summary>
        ///     Gets or sets a delegate that updates the material properties.
        /// </summary>
        public Action<MaterialRef<TMaterial>>? Update { get; set; }
        
        /// <summary>
        ///     Implicitly creates a material reference pointing to the given material.
        /// </summary>
        /// <param name="material">The material the reference should point to.</param>
        /// <returns>The created material reference.</returns>
        public static implicit operator MaterialRef<TMaterial>(TMaterial material)
        {
            return new MaterialRef<TMaterial>(material);
        }
        internal MaterialRef(TMaterial material)
        {
            Material = material;
            
            MaterialRefRegistry<TMaterial>.RegisterReference(this);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            MaterialRefRegistry<TMaterial>.UnregisterReference(this);
        }
    }
    /// <summary>
    ///     Generic base class for materials.
    /// </summary>
    /// <typeparam name="TMaterial">The type of the material itself.</typeparam>
    public abstract class Material<TMaterial> : Material
        where TMaterial : Material
    {
        private readonly TMaterial _materialRef;
        /// <summary>
        ///     Initializes a new instance of the <see cref="Material{TMaterial}"/> class.
        /// </summary>
        /// <param name="name">The name of the material.</param>
        /// <exception cref="NotSupportedException">
        ///     Thrown when <typeparamref name="TMaterial"/> is not a sealed material type,
        ///     <typeparamref name="TMaterial"/> is not the same type as this class type.
        /// </exception>
        protected Material(string name)
            : base(name)
        {
            if (!GetType().IsSealed)
                throw new NotSupportedException("Material implementation should be sealed");
            if (GetType() != typeof(TMaterial))
                throw new NotSupportedException("Material should implement Material<T> with generic parameter of the sealed type.");
            _materialRef = this as TMaterial ?? throw new NotSupportedException("Type of TMaterial needs to be compatible with type of 'this'");
        }

        /// <summary>
        ///     Set the values of all compatible material properties of this material to the given values of the preset.
        /// </summary>
        /// <param name="preset">The preset to take the compatible property values from.</param>
        /// <typeparam name="TPreset">The type of the preset.</typeparam>
        public void Set<TPreset>(TPreset preset) where TPreset : MaterialPreset
        {
            MaterialSetter<TMaterial, TPreset>.Set(_materialRef, preset);
        }

        /// <summary>
        ///     Create a new preset of a specific preset type from all compatible material properties of this material.
        /// </summary>
        /// <typeparam name="TPreset">The type of the preset to create.</typeparam>
        /// <returns>The preset containing all compatible property values of this material.</returns>
        public TPreset CreatePreset<TPreset>() where TPreset : MaterialPreset, new()
        {
            return MaterialSetter<TMaterial, TPreset>.CreatePreset(_materialRef);
        }

        /// <inheritdoc />
        public override void Update()
        {
            MaterialRefRegistry<TMaterial>.UpdateReferences(_materialRef);
        }
    }
}