using SunLibrary.SunFileLib.Structure;
using SunLibrary.SunFileLib.Util;
using NAudio.Wave;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Runtime.Serialization;

namespace SunLibrary.SunFileLib.Properties
{
    /// <summary>
    /// A SunProperty that contains data for an MP3 file.
    /// </summary>
    public class SunSoundProperty : SunPropertyExtended
    {
        #region Fields

        internal string name;
        internal byte[] mp3bytes = null;
        internal SunObject parent;
        internal int len_ms;
        internal byte[] header;
        internal SunBinaryReader sunReader;
        internal bool headerEncrypted = false;
        internal long offset;
        internal int soundDataLen;

        public static readonly byte[] soundHeader = new byte[] {
            0x02,
            0x83, 0xEB, 0x36, 0xE4, 0x4F, 0x52, 0xCE, 0x11, 0x9F, 0x53, 0x00, 0x20, 0xAF, 0x0B, 0xA7, 0x70,
            0x8B, 0xEB, 0x36, 0xE4, 0x4F, 0x52, 0xCE, 0x11, 0x9F, 0x53, 0x00, 0x20, 0xAF, 0x0B, 0xA7, 0x70,
            0x00,
            0x01,
            0x81, 0x9F, 0x58, 0x05, 0x56, 0xC3, 0xCE, 0x11, 0xBF, 0x01, 0x00, 0xAA, 0x00, 0x55, 0x59, 0x5A };

        internal WaveFormat wavFormat;

        #endregion Fields

        #region Inherited Members

        #region SunProperty

        /// <summary>
        /// Returns the type of the Property.
        /// <br>Sound = 10</br>
        /// </summary>
        public override SunPropertyType PropertyType
        {
            get { return SunPropertyType.Sound; }
        }

        public override void SetValue(object value)
        {
            return;
        }

        public override void WriteValue(SunBinaryWriter writer)
        {
            byte[] data = GetBytes(false);
            writer.Write((byte)SunPropertyType.Sound);
            writer.WriteCompressedInt(data.Length);
            writer.WriteCompressedInt(len_ms);
            writer.Write(header);
            writer.Write(data);
        }

        public override SunProperty DeepClone()
        {
            SunSoundProperty clone = new SunSoundProperty(this);
            return clone;
        }
        #endregion SunProperty

        #region SunObject

        public override void Dispose()
        {
            name = null;
            mp3bytes = null;
        }

        /// <summary>
        /// Returns the name of this Sound property.
        /// </summary>
        public override string Name
        { get { return name; } set { name = value; } }

        /// <summary>
        /// Returns the parent object containing this Vector Property.
        /// </summary>
        public override SunObject Parent
        { get { return parent; } internal set { parent = value; } }

        /// <summary>
        /// Returns the byte-value type of a property (4).
        /// </summary>
        public override SunObjectType ObjectType
        { get { return SunObjectType.Property; } }

        /// <summary>
        /// Returns the SunFile this property is a member of.
        /// </summary>
        public override SunFile SunFileParent
        { get { return Parent.SunFileParent; } }

        #endregion SunObject

        #endregion Inherited Members

        #region Custom Members

        /// <summary>
        /// The data of the MP3 header
        /// </summary>
        public byte[] Header
        { get { return header; } set { header = value; } }

        /// <summary>
        /// Length of the mp3 file in milliseconds
        /// </summary>
        public int Length
        { get { return len_ms; } set { len_ms = value; } }

        /// <summary>
        /// Frequency of the MP3 file in Hz
        /// </summary>
        public int Frequency
        {
            get { return wavFormat != null ? wavFormat.SampleRate : 0; }
        }

        /// <summary>
        /// Creates a SunSoundProperty with a provided name.
        /// </summary>
        public SunSoundProperty(string name, SunBinaryReader reader, bool parseNow)
        {
            this.name = name;
            sunReader = reader;
            //reader.BaseStream.Position++;

            //note - soundDataLen does NOT include the length of the header.
            soundDataLen = reader.ReadCompressedInt();
            len_ms = reader.ReadCompressedInt();

            long headerOff = reader.BaseStream.Position;
            reader.BaseStream.Position += soundHeader.Length; //skip GUIDs
            int wavFormatLen = reader.ReadByte();
            reader.BaseStream.Position = headerOff;

            header = reader.ReadBytes(soundHeader.Length + 1 + wavFormatLen);
            ParseSunSoundPropertyHeader();

            //sound file offset
            offset = reader.BaseStream.Position;
            if (parseNow)
                mp3bytes = reader.ReadBytes(soundDataLen);
            else
                reader.BaseStream.Position += soundDataLen;
        }

        /// <summary>
        /// Creates a SunSoundProperty with the specified name and data from another SunSoundProperty object
        /// </summary>
        public SunSoundProperty(SunSoundProperty otherProperty)
        {
            this.name = otherProperty.name;
            this.wavFormat = otherProperty.wavFormat;
            this.len_ms = otherProperty.len_ms;
            this.soundDataLen = otherProperty.soundDataLen;
            this.offset = otherProperty.offset;

            if (otherProperty.header == null) // not initialized yet
            {
                otherProperty.ParseSunSoundPropertyHeader();
            }
            this.header = new byte[otherProperty.header.Length];
            Array.Copy(otherProperty.header, this.header, otherProperty.header.Length);

            if (otherProperty.mp3bytes == null)
                this.mp3bytes = otherProperty.GetBytes(false);
            else
            {
                this.mp3bytes = new byte[otherProperty.mp3bytes.Length];
                Array.Copy(otherProperty.mp3bytes, mp3bytes, otherProperty.mp3bytes.Length);
            }
            this.headerEncrypted = otherProperty.headerEncrypted;
        }

        /// <summary>
        /// Creates a SunSoundProperty with the specified name and data
        /// </summary>
        public SunSoundProperty(string name, int len_ms, byte[] headerClone, byte[] data)
        {
            this.name = name;
            this.len_ms = len_ms;

            this.header = new byte[headerClone.Length];
            Array.Copy(headerClone, this.header, headerClone.Length);

            this.mp3bytes = new byte[data.Length];
            Array.Copy(data, mp3bytes, data.Length);

            ParseSunSoundPropertyHeader();
        }

        /// <summary>
        /// Creates a SunSoundProperty with the specified name from a file
        /// </summary>
        public SunSoundProperty(string name, string file)
        {
            this.name = name;
            MediaFoundationReader mfreader = new MediaFoundationReader(file);
            this.wavFormat = mfreader.WaveFormat;
            this.len_ms = (int)((double)mfreader.Length * 1000d / (double)mfreader.WaveFormat.AverageBytesPerSecond);
            RebuildHeader();
            mfreader.Dispose();
            this.mp3bytes = File.ReadAllBytes(file);
        }

        public static bool memcmp(byte[] a, byte[] b, int n)
        {
            for (int i = 0; i < n; i++)
            {
                if (a[i] != b[i])
                {
                    return false;
                }
            }
            return true;
        }

        public static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 3);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2} ", b);
            return hex.ToString();
        }

        public void RebuildHeader()
        {
            using (BinaryWriter bw = new BinaryWriter(new MemoryStream()))
            {
                bw.Write(soundHeader);
                byte[] wavHeader = StructToBytes(wavFormat);
                //if (headerEncrypted)
                //{
                //    for (int i = 0; i < wavHeader.Length; i++)
                //    {
                //        wavHeader[i] ^= this.sunReader.SunKey[i];
                //    }
                //}
                bw.Write((byte)wavHeader.Length);
                bw.Write(wavHeader, 0, wavHeader.Length);
                header = ((MemoryStream)bw.BaseStream).ToArray();
            }
        }

        private static byte[] StructToBytes<T>(T obj)
        {
            byte[] result = new byte[Marshal.SizeOf(obj)];
            GCHandle handle = GCHandle.Alloc(result, GCHandleType.Pinned);
            try
            {
                Marshal.StructureToPtr(obj, handle.AddrOfPinnedObject(), false);
                return result;
            }
            finally
            {
                handle.Free();
            }
        }

        private static T BytesToStruct<T>(byte[] data) where T : new()
        {
            GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);
            try
            {
                return Marshal.PtrToStructure<T>(handle.AddrOfPinnedObject());
            }
            finally
            {
                handle.Free();
            }
        }

        private static T BytesToStructConstructorless<T>(byte[] data)
        {
            GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);
            try
            {
                T obj = (T)FormatterServices.GetUninitializedObject(typeof(T));
                Marshal.PtrToStructure<T>(handle.AddrOfPinnedObject(), obj);
                return obj;
            }
            finally
            {
                handle.Free();
            }
        }

        private void ParseSunSoundPropertyHeader()
        {
            byte[] wavHeader = new byte[header.Length - soundHeader.Length - 1];
            Buffer.BlockCopy(header, soundHeader.Length + 1, wavHeader, 0, wavHeader.Length);

            if (wavHeader.Length < Marshal.SizeOf<WaveFormat>())
                return;

            WaveFormat wavFmt = BytesToStruct<WaveFormat>(wavHeader);

            if (Marshal.SizeOf<WaveFormat>() + wavFmt.ExtraSize != wavHeader.Length)
            {
                //try decrypt
                //for (int i = 0; i < wavHeader.Length; i++)
                //{
                //    wavHeader[i] ^= this.SunReader.SunKey[i];
                //}
                wavFmt = BytesToStruct<WaveFormat>(wavHeader);

                if (Marshal.SizeOf<WaveFormat>() + wavFmt.ExtraSize != wavHeader.Length)
                {
                    ErrorLogger.Log(ErrorLevel.Critical, "parse sound header failed");
                    return;
                }
                headerEncrypted = true;
            }

            // parse to mp3 header
            if (wavFmt.Encoding == WaveFormatEncoding.MpegLayer3 && wavHeader.Length >= Marshal.SizeOf<Mp3WaveFormat>())
            {
                this.wavFormat = BytesToStructConstructorless<Mp3WaveFormat>(wavHeader);
            }
            else if (wavFmt.Encoding == WaveFormatEncoding.Pcm)
            {
                this.wavFormat = wavFmt;
            }
            else
            {
                ErrorLogger.Log(ErrorLevel.MissingFeature, string.Format("Unknown wave encoding {0}", wavFmt.Encoding.ToString()));
            }
        }

        public byte[] GetBytes(bool saveInMemory)
        {
            if (mp3bytes != null)
                return mp3bytes;
            else
            {
                if (sunReader == null) return null;
                long currentPos = sunReader.BaseStream.Position;
                sunReader.BaseStream.Position = offset;
                mp3bytes = sunReader.ReadBytes(soundDataLen);
                sunReader.BaseStream.Position = currentPos;
                if (saveInMemory)
                    return mp3bytes;
                else
                {
                    byte[] result = mp3bytes;
                    mp3bytes = null;
                    return result;
                }
            }
        }

        public void SaveToFile(string file)
        {
            File.WriteAllBytes(file, GetBytes(false));
        }

        #endregion Custom Members
    }
}