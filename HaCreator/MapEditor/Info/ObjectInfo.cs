/* Copyright (C) 2015 haha01haha01

* This Source Code Form is subject to the terms of the Mozilla Public
* License, v. 2.0. If a copy of the MPL was not distributed with this
* file, You can obtain one at http://mozilla.org/MPL/2.0/. */

using HaCreator.MapEditor.Instance;
using HaCreator.MapEditor.Instance.Shapes;
using HaCreator.Wz;
using SunLibrary.SunFileLib.Properties;
using SunLibrary.SunFileLib.Structure;
using System;
using System.Collections.Generic;
using System.Drawing;
using XNA = Microsoft.Xna.Framework;

namespace HaCreator.MapEditor.Info
{
    public class ObjectInfo : MapleDrawableInfo
    {
        private string _objectSet;
        private string _l0;
        private string _l1;
        private string _l2;
        private List<List<XNA.Point>> footholdOffsets = null;
        private List<List<XNA.Point>> ropeOffsets = null;
        private List<List<XNA.Point>> ladderOffsets = null;
        private List<XNA.Point> chairOffsets = null;
        private bool connect;

        public ObjectInfo(Bitmap image, System.Drawing.Point origin, string objectSet, string l0, string l1, string l2, SunObject parentObject)
            : base(image, origin, parentObject)
        {
            this._objectSet = objectSet;
            this._l0 = l0;
            this._l1 = l1;
            this._l2 = l2;
            this.connect = objectSet.StartsWith("connect");
        }

        public static ObjectInfo Get(string objectSet, string l0, string l1, string l2)
        {
            try
            {
                SunProperty objInfoProp = Program.InfoManager.ObjectSets[objectSet][l0][l1][l2];
                if (objInfoProp.SETag == null)
                    objInfoProp.SETag = ObjectInfo.Load((SunSubProperty)objInfoProp, objectSet, l0, l1, l2);
                return (ObjectInfo)objInfoProp.SETag;
            }
            catch (KeyNotFoundException e) { return null; }
        }

        private static List<XNA.Point> ParsePropToOffsetList(SunProperty prop)
        {
            List<XNA.Point> result = new List<XNA.Point>();
            foreach (SunVectorProperty point in prop.SunProperties)
            {
                result.Add(SunInfoTools.VectorToXNAPoint(point));
            }
            return result;
        }

        private static List<List<XNA.Point>> ParsePropToOffsetMap(SunProperty prop)
        {
            if (prop == null)
                return null;
            List<List<XNA.Point>> result = new List<List<XNA.Point>>();
            if (prop is SunConvexProperty)
            {
                result.Add(ParsePropToOffsetList((SunConvexProperty)prop));
            }
            else if (prop is SunSubProperty)
            {
                try
                {
                    foreach (SunConvexProperty offsetSet in prop.SunProperties)
                    {
                        result.Add(ParsePropToOffsetList(offsetSet));
                    }
                }
                catch (InvalidCastException exc) { }
            }
            else
            {
                result = null;
            }
            return result;
        }

        private static ObjectInfo Load(SunSubProperty parentObject, string objectSet, string l0, string l1, string l2)
        {
            SunCanvasProperty frame1 = (SunCanvasProperty)SunInfoTools.GetRealProperty(parentObject["0"]);
            ObjectInfo result = new ObjectInfo(frame1.PNG.GetPNG(false), SunInfoTools.VectorToSystemPoint((SunVectorProperty)frame1["origin"]), objectSet, l0, l1, l2, parentObject);
            SunProperty chairs = parentObject["seat"];
            SunProperty ropes = frame1["rope"];
            SunProperty ladders = frame1["ladder"];
            SunProperty footholds = frame1["foothold"];
            result.footholdOffsets = ParsePropToOffsetMap(footholds);
            result.ropeOffsets = ParsePropToOffsetMap(ropes);
            result.ladderOffsets = ParsePropToOffsetMap(ladders);
            if (chairs != null)
                result.chairOffsets = ParsePropToOffsetList(chairs);
            return result;
        }

        private void CreateFootholdsFromAnchorList(Board board, List<FootholdAnchor> anchors)
        {
            for (int i = 0; i < anchors.Count - 1; i++)
            {
                FootholdLine fh = new FootholdLine(board, anchors[i], anchors[i + 1]);
                board.BoardItems.FootholdLines.Add(fh);
            }
        }

        public void ParseOffsets(ObjectInstance instance, Board board, int x, int y)
        {
            bool ladder = l0 == "ladder";
            if (footholdOffsets != null)
            {
                foreach (List<XNA.Point> anchorList in footholdOffsets)
                {
                    List<FootholdAnchor> anchors = new List<FootholdAnchor>();
                    foreach (XNA.Point foothold in anchorList)
                    {
                        FootholdAnchor anchor = new FootholdAnchor(board, x + foothold.X, y + foothold.Y, instance.LayerNumber, instance.PlatformNumber, true);
                        board.BoardItems.FHAnchors.Add(anchor);
                        instance.BindItem(anchor, foothold);
                        anchors.Add(anchor);
                    }
                    CreateFootholdsFromAnchorList(board, anchors);
                }
            }
            if (chairOffsets != null)
            {
                foreach (XNA.Point chairPos in chairOffsets)
                {
                    Chair chair = new Chair(board, x + chairPos.X, y + chairPos.Y);
                    board.BoardItems.Chairs.Add(chair);
                    instance.BindItem(chair, chairPos);
                }
            }
        }

        public override BoardItem CreateInstance(Layer layer, Board board, int x, int y, int z, bool flip)
        {
            ObjectInstance instance = new ObjectInstance(this, layer, board, x, y, z, layer.zMDefault, false, false, false, false, null, null, null, null, null, null, null, flip);
            ParseOffsets(instance, board, x, y);
            return instance;
        }

        public BoardItem CreateInstance(Layer layer, Board board, int x, int y, int z, int zM, SunBool r, SunBool hide, SunBool reactor, SunBool flow, int? rx, int? ry, int? cx, int? cy, string name, string tags, List<ObjectInstanceQuest> questInfo, bool flip, bool parseOffsets)
        {
            ObjectInstance instance = new ObjectInstance(this, layer, board, x, y, z, zM, r, hide, reactor, flow, rx, ry, cx, cy, name, tags, questInfo, flip);
            if (parseOffsets) ParseOffsets(instance, board, x, y);
            return instance;
        }

        public string objectSet
        {
            get
            {
                return _objectSet;
            }
            set
            {
                this._objectSet = value;
            }
        }

        public string l0
        {
            get
            {
                return _l0;
            }
            set
            {
                this._l0 = value;
            }
        }

        public string l1
        {
            get
            {
                return _l1;
            }
            set
            {
                this._l1 = value;
            }
        }

        public string l2
        {
            get
            {
                return _l2;
            }
            set
            {
                this._l2 = value;
            }
        }

        public List<List<XNA.Point>> FootholdOffsets
        {
            get
            {
                return footholdOffsets;
            }
        }

        public List<XNA.Point> ChairOffsets
        {
            get
            {
                return chairOffsets;
            }
        }

        public List<List<XNA.Point>> RopeOffsets
        {
            get
            {
                return ropeOffsets;
            }
        }

        public List<List<XNA.Point>> LadderOffsets
        {
            get
            {
                return ladderOffsets;
            }
        }

        public bool Connect
        { get { return connect; } }
    }
}