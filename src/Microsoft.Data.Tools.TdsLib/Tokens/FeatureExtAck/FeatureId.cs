// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Microsoft.Data.Tools.TdsLib.Tokens.FeatureExtAck
{
    /// <summary>
    /// Feature identifier.
    /// </summary>
    public enum FeatureId : byte
    {
        /// <summary>
        /// Session recovery.
        /// </summary>
        SessionRecovery = 0x01,

        /// <summary>
        /// Federated authentication.
        /// </summary>
        FedAuth = 0x02,

        /// <summary>
        /// Column encryption.
        /// </summary>
        ColumnEncryption = 0x04,

        /// <summary>
        /// Global transactions.
        /// </summary>
        GlobalTransactions = 0x05,

        /// <summary>
        /// Azure SQL Support.
        /// </summary>
        AzureSqlSupport = 0x08,

        /// <summary>
        /// Feature terminator.
        /// </summary>
        Terminator = 0xFF
    }
}
