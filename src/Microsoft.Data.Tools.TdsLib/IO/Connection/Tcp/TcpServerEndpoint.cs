// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

namespace Microsoft.Data.Tools.TdsLib.IO.Connection.Tcp
{
    /// <summary>
    /// TCP server endpoint information.
    /// </summary>
    public class TcpServerEndpoint : IEquatable<TcpServerEndpoint>
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
        public TcpServerEndpoint(string hostname, int port)
        {
            Hostname = hostname ?? throw new ArgumentNullException(nameof(hostname));

            if (port < ushort.MinValue || port > ushort.MaxValue)
            {
                throw new ArgumentOutOfRangeException(nameof(port), $"Specified port is invalid, outside valid range [{ushort.MinValue}-{ushort.MaxValue}]");
            }

            Port = port;
        }

        /// <summary>
        /// Compares this object to another object.
        /// </summary>
        /// <param name="obj">The other object to compare.</param>
        /// <returns>True if the objects are equal, False otherwise.</returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as TcpServerEndpoint);
        }

        /// <inheritdoc/>
        public bool Equals(TcpServerEndpoint other)
        {
            return other != null && 
                Hostname == other.Hostname && 
                Port == other.Port;
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
        public static bool operator ==(TcpServerEndpoint left, TcpServerEndpoint right)
        {
            return (left is null && right is null) || 
                !(left is null) && left.Equals(right);
        }

        /// <summary>
        /// Compares two server endpoints.
        /// </summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand.</param>
        /// <returns>True if the operands are not equal, False otherwise.</returns>
        public static bool operator !=(TcpServerEndpoint left, TcpServerEndpoint right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Gets a human readable string representation of this object.
        /// </summary>
        /// <returns>Human readable string representation.</returns>
        public override string ToString()
        {
            return $"{nameof(TcpServerEndpoint)}[{nameof(Hostname)}={Hostname}, {nameof(Port)}={Port}]";
        }
    }
}
