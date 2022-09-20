﻿// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Microsoft.Data.Tools.TdsLib.Tokens
{
    /// <summary>
    /// Token type.
    /// </summary>
    public enum TokenType : byte
    {
        /// <summary>
        /// Column data format.
        /// </summary>
        AltMetadata = 0x88,

        /// <summary>
        /// Row of data.
        /// </summary>
        AltRow = 0xD3,

        /// <summary>
        /// Column metadata.
        /// </summary>
        ColMetadata = 0x81,

        /// <summary>
        /// Column information in browse mode.
        /// </summary>
        ColInfo = 0xA5,

        /// <summary>
        /// Done.
        /// </summary>
        Done = 0xFD,

        /// <summary>
        /// Procedure done.
        /// </summary>
        DoneProc = 0xFE,

        /// <summary>
        /// Done in procedure.
        /// </summary>
        DoneInProc = 0xFF,

        /// <summary>
        /// Environment change.
        /// </summary>
        EnvChange = 0xE3,
        
        /// <summary>
        /// Error.
        /// </summary>
        Error = 0xAA,

        /// <summary>
        /// Feature extesion acknowledgment.
        /// </summary>
        FeatureExtAck = 0xAE,

        /// <summary>
        /// Federated authentication information.
        /// </summary>
        FedAuthInfo = 0xEE,

        /// <summary>
        /// Info.
        /// </summary>
        Info = 0xAB,

        /// <summary>
        /// Login acknowledgment.
        /// </summary>
        LoginAck = 0xAD,

        /// <summary>
        /// Row with Null Bitmap Compression.
        /// </summary>
        NbcRow = 0xD2,

        /// <summary>
        /// Offset.
        /// </summary>
        Offset = 0x78,

        /// <summary>
        /// Order.
        /// </summary>
        Order = 0xA9,

        /// <summary>
        /// Return status. 
        /// </summary>
        ReturnStatus = 0x79,

        /// <summary>
        /// Return value.
        /// </summary>
        ReturnValue = 0xAC,

        /// <summary>
        /// Complete Row.
        /// </summary>
        Row = 0xD1,

        /// <summary>
        /// SSPI.
        /// </summary>
        SSPI = 0xED,

        /// <summary>
        /// Table name.
        /// </summary>
        TabName = 0xA4
    }
}
