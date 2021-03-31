using SunFileManager.SunFileLib.Properties;
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
        /// The amount of top-level (not nested) entries within this SunObject.
        /// consider removing cuz int properties dont need this shit
        /// </summary>
        public virtual int TopLevelEntryCount { get { return 0; } set { } }

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