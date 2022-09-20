// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Microsoft.Data.Tools.TdsLib.Payloads.PreLogin
{
    /// <summary>
    /// Encryption type.
    /// </summary>
    public enum EncryptionType : byte
    {
        /// <summary>
        /// Off.
        /// </summary>
        Off = 0x00,

        /// <summary>
        /// On.
        /// </summary>
        On = 0x01,

        /// <summary>
        /// Not supported.
        /// </summary>
        NotSupported = 0x02,

        /// <summary>
        /// Required.
        /// </summary>
        Required = 0x03,

    }
}
