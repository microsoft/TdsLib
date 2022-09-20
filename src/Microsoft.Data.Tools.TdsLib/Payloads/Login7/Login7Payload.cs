// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Data.Tools.TdsLib.Buffer;
using Microsoft.Data.Tools.TdsLib.Payloads.Login7.Auth;

namespace Microsoft.Data.Tools.TdsLib.Payloads.Login7
{
    /// <summary>
    /// Login7 payload.
    /// </summary>
    public sealed class Login7Payload : Payload
    {

        private const byte FeatureExtensionTerminator = 0xFF;
        private const int ClientIdSize = 6;
        
        /// <summary>
        /// Login7 options.
        /// </summary>
        public Login7Options Options { get; set; }

        /// <summary>
        /// Option flags 1.
        /// </summary>
        public OptionFlags1 OptionFlags1 { get; set; }

        /// <summary>
        /// Option flags 2.
        /// </summary>
        public OptionFlags2 OptionFlags2 { get; set; }

        /// <summary>
        /// Option flags 3.
        /// </summary>
        public OptionFlags3 OptionFlags3 { get; set; }

        /// <summary>
        /// Type flags.
        /// </summary>
        public TypeFlags TypeFlags { get; set; }

        /// <summary>
        /// Username.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Password.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Server name.
        /// </summary>
        public string ServerName { get; set; }

        /// <summary>
        /// Application name.
        /// </summary>
        public string AppName { get; set; }

        /// <summary>
        /// Client hostname.
        /// </summary>
        public string Hostname { get; set; }

        /// <summary>
        /// Library name.
        /// </summary>
        public string LibraryName { get; set; }

        /// <summary>
        /// Language.
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// Database name.
        /// </summary>
        public string Database { get; set; }

        /// <summary>
        /// Client Identifier.
        /// Normally the MAC address or an identifier with 6 bytes.
        /// </summary>
        public ByteBuffer ClientId { get; set; }

        /// <summary>
        /// SSPI.
        /// </summary>
        public ByteBuffer SSPI { get; set; }

        /// <summary>
        /// Attach Database file.
        /// </summary>
        public string AttachDbFile { get; set; }

        /// <summary>
        /// New password to set.
        /// </summary>
        public string ChangePassword { get; set; }

        /// <summary>
        /// Federate authentication.
        /// </summary>
        public FedAuth FedAuth { get; set; }

        /// <summary>
        /// Creates a new Login7 payload instance.
        /// </summary>
        /// <param name="options">Login7 options.</param>
        public Login7Payload(Login7Options options = default)
        {
            Options = options ?? new Login7Options();
            OptionFlags1 = new OptionFlags1();
            OptionFlags2 = new OptionFlags2();
            OptionFlags3 = new OptionFlags3();
            TypeFlags = new TypeFlags();
            LibraryName = "TdsLib";

            BuildBufferInternal();
        }

        /// <summary>
        /// Builds the payload buffer.
        /// </summary>
        protected override void BuildBufferInternal()
        {
            List<ByteBuffer> buffers = new List<ByteBuffer>();

            ByteBuffer buffer = new ByteBuffer(94);
            buffers.Add(buffer);

            int offset = 4;
            int dataOffset = buffer.Length;

            // Options
            //
            offset = buffer.WriteUInt32LE((uint)Options.TdsVersion, offset);
            offset = buffer.WriteUInt32LE(Options.PacketSize, offset);
            offset = buffer.WriteUInt32LE(Options.ClientProgVer, offset);
            offset = buffer.WriteUInt32LE(Options.ClientPid, offset);
            offset = buffer.WriteUInt32LE(Options.ConnectionId, offset);

            offset = buffer.WriteUInt8((byte)OptionFlags1, offset);
            offset = buffer.WriteUInt8((byte)OptionFlags2, offset);
            offset = buffer.WriteUInt8((byte)TypeFlags, offset);
            offset = buffer.WriteUInt8((byte)OptionFlags3, offset);

            offset = buffer.WriteInt32LE(Options.ClientTimeZone, offset);
            offset = buffer.WriteUInt32LE(Options.ClientLcid, offset);

            // Hostname
            //
            offset = buffer.WriteUInt16LE((ushort)dataOffset, offset);

            if (string.IsNullOrEmpty(Hostname))
            {
                offset = buffer.WriteUInt16LE(0, offset);
            }
            else
            {
                ByteBuffer hostnameBuffer = new ByteBuffer(Encoding.Unicode.GetBytes(Hostname));
                buffers.Add(hostnameBuffer);

                offset = buffer.WriteUInt16LE((ushort)Hostname.Length, offset);
                dataOffset += hostnameBuffer.Length;
            }

            // Username
            //
            offset = buffer.WriteUInt16LE((ushort)dataOffset, offset);

            if (string.IsNullOrEmpty(Username))
            {
                offset = buffer.WriteUInt16LE(0, offset);
            }
            else
            {
                ByteBuffer usernameBuffer = new ByteBuffer(Encoding.Unicode.GetBytes(Username));
                buffers.Add(usernameBuffer);

                offset = buffer.WriteUInt16LE((ushort)Username.Length, offset);
                dataOffset += usernameBuffer.Length;
            }

            // Password
            //
            offset = buffer.WriteUInt16LE((ushort)dataOffset, offset);

            if (string.IsNullOrEmpty(Password))
            {
                offset = buffer.WriteUInt16LE(0, offset);
            }
            else
            {
                ByteBuffer passwordBuffer = new ByteBuffer(ScramblePassword(Encoding.Unicode.GetBytes(Password)));
                buffers.Add(passwordBuffer);

                offset = buffer.WriteUInt16LE((ushort)Password.Length, offset);
                dataOffset += passwordBuffer.Length;
            }

            // AppName
            //
            offset = buffer.WriteUInt16LE((ushort)dataOffset, offset);

            if (string.IsNullOrEmpty(AppName))
            {
                offset = buffer.WriteUInt16LE(0, offset);
            }
            else
            {
                ByteBuffer appNameBuffer = new ByteBuffer(Encoding.Unicode.GetBytes(AppName));
                buffers.Add(appNameBuffer);

                offset = buffer.WriteUInt16LE((ushort)AppName.Length, offset);
                dataOffset += appNameBuffer.Length;
            }

            // ServerName
            //
            offset = buffer.WriteUInt16LE((ushort)dataOffset, offset);

            if (string.IsNullOrEmpty(ServerName))
            {
                offset = buffer.WriteUInt16LE(0, offset);
            }
            else
            {
                ByteBuffer serverNameBuffer = new ByteBuffer(Encoding.Unicode.GetBytes(ServerName));
                buffers.Add(serverNameBuffer);

                offset = buffer.WriteUInt16LE((ushort)ServerName.Length, offset);
                dataOffset += serverNameBuffer.Length;
            }

            // Extensions
            //
            offset = buffer.WriteUInt16LE((ushort)dataOffset, offset);

            ByteBuffer extensionsBuffer = GetExtensionsBuffer();
            offset = buffer.WriteInt16LE(sizeof(uint), offset);

            dataOffset += sizeof(uint);
            ByteBuffer extensionOffsetBuffer = new ByteBuffer(sizeof(uint));
            extensionOffsetBuffer.WriteUInt32LE((uint)dataOffset);

            dataOffset += extensionsBuffer.Length;

            buffers.Add(extensionOffsetBuffer);
            buffers.Add(extensionsBuffer);

            // Library Name
            //
            offset = buffer.WriteUInt16LE((ushort)dataOffset, offset);

            if (string.IsNullOrEmpty(LibraryName))
            {
                offset = buffer.WriteInt16LE(0, offset);
            }
            else
            {
                ByteBuffer libraryNameBuffer = new ByteBuffer(Encoding.Unicode.GetBytes(LibraryName));
                buffers.Add(libraryNameBuffer);

                offset = buffer.WriteUInt16LE((ushort)LibraryName.Length, offset);
                dataOffset += libraryNameBuffer.Length;
            }

            // Language
            //
            offset = buffer.WriteUInt16LE((ushort)dataOffset, offset);

            if (string.IsNullOrEmpty(Language))
            {
                offset = buffer.WriteInt16LE(0, offset);
            }
            else
            {
                ByteBuffer languageBuffer = new ByteBuffer(Encoding.Unicode.GetBytes(Language));
                buffers.Add(languageBuffer);

                offset = buffer.WriteUInt16LE((ushort)Language.Length, offset);
                dataOffset += languageBuffer.Length;
            }

            // Database
            //
            offset = buffer.WriteUInt16LE((ushort)dataOffset, offset);

            if (string.IsNullOrEmpty(Database))
            {
                offset = buffer.WriteInt16LE(0, offset);
            }
            else
            {
                ByteBuffer databaseBuffer = new ByteBuffer(Encoding.Unicode.GetBytes(Database));
                buffers.Add(databaseBuffer);

                offset = buffer.WriteUInt16LE((ushort)Database.Length, offset);
                dataOffset += databaseBuffer.Length;
            }

            // Client ID
            //
            if (ClientId is null)
            {
                ClientId = GenerateRandomPhysicalAddress();
            }

            buffer.Write(ClientId, offset, 0, ClientIdSize);
            offset += 6;

            // SSPI
            //
            offset = buffer.WriteUInt16LE((ushort)dataOffset, offset);

            if (SSPI is object)
            {
                if (SSPI.Length > ushort.MaxValue)
                {
                    offset = buffer.WriteUInt16LE(ushort.MaxValue, offset);
                }
                else
                {
                    offset = buffer.WriteUInt16LE((ushort)SSPI.Length, offset);
                }

                buffers.Add(SSPI);
            }
            else
            {
                offset = buffer.WriteUInt16LE(0, offset);
            }

            // AttachDB
            //
            offset = buffer.WriteUInt16LE((ushort)dataOffset, offset);

            if (string.IsNullOrEmpty(AttachDbFile))
            {
                offset = buffer.WriteInt16LE(0, offset);
            }
            else
            {
                ByteBuffer attachDbFileBuffer = new ByteBuffer(Encoding.Unicode.GetBytes(AttachDbFile));
                buffers.Add(attachDbFileBuffer);

                offset = buffer.WriteUInt16LE((ushort)AttachDbFile.Length, offset);
                dataOffset += attachDbFileBuffer.Length;
            }

            // Change Password
            //
            offset = buffer.WriteUInt16LE((ushort)dataOffset, offset);

            if (string.IsNullOrEmpty(ChangePassword))
            {
                offset = buffer.WriteInt16LE(0, offset);
            }
            else
            {
                ByteBuffer changePasswordBuffer = new ByteBuffer(ScramblePassword(Encoding.Unicode.GetBytes(ChangePassword)));
                buffers.Add(changePasswordBuffer);

                offset = buffer.WriteUInt16LE((ushort)ChangePassword.Length, offset);
                dataOffset += changePasswordBuffer.Length;
            }

            // SSPI Long
            //
            if (SSPI?.Length > ushort.MaxValue)
            {
                buffer.WriteUInt32LE((uint)SSPI.Length, offset);
            }
            else
            {
                buffer.WriteUInt32LE(0, offset);
            }

            ByteBuffer data = new ByteBuffer(buffers);
            data.WriteInt32LE(data.Length);
            Buffer = data;

        }

        private ByteBuffer GenerateRandomPhysicalAddress()
        {
            byte[] addressBytes = new byte[ClientIdSize];
            Random random = new Random();
            random.NextBytes(addressBytes);
            return new ByteBuffer(addressBytes);
        }

        /// <summary>
        /// Scrambles a password according to TDS documentation, section 2.2.6.4 LOGIN7.
        /// </summary>
        /// <param name="byteArray">The UCS2 (Unicode) string byte array.</param>
        /// <returns>Scrambled byte array.</returns>
        private byte[] ScramblePassword(byte[] byteArray)
        {
            for (int i = 0; i < byteArray.Length; i++)
            {
                byte b = byteArray[i];

                b = (byte)((b >> 4) | (b << 4));
                b ^= 0xA5;

                byteArray[i] = b;
            }
            return byteArray;
        }

        private ByteBuffer GetExtensionsBuffer()
        {
            List<ByteBuffer> buffers = new List<ByteBuffer>();

            if (FedAuth is object)
            {
                buffers.Add(FedAuth.GetBuffer());
            }

            buffers.Add(new ByteBuffer(new byte[] { FeatureExtensionTerminator }));

            return new ByteBuffer(buffers);
        }

        /// <summary>
        /// Gets a human readable string representation of this object.
        /// </summary>
        /// <returns>Human readable string representation.</returns>
        public override string ToString()
        {
            return $"Login7Payload[Options={Options}, " +
                $"Flags1={OptionFlags1}, " +
                $"Flags2={OptionFlags2}, " +
                $"TypeFlags={TypeFlags}, " +
                $"Flags1={OptionFlags1}, " +
                $"Hostname={Hostname}, " +
                $"Username={Username}, " +
                $"Password={Password}, " +
                $"AppName={AppName}, " +
                $"ServerName={ServerName}, " +
                $"LibraryName={LibraryName}, " +
                $"Language={Language}, " +
                $"Database={Database}, " +
                $"ClientId={ClientId}, " +
                $"SSPI={SSPI}, " +
                $"AttachDbFile={AttachDbFile}, " +
                $"ChangePassword={ChangePassword}" +
                $"]";
        }

    }
}
