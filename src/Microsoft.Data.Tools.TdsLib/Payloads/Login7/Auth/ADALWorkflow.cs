// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Microsoft.Data.Tools.TdsLib.Payloads.Login7.Auth
{
    /// <summary>
    /// Active Directory Authentication Library (ADAL) workflow.
    /// </summary>
    public enum ADALWorkflow
    {
        /// <summary>
        /// Username and password.
        /// </summary>
        UserPass = 0x01,

        /// <summary>
        /// Integrated.
        /// </summary>
        Integrated = 0x02
    }
}
