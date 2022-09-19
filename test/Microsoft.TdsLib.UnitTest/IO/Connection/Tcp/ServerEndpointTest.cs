using System;
using Microsoft.TdsLib.IO.Connection.Tcp;
using Xunit;

namespace Microsoft.TdsLib.UnitTest.IO.Connection.Tcp
{
    public class ServerEndpointTest
    {

        [Fact]
        public void TestCreateInvalidEndpoint()
        {
            Assert.Throws<ArgumentNullException>(() => new ServerEndpoint(null, 0));
        }

        [Fact]
        public void TestCreateValidEndpoint()
        {
            string hostname = "test";
            int port = 1433;
            ServerEndpoint serverEndpoint = new ServerEndpoint(hostname, port);

            Assert.Equal(hostname, serverEndpoint.Hostname);
            Assert.Equal(port, serverEndpoint.Port);

            Assert.NotNull(serverEndpoint.ToString());
        }

    }
}
