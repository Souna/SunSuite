using SunFileManager.SunFileLib.Util;

namespace SunFileManager.SunFileLib.Properties
{
    /// <summary>
    /// A 16-bit Integer property represented as a signed byte.
    /// If the byte value is <b>-128</b> the value is the short (2 bytes) that follows it.
    /// <br>A SunShortProperty may be associated with a SunDirectory or SunCanvasProperty.</br>
    /// </summary>
    public class SunShortProperty : SunProperty
    {
        #region Fields

        private string name;
        private short val;
        private SunObject parent;

        #endregion Fields

        #region Inherited Members

        #region SunProperty

        /// <summary>
        /// Returns the type of the Property.
        /// <br>Short = 2</br>
        /// </summary>
        public override SunPropertyType PropertyType { get { return SunPropertyType.Short; } }

        public override void SetValue(object value)
        {
            val = (short)value;
        }

        public override void WriteValue(SunBinaryWriter writer)
        {
            //writer.Write(Name);
            writer.Write((byte)SunPropertyType.Short);
            writer.Write(Value);
        }

        #endregion SunProperty

        #region SunObject

        public override void Dispose()
        {
            name = null;
            val = 0;
        }

        /// <summary>
        /// Returns the name of this Short property.
        /// </summary>
        public override string Name { get { return name; } set { name = value; } }

        /// <summary>
        /// Returns the parent object containing this Short Property.
        /// </summary>
        public override SunObject Parent { get { return parent; } set { parent = value; } }

        /// <summary>
        /// Returns the byte-value type of a property (4).
        /// </summary>
        public override SunObjectType ObjectType { get { return SunObjectType.Property; } }

        /// <summary>
        /// Returns the SunFile this property is a member of.
        /// </summary>
        public override SunFile SunFileParent { get { return Parent.SunFileParent; } }

        #endregion SunObject

        #endregion Inherited Members

        #region Custom Members

        /// <summary>
        /// The value of the Short property.
        /// </summary>
        public short Value { get { return val; } set { val = (short)value; } }

        /// <summary>
        /// Creates a blank SunIntProperty object.
        /// </summary>
        public SunShortProperty() { }

        /// <summary>
        /// Creates a SunIntProperty with a provided name.
        /// </summary>
        public SunShortProperty(string name) { this.name = name; }

        /// <summary>
        /// Creates a SunIntProperty with a provided name and value.
        /// </summary>
        public SunShortProperty(string name, short value)
        {
            this.name = name;
            val = value;
        }

        /// <summary>
        /// Creates a SunIntProperty with a provided name, value, and parent.
        /// Unneeded.
        /// </summary>
        public SunShortProperty(string name, short value, SunObject sunParent)
        {
            this.name = name;
            val = value;
            Parent = sunParent;
        }

        public override string ToString()
        {
            return val.ToString();
        }

        #endregion Custom Members
    }
}