using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;

namespace engenious.Graphics
{
    /// <summary>
    /// Describes a dynamic vertex array, which can optionally contain positions, colors, normals and texture coordinates.
    /// </summary>
    public class ConditionalVertexArray
    {
        private Array _shit;
        private Type _type;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConditionalVertexArray"/> class.
        /// </summary>
        /// <param name="size">The size of this array.</param>
        /// <param name="hasPositions">Whether the vertex array ought to have position information.</param>
        /// <param name="hasColors">Whether the vertex array ought to have color information.</param>
        /// <param name="hasNormals">Whether the vertex array ought to have normals.</param>
        /// <param name="hasTextureCoordinates">Whether the vertex array ought to have texture coordinates.</param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="hasPositions"/> is set to <c>false</c>.</exception>
        public ConditionalVertexArray(int size, bool hasPositions, bool hasColors, bool hasNormals,
            bool hasTextureCoordinates)
        {
            if (!hasPositions)
                throw new ArgumentException("vertices without positions not known");
            if (hasColors)
            {
                if (hasNormals)
                {
                    if (hasTextureCoordinates)
                    {
                        SetVertices(new VertexPositionNormalColorTexture[size]);
                    }
                    else
                    {
                        SetVertices(new VertexPositionNormalColor[size]);
                    }
                }
                else
                {
                    if (hasTextureCoordinates)
                    {
                        SetVertices(new VertexPositionColorTexture[size]);
                    }
                    else
                    {
                        SetVertices(new VertexPositionColor[size]);
                    }
                }
            }
            else
            {
                if (hasNormals)
                {
                    if (hasTextureCoordinates)
                    {
                        SetVertices(new VertexPositionNormalTexture[size]);
                    }
                    else
                    {
                        SetVertices(new VertexPositionNormal[size]);
                    }
                }
                else
                {
                    if (hasTextureCoordinates)
                    {
                        SetVertices(new VertexPositionTexture[size]);
                    }
                    else
                    {
                        SetVertices(new VertexPosition[size]);
                    }
                }
            }
        }

        /// <summary>
        /// Gets the underlying array as a generic type.
        /// </summary>
        /// <typeparam name="T">The type to get the array as.</typeparam>
        /// <returns>The underlying array.</returns>
        public T[] GetVertices<T>() where T : unmanaged, IVertexType
        {
            return (T[]) _shit;
        }

        /// <summary>
        /// Gets the underlying array.
        /// </summary>
        /// <returns>The underlying array.</returns>
        public Array GetVertices()
        {
            return _shit;
        }

        /// <summary>
        /// Sets this arrays vertices to a new vertex array.
        /// </summary>
        /// <param name="vertices">The vertices to set the this array to.</param>
        /// <typeparam name="T">The type of the input vertex array.</typeparam>
        public void SetVertices<T>(T[] vertices) where T : unmanaged, IVertexType
        {
            SetVertices(vertices,vertices.Length > 0 ? vertices[0].VertexDeclaration : (VertexDeclaration)typeof(T).GetField("VertexDeclaration", BindingFlags.Public | BindingFlags.Static)?.GetValue(null));
        }

        /// <summary>
        /// Sets this arrays vertices to a new vertex array.
        /// </summary>
        /// <param name="vertices">The vertices to set the this array to.</param>
        /// <param name="vertexDeclaration">The <see cref="VertexDeclaration"/> of the given vertex array.</param>
        /// <typeparam name="T">The type of the input vertex array.</typeparam>
        public void SetVertices<T>(T[] vertices, VertexDeclaration vertexDeclaration) where T : unmanaged, IVertexType
        {
            _shit = vertices;

            var type = typeof(T);
            _type = type;

            VertexDeclaration = vertexDeclaration;
            
            
            HasPositions = typeof(IPositionVertex).IsAssignableFrom(type);
            HasColors = typeof(IColorVertex).IsAssignableFrom(type);
            HasNormals = typeof(INormalVertex).IsAssignableFrom(type);
            HasTextureCoordinates = typeof(ITextureCoordinatesVertex).IsAssignableFrom(type);
            
            
            AsPosition = HasPositions ? new GenericIndexer<T,IPositionVertex,Vector3>(vertices) : null;
            AsColor = HasColors ? new GenericIndexer<T,IColorVertex,Color>(vertices) : null;
            AsNormal = HasNormals ? new GenericIndexer<T,INormalVertex,Vector3>(vertices) : null;
            AsTextureCoordinate = HasTextureCoordinates ? new GenericIndexer<T,ITextureCoordinatesVertex,Vector2>(vertices) : null;
        }

        /// <summary>
        /// Gets the associated <see cref="VertexDeclaration"/>.
        /// </summary>
        public VertexDeclaration VertexDeclaration { get; private set; }

        /// <summary>
        /// Gets the length of the vertex array.
        /// </summary>
        public int Length => _shit.Length;

        /// <summary>
        /// Gets whether this vertex array has position information.
        /// </summary>
        public bool HasPositions { get; private set; }

        /// <summary>
        /// Gets whether this vertex array has color information.
        /// </summary>
        public bool HasColors { get; private set; }

        /// <summary>
        /// Gets whether this vertex array has normals.
        /// </summary>
        public bool HasNormals { get; private set; }

        /// <summary>
        /// Gets whether this vertex array has texture coordinates.
        /// </summary>
        public bool HasTextureCoordinates { get; private set; }

        /// <summary>
        /// Gets an indexer which is able to read position information from this array.
        /// </summary>
        public IGenericIndexer<Vector3> AsPosition { get; private set; }

        /// <summary>
        /// Gets an indexer which is able to read color information from this array.
        /// </summary>
        public IGenericIndexer<Color> AsColor { get; private set; }

        /// <summary>
        /// Gets an indexer which is able to read normals from this array.
        /// </summary>
        public IGenericIndexer<Vector3> AsNormal { get; private set; }

        /// <summary>
        /// Gets an indexer which is able to read texture coordinates from this array.
        /// </summary>
        public IGenericIndexer<Vector2> AsTextureCoordinate { get; private set; }
    }

    /// <summary>
    /// Interface for a generic indexer.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IGenericIndexer<T>
    {
        /// <summary>
        /// Gets an element at a given index.
        /// </summary>
        /// <param name="index">The element index to read from.</param>
        T this[int index] { get; set; }
    }

    /// <summary>
    /// Generic indexer implementation for reading <see cref="ConditionalVertexArray"/> information.
    /// </summary>
    /// <typeparam name="T">The base type to read from.</typeparam>
    /// <typeparam name="TU">The vertex interface type to read.</typeparam>
    /// <typeparam name="TUu">The type of the vertex information to read.</typeparam>
    public class GenericIndexer<T,TU,TUu> : IGenericIndexer<TUu>
    {
        private readonly T[] _array;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericIndexer{T,TU,TUu}"/> class.
        /// </summary>
        /// <param name="array">The base array to read from.</param>
        public GenericIndexer(T[] array)
        {
            _array = array;
        }

        static GenericIndexer()
        {
            if (!typeof(TU).IsAssignableFrom(typeof(T)))
                throw new ArgumentException("types not compatible");

            PropertyInfo propInfo = null;
            var retType = typeof(TUu);
            var props = typeof(TU).GetProperties(BindingFlags.Instance | BindingFlags.Public);
            if (props.Length > 2)
                throw new ArgumentException("type not supported");
            foreach (var prop in props)
            {
                if (prop.Name != "VertexDeclaration" && retType == prop.PropertyType)
                    propInfo = prop;
            }
            if (propInfo == null)
                throw new ArgumentException("type not supported");
            
            
            
            
            var arrayParam = Expression.Parameter(typeof(T[]), "array");
            var indexParam = Expression.Parameter(typeof(int), "index");
            var valueParam = Expression.Parameter(retType, "value");
            var arrayAccess = Expression.ArrayAccess(arrayParam, indexParam);
            

            GetValue = Expression.Lambda<Func<T[],int,TUu>>(Expression.Property(Expression.Convert(arrayAccess, typeof(TU)),propInfo),arrayParam,indexParam).Compile();
            SetValue = Expression.Lambda<Action<T[], int, TUu>>(Expression.Assign(Expression.Property(arrayAccess,propInfo), valueParam), arrayParam,
                indexParam, valueParam).Compile();
        }

        private static readonly Func<T[],int,TUu> GetValue;
        private static readonly Action<T[],int,TUu> SetValue;

        /// <inheritdoc />
        public TUu this[int index]
        {
            get => GetValue(_array,index);
            set => SetValue(_array, index, value);
        }
    }
}