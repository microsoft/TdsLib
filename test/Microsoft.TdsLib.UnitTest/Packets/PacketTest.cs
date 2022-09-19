// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using Microsoft.TdsLib.Buffer;
using Microsoft.TdsLib.Packets;
using Xunit;

namespace Microsoft.TdsLib.UnitTest.Packets
{
    public class PacketTest
    {

        [Fact]
        public void CreatePacketSqlBatch()
        {
            Packet packet = new Packet(PacketType.SqlBatch);

            Assert.Equal(PacketType.SqlBatch, packet.Type);
            Assert.Equal(Packet.DefaultPacketId, packet.Id);

            Assert.Equal(Packet.HeaderLength, packet.Length);
            Assert.Equal(0, packet.Data.Length);
        }

        [Fact]
        public void CreatePacketTabularResult()
        {
            Packet packet = new Packet(PacketType.TabularResult);

            Assert.Equal(PacketType.TabularResult, packet.Type);
            Assert.Equal(Packet.DefaultPacketId, packet.Id);

            Assert.Equal(Packet.HeaderLength, packet.Length);
            Assert.Equal(0, packet.Data.Length);
        }

        [Fact]
        public void CreatePacketNullBuffer()
        {
            Assert.Throws<ArgumentNullException>(() => new Packet(null));
        }

        [Fact]
        public void CreatePacketInvalidBufferSize()
        {
            ByteBuffer byteBuffer = new ByteBuffer(new byte[Packet.HeaderLength - 1]);
            Assert.Throws<ArgumentException>(() => new Packet(byteBuffer));
        }

        [Fact]
        public void CreatePacketInvalidType()
        {
            Assert.Throws<ArgumentException>(() => new Packet(new ByteBuffer(new byte[] { 0x17, 0x0B, 0x00, 0x0B, 0x00, 0x00, 0x42, 0x00, 0xEA, 0xEA, 0xEA })));
        }

        [Fact]
        public void CreatePacketWithBuffer()
        {
            Packet packet = new Packet(new ByteBuffer(new byte[] { 0x04, 0x0B, 0x00, 0x0B, 0x00, 0x00, 0x42, 0x00, 0xEA, 0xEA, 0xEA }));

            Assert.Equal(PacketType.TabularResult, packet.Type);
            Assert.Equal(66, packet.Id);
            Assert.True(packet.Last);
            Assert.True(packet.ResetConnection);
            Assert.True(packet.Ignore);
            
            Assert.Equal(Packet.HeaderLength + 3, packet.Length);
            Assert.Equal(3, packet.Data.Length);
        }

        [Fact]
        public void SetIgnoreDefaultValue()
        {
            Packet packet = new Packet(PacketType.SqlBatch);
            Assert.False(packet.Ignore);
        }

        [Fact]
        public void SetIgnoreTrue()
        {
            Packet packet = new Packet(PacketType.SqlBatch)
            {
                Ignore = true
            };

            Assert.True(packet.Ignore);
        }

        [Fact]
        public void SetIgnoreFalse()
        {
            Packet packet = new Packet(PacketType.SqlBatch)
            {
                Ignore = false
            };

            Assert.False(packet.Ignore);
        }

        [Fact]
        public void SetResetConnectionDefaultValue()
        {
            Packet packet = new Packet(PacketType.SqlBatch);
            Assert.False(packet.ResetConnection);
        }

        [Fact]
        public void SetResetConnectionTrue()
        {
            Packet packet = new Packet(PacketType.SqlBatch)
            {
                ResetConnection = true
            };

            Assert.True(packet.ResetConnection);
        }

        [Fact]
        public void SetResetConnectionFalse()
        {
            Packet packet = new Packet(PacketType.SqlBatch)
            {
                ResetConnection = false
            };

            Assert.False(packet.ResetConnection);
        }

        [Fact]
        public void TestStatusResetConnectionIgnore()
        {
            Packet packet = new Packet(PacketType.SqlBatch)
            {
                ResetConnection = true, 
                Ignore = true
            };

            Assert.Equal(PacketStatus.ResetConnection | PacketStatus.Ignore, packet.Status);
        }

        [Fact]
        public void TestStatusIgnoreLast()
        {
            Packet packet = new Packet(PacketType.SqlBatch)
            {
                Last = true,
                Ignore = true
            };

            Assert.Equal(PacketStatus.EOM | PacketStatus.Ignore, packet.Status);
        }

        [Fact]
        public void TestAddData()
        {
            Packet packet = new Packet(PacketType.SqlBatch);

            Assert.Equal(0, packet.Data.Length);

            byte[] data = new byte[] { 0xDE, 0xAD, 0xBE, 0xEF };
            packet.AddData(new ByteBuffer(data));

            Assert.Equal(4, packet.Data.Length);
            Assert.Equal(data, packet.Data.ToArray());
        }

        [Fact]
        public void TestPackedId()
        {
            Packet packet = new Packet(PacketType.SqlBatch);

            Assert.Equal(Packet.DefaultPacketId, packet.Id);

            packet.Id = byte.MaxValue;
            Assert.Equal(byte.MaxValue, packet.Id);

            packet.SetPacketId(byte.MaxValue + 1);
            Assert.Equal(0, packet.Id);
        }

        [Fact]
        public void TestToString()
        {
            Packet packet = new Packet(PacketType.SqlBatch);
            Assert.NotNull(packet.ToString());
        }

    }
}
