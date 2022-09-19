using System.Threading.Tasks;

namespace Microsoft.TdsLib.Tokens.Done
{
    internal sealed class DoneTokenParser : TokenParser
    {
        public override async Task<Token> Parse(TokenType tokenType, TokenStreamHandler tokenStreamHandler)
        {
            ushort status = await tokenStreamHandler.ReadUInt16LE().ConfigureAwait(false);
            DoneStatus doneStatus = (DoneStatus)status;

            ushort currentCommand = await tokenStreamHandler.ReadUInt16LE().ConfigureAwait(false);

            ulong rowCount;
            if (tokenStreamHandler.Options.TdsVersion > TdsVersion.V7_2)
            {
                rowCount = await tokenStreamHandler.ReadUInt64LE().ConfigureAwait(false);
            }
            else
            {
                rowCount = await tokenStreamHandler.ReadUInt32LE().ConfigureAwait(false);
            }

            return new DoneToken(doneStatus, currentCommand, rowCount);
        }
    }
}
