using SunLibrary.SunFileLib.Structure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunEditor.Map_Editor
{
    public struct SelectionInfo
    {
        public SelectionInfo(int selectedLayer, int selectedPlatform, ItemTypes visibleTypes, ItemTypes editedTypes)
        {
            _selectedLayer = selectedLayer;
            _selectedPlatform = selectedPlatform;
            _visibleTypes = visibleTypes;
            _editedTypes = editedTypes;
        }

        public int _selectedLayer;
        public int _selectedPlatform;
        public ItemTypes _visibleTypes;
        public ItemTypes _editedTypes;
    }
}
