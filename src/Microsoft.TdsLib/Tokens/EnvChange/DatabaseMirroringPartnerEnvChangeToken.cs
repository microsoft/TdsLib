// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Microsoft.TdsLib.Tokens.EnvChange
{
    /// <summary>
    /// Database mirroring partner envirionment change token.
    /// </summary>
    public sealed class DatabaseMirroringPartnerEnvChangeToken : EnvChangeToken<string>
    {

        /// <summary>
        /// EnvChange token sub type.
        /// </summary>
        public override EnvChangeTokenSubType SubType => EnvChangeTokenSubType.DatabaseMirroringPartner;

        /// <summary>
        /// Create a new instance of this token.
        /// </summary>
        /// <param name="oldValue">Old value./</param>
        /// <param name="newValue">New value.</param>
        public DatabaseMirroringPartnerEnvChangeToken(string oldValue, string newValue) : base(oldValue, newValue)
        {
        }

    }
}
