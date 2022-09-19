// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Microsoft.TdsLib.Payloads.PreLogin
{
    /// <summary>
    /// SQL app version.
    /// </summary>
    public struct SqlVersion
    {
        /// <summary>
        /// Major.
        /// </summary>
        public byte Major { get; set; }

        /// <summary>
        /// Minor.
        /// </summary>
        public byte Minor { get; set; }

        /// <summary>
        /// Patch.
        /// </summary>
        public byte Patch { get; set; }

        /// <summary>
        /// Trivial.
        /// </summary>
        public byte Trivial { get; set; }
        
        /// <summary>
        /// Sub build.
        /// </summary>
        public ushort SubBuild { get; set; }

        /// <summary>
        /// Compares an object with this object.
        /// </summary>
        /// <param name="obj">The object to compare.</param>
        /// <returns>True if the objects are equal, false otherwise.</returns>
        public override bool Equals(object obj)
        {
            return obj is SqlVersion version &&
                   Major == version.Major &&
                   Minor == version.Minor &&
                   Patch == version.Patch &&
                   Trivial == version.Trivial &&
                   SubBuild == version.SubBuild;
        }

        /// <summary>
        /// Gets the hash code for this object.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            var hashCode = 316998042;
            hashCode = hashCode * -1521134295 + Major.GetHashCode();
            hashCode = hashCode * -1521134295 + Minor.GetHashCode();
            hashCode = hashCode * -1521134295 + Patch.GetHashCode();
            hashCode = hashCode * -1521134295 + Trivial.GetHashCode();
            hashCode = hashCode * -1521134295 + SubBuild.GetHashCode();
            return hashCode;
        }

        /// <summary>
        /// Compares two versions.
        /// </summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand.</param>
        /// <returns>True if the operands are equal, False otherwise.</returns>
        public static bool operator ==(SqlVersion left, SqlVersion right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Compares two versions.
        /// </summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand.</param>
        /// <returns>True if the operands are different, False otherwise.</returns>
        public static bool operator !=(SqlVersion left, SqlVersion right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Gets a human readable string representation of this object.
        /// </summary>
        /// <returns>Human readable string representation.</returns>
        public override string ToString()
        {
            return $"SqlVersion[{Major}.{Minor}.{Patch}.{Trivial} {SubBuild}]";
        }

    }
}
