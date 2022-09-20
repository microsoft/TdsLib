// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Microsoft.Data.Tools.TdsLib.Buffer;

namespace Microsoft.Data.Tools.TdsLib.Payloads
{
    /// <summary>
    /// Payload for messages.
    /// </summary>
    public abstract class Payload
    {
        /// <summary>
        /// The buffer with the payload data.
        /// </summary>
        public ByteBuffer Buffer { get; protected set; }

        /// <summary>
        /// Internally builds the payload buffer.
        /// </summary>
        protected abstract void BuildBufferInternal();

        /// <summary>
        /// Builds the payload buffer.
        /// </summary>
        /// <returns>Buffer with the payload raw data.</returns>
        public ByteBuffer BuildBuffer()
        {
            BuildBufferInternal();
            return Buffer;
        }

    }
}
