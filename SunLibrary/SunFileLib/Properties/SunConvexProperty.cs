using SunLibrary.SunFileLib.Structure;
using SunLibrary.SunFileLib.Util;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace SunLibrary.SunFileLib.Properties
{
    /// <summary>
    /// A property that contains extended properties
    /// </summary>
    public class SunConvexProperty : SunPropertyExtended, IPropertyContainer
    {
        #region Fields

        internal List<SunProperty> properties = new List<SunProperty>();
        internal string name;
        internal SunObject parent;

        #endregion Fields

        #region Inherited Members

        #region SunProperty

        public override SunPropertyType PropertyType
        { get { return SunPropertyType.Convex; } }

        public override void SetValue(object value)
        {
            throw new NotImplementedException();
        }

        public void WriteValue(SunBinaryWriter writer, bool isSavingImg = false)
        {
            if (isSavingImg)
                WritePropertyList(writer, properties);
            else
            {
                writer.Write((byte)SunPropertyType.SubProperty);
                WritePropertyList(writer, properties);
            }
        }

        public override void WriteValue(SunBinaryWriter writer)
        {
            writer.Write((byte)SunPropertyType.Convex);
            WritePropertyList(writer, properties);
        }

        public override SunProperty DeepClone()
        {
            SunConvexProperty clone = new SunConvexProperty(Name);
            foreach (SunProperty prop in properties)
                clone.AddProperty(prop.DeepClone());
            return clone;
        }

        public override SunProperty this[string name]
        {
            get
            {
                foreach (SunProperty prop in properties)
                    if (prop.Name.ToLower() == name.ToLower())
                        return prop;
                return null;
            }
        }

        #endregion SunProperty

        #region IPropertyContainer

        public void AddProperty(SunProperty prop)
        {
            if (!(prop is SunPropertyExtended))
                throw new Exception("Property is not extended");
            prop.Parent = this;
            properties.Add((SunPropertyExtended)prop);
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
            foreach (SunProperty prop in properties)
                prop.Parent = null;
            properties.Clear();
        }

        public override List<SunProperty> SunProperties
        {
            get { return properties; }
        }

        #endregion IPropertyContainer

        #region SunObject

        public override void Dispose()
        {
            name = null;
        }

        public override int GetInt()
        {
            throw new NotImplementedException();
        }

        public override short GetShort()
        {
            throw new NotImplementedException();
        }

        public override long GetLong()
        {
            throw new NotImplementedException();
        }

        public override float GetFloat()
        {
            throw new NotImplementedException();
        }

        public override double GetDouble()
        {
            throw new NotImplementedException();
        }

        public override string GetString()
        {
            return Name;
        }

        public override Point GetPoint()
        {
            throw new NotImplementedException();
        }

        public override Bitmap GetBitmap()
        {
            throw new NotImplementedException();
        }

        public override byte[] GetBytes()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns the name of this property.
        /// </summary>
        public override string Name
        { get { return name; } set { name = value; } }

        /// <summary>
        /// Returns the parent object containing this Property.
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

        public SunConvexProperty()
        {
        }

        public SunConvexProperty(string name)
        {
            this.name = name;
        }

        #endregion Custom Members
    }
}