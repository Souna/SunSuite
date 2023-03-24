﻿/* Copyright (C) 2015 haha01haha01

* This Source Code Form is subject to the terms of the Mozilla Public
* License, v. 2.0. If a copy of the MPL was not distributed with this
* file, You can obtain one at http://mozilla.org/MPL/2.0/. */

using HaCreator.MapEditor.Instance;
using HaCreator.Wz;
using SunLibrary.SunFileLib.Properties;
using SunLibrary.SunFileLib.Structure;
using SunLibrary.SunFileLib.Structure.Data;
using System.Drawing;

namespace HaCreator.MapEditor.Info
{
    public class BackgroundInfo : MapleDrawableInfo
    {
        private string _backgroundSet;
        private bool _ani; //1 if animated, 0 otherwise.If the background is animated, it has more than 1 frame
        private string _no; /*Background sprite number. If ani is 0, the background is loaded from
                             * Map.sun/Back/${backgroundSet}.img/back/${no}.
                             * Otherwise, it is loaded from Map.sun/Back/${backgroundSet}.img/ani/${no}.*/

        public BackgroundInfo(Bitmap image, System.Drawing.Point origin, string bS, bool ani, string no, SunObject parentObject)
            : base(image, origin, parentObject)
        {
            _backgroundSet = bS;
            _ani = ani;
            _no = no;
        }

        public static BackgroundInfo Get(string bS, bool ani, string no)
        {
            if (!Program.InfoManager.BackgroundSets.ContainsKey(bS))
                return null;
            SunImage bgSetImg = Program.InfoManager.BackgroundSets[bS];
            SunProperty bgInfoProp = bgSetImg[ani ? "ani" : "back"][no];
            if (bgInfoProp.SETag == null)
                bgInfoProp.SETag = Load(bgInfoProp, bS, ani, no);
            return (BackgroundInfo)bgInfoProp.SETag;
        }

        private static BackgroundInfo Load(SunProperty parentObject, string bS, bool ani, string no)
        {
            SunCanvasProperty frame0 = ani ? (SunCanvasProperty)WzInfoTools.GetRealProperty(parentObject["0"]) : (SunCanvasProperty)WzInfoTools.GetRealProperty(parentObject);
            return new BackgroundInfo(frame0.PNG.GetPNG(false), WzInfoTools.VectorToSystemPoint((SunVectorProperty)frame0["origin"]), bS, ani, no, parentObject);
        }

        public override BoardItem CreateInstance(Layer layer, Board board, int x, int y, int z, bool flip)
        {
            return new BackgroundInstance(this, board, x, y, z, -100, -100, 0, 0, 0, 255, false, flip);
        }

        public BoardItem CreateInstance(Board board, int x, int y, int z, int rx, int ry, int cx, int cy, BackgroundType type, int a, bool front, bool flip)
        {
            return new BackgroundInstance(this, board, x, y, z, rx, ry, cx, cy, type, a, front, flip);
        }

        public string bS
        {
            get
            {
                return _backgroundSet;
            }
            set
            {
                this._backgroundSet = value;
            }
        }

        public bool ani
        {
            get
            {
                return _ani;
            }
            set
            {
                this._ani = value;
            }
        }

        public string no
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
    }
}