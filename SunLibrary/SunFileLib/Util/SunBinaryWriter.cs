using System.Collections;
using System.IO;
using SunLibrary.SunFileLib.Structure;

namespace SunLibrary.SunFileLib.Util
{
    /// <summary>
    /// Class that extends BinaryWriter with additional methods for writing data to Sun files.
    /// </summary>
    public class SunBinaryWriter : BinaryWriter
    {
        #region Properties

        public SunHeader Header { get; set; }

        public Hashtable StringCache { get; set; }

        #endregion Properties

        #region Constructors

        public SunBinaryWriter(Stream output) : base(output)
        {
            StringCache = new Hashtable();
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
        public void WriteSunObjectValue(string sObjectName, byte type)
        {
            //	All encryption related shit idek lol
            //string storeName = type + "_" + s;
            //if (s.Length > 4 && StringCache.ContainsKey(storeName))
            //{
            //    Write((byte)2);
            //    Write((int)StringCache[storeName]);
            //}
            //else
            //{
            //    int sOffset = (int)(this.BaseStream.Position - Header.FStart);
            //    Write(type);
            //    base.Write(s);  //remove base later. We need this to learn 2 encrypt
            //    if (!StringCache.ContainsKey(storeName))
            //    {
            //        StringCache[storeName] = sOffset;
            //    }
            //}
            Write(type);
            base.Write(sObjectName);
        }

        public void WriteOffset(uint value)
        {
            //Uncomment all of this when it's time to encrypt things
            //uint encOffset = (uint)BaseStream.Position;
            //encOffset = (encOffset - Header.FStart) ^ 0xFFFFFFFF;
            //encOffset *= Hash;
            //encOffset -= CryptoConstants.WZ_OffsetConstant;
            //encOffset = RotateLeft(encOffset, (byte)(encOffset & 0x1F));
            //uint writeOffset = encOffset ^ (value - (Header.FStart * 2));
            //Write(writeOffset);
            Write(value);
        }

        #endregion Methods
    }
}