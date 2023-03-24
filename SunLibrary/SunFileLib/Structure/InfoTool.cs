using SunLibrary.SunFileLib.Properties;

namespace SunLibrary.SunFileLib.Structure
{
    public static class InfoTool
    {
        public static string GetString(SunProperty source)
        {
            return source.GetString();
        }

        public static SunStringProperty SetString(string value)
        {
            return new SunStringProperty("", value);
        }

        public static string GetOptionalString(SunProperty source)
        {
            return source == null ? null : source.GetString();
        }

        public static SunStringProperty SetOptionalString(string value)
        {
            return value == null ? null : SetString(value);
        }

        public static double GetDouble(SunProperty source)
        {
            return source.GetDouble();
        }

        public static SunDoubleProperty SetDouble(double value)
        {
            return new SunDoubleProperty("", value);
        }

        public static int GetInt(SunProperty source)
        {
            return source.GetInt();
        }

        public static SunIntProperty SetInt(int value)
        {
            return new SunIntProperty("", value);
        }

        public static int? GetOptionalInt(SunProperty source)
        {
            return source == null ? (int?)null : source.GetInt();
        }

        public static SunIntProperty SetOptionalInt(int? value)
        {
            return value.HasValue ? SetInt(value.Value) : null;
        }

        public static bool GetBool(SunProperty source)
        {
            if (source == null)
                return false;
            return source.GetInt() == 1;
        }

        public static SunIntProperty SetBool(bool value)
        {
            return new SunIntProperty("", value ? 1 : 0);
        }

        public static SunBool GetOptionalBool(SunProperty source)
        {
            if (source == null) return SunBool.NotExist;
            else return source.GetInt() == 1;
        }

        public static SunIntProperty SetOptionalBool(SunBool value)
        {
            return value.HasValue ? SetBool(value.Value) : null;
        }

        public static float GetFloat(SunProperty source)
        {
            return source.GetFloat();
        }

        public static SunFloatProperty SetFloat(float value)
        {
            return new SunFloatProperty("", value);
        }

        public static float? GetOptionalFloat(SunProperty source)
        {
            return source == null ? (float?)null : source.GetFloat();
        }

        public static SunFloatProperty SetOptionalFloat(float? value)
        {
            return value.HasValue ? SetFloat(value.Value) : null;
        }

        public static int? GetOptionalTranslatedInt(SunProperty source)
        {
            string str = InfoTool.GetOptionalString(source);
            if (str == null) return null;
            return int.Parse(str);
        }

        public static SunStringProperty SetOptionalTranslatedInt(int? value)
        {
            if (value.HasValue)
            {
                return SetString(value.Value.ToString());
            }
            else
            {
                return null;
            }
        }
    }
}