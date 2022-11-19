using SunLibrary.SunFileLib.Structure;
using SunLibrary.SunFileLib.Util;

namespace SunLibrary.SunFileLib.Properties
{
    /// <summary>
    /// A property whose value is null.
    /// </summary>
    public class SunNullProperty : SunProperty
    {
        #region Fields

        private string name;
        private SunObject parent;

        #endregion Fields

        #region Inherited Members

        #region SunProperty

        /// <summary>
        /// Returns the type of the Property.
        /// <br>Null = 0</br>
        /// </summary>
        public override SunPropertyType PropertyType
        { get { return SunPropertyType.Null; } }

        public override void SetValue(object value)
        {
            throw new System.NotImplementedException();
        }

        public override void WriteValue(SunBinaryWriter writer)
        {
            //writer.Write(Name);
            writer.Write((byte)SunPropertyType.Null);
        }

        public override SunProperty DeepClone()
        {
            SunNullProperty clone = new SunNullProperty(Name);
            return clone;
        }
        #endregion SunProperty

        #region SunObject

        public override void Dispose()
        {
            name = null;
        }

        /// <summary>
        /// Returns the name of this Null property.
        /// </summary>
        public override string Name
        { get { return name; } set { name = value; } }

        /// <summary>
        /// Returns the parent object containing this Null Property.
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
        /// Creates a blank SunNullProperty object.
        /// </summary>
        public SunNullProperty()
        { }

        /// <summary>
        /// Creates a SunNullProperty with a provided name.
        /// </summary>
        public SunNullProperty(string name)
        { this.name = name; }

        #endregion Custom Members
    }
}