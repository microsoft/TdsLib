// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Microsoft.Data.TdsLib.Tokens.EnvChange
{
    /// <summary>
    /// Database change token.
    /// </summary>
    public sealed class DatabaseEnvChangeToken : EnvChangeToken<string>
    {
        /// <summary>
        /// EnvChange token sub type.
        /// </summary>
        public override EnvChangeTokenSubType SubType => EnvChangeTokenSubType.Database;

        /// <summary>
        /// Create a new instance of this token.
        /// </summary>
        /// <param name="oldValue">Old value./</param>
        /// <param name="newValue">New value.</param>
        public DatabaseEnvChangeToken(string oldValue, string newValue)
            : base(oldValue, newValue)
        {
        }

    }
}
