// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.TdsLib.Buffer;
using Xunit;

namespace Microsoft.Data.TdsLib.UnitTest.Buffer
{
    public class ByteBufferTest
    {

        [Fact]
        public void CreateBuffer()
        {
            _ = new ByteBuffer(16);
        }

        [Fact]
        public void TestToString()
        {
            ByteBuffer buffer = new ByteBuffer(16);
            Assert.NotNull(buffer.ToString());
        }

        [Fact]
        public void TestToStringEmpty()
        {
            ByteBuffer buffer = new ByteBuffer(0);
            Assert.NotNull(buffer.ToString());
        }

        [Fact]
        public void TestLength()
        {
            ByteBuffer buffer = new ByteBuffer(16);
            Assert.Equal(16, buffer.Length);
        }

        [Fact]
        public void TestPositiveInt8()
        {
            ByteBuffer buffer = new ByteBuffer(1);
            sbyte value = sbyte.MaxValue;
            buffer.WriteInt8(value);
            sbyte newValue = buffer.ReadInt8();

            Assert.Equal(value, newValue);
        }

        [Fact]
        public void TestNegativeInt8()
        {
            ByteBuffer buffer = new ByteBuffer(1);
            sbyte value = sbyte.MinValue;
            buffer.WriteInt8(value);
            sbyte newValue = buffer.ReadInt8();

            Assert.Equal(value, newValue);
        }

        [Fact]
        public void TestUInt8()
        {
            ByteBuffer buffer = new ByteBuffer(1);
            byte value = byte.MaxValue;
            buffer.WriteUInt8(value);
            byte newValue = buffer.ReadUInt8();

            Assert.Equal(value, newValue);
        }

        [Fact]
        public void TestPositiveInt16LE()
        {
            ByteBuffer buffer = new ByteBuffer(2);
            short value = short.MaxValue;
            buffer.WriteInt16LE(value);
            short newValue = buffer.ReadInt16LE();

            Assert.Equal(value, newValue);

            byte[] bytes = buffer.ToArray();
            Assert.Equal(0xFF, bytes[0]);
            Assert.Equal(0x7F, bytes[1]);
        }

        [Fact]
        public void TestNegativeInt16LE()
        {
            ByteBuffer buffer = new ByteBuffer(2);
            short value = short.MinValue;
            buffer.WriteInt16LE(value);
            short newValue = buffer.ReadInt16LE();

            Assert.Equal(value, newValue);

            byte[] bytes = buffer.ToArray();
            Assert.Equal(0x00, bytes[0]);
            Assert.Equal(0x80, bytes[1]);
        }

        [Fact]
        public void TestPositiveInt16BE()
        {
            ByteBuffer buffer = new ByteBuffer(2);
            short value = short.MaxValue;
            buffer.WriteInt16BE(value);
            short newValue = buffer.ReadInt16BE();

            Assert.Equal(value, newValue);

            byte[] bytes = buffer.ToArray();
            Assert.Equal(0x7F, bytes[0]);
            Assert.Equal(0xFF, bytes[1]);
        }

        [Fact]
        public void TestNegativeInt16BE()
        {
            ByteBuffer buffer = new ByteBuffer(2);
            short value = short.MinValue;
            buffer.WriteInt16BE(value);
            short newValue = buffer.ReadInt16BE();

            Assert.Equal(value, newValue);

            byte[] bytes = buffer.ToArray();
            Assert.Equal(0x80, bytes[0]);
            Assert.Equal(0x00, bytes[1]);
        }

        [Fact]
        public void TestUInt16LE()
        {
            ByteBuffer buffer = new ByteBuffer(2);
            ushort value = ushort.MaxValue - 1;
            buffer.WriteUInt16LE(value);
            ushort newValue = buffer.ReadUInt16LE();

            Assert.Equal(value, newValue);

            byte[] bytes = buffer.ToArray();
            Assert.Equal(0xFE, bytes[0]);
            Assert.Equal(0xFF, bytes[1]);
        }

        [Fact]
        public void TestUInt16BE()
        {
            ByteBuffer buffer = new ByteBuffer(2);
            ushort value = ushort.MaxValue - 1;
            buffer.WriteUInt16BE(value);
            ushort newValue = buffer.ReadUInt16BE();

            Assert.Equal(value, newValue);

            byte[] bytes = buffer.ToArray();
            Assert.Equal(0xFF, bytes[0]);
            Assert.Equal(0xFE, bytes[1]);
        }

        [Fact]
        public void TestPositiveInt32LE()
        {
            ByteBuffer buffer = new ByteBuffer(4);
            int value = 0x7F_6F_5F_4F;
            buffer.WriteInt32LE(value);
            int newValue = buffer.ReadInt32LE();

            Assert.Equal(value, newValue);

            byte[] bytes = buffer.ToArray();
            Assert.Equal(0x4F, bytes[0]);
            Assert.Equal(0x5F, bytes[1]);
            Assert.Equal(0x6F, bytes[2]);
            Assert.Equal(0x7F, bytes[3]);
        }

        [Fact]
        public void TestNegativeInt32LE()
        {
            ByteBuffer buffer = new ByteBuffer(4);
            int value = -235736076;
            buffer.WriteInt32LE(value);
            int newValue = buffer.ReadInt32LE();

            Assert.Equal(value, newValue);

            byte[] bytes = buffer.ToArray();
            Assert.Equal(0xF4, bytes[0]);
            Assert.Equal(0xF3, bytes[1]);
            Assert.Equal(0xF2, bytes[2]);
            Assert.Equal(0xF1, bytes[3]);
        }

        [Fact]
        public void TestPositiveInt32BE()
        {
            ByteBuffer buffer = new ByteBuffer(4);
            int value = 0x7F_6F_5F_4F;
            buffer.WriteInt32BE(value);
            int newValue = buffer.ReadInt32BE();

            Assert.Equal(value, newValue);

            byte[] bytes = buffer.ToArray();
            Assert.Equal(0x7F, bytes[0]);
            Assert.Equal(0x6F, bytes[1]);
            Assert.Equal(0x5F, bytes[2]);
            Assert.Equal(0x4F, bytes[3]);
        }

        [Fact]
        public void TestNegativeInt32BE()
        {
            ByteBuffer buffer = new ByteBuffer(4);
            int value = -235736076;
            buffer.WriteInt32BE(value);
            int newValue = buffer.ReadInt32BE();

            Assert.Equal(value, newValue);

            byte[] bytes = buffer.ToArray();
            Assert.Equal(0xF1, bytes[0]);
            Assert.Equal(0xF2, bytes[1]);
            Assert.Equal(0xF3, bytes[2]);
            Assert.Equal(0xF4, bytes[3]);
        }

        [Fact]
        public void TestUInt32LE()
        {
            ByteBuffer buffer = new ByteBuffer(4);
            uint value = 0x7F_6F_5F_4F;
            buffer.WriteUInt32LE(value);
            uint newValue = buffer.ReadUInt32LE();

            Assert.Equal(value, newValue);

            byte[] bytes = buffer.ToArray();
            Assert.Equal(0x4F, bytes[0]);
            Assert.Equal(0x5F, bytes[1]);
            Assert.Equal(0x6F, bytes[2]);
            Assert.Equal(0x7F, bytes[3]);
        }

        [Fact]
        public void TestUInt32BE()
        {
            ByteBuffer buffer = new ByteBuffer(4);
            uint value = 0xF1_F2_F3_F4;
            buffer.WriteUInt32BE(value);
            uint newValue = buffer.ReadUInt32BE();

            Assert.Equal(value, newValue);

            byte[] bytes = buffer.ToArray();
            Assert.Equal(0xF1, bytes[0]);
            Assert.Equal(0xF2, bytes[1]);
            Assert.Equal(0xF3, bytes[2]);
            Assert.Equal(0xF4, bytes[3]);
        }

        [Fact]
        public void TestPositiveInt64LE()
        {
            ByteBuffer buffer = new ByteBuffer(sizeof(long));
            long value = 0x0F_1F_2F_3F_4F_5F_6F_7F;
            buffer.WriteInt64LE(value);
            long newValue = buffer.ReadInt64LE();

            Assert.Equal(value, newValue);

            byte[] bytes = buffer.ToArray();
            Assert.Equal(0x7F, bytes[0]);
            Assert.Equal(0x6F, bytes[1]);
            Assert.Equal(0x5F, bytes[2]);
            Assert.Equal(0x4F, bytes[3]);
            Assert.Equal(0x3F, bytes[4]);
            Assert.Equal(0x2F, bytes[5]);
            Assert.Equal(0x1F, bytes[6]);
            Assert.Equal(0x0F, bytes[7]);
        }

        [Fact]
        public void TestNegativeInt64LE()
        {
            ByteBuffer buffer = new ByteBuffer(sizeof(long));
            long value = -4256412436750191;
            buffer.WriteInt64LE(value);
            long newValue = buffer.ReadInt64LE();

            Assert.Equal(value, newValue);

            byte[] bytes = buffer.ToArray();
            Assert.Equal(0x91, bytes[0]);
            Assert.Equal(0xA0, bytes[1]);
            Assert.Equal(0xB0, bytes[2]);
            Assert.Equal(0xC0, bytes[3]);
            Assert.Equal(0xD0, bytes[4]);
            Assert.Equal(0xE0, bytes[5]);
            Assert.Equal(0xF0, bytes[6]);
            Assert.Equal(0xFF, bytes[7]);
        }

        [Fact]
        public void TestPositiveInt64BE()
        {
            ByteBuffer buffer = new ByteBuffer(sizeof(long));
            long value = 0x0F_1F_2F_3F_4F_5F_6F_7F;
            buffer.WriteInt64BE(value);
            long newValue = buffer.ReadInt64BE();

            Assert.Equal(value, newValue);

            byte[] bytes = buffer.ToArray();
            Assert.Equal(0x0F, bytes[0]);
            Assert.Equal(0x1F, bytes[1]);
            Assert.Equal(0x2F, bytes[2]);
            Assert.Equal(0x3F, bytes[3]);
            Assert.Equal(0x4F, bytes[4]);
            Assert.Equal(0x5F, bytes[5]);
            Assert.Equal(0x6F, bytes[6]);
            Assert.Equal(0x7F, bytes[7]);
        }

        [Fact]
        public void TestNegativeInt64BE()
        {
            ByteBuffer buffer = new ByteBuffer(sizeof(long));
            long value = -4256412436750191;
            buffer.WriteInt64BE(value);
            long newValue = buffer.ReadInt64BE();

            Assert.Equal(value, newValue);

            byte[] bytes = buffer.ToArray();
            Assert.Equal(0xFF, bytes[0]);
            Assert.Equal(0xF0, bytes[1]);
            Assert.Equal(0xE0, bytes[2]);
            Assert.Equal(0xD0, bytes[3]);
            Assert.Equal(0xC0, bytes[4]);
            Assert.Equal(0xB0, bytes[5]);
            Assert.Equal(0xA0, bytes[6]);
            Assert.Equal(0x91, bytes[7]);
        }

        [Fact]
        public void TestUInt64LE()
        {
            ByteBuffer buffer = new ByteBuffer(sizeof(ulong));
            ulong value = 0xFF_1F_2F_3F_4F_5F_6F_7F;
            buffer.WriteUInt64LE(value);
            ulong newValue = buffer.ReadUInt64LE();

            Assert.Equal(value, newValue);

            byte[] bytes = buffer.ToArray();
            Assert.Equal(0x7F, bytes[0]);
            Assert.Equal(0x6F, bytes[1]);
            Assert.Equal(0x5F, bytes[2]);
            Assert.Equal(0x4F, bytes[3]);
            Assert.Equal(0x3F, bytes[4]);
            Assert.Equal(0x2F, bytes[5]);
            Assert.Equal(0x1F, bytes[6]);
            Assert.Equal(0xFF, bytes[7]);
        }

        [Fact]
        public void TestUInt64BE()
        {
            ByteBuffer buffer = new ByteBuffer(sizeof(ulong));
            ulong value = 0xFF_1F_2F_3F_4F_5F_6F_7F;
            buffer.WriteUInt64BE(value);
            ulong newValue = buffer.ReadUInt64BE();

            Assert.Equal(value, newValue);

            byte[] bytes = buffer.ToArray();
            Assert.Equal(0xFF, bytes[0]);
            Assert.Equal(0x1F, bytes[1]);
            Assert.Equal(0x2F, bytes[2]);
            Assert.Equal(0x3F, bytes[3]);
            Assert.Equal(0x4F, bytes[4]);
            Assert.Equal(0x5F, bytes[5]);
            Assert.Equal(0x6F, bytes[6]);
            Assert.Equal(0x7F, bytes[7]);
        }

        [Fact]
        public void TestFloatLE()
        {
            ByteBuffer buffer = new ByteBuffer(sizeof(float));
            float value = float.Epsilon;
            buffer.WriteFloatLE(value);
            float newValue = buffer.ReadFloatLE();

            Assert.Equal(value, newValue);

            byte[] bytes = buffer.ToArray();
            byte[] bitConBytes = BitConverter.GetBytes(float.Epsilon);
            Assert.Equal(bitConBytes, bytes);
        }

        [Fact]
        public void TestFloatBE()
        {
            ByteBuffer buffer = new ByteBuffer(sizeof(float));
            float value = float.Epsilon;
            buffer.WriteFloatBE(value);
            float newValue = buffer.ReadFloatBE();

            Assert.Equal(value, newValue);

            byte[] bytes = buffer.ToArray();
            byte[] bitConBytes = BitConverter.GetBytes(float.Epsilon);
            Assert.Equal(bitConBytes, bytes.Reverse().ToArray());
        }

        [Fact]
        public void TestDoubleLE()
        {
            ByteBuffer buffer = new ByteBuffer(sizeof(double));
            double value = double.Epsilon;
            buffer.WriteDoubleLE(value);
            double newValue = buffer.ReadDoubleLE();

            Assert.Equal(value, newValue);

            byte[] bytes = buffer.ToArray();
            byte[] bitConBytes = BitConverter.GetBytes(value);
            Assert.Equal(bitConBytes, bytes);
        }

        [Fact]
        public void TestDoubleBE()
        {
            ByteBuffer buffer = new ByteBuffer(sizeof(double));
            double value = double.Epsilon;
            buffer.WriteDoubleBE(value);
            double newValue = buffer.ReadDoubleBE();

            Assert.Equal(value, newValue);

            byte[] bytes = buffer.ToArray();
            byte[] bitConBytes = BitConverter.GetBytes(value);
            Assert.Equal(bitConBytes, bytes.Reverse().ToArray());
        }

        [Fact]
        public void TestFill()
        {
            ByteBuffer buffer = new ByteBuffer(100);
            buffer.Fill(0xEA);

            Assert.All(buffer.ToArray(), b => Assert.Equal(0xEA, b));
        }

        [Fact]
        public void TestPartialStartFill()
        {
            ByteBuffer buffer = new ByteBuffer(100);
            buffer.Fill(0xEA, 90);

            Assert.All(buffer.Slice(0, 90).ToArray(), b => Assert.Equal(0x00, b));
            Assert.All(buffer.Slice(90).ToArray(), b => Assert.Equal(0xEA, b));
        }

        [Fact]
        public void TestPartialStartEndFill()
        {
            ByteBuffer buffer = new ByteBuffer(100);
            buffer.Fill(0xEA, 90, 5);

            Assert.All(buffer.Slice(0, 90).ToArray(), b => Assert.Equal(0x00, b));
            Assert.All(buffer.Slice(90, 5).ToArray(), b => Assert.Equal(0xEA, b));
            Assert.All(buffer.Slice(95, 5).ToArray(), b => Assert.Equal(0x00, b));
        }

        [Fact]
        public void TestCopyToArray()
        {
            ByteBuffer buffer = new ByteBuffer(10);
            buffer.Fill(0xEA);

            byte[] destinationArray = new byte[5];

            buffer.CopyTo(destinationArray, 0, destinationArray.Length);
            Assert.All(destinationArray, b => Assert.Equal(0xEA, b));
        }

        [Fact]
        public void TestCopyToStream()
        {
            ByteBuffer buffer = new ByteBuffer(10);
            buffer.Fill(0xEA);

            MemoryStream memoryStream = new MemoryStream();
            
            buffer.CopyTo(memoryStream);

            Assert.Equal(buffer.Length, memoryStream.Length);
            Assert.All(memoryStream.ToArray(), b => Assert.Equal(0xEA, b));
        }

        [Fact]
        public async Task TestCopyToStreamAsync()
        {
            ByteBuffer buffer = new ByteBuffer(10);
            buffer.Fill(0xEA);

            MemoryStream memoryStream = new MemoryStream();

            await buffer.CopyToAsync(memoryStream);

            Assert.Equal(buffer.Length, memoryStream.Length);
            Assert.All(memoryStream.ToArray(), b => Assert.Equal(0xEA, b));
        }

        [Fact]
        public void TestToArray()
        {
            ByteBuffer buffer = new ByteBuffer(10);
            buffer.Fill(0xEA);

            byte[] array = buffer.ToArray();

            Assert.Equal(buffer.Length, array.Length);
            Assert.All(array, b => Assert.Equal(0xEA, b));
        }

        [Fact]
        public void TestToArraySegment()
        {
            ByteBuffer buffer = new ByteBuffer(10);
            buffer.Fill(0xEA);

            ArraySegment<byte> arraySegment = buffer.ToArraySegment();

            Assert.Equal(buffer.Length, arraySegment.Count);
            Assert.All(arraySegment, b => Assert.Equal(0xEA, b));
        }

        [Fact]
        public void TestConcatByteArray()
        {
            ByteBuffer buffer = new ByteBuffer(10).Concat(new byte[] { 0x11, 0x11 }); 
            Assert.Equal(12, buffer.Length);
            Assert.All(buffer.Slice(0, 10), b => Assert.Equal(0x00, b));
            Assert.All(buffer.Slice(10, 2), b => Assert.Equal(0x11, b));
        }

        [Fact]
        public void TestConcatByteBuffer()
        {
            ByteBuffer buffer = new ByteBuffer(10).Concat(new ByteBuffer(new byte[] { 0x22, 0x22, 0x22 }));
            Assert.Equal(13, buffer.Length);
            Assert.All(buffer.Slice(0, 10), b => Assert.Equal(0x00, b));
            Assert.All(buffer.Slice(10, 3), b => Assert.Equal(0x22, b));
        }

        [Fact]
        public void TestGetEnumeratorExplicit()
        {
            ByteBuffer buffer = new ByteBuffer(10);
            buffer.Fill(0xEA);

            IEnumerator<byte> enumerator = buffer.GetEnumerator();

            while (enumerator.MoveNext())
            {
                Assert.Equal(0xEA, enumerator.Current);
            }
        }

    }
}
