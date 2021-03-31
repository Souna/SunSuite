using SunFileManager.SunFileLib.Properties;
using SunFileManager.SunFileLib.Util;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SunFileManager.SunFileLib
{
    /// <summary>
    /// The "folders" of the SunFile structure.
    /// <para>SunDirectorys contain all of the content of the SunFile. Every SunDirectory is
    /// capable of storing data such as images, properties, as well as additional nested sub-directories.</para>
    /// </summary>
    public class SunDirectory : SunObject, IPropertyContainer, IEnumerable
    {
        #region Fields

        public List<SunDirectory> subDirectoryList = new List<SunDirectory>();
        public List<SunProperty> sunPropertyList = new List<SunProperty>();
        private string name;
        private SunObject parent;
        private SunFile sunFile;
        private int size, offsetSize;
        private uint offset = 0;

        #endregion Fields

        #region Inherited Members

        #region SunObject

        public override void Dispose()
        {
            name = null;
            foreach (SunDirectory dir in SubDirectories)
                dir.Dispose();
            SubDirectories.Clear();
            subDirectoryList = null;
        }

        /// <summary>
        /// The name of the SunDirectory.
        /// </summary>
        public override string Name { get { return name; } set { name = value; } }

        /// <summary>
        /// Returns the parent object of this SunDirectory.
        /// </summary>
        public override SunObject Parent { get { return parent; } set { parent = value; } }

        /// <summary>
        /// Returns the SunFile this directory belongs to.
        /// </summary>
        public override SunFile SunFileParent { get { return sunFile; } }

        /// <summary>
        /// Returns the byte-value type of the SunDirectory.
        /// </summary>
        public override SunObjectType ObjectType { get { return SunObjectType.Directory; } }

        /// <summary>
        /// Removes a directory from its parent directory.
        /// <br>All child nodes are deleted as well.</br>
        /// </summary>
        public override void Remove()
        {
            ((SunDirectory)Parent).RemoveDirectory(this);
        }

        /// <summary>
        /// Returns amount of top-level entries in this SunDirectory.
        /// </summary>
        public override int TopLevelEntryCount { get { return subDirectoryList.Count + sunPropertyList.Count; } }

        /// <summary>
        /// Returns a directory by its given name.
        /// </summary>
        /// <param name="directory">Bool value to indicate we're after a directory and not a property.
        /// Properties get addressed with just name.</param>
        public SunObject this[string name, bool directory]
        {
            // >=( only stupid shitty way to make this possible without making an entirely
            // new class for "SunImage".
            // this[name] refers to properties, this[name, 1] refers to directories themselves.
            get
            {
                foreach (SunDirectory dir in SubDirectories)
                    if (dir.name.ToLower() == name.ToLower())
                        return dir;
                return null;
            }
            set
            {
                if (value != null)
                {
                    value.Name = name;
                    if (value is SunDirectory d)
                        AddDirectory(d);
                    else throw new ArgumentException("Value must be a directory or bool to indicate directory");
                }
            }
        }

        #endregion SunObject

        #region IPropertyContainer

        /// <summary>
        /// Adds a property to the directory.
        /// </summary>
        public void AddProperty(SunProperty property)
        {
            property.Parent = this;
            sunPropertyList.Add(property);
        }

        /// <summary>
        /// Deletes the selected property under this directory.
        /// </summary>
        public void RemoveProperty(SunProperty property)
        {
            property.Parent = null;
            sunPropertyList.Remove(property);
        }

        /// <summary>
        /// Returns the list of properties belonging to this directory.
        /// </summary>
        public List<SunProperty> SunProperties
        {
            get { return sunPropertyList; }
        }

        /// <summary>
        /// Gets a SunProperty by its name.
        /// </summary>
        public new SunProperty this[string name]
        {
            get
            {
                foreach (SunProperty prop in sunPropertyList)
                    if (prop.Name.ToLower() == name.ToLower())
                        return prop;
                return null;
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

        public IEnumerator GetEnumerator()
        {
            return this.SubDirectories.GetEnumerator();
        }

        #endregion Inherited Members

        #region Custom Members

        public SunDirectory()
        {
        }

        /// <summary>
        /// Regular constructor to use when creating new directories.
        /// </summary>
        /// <param name="dirName">Name of SunDirectory.</param>
        /// <param name="dirParent">The SunDirectory this directory will belong to. Master or nested.</param>
        public SunDirectory(string dirName, SunDirectory dirParent)
        {
            name = dirName;
            sunFile = dirParent.SunFileParent;
            parent = dirParent;
        }

        /// <summary>
        /// Constructor called during initial SunFile and master SunDirectory creation, and nowhere else.
        /// </summary>
        /// <param name="dirName">Name of SunDirectory.</param>
        /// <param name="fileParent">The SunFile this directory is part of.</param>
        public SunDirectory(string dirName, SunFile fileParent)
        {
            name = dirName;
            sunFile = fileParent;
            parent = fileParent;
        }

        /// <summary>
        /// Returns size, in bytes, of the directory in the SunFile.
        /// </summary>
        public int Size { get { return size; } set { size = value; } }

        /// <summary>
        /// Returns offset to the directory.
        /// </summary>
        public uint Offset { get { return offset; } set { offset = value; } }

        /// <summary>
        /// Returns the list of subdirectories inside this directory.
        /// </summary>
        public List<SunDirectory> SubDirectories
        {
            get { return subDirectoryList; }
        }

        /// <summary>
        /// Removes a directory from the list.
        /// </summary>
        public void RemoveDirectory(SunDirectory dir)
        {
            subDirectoryList.Remove(dir);
            dir.Parent = null;
        }

        /// <summary>
        /// Adds a SunDirectory to the list of subdirectories.
        /// </summary>
        public void AddDirectory(SunDirectory dir)
        {
            subDirectoryList.Add(dir);
        }

        /// <summary>
        /// Saves the SunDirectory to disk within the <b>SunFile</b>.
        /// <br>Called by the <b>SaveToDisk()</b> method in SunFile.</br>
        /// </summary>
        public void SaveToDisk(SunBinaryWriter sbw)
        {
            Offset = (uint)sbw.BaseStream.Position;

            TopLevelEntryCount = SubDirectories.Count + SunProperties.Count;
            if (TopLevelEntryCount == 0)
            {
                Size = 0;
                return;
            }
            sbw.WriteCompressedInt(TopLevelEntryCount);
            SaveDirectoryContents(sbw);
        }

        public void SaveDirectoryContents(SunBinaryWriter writer)
        {
            foreach (SunDirectory directory in subDirectoryList)
            {
                writer.WriteSunObjectValue(directory.Name, (byte)SunObjectType.Directory);
                writer.WriteCompressedInt(directory.Size);    // Make sure this is always accurate.
                writer.Write(directory.Offset);        // This as well.
            }

            if (!(Parent is SunFile))
            {
                if (SunProperties.Count > 0)
                {   // Writes property list header.
                    // It's basically telling the file "Heads up, there are X amount of properties about to be sequentially written."
                    writer.WriteCompressedInt(SunProperties.Count);
                    writer.Write((byte)SunObjectType.Property);
                }
            }

            foreach (SunProperty prop in sunPropertyList)
                prop.WriteValue(writer);

            foreach (SunDirectory directory in subDirectoryList)
            {
                if (directory.Size > 0)
                    directory.SaveDirectoryContents(writer);
                else
                    writer.Write((byte)0);
            }
        }

        public uint GetOffsets(uint currentOffset)
        {
            Offset = currentOffset;
            currentOffset += (uint)offsetSize;
            foreach (SunDirectory dir in subDirectoryList)
            {
                currentOffset = dir.GetOffsets(currentOffset);
            }
            return currentOffset;
        }

        /// <summary>
        /// Calculates the sizes and offset sizes of every SunDirectory in the File.
        /// </summary>
        public int GenerateFileInfo()
        {
            int a;
            Size = 0;
            offsetSize = 0;
            if (Parent is SunFile)
            {
                TopLevelEntryCount = subDirectoryList.Count + sunPropertyList.Count;
                if (TopLevelEntryCount == 0)
                {
                    offsetSize = 1;
                    return Size = 0;
                }
                Size += SunFileHelper.GetCompressedIntLength(TopLevelEntryCount);
                offsetSize += SunFileHelper.GetCompressedIntLength(TopLevelEntryCount);
            }
            else
            {
                if (SunProperties.Count > 0)
                {
                    a = SunFileHelper.GetCompressedIntLength(SunProperties.Count);
                    Size += a;  /**/ offsetSize += a;
                    Size++;     /**/ offsetSize++;  // Byte SunObjectType (4)
                }
            }

            foreach (SunProperty prop in SunProperties)
            {
                // Calculating size of directory (not the whole sunfile; an individual sundir)
                Size += prop.Name.Length + 1; /**/ offsetSize += prop.Name.Length + 1;
                Size++;     /**/ offsetSize++;       // Byte SunPropertyType

                switch (prop.PropertyType)
                {
                    case SunPropertyType.Null:
                        Size++; /**/ offsetSize++;
                        break;

                    case SunPropertyType.Short:
                        Size += 2; /**/ offsetSize += 2;
                        break;

                    case SunPropertyType.Int:
                        a = SunFileHelper.GetCompressedIntLength(((SunIntProperty)prop).Value);
                        Size += a; /**/ offsetSize += a;
                        break;

                    case SunPropertyType.Long:
                        a = SunFileHelper.GetCompressedLongLength(((SunIntProperty)prop).Value);
                        Size += a; /**/ offsetSize += a;
                        break;

                    case SunPropertyType.Float:
                        if (((SunFloatProperty)prop).Value == 0)
                        {
                            Size++; /**/ offsetSize++;
                            break;
                        }
                        Size += 4; /**/ offsetSize += 4;
                        break;

                    case SunPropertyType.Double:
                        if (((SunDoubleProperty)prop).Value == 0)
                        {
                            Size++; /**/ offsetSize++;
                            break;
                        }
                        Size += 8; /**/ offsetSize += 8;
                        break;

                    case SunPropertyType.String:
                        a = ((SunStringProperty)prop).Value.Length + 1;
                        Size += a; /**/ offsetSize += a;
                        break;

                    case SunPropertyType.Image:
                        SunImageProperty imageProperty = (SunImageProperty)prop;
                        CalculateImageSize(imageProperty);
                        break;

                    case SunPropertyType.Vector:
                        a = SunFileHelper.GetCompressedIntLength(((SunVectorProperty)prop).X.Value);
                        Size += a; /**/ offsetSize += a;
                        a = SunFileHelper.GetCompressedIntLength(((SunVectorProperty)prop).Y.Value);
                        Size += a; /**/ offsetSize += a;
                        break;

                    case SunPropertyType.Sound: break;

                    default:
                        break;
                }
            }

            foreach (SunDirectory dir in subDirectoryList)
            {
                Size++; /**/ offsetSize++;            // Byte SunObjectType

                Size += dir.Name.Length + 1; /**/ offsetSize += dir.Name.Length + 1;

                Size += dir.GenerateFileInfo();

                a = SunFileHelper.GetCompressedIntLength(dir.Size);
                Size += a; /**/ offsetSize += a;

                Size += sizeof(int); /**/ offsetSize += sizeof(int);  // Offset

                if (dir.Size == 0)      // Byte of empty data
                    Size++;
            }
            if (Size == 0)
                offsetSize++;           // To account for the blank byte
            return Size;
        }

        public void CalculateImageSize(SunImageProperty imageProperty)
        {
            int a;

            Size += 4;  /**/ offsetSize += 4;   // Picture property size
            Size++;     /**/ offsetSize++;      // Property bool ("yes, there are properties associated with this image")
            Size++;     /**/ offsetSize++;      // Gif bool (Marks image as a gif)

            if (imageProperty.SunProperties.Count > 0)
            {
                a = SunFileHelper.GetCompressedIntLength(imageProperty.SunProperties.Count);
                Size += a;  /**/ offsetSize += a;
                Size++;     /**/ offsetSize++;      // Byte SunObjectType.Property
                foreach (SunProperty property in imageProperty.SunProperties)
                {
                    Size += property.Name.Length + 1; /**/ offsetSize += property.Name.Length + 1;
                }
            }

            if (imageProperty.IsGif)
            {
                a = SunFileHelper.GetCompressedIntLength(imageProperty.Frames.Count);
                Size += a; /**/ offsetSize += a;

                Size++; /**/ offsetSize++;  //Byte SunObjectType.Property

                foreach (SunImageProperty frame in imageProperty.Frames)
                {
                    Size += frame.Name.Length + 1; /**/ offsetSize += frame.Name.Length + 1;
                    Size++;     /**/ offsetSize++;       // Byte SunPropertyType

                    Size += 4;  /**/ offsetSize += 4;   // Picture property size
                    Size++; /**/ offsetSize++;          // Property bool ("yes, there are properties associated with this image")
                    Size++; /**/ offsetSize++;          // Gif bool

                    a = SunFileHelper.GetCompressedIntLength(frame.Width);
                    Size += a; /**/ offsetSize += a;

                    a = SunFileHelper.GetCompressedIntLength(frame.Height);
                    Size += a; /**/ offsetSize += a;

                    Size += 4; /**/ offsetSize += 4;    // Image size length

                    a = frame.GetCompressedBytes().Length;
                    Size += a; /**/ offsetSize += a;
                }
            }
            else
            {
                a = SunFileHelper.GetCompressedIntLength(imageProperty.Width);
                Size += a; /**/ offsetSize += a;

                a = SunFileHelper.GetCompressedIntLength(imageProperty.Height);
                Size += a; /**/ offsetSize += a;

                Size += 4; /**/ offsetSize += 4; // Image size length

                a = imageProperty.GetCompressedBytes().Length;
                Size += a; /**/ offsetSize += a;
            }
        }

        #endregion Custom Members
    }
}