using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SunFileManager.SunFileLib.Properties;
using SunFileManager.SunFileLib.Util;

namespace SunFileManager.SunFileLib.Structure
{
    public class SunImage : SunObject, IPropertyContainer
    {
        public const int SunImageHeaderByte = 0x69;

        #region Fields

        internal bool parsed = false;
        internal string name;
        internal int size, checksum;
        internal uint offset = 0;
        internal SunBinaryReader reader;
        internal List<SunProperty> properties = new List<SunProperty>();
        internal SunObject parent;
        internal int blockStart = 0;
        internal long tempFileStart = 0;
        internal long tempFileEnd = 0;
        internal bool changed = false;
        internal bool parseEverything = false;

        /// <summary>
        /// SunImage embedding .lua file
        /// </summary>
        public bool IsLuaSunImage
        {
            get { return Name.EndsWith(".lua"); }
        }

        #endregion Fields

        #region Inherited Members

        #region SunObject

        public override void Dispose()
        {
            name = null;
            reader = null;
            if (properties != null)
            {
                foreach (SunProperty prop in properties)
                {
                    prop.Dispose();
                }
                properties.Clear();
                properties = null;
            }
        }

        /// <summary>
        /// The name of the SunImage.
        /// </summary>
        public override string Name
        { get { return name; } set { name = value; } }

        /// <summary>
        /// Returns the parent object of this .img.
        /// </summary>
        public override SunObject Parent
        { get { return parent; } internal set { parent = value; } }

        /// <summary>
        /// Returns the SunFile this .img belongs to.
        /// </summary>
        public override SunFile SunFileParent
        { get { return parent.SunFileParent; } }

        /// <summary>
        /// Returns the byte-value type of the SunImage.
        /// </summary>
        public override SunObjectType ObjectType
        {
            get
            {
                if (reader != null && !parsed) ParseImage();
                return SunObjectType.Image;
            }
        }

        /// <summary>
        /// Removes a directory from its parent directory.
        /// <br>All child nodes are deleted as well.</br>
        /// </summary>
        public override void Remove()
        {
            if (Parent != null)
            {
                ((SunDirectory)Parent).RemoveImage(this);
            }
        }

        #endregion SunObject

        #region IPropertyContainer

        public void AddProperty(SunProperty prop)
        {
            prop.Parent = this;
            if (reader != null && !parsed) ParseImage();
            properties.Add(prop);
        }

        public void AddProperties(List<SunProperty> props)
        {
            foreach (SunProperty prop in props)
            {
                AddProperty(prop);
            }
        }

        public void RemoveProperty(SunProperty prop)
        {
            if (reader != null && !parsed) ParseImage();
            prop.Parent = null;
            properties.Remove(prop);
        }

        public void ClearProperties()
        {
            foreach (SunProperty prop in properties) prop.Parent = null;
            properties.Clear();
        }

        public List<SunProperty> SunProperties
        {
            get
            {
                if (reader != null && !parsed) ParseImage();
                return properties;
            }
        }

        public new SunProperty this[string name]
        {
            get
            {
                if (reader != null && !parsed) ParseImage();
                foreach (SunProperty prop in properties)
                    if (prop.Name.ToLower() == name.ToLower())
                        return prop;
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

        #endregion IPropertyContainer

        #endregion Inherited Members

        #region Custom Members

        public SunImage()
        {
        }

        public SunImage(string name)
        {
            this.name = name;
        }

        public SunImage(string name, SunBinaryReader reader)
        {
            this.name = name;
            this.reader = reader;
            this.blockStart = (int)reader.BaseStream.Position;
        }

        public bool Parsed
        { get { return parsed; } set { parsed = value; } }

        public bool Changed
        { get { return changed; } set { changed = value; } }

        public int Size
        { get { return size; } set { size = value; } }

        public int Checksum
        { get { return checksum; } set { checksum = value; } }

        public uint Offset
        { get { return offset; } set { offset = value; } }

        #endregion Custom Members

        #region Parsing Methods

        public bool ParseImage(bool parseEverything = false)
        {
            if (Parsed)
            {
                return true;
            }
            else if (Changed)
            {
                Parsed = true;
                return true;
            }

            lock (reader) // for multi threaded XMLWZ export.
            {
                this.parseEverything = parseEverything;
                reader.BaseStream.Position = offset + 1;

                List<SunProperty> props = SunProperty.ParsePropertyList(offset, reader, this, this);
                properties.AddRange(props);

                parsed = true;
            }
            return true;
        }

        public void UnparseImage()
        {
            parsed = false;
            this.properties = new List<SunProperty>();
        }

        /// <summary>
        /// Writes the SunImage to the underlying SunBinaryWriter
        /// </summary>
        public void SaveImage(SunBinaryWriter writer, bool forceReadFromData = false)
        {
            if (changed || forceReadFromData)
            {
                if (reader != null && !parsed)
                    ParseImage();
                SunSubProperty imgProp = new SunSubProperty();
                long startPosition = writer.BaseStream.Position;
                imgProp.AddProperties(SunProperties);
                imgProp.WriteValue(writer, true);
                Size = (int)(writer.BaseStream.Position - startPosition);
            }
            else
            {
                long position = reader.BaseStream.Position;
                reader.BaseStream.Position = Offset;
                writer.Write(reader.ReadBytes(size));
                reader.BaseStream.Position = position;
            }
        }

        #endregion Parsing Methods
    }
}