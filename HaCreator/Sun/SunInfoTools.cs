/* Copyright (C) 2015 haha01haha01

* This Source Code Form is subject to the terms of the Mozilla Public
* License, v. 2.0. If a copy of the MPL was not distributed with this
* file, You can obtain one at http://mozilla.org/MPL/2.0/. */

using HaCreator.MapEditor.Info;
using SunLibrary.SunFileLib.Properties;
using SunLibrary.SunFileLib.Structure;
using System;
using System.Drawing;
using XNA = Microsoft.Xna.Framework;

namespace HaCreator.Wz
{
    public static class SunInfoTools
    {
        public static System.Drawing.Point VectorToSystemPoint(SunVectorProperty source)
        {
            return new System.Drawing.Point(source.X.Value, source.Y.Value);
        }

        public static Microsoft.Xna.Framework.Point VectorToXNAPoint(SunVectorProperty source)
        {
            return new Microsoft.Xna.Framework.Point(source.X.Value, source.Y.Value);
        }

        public static SunVectorProperty PointToVector(string name, System.Drawing.Point source)
        {
            return new SunVectorProperty(name, new SunIntProperty("X", source.X), new SunIntProperty("Y", source.Y));
        }

        public static SunVectorProperty PointToVector(string name, Microsoft.Xna.Framework.Point source)
        {
            return new SunVectorProperty(name, new SunIntProperty("X", source.X), new SunIntProperty("Y", source.Y));
        }

        public static string AddLeadingZeros(string source, int maxLength)
        {
            while (source.Length < maxLength)
                source = "0" + source;
            return source;
        }

        public static string RemoveLeadingZeros(string source)
        {
            int firstNonZeroIndex = 0;
            for (int i = 0; i < source.Length; i++)
            {
                if (source[i] != (char)0x30) //char at index i is not 0
                {
                    firstNonZeroIndex = i;
                    break;
                }
                else if (i == source.Length - 1) //all chars are 0, return 0
                    return "0";
            }
            return source.Substring(firstNonZeroIndex);
        }

        public static string GetMobNameById(string id)
        {
            id = RemoveLeadingZeros(id);
            SunObject obj = Program.WzManager.String["Mob.img"][id];
            if (obj == null)
            {
                return "";
            }
            SunStringProperty mobName = (SunStringProperty)obj["name"];
            if (mobName == null)
            {
                return "";
            }
            return mobName.Value;
        }

        public static string GetNpcNameById(string id)
        {
            id = RemoveLeadingZeros(id);
            SunObject obj = Program.WzManager.String["Npc.img"][id];
            if (obj == null)
            {
                return "";
            }
            SunStringProperty npcName = (SunStringProperty)obj["name"];
            if (npcName == null)
            {
                return "";
            }
            return npcName.Value;
        }

        public static SunSubProperty GetMapStringProp(string id)
        {
            id = RemoveLeadingZeros(id);
            SunImage mapNameParent = (SunImage)Program.WzManager.String["Map.img"];
            foreach (SunSubProperty mapNameCategory in mapNameParent.SunProperties)
            {
                SunSubProperty mapNameDirectory = (SunSubProperty)mapNameCategory[id];
                if (mapNameDirectory != null)
                {
                    return mapNameDirectory;
                }
            }
            return null;
        }

        public static string GetMapName(SunSubProperty mapProp)
        {
            if (mapProp == null)
            {
                return "";
            }
            SunStringProperty mapName = (SunStringProperty)mapProp["mapName"];
            if (mapName == null)
            {
                return "";
            }
            return mapName.Value;
        }

        public static string GetMapStreetName(SunSubProperty mapProp)
        {
            if (mapProp == null)
            {
                return "";
            }
            SunStringProperty streetName = (SunStringProperty)mapProp["streetName"];
            if (streetName == null)
            {
                return "";
            }
            return streetName.Value;
        }

        public static string GetMapCategoryName(SunSubProperty mapProp)
        {
            if (mapProp == null)
            {
                return "";
            }
            return mapProp.Parent.Name;
        }

        public static SunObject GetObjectByRelativePath(SunObject currentObject, string path)
        {
            foreach (string directive in path.Split("/".ToCharArray()))
            {
                if (directive == "..") currentObject = currentObject.Parent;
                else if (currentObject is SunProperty)
                    currentObject = ((SunProperty)currentObject)[directive];
                else if (currentObject is SunImage)
                    currentObject = ((SunImage)currentObject)[directive];
                else if (currentObject is SunDirectory)
                    currentObject = ((SunDirectory)currentObject)[directive];
                else throw new Exception("invalid type");
            }
            return currentObject;
        }

        public static SunObject ResolveLinkProperty(SunLinkProperty link)
        {
            return GetObjectByRelativePath(link.Parent, link.Value);
        }

        public static string RemoveExtension(string source)
        {
            if (source.Substring(source.Length - 4) == ".img")
                return source.Substring(0, source.Length - 4);
            return source;
        }

        public static SunProperty GetRealProperty(SunProperty prop)
        {
            if (prop is SunLinkProperty) return (SunProperty)ResolveLinkProperty((SunLinkProperty)prop);
            else return prop;
        }

        public static SunCanvasProperty GetMobImage(SunImage parentImage)
        {
            SunSubProperty standParent = (SunSubProperty)parentImage["stand"];
            if (standParent != null)
            {
                SunCanvasProperty frame1 = (SunCanvasProperty)GetRealProperty(standParent["0"]);
                if (frame1 != null) return frame1;
            }
            SunSubProperty flyParent = (SunSubProperty)parentImage["fly"];
            if (flyParent != null)
            {
                SunCanvasProperty frame1 = (SunCanvasProperty)GetRealProperty(flyParent["0"]);
                if (frame1 != null) return frame1;
            }
            return null;
        }

        public static SunCanvasProperty GetNpcImage(SunImage parentImage)
        {
            SunSubProperty standParent = (SunSubProperty)parentImage["stand"];
            if (standParent != null)
            {
                SunCanvasProperty frame1 = (SunCanvasProperty)GetRealProperty(standParent["0"]);
                if (frame1 != null) return frame1;
            }
            return null;
        }

        public static SunCanvasProperty GetReactorImage(SunImage parentImage)
        {
            SunSubProperty action0 = (SunSubProperty)parentImage["0"];
            if (action0 != null)
            {
                SunCanvasProperty frame1 = (SunCanvasProperty)GetRealProperty(action0["0"]);
                if (frame1 != null) return frame1;
            }
            return null;
        }

        public static MobInfo GetMobInfoById(string id)
        {
            id = AddLeadingZeros(id, 7);
            return MobInfo.Get(id);
        }

        public static NpcInfo GetNpcInfoById(string id)
        {
            id = AddLeadingZeros(id, 7);
            return NpcInfo.Get(id);
        }

        public static Color XNAToDrawingColor(XNA.Color c)
        {
            return Color.FromArgb(c.A, c.R, c.G, c.B);
        }
    }
}