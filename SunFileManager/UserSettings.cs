using SunLibrary.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunFileManager.Config
{
    public class UserSettings : SettingsBase
    {
        public bool DarkMode { get; set; } = false;

        public bool AutoParseImages { get; set; } = false;

        public bool NodeWarnings { get; set; } = true;

        public bool FileBoxes { get; set; } = true;

        public bool NodeLines { get; set; } = true;

        public bool HighlightLine { get; set; } = false;

        public override void WriteSettings(SettingsWriter writer)
        {
            writer.Write("DarkMode", DarkMode);
            writer.Write("AutoParseImages", AutoParseImages);
            writer.Write("NodeWarnings", NodeWarnings);
            writer.Write("FileBoxes", FileBoxes);
            writer.Write("NodeLines", NodeLines);
            writer.Write("HighlightLine", HighlightLine);
        }

        public override void ReadSettings(SettingsReader reader)
        {
            DarkMode = reader.Read("DarkMode", false);
            AutoParseImages = reader.Read("AutoParseImages", false);
            NodeWarnings = reader.Read("NodeWarnings", true);
            FileBoxes = reader.Read("FileBoxes", true);
            NodeLines = reader.Read("NodeLines", true);
            HighlightLine = reader.Read("HighlightLine", false);
        }
    }
}