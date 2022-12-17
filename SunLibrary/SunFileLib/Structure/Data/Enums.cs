using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunLibrary.SunFileLib.Structure.Data
{
    public enum ItemTypes
    {
        None = 0,
        Backgrounds = 1,
        Tiles = 2,
        Footholds = 3,
        Objects = 4
    }

    [Flags]
    public enum FieldLimit  // Where we would define any field limits/forced behavior
    {
        FIELD_OPTION_NONE = 0
    }

    public enum FieldType   // Where we'd define whether or not a map is of a specific type which would have its own rules
    {
        DEFAULT = 0
    }

    public static class PortalType  // Definitions for different types of portals available for use on maps
    {
        public const string
            SPAWN_POINT = "portal_spawnPoint",
            VISIBLE = "portal_visible",
            INVISIBLE = "portal_invisible";
    }

    public enum BackgroundType
    {
        Regular = 0,
        HorizontalTiles = 1,
        VerticalTiles = 2,
        HVTiles = 3,
        HorizontalMoving = 4,
        VerticalMoving = 5,
        HorizontalMovingHVTiles = 6,
        VerticalMovingHVTiles = 7
    }
}