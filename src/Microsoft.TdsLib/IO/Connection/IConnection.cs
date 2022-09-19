// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Threading.Tasks;
using Microsoft.TdsLib.Buffer;

namespace Microsoft.TdsLib.IO.Connection
{
    /// <summary>
    /// Connection to the SQL Server.
    /// </summary>
    public interface IConnection : IDisposable
    {

        /// <summary>
        /// Connection options.
        /// </summary>
        ConnectionOptions Options { get; }

        /// <summary>
        /// Starts the SSL/TLS by performing a handshake with the SQL Server.
        /// </summary>
        /// <returns>Awaitable task.</returns>
        Task StartTLS();

        /// <summary>
        /// Sends data.
        /// </summary>
        /// <param name="byteBuffer"><see cref="ByteBuffer"/> containing data to be sent.</param>
        /// <returns>Awaitable task.</returns>
        Task SendData(ByteBuffer byteBuffer);

        /// <summary>
        /// Receives data.
        /// </summary>
        /// <returns>Awaitable task. <see cref="ByteBuffer"/> containing received data.</returns>
        Task<ByteBuffer> ReceiveData();

        /// <summary>
        /// Clear all incoming data.
        /// </summary>
        void ClearIncomingData();

    }
}
