﻿// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Threading.Tasks;

namespace Microsoft.Data.Tools.TdsLib.Tokens.ReturnStatus
{
    internal class ReturnStatusTokenParser : TokenParser
    {
        public override async Task<Token> Parse(TokenType tokenType, TokenStreamHandler tokenStreamHandler)
        {
            return new ReturnStatusToken(await tokenStreamHandler.ReadInt32LE().ConfigureAwait(false));
        }
    }
}
