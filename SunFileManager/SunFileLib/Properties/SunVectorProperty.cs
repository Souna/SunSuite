using System.Drawing;
using SunFileManager.SunFileLib.Structure;
using SunFileManager.SunFileLib.Util;

namespace SunFileManager.SunFileLib.Properties
{
    /// <summary>
    /// A SunProperty that contains an x and a y value.
    /// <br>A SunVectorProperty may be associated with a SunDirectory or SunCanvasProperty.</br>
    /// </summary>
    public class SunVectorProperty : SunPropertyExtended
    {
        #region Fields

        private string name;
        private SunIntProperty x, y;
        private SunObject parent;

        #endregion Fields

        #region Inherited Members

        #region SunProperty

        /// <summary>
        /// Returns the type of the Property.
        /// <br>Vector = 9</br>
        /// </summary>
        public override SunPropertyType PropertyType
        { get { return SunPropertyType.Vector; } }

        public override void SetValue(object value)
        {
            if (value is Point p)
            {
                x.Value = p.X;
                y.Value = p.Y;
            }
            else
            {
                x.Value = ((Size)value).Width;
                x.Value = ((Size)value).Height;
            }
        }

        public override void WriteValue(SunBinaryWriter writer)
        {
            writer.Write((byte)SunPropertyType.Vector);
            writer.WriteCompressedInt(X.Value);
            writer.WriteCompressedInt(Y.Value);
        }

        #endregion SunProperty

        #region SunObject

        public override void Dispose()
        {
            name = null;
            x = null;
            y = null;
        }

        /// <summary>
        /// Returns the name of this Vector property.
        /// </summary>
        public override string Name
        { get { return name; } set { name = value; } }

        /// <summary>
        /// Returns the parent object containing this Vector Property.
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
        /// Returns a new Point with both X and Y values.
        /// </summary>
        public Point Value
        { get { return new Point(x.Value, y.Value); } set { x.Value = value.X; y.Value = value.Y; } }

        /// <summary>
        /// The X-value of the vector point.
        /// </summary>
        public SunIntProperty X
        { get { return x; } set { x = value; } }

        /// <summary>
        /// The Y-value of the vector point.
        /// </summary>
        public SunIntProperty Y
        { get { return y; } set { y = value; } }

        /// <summary>
        /// Creates a blank SunVectorProperty object.
        /// </summary>
        public SunVectorProperty()
        { }

        /// <summary>
        /// Creates a SunVectorProperty with a provided name.
        /// </summary>
        public SunVectorProperty(string name)
        { this.name = name; }

        /// <summary>
        /// Creates a SunVectorProperty with a provided name and values.
        /// </summary>
        public SunVectorProperty(string name, SunIntProperty x, SunIntProperty y)
        {
            this.name = name;
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// Creates a SunVectorProperty with a provided name and Point value.
        /// </summary>
        public SunVectorProperty(string name, Point value)
        {
            this.name = name;
            X = new SunIntProperty("X", value.X);
            Y = new SunIntProperty("Y", value.Y);
        }

        /// <summary>
        /// Creates a SunVectorProperty with a provided name, value, and parent.
        /// Unneeded.
        /// </summary>
        public SunVectorProperty(string name, SunIntProperty x, SunIntProperty y, SunObject sunParent)
        {
            this.name = name;
            this.x = x;
            this.y = y;
            Parent = sunParent;
        }

        public Point GetPoint()
        {
            return new Point(x.Value, y.Value);
        }

        public override string ToString()
        {
            return "X: " + x.Value.ToString() + ", Y: " + y.Value.ToString();
        }

        #endregion Custom Members
    }
}