using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunCreator.MapEditor.UndoRedo
{
    public class UndoRedoManager
    {
        public List<UndoRedoBatch> UndoList = new List<UndoRedoBatch>();
        public List<UndoRedoBatch> RedoList = new List<UndoRedoBatch>();
        private Board parentBoard;

        public UndoRedoManager(Board parentBoard)
        {
            this.parentBoard = parentBoard;
        }
    }
}