using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunCreator.MapEditor
{
    public class Layer
    {
        //private List<LayeredItem> items = new List<LayeredItem>(); //needed?
        private SortedSet<int> zms = new SortedSet<int>();

        private int num;
        private Board board;
        private string tileSet = null;

        public Layer(Board board)
        {
            this.board = board;
            if (board.Layers.Count > 8) throw new NotSupportedException("Max layers = 8");
            num = board.Layers.Count;
            board.Layers.Add(this);
        }
    }
}