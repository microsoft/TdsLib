namespace Microsoft.TdsLib.Tokens.EnvChange
{
    /// <summary>
    /// Routing information.
    /// </summary>
    public class RoutingInfo
    {

        /// <summary>
        /// Protocol type.
        /// </summary>
        public byte Protocol { get; }

        /// <summary>
        /// Port.
        /// </summary>
        public ushort Port { get; }

        /// <summary>
        /// Server hostname.
        /// </summary>
        public string Server { get; }
        
        /// <summary>
        /// Creates a new instance of this class.
        /// </summary>
        /// <param name="protocol">Protocol type.</param>
        /// <param name="port">Endpoint port.</param>
        /// <param name="server">Endpoint server hostname.</param>
        public RoutingInfo(byte protocol, ushort port, string server)
        {
            Protocol = protocol;
            Port = port;
            Server = server;
        }

        /// <summary>
        /// Gets a human readable string representation of this object.
        /// </summary>
        /// <returns>Human readable string representation.</returns>
        public override string ToString()
        {
            return $"{nameof(RoutingInfo)}=[{nameof(Protocol)}={Protocol}, {nameof(Server)}={Server}, {nameof(Port)}={Port}]";
        }
    }
}
