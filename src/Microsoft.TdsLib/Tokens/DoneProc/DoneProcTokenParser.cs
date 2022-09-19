// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Threading.Tasks;
using Microsoft.TdsLib.Tokens.Done;

namespace Microsoft.TdsLib.Tokens.DoneProc
{
    internal sealed class DoneProcTokenParser : TokenParser
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

            return new DoneProcToken(doneStatus, currentCommand, rowCount);
        }
    }
}
