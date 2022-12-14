// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Microsoft.Data.Tools.TdsLib.Buffer;

namespace Microsoft.Data.Tools.TdsLib.Payloads
{
    /// <summary>
    /// Raw message payload.
    /// </summary>
    public class RawPayload : Payload
    {
        /// <summary>
        /// Create a new instance of this class.
        /// </summary>
        /// <param name="byteBuffer">Buffer with the payload data.</param>
        public RawPayload(ByteBuffer byteBuffer)
        {
            Buffer = byteBuffer;
        }

        /// <summary>
        /// Builds the payload buffer.
        /// </summary>
        protected override void BuildBufferInternal()
        {
            // no operation
        }

        /// <summary>
        /// Gets a human readable string representation of this object.
        /// </summary>
        /// <returns>Human readable string representation.</returns>
        public override string ToString()
        {
            return $"{nameof(RawPayload)}[{nameof(Buffer)}={Buffer}]";
        }

    }
}
