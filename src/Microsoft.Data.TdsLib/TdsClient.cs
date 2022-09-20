// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.IO;
using System.Security.Authentication;
using System.Threading.Tasks;
using Microsoft.Data.TdsLib.Exceptions;
using Microsoft.Data.TdsLib.IO.Connection;
using Microsoft.Data.TdsLib.IO.Connection.Tcp;
using Microsoft.Data.TdsLib.Messages;
using Microsoft.Data.TdsLib.Tokens;

namespace Microsoft.Data.TdsLib
{
    /// <summary>
    /// TDS Client.
    /// </summary>
    public sealed class TdsClient : IDisposable
    {
        /// <summary>
        /// Underlying connection used to communicate with the SQL Server.
        /// </summary>
        public IConnection Connection { get; private set; }

        /// <summary>
        /// The <see cref="MessageHandler"/> of this client.
        /// </summary>
        public MessageHandler MessageHandler { get; }

        /// <summary>
        /// The <see cref="TokenStreamHandler"/> of this client.
        /// </summary>
        public TokenStreamHandler TokenStreamHandler { get; }

        /// <summary>
        /// Creates a new TDS Client and establishes a Tcp connection to the endpoint specified by the <see cref="TcpServerEndpoint"/> using default connection options.
        /// </summary>
        /// <param name="serverEndpoint">The database server endpoint.</param>
        /// <exception cref="IOException">If any IO error occurs.</exception>
        public TdsClient(TcpServerEndpoint serverEndpoint)
            : this(new TcpConnectionOptions(), serverEndpoint)
        {
        }

        /// <summary>
        /// Creates a new TDS Client and establishes a Tcp connection to the endpoint specified by the <see cref="TcpServerEndpoint"/> using the specified <see cref="ConnectionOptions"/>.
        /// </summary>
        /// <param name="options">The connection options.</param>
        /// <param name="serverEndpoint">The database server endpoint.</param>
        /// <exception cref="IOException">If any IO error occurs.</exception>
        public TdsClient(TcpConnectionOptions options, TcpServerEndpoint serverEndpoint)
            : this(new TcpConnection(options, serverEndpoint))
        {
        }

        /// <summary>
        /// Creates a new TDS Client with a connection to a SQL Server.
        /// </summary>
        /// <param name="connection">Underlying connection to use for communication with the SQL Server.</param>
        public TdsClient(IConnection connection)
        {
            Connection = connection ?? throw new ArgumentNullException(nameof(connection));
            MessageHandler = new MessageHandler(this);
            TokenStreamHandler = new TokenStreamHandler(this);
        }

        /// <summary>
        /// Performs the TLS handshake between the client and the database server.
        /// </summary>
        /// <returns>Awaitable task.</returns>
        /// <exception cref="AuthenticationException">If the TLS authentication fails.</exception>
        /// <exception cref="InvalidOperationException">If the TLS authentication is not possible in the current connection phase.</exception>
        /// <exception cref="ConnectionClosedException">If the connected was unexpectedly closed.</exception>
        /// <exception cref="IOException">If any IO error occurs.</exception>
        public async Task PerformTlsHandshake()
        {
            await Connection.StartTLS().ConfigureAwait(false);
        }

        /// <summary>
        /// Closes the connection to the actual database server and re-establishes a Tcp connection to a new database server endpoint.
        /// </summary>
        /// <param name="options">Tcp connection options.</param>
        /// <param name="serverEndpoint">The new database server endpoint.</param>
        /// <exception cref="IOException">If any IO error occurs.</exception>
        public void ReEstablishConnection(TcpConnectionOptions options, TcpServerEndpoint serverEndpoint)
        {
            Connection.Dispose();
            Connection = new TcpConnection(options, serverEndpoint);
        }

        /// <summary>
        /// Closes the connection to the actual database server and re-establishes a connection to a SQL Server.
        /// </summary>
        /// <param name="connection">New connection to use.</param>
        public void ReEstablishConnection(IConnection connection)
        {
            Connection.Dispose();
            Connection = connection ?? throw new ArgumentNullException(nameof(connection));
        }
        
        /// <summary>
        /// Disposes resources from this TDS client and underlying components.
        /// </summary>
        public void Dispose()
        {
            Connection.Dispose();
        }

    }
}
