using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using engenious.Graphics;

namespace engenious.Content.Serialization
{
    /// <summary>
    /// Content type reader to load <see cref="Effect"/> instances.
    /// </summary>
    [ContentTypeReader(typeof(Effect))]
    public class EffectTypeReader : ContentTypeReader<Effect>
    {
        private readonly EffectInstantiatorTypeReader _instantiatorTypeReader = new();
        /// <inheritdoc />
        public override Effect Read(ContentManagerBase managerBase, ContentReader reader, Type? customType = null)
        {
            return _instantiatorTypeReader.Read(managerBase, reader)
                .CreateInstance(customType);
        }

        /// <inheritdoc />
        public EffectTypeReader()
            : base(1)
        {
        }
    }
}

