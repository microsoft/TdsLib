using System;
using System.Net;
using System.Net.Security;

namespace Microsoft.TdsLib.IO.Connection.Tcp
{
    /// <summary>
    /// Connection options for Tcp connection.
    /// </summary>
    public class TcpConnectionOptions : ConnectionOptions
    {
        /// <summary>
        /// Local endpoint to use for the socket.
        /// If this value is <i>null</i> then the local endpoint will be assigned by the operating system.
        /// </summary>
        public IPEndPoint LocalEndpoint { get; set; }

        /// <summary>
        /// Connect timeout for the connection. 
        /// A value of <c>TimeSpan.FromMilliseconds(-1)</c> indicates default operating system timeout. Default value is the operating system timeout.
        /// </summary>
        public TimeSpan ConnectTimeout { get; set; } = TimeSpan.FromMilliseconds(-1);

        /// <summary>
        /// Receive timeout for the connection. 
        /// A value of <see cref="TimeSpan.Zero"/> indicates infinite timeout. Default value is 5 seconds.
        /// </summary>
        public TimeSpan ReceiveTimeout { get; set; } = TimeSpan.FromSeconds(5);

        /// <summary>
        /// Send timeout for the connection.
        /// A value of <see cref="TimeSpan.Zero"/> indicates infinite timeout. Default value is 5 seconds.
        /// </summary>
        public TimeSpan SendTimeout { get; set; } = TimeSpan.FromSeconds(5);

        /// <summary>
        /// Optional remote certificate validation callback.
        /// </summary>
        public RemoteCertificateValidationCallback RemoteCertificateValidationCallback { get; set; }

        /// <summary>
        /// Optional hostname to be used for TLS server certificate name validation.
        /// </summary>
        public string TLSCertificateHostname { get; set; }

    }
}
