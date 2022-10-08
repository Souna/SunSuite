using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunCreator.MapEditor.UndoRedo
{
    public enum UndoRedoType
    {
        ItemDeleted,
        ItemAdded,
        ItemMoved,
        ItemFlipped,
        LineRemoved,
        LineAdded,
        ToolTipLinked,
        ToolTipUnlinked,
        BackgroundMoved,
        ItemsUnlinked,
        ItemsLinked,
        ItemsLayerChanged,
        ItemLayerPlatformChanged,
        RopeRemoved,
        RopeAdded,
        ItemZChanged,
        ViewRangeChanged,
        MapCenterChanged,
        LayerTileSetChanged,
        zMChanged
    }
}