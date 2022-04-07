using System;
using System.Collections.Generic;
using System.IO;

namespace SunFileManager.SunFileLib.Util
{
    public static class ErrorLogger
    {
        private static List<Error> errorList = new List<Error>();
        public static void Log(ErrorLevel level, string message)
        {
            errorList.Add(new Error(level, message));
        }

        public static bool ErrorsPresent()
        {
            return errorList.Count > 0;
        }

        public static void ClearErrors()
        {
            errorList.Clear();
        }

        public static void SaveToFile(string filename)
        {
            using (StreamWriter sw = new StreamWriter(File.Open(filename, FileMode.Append, FileAccess.Write, FileShare.Read)))
            {
                sw.WriteLine("Starting error log on " + DateTime.Today.ToShortDateString());
                foreach (Error e in errorList)
                {
                    sw.WriteLine(e.level.ToString() + ":" + e.message);
                }
                sw.WriteLine();
            }
        }
    }

    public class Error
    {
        internal ErrorLevel level;
        internal string message;

        internal Error(ErrorLevel level, string message)
        {
            this.level = level;
            this.message = message;
        }
    }

    public enum ErrorLevel
    {
        MissingFeature,
        IncorrectStructure,
        Critical,
        Crash
    }
}