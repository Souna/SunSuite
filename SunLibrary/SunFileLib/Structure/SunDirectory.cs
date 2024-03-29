﻿using SunLibrary.SunFileLib.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace SunLibrary.SunFileLib.Structure
{
    /// <summary>
    /// The "folders" of the SunFile structure.
    /// <para>SunDirectories contain all of the content of the SunFile. Every SunDirectory is
    /// capable of storing images, as well as having additional nested sub-directories.</para>
    /// </summary>
    public class SunDirectory : SunObject
    {
        #region Fields

        public List<SunImage> images = new List<SunImage>();
        public List<SunDirectory> subDirs = new List<SunDirectory>();
        public SunBinaryReader reader;
        private uint offset = 0;
        private string name;
        private uint hash;
        private int size, checksum, offsetSize;
        private SunObject parent;
        private SunFile sunFile;
        private int topLevelEntryCount;

        #endregion Fields

        #region Inherited Members

        #region SunObject

        public override void Dispose()
        {
            name = null;
            reader = null;
            foreach (SunImage img in images)
                img.Dispose();
            foreach (SunDirectory dir in SubDirectories)
                dir.Dispose();
            SunImages.Clear();
            SubDirectories.Clear();
            images = null;
            subDirs = null;
        }

        /// <summary>
        /// The name of the SunDirectory.
        /// </summary>
        public override string Name
        { get { return name; } set { name = value; } }

        /// <summary>
        /// Returns the parent object of this SunDirectory.
        /// </summary>
        public override SunObject Parent
        { get { return parent; } internal set { parent = value; } }

        /// <summary>
        /// Returns the SunFile this directory belongs to.
        /// </summary>
        public override SunFile SunFileParent
        { get { return sunFile; } }

        /// <summary>
        /// Returns the byte-value type of the SunDirectory.
        /// </summary>
        public override SunObjectType ObjectType
        { get { return SunObjectType.Directory; } }

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
        public int TopLevelEntryCount
        { get { return SubDirectories.Count + SunImages.Count; } set { topLevelEntryCount = value; } }

        /// <summary>
        /// Returns a directory by its given name.
        /// </summary>
        /// <param name="directory">Bool value to indicate we're after a directory and not a property.
        /// Properties get addressed with just name.</param>
        public new SunObject this[string name]
        {
            get
            {
                foreach (SunImage i in SunImages)
                    if (i.Name.ToLower() == name.ToLower())
                        return i;

                foreach (SunDirectory d in SubDirectories)
                    if (d.name.ToLower() == name.ToLower())
                        return d;
                return null;
            }
            set
            {
                if (value != null)
                {
                    value.Name = name;
                    if (value is SunDirectory directory)
                        AddDirectory(directory);
                    else if (value is SunImage image)
                        AddImage(image);
                    else
                        throw new ArgumentException("Value must be a Directory or an Image.");
                }
            }
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

        #endregion SunObject

        #endregion Inherited Members

        #region Custom Members

        public SunDirectory()
        {
        }

        public SunDirectory(string dirName)
        {
            name = dirName;
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

        public SunDirectory(SunBinaryReader reader, string dirName, SunFile sunFile)
        {
            this.reader = reader;
            this.name = dirName;
            this.sunFile = sunFile;
            this.parent = sunFile;//idk if this parent thing works the same as the line above it
        }

        /// <summary>
        /// Finds all child images in a SunDirectory
        /// </summary>
        public List<SunImage> FindChildImages()
        {
            List<SunImage> imgList = new List<SunImage>();
            imgList.AddRange(images);
            foreach (SunDirectory subdir in SubDirectories)
            {
                imgList.AddRange(subdir.FindChildImages());
            }
            return imgList;
        }

        /// <summary>
        /// Returns size, in bytes, of the directory in the SunFile.
        /// </summary>
        public int Size
        { get { return size; } set { size = value; } }

        /// <summary>
        /// Returns offset to the folder.
        /// </summary>
        public uint Offset
        { get { return offset; } set { offset = value; } }

        /// <summary>
        /// Returns the list of subdirectories inside the master directory.
        /// </summary>
        public List<SunDirectory> SubDirectories
        {
            get { return subDirs; }
        }

        /// <summary>
        /// Returns the directory's Checksum value
        /// </summary>
        public int Checksum
        {
            get { return checksum; }
            set { checksum = value; }
        }

        /// <summary>
        /// Returns the list of images inside this directory.
        /// </summary>
        public List<SunImage> SunImages
        {
            get { return images; }
        }

        /// <summary>
        /// Removes a directory from the list.
        /// </summary>
        public void RemoveDirectory(SunDirectory dir)
        {
            subDirs.Remove(dir);
            dir.Parent = null;
        }

        public void RemoveImage(SunImage img)
        {
            images.Remove(img);
            img.Parent = null;
        }

        /// <summary>
        /// Adds a SunDirectory to the list of subdirectories.
        /// </summary>
        public void AddDirectory(SunDirectory dir)
        {
            subDirs.Add(dir);
        }

        /// <summary>
        /// Adds a SunImage to the list of sun images.
        /// </summary>
        public void AddImage(SunImage img)
        {
            images.Add(img);
            img.Parent = this;
        }

        /// <summary>
        /// Saves the SunDirectory to disk within the <b>SunFile</b>.
        /// </summary>
        public void SaveDirectory(SunBinaryWriter writer)
        {
            Offset = (uint)writer.BaseStream.Position;

            writer.WriteCompressedInt(TopLevelEntryCount);
            if (TopLevelEntryCount == 0)
            {
                Size = 0;
                return;
            }

            foreach (SunImage img in images)
            {
                writer.WriteSunObjectValue(img.Name, (byte)SunObjectType.Image);
                writer.WriteCompressedInt(img.Size);
                writer.WriteCompressedInt(img.Checksum);
                writer.WriteOffset(img.Offset);
            }
            foreach (SunDirectory dir in subDirs)
            {
                writer.WriteSunObjectValue(dir.Name, (byte)SunObjectType.Directory);
                writer.WriteCompressedInt(dir.Size);
                writer.WriteCompressedInt(dir.Checksum);
                writer.WriteOffset(dir.Offset);
            }
            foreach (SunDirectory dir in subDirs)
            {
                if (dir.Size > 0)
                    dir.SaveDirectory(writer);
                else
                    writer.Write((byte)0);  //00 indicates empty directory
            }
        }

        public void SaveImages(SunBinaryWriter sunWriter, FileStream fileStream)
        {
            foreach (SunImage img in images)
            {
                if (img.Changed)
                {
                    fileStream.Position = img.tempFileStart;
                    byte[] buffer = new byte[img.Size];
                    fileStream.Read(buffer, 0, img.Size);
                    sunWriter.Write(buffer);
                }
                else
                {
                    img.reader.BaseStream.Position = img.tempFileStart;
                    sunWriter.Write(img.reader.ReadBytes((int)(img.tempFileEnd - img.tempFileStart)));
                }
            }
            foreach (SunDirectory dir in subDirs)
            {
                dir.SaveImages(sunWriter, fileStream);
            }
        }

        public uint GetOffsets(uint currentOffset)
        {
            Offset = currentOffset;
            currentOffset += (uint)offsetSize;
            foreach (SunDirectory dir in subDirs)
            {
                currentOffset = dir.GetOffsets(currentOffset);
            }
            return currentOffset;
        }

        public uint GetImgOffsets(uint currentOffset)
        {
            foreach (SunImage img in images)
            {
                img.Offset = currentOffset;
                currentOffset += (uint)img.Size;
            }
            foreach (SunDirectory dir in subDirs)
            {
                currentOffset = dir.GetImgOffsets(currentOffset);
            }
            return currentOffset;
        }

        /// <summary>
        /// Calculates the size and offset of every SunDirectory/SunImage in the File.
        /// </summary>
        public int GenerateFileInfo(string tempFileName)
        {
            size = 0;
            if (TopLevelEntryCount == 0)
            {
                offsetSize = 1;
                return (size = 0);
            }
            size = SunFileHelper.GetCompressedIntLength(TopLevelEntryCount);
            offsetSize = SunFileHelper.GetCompressedIntLength(TopLevelEntryCount);

            SunBinaryWriter imgWriter = null;
            MemoryStream memoryStream = null;
            FileStream fileWrite = new FileStream(tempFileName, FileMode.Append, FileAccess.Write);

            foreach (SunImage img in SunImages)
            {
                if (img.changed)
                {
                    memoryStream = new MemoryStream();
                    imgWriter = new SunBinaryWriter(memoryStream);
                    img.SaveImage(imgWriter);
                    img.Checksum = 0;
                    foreach (byte b in memoryStream.ToArray())
                    {
                        img.Checksum += b;
                    }
                    img.tempFileStart = fileWrite.Position;
                    fileWrite.Write(memoryStream.ToArray(), 0, (int)memoryStream.Length);
                    img.tempFileEnd = fileWrite.Position;
                    memoryStream.Dispose();
                    imgWriter.Close();
                }
                else
                {
                    img.tempFileStart = img.Offset;
                    img.tempFileEnd = img.Offset + img.Size;
                }
                img.UnparseImage();

                Size++; //entrycount
                int nameLength = img.Name.Length + 1;
                Size += nameLength;
                int imgLength = img.Size;
                Size += SunFileHelper.GetCompressedIntLength(imgLength);
                Size += imgLength;
                Size += SunFileHelper.GetCompressedIntLength(img.Checksum);
                Size += 4;

                offsetSize++;   //entrycount
                offsetSize += nameLength;
                offsetSize += SunFileHelper.GetCompressedIntLength(imgLength);
                offsetSize += SunFileHelper.GetCompressedIntLength(img.Checksum);
                offsetSize += 4;
                //if (img.Changed)
                //    imgWriter.Close();
            }
            fileWrite.Close();

            foreach (SunDirectory dir in SubDirectories)
            {
                int nameLength = dir.Name.Length + 1;
                Size++; //entrycount
                Size += nameLength;
                Size += dir.GenerateFileInfo(tempFileName);
                Size += SunFileHelper.GetCompressedIntLength(dir.Size);
                Size += SunFileHelper.GetCompressedIntLength(dir.Checksum);
                Size += 4;  //offset

                offsetSize++; //entrycount
                offsetSize += nameLength;
                offsetSize += SunFileHelper.GetCompressedIntLength(dir.Size);
                offsetSize += SunFileHelper.GetCompressedIntLength(dir.Checksum);
                offsetSize += 4;
            }
            return Size;
        }

        /// <summary>
        /// Parse directories and images in the SunFile.
        /// </summary>
        /// <param name="parseImages">Only load images but does not parse them if true.</param>
        public void ParseDirectory(bool parseImages)
        {
            int entryCount = reader.ReadCompressedInt();
            for (int i = 0; i < entryCount; i++)
            {
                byte type = reader.ReadByte();
                string fileName = null;

                long rememberPos = 0;
                switch (type)
                {
                    case 2: // 2 = .img
                    case 3: // 3 = directory
                        {
                            fileName = reader.ReadString();
                            rememberPos = reader.BaseStream.Position;
                            break;
                        }
                    case 4:// 4 = SunProperty
                        {
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
                reader.BaseStream.Position = rememberPos;
                int fileSize = reader.ReadCompressedInt();
                int checksum = reader.ReadCompressedInt();
                uint offset = reader.ReadOffset();
                if (type == 2) // Image
                {
                    SunImage img = new SunImage(fileName, reader);
                    img.Size = fileSize;
                    img.Checksum = checksum;
                    img.Offset = offset;
                    img.Parent = this;  //how does this line tell how many properties are in the img???
                    images.Add(img);
                }
                else if (type == 3)  // Directory
                {
                    SunDirectory subDir = new SunDirectory(reader, fileName, sunFile);
                    subDir.Size = fileSize;
                    subDir.Checksum = checksum;
                    subDir.Offset = offset;
                    subDir.Parent = this;
                    subDirs.Add(subDir);
                }
            }

            if (parseImages)
            {
                foreach (SunImage img in SunImages)
                {
                    img.ParseImage();
                }
            }

            foreach (SunDirectory subdir in subDirs)
            {
                reader.BaseStream.Position = subdir.offset;  //see if this line is even needed
                subdir.ParseDirectory(parseImages);
            }
        }

        public SunDirectory DeepClone()
        {
            SunDirectory result = (SunDirectory)MemberwiseClone();
            result.SubDirectories.Clear();
            result.SunImages.Clear();
            foreach (SunDirectory dir in SubDirectories)
                result.SubDirectories.Add(dir.DeepClone());
            foreach (SunImage img in SunImages)
                result.SunImages.Add(img.DeepClone());
            return result;
        }

        /// <summary>
        /// Returns amount of images under this directory.
        /// </summary>
        public int CountImages()
        {
            int result = images.Count;
            foreach (SunDirectory subdir in SubDirectories)
                result += subdir.CountImages();
            return result;
        }

        #endregion Custom Members
    }
}