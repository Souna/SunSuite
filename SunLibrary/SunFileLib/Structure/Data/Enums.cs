using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunLibrary.SunFileLib.Structure.Data
{
    public enum ItemTypes
    {
        None,
        Backgrounds,
        Tiles,
        Footholds,
        Objects
    }

    public enum FieldType
    {
        DEFAULT
    }

    public static class PortalType
    {
        public const string
            STARTPOINT = "sp",
            VISIBLE = "pv",
            INVISIBLE = "pi";
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