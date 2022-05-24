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
        public bool UseDark { get; set; }

        public bool ParseImagesAutomatically { get; set; }

        public bool DisplayNodeWarnings { get; set; }

        public bool DisplayLinesBetweenNodes { get; set; }

        public bool DisplayLinesOnRootNodes { get; set; }

        public bool HighlightWholeWidth { get; set; }

        public override void WriteSettings(SettingsWriter writer)
        {
            writer.Write("UseDark", UseDark);
            writer.Write("ParseImagesAutomatically", ParseImagesAutomatically);
            writer.Write("DisplayNodeWarnings", DisplayNodeWarnings);
            writer.Write("DisplayLinesBetweenNodes", DisplayLinesBetweenNodes);
            writer.Write("DisplayLinesOnRootNodes", DisplayLinesOnRootNodes);
            writer.Write("HighlightWholeNodeWidth", HighlightWholeWidth);
        }

        public override void ReadSettings(SettingsReader reader)
        {
            UseDark = reader.Read("UseDark", false);
            ParseImagesAutomatically = reader.Read("ParseImagesAutomatically", true);
            DisplayNodeWarnings = reader.Read("DisplayNodeWarnings", true);
            DisplayLinesBetweenNodes = reader.Read("DisplayLinesBetweenNodes", true);
            DisplayLinesOnRootNodes = reader.Read("DisplayLinesOnRootNodes", true);
            HighlightWholeWidth = reader.Read("HighlightWholeNodeWidth", false);
        }
    }
}