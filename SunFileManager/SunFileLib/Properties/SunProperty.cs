using SunFileManager.SunFileLib.Util;

namespace SunFileManager.SunFileLib.Properties
{
    /// <summary>
    /// Base class for defining the properties associated with different Nodes.
    /// </summary>
    public abstract class SunProperty : SunObject
    {
        #region Members

        public virtual new SunProperty this[string name] { get { return null; } set { } }

        public abstract SunPropertyType PropertyType { get; }

        /// <summary>
        /// The SunDirectory this Property is contained within.
        /// </summary>
        public SunDirectory ParentDirectory
        {
            get
            {
                SunObject parent = Parent;
                while (parent != null)
                {
                    if (parent is SunDirectory) return (SunDirectory)parent;
                    else parent = parent.Parent;
                }
                return null;
            }
        }

        //                            Object so we can use different data types later.
        public abstract void SetValue(object value);

        public abstract void WriteValue(SunBinaryWriter writer);

        #endregion Members

        #region Inherited Members

        /// <summary>
        /// Returns the byte-value type of the SunProperty.
        /// </summary>
        public override SunObjectType ObjectType { get { return SunObjectType.Property; } }

        /// <summary>
        /// Returns the SunFile this property is a part of.
        /// </summary>
        public override SunFile SunFileParent { get { return ParentDirectory.SunFileParent; } }

        /// <summary>
        /// Removes the property from its parent in the file structure.
        /// </summary>
        public override void Remove()
        {
            ((IPropertyContainer)Parent).RemoveProperty(this);
        }

        #endregion Inherited Members
    }
}