﻿/* Copyright (C) 2015 haha01haha01

* This Source Code Form is subject to the terms of the Mozilla Public
* License, v. 2.0. If a copy of the MPL was not distributed with this
* file, You can obtain one at http://mozilla.org/MPL/2.0/. */

using HaCreator.MapEditor.Instance.Shapes;
using XNA = Microsoft.Xna.Framework;

namespace HaCreator.MapEditor.Instance.Misc
{
    public class Area : MiscRectangle, ISerializable
    {
        private string id;

        public Area(Board board, XNA.Rectangle rect, string id)
            : base(board, rect)
        {
            this.id = id;
        }

        public string Identifier
        {
            get { return id; }
            set { id = value; }
        }

        public override string Name
        {
            get { return "Area " + id; }
        }

        public new class SerializationForm : MapleRectangle.SerializationForm
        {
            public string id;
        }

        public override object Serialize()
        {
            SerializationForm result = new SerializationForm();
            UpdateSerializedForm(result);
            return result;
        }

        protected void UpdateSerializedForm(SerializationForm result)
        {
            base.UpdateSerializedForm(result);
            result.id = id;
        }

        public Area(Board board, SerializationForm json)
            : base(board, json)
        {
            id = json.id;
        }
    }
}