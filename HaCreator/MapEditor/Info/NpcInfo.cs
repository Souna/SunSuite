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
    public class NpcInfo : MapleExtractableInfo
    {
        private string id;
        private string name;

        private SunImage LinkedImage;

        public NpcInfo(Bitmap image, System.Drawing.Point origin, string id, string name, SunObject parentObject)
            : base(image, origin, parentObject)
        {
            this.id = id;
            this.name = name;
            if (image != null && image.Width == 1 && image.Height == 1)
                image = global::HaCreator.Properties.Resources.placeholder;
        }

        private void ExtractPNGFromImage(SunImage image)
        {
            SunCanvasProperty npcImage = SunInfoTools.GetNpcImage(image);
            if (npcImage != null)
            {
                Image = npcImage.PNG.GetPNG(false);
                if (Image.Width == 1 && Image.Height == 1)
                {
                    Image = global::HaCreator.Properties.Resources.placeholder;
                }
                Origin = SunInfoTools.VectorToSystemPoint((SunVectorProperty)npcImage["origin"]);
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
                LinkedImage = (SunImage)Program.SfManager["npc"][link.Value + ".img"];
                ExtractPNGFromImage(LinkedImage);
            }
            else
            {
                ExtractPNGFromImage((SunImage)ParentObject);
            }
        }

        public static NpcInfo Get(string id)
        {
            SunImage npcImage = (SunImage)Program.SfManager["npc"][id + ".img"];
            if (npcImage == null)
                return null;
            if (!npcImage.Parsed)
                npcImage.ParseImage();
            if (npcImage.SETag == null)
                npcImage.SETag = NpcInfo.Load(npcImage);
            NpcInfo result = (NpcInfo)npcImage.SETag;
            result.ParseImageIfNeeded();
            return result;
        }

        private static NpcInfo Load(SunImage parentObject)
        {
            string id = SunInfoTools.RemoveExtension(parentObject.Name);
            return new NpcInfo(null, new System.Drawing.Point(), id, SunInfoTools.GetNpcNameById(id), parentObject);
        }

        public override BoardItem CreateInstance(Layer layer, Board board, int x, int y, int z, bool flip)
        {
            if (Image == null) ParseImage();
            return new NpcInstance(this, board, x, y, UserSettings.Npcrx0Offset, UserSettings.Npcrx1Offset, 8, null, 0, flip, false, null, null);
        }

        public BoardItem CreateInstance(Board board, int x, int y, int rx0Shift, int rx1Shift, int yShift, string limitedname, int? mobTime, SunBool flip, SunBool hide, int? info, int? team)
        {
            if (Image == null) ParseImage();
            return new NpcInstance(this, board, x, y, rx0Shift, rx1Shift, yShift, limitedname, mobTime, flip, hide, info, team);
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

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                this.name = value;
            }
        }
    }
}