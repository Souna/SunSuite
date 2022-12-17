using SunEditor.Map_Editor.Instance.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Point = Microsoft.Xna.Framework.Point;

namespace SunEditor.Map_Editor.Input
{
    public class Mouse : Dot
    {
        #region Fields
        private Point _origin = new Point(0, 0);
        private Point _singleSelectStartPoint;
        private Point _multiSelectStartPoint;
        private bool _isDown;
        private bool _isMinimapBrowseOngoing;
        private bool _isMultiSelectOngoing;
        private bool _singleSelectStarting;

        #endregion Fields

        #region Methods
        public Mouse(Board board): base(board, 0 ,0) 
        {
            IsDown = false;
        }
        #endregion Methods

        #region Properties

        public Point Origin
        {
            get { return _origin; }
        }

        public Point SingleSelectStartPoint
        {

        }

        public Point MultiSelectStartPoint
        {

        }

        public bool IsDown
        {

        }

        public bool MinimapBrowseOngoing
        {

        }

        public bool MultiSelectStarting
        {

        }

        public bool SingleSelectStarting
        {

        }

        #endregion Properties
    }
}
