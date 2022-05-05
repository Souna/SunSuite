using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunLibrary.SunFileLib.Util
{
    public class SunTool
    {
        public static Hashtable StringCache = new Hashtable();

        public static UInt32 RotateLeft(UInt32 x, byte n)
        {
            return (UInt32)(((x) << (n)) | ((x) >> (32 - (n))));
        }

        public static UInt32 RotateRight(UInt32 x, byte n)
        {
            return (UInt32)(((x) >> (n)) | ((x) << (32 - (n))));
        }
    }
}