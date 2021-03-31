using System.IO;
using System.Text;

namespace SunFileManager.SunFileLib.Util
{
    public class SunBinaryReader : BinaryReader
    {
        #region Properties

        public SunHeader Header { get; set; }

        #endregion Properties

        #region Constructors

        public SunBinaryReader(Stream input) : base(input)
        {
        }

        #endregion Constructors

        #region Methods

        public string ReadStringAtOffset(long Offset)
        {
            return ReadStringAtOffset(Offset, false);
        }

        public string ReadStringAtOffset(long Offset, bool readByte)
        {
            long currentOffset = BaseStream.Position;
            BaseStream.Position = Offset;
            if (readByte)
            {
                ReadByte();
            }
            string returnString = ReadString();
            BaseStream.Position = currentOffset;
            return returnString;
        }

        public override string ReadString()
        {
            int size = ReadByte();
            return Encoding.ASCII.GetString(ReadBytes(size));
        }

        /// <summary>
        /// Reads an ASCII string, without decryption
        /// </summary>
        /// <param name="filePath">Length of bytes to read</param>
        public string ReadString(int length)
        {
            return Encoding.ASCII.GetString(ReadBytes(length));
        }

        public string ReadNullTerminatedString()
        {
            StringBuilder retString = new StringBuilder();
            byte b = ReadByte();
            while (b != 0)
            {
                retString.Append((char)b);
                b = ReadByte();
            }
            return retString.ToString();
        }

        public int ReadCompressedInt()
        {
            sbyte sb = base.ReadSByte();
            if (sb == sbyte.MinValue)
            {
                return ReadInt32();
            }
            return sb;
        }

        public long ReadLong()
        {
            sbyte sb = base.ReadSByte();
            if (sb == sbyte.MinValue)
            {
                return ReadInt64();
            }
            return sb;
        }

        #endregion Methods
    }
}