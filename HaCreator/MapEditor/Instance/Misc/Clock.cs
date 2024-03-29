﻿/* Copyright (C) 2015 haha01haha01

* This Source Code Form is subject to the terms of the Mozilla Public
* License, v. 2.0. If a copy of the MPL was not distributed with this
* file, You can obtain one at http://mozilla.org/MPL/2.0/. */

using HaCreator.MapEditor.Instance.Shapes;
using XNA = Microsoft.Xna.Framework;

namespace HaCreator.MapEditor.Instance.Misc
{
    public class Clock : MiscRectangle, ISerializable
    {
        public Clock(Board board, XNA.Rectangle rect)
            : base(board, rect)
        {
        }

        public override string Name
        {
            get { return "Clock"; }
        }

        public Clock(Board board, MapleRectangle.SerializationForm json)
            : base(board, json) { }
    }
}