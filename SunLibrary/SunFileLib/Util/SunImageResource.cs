using SunLibrary.SunFileLib.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunLibrary.SunFileLib.Util
{
    public class SunImageResource : IDisposable
    {
        bool parsed;
        SunImage img;
        public SunImageResource(SunImage img)
        {
            this.img = img;
            this.parsed = img.Parsed;
            if (!parsed)
            {
                img.ParseImage();
            }
        }

        public void Dispose()
        {
            if (!parsed)
            {
                img.UnparseImage();
            }
        }
    }
}
