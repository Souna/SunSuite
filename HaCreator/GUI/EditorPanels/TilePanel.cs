/* Copyright (C) 2015 haha01haha01

* This Source Code Form is subject to the terms of the Mozilla Public
* License, v. 2.0. If a copy of the MPL was not distributed with this
* file, You can obtain one at http://mozilla.org/MPL/2.0/. */

using HaCreator.MapEditor;
using HaCreator.MapEditor.Info;
using HaCreator.MapEditor.UndoRedo;
using HaCreator.ThirdParty;
using SunLibrary.SunFileLib.Properties;
using SunLibrary.SunFileLib.Structure;
using SunLibrary.SunFileLib.Structure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace HaCreator.GUI.EditorPanels
{
    public partial class TilePanel : DockContent
    {
        private HaCreatorStateManager hcsm;

        public TilePanel(HaCreatorStateManager hcsm)
        {
            this.hcsm = hcsm;
            hcsm.SetTilePanel(this);
            InitializeComponent();

            List<string> sortedTileSets = new List<string>();
            foreach (KeyValuePair<string, SunImage> tS in Program.InfoManager.TileSets)
                sortedTileSets.Add(tS.Key);
            sortedTileSets.Sort();
            foreach (string tS in sortedTileSets)
                tileSetList.Items.Add(tS);
        }

        private void searchResultsBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedIndexChanged.Invoke(sender, e);
        }

        public event EventHandler SelectedIndexChanged;

        private void tileBrowse_Click(object sender, EventArgs e)
        {
            lock (hcsm.MultiBoard)
            {
                new TileSetBrowser(tileSetList).ShowDialog();
            }
        }

        private void tileSetList_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadTileSetList();
        }

        public void LoadTileSetList()
        {
            lock (hcsm.MultiBoard)
            {
                if (tileSetList.SelectedItem == null) return;
                tileImagesContainer.Controls.Clear();
                string selectedSetName = (string)tileSetList.SelectedItem;
                if (!Program.InfoManager.TileSets.ContainsKey(selectedSetName))
                    return;
                SunImage tileSetImage = Program.InfoManager.TileSets[selectedSetName];
                int? mag = InfoTool.GetOptionalInt(tileSetImage["info"]["mag"]);
                foreach (SunSubProperty tCat in tileSetImage.SunProperties)
                {
                    if (tCat.Name == "info") continue;
                    if (ApplicationSettings.randomTiles)
                    {
                        SunCanvasProperty canvasProp = (SunCanvasProperty)tCat["0"];
                        if (canvasProp == null) continue;
                        ImageViewer item = tileImagesContainer.Add(canvasProp.PNG.GetPNG(false), tCat.Name, true);
                        TileInfo[] randomInfos = new TileInfo[tCat.SunProperties.Count];
                        for (int i = 0; i < randomInfos.Length; i++)
                        {
                            randomInfos[i] = TileInfo.Get((string)tileSetList.SelectedItem, tCat.Name, tCat.SunProperties[i].Name, mag);
                        }
                        item.Tag = randomInfos;
                        item.MouseDown += new MouseEventHandler(tileItem_Click);
                        item.MouseUp += new MouseEventHandler(ImageViewer.item_MouseUp);
                    }
                    else
                    {
                        foreach (SunCanvasProperty tile in tCat.SunProperties)
                        {
                            ImageViewer item = tileImagesContainer.Add(tile.PNG.GetPNG(false), tCat.Name + "/" + tile.Name, true);
                            item.Tag = TileInfo.Get((string)tileSetList.SelectedItem, tCat.Name, tile.Name, mag);
                            item.MouseDown += new MouseEventHandler(tileItem_Click);
                            item.MouseUp += new MouseEventHandler(ImageViewer.item_MouseUp);
                        }
                    }
                }
            }
        }

        private void tileItem_Click(object sender, MouseEventArgs e)
        {
            lock (hcsm.MultiBoard)
            {
                ImageViewer item = (ImageViewer)sender;
                if (!hcsm.MultiBoard.AssertLayerSelected())
                {
                    return;
                }
                Layer layer = hcsm.MultiBoard.SelectedBoard.SelectedLayer;
                if (layer.tS != null)
                {
                    TileInfo infoToAdd = null;
                    if (ApplicationSettings.randomTiles)
                        infoToAdd = ((TileInfo[])item.Tag)[0];
                    else
                        infoToAdd = (TileInfo)item.Tag;
                    if (infoToAdd.tS != layer.tS)
                    {
                        if (MessageBox.Show("This action will change the layer's tS. Proceed?", "Layer tS Change", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != System.Windows.Forms.DialogResult.Yes)
                            return;
                        List<UndoRedoAction> actions = new List<UndoRedoAction>();
                        actions.Add(UndoRedoManager.LayerTSChanged(layer, layer.tS, infoToAdd.tS));
                        layer.ReplaceTS(infoToAdd.tS);
                        hcsm.MultiBoard.SelectedBoard.UndoRedoMan.AddUndoBatch(actions);
                    }
                }
                hcsm.EnterEditMode(ItemTypes.Tiles);
                if (ApplicationSettings.randomTiles)
                    hcsm.MultiBoard.SelectedBoard.Mouse.SetRandomTilesMode((TileInfo[])item.Tag);
                else
                    hcsm.MultiBoard.SelectedBoard.Mouse.SetHeldInfo((TileInfo)item.Tag);
                hcsm.MultiBoard.Focus();
                item.IsActive = true;
            }
        }
    }
}