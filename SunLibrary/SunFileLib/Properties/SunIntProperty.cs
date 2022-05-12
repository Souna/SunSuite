using SunLibrary.SunFileLib.Structure;
using SunLibrary.SunFileLib.Util;

namespace SunLibrary.SunFileLib.Properties
{
    /// <summary>
    /// A 32-bit Integer property represented as a signed byte.
    /// If the byte value is <b>-128</b> the value is the int (4 bytes) that follows it.
    /// <br>A SunIntProperty may be associated with a SunDirectory or SunCanvasProperty.</br>
    /// </summary>
    public class SunIntProperty : SunProperty
    {
        #region Fields

        private string name;
        private int val;
        private SunObject parent;

        #endregion Fields

        #region Inherited Members

        #region SunProperty

        /// <summary>
        /// Returns the type of the Property.
        /// <br>Int = 3</br>
        /// </summary>
        public override SunPropertyType PropertyType
        { get { return SunPropertyType.Int; } }

        public override void SetValue(object value)
        {
            val = System.Convert.ToInt32(value);
            ParentImage.Changed = true;
        }

        public override void WriteValue(SunBinaryWriter writer)
        {
            //writer.Write(Name);
            writer.Write((byte)SunPropertyType.Int);
            writer.WriteCompressedInt(Value);
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
        /// The value of the Int property.
        /// </summary>
        public int Value
        { get { return val; } set { val = value; ParentImage.Changed = true; } }

        /// <summary>
        /// Creates a blank SunIntProperty object.
        /// </summary>
        public SunIntProperty()
        { }

        /// <summary>
        /// Creates a SunIntProperty with a provided name.
        /// </summary>
        public SunIntProperty(string name)
        { this.name = name; }

        /// <summary>
        /// Creates a SunIntProperty with a provided name and value.
        /// </summary>
        public SunIntProperty(string name, int value)
        {
            this.name = name;
            val = value;
        }

        /// <summary>
        /// Creates a SunIntProperty with a provided name, value, and parent.
        /// Unneeded.
        /// </summary>
        public SunIntProperty(string name, int value, SunObject sunParent)
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