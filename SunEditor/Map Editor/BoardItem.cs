using Microsoft.Xna.Framework;
using SharpDX.MediaFoundation;
using SunEditor.Map_Editor.Instance.Interface;
using SunLibrary.SunFileLib.Structure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using static System.ComponentModel.Design.ObjectSelectorEditor;
using Point = Microsoft.Xna.Framework.Point;

namespace SunEditor.Map_Editor
{
    public abstract class BoardItem : ISerializableSelector
    {
        protected Vector3 _position;
        private Dictionary<BoardItem, Point> _boundItems = new Dictionary<BoardItem, Point>(); // Key = BoardItem; Value = point (Distance)
        private List<BoardItem> _boundItemsList = new List<BoardItem>();
        private BoardItem _parent = null;
        private bool _selected = false;
        protected Board _board;

        public BoardItem(Board board, int x, int y, int z)
        {
            _position= new Vector3(x, y, z);
            this._board = board;
        }

        #region Methods

        public virtual void Move(int x, int y)
        {
            lock(Board.Parent)
            {

            }
        }

        #endregion Methods

        #region ISerializableSelector

        public class SerializationForm
        {
            public float x, y, z;
        }

        public virtual bool ShouldSelectSerialized
        {
            get
            {
                return _boundItems.Count > 0;
            }
        }

        public virtual List<ISerializableSelector> SelectSerialized(HashSet<ISerializableSelector> serializedItems)
        {
            List<ISerializableSelector> serList = new List<ISerializableSelector>();
            foreach (BoardItem item in BoundItems.Keys)
            {
                serList.Add(item);
            }
            return serList;
        }

        public virtual object Serialize()
        {
            SerializationForm result = new SerializationForm();
            UpdateSerializedForm(result);
            return result;
        }

        protected void UpdateSerializedForm(SerializationForm result)
        {
            result.x = _position.X;
            result.y = _position.Y;
            result.z = _position.Z;
        }

        public virtual IDictionary<string, object> SerializeBindings(Dictionary<ISerializable, long> refDict)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            List<long> bindOrder = new List<long>();
            foreach (BoardItem item in _boundItemsList)
            {
                if (!(item is ISerializable)) // We should only have bound ISerializables (specifically, chairs and foothold anchors)
                    throw new SerializationException("Bound item is not ISerializable");
                long refNum = refDict[(ISerializable)item];
                result.Add(refNum.ToString(), SerializationManager.SerializePoint(_boundItems[item]));
                bindOrder.Add(refNum);
            }
            if (bindOrder.Count > 0)
                result.Add("bindOrder", bindOrder.ToArray());
            return result;
        }

        public BoardItem(Board board, SerializationForm json)
        {
            this._board = board;
            _position = new Vector3(json.x, json.y, json.z);
        }

        public virtual void DeserializeBindings(IDictionary<string, object> bindSer, Dictionary<long, ISerializable> refDict)
        {
            if (!bindSer.ContainsKey("bindOrder"))
                return; // No bindings were serialized
            long[] bindOrder = ((object[])bindSer["bindOrder"]).Cast<long>().ToArray();
            foreach (long id in bindOrder)
            {
                BoardItem item = (BoardItem)refDict[id];
                Point offset = SerializationManager.DeserializePoint(bindSer[id.ToString()]);
                _boundItems.Add(item, offset);
                _boundItemsList.Add(item);
                item._parent = this;
            }
        }

        public virtual void AddToBoard(List<UndoRedoAction> undoPipe)
        {
            if (undoPipe != null)
            {
                OnItemPlaced(undoPipe);
            }
            _board.BoardItems.Add(this, false);
        }

        public virtual void PostDeserializationActions(bool? selected, Point? offset)
        {
            if (selected.HasValue)
            {
                Selected = selected.Value;
            }
            if (offset.HasValue)
            {
                _position.X += offset.Value.X;
                _position.Y += offset.Value.Y;
            }
        }
        #endregion ISerializableSelector

            #region Properties
        public abstract Bitmap Image { get; }
        public abstract Point Origin { get; }
        public abstract ItemTypes Type { get; }

        public abstract int Width { get; }
        public abstract int Height { get; }

        public virtual int X
        {
            get
            {
                return (int) _position.X;
            }
            set
            {
                Move(value, (int)_position.Y);
            }
        }

        public virtual int Y
        {
            get
            {
                return (int)_position.Y;
            }
            set
            {
                Move((int)_position.X, value);
            }
        }

        public virtual int Z
        {
            get
            {
                return (int)_position.Z;
            }
            set
            {
                _position.Z = Math.Max(0, value);
            }
        }
        public virtual int Left
        {
            get { return (int)X - Origin.X; }
        }

        public virtual int Top
        {
            get { return (int)Y - Origin.Y; }
        }

        public virtual int Right
        {
            get { return (int)X - Origin.X + Width; }
        }

        public virtual int Bottom
        {
            get { return (int)Y - Origin.Y + Height; }
        }

        public virtual bool Selected
        {
            get
            {
                return _selected;
            }
            set
            {
                lock (Board.Parent)
                {
                    if (_selected == value) return;
                    _selected = value;
                    if (value && !Board.SelectedItems.Contains(this))
                        Board.SelectedItems.Add(this);
                    else if (!value && Board.SelectedItems.Contains(this))
                        Board.SelectedItems.Remove(this);
                    if (Board.SelectedItems.Count == 1)
                        Board.ParentControl.OnSelectedItemChanged(Board.SelectedItems[0]);
                    else if (Board.SelectedItems.Count == 0)
                        Board.ParentControl.OnSelectedItemChanged(null);
                }
            }
        }

        public Board Board
        {
            get { return _board; }
        }

        #endregion Properties
    }
}
