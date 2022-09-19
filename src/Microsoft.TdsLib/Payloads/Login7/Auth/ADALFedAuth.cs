// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Microsoft.TdsLib.Buffer;

namespace Microsoft.TdsLib.Payloads.Login7.Auth
{

    /// <summary>
    /// Active Directory Authentication Library Federate authentication.
    /// </summary>
    public sealed class ADALFedAuth : FedAuth
    {
        /// <summary>
        /// Echo.
        /// </summary>
        public bool Echo { get; }

        /// <summary>
        /// Workflow.
        /// </summary>
        public ADALWorkflow Workflow { get; }

        /// <summary>
        /// Creates a new instance of this class.
        /// </summary>
        /// <param name="workflow">Workflow.</param>
        /// <param name="echo">Echo.</param>
        public ADALFedAuth(ADALWorkflow workflow, bool echo = false)
        {
            Workflow = workflow;
            Echo = echo;
        }

        internal override ByteBuffer GetBuffer()
        {
            ByteBuffer buffer = new ByteBuffer(7);
            int offset = buffer.WriteUInt8(FeatureId);
            offset = buffer.WriteUInt32LE(2, offset);

            byte options = (byte)(LibraryADAL | (Echo ? FedAuthEchoYes : FedAuthEchoNo));
            offset = buffer.WriteUInt8(options, offset);
            buffer.WriteUInt8((byte)Workflow, offset);
            return buffer;
        }
    }
}
