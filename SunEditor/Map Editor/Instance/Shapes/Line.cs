using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunEditor.Map_Editor.Instance.Shapes
{
    //it is important to remember that if the line is connecting mouse and a Dot, the mouse is ALWAYS the second dot.

    public abstract class Line
    {
        private Board _board;
        protected Dot _firstDot;
        protected Dot _secondDot;
        private bool _beforeConnecting;
        private bool _xBind = false;
        private bool _yBind = false;

        public Line(Board board, Dot firstDot)
        {
            _board = board;
            _firstDot = firstDot;
            _firstDot.connectedLines.Add(this);
            _secondDot = board.Mouse;
            _secondDot.connectedLines.Add(this);
            _beforeConnecting = true;
            firstDot.PointMoved += OnFirstDotMoved;
        }

        public Line(Board board, Dot firstDot, Dot secondDot)
        {
            _board = board;
            _firstDot = firstDot;
            _firstDot.connectedLines.Add(this);
            _secondDot = secondDot;
            _secondDot.connectedLines.Add(this);
            _beforeConnecting = false;
            firstDot.PointMoved += OnFirstDotMoved;
            secondDot.PointMoved += OnSecondDotMoved;
        }

        protected Line(Board board)
        {
            _board = board;
            _beforeConnecting = false;
        }

        public void ConnectSecondDot(Dot secondDot)
        {
            if (!_beforeConnecting) return;
            _secondDot.connectedLines.Clear();
            _secondDot = secondDot;
            _secondDot.connectedLines.Add(this);
            secondDot.PointMoved += OnSecondDotMoved;
        }

        public virtual void OnPlaced(List<UndoRedoAction> undoPipe)
        {
            lock (board.ParentControl)
            {
                if (undoPipe != null)
                {
                    undoPipe.Add(UndoRedoManager.LineAdded(this, firstDot, secondDot));
                }
            }
        }

        public virtual void Remove(bool removeDots, List<UndoRedoAction> undoPipe)
        {
            lock (board.ParentControl)
            {
                firstDot.DisconnectLine(this);
                secondDot.DisconnectLine(this);
                if (this is FootholdLine) board.BoardItems.FootholdLines.Remove((FootholdLine)this);
                else if (this is RopeLine) board.BoardItems.RopeLines.Remove((RopeLine)this);
                if (!(secondDot is Mouse) && undoPipe != null)
                {
                    undoPipe.Add(UndoRedoManager.LineRemoved(this, firstDot, secondDot));
                }
                if (removeDots)
                {
                    firstDot.RemoveItem(undoPipe);
                    if (secondDot != null)
                    {
                        secondDot.RemoveItem(undoPipe);
                    }
                }
            }
        }

    }

}
