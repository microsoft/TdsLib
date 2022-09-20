﻿// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Microsoft.Data.Tools.TdsLib.Tokens
{
    /// <summary>
    /// Token event.
    /// </summary>
    public sealed class TokenEvent
    {
        /// <summary>
        /// The token that was received.
        /// </summary>
        public Token Token { get; internal set; }

        /// <summary>
        /// Indicate if the token handler should stop receiving tokens.
        /// </summary>
        public bool Exit { get; set; }

    }
}
