/*
 Thanks to http://www.blackbeltcoder.com/Articles/winforms/a-custom-settings-class-for-winforms
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SunLibrary.Config
{
    public abstract class SettingsBase
    {
        public string SettingsPath { get; set; }

        public SettingsBase()
        {
            // These properties must be set by derived class
            SettingsPath = null;
        }

        /// <summary>
        /// Loads user settings from the specified file. The file should
        /// have been created using this class' Save method.
        ///
        /// You must implement ReadSettings for any data to be read.
        /// </summary>
        public void Load(string path)
        {
            SettingsReader reader = new SettingsReader();

            SettingsPath = Path.Combine(path, "Settings.xml");

            // Create folder if it doesn't already exist
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            if (!File.Exists(SettingsPath))
            {
                XmlWriter writer = XmlWriter.Create(SettingsPath);
                writer.WriteStartElement("Settings");

                writer.WriteEndElement();
                writer.WriteEndDocument();
                writer.Flush();
                writer.Close();
                Save();
            }

            reader.Load(SettingsPath);
            ReadSettings(reader);
        }

        /// <summary>
        /// Saves the current settings to the specified file.
        ///
        /// You must implement WriteSettings for any data to be written.
        /// </summary>
        public void Save()
        {
            SettingsWriter writer = new SettingsWriter();
            WriteSettings(writer);
            writer.Save(SettingsPath);
        }

        // Abstract methods
        public abstract void ReadSettings(SettingsReader reader);

        public abstract void WriteSettings(SettingsWriter writer);
    }
}