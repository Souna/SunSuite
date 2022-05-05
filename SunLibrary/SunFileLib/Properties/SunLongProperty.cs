using SunLibrary.SunFileLib.Structure;
using SunLibrary.SunFileLib.Util;

namespace SunLibrary.SunFileLib.Properties
{
    /// <summary>
    /// A 64-bit Integer property represented as a signed byte.
    /// If the byte value is <b>-128</b> the value is the int (4 bytes) that follows it.
    /// <br>A SunLongProperty may be associated with a SunDirectory or SunCanvasProperty.</br>
    /// </summary>
    public class SunLongProperty : SunProperty
    {
        #region Fields

        private string name;
        private long val;
        private SunObject parent;

        #endregion Fields

        #region Inherited Members

        #region SunProperty

        /// <summary>
        /// Returns the type of the Property.
        /// <br>Long = 4</br>
        /// </summary>
        public override SunPropertyType PropertyType
        { get { return SunPropertyType.Long; } }

        public override void SetValue(object value)
        {
            val = System.Convert.ToInt64(value);
        }

        public override void WriteValue(SunBinaryWriter writer)
        {
            //writer.Write(Name);
            writer.Write((byte)SunPropertyType.Long);
            writer.WriteCompressedLong(Value);
        }

        #endregion SunProperty

        #region SunObject

        public override void Dispose()
        {
            name = null;
            val = 0;
        }

        /// <summary>
        /// Returns the name of this Int property.
        /// </summary>
        public override string Name
        { get { return name; } set { name = value; } }

        /// <summary>
        /// Returns the parent object containing this Int Property.
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
        /// The value of the property.
        /// </summary>
        public long Value
        { get { return val; } set { val = value; } }

        /// <summary>
        /// Creates a blank SunLongProperty object.
        /// </summary>
        public SunLongProperty()
        { }

        /// <summary>
        /// Creates a SunLongProperty with a provided name.
        /// </summary>
        public SunLongProperty(string name)
        { this.name = name; }

        /// <summary>
        /// Creates a SunLongProperty with a provided name and value.
        /// </summary>
        public SunLongProperty(string name, long value)
        {
            this.name = name;
            val = value;
        }

        /// <summary>
        /// Creates a SunLongProperty with a provided name, value, and parent.
        /// Unneeded.
        /// </summary>
        public SunLongProperty(string name, long value, SunObject sunParent)
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