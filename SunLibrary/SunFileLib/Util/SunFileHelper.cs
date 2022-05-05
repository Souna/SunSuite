namespace SunLibrary.SunFileLib.Util
{
    public class SunFileHelper
    {
        public static int GetCompressedIntLength(int i)
        {
            if (i > 127 || i < -127)
                return 5;
            return 1;
        }

        public static int GetCompressedLongLength(long i)
        {
            if (i > 127 || i < -127)
                return 9;
            return 1;
        }
    }
}