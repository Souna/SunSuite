/*  MapleLib - A general-purpose MapleStory library
 * Copyright (C) 2009, 2010, 2015 Snow and haha01haha01

 * This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

 * This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

 * You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.*/

//uncomment to enable automatic UOL (Link) resolving, comment to disable it
#define UOLRES

using SunLibrary.SunFileLib.Structure;
using SunLibrary.SunFileLib.Util;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace SunLibrary.SunFileLib.Properties
{
    /// <summary>
    /// A property that contains a String property to link to another property.
    /// </summary>
    public class SunLinkProperty : SunPropertyExtended
    {
        #region Fields

        internal string name, val;
        internal SunObject parent;
        internal SunObject linkValue;

        #endregion Fields

        #region Inherited Members

        #region SunProperty

        public override SunPropertyType PropertyType
        { get { return SunPropertyType.Link; } }

        public override void SetValue(object value)
        {
            val = (string)value;
            ParentImage.Changed = true;
        }

        public override void WriteValue(SunBinaryWriter writer)
        {
            writer.Write((byte)SunPropertyType.Link);
            writer.Write(val);
        }

        public override SunProperty DeepClone()
        {
            SunLinkProperty clone = new SunLinkProperty(Name, val);
            clone.linkValue = null;
            return clone;
        }

        public override SunProperty this[string name]
        {
            get
            {
                return LinkValue is SunProperty
                    ? ((SunProperty)LinkValue)[name]
                    : linkValue is SunImage
                    ? ((SunImage)LinkValue)[name]
                    : null;
            }
        }

        #endregion SunProperty

        #region SunObject

        public override void Dispose()
        {
            name = null;
        }

        #region Cast Values

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
            return val;
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

        public override string ToString()
        {
            return val;
        }

        #endregion Cast Values

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

        public override List<SunProperty> SunProperties
        {
            get
            {
                return LinkValue is SunProperty ? ((SunProperty)LinkValue).SunProperties : null;
            }
        }

        public string Value
        {
            get { return val; }
            set { val = value; ParentImage.Changed = true; }
        }

        public SunObject LinkValue
        {
            get
            {
                if (linkValue == null)
                {
                    string[] paths = val.Split('/');
                    linkValue = parent;
                    foreach (string path in paths)
                    {
                        if (path == "..")
                        {
                            linkValue = Parent;
                        }
                        else
                        {
                            if (linkValue is SunProperty)
                                linkValue = ((SunProperty)linkValue)[path];
                            else if (linkValue is SunImage)
                                linkValue = ((SunImage)linkValue)[path];
                            else if (linkValue is SunDirectory)
                                linkValue = ((SunDirectory)linkValue)[path];
                            else
                            {
                                ErrorLogger.Log(ErrorLevel.Critical, "Link property is corrupted at property: " + FullPath);
                                return null;
                            }
                        }
                    }
                }
                return linkValue;
            }
        }

        /// <summary>
        /// Creats a blank SunLinkProperty.
        /// </summary>
        public SunLinkProperty()
        {
        }

        /// <summary>
        /// Creates a SunLink with a provided name.
        /// </summary>
        public SunLinkProperty(string name)
        {
            this.name = name;
        }

        /// <summary>
        /// Creates a SunLinkProperty with a provided name and value.
        /// </summary>
        public SunLinkProperty(string name, string value)
        {
            this.name = name;
            val = value;
        }

        #endregion Custom Members
    }
}