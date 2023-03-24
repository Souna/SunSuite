using SunLibrary.SunFileLib.Structure;
using SunLibrary.SunFileLib.Util;
using System.Drawing;

namespace SunLibrary.SunFileLib.Properties
{
    /// <summary>
    /// A double-precision floating-point property. (8 bytes)
    /// </summary>
    public class SunDoubleProperty : SunProperty
    {
        #region Fields

        private string name;
        private double val;
        private SunObject parent;

        #endregion Fields

        #region Inherited Members

        #region SunProperty

        /// <summary>
        /// Returns the type of the Property.
        /// <br>Double = 6</br>
        /// </summary>
        public override SunPropertyType PropertyType
        { get { return SunPropertyType.Double; } }

        public override void SetValue(object value)
        {
            val = (double)value;
            ParentImage.Changed = true;
        }

        public override void WriteValue(SunBinaryWriter writer)
        {
            //writer.Write(Name);
            writer.Write((byte)SunPropertyType.Double);
            if (Value == 0f)
            {
                writer.Write((byte)0);
            }
            else
            {
                writer.Write(Value);
            }
        }

        public override SunProperty DeepClone()
        {
            SunDoubleProperty clone = new SunDoubleProperty(Name, Value);
            return clone;
        }

        #endregion SunProperty

        #region SunObject

        public override void Dispose()
        {
            name = null;
            val = 0.0d;
        }

        /// <summary>
        /// Returns the name of this Double property.
        /// </summary>
        public override string Name
        { get { return name; } set { name = value; } }

        /// <summary>
        /// Returns the parent object containing this Double Property.
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

        public override int GetInt()
        {
            return (int)val;
        }

        public override short GetShort()
        {
            return (short)val;
        }

        public override long GetLong()
        {
            return (long)val;
        }

        public override float GetFloat()
        {
            return (float)val;
        }

        public override double GetDouble()
        {
            return val;
        }

        public override string GetString()
        {
            return val.ToString();
        }

        public override Point GetPoint()
        {
            throw new System.NotImplementedException();
        }

        public override Bitmap GetBitmap()
        {
            throw new System.NotImplementedException();
        }

        public override byte[] GetBytes()
        {
            throw new System.NotImplementedException();
        }

        #endregion SunObject

        #endregion Inherited Members

        #region Custom Members

        /// <summary>
        /// The value of the Double property.
        /// </summary>
        public double Value
        { get { return val; } set { val = value; ParentImage.Changed = true; } }

        /// <summary>
        /// Creates a blank SunDoubleProperty object.
        /// </summary>
        public SunDoubleProperty()
        { }

        /// <summary>
        /// Creates a SunDoubleProperty with a provided name.
        /// </summary>
        public SunDoubleProperty(string name)
        { this.name = name; }

        /// <summary>
        /// Creates a SunDoubleProperty with a provided name and value.
        /// </summary>
        public SunDoubleProperty(string name, double value)
        {
            this.name = name;
            val = value;
        }

        /// <summary>
        /// Creates a SunDoubleProperty with a provided name, value, and parent.
        /// Unneeded.
        /// </summary>
        public SunDoubleProperty(string name, double value, SunObject sunParent)
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