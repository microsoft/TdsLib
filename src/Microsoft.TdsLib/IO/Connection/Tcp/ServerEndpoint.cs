// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

namespace Microsoft.TdsLib.IO.Connection.Tcp
{
    /// <summary>
    /// Server endpoint information.
    /// </summary>
    public class ServerEndpoint
    {
        /// <summary>
        /// The hostname of the server endpoint.
        /// </summary>
        public string Hostname { get; }

        /// <summary>
        /// The port of the server endpoint.
        /// </summary>
        public int Port { get; }

        /// <summary>
        /// Create a new endpoint with a hostname and port.
        /// </summary>
        /// <param name="hostname">The server endpoint hostname.</param>
        /// <param name="port">The server endpoint port.</param>
        public ServerEndpoint(string hostname, int port)
        {
            Hostname = hostname ?? throw new ArgumentNullException(nameof(hostname));
            Port = port;
        }

        /// <summary>
        /// Compares this object to another object.
        /// </summary>
        /// <param name="obj">The other object to compare.</param>
        /// <returns>True if the objects are equal, False otherwise.</returns>
        public override bool Equals(object obj)
        {
            return obj is ServerEndpoint endpoint &&
                   Hostname == endpoint.Hostname &&
                   Port == endpoint.Port;
        }

        /// <summary>
        /// Gets the hash code of this object.
        /// </summary>
        /// <returns>Hash code of the object.</returns>
        public override int GetHashCode()
        {
            var hashCode = 593727026;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Hostname);
            hashCode = hashCode * -1521134295 + Port.GetHashCode();
            return hashCode;
        }

        /// <summary>
        /// Compares two server endpoints.
        /// </summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand.</param>
        /// <returns>True if the operands are equal, False otherwise.</returns>
        public static bool operator ==(ServerEndpoint left, ServerEndpoint right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Compares two server endpoints.
        /// </summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand.</param>
        /// <returns>True if the operands are not equal, False otherwise.</returns>
        public static bool operator !=(ServerEndpoint left, ServerEndpoint right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Gets a human readable string representation of this object.
        /// </summary>
        /// <returns>Human readable string representation.</returns>
        public override string ToString()
        {
            return $"{nameof(ServerEndpoint)}[{nameof(Hostname)}={Hostname}, {nameof(Port)}={Port}]";
        }
    }
}
