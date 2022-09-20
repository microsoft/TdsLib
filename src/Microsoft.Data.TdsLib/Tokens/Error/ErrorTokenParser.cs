// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Threading.Tasks;

namespace Microsoft.Data.TdsLib.Tokens.Error
{
    internal class ErrorTokenParser : TokenParser
    {
        public override async Task<Token> Parse(TokenType tokenType, TokenStreamHandler tokenStreamHandler)
        {
            _ = await tokenStreamHandler.ReadUInt16LE().ConfigureAwait(false);
            uint number = await tokenStreamHandler.ReadUInt32LE().ConfigureAwait(false);
            byte state = await tokenStreamHandler.ReadUInt8().ConfigureAwait(false);
            byte severity = await tokenStreamHandler.ReadUInt8().ConfigureAwait(false);

            string message = await tokenStreamHandler.ReadUsVarChar().ConfigureAwait(false);
            string serverName = await tokenStreamHandler.ReadBVarChar().ConfigureAwait(false);
            string procName = await tokenStreamHandler.ReadBVarChar().ConfigureAwait(false);

            uint lineNumber;
            if (tokenStreamHandler.Options.TdsVersion < TdsVersion.V7_2)
            {
                lineNumber = await tokenStreamHandler.ReadUInt16LE().ConfigureAwait(false);
            }
            else
            {
                lineNumber = await tokenStreamHandler.ReadUInt32LE().ConfigureAwait(false);
            }

            return new ErrorToken(number, state, severity, message, serverName, procName, lineNumber);
        }
    }
}
