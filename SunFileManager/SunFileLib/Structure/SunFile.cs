using SunFileManager.SunFileLib.Util;
using System;
using System.Collections.Generic;
using System.IO;

namespace SunFileManager.SunFileLib
{
    /// <summary>
    /// The file that contains all of the directories. As top-level as it gets.
    /// <para>A SunFile may only allow SunDirectorys as child nodes. Everything in the file
    /// is then contained within said directories.</para>
    /// <para>By default, a SunFile has an 'invisible' "master" SunDirectory with the same name as the
    /// SunFile which is the parent of all subsequent SunDirectorys.</para>
    /// </summary>
    public class SunFile : SunObject
    {
        #region Fields

        private List<SunDirectory> directoryList = new List<SunDirectory>();
        private string path;
        private string name;
        public string DefaultPath = "@C:\\Users\\SOUND\\Desktop";
        private SunDirectory sunDir;
        public SunHeader header;

        #endregion Fields

        #region Inherited Members

        /// <summary>
        /// The name of the SunFile.
        /// </summary>
        public override string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// Returns the parent object of this SunFile (none).
        /// </summary>
        public override SunObject Parent
        {
            get { return null; }
            set { }
        }

        /// <summary>
        /// Returns the SunFile this... SunFile belongs to.
        /// </summary>
        public override SunFile SunFileParent
        {
            get { return this; }
        }

        /// <summary>
        /// Returns the byte-value type of the SunFile.
        /// </summary>
        public override SunObjectType ObjectType { get { return SunObjectType.File; } }

        public override void Remove()
        {
            try
            {
            }
            catch (Exception e)
            {
                throw new Exception("Error attempting to remove SunFile " + Name);
            }
        }

        /// <summary>
        /// Returns SunDirectory[name]
        /// </summary>
        public new SunObject this[string name]
        {
            get { return SunDirectory[name]; }
        }

        /// <summary>
        /// Returns amount of top-level entries (SunDirectorys) in this SunFile.
        /// </summary>
        public override int TopLevelEntryCount
        {
            get { return sunDir.TopLevelEntryCount; }
        }

        public override void Dispose()
        {
            if (sunDir == null)
                return;
            Header = null;
            path = null;
            name = null;
            sunDir.Dispose();
        }

        #endregion Inherited Members

        /// <summary>
        /// Returns a list of every SunDirectory, nested or top-level, in the file.
        /// </summary>
        public List<SunDirectory> Directories
        { //no work wtf
            get
            {
                directoryList.Clear();
                foreach (SunDirectory dir in SunDirectory.SubDirectories)
                {
                    foreach (SunDirectory nestedDir in dir.SubDirectories)
                    {
                    }
                }
                return directoryList;
            }
        }

        /// <summary>
        /// Returns the file header preceding the SunFile data.
        /// </summary>
        public SunHeader Header { get { return header; } set { header = value; } }

        /// <summary>
        /// Returns the "master" SunDirectory which contains all subsequent directories and their properties.
        /// </summary>
        public SunDirectory SunDirectory { get { return sunDir; } }

        public SunFile()
        {
        }

        /// <summary>
        /// Initialize SunFile with filename and filepath.
        /// </summary>
        public SunFile(string fileName, string filePath)
        {
            this.header = SunHeader.Default();
            // Creates the master SunDirectory which has the same name as the SunFile.
            sunDir = new SunDirectory(fileName, this);
            name = fileName;
            path = filePath;
        }

        /// <summary>
        /// Initialize Sunfile with filepath.
        /// </summary>
        public SunFile(string filePath)
        {
            name = Path.GetFileName(filePath);
            path = filePath;
        }

        /// <summary>
        /// Returns filepath of this SunFile.
        /// </summary>
        public string FilePath
        {
            get { return path; }
        }

        /// <summary>
        /// Saves the SunFile to disk.
        /// <br>Called by the <b>FileManager</b>.</br>
        /// </summary>
        public void SaveToDisk(string path)
        {
            sunDir.GenerateFileInfo();
            uint totalLength = sunDir.GetOffsets(Header.FileStart);
            Header.FileSize = totalLength - Header.FileStart;

            SunBinaryWriter sunWriter = new SunBinaryWriter(File.Create(path));

            for (int i = 0; i < Header.Identifier.Length; i++)
                sunWriter.Write((byte)Header.Identifier[i]);
            for (int i = 0; i < Header.Ascii.Length; i++)
                sunWriter.Write((byte)Header.Ascii[i]);
            sunWriter.Write(Header.FileSize);
            sunWriter.Write(Header.FileStart);
            sunWriter.WriteNullTerminatedString(Header.Copyright);

            sunDir.SaveToDisk(sunWriter);

            sunWriter.Close();
        }
    }
}