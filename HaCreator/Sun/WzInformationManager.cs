﻿/* Copyright (C) 2015 haha01haha01

* This Source Code Form is subject to the terms of the Mozilla Public
* License, v. 2.0. If a copy of the MPL was not distributed with this
* file, You can obtain one at http://mozilla.org/MPL/2.0/. */

using HaCreator.MapEditor.Info;
using SunLibrary.SunFileLib.Properties;
using SunLibrary.SunFileLib.Structure;
using System.Collections.Generic;
using System.Drawing;

namespace HaCreator.Wz
{
    public class WzInformationManager
    {
        public Dictionary<string, string> NPCs = new Dictionary<string, string>();
        public Dictionary<string, string> Mobs = new Dictionary<string, string>();
        public Dictionary<string, ReactorInfo> Reactors = new Dictionary<string, ReactorInfo>();
        public Dictionary<string, SunImage> TileSets = new Dictionary<string, SunImage>();
        public Dictionary<string, SunImage> ObjectSets = new Dictionary<string, SunImage>();
        public Dictionary<string, SunImage> BackgroundSets = new Dictionary<string, SunImage>();
        public Dictionary<string, SunSoundProperty> BGMs = new Dictionary<string, SunSoundProperty>();
        public Dictionary<string, Bitmap> MapMarks = new Dictionary<string, Bitmap>();
        public Dictionary<string, string> Maps = new Dictionary<string, string>();
        public Dictionary<string, PortalInfo> Portals = new Dictionary<string, PortalInfo>();
        public List<string> PortalTypeById = new List<string>();
        public Dictionary<string, int> PortalIdByType = new Dictionary<string, int>();
        public Dictionary<string, PortalGameImageInfo> GamePortals = new Dictionary<string, PortalGameImageInfo>();
    }
}