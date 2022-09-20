// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;

namespace Microsoft.Data.TdsLib.Payloads.Login7
{

    /// <summary>
    /// Change password.
    /// </summary>
    public enum OptionChangePassword
    {
        /// <summary>
        /// Do not change password.
        /// </summary>
        No,

        /// <summary>
        /// Change password.
        /// </summary>
        Yes
    }

    /// <summary>
    /// Option flags 3.
    /// </summary>
    public sealed class OptionFlags3
    {
        private const int OptionChangePasswordBitIndex = 0x01;
        private const int OptionBinaryXmlBitIndex = 0x02;
        private const int OptionSpawnUserInstanceBitIndex = 0x04;
        private const int OptionUnkownCollationHandlingBitIndex = 0x08;
        private const int OptionExtensionUsedBitIndex = 0x10;

        /// <summary>
        /// The option raw value.
        /// </summary>
        public byte Value { get; private set; }

        /// <summary>
        /// Change password flag.
        /// </summary>
        public OptionChangePassword ChangePassword
        {
            get
            {
                if ((Value & OptionChangePasswordBitIndex) == OptionChangePasswordBitIndex)
                {
                    return OptionChangePassword.Yes;
                }

                return OptionChangePassword.No;
            }

            set
            {
                if (value == OptionChangePassword.No)
                {
                    Value &= byte.MaxValue - OptionChangePasswordBitIndex;
                }
                else
                {
                    Value |= OptionChangePasswordBitIndex;
                }
            }
        }

        /// <summary>
        /// Binary XML flag.
        /// </summary>
        public bool BinaryXml
        {
            get => (Value & OptionBinaryXmlBitIndex) == OptionBinaryXmlBitIndex;

            set
            {
                if (value)
                {
                    Value |= OptionBinaryXmlBitIndex;
                }
                else
                {
                    Value &= byte.MaxValue - OptionBinaryXmlBitIndex;
                }
            }
        }

        /// <summary>
        /// Spawn user interface flag.
        /// </summary>
        public bool SpawnUserInstance
        {
            get => (Value & OptionSpawnUserInstanceBitIndex) == OptionSpawnUserInstanceBitIndex;

            set
            {
                if (value)
                {
                    Value |= OptionSpawnUserInstanceBitIndex;
                }
                else
                {
                    Value &= byte.MaxValue - OptionSpawnUserInstanceBitIndex;
                }
            }
        }

        /// <summary>
        /// Unknown collation handling flag.
        /// </summary>
        public bool UnknownCollationHandling
        {
            get => (Value & OptionUnkownCollationHandlingBitIndex) == OptionUnkownCollationHandlingBitIndex;

            set
            {
                if (value)
                {
                    Value |= OptionUnkownCollationHandlingBitIndex;
                }
                else
                {
                    Value &= byte.MaxValue - OptionUnkownCollationHandlingBitIndex;
                }
            }
        }

        /// <summary>
        /// Extension used flag.
        /// </summary>
        public bool ExtensionUsed
        {
            get => (Value & OptionExtensionUsedBitIndex) == OptionExtensionUsedBitIndex;

            set
            {
                if (value)
                {
                    Value |= OptionExtensionUsedBitIndex;
                }
                else
                {
                    Value &= byte.MaxValue - OptionExtensionUsedBitIndex;
                }
            }
        }

        /// <summary>
        /// Create a new instance of this class from a raw value.
        /// </summary>
        /// <param name="value">Raw value.</param>
        public OptionFlags3(byte value)
        {
            Value = value;
        }

        /// <summary>
        /// Create a new instance of this class with a default value.
        /// </summary>
        public OptionFlags3()
        {
            ChangePassword = OptionChangePassword.No;
            BinaryXml = false;
            SpawnUserInstance = false;
            UnknownCollationHandling = true;
            ExtensionUsed = true;
        }

        /// <summary>
        /// Convert the object explicitly into a byte.
        /// </summary>
        /// <param name="optionFlags3">The option instance.</param>
        public static explicit operator byte(OptionFlags3 optionFlags3) => optionFlags3.Value;

        /// <summary>
        /// Creates a new instance of this class from a byte value. 
        /// </summary>
        /// <param name="value">The raw value.</param>
        public static explicit operator OptionFlags3(byte value) => new OptionFlags3(value);

        /// <summary>
        /// Gets a human readable string representation of this object.
        /// </summary>
        /// <returns>Human readable string representation.</returns>
        public override string ToString()
        {
            return $"OptionFlags3[0b{Convert.ToString(Value, 2).PadLeft(8, '0')}(ChangePassword={ChangePassword}, BinaryXml={BinaryXml}, SpawnUserInstance={SpawnUserInstance}, UnknownCollationHandling={UnknownCollationHandling}, ExtensionUsed={ExtensionUsed})]";
        }

    }
}
