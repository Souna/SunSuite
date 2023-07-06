/* Copyright (C) 2015 haha01haha01

* This Source Code Form is subject to the terms of the Mozilla Public
* License, v. 2.0. If a copy of the MPL was not distributed with this
* file, You can obtain one at http://mozilla.org/MPL/2.0/. */

using HaCreator.MapEditor.Instance;
using HaCreator.Wz;
using SunLibrary.SunFileLib.Properties;
using SunLibrary.SunFileLib.Structure;
using System.Drawing;

namespace HaCreator.MapEditor.Info
{
    public class MobInfo : MapleExtractableInfo
    {
        private string id;
        private string name;

        private SunImage LinkedImage;

        public MobInfo(Bitmap image, System.Drawing.Point origin, string id, string name, SunObject parentObject)
            : base(image, origin, parentObject)
        {
            this.id = id;
            this.name = name;
        }

        private void ExtractPNGFromImage(SunImage image)
        {
            SunCanvasProperty mobImage = SunInfoTools.GetMobImage(image);
            if (mobImage != null)
            {
                Image = mobImage.PNG.GetPNG(false);
                Origin = SunInfoTools.VectorToSystemPoint((SunVectorProperty)mobImage["origin"]);
            }
            else
            {
                Image = new Bitmap(1, 1);
                Origin = new System.Drawing.Point();
            }
        }

        public override void ParseImage()
        {
            SunStringProperty link = (SunStringProperty)((SunSubProperty)((SunImage)ParentObject)["info"])["link"];
            if (link != null)
            {
                LinkedImage = (SunImage)Program.WzManager["mob"][link.Value + ".img"];
                ExtractPNGFromImage(LinkedImage);
            }
            else
            {
                ExtractPNGFromImage((SunImage)ParentObject);
            }
        }

        public static MobInfo Get(string id)
        {
            SunImage mobImage = (SunImage)Program.WzManager["mob"][id + ".img"];
            if (mobImage == null)
                return null;
            if (!mobImage.Parsed)
                mobImage.ParseImage();
            if (mobImage.SETag == null)
                mobImage.SETag = MobInfo.Load(mobImage);
            MobInfo result = (MobInfo)mobImage.SETag;
            result.ParseImageIfNeeded();
            return result;
        }

        private static MobInfo Load(SunImage parentObject)
        {
            string id = SunInfoTools.RemoveExtension(parentObject.Name);
            return new MobInfo(null, new System.Drawing.Point(), id, SunInfoTools.GetMobNameById(id), parentObject);
        }

        public override BoardItem CreateInstance(Layer layer, Board board, int x, int y, int z, bool flip)
        {
            if (Image == null) ParseImage();
            return new MobInstance(this, board, x, y, UserSettings.Mobrx0Offset, UserSettings.Mobrx1Offset, 20, null, UserSettings.defaultMobTime, flip, false, null, null);
        }

        public BoardItem CreateInstance(Board board, int x, int y, int rx0Shift, int rx1Shift, int yShift, string limitedname, int? mobTime, SunBool flip, SunBool hide, int? info, int? team)
        {
            if (Image == null) ParseImage();
            return new MobInstance(this, board, x, y, rx0Shift, rx1Shift, yShift, limitedname, mobTime, flip, hide, info, team);
        }

        public string ID
        {
            get { return id; }
            set { this.id = value; }
        }

        public string Name
        {
            get { return name; }
            set { this.name = value; }
        }
    }
}