using System.IO;

namespace engenious.Audio
{
    /// <summary>
    /// A stream that can't be seeked lower than the inital offset.
    /// </summary>
    public class OffsetedStream : Stream
    {
        private readonly Stream _baseStream;
        private readonly long _offset;

        /// <inheritdoc />
        public OffsetedStream(Stream baseStream)
        {
            _offset = baseStream.Position;
            _baseStream = baseStream;
        }

        /// <inheritdoc />
        public override void Flush()
        {
            _baseStream.Flush();
        }

        /// <inheritdoc />
        public override int Read(byte[] buffer, int offset, int count)
        {
            return _baseStream.Read(buffer, offset, count);
        }

        /// <inheritdoc />
        public override long Seek(long offset, SeekOrigin origin)
        {
            return origin switch
            {
                SeekOrigin.Begin => _baseStream.Seek(offset + _offset, origin),
                _ => _baseStream.Seek(offset, origin)
            };
        }

        /// <inheritdoc />
        public override void SetLength(long value)
        {
            _baseStream.SetLength(_offset + value);
        }

        /// <inheritdoc />
        public override void Write(byte[] buffer, int offset, int count)
        {
            _baseStream.Write(buffer, offset, count);
        }

        /// <inheritdoc />
        public override bool CanRead => _baseStream.CanRead;

        /// <inheritdoc />
        public override bool CanSeek => _baseStream.CanSeek;

        /// <inheritdoc />
        public override bool CanWrite => _baseStream.CanWrite;

        /// <inheritdoc />
        public override long Length => _baseStream.Length - _offset;

        /// <inheritdoc />
        public override long Position
        {
            get => _baseStream.Position - _offset;
            set => _baseStream.Position = value + _offset;
        }
    }
}