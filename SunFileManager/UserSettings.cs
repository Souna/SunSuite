using System;
using System.IO;
using System.Text.Json;

namespace SunFileManager.Config
{
    public class UserSettings
    {
        private static readonly string SettingsPath =
            Path.Combine(AppContext.BaseDirectory, "settings.json");

        public bool DarkMode { get; set; } = false;
        public bool AutoParseImages { get; set; } = false;
        public bool ShowOriginCross { get; set; } = true;

        public static UserSettings Load()
        {
            try
            {
                if (File.Exists(SettingsPath))
                {
                    string json = File.ReadAllText(SettingsPath);
                    return JsonSerializer.Deserialize<UserSettings>(json) ?? new UserSettings();
                }
            }
            catch { }
            return new UserSettings();
        }

        public void Save()
        {
            try
            {
                string json = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(SettingsPath, json);
            }
            catch { }
        }
    }
}
