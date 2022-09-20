// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Microsoft.Data.Tools.TdsLib.Tokens.FedAuthInfo
{
    /// <summary>
    /// Federate authentication information identifier.
    /// </summary>
    internal enum FedAuthInfoId : byte
    {
        /// <summary>
        /// Identifier for SPN.
        /// </summary>
        SPN = 0x02,

        /// <summary>
        /// Identifier for STSUrl.
        /// </summary>
        STSUrl = 0x01
    }
}
