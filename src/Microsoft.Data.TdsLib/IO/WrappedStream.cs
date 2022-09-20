// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.IO;

namespace Microsoft.Data.TdsLib.IO
{
    internal class WrappedStream : Stream
    {
        public Stream InnerStream { get; set; }

        public override bool CanRead => InnerStream.CanRead;

        public override bool CanSeek => InnerStream.CanSeek;

        public override bool CanWrite => InnerStream.CanWrite;

        public override long Length => InnerStream.Length;

        public override long Position { get => InnerStream.Position; set => InnerStream.Position = value; }

        public WrappedStream(Stream stream)
        {
            InnerStream = stream;
        }

        public override void Flush() => InnerStream.Flush();

        public override int Read(byte[] buffer, int offset, int count) => InnerStream.Read(buffer, offset, count);

        public override long Seek(long offset, SeekOrigin origin) => InnerStream.Seek(offset, origin);

        public override void SetLength(long value) => InnerStream.SetLength(value);

        public override void Write(byte[] buffer, int offset, int count) => InnerStream.Write(buffer, offset, count);
    }
}
