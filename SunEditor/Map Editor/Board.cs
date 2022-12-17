using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SunEditor.Map_Editor.Instance.Shapes;
using SunLibrary.SunFileLib.Structure.Data;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;
using Point = Microsoft.Xna.Framework.Point;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using SunEditor.Map_Editor.Input;
using SunEditor.Collections;
using SunEditor.Collections.Interface;

namespace SunEditor.Map_Editor
{
    /// <summary>
    /// The board holding all of our objects
    /// </summary>
    public class Board
    {
        // Map-specific fields
        private Point _mapSize;
        private Point _centerPoint;
        private Point _miniMapPosition;

        // MiniMap
        private Bitmap _miniMap;
        private Rectangle _miniMapArea;
        private MiniMapRectangle _miniMapRectangle;
        private Texture2D _miniMapTexture;

        private int _mag = 16;
        private int _horizontalScroll = 0;
        private int _verticalScroll = 0;
        private int _selectedPlatform = 0;
        private int _selectedLayerIndex = 0;
        ItemTypes _visibleTypes;
        ItemTypes _editedTypes;
        private MultiBoard _parent;
        private MapInfo _mapInfo = new MapInfo();
        private ViewRangeRectangle _viewRangeRectangle = null;
        private List<BoardItem> _selected = new List<BoardItem>();
        private List<Layer> _layers= new List<Layer>();
        private bool _selectedAllLayers;
        private bool _selectedAllPlatforms;
        private bool _loading;
        private Mouse _mouse;
        private ContextMenuStrip _contextMenu = null;
        private BoardItemsManager _boardItemsManager;
        private SerializationManager _serializationManager = null;


        public Board(Point mapSize, Point center, MultiBoard parent, ItemTypes visibleTypes, ItemTypes editedTypes, ContextMenuStrip menu)
        {
            _mapSize = mapSize;
            _centerPoint = center;
            _parent = parent;
            _visibleTypes = visibleTypes;
            _editedTypes = editedTypes;
            _contextMenu = menu;

            _boardItemsManager= new BoardItemsManager(this);
            _mouse = new Mouse(this);
            _serializationManager= new SerializationManager(this);
        }

        #region Methods
        public void RenderList(ISunList list, SpriteBatch sprite, int xShift, int yShift, SelectionInfo selInfo)
        {
            if (list.ListType == ItemTypes.None)
            {
                foreach (BoardItem item in list)
                {

                    if (Parent.IsItemInRange(item.X, item.Y, item.Width, item.Height, xShift - item.Origin.X, yShift - item.Origin.Y) && ((selInfo.VisibleTypes & item.Type) != 0))
                        item.Draw(sprite, item.GetColor(sel, item.Selected), xShift, yShift);
                }
            }
            else if ((sel.visibleTypes & list.ListType) != 0)
            {
                if (list.IsItem)
                {
                    foreach (BoardItem item in list)
                    {
                        if (parent.IsItemInRange(item.X, item.Y, item.Width, item.Height, xShift - item.Origin.X, yShift - item.Origin.Y))
                            item.Draw(sprite, item.GetColor(sel, item.Selected), xShift, yShift);
                    }
                }
                else
                {
                    foreach (MapleLine line in list)
                    {
                        if (parent.IsItemInRange(Math.Min(line.FirstDot.X, line.SecondDot.X), Math.Min(line.FirstDot.Y, line.SecondDot.Y), Math.Abs(line.FirstDot.X - line.SecondDot.X), Math.Abs(line.FirstDot.Y - line.SecondDot.Y), xShift, yShift))
                            line.Draw(sprite, line.GetColor(sel), xShift, yShift);
                    }
                }
            }
        }

        //public static System.Drawing.Bitmap ResizeImage()

        //public static System.Drawing.Bitmap CropImage()

        //public void RenderBoard()

        //public void CreateLayers()

        //public void Dispose()

        public SelectionInfo GetUserSelectionInfo()
        {
            return new SelectionInfo(_selectedAllLayers ? -1 : _selectedLayerIndex, _selectedAllPlatforms ? -1 : _selectedPlatform, _visibleTypes, _editedTypes);
        }
        #endregion Methods

        #region Properties
        public ItemTypes VisibleTypes { get { return _visibleTypes; } set { _visibleTypes = value; } }

        public ItemTypes EditedTypes { get { return _editedTypes; } set { _editedTypes = value; } }

        public int Mag
        {
            get { return _mag; }
            set { lock(Parent) { _mag = value; } }
        }
        public MapInfo MapInfo
        {
            get { return _mapInfo; }
            set
            {
                lock(Parent)
                {
                    _mapInfo = value;
                }
            }
        }
        public Bitmap MiniMap
        {
            get { return _miniMap; }
            set { lock (Parent) { _miniMap = value; _miniMapTexture = null; } }
        }

        public Point MinimapPosition
        {
            get { return _miniMapPosition; }
            set { _miniMapPosition = value; }
        }

        public int HorizontalScroll
        {
            get
            {
                return _horizontalScroll;
            }
            set
            {
                lock (Parent)
                {
                    _horizontalScroll = value;
                    Parent.SetHorizontalScrollbarValue(_horizontalScroll);
                }
            }
        }

        public int VerticalScroll
        {
            get
            {
                return _verticalScroll;
            }
            set
            {
                lock (Parent)
                {
                    _verticalScroll = value;
                    Parent.SetVerticalScrollbarValue(_verticalScroll);
                }
            }
        }

        public Point CenterPoint
        {
            get { return _centerPoint; }
            internal set { _centerPoint = value; }
        }

        public MultiBoard Parent
        {
            get
            {
                return _parent;
            }
            set
            {
                _parent = value;
            }
        }

        public Mouse Mouse
        {
            get { return _mouse; }
        }

        public Point MapSize
        {
            get { return _mapSize; }
            set
            {
                _mapSize = value;
                _miniMapArea = new Rectangle(0, 0, MapSize.X / _mag, MapSize.Y / _mag);
            }
        }
        public Rectangle MinimapArea
        {
            get { return _miniMapArea; }
        }

        public ViewRangeRectangle ViewRangeRectangle
        {
            get { return _viewRangeRectangle; }
            set
            {
                _viewRangeRectangle = value;
                //_contextMenu.Items[1].Enabled = value == null;
            }
        }
        public MiniMapRectangle MinimapRectangle
        {
            get { return _miniMapRectangle; }
            set
            {
                _miniMapRectangle = value;
                //_contextMenu.Items[2].Enabled = value == null;
                Parent.OnMiniMapStateChanged(this, _miniMapRectangle != null);
            }
        }

        public BoardItemsManager BoardItemsManager
        {
            get { return _boardItemsManager; }
        }

        public List<BoardItem> SelectedItems
        {
            get { return _selected; }
        }

        public List<Layer> Layers
        { 
            get { return _layers; }
        }

        public int SelectedLayerIndex
        {
            get { return _selectedLayerIndex; }
            set
            {
                lock(Parent)
                {
                    _selectedLayerIndex= value;
                }
            }
        }

        public bool SelectedAllLayers
        {
            get { return _selectedAllLayers; }
            set { _selectedAllLayers = value; }
        }

        public Layer SelectedLayer
        {
            get { return Layers[SelectedLayerIndex]; }
        }

        public ContextMenuStrip Menu
        {
            get { return _contextMenu; }
        }

        public int SelectedPlatform
        {
            get { return _selectedPlatform; }
            set { _selectedPlatform= value; }
        }

        public bool SelectedAllPlatforms
        {
            get { return _selectedAllPlatforms; }
            set { _selectedAllPlatforms = value; }
        }

        public bool Loading
        {
            get { return _loading; }
            set { _loading = value; }
        }

        public SerializationManager SerializationManager
        {
            get { return _serializationManager; }
        }

        #endregion Properties
    }
}
