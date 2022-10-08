using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunCreator.MapEditor
{
    public class Board
    {
        private Point mapSize;
        private Point centerPoint;
        private List<Layer> layers = new List<Layer>();

        public List<Layer> Layers
        { get { return layers; } }
    }
}