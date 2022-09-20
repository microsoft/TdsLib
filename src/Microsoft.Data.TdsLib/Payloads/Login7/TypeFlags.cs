// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;

namespace Microsoft.Data.TdsLib.Payloads.Login7
{

    /// <summary>
    /// SQL interface type.
    /// </summary>
    public enum OptionSqlType
    {
        /// <summary>
        /// Default.
        /// </summary>
        Default,

        /// <summary>
        /// T-SQL.
        /// </summary>
        TSQL
    }

    /// <summary>
    /// OLE DB.
    /// </summary>
    public enum OptionOleDb
    {
        /// <summary>
        /// Off.
        /// </summary>
        Off,

        /// <summary>
        /// On.
        /// </summary>
        On
    }

    /// <summary>
    /// Access intent.
    /// </summary>
    public enum OptionAccessIntent
    {
        /// <summary>
        /// Read and write.
        /// </summary>
        ReadWrite,

        /// <summary>
        /// Read only.
        /// </summary>
        ReadOnly
    }

    /// <summary>
    /// Type flags.
    /// </summary>
    public sealed class TypeFlags
    {
        private const int OptionSqlTypeBitIndex = 0x08;
        private const int OptionOleDbBitIndex = 0x10;
        private const int OptionAccesIntentBitIndex = 0x20;

        /// <summary>
        /// The option raw value.
        /// </summary>
        public byte Value { get; private set; }

        /// <summary>
        /// SQL Type.
        /// </summary>
        public OptionSqlType SqlType
        {
            get
            {
                if ((Value & OptionSqlTypeBitIndex) == OptionSqlTypeBitIndex)
                {
                    return OptionSqlType.TSQL;
                }

                return OptionSqlType.Default;
            }

            set
            {
                if (value == OptionSqlType.Default)
                {
                    Value &= byte.MaxValue - OptionSqlTypeBitIndex;
                }
                else
                {
                    Value |= OptionSqlTypeBitIndex;
                }
            }
        }

        /// <summary>
        /// OLE DB.
        /// </summary>
        public OptionOleDb OleDb
        {
            get
            {
                if ((Value & OptionOleDbBitIndex) == OptionOleDbBitIndex)
                {
                    return OptionOleDb.On;
                }

                return OptionOleDb.Off;
            }

            set
            {
                if (value == OptionOleDb.Off)
                {
                    Value &= byte.MaxValue - OptionOleDbBitIndex;
                }
                else
                {
                    Value |= OptionOleDbBitIndex;
                }
            }
        }

        /// <summary>
        /// Access intent.
        /// </summary>
        public OptionAccessIntent AccessIntent
        {
            get
            {
                if ((Value & OptionAccesIntentBitIndex) == OptionAccesIntentBitIndex)
                {
                    return OptionAccessIntent.ReadOnly;
                }

                return OptionAccessIntent.ReadWrite;
            }

            set
            {
                if (value == OptionAccessIntent.ReadWrite)
                {
                    Value &= byte.MaxValue - OptionAccesIntentBitIndex;
                }
                else
                {
                    Value |= OptionAccesIntentBitIndex;
                }
            }
        }

        /// <summary>
        /// Create a new instance of this class from a raw value.
        /// </summary>
        /// <param name="value">Raw value.</param>
        public TypeFlags(byte value)
        {
            Value = value;
        }

        /// <summary>
        /// Create a new instance of this class with a default value.
        /// </summary>
        public TypeFlags()
        {
            SqlType = OptionSqlType.Default;
            OleDb = OptionOleDb.Off;
            AccessIntent = OptionAccessIntent.ReadWrite;
        }

        /// <summary>
        /// Convert the object explicitly into a byte.
        /// </summary>
        /// <param name="typeFlags">The option instance.</param>
        public static explicit operator byte(TypeFlags typeFlags) => typeFlags.Value;

        /// <summary>
        /// Creates a new instance of this class from a byte value. 
        /// </summary>
        /// <param name="value">The raw value.</param>
        public static explicit operator TypeFlags(byte value) => new TypeFlags(value);

        /// <summary>
        /// Gets a human readable string representation of this object.
        /// </summary>
        /// <returns>Human readable string representation.</returns>
        public override string ToString()
        {
            return $"TypeFlags[0b{Convert.ToString(Value, 2).PadLeft(8, '0')}(SqlType={SqlType}, OleDb={OleDb}, AccessIntent={AccessIntent})]";
        }

    }
}
