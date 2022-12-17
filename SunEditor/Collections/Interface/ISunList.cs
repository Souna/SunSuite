using SunLibrary.SunFileLib.Structure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunEditor.Collections.Interface
{
    public interface ISunList
    {
        bool IsItem { get; }
        ItemTypes ListType { get; }
    }
}
