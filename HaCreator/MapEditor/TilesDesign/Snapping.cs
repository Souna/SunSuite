/* Copyright (C) 2015 haha01haha01

* This Source Code Form is subject to the terms of the Mozilla Public
* License, v. 2.0. If a copy of the MPL was not distributed with this
* file, You can obtain one at http://mozilla.org/MPL/2.0/. */

using System.Collections.Generic;

namespace HaCreator.MapEditor.TilesDesign
{
    public static class TileSnap
    {
        public static Dictionary<string, MapTileDesign> tileCategories;

        static TileSnap()
        {
            tileCategories = new Dictionary<string, MapTileDesign>();
            tileCategories["basic"] = new basic();
            tileCategories["floatTop"] = new floatTop();
            tileCategories["floatBtm"] = new floatBtm();
            tileCategories["platTop"] = new platTop();
            tileCategories["platBtm"] = new platBtm();
            tileCategories["wallL"] = new wallL();
            tileCategories["wallR"] = new wallR();
            tileCategories["slopeLD"] = new slopeLD();
            tileCategories["slopeLU"] = new slopeLU();
            tileCategories["slopeRD"] = new slopeRD();
            tileCategories["slopeRU"] = new slopeRU();
        }
    }
}