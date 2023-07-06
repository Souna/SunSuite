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
    public class ReactorInfo : MapleExtractableInfo
    {
        private string id;

        private SunImage LinkedImage;

        public ReactorInfo(Bitmap image, System.Drawing.Point origin, string id, SunObject parentObject)
            : base(image, origin, parentObject)
        {
            this.id = id;
        }

        private void ExtractPNGFromImage(SunImage image)
        {
            SunCanvasProperty reactorImage = SunInfoTools.GetReactorImage(image);
            if (reactorImage != null)
            {
                Image = reactorImage.PNG.GetPNG(false);
                Origin = SunInfoTools.VectorToSystemPoint((SunVectorProperty)reactorImage["origin"]);
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
                LinkedImage = (SunImage)Program.WzManager["reactor"][link.Value + ".img"];
                ExtractPNGFromImage(LinkedImage);
            }
            else
            {
                ExtractPNGFromImage((SunImage)ParentObject);
            }
        }

        public static ReactorInfo Get(string id)
        {
            ReactorInfo result = Program.InfoManager.Reactors[id];
            result.ParseImageIfNeeded();
            return result;
        }

        public static ReactorInfo Load(SunImage parentObject)
        {
            return new ReactorInfo(null, new System.Drawing.Point(), SunInfoTools.RemoveExtension(parentObject.Name), parentObject);
        }

        public override BoardItem CreateInstance(Layer layer, Board board, int x, int y, int z, bool flip)
        {
            if (Image == null) ParseImage();
            return new ReactorInstance(this, board, x, y, UserSettings.defaultReactorTime, "", flip);
        }

        public BoardItem CreateInstance(Board board, int x, int y, int reactorTime, string name, bool flip)
        {
            if (Image == null) ParseImage();
            return new ReactorInstance(this, board, x, y, reactorTime, name, flip);
        }

        public string ID
        {
            get
            {
                return id;
            }
            set
            {
                this.id = value;
            }
        }
    }
}