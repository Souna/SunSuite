using SunLibrary.SunFileLib.Structure;
using SunLibrary.SunFileLib.Util;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Compression;
using System.Runtime.InteropServices;

namespace SunLibrary.SunFileLib.Properties
{
    /// <summary>
    /// The PNG property inside of the SunCanvasProperty.
    /// </summary>
    public class SunPngProperty : SunProperty
    {
        #region Fields

        internal int width, height;
        internal byte[] compressedBytes;
        internal Bitmap pngProp;
        internal SunObject parent;
        internal SunBinaryReader sunReader;
        internal long offset;

        // Different Zlib header values. Little-endian.
        private static int ZLIB_LOW_COMPRESSION = 0x0178;

        private static int ZLIB_LOW_MEDIUM_COMPRESSION = 0x5E78;
        private static int ZLIB_MEDIUM_HIGH_COMPRESSION = 0x9C78;
        private static int ZLIB_BEST_COMPRESSION = 0xDA78;

        #endregion Fields

        #region Inherited Members

        public override SunPropertyType PropertyType
        { get { return SunPropertyType.Png; } }

        public override string Name
        { get { return "PNG"; } set { } }

        public override SunObject Parent
        { get { return parent; } internal set { parent = value; } }

        public override void Dispose()
        {
            compressedBytes = null;
            if (pngProp != null)
            {
                pngProp.Dispose();
                pngProp = null;
            }
        }

        public override void SetValue(object value)
        {
            if (value is Bitmap bitmap)
            {
                SetPNG(bitmap);
            }
            else compressedBytes = (byte[])value;
        }

        public override void WriteValue(SunBinaryWriter writer)
        {
            throw new NotImplementedException("Cannot write PNG Property");
        }

        public override SunProperty DeepClone()
        {
            SunPngProperty clone = new SunPngProperty();
            clone.SetPNG(GetPNG(false));
            return clone;
        }

        #region SunObject Cast Values

        public override int GetInt()
        {
            throw new NotImplementedException();
        }

        public override short GetShort()
        {
            throw new NotImplementedException();
        }

        public override long GetLong()
        {
            throw new NotImplementedException();
        }

        public override float GetFloat()
        {
            throw new NotImplementedException();
        }

        public override double GetDouble()
        {
            throw new NotImplementedException();
        }

        public override string GetString()
        {
            return Name;
        }

        public override Point GetPoint()
        {
            throw new System.NotImplementedException();
        }

        public override Bitmap GetBitmap()
        {
            return GetPNG(false);
        }

        public override byte[] GetBytes()
        {
            throw new System.NotImplementedException();
        }

        #endregion SunObject Cast Values

        #endregion Inherited Members

        #region Custom Members

        public SunPngProperty()
        { }

        internal SunPngProperty(SunBinaryReader reader, bool parseNow)
        {
            width = reader.ReadCompressedInt();
            height = reader.ReadCompressedInt();
            offset = reader.BaseStream.Position;    //Offset is being set to position of Length
            int length = reader.ReadInt32();

            sunReader = reader;

            if (length > 0)
            {
                if (parseNow)
                {
                    compressedBytes = sunReader.ReadBytes(length);
                    ParsePNG();
                }
                else reader.BaseStream.Position += length;
            }
        }

        /// <summary>
        /// The width of the bitmap
        /// </summary>
        public int Width
        { get { return width; } set { width = value; } }

        /// <summary>
        /// The height of the bitmap
        /// </summary>
        public int Height
        { get { return height; } set { height = value; } }

        public void SetPNG(Bitmap png)
        {
            this.pngProp = png;
            CompressPNG(pngProp);
        }

        public Bitmap GetPNG(bool saveInMemory)
        {
            if (pngProp == null)
            {
                long pos = sunReader.BaseStream.Position;
                sunReader.BaseStream.Position = offset;
                int len = sunReader.ReadInt32();
                if (len > 0)
                    compressedBytes = sunReader.ReadBytes(len);
                ParsePNG();
                sunReader.BaseStream.Position = pos;
                if (!saveInMemory)
                {
                    Bitmap pngImage = pngProp;
                    pngProp = null;
                    compressedBytes = null;
                    return pngImage;
                }
            }
            return pngProp;
        }

        public byte[] GetCompressedBytes(bool saveInMemory)
        {
            if (compressedBytes == null)
            {
                long position = sunReader.BaseStream.Position;
                sunReader.BaseStream.Position = offset;
                int length = sunReader.ReadInt32();

                if (length > 0)
                    compressedBytes = sunReader.ReadBytes(length);

                sunReader.BaseStream.Position = position;

                if (!saveInMemory)
                {
                    //we're removing the reference to compressedBytes, so a backup for the ret value is needed
                    byte[] returnBytes = compressedBytes;
                    compressedBytes = null;
                    return returnBytes;
                }
            }
            return compressedBytes;
        }

        internal void ParsePNG()
        {
            try
            {
                int uncompressedSize = 0;
                Bitmap bmp = null;
                BitmapData bmpData;
                byte[] decompressedBuffer;

                BinaryReader reader = new BinaryReader(new MemoryStream(compressedBytes));
                reader.ReadUInt16();    // Read Zlib header. 9c78 most of the time.

                DeflateStream zlib = new DeflateStream(reader.BaseStream, CompressionMode.Decompress);

                bmp = new Bitmap(width, height, PixelFormat.Format32bppArgb);
                bmpData = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
                uncompressedSize = width * height * 4;
                decompressedBuffer = new byte[uncompressedSize];
                zlib.Read(decompressedBuffer, 0, uncompressedSize);
                Marshal.Copy(decompressedBuffer, 0, bmpData.Scan0, decompressedBuffer.Length);
                bmp.UnlockBits(bmpData);
                pngProp = bmp;
            }
            catch (Exception)
            {
                pngProp = null;
            }
        }

        public byte[] CompressBuffer(byte[] decompressedBuffer)
        {
            /*Using DeflateStream instead of GZipStream. GZipStream includes crc info automatically
             GZipStream is the same as DeflateStream but it adds some CRC to ensure the data has no error.*/
            MemoryStream memoryStream = new MemoryStream();
            DeflateStream zlib = new DeflateStream(memoryStream, CompressionMode.Compress, true);
            zlib.Write(decompressedBuffer, 0, decompressedBuffer.Length);
            zlib.Close();

            memoryStream.Position = 0;
            byte[] newBuffer = new byte[memoryStream.Length + 2];   //+2
            memoryStream.Read(newBuffer, 2, newBuffer.Length - 2);  //offset = 2, newBuffer.length -2
            memoryStream.Close();
            memoryStream.Dispose();

            zlib.Dispose();

            // Writes zlib medium compression header to start of buffer
            Buffer.BlockCopy(new byte[] { 0x78, 0x9C }, 0, newBuffer, 0, 2);
            //Buffer.BlockCopy(new byte[] { (byte)ZLIB_MEDIUM_HIGH_COMPRESSION }, 0, newBuffer, 0, 1);

            return newBuffer;
        }

        public void CompressPNG(Bitmap bmp)
        {
            // experiment?
            byte[] buffer = new byte[bmp.Width * bmp.Height * 8];   // 8 for 8 bits per channel?
            Width = bmp.Width;
            Height = bmp.Height;

            int position = 0;
            for (int i = 0; i < bmp.Height; i++)
                for (int j = 0; j < bmp.Width; j++)
                {
                    Color currentPixel = bmp.GetPixel(j, i);
                    buffer[position] = currentPixel.B;
                    buffer[position + 1] = currentPixel.G;
                    buffer[position + 2] = currentPixel.R;
                    buffer[position + 3] = currentPixel.A;
                    position += 4;
                }
            compressedBytes = CompressBuffer(buffer);
        }

        #endregion Custom Members
    }
}