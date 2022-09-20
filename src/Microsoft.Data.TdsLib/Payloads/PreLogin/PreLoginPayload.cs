// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Data.TdsLib.Buffer;

namespace Microsoft.Data.TdsLib.Payloads.PreLogin
{
    /// <summary>
    /// PreLogin payload.
    /// </summary>
    public class PreLoginPayload : Payload
    {
        private const int TokenHeaderSize = 5;
        
        private readonly bool encrypt;

        /// <summary>
        /// Encryption.
        /// </summary>
        public EncryptionType Encryption { get; private set; }

        /// <summary>
        /// Version.
        /// </summary>
        public SqlVersion Version { get; private set; }

        /// <summary>
        /// Instance Id.
        /// </summary>
        public byte Instance { get; private set; }

        /// <summary>
        /// Thread Id.
        /// </summary>
        public uint ThreadId { get; private set; }

        /// <summary>
        /// Mars.
        /// </summary>
        public byte Mars { get; private set; }

        /// <summary>
        /// Federate authentication.
        /// </summary>
        public byte FedAuth { get; private set; }

        /// <summary>
        /// Create a new instance of this class.
        /// </summary>
        /// <param name="encrypt">True to enable encryption, false to disable.</param>
        public PreLoginPayload(bool encrypt = false)
        {
            this.encrypt = encrypt;
            BuildBufferInternal();
            ExtractBufferData();
        }

        /// <summary>
        /// Create a new instance of this class from a raw buffer.
        /// </summary>
        /// <param name="buffer">The raw payload buffer.</param>
        public PreLoginPayload(ByteBuffer buffer)
        {
            Buffer = buffer;
            ExtractBufferData();
        }

        /// <summary>
        /// Builds the payload buffer.
        /// </summary>
        protected override void BuildBufferInternal()
        {
            List<PreLoginOption> optionList = new List<PreLoginOption>
            {
                GetVersionOption(),
                GetEncryptionOption(),
                GetInstanceOption(),
                GetThreadIdOption(),
                GetMarsOption(),
                GetFedAuthOption()
            };

            int dataLength = optionList.Sum(b => TokenHeaderSize + b.ByteBuffer.Length) + 1;

            Buffer = new ByteBuffer(dataLength);
            int offsetOption = 0;
            int offsetData = TokenHeaderSize * optionList.Count + 1;

            foreach (var option in optionList)
            {
                Buffer.WriteUInt8(option.TokenType, offsetOption);
                Buffer.WriteUInt16BE((ushort)offsetData, offsetOption + 1);
                Buffer.WriteUInt16BE((ushort)option.ByteBuffer.Length, offsetOption + 3);

                Buffer.Write(option.ByteBuffer, offsetData);

                offsetOption += TokenHeaderSize;
                offsetData += option.ByteBuffer.Length;
            }

            Buffer.WriteUInt8(TokenType.Terminator, offsetOption);
        }

        private PreLoginOption GetVersionOption()
        {
            ByteBuffer byteBuffer = new ByteBuffer(6);
            int offset = 0;

            Version version = GetLibraryVersion();

            offset = byteBuffer.WriteUInt8((byte)(version.Major & 0xff), offset);
            offset = byteBuffer.WriteUInt8((byte)(version.Minor & 0xff), offset);

            offset = byteBuffer.WriteUInt8((byte)((version.Build & 0xff00) >> 8), offset);
            offset = byteBuffer.WriteUInt8((byte)(version.Build & 0xff), offset);

            offset = byteBuffer.WriteUInt8((byte)(version.Revision & 0xff), offset);
            byteBuffer.WriteUInt8((byte)((version.Revision & 0xff00) >> 8), offset);

            return new PreLoginOption { TokenType = TokenType.Version, ByteBuffer = byteBuffer };
        }

        private Version GetLibraryVersion()
        {
            AssemblyName assemblyName = typeof(PreLoginPayload).Assembly.GetName();

            if (assemblyName?.Version is object)
            {
                return assemblyName.Version;
            }

            return new Version(0, 0, 1, 0);
        }

        private PreLoginOption GetEncryptionOption()
        {
            ByteBuffer byteBuffer = new ByteBuffer(1);
            if (encrypt)
            {
                byteBuffer.WriteUInt8((byte)EncryptionType.On);
            }
            else
            {
                byteBuffer.WriteUInt8((byte)EncryptionType.NotSupported);
            }
            return new PreLoginOption { TokenType = TokenType.Encryption, ByteBuffer = byteBuffer };
        }

        private PreLoginOption GetInstanceOption()
        {
            ByteBuffer byteBuffer = new ByteBuffer(1);
            byteBuffer.WriteUInt8(0x00);
            return new PreLoginOption { TokenType = TokenType.InstOpt, ByteBuffer = byteBuffer };
        }

        private PreLoginOption GetThreadIdOption()
        {
            ByteBuffer byteBuffer = new ByteBuffer(4);
            byteBuffer.WriteUInt32BE(0x00);
            return new PreLoginOption { TokenType = TokenType.ThreadId, ByteBuffer = byteBuffer };
        }

        private PreLoginOption GetMarsOption()
        {
            ByteBuffer byteBuffer = new ByteBuffer(1);
            byteBuffer.WriteUInt8(MarsType.Off);
            return new PreLoginOption { TokenType = TokenType.Mars, ByteBuffer = byteBuffer };
        }

        private PreLoginOption GetFedAuthOption()
        {
            ByteBuffer byteBuffer = new ByteBuffer(1);
            byteBuffer.WriteUInt8(0x01);
            return new PreLoginOption { TokenType = TokenType.FedAuthRequired, ByteBuffer = byteBuffer };
        }

        private void ExtractBufferData()
        {
            int offset = 0;

            while (Buffer.ReadUInt8(offset) != TokenType.Terminator)
            {
                byte token = Buffer.ReadUInt8(offset);
                ushort dataOffset = Buffer.ReadUInt16BE(offset + 1);
                ushort dataLength = Buffer.ReadUInt16BE(offset + 3);

                if (dataLength > 0)
                {
                    switch (token)
                    {
                        case TokenType.Version:
                            ParseVersion(dataOffset);
                            break;
                        case TokenType.Encryption:
                            ParseEncryption(dataOffset);
                            break;
                        case TokenType.InstOpt:
                            ParseInstance(dataOffset);
                            break;
                        case TokenType.ThreadId:
                            ParseThreadId(dataOffset);
                            break;
                        case TokenType.Mars:
                            ParseMars(dataOffset);
                            break;
                        case TokenType.FedAuthRequired:
                            ParseFedAuth(dataOffset);
                            break;
                        default:
                            throw new InvalidOperationException($"PreLogin payload is malformed at offset: {offset}, Token type: {token:X}");
                    }
                }

                offset += TokenHeaderSize;
            }

        }

        private void ParseVersion(ushort dataOffset)
        {
            Version = new SqlVersion
            {
                Major = Buffer.ReadUInt8(dataOffset),
                Minor = Buffer.ReadUInt8(dataOffset + 1),
                Patch = Buffer.ReadUInt8(dataOffset + 2),
                Trivial = Buffer.ReadUInt8(dataOffset + 3),
                SubBuild = Buffer.ReadUInt16BE(dataOffset + 4)
            };
        }

        private void ParseEncryption(ushort dataOffset)
        {
            byte encryptionValue = Buffer.ReadUInt8(dataOffset);

            if (!Enum.IsDefined(typeof(EncryptionType), encryptionValue))
            {
                throw new InvalidOperationException($"Invalid encryption type: 0x{encryptionValue:X2}");
            }

            Encryption = (EncryptionType)encryptionValue;
        }

        private void ParseInstance(ushort dataOffset)
        {
            Instance = Buffer.ReadUInt8(dataOffset);
        }

        private void ParseThreadId(ushort dataOffset)
        {
            ThreadId = Buffer.ReadUInt32BE(dataOffset);
        }

        private void ParseMars(ushort dataOffset)
        {
            Mars = Buffer.ReadUInt8(dataOffset);
        }

        private void ParseFedAuth(ushort dataOffset)
        {
            FedAuth = Buffer.ReadUInt8(dataOffset);
        }

        /// <summary>
        /// Gets a human readable string representation of this object.
        /// </summary>
        /// <returns>Human readable string representation.</returns>
        public override string ToString()
        {
            return $"PreLoginPayload[Version={Version}, " +
                $"Encryption=0x{(byte)Encryption:X2}({Encryption}), " +
                $"Instance=0x{Instance:X2}, " +
                $"ThreadId=0x{ThreadId:X8}, " +
                $"Mars=0x{Mars:X2}({MarsType.GetString(Mars)})]";
        }

    }
}
