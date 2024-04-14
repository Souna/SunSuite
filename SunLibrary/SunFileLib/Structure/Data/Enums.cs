using System;

namespace SunLibrary.SunFileLib.Structure.Data
{
    [Flags]
    public enum ItemTypes
    {
        None = 0x0,
        Tiles = 0x1,
        Objects = 0x2,
        Mobs = 0x4,
        NPCs = 0x8,
        Ropes = 0x10,
        Footholds = 0x20,
        Portals = 0x40,
        Chairs = 0x80,
        Reactors = 0x100,
        ToolTips = 0x200,
        Backgrounds = 0x400,
        Misc = 0x800,

        All = 0xFFF
    }

    // Where we would define any field limits/forced behavior
    [Flags]
    public enum FieldLimit
    {
        FIELD_OPTION_NONE = 0
    }

    // Where we'd define whether or not a map is of a specific type which would have its own rules
    public enum FieldType
    {
        DEFAULT = 0
    }

    // Definitions for different types of portals available for use on maps
    public static class PortalType
    {
        public const string
            SPAWN_POINT = "portal_spawnPoint",
            VISIBLE = "portal_visible",
            INVISIBLE = "portal_invisible";
    }

    /// <summary>
    /// A flag that determines how the background is rendered.
    /// If vertical tiling and cy is 0, then the height of the image is used to separate the tiles.
    /// Otherwise, cy is used.
    /// If horizontal tiling and cx is 0, then the width of the image is used to separate the tiles.
    /// Otherwise, cx is used.
    /// </summary>
    public enum BackgroundType
    {
        //No tiling. Use rx and ry for parallax.
        Regular = 0,

        //Horizontal tiling. Use rx and ry for parallax.
        HorizontalTiles = 1,

        //Vertical tiling. Use rx and ry for parallax.
        VerticalTiles = 2,

        //Both vertical and horizontal tiling. Use rx and ry for parallax.
        HVTiles = 3,

        //Horizontal tiling. Use ry along with timestamp for moving and rx for parallax
        HorizontalMoving = 4,

        //Vertical tiling. Use rx along with timestamp for moving and ry for parallax.
        VerticalMoving = 5,

        //Both vertical and horizontal tiling. Use ry along with timestamp for moving and rx for parallax.
        HorizontalMovingHVTiles = 6,

        //Both vertical and horizontal tiling. Use rx along with timestamp for moving and ry for parallax.
        VerticalMovingHVTiles = 7
    }

    public enum MapType
    {
        RegularMap
    }
}