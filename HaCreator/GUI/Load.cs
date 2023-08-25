/* Copyright (C) 2015 haha01haha01

* This Source Code Form is subject to the terms of the Mozilla Public
* License, v. 2.0. If a copy of the MPL was not distributed with this
* file, You can obtain one at http://mozilla.org/MPL/2.0/. */

//uncomment the line below to create a space-time tradeoff (saving RAM by wasting more CPU cycles)
#define SPACETIME

using HaCreator.MapEditor;
using HaCreator.Wz;
using Microsoft.Xna.Framework;
using SunLibrary.SunFileLib.Properties;
using SunLibrary.SunFileLib.Structure;
using SunLibrary.SunFileLib.Util;
using System;
using System.IO;
using System.Windows.Forms;

namespace HaCreator.GUI
{
    public partial class Load : System.Windows.Forms.Form
    {
        public bool usebasepng = false;
        public int bufferzone = 100;
        private MultiBoard multiBoard;
        private HaCreator.ThirdParty.TabPages.PageCollection Tabs;
        private EventHandler[] rightClickHandler;

        public Load(MultiBoard board, HaCreator.ThirdParty.TabPages.PageCollection Tabs, EventHandler[] rightClickHandler)
        {
            InitializeComponent();
            DialogResult = DialogResult.Cancel;
            this.multiBoard = board;
            this.Tabs = Tabs;
            this.rightClickHandler = rightClickHandler;
            this.searchBox.TextChanged += this.mapBrowser.searchBox_TextChanged;
        }

        private void Load_Load(object sender, EventArgs e)
        {
            this.mapBrowser.InitializeMaps();
        }

        /// <summary>
        /// Fires when you click load after selecting a map in the list
        /// </summary>
        private void loadButton_Click(object sender, EventArgs e)
        {
            //Hide();
            WaitWindow ww = new WaitWindow("Loading...");
            ww.Show();
            Application.DoEvents();

            MapLoader loader = new MapLoader();
            SunImage mapImage = null;
            string mapName = null, streetName = "", categoryName = "";
            SunSubProperty strMapProp = null;
            string mapid = "1"/*mapBrowser.SelectedItem.Substring(0, 9);*/;
            string mapcat = "Map" + mapid/*.Substring(0, 1)*/;
                                                    //Look inside GeneralAssetTest.sun/Map/Map1/1.img
            mapImage = (SunImage)Program.SfManager["GeneralAssetTest"]["Map"][mapcat][mapid + ".img"];
            strMapProp = SunInfoTools.GetMapStringProp(mapid);
            mapName = SunInfoTools.GetMapName(strMapProp);
            streetName = SunInfoTools.GetMapStreetName(strMapProp);
            categoryName = SunInfoTools.GetMapCategoryName(strMapProp);
            loader.CreateMapFromImage(mapImage, mapName, streetName, categoryName, strMapProp, Tabs, multiBoard, rightClickHandler);
            DialogResult = DialogResult.OK;
            ww.EndWait();
            Close();
        }

        /// <summary>
        /// Fires when you click the start button without selecting a map
        /// </summary>
        private void btnStart_Click(object sender, EventArgs e)
        {
            Application.DoEvents();
            MapLoader loader = new MapLoader();
            //loader.LoadBackgrounds();
            loader.CreateMap(string.Empty, string.Empty, loader.CreateStandardMapMenu(rightClickHandler), new Point(), new Point(), 8, Tabs, multiBoard);
            DialogResult = DialogResult.OK;
            Close();

        }

        private void mapBrowser_SelectionChanged()
        {
            loadButton.Enabled = mapBrowser.LoadAvailable;
        }

        private void Load_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Close();
            }
            else if (e.KeyCode == Keys.Enter)
            {
                loadButton_Click(null, null);
            }
        }
    }
}