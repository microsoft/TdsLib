// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Microsoft.Data.TdsLib.Tokens
{
    /// <summary>
    /// Tds data stream token.
    /// </summary>
    public abstract class Token
    {

        /// <summary>
        /// Type of the token.
        /// </summary>
        public abstract TokenType Type { get; }

    }
}
