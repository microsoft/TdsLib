using Microsoft.TdsLib.IO.Connection;
using System;
using Xunit;

namespace Microsoft.TdsLib.UnitTest.IO.Connection
{
    public class ConnectionOptionsTest
    {

        [Fact]
        public void TestCreate()
        {
            _ = new ConnectionOptions();
        }

        [Fact]
        public void TestSetValidPacketSize()
        {
            ConnectionOptions connectionOptions = new ConnectionOptions
            {
                PacketSize = 8192
            };

            Assert.Equal(8192, connectionOptions.PacketSize);
        }

        [Fact]
        public void TestSetInvalidPacketSize()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                _ = new ConnectionOptions
                {
                    PacketSize = 4
                };
            });
        }

    }
}
