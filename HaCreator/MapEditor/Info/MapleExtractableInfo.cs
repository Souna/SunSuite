/* Copyright (C) 2015 haha01haha01

* This Source Code Form is subject to the terms of the Mozilla Public
* License, v. 2.0. If a copy of the MPL was not distributed with this
* file, You can obtain one at http://mozilla.org/MPL/2.0/. */

using SunLibrary.SunFileLib.Structure;
using System.Drawing;

namespace HaCreator.MapEditor.Info
{
    public abstract class MapleExtractableInfo : MapleDrawableInfo
    {
        public MapleExtractableInfo(Bitmap image, System.Drawing.Point origin, SunObject parentObject)
            : base(image, origin, parentObject)
        {
        }

        public override Bitmap Image
        {
            get
            {
                if (base.Image == null)
                    ParseImage();
                if (base.Image.Width == 1 && base.Image.Height == 1)
                {
                    return global::HaCreator.Properties.Resources.placeholder;
                }
                return base.Image;
            }
            set
            {
                base.Image = value;
            }
        }

        public void ParseImageIfNeeded()
        {
            if (Image == null)
                ParseImage();
        }

        public abstract void ParseImage();
    }
}