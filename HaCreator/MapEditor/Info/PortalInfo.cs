/* Copyright (C) 2015 haha01haha01

* This Source Code Form is subject to the terms of the Mozilla Public
* License, v. 2.0. If a copy of the MPL was not distributed with this
* file, You can obtain one at http://mozilla.org/MPL/2.0/. */

using HaCreator.MapEditor.Instance;
using HaCreator.Wz;
using SunLibrary.SunFileLib.Properties;
using SunLibrary.SunFileLib.Structure;
using SunLibrary.SunFileLib.Structure.Data;
using System;
using System.Drawing;

namespace HaCreator.MapEditor.Info
{
    public class PortalInfo : MapleDrawableInfo
    {
        private string type;

        public PortalInfo(string type, Bitmap image, System.Drawing.Point origin, SunObject parentObject)
            : base(image, origin, parentObject)
        {
            this.type = type;
        }

        public static PortalInfo Load(SunCanvasProperty parentObject)
        {
            PortalInfo portal = new PortalInfo(parentObject.Name, parentObject.PNG.GetPNG(false), WzInfoTools.VectorToSystemPoint((SunVectorProperty)parentObject["origin"]), parentObject);
            Program.InfoManager.Portals.Add(portal.type, portal);
            return portal;
        }

        public override BoardItem CreateInstance(Layer layer, Board board, int x, int y, int z, bool flip)
        {
            switch (type)
            {
                case PortalType.SPAWN_POINT:
                    return new PortalInstance(this, board, x, y, "sp", type, "", 999999999, null, null, null, null, null, null, null, null, null);

                case PortalType.INVISIBLE:
                case PortalType.VISIBLE:
                    //case PortalType.PORTALTYPE_COLLISION:
                    //case PortalType.PORTALTYPE_CHANGABLE:
                    //case PortalType.PORTALTYPE_CHANGABLE_INVISIBLE:
                    return new PortalInstance(this, board, x, y, "portal", type, "", 999999999, null, null, null, null, null, null, null, null, null);
                //  case PortalType.PORTALTYPE_TOWNPORTAL_POINT:
                //      return new PortalInstance(this, board, x, y, "tp", type, "", 999999999, null, null, null, null, null, null, null, null, null);
                //  case PortalType.PORTALTYPE_SCRIPT:
                //  case PortalType.PORTALTYPE_SCRIPT_INVISIBLE:
                //  case PortalType.PORTALTYPE_COLLISION_SCRIPT:
                //      return new PortalInstance(this, board, x, y, "portal", type, "", 999999999, "script", null, null, null, null, null, null, null, null);
                //  case PortalType.PORTALTYPE_HIDDEN:
                //      return new PortalInstance(this, board, x, y, "portal", type, "", 999999999, null, null, null, null, null, null, "", null, null);
                //  case PortalType.PORTALTYPE_SCRIPT_HIDDEN:
                //      return new PortalInstance(this, board, x, y, "portal", type, "", 999999999, "script", null, null, null, null, null, "", null, null);
                //  case PortalType.PORTALTYPE_COLLISION_VERTICAL_JUMP:
                //  case PortalType.PORTALTYPE_COLLISION_CUSTOM_IMPACT:
                //  case PortalType.PORTALTYPE_COLLISION_UNKNOWN_PCIG:
                ////  case PortalType.PORTALTYPE_SCRIPT_HIDDEN_UNG:
                //      return new PortalInstance(this, board, x, y, "portal", type, "", 999999999, "script", null, null, null, null, null, "", null, null);
                default:
                    throw new Exception("unknown pt @ CreateInstance");
            }
        }

        public PortalInstance CreateInstance(Board board, int x, int y, string pn, string tn, int tm, string script, int? delay, SunBool hideTooltip, SunBool onlyOnce, int? horizontalImpact, int? verticalImpact, string image, int? hRange, int? vRange)
        {
            return new PortalInstance(this, board, x, y, pn, type, tn, tm, script, delay, hideTooltip, onlyOnce, horizontalImpact, verticalImpact, image, hRange, vRange);
        }

        public string Type
        {
            get { return type; }
        }

        public static PortalInfo GetPortalInfoByType(string type)
        {
            return Program.InfoManager.Portals[type];
        }
    }
}