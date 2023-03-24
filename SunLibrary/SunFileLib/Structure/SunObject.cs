using SunLibrary.SunFileLib.Properties;
using System;
using System.Drawing;

namespace SunLibrary.SunFileLib.Structure
{
    /// <summary>
    /// An abstract class for SunObjects.
    /// </summary>
    public abstract class SunObject : IDisposable
    {
        private object _tag = null;
        private object _tag2 = null;

        public abstract void Dispose();

        /// <summary>
        /// The name of the object.
        /// </summary>
        public abstract string Name { get; set; }

        /// <summary>
        /// Returns the parent object.
        /// </summary>
        public abstract SunObject Parent { get; internal set; }

        /// <summary>
        /// Returns the parent SunFile.
        /// </summary>
        public abstract SunFile SunFileParent { get; }

        /// <summary>
        /// Returns the ObjectType of this SunObject.
        /// </summary>
        public abstract SunObjectType ObjectType { get; }

        /// <summary>
        /// Deletes object from its parent.
        /// </summary>
        public abstract void Remove();

        /// <summary>
        /// Casting functions
        /// </summary>
        public abstract int GetInt();

        public abstract short GetShort();

        public abstract long GetLong();

        public abstract float GetFloat();

        public abstract double GetDouble();

        public abstract string GetString();

        public abstract Point GetPoint();

        public abstract Bitmap GetBitmap();

        public abstract byte[] GetBytes();

        /// <summary>
        /// Returns the SunObject by name.
        /// </summary>
        public SunObject this[string name]
        {
            get
            {
                if (this is SunFile file)
                {
                    return file[name];
                }
                else if (this is SunDirectory directory)
                {
                    return directory[name];
                }
                else if (this is SunImage image)
                {
                    return image[name];
                }
                else if (this is SunProperty property)
                {
                    return property[name];
                }
                else throw new NotImplementedException();
            }
        }

        /// <summary>
        /// The full filepath of SunObjects within SunFiles.
        /// </summary>
        public string FullPath
        {
            get
            {
                if (this is SunFile file) return file.SunDirectory.Name;
                string result = this.Name;
                SunObject currentObject = this;
                while (currentObject.Parent != null)
                {
                    currentObject = currentObject.Parent;
                    result = currentObject.Name + @"\" + result;
                }
                return result;
            }
        }

        /// <summary>
        /// Used in Editor to save already parsed images
        /// </summary>
        public virtual object SETag
        {
            get { return _tag; }
            set { _tag = value; }
        }

        /// <summary>
        /// Used in Map Simulator to save already parsed textures
        /// </summary>
        public virtual object MapSimTag
        {
            get { return _tag2; }
            set { _tag2 = value; }
        }
    }
}