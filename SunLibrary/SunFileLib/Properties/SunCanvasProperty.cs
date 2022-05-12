using System.Collections.Generic;
using System.Drawing;
using SunLibrary.SunFileLib.Structure;
using SunLibrary.SunFileLib.Util;

namespace SunLibrary.SunFileLib.Properties
{
    /// <summary>
    /// A property containing a bitmap image.
    /// <br>A SunCanvasProperty may have its own subproperties just as well.</br>
    /// </summary>
    public class SunCanvasProperty : SunPropertyExtended, IPropertyContainer
    {
        #region Fields

        private string name;
        private List<SunProperty> properties = new List<SunProperty>();
        private List<SunCanvasProperty> frameList = new List<SunCanvasProperty>();
        private SunObject parent;
        private SunPngProperty png;

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
                foreach (SunProperty property in properties)
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
            ParentImage.Changed = true;
        }

        public override void WriteValue(SunBinaryWriter writer)
        {
            writer.Write((byte)SunPropertyType.Canvas);
            if (SunProperties.Count > 0)
            {
                writer.Write(true);
                WritePropertyList(writer, properties);
            }
            else
            {
                writer.Write(false);
            }

            /*if (IsGif)
            {
                writer.Write(0);
            }*/

            //writer.Write(size);

            writer.WriteCompressedInt(PNG.Width);
            writer.WriteCompressedInt(PNG.Height);
            byte[] canvasBytes = PNG.GetCompressedBytes(false);
            writer.Write(canvasBytes.Length);
            writer.Write(canvasBytes);
        }

        #endregion SunProperty

        #region SunObject

        public override void Dispose()
        {
            name = null;
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
            properties = null;
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
        { get { return parent; } internal set { parent = value; } }

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
            properties.Add(property);
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
            properties.Remove(property);
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
        { get { return properties; } }

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

        public SunCanvasProperty(string name)
        {
            Name = name;
        }

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
        /// The actual bitmap or gif.
        /// </summary>
        public SunPngProperty PNG
        {
            get
            {
                return png;
            }
            set
            {
                png = value;
                //Width = value.Width;
                //Height = value.Height;
                //CompressPNG(value);
            }
        }

        public bool IsGif { get; set; }

        /// <summary>
        /// Gets the origin position of the canvas.
        /// A canvas is drawn relative to its origin property.
        /// <br>Defaults to (0, 0) if unavailable.</br>
        /// </summary>
        public Point GetCanvasOriginPosition()
        {
            return new Point(0, 0);
        }

        public Bitmap GetBitmap()
        {
            return PNG.GetPNG(false);
        }

        #endregion Custom Members
    }
}