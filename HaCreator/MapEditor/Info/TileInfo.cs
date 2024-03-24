/* Copyright (C) 2015 haha01haha01

* This Source Code Form is subject to the terms of the Mozilla Public
* License, v. 2.0. If a copy of the MPL was not distributed with this
* file, You can obtain one at http://mozilla.org/MPL/2.0/. */

using HaCreator.MapEditor.Instance;
using HaCreator.MapEditor.Instance.Shapes;
using HaCreator.Wz;
using SunLibrary.SunFileLib.Properties;
using SunLibrary.SunFileLib.Structure;
using System.Collections.Generic;
using System.Drawing;
using XNA = Microsoft.Xna.Framework;

namespace HaCreator.MapEditor.Info
{
    public class TileInfo : MapleDrawableInfo
    {
        private string _tileSet;
        private string _type;
        private string _no;
        private int _mag;
        private int _z;
        private List<XNA.Point> footholdOffsets = new List<XNA.Point>();

        public TileInfo(Bitmap image, System.Drawing.Point origin, string tileSet, string type, string no, int mag, int z, SunObject parentObject)
            : base(image, origin, parentObject)
        {
            this._tileSet = tileSet;
            this._type = type;
            this._no = no;
            this._mag = mag;
            this._z = z;
        }

        public static TileInfo Get(string tileSet, string type, string no)
        {
            int? mag = InfoTool.GetOptionalInt(Program.InfoManager.TileSets[tileSet]["info"]["mag"]);
            return Get(tileSet, type, no, mag);
        }

        public static TileInfo GetWithDefaultNo(string tileSet, string type, string no, string defaultNo)
        {
            int? mag = InfoTool.GetOptionalInt(Program.InfoManager.TileSets[tileSet]["info"]["mag"]);
            SunProperty prop = Program.InfoManager.TileSets[tileSet][type];
            SunProperty tileInfoProp = prop[no];
            if (tileInfoProp == null)
            {
                tileInfoProp = prop[defaultNo];
            }
            if (tileInfoProp.SETag == null)
                tileInfoProp.SETag = TileInfo.Load((SunCanvasProperty)tileInfoProp, tileSet, type, no, mag);
            return (TileInfo)tileInfoProp.SETag;
        }

        // Optimized version, for cases where you already know the mag (e.g. mass loading tiles of the same tileSet)
        public static TileInfo Get(string tileSet, string type, string no, int? mag)
        {
            SunProperty tileInfoProp = Program.InfoManager.TileSets[tileSet][type][no];
            if (tileInfoProp.SETag == null)
                tileInfoProp.SETag = TileInfo.Load((SunCanvasProperty)tileInfoProp, tileSet, type, no, mag);
            return (TileInfo)tileInfoProp.SETag;
        }

        private static TileInfo Load(SunCanvasProperty parentObject, string tileSet, string type, string no, int? mag)
        {
            SunProperty zProp = parentObject["z"];
            int z = zProp == null ? 0 : InfoTool.GetInt(zProp);
            TileInfo result = new TileInfo(parentObject.PNG.GetPNG(false), SunInfoTools.VectorToSystemPoint((SunVectorProperty)parentObject["origin"]), tileSet, type, no, mag.HasValue ? mag.Value : 1, z, parentObject);
            SunConvexProperty footholds = (SunConvexProperty)parentObject["foothold"];
            if (footholds != null)
                foreach (SunVectorProperty foothold in footholds.SunProperties)
                    result.footholdOffsets.Add(SunInfoTools.VectorToXNAPoint(foothold));
            if (UserSettings.FixFootholdMispositions)
            {
                FixFootholdMispositions(result);
            }
            return result;
        }

        /* The sole reason behind this function's existence is that Nexon's designers are a bunch of incompetent goons.

         * In a nutshell, some tiles (mostly old ones) have innate footholds that do not overlap when snapping them to each other, causing a weird foothold structure.
         * I do not know how Nexon's editor overcame this; I am assuming they manually connected the footholds to sort that out. However, since HaCreator only allows automatic
         * connection of footholds, we need to sort these cases out preemptively here.
        */

        private static void FixFootholdMispositions(TileInfo result)
        {
            switch (result.Type)
            {
                case "wallL":
                    MoveFootholdY(result, true, false, 60);
                    MoveFootholdY(result, false, true, 60);
                    break;

                case "wallR":
                    MoveFootholdY(result, true, true, 60);
                    MoveFootholdY(result, false, false, 60);
                    break;

                case "platTop":
                    MoveFootholdX(result, true, true, 90);
                    MoveFootholdX(result, false, false, 90);
                    break;

                case "slopeLU":
                    MoveFootholdX(result, true, false, -90);
                    MoveFootholdX(result, false, true, -90);
                    break;

                case "slopeRU":
                    MoveFootholdX(result, true, true, 90);
                    MoveFootholdX(result, false, false, 90);
                    break;

                case "floatTop":
                    MoveFootholdY(result, true, false, 0);
                    MoveFootholdY(result, false, false, 0);
                    break;
            }
        }

        private static void MoveFootholdY(TileInfo result, bool first, bool top, int height)
        {
            if (result.footholdOffsets.Count < 1)
                return;
            int idx = first ? 0 : result.footholdOffsets.Count - 1;
            int y = top ? 0 : (height * result.Mag);
            if (result.footholdOffsets[idx].Y != y)
            {
                result.footholdOffsets[idx] = new XNA.Point(result.footholdOffsets[idx].X, y);
            }
        }

        private static void MoveFootholdX(TileInfo result, bool first, bool left, int width)
        {
            if (result.footholdOffsets.Count < 1)
                return;
            int idx = first ? 0 : result.footholdOffsets.Count - 1;
            int x = left ? 0 : (width * result.Mag);
            if (result.footholdOffsets[idx].X != x)
            {
                result.footholdOffsets[idx] = new XNA.Point(x, result.footholdOffsets[idx].Y);
            }
        }

        public void ParseOffsets(TileInstance instance, Board board, int x, int y)
        {
            List<FootholdAnchor> anchors = new List<FootholdAnchor>();
            foreach (XNA.Point foothold in footholdOffsets)
            {
                FootholdAnchor anchor = new FootholdAnchor(board, x + foothold.X, y + foothold.Y, instance.LayerNumber, instance.PlatformNumber, true);
                anchors.Add(anchor);
                board.BoardItems.FHAnchors.Add(anchor);
                instance.BindItem(anchor, foothold);
            }
            for (int i = 0; i < anchors.Count - 1; i++)
            {
                FootholdLine fh = new FootholdLine(board, anchors[i], anchors[i + 1]);
                board.BoardItems.FootholdLines.Add(fh);
            }
        }

        public override BoardItem CreateInstance(Layer layer, Board board, int x, int y, int z, bool flip)
        {
            TileInstance instance = new TileInstance(this, layer, board, x, y, z, layer.zMDefault);
            ParseOffsets(instance, board, x, y);
            return instance;
        }

        public BoardItem CreateInstance(Layer layer, Board board, int x, int y, int z, int zM, bool flip, bool parseOffsets)
        {
            TileInstance instance = new TileInstance(this, layer, board, x, y, z, zM);
            if (parseOffsets) ParseOffsets(instance, board, x, y);
            return instance;
        }

        public string TileSet
        {
            get
            {
                return _tileSet;
            }
            set
            {
                this._tileSet = value;
            }
        }

        public string Type
        {
            get
            {
                return _type;
            }
            set
            {
                this._type = value;
            }
        }

        public string No
        {
            get
            {
                return _no;
            }
            set
            {
                this._no = value;
            }
        }

        public int Mag
        {
            get { return _mag; }
            set { _mag = value; }
        }

        public List<XNA.Point> FootholdOffsets
        {
            get
            {
                return footholdOffsets;
            }
        }

        public int Z
        { get { return _z; } set { _z = value; } }
    }
}