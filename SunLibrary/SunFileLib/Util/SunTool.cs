using SunLibrary.SunFileLib.Properties;
using System;

namespace SunLibrary.SunFileLib.Util
{
    public static class SunTool
    {
        public static UInt32 RotateLeft(UInt32 x, byte n)
        {
            return (UInt32)(((x) << (n)) | ((x) >> (32 - (n))));
        }

        public static UInt32 RotateRight(UInt32 x, byte n)
        {
            return (UInt32)(((x) >> (n)) | ((x) << (32 - (n))));
        }

        public static int GetInt(SunProperty source)
        {
            //return source.GetInt();
            return 0;
        }

        public static SunIntProperty SetInt(int value)
        {
            return new SunIntProperty("", value);
        }

        public static string GetString(SunProperty source)
        {
            //return source.GetString();
            return null;
        }

        public static SunStringProperty SetString(string value)
        {
            return new SunStringProperty("", value);
        }

        public static bool GetBool(SunProperty source)
        {
            if (source == null)
                return false;
            //return source.GetInt() == 1;
            return false;
        }

        public static SunIntProperty SetBool(bool value)
        {
            return new SunIntProperty("", value ? 1 : 0);
        }
    }
}