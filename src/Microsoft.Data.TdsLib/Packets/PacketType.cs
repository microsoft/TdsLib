// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Microsoft.Data.TdsLib.Packets
{
    /// <summary>
    /// Type of <see cref="Packet"/>.
    /// </summary>
    public enum PacketType : byte
    {
        /// <summary>
        /// Unknown packet type.
        /// </summary>
        Unknown = 0x00,

        /// <summary>
        /// SQL batch.
        /// </summary>
        SqlBatch = 0x01,

        /// <summary>
        /// RPC.
        /// </summary>
        RpcRequest = 0x03,

        /// <summary>
        /// Tabular result.
        /// </summary>
        TabularResult = 0x04,

        /// <summary>
        /// Attention signal.
        /// </summary>
        Attention = 0x06,

        /// <summary>
        /// Bulk load data.
        /// </summary>
        BulkLoad = 0x07,

        /// <summary>
        /// Federated Authentication Token.
        /// </summary>
        FedAuthToken = 0x08,

        /// <summary>
        /// Transaction manager request.
        /// </summary>
        TransactionManager = 0x0E,

        /// <summary>
        /// TDS7 Login.
        /// </summary>
        Login7 = 0x10,

        /// <summary>
        /// SSPI (Security Support Provider Interface).
        /// </summary>
        SSPI = 0x11,

        /// <summary>
        /// Pre-Login.
        /// </summary>
        PreLogin = 0x12,

    }
}
