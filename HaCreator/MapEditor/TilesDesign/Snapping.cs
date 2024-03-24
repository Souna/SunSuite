/* Copyright (C) 2015 haha01haha01

* This Source Code Form is subject to the terms of the Mozilla Public
* License, v. 2.0. If a copy of the MPL was not distributed with this
* file, You can obtain one at http://mozilla.org/MPL/2.0/. */

using System.Collections.Generic;

namespace HaCreator.MapEditor.TilesDesign
{
    public static class TileSnap
    {
        public static Dictionary<string, MapTileDesign> tileCats;

        static TileSnap()
        {
            tileCats = new Dictionary<string, MapTileDesign>();
            tileCats["basic"] = new basic();
            tileCats["floatTop"] = new floatTop();
            tileCats["floatBtm"] = new floatBtm();
            tileCats["platTop"] = new platTop();
            tileCats["platBtm"] = new platBtm();
            tileCats["wallL"] = new wallL();
            tileCats["wallR"] = new wallR();
            tileCats["slopeLD"] = new slopeLD();
            tileCats["slopeLU"] = new slopeLU();
            tileCats["slopeRD"] = new slopeRD();
            tileCats["slopeRU"] = new slopeRU();
        }
    }
}