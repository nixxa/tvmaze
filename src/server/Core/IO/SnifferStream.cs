using System;
using System.IO;

namespace Core.IO
{
    public class SnifferStream : Stream
    {
        private readonly Stream _master;
        private readonly Stream _sink;

        public SnifferStream(Stream master, Stream sink)
        {
            if (!sink.CanWrite)
            {
                throw new ArgumentException("Sink stream must be writeable", nameof(sink));
            }

            _master = master;
            _sink = sink;
        }

        public override void Flush()
        {
            _master.Flush();
            _sink.Flush();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return _master.Seek(offset, origin);
        }


        public override void SetLength(long value)
        {
            _master.SetLength(value);
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            var bytesRead = _master.Read(buffer, offset, count);
            _sink.Write(buffer, offset, bytesRead);
            return bytesRead;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            if (_master.CanWrite)
                _master.Write(buffer, offset, count);
            if (_sink.CanWrite)
                _sink.Write(buffer, offset, count);
        }

        public override bool CanRead => _master.CanRead;

        public override bool CanSeek => _master.CanSeek;

        public override bool CanWrite => _master.CanWrite;

        public override long Length => _master.Length;

        public override long Position
        {
            get => _master.Position;
            set => _master.Position = value;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _master.Dispose();
                _sink.Dispose();
            }
            base.Dispose(disposing);
        }
    }

}