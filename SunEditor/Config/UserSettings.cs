using SunLibrary.Config;

namespace SunEditor.Config
{
    public class UserSettings : SettingsBase
    {
        public bool DarkMode { get; set; } = false;

        public override void WriteSettings(SettingsWriter writer)
        {
            writer.Write("DarkMode", DarkMode);
        }

        public override void ReadSettings(SettingsReader reader)
        {
            DarkMode = reader.Read("DarkMode", false);
        }
    }
}