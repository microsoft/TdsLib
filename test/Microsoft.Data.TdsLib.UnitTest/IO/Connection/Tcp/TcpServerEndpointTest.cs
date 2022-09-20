// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Microsoft.Data.TdsLib.IO.Connection.Tcp;
using System;
using Xunit;

namespace Microsoft.Data.TdsLib.UnitTest.IO.Connection.Tcp
{
    public class TcpServerEndpointTest
    {

        [Fact]
        public void CreateEndpoint_InvalidHostname()
        {
            Assert.Throws<ArgumentNullException>(() => new TcpServerEndpoint(null, 0));
        }

        [Fact]
        public void CreateEndpoint_InvalidPort_Min()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new TcpServerEndpoint("test", -1));
        }

        [Fact]
        public void CreateEndpoint_InvalidPort_Max()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new TcpServerEndpoint("test", ushort.MaxValue + 1));
        }

        [Fact]
        public void CreateEndpoint_Valid()
        {
            string hostname = "test";
            int port = 1433;
            TcpServerEndpoint serverEndpoint = new TcpServerEndpoint(hostname, port);

            Assert.Equal(hostname, serverEndpoint.Hostname);
            Assert.Equal(port, serverEndpoint.Port);

            Assert.NotNull(serverEndpoint.ToString());
        }

        [Fact]
        public void Equals_Object_Equal()
        {
            string hostname = "test";
            int port = 1433;
            TcpServerEndpoint serverEndpoint1 = new TcpServerEndpoint(hostname, port);
            TcpServerEndpoint serverEndpoint2 = new TcpServerEndpoint(hostname, port);

            Assert.NotSame(serverEndpoint1, serverEndpoint2);
            Assert.Equal(serverEndpoint1, (object)serverEndpoint2);
        }

        [Fact]
        public void Equals_Typed_Equal()
        {
            string hostname = "test";
            int port = 1433;
            TcpServerEndpoint serverEndpoint1 = new TcpServerEndpoint(hostname, port);
            TcpServerEndpoint serverEndpoint2 = new TcpServerEndpoint(hostname, port);

            Assert.NotSame(serverEndpoint1, serverEndpoint2);
            Assert.Equal(serverEndpoint1, serverEndpoint2);
        }

        [Fact]
        public void EqualityOperator_Equal()
        {
            string hostname = "test";
            int port = 1433;
            TcpServerEndpoint serverEndpoint1 = new TcpServerEndpoint(hostname, port);
            TcpServerEndpoint serverEndpoint2 = new TcpServerEndpoint(hostname, port);

            Assert.NotSame(serverEndpoint1, serverEndpoint2);
            Assert.True(serverEndpoint1 == serverEndpoint2);
        }

        [Fact]
        public void EqualityOperator_Equal_LeftNull_RightNull()
        {
            TcpServerEndpoint serverEndpoint1 = null;
            TcpServerEndpoint serverEndpoint2 = null;

            Assert.Same(serverEndpoint1, serverEndpoint2);
            Assert.True(serverEndpoint1 == serverEndpoint2);
        }

        [Fact]
        public void EqualityOperator_NotEqual_DifferentPort()
        {
            string hostname = "test";
            TcpServerEndpoint serverEndpoint1 = new TcpServerEndpoint(hostname, 1433);
            TcpServerEndpoint serverEndpoint2 = new TcpServerEndpoint(hostname, 1466);

            Assert.NotSame(serverEndpoint1, serverEndpoint2);
            Assert.False(serverEndpoint1 == serverEndpoint2);
        }

        [Fact]
        public void EqualityOperator_NotEqual_DifferentHostname()
        {
            int port = 1433;
            TcpServerEndpoint serverEndpoint1 = new TcpServerEndpoint("test1", port);
            TcpServerEndpoint serverEndpoint2 = new TcpServerEndpoint("test2", port);

            Assert.NotSame(serverEndpoint1, serverEndpoint2);
            Assert.False(serverEndpoint1 == serverEndpoint2);
        }

        [Fact]
        public void EqualityOperator_NotEqual_LeftNull_RightNotNull()
        {
            string hostname = "test";
            int port = 1433;
            TcpServerEndpoint serverEndpoint1 = null;
            TcpServerEndpoint serverEndpoint2 = new TcpServerEndpoint(hostname, port);

            Assert.NotSame(serverEndpoint1, serverEndpoint2);
            Assert.False(serverEndpoint1 == serverEndpoint2);
        }

        [Fact]
        public void EqualityOperator_NotEqual_LeftNotNull_RightNull()
        {
            string hostname = "test";
            int port = 1433;
            TcpServerEndpoint serverEndpoint1 = new TcpServerEndpoint(hostname, port);
            TcpServerEndpoint serverEndpoint2 = null;

            Assert.NotSame(serverEndpoint1, serverEndpoint2);
            Assert.False(serverEndpoint1 == serverEndpoint2);
        }

    }
}
