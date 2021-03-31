using System.IO;

namespace SunFileManager.SunFileLib.Util
{
    /// <summary>
    /// Class that extends BinaryWriter with additional methods for writing data to Sun files.
    /// </summary>
    public class SunBinaryWriter : BinaryWriter
    {
        #region Constructors

        public SunBinaryWriter(Stream output) : base(output)
        {
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Writes a null-terminated (ends with 00) string to file.
        /// </summary>
        public void WriteNullTerminatedString(string value)
        {
            for (int i = 0; i < value.Length; i++)
            {
                Write((byte)value[i]);
            }
            Write((byte)0);
        }

        /// <summary>
        /// Writes an 8-bit signed int representation of a 32-bit signed int.
        /// <br>If the byte value is <b>-128 (0x80)</b> the value is the int that follows it.</br>
        /// </summary>
        public void WriteCompressedInt(int value)
        {
            if (value > sbyte.MaxValue || value <= sbyte.MinValue)
            {
                Write(sbyte.MinValue);
                Write(value);
            }
            else
            {
                Write((sbyte)value);
            }
        }

        public void WriteCompressedLong(long value)
        {
            if (value > sbyte.MaxValue || value <= sbyte.MinValue)
            {
                Write(sbyte.MinValue);
                Write(value);
            }
            else
            {
                Write((sbyte)value);
            }
        }

        /// <summary>
        /// Writes the byte type of the SunObject passed to the method,
        /// <br>followed by its name as a length-prefixed string.</br>
        /// </summary>
        public void WriteSunObjectValue(string sobjectName, byte type)
        {
            Write(type);
            Write(sobjectName);
        }

        #endregion Methods
    }
}