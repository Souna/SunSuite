﻿/* Copyright (C) 2015 haha01haha01

* This Source Code Form is subject to the terms of the Mozilla Public
* License, v. 2.0. If a copy of the MPL was not distributed with this
* file, You can obtain one at http://mozilla.org/MPL/2.0/. */

using HaCreator.MapEditor.Instance.Shapes;
using SunLibrary.SunFileLib.Structure.Data;
using XNA = Microsoft.Xna.Framework;

namespace HaCreator.MapEditor.Instance.Misc
{
    public class MiscDot : MapleDot
    {
        private MiscRectangle parentItem;

        public MiscDot(MiscRectangle parentItem, Board board, int x, int y)
            : base(board, x, y)
        {
            this.parentItem = parentItem;
        }

        public override XNA.Color Color
        {
            get { return UserSettings.MiscColor; }
        }

        public override XNA.Color InactiveColor
        {
            get { return MultiBoard.MiscInactiveColor; }
        }

        public override ItemTypes Type
        {
            get { return ItemTypes.Misc; }
        }

        protected override bool RemoveConnectedLines
        {
            get { return false; }
        }

        public MiscRectangle ParentRectangle
        { get { return parentItem; } set { parentItem = value; } }
    }
}