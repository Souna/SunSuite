﻿/*
  koolk's Map Editor

  Copyright (c) 2009-2013 koolk

  This software is provided 'as-is', without any express or implied
  warranty. In no event will the authors be held liable for any damages
  arising from the use of this software.

  Permission is granted to anyone to use this software for any purpose,
  including commercial applications, and to alter it and redistribute it
  freely, subject to the following restrictions:

     1. The origin of this software must not be misrepresented; you must not
     claim that you wrote the original software. If you use this software
     in a product, an acknowledgment in the product documentation would be
     appreciated but is not required.

     2. Altered source versions must be plainly marked as such, and must not be
     misrepresented as being the original software.

     3. This notice may not be removed or altered from any source
     distribution.
*/

namespace HaCreator.MapEditor.TilesDesign
{
    internal class floatTop : MapTileDesign
    {
        public floatTop()
        {
            type = "floatTop";

            potentials.Add(new MapTileDesignPotential("platTop", -90, 0));
            potentials.Add(new MapTileDesignPotential("platTop", 0, 0));
            potentials.Add(new MapTileDesignPotential("wallL", 0, 0));
            potentials.Add(new MapTileDesignPotential("wallR", 0, 0));
            potentials.Add(new MapTileDesignPotential("floatBtm", 0, 0));
            potentials.Add(new MapTileDesignPotential("slopeLU", 90, 0));
            potentials.Add(new MapTileDesignPotential("slopeLU", 0, 60));
            potentials.Add(new MapTileDesignPotential("slopeRU", -90, 0));
            potentials.Add(new MapTileDesignPotential("slopeRU", 0, 60));
        }
    }
}