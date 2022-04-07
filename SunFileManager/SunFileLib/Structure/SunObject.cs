using SunFileManager.SunFileLib.Properties;
using SunFileManager.SunFileLib.Structure;
using System;

namespace SunFileManager.SunFileLib
{
    /// <summary>
    /// An abstract class for SunObjects.
    /// </summary>
    public abstract class SunObject : IDisposable
    {
        public abstract void Dispose();

        /// <summary>
        /// The name of the object.
        /// </summary>
        public abstract string Name { get; set; }

        /// <summary>
        /// Returns the parent object.
        /// </summary>
        public abstract SunObject Parent { get; set; }

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
                if (this is SunFile) return ((SunFile)this).SunDirectory.Name;
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
    }
}