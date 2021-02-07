using System;
using engenious.Audio;

namespace engenious.Content.Serialization
{
    /// <summary>
    /// Content type reader to load <see cref="SoundEffect"/> instances.
    /// </summary>
    [ContentTypeReaderAttribute(typeof(SoundEffect))]
    public class SoundEffectTypeReader:ContentTypeReader<SoundEffect>
    {
        /// <inheritdoc />
        public override SoundEffect Read(ContentManagerBase managerBase, ContentReader reader, Type customType = null)
        {
            var format = (SoundEffect.AudioFormat)reader.ReadByte();
            return new SoundEffect(reader.BaseStream,format);
        }
    }
}