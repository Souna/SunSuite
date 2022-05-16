using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using SunLibrary.SunFileLib.Structure;
using SunLibrary.SunFileLib.Util;

namespace SunLibrary.SunFileLib.Structure
{
    /// <summary>
    /// The file that contains all of the directories. As top-level as it gets.
    /// <para>A SunFile allows for SunDirectories as child nodes. Everything in the file
    /// is then contained within said directories.</para>
    /// <para>By default, a SunFile has an 'invisible' "master" SunDirectory with the same name as the
    /// SunFile which is the parent of all subsequent SunDirectories.</para>
    /// </summary>
    public class SunFile : SunObject
    {
        #region Fields

        private string path;
        private SunDirectory sunDir;
        public SunHeader header;
        private string name;

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
            internal set { }
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
        public override SunObjectType ObjectType
        { get { return SunObjectType.File; } }

        public override void Remove()
        {
            try
            {
                Dispose();
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
        public int TopLevelEntryCount
        {
            get { return sunDir.TopLevelEntryCount; }
        }

        public override void Dispose()
        {
            if (sunDir == null || SunDirectory.reader == null)
                return;
            SunDirectory.reader.Close();
            Header = null;
            path = null;
            name = null;
            SunDirectory.Dispose();
        }

        #endregion Inherited Members

        /// <summary>
        /// Returns the file header preceding the SunFile data.
        /// </summary>
        public SunHeader Header
        { get { return header; } set { header = value; } }

        /// <summary>
        /// Returns the "master" SunDirectory which contains all subsequent directories and their properties.
        /// </summary>
        public SunDirectory SunDirectory
        { get { return sunDir; } set { sunDir = value; } }

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
            // Create Temp File
            string tempFile = Path.GetFileNameWithoutExtension(path) + ".TEMP";
            File.Create(tempFile).Close();

            sunDir.GenerateFileInfo(tempFile);
            uint totalLength = sunDir.GetImgOffsets(sunDir.GetOffsets(Header.FileStart));
            Header.FileSize = totalLength - Header.FileStart;

            try
            {
                SunBinaryWriter sunWriter = new SunBinaryWriter(File.Create(path));
                for (int i = 0; i < Header.Identifier.Length; i++)
                {
                    sunWriter.Write((byte)Header.Identifier[i]);
                }
                //Bloat trash
                for (int i = 0; i < Header.Ascii.Length; i++)
                {
                    sunWriter.Write((byte)Header.Ascii[i]);
                }
                sunWriter.Write((long)Header.FileSize);
                sunWriter.Write(Header.FileStart);
                sunWriter.WriteNullTerminatedString(Header.Copyright);

                long extraHeaderLength = Header.FileStart - sunWriter.BaseStream.Position;
                if (extraHeaderLength > 0)
                {
                    sunWriter.Write(new byte[(int)extraHeaderLength]);
                }
                sunWriter.Header = header;
                sunDir.SaveDirectory(sunWriter);

                using (FileStream fileStream = File.OpenRead(tempFile))
                {
                    sunDir.SaveImages(sunWriter, fileStream);
                    fileStream.Close();
                }
                File.Delete(tempFile);
                sunWriter.Close();
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, null, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Parses the SunFile with all of its contents for loading.
        /// </summary>
        public bool ParseSunFile(out string parseError, bool parseImages)
        {
            bool success = ParseMasterSunDirectory(out parseError, parseImages);
            return success;
        }

        /// <summary>
        /// Parse the directories in the SunFile.
        /// </summary>
        public bool ParseMasterSunDirectory(out string parseError, bool parseImages)
        {
            if (path == null)
            {
                parseError = "Path is null.";
                return false;
            }

            SunBinaryReader reader = new SunBinaryReader(File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read));

            this.Header = new SunHeader();
            this.Header.Identifier = reader.ReadString(8);
            this.Header.Ascii = reader.ReadString(112);
            this.Header.FileSize = reader.ReadUInt64();
            this.Header.FileStart = reader.ReadUInt32();
            this.Header.Copyright = reader.ReadNullTerminatedString();

            reader.ReadBytes((int)(header.FileStart - reader.BaseStream.Position)); //reset to beginning? Does this even do anything?
            reader.Header = this.Header;
            long resetPos = reader.BaseStream.Position;     //position to rollback to if things go bad

            try
            {
                SunDirectory masterDirectory = new SunDirectory(reader, name, this);
                masterDirectory.ParseDirectory(parseImages);
                this.SunDirectory = masterDirectory;
                parseError = "Success";
                return true;
            }
            catch
            {
                reader.BaseStream.Position = resetPos;
                ErrorLogger.Log(ErrorLevel.Critical, "Error occured parsing SunFile.");
                parseError = "Fail";
                return false;
            }
        }
    }
}