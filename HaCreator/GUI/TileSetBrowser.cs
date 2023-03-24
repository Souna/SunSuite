/* Copyright (C) 2015 haha01haha01

* This Source Code Form is subject to the terms of the Mozilla Public
* License, v. 2.0. If a copy of the MPL was not distributed with this
* file, You can obtain one at http://mozilla.org/MPL/2.0/. */

using HaCreator.ThirdParty;
using SunLibrary.SunFileLib.Properties;
using SunLibrary.SunFileLib.Structure;
using System.Collections.Generic;
using System.Windows.Forms;

namespace HaCreator.GUI
{
    public partial class TileSetBrowser : Form
    {
        private ListBox targetListBox;
        public ImageViewer selectedItem = null;

        public TileSetBrowser(ListBox target)
        {
            InitializeComponent();
            targetListBox = target;
            List<string> sortedTileSets = new List<string>();
            foreach (KeyValuePair<string, SunImage> tS in Program.InfoManager.TileSets)
                sortedTileSets.Add(tS.Key);
            sortedTileSets.Sort();
            foreach (string tS in sortedTileSets)
            {
                SunImage tSImage = Program.InfoManager.TileSets[tS];
                if (!tSImage.Parsed) tSImage.ParseImage();
                SunProperty enh0 = tSImage["enH0"];
                if (enh0 == null) continue;
                SunCanvasProperty image = (SunCanvasProperty)enh0["0"];
                if (image == null) continue;
                //image.PNG.GetPNG(true);
                ImageViewer item = koolkLVContainer.Add(image.PNG.GetPNG(true), tS, true);
                item.MouseDown += new MouseEventHandler(item_Click);
                item.MouseDoubleClick += new MouseEventHandler(item_DoubleClick);
            }
        }

        private void item_DoubleClick(object sender, MouseEventArgs e)
        {
            if (selectedItem == null) return;
            targetListBox.SelectedItem = selectedItem.Name;
            Close();
        }

        private void item_Click(object sender, MouseEventArgs e)
        {
            if (selectedItem != null)
                selectedItem.IsActive = false;
            selectedItem = (ImageViewer)sender;
            selectedItem.IsActive = true;
        }

        private void TileSetBrowser_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                e.Handled = true;
                Close();
            }
        }
    }
}