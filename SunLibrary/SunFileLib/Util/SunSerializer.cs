/*  MapleLib - A general-purpose MapleStory library
 * Copyright (C) 2009, 2010, 2015 Snow and haha01haha01

 * This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

 * This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

 * You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.*/

using SunLibrary.SunFileLib.Properties;

//using static System.Net.Mime.MediaTypeNames;
using SunLibrary.SunFileLib.Structure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using File = System.IO.File;

namespace SunLibrary.SunFileLib.Util
{
    public abstract class ProgressingSunSerializer
    {
        protected int total = 0;
        protected int curr = 0;
        public int Total
        { get { return total; } }
        public int Current
        { get { return curr; } }

        protected static void createDirSafe(ref string path)
        {
            if (path.Substring(path.Length - 1, 1) == @"\")
                path = path.Substring(0, path.Length - 1);

            string basePath = path;
            int curridx = 0;
            while (Directory.Exists(path) || System.IO.File.Exists(path))
            {
                curridx++;
                path = basePath + curridx;
            }
            Directory.CreateDirectory(path);
        }

        private static string regexSearch = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
        private static Regex regex_invalidPath = new Regex(string.Format("[{0}]", Regex.Escape(regexSearch)));

        /// <summary>
        /// Escapes invalid file name and paths (if nexon uses any illegal character that causes issue during saving)
        /// </summary>
        /// <param name="path"></param>
        public static string EscapeInvalidFilePathNames(string path)
        {
            return regex_invalidPath.Replace(path, "");
        }
    }

    public abstract class SunXmlSerializer : ProgressingSunSerializer
    {
        protected string indent;
        protected string lineBreak;
        public static NumberFormatInfo formattingInfo;
        protected bool ExportBase64Data = false;

        protected static char[] amp = "&amp;".ToCharArray();
        protected static char[] lt = "&lt;".ToCharArray();
        protected static char[] gt = "&gt;".ToCharArray();
        protected static char[] apos = "&apos;".ToCharArray();
        protected static char[] quot = "&quot;".ToCharArray();

        static SunXmlSerializer()
        {
            formattingInfo = new NumberFormatInfo();
            formattingInfo.NumberDecimalSeparator = ".";
            formattingInfo.NumberGroupSeparator = ",";
        }

        public SunXmlSerializer(int indentation, LineBreak lineBreakType)
        {
            switch (lineBreakType)
            {
                case LineBreak.None:
                    lineBreak = "";
                    break;

                case LineBreak.Windows:
                    lineBreak = "\r\n";
                    break;

                case LineBreak.Unix:
                    lineBreak = "\n";
                    break;
            }
            char[] indentArray = new char[indentation];
            for (int i = 0; i < indentation; i++)
                indentArray[i] = (char)0x20;
            indent = new string(indentArray);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="tw"></param>
        /// <param name="depth"></param>
        /// <param name="prop"></param>
        /// <param name="exportFilePath"></param>
        protected void WritePropertyToXML(TextWriter tw, string depth, SunProperty prop, string exportFilePath)
        {
            if (prop is SunCanvasProperty)
            {
                SunCanvasProperty property3 = (SunCanvasProperty)prop;
                if (ExportBase64Data)
                {
                    MemoryStream stream = new MemoryStream();
                    property3.PNG.GetPNG(false).Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                    byte[] pngbytes = stream.ToArray();
                    stream.Close();
                    tw.Write(string.Concat(new object[] { depth, "<canvas name=\"", XmlUtil.SanitizeText(property3.Name), "\" width=\"", property3.PNG.Width, "\" height=\"", property3.PNG.Height, "\" basedata=\"", Convert.ToBase64String(pngbytes), "\">" }) + lineBreak);
                }
                else
                    tw.Write(string.Concat(new object[] { depth, "<canvas name=\"", XmlUtil.SanitizeText(property3.Name), "\" width=\"", property3.PNG.Width, "\" height=\"", property3.PNG.Height, "\">" }) + lineBreak);
                string newDepth = depth + indent;
                foreach (SunProperty property in property3.SunProperties)
                {
                    WritePropertyToXML(tw, newDepth, property, exportFilePath);
                }
                tw.Write(depth + "</canvas>" + lineBreak);
            }
            else if (prop is SunIntProperty)
            {
                SunIntProperty property4 = (SunIntProperty)prop;
                tw.Write(string.Concat(new object[] { depth, "<int name=\"", XmlUtil.SanitizeText(property4.Name), "\" value=\"", property4.Value, "\"/>" }) + lineBreak);
            }
            else if (prop is SunDoubleProperty)
            {
                SunDoubleProperty property5 = (SunDoubleProperty)prop;
                tw.Write(string.Concat(new object[] { depth, "<double name=\"", XmlUtil.SanitizeText(property5.Name), "\" value=\"", property5.Value, "\"/>" }) + lineBreak);
            }
            else if (prop is SunNullProperty)
            {
                SunNullProperty property6 = (SunNullProperty)prop;
                tw.Write(depth + "<null name=\"" + XmlUtil.SanitizeText(property6.Name) + "\"/>" + lineBreak);
            }
            else if (prop is SunSoundProperty)
            {
                SunSoundProperty property7 = (SunSoundProperty)prop;
                if (ExportBase64Data)
                    tw.Write(string.Concat(new object[] { depth, "<sound name=\"", XmlUtil.SanitizeText(property7.Name), "\" length=\"", property7.Length.ToString(), "\" basehead=\"", Convert.ToBase64String(property7.Header), "\" basedata=\"", Convert.ToBase64String(property7.GetBytes(false)), "\"/>" }) + lineBreak);
                else
                    tw.Write(depth + "<sound name=\"" + XmlUtil.SanitizeText(property7.Name) + "\"/>" + lineBreak);
            }
            else if (prop is SunStringProperty)
            {
                SunStringProperty property8 = (SunStringProperty)prop;
                string str = XmlUtil.SanitizeText(property8.Value);
                tw.Write(depth + "<string name=\"" + XmlUtil.SanitizeText(property8.Name) + "\" value=\"" + str + "\"/>" + lineBreak);
            }
            else if (prop is SunSubProperty)
            {
                SunSubProperty property9 = (SunSubProperty)prop;
                tw.Write(depth + "<imgdir name=\"" + XmlUtil.SanitizeText(property9.Name) + "\">" + lineBreak);
                string newDepth = depth + indent;
                foreach (SunProperty property in property9.SunProperties)
                {
                    WritePropertyToXML(tw, newDepth, property, exportFilePath);
                }
                tw.Write(depth + "</imgdir>" + lineBreak);
            }
            else if (prop is SunShortProperty)
            {
                SunShortProperty property10 = (SunShortProperty)prop;
                tw.Write(string.Concat(new object[] { depth, "<short name=\"", XmlUtil.SanitizeText(property10.Name), "\" value=\"", property10.Value, "\"/>" }) + lineBreak);
            }
            else if (prop is SunLongProperty)
            {
                SunLongProperty long_prop = (SunLongProperty)prop;
                tw.Write(string.Concat(new object[] { depth, "<long name=\"", XmlUtil.SanitizeText(long_prop.Name), "\" value=\"", long_prop.Value, "\"/>" }) + lineBreak);
            }
            else if (prop is SunLinkProperty)
            {
                SunLinkProperty property11 = (SunLinkProperty)prop;
                tw.Write(depth + "<uol name=\"" + property11.Name + "\" value=\"" + XmlUtil.SanitizeText(property11.Value) + "\"/>" + lineBreak);
            }
            else if (prop is SunVectorProperty)
            {
                SunVectorProperty property12 = (SunVectorProperty)prop;
                tw.Write(string.Concat(new object[] { depth, "<vector name=\"", XmlUtil.SanitizeText(property12.Name), "\" x=\"", property12.X.Value, "\" y=\"", property12.Y.Value, "\"/>" }) + lineBreak);
            }
            else if (prop is SunFloatProperty)
            {
                SunFloatProperty property13 = (SunFloatProperty)prop;
                string str2 = Convert.ToString(property13.Value, formattingInfo);
                if (!str2.Contains("."))
                    str2 = str2 + ".0";
                tw.Write(depth + "<float name=\"" + XmlUtil.SanitizeText(property13.Name) + "\" value=\"" + str2 + "\"/>" + lineBreak);
            }
            else if (prop is SunConvexProperty)
            {
                tw.Write(depth + "<extended name=\"" + XmlUtil.SanitizeText(prop.Name) + "\">" + lineBreak);

                SunConvexProperty property14 = (SunConvexProperty)prop;
                string newDepth = depth + indent;
                foreach (SunProperty property in property14.SunProperties)
                {
                    WritePropertyToXML(tw, newDepth, property, exportFilePath);
                }
                tw.Write(depth + "</extended>" + lineBreak);
            }
        }
    }

    public interface ISunFileSerializer
    {
        void SerializeFile(SunFile file, string path);
    }

    public interface ISunDirectorySerializer : ISunFileSerializer
    {
        void SerializeDirectory(SunDirectory dir, string path);
    }

    public interface ISunImageSerializer : ISunDirectorySerializer
    {
        void SerializeImage(SunImage img, string path);
    }

    public interface ISunObjectSerializer
    {
        void SerializeObject(SunObject file, string path);
    }

    public enum LineBreak
    {
        None,
        Windows,
        Unix
    }

    public class NoBase64DataException : System.Exception
    {
        public NoBase64DataException() : base()
        {
        }

        public NoBase64DataException(string message) : base(message)
        {
        }

        public NoBase64DataException(string message, System.Exception inner) : base(message, inner)
        {
        }

        protected NoBase64DataException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context)
        { }
    }

    public class SunImgSerializer : ProgressingSunSerializer, ISunImageSerializer
    {
        public byte[] SerializeImage(SunImage img)
        {
            total = 1; curr = 0;

            using (MemoryStream stream = new MemoryStream())
            {
                using (SunBinaryWriter sunWriter = new SunBinaryWriter(stream))
                {
                    img.SaveImage(sunWriter);
                    byte[] result = stream.ToArray();

                    return result;
                }
            }
        }

        public void SerializeImage(SunImage img, string outPath)
        {
            total = 1; curr = 0;
            if (Path.GetExtension(outPath) != ".img")
            {
                outPath += ".img";
            }

            using (FileStream stream = File.Create(outPath))
            {
                using (SunBinaryWriter sunWriter = new SunBinaryWriter(stream))
                {
                    img.SaveImage(sunWriter);
                }
            }
        }

        public void SerializeDirectory(SunDirectory dir, string outPath)
        {
            total = dir.CountImages();
            curr = 0;

            if (!Directory.Exists(outPath))
                SunXmlSerializer.createDirSafe(ref outPath);

            if (outPath.Substring(outPath.Length - 1, 1) != @"\")
            {
                outPath += @"\";
            }

            foreach (SunDirectory subdir in dir.SubDirectories)
            {
                SerializeDirectory(subdir, outPath + subdir.Name + @"\");
            }
            foreach (SunImage img in dir.SunImages)
            {
                SerializeImage(img, outPath + img.Name);
            }
        }

        public void SerializeFile(SunFile f, string outPath)
        {
            SerializeDirectory(f.SunDirectory, outPath);
        }
    }

    public class SunImgDeserializer : ProgressingSunSerializer
    {
        private bool freeResources;

        public SunImgDeserializer(bool freeResources)
            : base()
        {
            this.freeResources = freeResources;
        }

        public SunImage SunImageFromIMGBytes(byte[] bytes, string name, bool freeResources)
        {
            MemoryStream stream = new MemoryStream(bytes);
            SunBinaryReader sunReader = new SunBinaryReader(stream);
            SunImage img = new SunImage(name, sunReader);
            img.Size = bytes.Length;
            img.Checksum = 0;
            foreach (byte b in bytes) img.Checksum += b;
            img.Offset = 0;
            if (freeResources)
            {
                img.ParseImage(true);
                img.Changed = true;
                sunReader.Close();
            }
            return img;
        }

        /// <summary>
        /// Parse a Sun image from .img file/
        /// </summary>
        /// <param name="inPath"></param>
        /// <param name="iv"></param>
        /// <param name="name"></param>
        /// <param name="successfullyParsedImage"></param>
        /// <returns></returns>
        public SunImage SunImageFromIMGFile(string inPath, string name, out bool successfullyParsedImage)
        {
            FileStream stream = File.OpenRead(inPath);
            SunBinaryReader sunReader = new SunBinaryReader(stream);

            SunImage img = new SunImage(name, sunReader);
            img.Size = (int)stream.Length;
            img.Checksum = 0;
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, (int)stream.Length);
            stream.Position = 0;
            foreach (byte b in bytes) img.Checksum += b;
            img.Offset = 0;
            if (freeResources)
            {
                successfullyParsedImage = img.ParseImage(true);
                img.Changed = true;
                sunReader.Close();
            }
            else
            {
                successfullyParsedImage = true;
            }
            return img;
        }
    }

    public class SunPngMp3Serializer : ProgressingSunSerializer, ISunImageSerializer, ISunObjectSerializer
    {
        //List<SunImage> imagesToUnparse = new List<SunImage>();
        private string outPath;

        public void SerializeObject(SunObject obj, string outPath)
        {
            //imagesToUnparse.Clear();
            total = 0; curr = 0;
            this.outPath = outPath;
            if (!Directory.Exists(outPath))
            {
                SunXmlSerializer.createDirSafe(ref outPath);
            }

            if (outPath.Substring(outPath.Length - 1, 1) != @"\")
                outPath += @"\";

            total = CalculateTotal(obj);
            ExportRecursion(obj, outPath);
            /*foreach (SunImage img in imagesToUnparse)
                img.UnparseImage();
            imagesToUnparse.Clear();*/
        }

        public void SerializeFile(SunFile file, string path)
        {
            SerializeObject(file, path);
        }

        public void SerializeDirectory(SunDirectory file, string path)
        {
            SerializeObject(file, path);
        }

        public void SerializeImage(SunImage file, string path)
        {
            SerializeObject(file, path);
        }

        private int CalculateTotal(SunObject currObj)
        {
            int result = 0;
            if (currObj is SunFile)
                result += ((SunFile)currObj).SunDirectory.CountImages();
            else if (currObj is SunDirectory)
                result += ((SunDirectory)currObj).CountImages();
            return result;
        }

        private void ExportRecursion(SunObject currObj, string outPath)
        {
            if (currObj is SunFile)
                ExportRecursion(((SunFile)currObj).SunDirectory, outPath);
            else if (currObj is SunDirectory)
            {
                outPath += ProgressingSunSerializer.EscapeInvalidFilePathNames(currObj.Name) + @"\";
                if (!Directory.Exists(outPath))
                    Directory.CreateDirectory(outPath);
                foreach (SunDirectory subdir in ((SunDirectory)currObj).SubDirectories)
                {
                    ExportRecursion(subdir, outPath + subdir.Name + @"\");
                }
                foreach (SunImage subimg in ((SunDirectory)currObj).SunImages)
                {
                    ExportRecursion(subimg, outPath + subimg.Name + @"\");
                }
            }
            else if (currObj is SunCanvasProperty)
            {
                Bitmap bmp = ((SunCanvasProperty)currObj).PNG.GetPNG(false);

                string path = outPath + ProgressingSunSerializer.EscapeInvalidFilePathNames(currObj.Name) + ".png";

                bmp.Save(path, ImageFormat.Png);
                //curr++;
            }
            else if (currObj is SunSoundProperty)
            {
                string path = outPath + ProgressingSunSerializer.EscapeInvalidFilePathNames(currObj.Name) + ".mp3";
                ((SunSoundProperty)currObj).SaveToFile(path);
            }
            else if (currObj is SunImage)
            {
                outPath += ProgressingSunSerializer.EscapeInvalidFilePathNames(currObj.Name) + @"\";
                if (!Directory.Exists(outPath))
                    Directory.CreateDirectory(outPath);

                bool parse = ((SunImage)currObj).Parsed || ((SunImage)currObj).Changed;
                if (!parse)
                    ((SunImage)currObj).ParseImage();
                foreach (SunProperty subprop in ((IPropertyContainer)currObj).SunProperties)
                {
                    ExportRecursion(subprop, outPath);
                }
                if (!parse)
                    ((SunImage)currObj).UnparseImage();
                curr++;
            }
            else if (currObj is IPropertyContainer)
            {
                outPath += currObj.Name + ".";
                foreach (SunProperty subprop in ((IPropertyContainer)currObj).SunProperties)
                    ExportRecursion(subprop, outPath);
            }
            else if (currObj is SunLinkProperty)
                ExportRecursion(((SunLinkProperty)currObj).LinkValue, outPath);
        }
    }

    public class SunClassicXmlSerializer : SunXmlSerializer, ISunImageSerializer
    {
        public SunClassicXmlSerializer(int indentation, LineBreak lineBreakType, bool exportbase64)
            : base(indentation, lineBreakType)
        { ExportBase64Data = exportbase64; }

        private void exportXmlInternal(SunImage img, string path)
        {
            bool parsed = img.Parsed || img.Changed;
            if (!parsed)
                img.ParseImage();
            curr++;

            using (TextWriter tw = new StreamWriter(File.Create(path)))
            {
                tw.Write("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>" + lineBreak);
                tw.Write("<imgdir name=\"" + XmlUtil.SanitizeText(img.Name) + "\">" + lineBreak);
                foreach (SunProperty property in img.SunProperties)
                {
                    WritePropertyToXML(tw, indent, property, path);
                }
                tw.Write("</imgdir>" + lineBreak);
            }

            if (!parsed)
                img.UnparseImage();
        }

        private void exportDirXmlInternal(SunDirectory dir, string path)
        {
            if (!Directory.Exists(path))
                createDirSafe(ref path);

            if (path.Substring(path.Length - 1) != @"\")
                path += @"\";

            foreach (SunDirectory subdir in dir.SubDirectories)
            {
                exportDirXmlInternal(subdir, path + ProgressingSunSerializer.EscapeInvalidFilePathNames(subdir.Name) + @"\");
            }
            foreach (SunImage subimg in dir.SunImages)
            {
                exportXmlInternal(subimg, path + ProgressingSunSerializer.EscapeInvalidFilePathNames(subimg.Name) + ".xml");
            }
        }

        public void SerializeImage(SunImage img, string path)
        {
            total = 1; curr = 0;
            if (Path.GetExtension(path) != ".xml") path += ".xml";
            exportXmlInternal(img, path);
        }

        public void SerializeDirectory(SunDirectory dir, string path)
        {
            total = dir.CountImages(); curr = 0;
            exportDirXmlInternal(dir, path);
        }

        public void SerializeFile(SunFile file, string path)
        {
            SerializeDirectory(file.SunDirectory, path);
        }
    }

    public class SunNewXmlSerializer : SunXmlSerializer
    {
        public SunNewXmlSerializer(int indentation, LineBreak lineBreakType)
            : base(indentation, lineBreakType)
        { }

        internal void DumpImageToXML(TextWriter tw, string depth, SunImage img, string exportFilePath)
        {
            bool parsed = img.Parsed || img.Changed;
            if (!parsed) img.ParseImage();
            curr++;
            tw.Write(depth + "<sunimg name=\"" + XmlUtil.SanitizeText(img.Name) + "\">" + lineBreak);
            string newDepth = depth + indent;
            foreach (SunProperty property in img.SunProperties)
            {
                WritePropertyToXML(tw, newDepth, property, exportFilePath);
            }
            tw.Write(depth + "</sunimg>");
            if (!parsed)
                img.UnparseImage();
        }

        internal void DumpDirectoryToXML(TextWriter tw, string depth, SunDirectory dir, string exportFilePath)
        {
            tw.Write(depth + "<sundir name=\"" + XmlUtil.SanitizeText(dir.Name) + "\">" + lineBreak);
            foreach (SunDirectory subdir in dir.SubDirectories)
                DumpDirectoryToXML(tw, depth + indent, subdir, exportFilePath);
            foreach (SunImage img in dir.SunImages)
            {
                DumpImageToXML(tw, depth + indent, img, exportFilePath);
            }
            tw.Write(depth + "</sundir>" + lineBreak);
        }

        /// <summary>
        /// Export combined XML
        /// </summary>
        /// <param name="objects"></param>
        /// <param name="exportFilePath"></param>
        public void ExportCombinedXml(List<SunObject> objects, string exportFilePath)
        {
            total = 1; curr = 0;

            if (Path.GetExtension(exportFilePath) != ".xml")
                exportFilePath += ".xml";
            foreach (SunObject obj in objects)
            {
                if (obj is SunImage)
                    total++;
                else if (obj is SunDirectory)
                    total += ((SunDirectory)obj).CountImages();
            }

            ExportBase64Data = true;
            TextWriter tw = new StreamWriter(exportFilePath);
            tw.Write("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>" + lineBreak);
            tw.Write("<xmldump>" + lineBreak);
            foreach (SunObject obj in objects)
            {
                if (obj is SunDirectory)
                {
                    DumpDirectoryToXML(tw, indent, (SunDirectory)obj, exportFilePath);
                }
                else if (obj is SunImage)
                {
                    DumpImageToXML(tw, indent, (SunImage)obj, exportFilePath);
                }
                else if (obj is SunProperty)
                {
                    WritePropertyToXML(tw, indent, (SunProperty)obj, exportFilePath);
                }
            }
            tw.Write("</xmldump>" + lineBreak);
            tw.Close();
        }
    }

    public class SunXmlDeserializer : ProgressingSunSerializer
    {
        public static NumberFormatInfo formattingInfo;

        private bool useMemorySaving;
        private byte[] iv;
        private SunImgDeserializer imgDeserializer = new SunImgDeserializer(false);

        public SunXmlDeserializer(bool useMemorySaving, byte[] iv)
            : base()
        {
            this.useMemorySaving = useMemorySaving;
            this.iv = iv;
        }

        #region Public Functions

        public List<SunObject> ParseXML(string path)
        {
            List<SunObject> result = new List<SunObject>();
            XmlDocument doc = new XmlDocument();
            doc.Load(path);
            XmlElement mainElement = (XmlElement)doc.ChildNodes[1];
            curr = 0;
            if (mainElement.Name == "xmldump")
            {
                total = CountImgs(mainElement);
                foreach (XmlElement subelement in mainElement)
                {
                    if (subelement.Name == "sundir")
                        result.Add(ParseXMLSunDir(subelement));
                    else if (subelement.Name == "sunimg")
                        result.Add(ParseXMLsunimg(subelement));
                    else throw new InvalidDataException("unknown XML prop " + subelement.Name);
                }
            }
            else if (mainElement.Name == "imgdir")
            {
                total = 1;
                result.Add(ParseXMLsunimg(mainElement));
                curr++;
            }
            else throw new InvalidDataException("unknown main XML prop " + mainElement.Name);
            return result;
        }

        #endregion Public Functions

        #region Internal Functions

        internal int CountImgs(XmlElement element)
        {
            int result = 0;
            foreach (XmlElement subelement in element)
            {
                if (subelement.Name == "sunimg") result++;
                else if (subelement.Name == "sundir") result += CountImgs(subelement);
            }
            return result;
        }

        internal SunDirectory ParseXMLSunDir(XmlElement dirElement)
        {
            SunDirectory result = new SunDirectory(dirElement.GetAttribute("name"));
            foreach (XmlElement subelement in dirElement)
            {
                if (subelement.Name == "sundir")
                    result.AddDirectory(ParseXMLSunDir(subelement));
                else if (subelement.Name == "sunimg")
                    result.AddImage(ParseXMLsunimg(subelement));
                else throw new InvalidDataException("unknown XML prop " + subelement.Name);
            }
            return result;
        }

        internal SunImage ParseXMLsunimg(XmlElement imgElement)
        {
            string name = imgElement.GetAttribute("name");
            SunImage result = new SunImage(name);
            foreach (XmlElement subelement in imgElement)
                result.SunProperties.Add(ParsePropertyFromXMLElement(subelement));
            result.Changed = true;
            if (this.useMemorySaving)
            {
                string path = Path.GetTempFileName();
                SunBinaryWriter sunWriter = new SunBinaryWriter(File.Create(path));
                result.SaveImage(sunWriter);
                sunWriter.Close();
                result.Dispose();

                bool successfullyParsedImage;
                result = imgDeserializer.SunImageFromIMGFile(path, name, out successfullyParsedImage);
            }
            return result;
        }

        internal SunProperty ParsePropertyFromXMLElement(XmlElement element)
        {
            switch (element.Name)
            {
                case "imgdir":
                    SunSubProperty sub = new SunSubProperty(element.GetAttribute("name"));
                    foreach (XmlElement subelement in element)
                        sub.AddProperty(ParsePropertyFromXMLElement(subelement));
                    return sub;

                case "canvas":
                    SunCanvasProperty canvas = new SunCanvasProperty(element.GetAttribute("name"));
                    if (!element.HasAttribute("basedata")) throw new NoBase64DataException("no base64 data in canvas element with name " + canvas.Name);
                    canvas.PNG = new SunPngProperty();
                    MemoryStream pngstream = new MemoryStream(Convert.FromBase64String(element.GetAttribute("basedata")));
                    canvas.PNG.SetPNG((Bitmap)Image.FromStream(pngstream, true, true));
                    foreach (XmlElement subelement in element)
                        canvas.AddProperty(ParsePropertyFromXMLElement(subelement));
                    return canvas;

                case "int":
                    SunIntProperty compressedInt = new SunIntProperty(element.GetAttribute("name"), int.Parse(element.GetAttribute("value"), formattingInfo));
                    return compressedInt;

                case "double":
                    SunDoubleProperty doubleProp = new SunDoubleProperty(element.GetAttribute("name"), double.Parse(element.GetAttribute("value"), formattingInfo));
                    return doubleProp;

                case "null":
                    SunNullProperty nullProp = new SunNullProperty(element.GetAttribute("name"));
                    return nullProp;

                case "sound":
                    if (!element.HasAttribute("basedata") || !element.HasAttribute("basehead") || !element.HasAttribute("length")) throw new NoBase64DataException("no base64 data in sound element with name " + element.GetAttribute("name"));
                    SunSoundProperty sound = new SunSoundProperty(element.GetAttribute("name"),
                        int.Parse(element.GetAttribute("length")),
                        Convert.FromBase64String(element.GetAttribute("basehead")),
                    Convert.FromBase64String(element.GetAttribute("basedata")));
                    return sound;

                case "string":
                    SunStringProperty stringProp = new SunStringProperty(element.GetAttribute("name"), element.GetAttribute("value"));
                    return stringProp;

                case "short":
                    SunShortProperty shortProp = new SunShortProperty(element.GetAttribute("name"), short.Parse(element.GetAttribute("value"), formattingInfo));
                    return shortProp;

                case "long":
                    SunLongProperty longProp = new SunLongProperty(element.GetAttribute("name"), long.Parse(element.GetAttribute("value"), formattingInfo));
                    return longProp;

                case "uol":
                    SunLinkProperty uol = new SunLinkProperty(element.GetAttribute("name"), element.GetAttribute("value"));
                    return uol;

                case "vector":
                    SunVectorProperty vector = new SunVectorProperty(element.GetAttribute("name"), new SunIntProperty("x", Convert.ToInt32(element.GetAttribute("x"))), new SunIntProperty("y", Convert.ToInt32(element.GetAttribute("y"))));
                    return vector;

                case "float":
                    SunFloatProperty floatProp = new SunFloatProperty(element.GetAttribute("name"), float.Parse(element.GetAttribute("value"), formattingInfo));
                    return floatProp;

                case "extended":
                    SunConvexProperty convex = new SunConvexProperty(element.GetAttribute("name"));
                    foreach (XmlElement subelement in element)
                        convex.AddProperty(ParsePropertyFromXMLElement(subelement));
                    return convex;
            }
            throw new InvalidDataException("unknown XML prop " + element.Name);
        }

        #endregion Internal Functions
    }
}