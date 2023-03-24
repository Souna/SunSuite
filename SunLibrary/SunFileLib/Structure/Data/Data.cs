using System.Collections.Generic;

namespace SunLibrary.SunFileLib.Structure.Data
{
    public static class Tables
    {
        public static Dictionary<string, string> PortalTypeNames = new Dictionary<string, string>() {
            { "sp", "Start Point"},
            { "pi", "Invisible" },
            { "pv", "Visible" },
            { "pc", "Collision" },
            { "pg", "Changable" },
            { "pgi", "Changable Invisible" },
            { "tp", "Town Portal" },
            { "ps", "Script" },
            { "psi", "Script Invisible" },
            { "pcs", "Script Collision" },
            { "ph", "Hidden" },
            { "psh", "Script Hidden" },
            { "pcj", "Vertical Spring" },
            { "pci", "Custom Impact Spring" },
            { "pcig", "Unknown (PCIG)" }};

        public static string[] BackgroundTypeNames = new string[] {
            "Regular",
            "Horizontal Copies",
            "Vertical Copies",
            "H+V Copies",
            "Horizontal Moving+Copies",
            "Vertical Moving+Copies",
            "H+V Copies, Horizontal Moving",
            "H+V Copies, Vertical Moving"
        };
    }

    public static class SunConstants
    {
        public const int MinMapId = 0;
        public const int MaxMapId = 999999999;
    }

    public enum QuestState
    {
        Available = 0,
        InProgress = 1,
        Completed = 2
    }
}