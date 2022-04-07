using SunFileManager.SunFileLib.Structure;
using SunFileManager.SunFileLib.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunFileManager.SunFileLib.Properties
{
    /// <summary>
    /// A property that contains a set of properties
    /// </summary>
    class SunSubProperty : SunPropertyExtended, IPropertyContainer
    {
        #region Fields
        internal List<SunProperty> properties = new List<SunProperty>();
        internal string name;
        internal SunObject parent;
        #endregion

        #region Inherited Members

        #region SunProperty
        public override SunPropertyType PropertyType { get { return SunPropertyType.SubProperty; } }

        public override void SetValue(object value)
        {
            throw new NotImplementedException();
        }

        public override void WriteValue(SunBinaryWriter writer)
        {
            SunProperty.WritePropertyList(writer, properties);
        }
        #endregion SunProperty

        #region IPropertyContainer
        public void AddProperty(SunProperty prop)
        {
            prop.Parent = this;
            properties.Add(prop);
        }

        public void AddProperties(List<SunProperty> props)
        {
            foreach (SunProperty prop in props)
                AddProperty(prop);
        }

        public void RemoveProperty(SunProperty prop)
        {
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
                return properties;
            }
        }

        public override SunProperty this[string name]
        {
            get
            {
                foreach (SunProperty p in properties)
                    if (p.Name.ToLower() == name.ToLower())
                        return p;
                throw new KeyNotFoundException("A SunProperty with the specified name was not found: " + name);
                //return null;
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

        #region SunObject
        public override void Dispose()
        {
            name = null;
        }

        /// <summary>
        /// Returns the name of this property.
        /// </summary>
        public override string Name { get { return name; } set { name = value; } }

        /// <summary>
        /// Returns the parent object containing this Property.
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

        #endregion

        #region Custom Members
        public SunSubProperty() { }

        public SunSubProperty(string name)
        {
            this.name = name;
        }
        #endregion
    }
}
