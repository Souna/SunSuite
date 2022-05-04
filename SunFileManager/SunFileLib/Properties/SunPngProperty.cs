﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using SunFileManager.SunFileLib.Structure;
using SunFileManager.SunFileLib.Util;

namespace SunFileManager.SunFileLib.Properties
{
    public class SunPngProperty : SunProperty
    {
        #region Fields

        internal int width, height;
        internal byte[] compressedBytes;
        internal Bitmap pngProp;
        internal SunObject parent;
        internal SunBinaryReader sunReader;
        internal long offset;

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

        #endregion Inherited Members

        #region Custom Members

        public SunPngProperty()
        { }

        internal SunPngProperty(SunBinaryReader reader, bool parseNow)
        {
            width = reader.ReadCompressedInt();
            height = reader.ReadCompressedInt();
            offset = reader.BaseStream.Position;    //Offset is being set to position of Length
            int length = reader.ReadInt32();    // Starts at 78 9C

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
            CompressPNG(pngProp);    //use param or local?
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
                int length = sunReader.ReadInt32() - 1;
                sunReader.BaseStream.Position += 1;

                if (length > 0)
                    compressedBytes = sunReader.ReadBytes(length);

                sunReader.BaseStream.Position = position;

                if (!saveInMemory)
                {
                    //were removing the reference to compressedBytes, so a backup for the ret value is needed
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
                int x = 0, y = 0, b = 0, g = 0;
                Bitmap bmp = null;
                BitmapData bmpData;
                SunImage parentImg = ParentImage;
                byte[] decompressedBuffer;

                BinaryReader reader = new BinaryReader(new MemoryStream(compressedBytes));
                MemoryStream dataStream = new MemoryStream();
                int blockSize = 0;
                int endofPng = compressedBytes.Length;

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
            MemoryStream memoryStream = new MemoryStream();
            DeflateStream zlib = new DeflateStream(memoryStream, CompressionMode.Compress, true);
            zlib.Write(decompressedBuffer, 0, decompressedBuffer.Length);
            zlib.Close();

            memoryStream.Position = 0;
            byte[] newBuffer = new byte[memoryStream.Length];   //+2
            memoryStream.Read(newBuffer, 0, newBuffer.Length);  //offset = 2, newBuffer.length -2
            memoryStream.Close();
            memoryStream.Dispose();

            zlib.Dispose();

            // Writes List header to start of image data.
            //Buffer.BlockCopy(new byte[] { 0x78, 0x9C }, 0, newBuffer, 0, 2);

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