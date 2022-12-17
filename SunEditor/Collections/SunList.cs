using SunEditor.Collections.Interface;
using SunLibrary.SunFileLib.Structure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunEditor.Collections
{
    public class SunList<T> : List<T>, ISunList
    {
        private ItemTypes _listType;
        private bool _isItem;

        public SunList(ItemTypes listType, bool isItem) : base()
        {
            _listType = listType;
            _isItem = isItem;
        }

        #region ISunList
        public bool IsItem
        {
            get { return _isItem; }
        }

        public ItemTypes ListType 
        {
            get { return _listType; }
        }
        #endregion
    }
}
