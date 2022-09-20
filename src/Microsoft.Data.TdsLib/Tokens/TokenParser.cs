// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Threading.Tasks;

namespace Microsoft.Data.TdsLib.Tokens
{
    /// <summary>
    /// Token parser.
    /// </summary>
    internal abstract class TokenParser
    {

        /// <summary>
        /// Parse a token from the token handler.
        /// </summary>
        /// <param name="tokenType">Token type.</param>
        /// <param name="tokenStreamHandler">Token stream handler.</param>
        /// <returns>Awaitable task. Parsed token.</returns>
        public abstract Task<Token> Parse(TokenType tokenType, TokenStreamHandler tokenStreamHandler);

    }
}
