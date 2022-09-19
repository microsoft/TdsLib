// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Threading.Tasks;
using Microsoft.TdsLib.Buffer;
using Microsoft.TdsLib.Exceptions;

namespace Microsoft.TdsLib.IO.Connection.Tcp
{
    /// <summary>
    /// Connection used by the TDS Client to communicate with the SQL Server.
    /// </summary>
    public sealed class TcpConnection : IConnection
    {
        private readonly Socket socket;
        private readonly ServerEndpoint endpoint;
        private Stream stream;

        /// <inheritdoc/>
        ConnectionOptions IConnection.Options => Options;

        /// <summary>
        /// TCP Connection options.
        /// </summary>
        public TcpConnectionOptions Options { get; private set; }

        /// <summary>
        /// Local Endpoint information.
        /// May be <i>null</i> if the connection is not established.
        /// </summary>
        /// <exception cref="SocketException">An error occurred when attempting to access the socket.</exception>
        /// <exception cref="ObjectDisposedException">If the connection was closed or disposed.</exception>
        public IPEndPoint LocalEndpoint => socket?.LocalEndPoint as IPEndPoint;

        /// <summary>
        /// Remote Endpoint information.
        /// May be <i>null</i> if the connection is not established.
        /// </summary>
        /// <exception cref="SocketException">An error occurred when attempting to access the socket.</exception>
        /// <exception cref="ObjectDisposedException">If the connection was closed or disposed.</exception>
        public IPEndPoint RemoteEndpoint => socket?.RemoteEndPoint as IPEndPoint;

        /// <summary>
        /// Indicates if the connection is established to the remote endpoint.
        /// </summary>
        /// <remarks>
        /// This property does not actively check the connection state, it only updates/remembers the state from the last send/receive operation.
        /// If the current value is True, it does not garantuee that at the current time the connection is still active.
        /// </remarks>
        public bool Connected => socket?.Connected ?? false;

        /// <summary>
        /// Creates a Tcp Connection to a SQL Server endpoint.
        /// </summary>
        /// <param name="options">Connection options.</param>
        /// <param name="serverEndpoint">SQL Server endpoint.</param>
        /// <exception cref="ArgumentNullException">If any parameter is <c>null</c>.</exception>
        /// <exception cref="SocketException">If a problem occurs while setting up the socket.</exception>
        /// <exception cref="IOException">If the socket is not valid for IO.</exception>
        /// <exception cref="OverflowException">If the timeout option values are invalid.</exception>
        /// <exception cref="ArgumentOutOfRangeException">If the timeout option values are invalid.</exception>
        public TcpConnection(TcpConnectionOptions options, ServerEndpoint serverEndpoint)
        {
            endpoint = serverEndpoint ?? throw new ArgumentNullException(nameof(serverEndpoint));
            Options = options ?? throw new ArgumentNullException(nameof(options));

            socket = new Socket(SocketType.Stream, ProtocolType.Tcp)
            {
                NoDelay = true,
                ReceiveTimeout = checked((int)options.ReceiveTimeout.TotalMilliseconds),
                SendTimeout = checked((int)options.SendTimeout.TotalMilliseconds)
            };

            try
            {
                if (options.LocalEndpoint is object)
                {
                    socket.Bind(options.LocalEndpoint);
                }

                IAsyncResult asyncResult = socket.BeginConnect(endpoint.Hostname, endpoint.Port, null, socket);
                bool anySignal = asyncResult.AsyncWaitHandle.WaitOne(options.ConnectTimeout, false);

                if (socket.Connected || anySignal)
                {
                    socket.EndConnect(asyncResult);
                }
                else
                {
                    throw new SocketException((int)SocketError.TimedOut);
                }

                stream = new NetworkStream(socket);
            }
            catch (Exception)
            {
                socket?.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Disposes resources.
        /// </summary>
        public void Dispose()
        {
            stream?.Dispose();
            socket?.Dispose();
        }

        /// <summary>
        /// Starts the SSL/TLS by performing a handshake with the SQL Server.
        /// </summary>
        /// <returns>Awaitable task.</returns>
        public async Task StartTLS()
        {
            PreLoginTlsWrapperStream preLoginStream = new PreLoginTlsWrapperStream(Options.PacketSize, stream);
            WrappedStream wrappedStream = new WrappedStream(preLoginStream);

            SslStream secureStream;
            if (Options.RemoteCertificateValidationCallback is null)
            {
                secureStream = new SslStream(wrappedStream);
            }
            else
            {
                secureStream = new SslStream(wrappedStream, false, Options.RemoteCertificateValidationCallback);
            }

            await secureStream.AuthenticateAsClientAsync(Options.TLSCertificateHostname ?? endpoint.Hostname).ConfigureAwait(false);

            wrappedStream.InnerStream = stream;
            stream = secureStream;
        }

        /// <summary>
        /// Sends data.
        /// </summary>
        /// <param name="byteBuffer"><see cref="ByteBuffer"/> containing data to be sent.</param>
        /// <returns>Awaitable task.</returns>
        public async Task SendData(ByteBuffer byteBuffer)
        {
            await byteBuffer.CopyToAsync(stream).ConfigureAwait(false);
        }

        /// <summary>
        /// Receives data.
        /// </summary>
        /// <returns>Awaitable task. <see cref="ByteBuffer"/> containing received data.</returns>
        public async Task<ByteBuffer> ReceiveData()
        {
            byte[] buffer = new byte[Options.PacketSize];
            int size = await stream.ReadAsync(buffer, 0, buffer.Length).ConfigureAwait(false);

            if (size == 0)
            {
                throw new ConnectionClosedException("Connection closed unexpectedly. Read returned 0 bytes.");
            }

            if (size != buffer.Length)
            {
                return new ByteBuffer(buffer, 0, size);
            }

            return new ByteBuffer(buffer);
        }

        /// <summary>
        /// Clear all incoming data.
        /// </summary>
        public void ClearIncomingData()
        {
            while (socket.Available > 0)
            {
                stream.ReadByte();
            }
        }

    }
}
