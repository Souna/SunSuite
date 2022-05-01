using SunFileManager.SunFileLib.Util;

namespace SunFileManager.SunFileLib.Properties
{
    /// <summary>
    /// A SunProperty with a string as its value.
    /// </summary>
    public class SunStringProperty : SunProperty
    {
        #region Fields

        private string name, val;
        private SunObject parent;

        #endregion Fields

        #region Inherited Members

        #region SunProperty

        /// <summary>
        /// Returns the type of the property.
        /// <br>String = 7</br>
        /// </summary>
        public override SunPropertyType PropertyType { get { return SunPropertyType.String; } }

        public override void SetValue(object value)
        {
            val = (string)value;
        }

        public override void WriteValue(SunBinaryWriter writer)
        {
            //writer.Write(Name);
            writer.Write((byte)SunPropertyType.String);
            writer.Write(Value);
        }

        #endregion SunProperty

        #region SunObject

        public override void Dispose()
        {
            name = null;
            val = null;
        }

        /// <summary>
        /// Returns the name of this String property.
        /// </summary>
        public override string Name { get { return name; } set { name = value; } }

        /// <summary>
        /// Returns the parent object containing this String Property.
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
        /// The value of the String property.
        /// </summary>
        public string Value { get { return val; } set { val = value; } }

        /// <summary>
        /// Creates a blank SunStringProperty object.
        /// </summary>
        public SunStringProperty() { }

        /// <summary>
        /// Creates a SunStringProperty with a given name.
        /// </summary>
        public SunStringProperty(string name) { this.name = name; }

        /// <summary>
        /// Creates a SunStringProperty with a given name and string value.
        /// </summary>
        public SunStringProperty(string name, string value)
        {
            this.name = name;
            val = value;
        }

        #endregion Custom Members
    }
}