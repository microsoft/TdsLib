using Microsoft.TdsLib.Buffer;

namespace Microsoft.TdsLib.Payloads.PreLogin
{
    internal struct PreLoginOption
    {
        public byte TokenType { get; set; }

        public ByteBuffer ByteBuffer { get; set; }

    }
}
