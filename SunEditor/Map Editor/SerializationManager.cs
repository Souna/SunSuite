using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunEditor.Map_Editor
{
    public class SerializationManager
    {
        public Board _board;

        public SerializationManager(Board board)
        {
            _board = board;
        }
    }
}
