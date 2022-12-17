using SunEditor.Map_Editor.Instance.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunEditor.Map_Editor.Instance.Shapes
{
    /// <summary>
    /// Represents the handles/anchor points of footholds, ropes, and maybe more.
    /// </summary>
    public abstract class Dot : BoardItem, ISnappable
    {
        public Dot(Board board, int x, int y) : base(board, x, y, -1)
        {

        }

       

    }
}
