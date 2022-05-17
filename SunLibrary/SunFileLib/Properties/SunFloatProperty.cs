using SunLibrary.SunFileLib.Structure;
using SunLibrary.SunFileLib.Util;

namespace SunLibrary.SunFileLib.Properties
{
    /// <summary>
    /// A floating-point property. (4 bytes)
    /// </summary>
    public class SunFloatProperty : SunProperty
    {
        #region Fields

        private string name;
        private float val;
        private SunObject parent;

        #endregion Fields

        #region Inherited Members

        #region SunProperty

        /// <summary>
        /// Returns the type of the Property.
        /// <br>Float = 5</br>
        /// </summary>
        public override SunPropertyType PropertyType
        { get { return SunPropertyType.Float; } }

        public override void SetValue(object value)
        {
            val = (float)value;
            ParentImage.Changed = true;
        }

        public override void WriteValue(SunBinaryWriter writer)
        {
            //writer.Write(Name);
            writer.Write((byte)SunPropertyType.Float);
            if (Value == 0f)
            {
                writer.Write((float)0);
            }
            else
            {
                writer.Write(Value);
            }
        }

        #endregion SunProperty

        #region SunObject

        public override void Dispose()
        {
            name = null;
            val = 0.0f;
        }

        /// <summary>
        /// Returns the name of this Float property.
        /// </summary>
        public override string Name
        { get { return name; } set { name = value; } }

        /// <summary>
        /// Returns the parent object containing this Float Property.
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
        /// The value of the Float property.
        /// </summary>
        public float Value
        { get { return val; } set { val = value; ParentImage.Changed = true; } }

        /// <summary>
        /// Creates a blank SunFloatProperty object.
        /// </summary>
        public SunFloatProperty()
        { }

        /// <summary>
        /// Creates a SunFloatProperty with a provided name.
        /// </summary>
        public SunFloatProperty(string name)
        { this.name = name; }

        /// <summary>
        /// Creates a SunFloatProperty with a provided name and value.
        /// </summary>
        public SunFloatProperty(string name, float value)
        {
            this.name = name;
            val = value;
        }

        /// <summary>
        /// Creates a SunFloatProperty with a provided name, value, and parent.
        /// Unneeded.
        /// </summary>
        public SunFloatProperty(string name, float value, SunObject sunParent)
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