using SunLibrary.SunFileLib.Properties;
using SunLibrary.SunFileLib.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunLibrary.SunFileLib.Structure.Data
{
    public class MapInfo
    {
        public static MapInfo Default = new MapInfo();
        private SunImage img = null;

        public string bgm = "Bgm00";
        public string editorMapName = "<Untitled>";
        public string mapName = null;
        public bool town = false;
        public bool snow = false;
        public bool rain = false;

        public MapInfo()
        {
        }

        public MapInfo(SunImage image, string name, string categoryName)
        {
            this.img = image;
            this.editorMapName = name;
            SunFile file = image.SunFileParent;

            foreach (SunProperty prop in image["info"].SunProperties)
            {
                switch (prop.Name)
                {
                    case "bgm":
                        bgm = SunTool.GetString(prop);
                        break;

                    case "mapName":
                        mapName = SunTool.GetString(prop);
                        break;

                    case "town":
                        town = SunTool.GetBool(prop);
                        break;

                    case "snow":
                        snow = SunTool.GetBool(prop);
                        break;

                    case "rain":
                        rain = SunTool.GetBool(prop);
                        break;

                    default:
                        ErrorLogger.Log(ErrorLevel.MissingFeature, "Unknown Property: " + prop.Name);
                        break;
                }
            }
        }

        public static Rectangle? GetViewRange(SunImage image)
        {
            Rectangle? result = null;
            if (image["info"]["VRLeft"] != null)
            {
                SunProperty info = image["info"];
                int left = SunTool.GetInt(info["VRLeft"]);
                int right = SunTool.GetInt(info["VRRight"]);
                int top = SunTool.GetInt(info["VRTop"]);
                int bottom = SunTool.GetInt(info["VRBottom"]);
                result = new Rectangle(left, top, right - left, bottom - top);
            }
            return result;
        }

        public void Save(SunImage destination, Rectangle? viewRange)
        {
            SunSubProperty info = new SunSubProperty();
            info["bgm"] = SunTool.SetString(bgm);
            info["mapName"] = SunTool.SetString(mapName);
            info["town"] = SunTool.SetBool(town);
            info["snow"] = SunTool.SetBool(snow);
            info["rain"] = SunTool.SetBool(rain);
        }

        public SunImage Image
        {
            get { return img; }
            set { img = value; }
        }
    }
}