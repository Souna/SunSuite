using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using SunFileManager.SunFileLib.Util;

namespace SunFileManager.SunFileLib.Properties
{
    /// <summary>
    /// A property containing a bitmap image.
    /// <br>A SunCanvasProperty may have its own subproperties just as well.</br>
    /// </summary>
    public class SunCanvasProperty : SunProperty, IPropertyContainer
    {
        #region Fields

        private string name;
        private List<SunProperty> sunPropertyList = new List<SunProperty>();
        private List<SunCanvasProperty> frameList = new List<SunCanvasProperty>();
        private SunObject parent;
        private byte[] compressedBytes;
        private Bitmap png;
        private long offset;
        private SunBinaryReader sunReader;

        // Different Zlib header values. Little-endian.
        private static int ZLIB_LOW_COMPRESSION = 0x0178;

        private static int ZLIB_LOW_MEDIUM_COMPRESSION = 0x5E78;
        private static int ZLIB_MEDIUM_HIGH_COMPRESSION = 0x9C78;
        private static int ZLIB_BEST_COMPRESSION = 0xDA78;

        #endregion Fields

        #region Inherited Members

        #region SunProperty

        /// <summary>
        /// Grabs a SunProperty belonging to this Image by name.
        /// </summary>
        public override SunProperty this[string name]
        {
            get
            {
                foreach (SunProperty property in sunPropertyList)
                    if (property.Name.ToLower() == name.ToLower())
                        return property;
                return null;
            }
            set
            {
                if (value != null)
                {
                    value.Name = name;
                    AddProperty(value);
                }
            }
        }

        /// <summary>
        /// Returns the type of the Property
        /// <br>Image = 8</br>
        /// </summary>
        public override SunPropertyType PropertyType
        { get { return SunPropertyType.Canvas; } }

        /// <summary>
        /// Sets the value of the image property.
        /// </summary>
        /// <param name="value"></param>
        public override void SetValue(object value)
        {
        }

        public override void WriteValue(SunBinaryWriter writer)
        {
            byte[] canvasBytes = new byte[0];

            writer.Write(Name);
            writer.Write((byte)SunPropertyType.Canvas);

            // Writing size. If gif, size = 0.
            if (IsGif)
            {
                writer.Write(0);
            }
            else
            {
                canvasBytes = GetCompressedBytes();
                int size = sizeof(byte);    // SunPropertyType (8)
                size += sizeof(byte);       // Property bool
                size += sizeof(byte);       // Gif bool
                size += SunFileHelper.GetCompressedIntLength(Width);
                size += SunFileHelper.GetCompressedIntLength(Height);
                size += 4;                  // Image Length
                size += canvasBytes.Length;
                writer.Write(size);
            }

            if (SunProperties.Count > 0)
            {
                writer.Write(true);  //bool ("yes - there are properties associated")
                writer.WriteCompressedInt(SunProperties.Count);
                writer.Write((byte)SunObjectType.Property);
                foreach (SunProperty prop in SunProperties)
                {
                    //writer.Write(prop.Name);
                    prop.WriteValue(writer);
                }
            }
            else
            {
                writer.Write(false);
            }

            if (IsGif)
            {
                writer.Write(true);
                writer.WriteCompressedInt(Frames.Count);
                writer.Write((byte)SunObjectType.Property);
                foreach (SunCanvasProperty frame in Frames)
                {
                    frame.WriteValue(writer);
                }
            }
            else
            {
                writer.Write(false);
            }

            if (IsGif)
            {
                return;
            }
            writer.WriteCompressedInt(Width);
            writer.WriteCompressedInt(Height);

            writer.Write(canvasBytes.Length);     // Write total size of image data

            writer.Write(canvasBytes);

            //byte[] imageBytes = new byte[0];

            //writer.Write(Name);

            //if (!IsGif)
            //{
            //    imageBytes = GetCompressedBytes();
            //}

            //writer.Write((byte)SunPropertyType.Image);

            //int size = sizeof(byte);    // SunPropertyType (8)
            //size += sizeof(byte);       // Property bool
            //size += sizeof(byte);       // Gif bool
            //size += SunFileHelper.GetCompressedIntLength(Width);
            //size += SunFileHelper.GetCompressedIntLength(Height);
            //size += 4;                  // Image Length
            //size += imageBytes.Length;
            //writer.Write(size);

            //if (SunProperties.Count > 0)
            //{
            //    writer.Write((byte)1);  //bool ("yes - there are properties associated")
            //    writer.WriteCompressedInt(SunProperties.Count);
            //    writer.Write((byte)SunObjectType.Property);
            //    foreach (SunProperty prop in SunProperties)
            //    {
            //        writer.Write(prop.Name);
            //        prop.WriteValue(writer);
            //    }
            //}
            //else
            //{
            //    writer.Write((byte)0);
            //}

            //if (IsGif)
            //    writer.Write(true);
            //else
            //    writer.Write(false);

            //writer.WriteCompressedInt(Width);
            //writer.WriteCompressedInt(Height);

            //writer.Write(imageBytes.Length);     // Write total size of image data

            //writer.Write(imageBytes);
        }

        #endregion SunProperty

        #region SunObject

        public override void Dispose()
        {
            name = null;
            compressedBytes = null;
            if (png != null)
            {
                png.Dispose();
                png = null;
            }
            foreach (SunProperty property in SunProperties)
                property.Dispose();
            SunProperties.Clear();
            if (Frames.Count > 0)
                foreach (SunCanvasProperty frame in Frames)
                    frame.Dispose();
            sunPropertyList = null;
            frameList = null;
        }

        /// <summary>
        /// Returns the name of this SunCanvasProperty.
        /// </summary>
        public override string Name
        { get { return name; } set { name = value; } }

        /// <summary>
        /// Returns the parent object containing this canvas.
        /// </summary>
        public override SunObject Parent
        { get { return parent; } set { parent = value; } }

        /// <summary>
        /// Returns the byte-value type of a canvas (3).
        /// Deprecated.
        /// </summary>
        public override SunObjectType ObjectType
        { get { return SunObjectType.Property; } }

        /// <summary>
        /// Returns the SunFile this canvas is a part of.
        /// </summary>
        public override SunFile SunFileParent
        { get { return Parent.SunFileParent; } }

        #endregion SunObject

        #region IPropertyContainer

        /// <summary>
        /// Adds a property to the canvas.
        /// </summary>
        public void AddProperty(SunProperty property)
        {
            property.Parent = this;
            sunPropertyList.Add(property);
        }

        /// <summary>
        /// Add a list of properties at once
        /// </summary>
        public void AddProperties(List<SunProperty> props)
        {
            foreach (SunProperty prop in props)
            {
                AddProperty(prop);
            }
        }

        /// <summary>
        /// Deletes the selected property under the canvas.
        /// </summary>
        public void RemoveProperty(SunProperty property)
        {
            property.Parent = null;
            sunPropertyList.Remove(property);
        }

        /// <summary>
        /// Clear the list of properties
        /// </summary>
        public void ClearProperties()
        {
            foreach (SunProperty prop in SunProperties)
                prop.Parent = null;
            SunProperties.Clear();
        }

        /// <summary>
        /// The list of properties belonging to this canvas.
        /// </summary>
        public List<SunProperty> SunProperties
        { get { return sunPropertyList; } }

        #endregion IPropertyContainer

        #endregion Inherited Members

        #region Custom Members

        public void AddFrame(SunCanvasProperty frame)
        {
            frame.Parent = this;
            Frames.Add(frame);
        }

        public void RemoveFrame(SunCanvasProperty frame)
        {
            frame.Parent = null;
            Frames.Remove(frame);
        }

        /// <summary>
        /// Creates a blank canvas property.
        /// </summary>
        public SunCanvasProperty()
        { }

        /// <summary>
        /// Creates a SunCanvasProperty with a specified name and parent.
        /// </summary>
        public SunCanvasProperty(string name, SunObject sunParent, bool gif = false)
        {
            Name = name;
            Parent = sunParent;
            IsGif = gif;
        }

        /// <summary>
        /// List of gif frames.
        /// </summary>
        public List<SunCanvasProperty> Frames
        { get { return frameList; } }

        /// <summary>
        /// The bitmap width.
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// The bitmap height.
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// The actual bitmap or gif.
        /// </summary>
        public Bitmap PNG
        {
            get
            {
                return png;
            }
            set
            {
                png = value;
                Width = value.Width;
                Height = value.Height;
                CompressPNG(value);
            }
        }

        public void SetPNG(Bitmap png)
        {
            this.png = png;
            CompressPNG(png);
        }

        public bool IsGif { get; set; }

        public void CompressPNG(Bitmap bmp)
        {
            // experiment?
            byte[] buffer = new byte[bmp.Width * bmp.Height * 8];   // 8 for 8 bits per channel?

            int position = 0;
            for (int i = 0; i < Height; i++)
                for (int j = 0; j < Width; j++)
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

        public byte[] CompressBuffer(byte[] buffer)
        {
            MemoryStream memoryStream = new MemoryStream();
            DeflateStream deflateStream = new DeflateStream(memoryStream, CompressionMode.Compress, true);
            deflateStream.Write(buffer, 0, buffer.Length);
            deflateStream.Close();

            memoryStream.Position = 0;
            byte[] newBuffer = new byte[memoryStream.Length + 2];
            memoryStream.Read(newBuffer, 2, newBuffer.Length - 2);
            memoryStream.Close();
            memoryStream.Dispose();

            deflateStream.Dispose();

            // Writes 0x78(120) and 0x9C(156) to the start of newBuffer
            System.Buffer.BlockCopy(new byte[] { 0x78, 0x9C }, 0, newBuffer, 0, 2);

            return newBuffer;
        }

        public byte[] GetCompressedBytes()
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

                byte[] returnBytes = compressedBytes;
                compressedBytes = null;
                return returnBytes;
            }
            return compressedBytes;
        }

        /// <summary>
        /// Gets the origin position of the canvas.
        /// <br>Defaults to 0, 0 if unavailable.</br>
        /// </summary>
        public Point GetCanvasOriginPosition()
        {
            return new Point(0, 0);
        }

        #endregion Custom Members
    }
}