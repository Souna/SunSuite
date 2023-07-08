/* Copyright (C) 2015 haha01haha01

* This Source Code Form is subject to the terms of the Mozilla Public
* License, v. 2.0. If a copy of the MPL was not distributed with this
* file, You can obtain one at http://mozilla.org/MPL/2.0/. */

using HaCreator.MapEditor;
using HaCreator.Wz;
using SunLibrary.SunFileLib.Structure;
using SunLibrary.SunFileLib.Structure.Data;
using SunLibrary.SunFileLib.Util;
using System;
using System.IO;
using System.Windows.Forms;

namespace HaCreator.GUI
{
    public partial class Initialization : System.Windows.Forms.Form
    {
        public HaEditor editor = null;

        public Initialization()
        {
            InitializeComponent();

#if RELEASE
                debugButton.Visible = false;
#endif
        }

        private bool IsPathCommon(string path)
        {
            foreach (string commonPath in commonSfPaths)
            {
                if (commonPath == path)
                    return true;
            }
            return false;
        }

        private void btnInitializeFiles_Click(object sender, EventArgs e)
        {
            ApplicationSettings.SunFileFolderIndex = pathBox.SelectedIndex;
            string sfPath = pathBox.Text;

            if (sfPath == "Select SunFile Folder")
            {
                MessageBox.Show("Please select the folder containing the SunFiles.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!ApplicationSettings.SunFileFolder.Contains(sfPath) && !IsPathCommon(sfPath))
            {
                ApplicationSettings.SunFileFolder = ApplicationSettings.SunFileFolder == "" ? sfPath : (ApplicationSettings.SunFileFolder + "," + sfPath);
            }

            InitializeSunFiles(sfPath);

            Hide();
            Application.DoEvents();
            editor = new HaEditor();
            editor.ShowDialog();
            Application.Exit();
        }

        private void InitializeSunFiles(string sfPath)
        {
            Program.SfManager = new FileManager(sfPath);

            txtStatus.Text = "Initializing test.sun...";
            Application.DoEvents();
            Program.SfManager.LoadSunFile("test");
            //Program.SfManager.ExtractMaps();
            Program.SfManager.ExtractBackgroundSets();

            //txtStatus.Text = "Initializing String.wz...";
            //Application.DoEvents();
            //Program.SfManager.LoadSunFile("string");
            //Program.SfManager.ExtractMaps();

            //txtStatus.Text = "Initializing Mob.wz...";
            //Application.DoEvents();
            //Program.SfManager.LoadSunFile("mob");
            //Program.SfManager.ExtractMobFile();

            //txtStatus.Text = "Initializing Npc.wz...";
            //Application.DoEvents();
            //Program.SfManager.LoadSunFile("npc");
            //Program.SfManager.ExtractNpcFile();

            //txtStatus.Text = "Initializing Reactor.wz...";
            //Application.DoEvents();
            //Program.SfManager.LoadSunFile("reactor");
            //Program.SfManager.ExtractReactorFile();

            //txtStatus.Text = "Initializing Sound.wz...";
            //Application.DoEvents();
            //Program.SfManager.LoadSunFile("sound");
            //Program.SfManager.ExtractSoundFile();

            //txtStatus.Text = "Initializing Map.wz...";
            //Application.DoEvents();
            //Program.SfManager.LoadSunFile("map");
            //Program.SfManager.ExtractMapMarks();
            //Program.SfManager.ExtractPortals();
            //Program.SfManager.ExtractTileSets();
            //Program.SfManager.ExtractObjSets();
            //Program.SfManager.ExtractBackgroundSets();

            //txtStatus.Text = "Initializing UI.wz...";
            //Application.DoEvents();
            //Program.SfManager.LoadSunFile("ui");
        }

        private static readonly string[] commonSfPaths = new string[] { @"C:\Users\lapto\Desktop\stuff\NewSunFiles" };

        private void Initialization_Load(object sender, EventArgs e)
        {
            try
            {
                string[] paths = ApplicationSettings.SunFileFolder.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                foreach (string x in paths)
                {
                    pathBox.Items.Add(x);
                }
                foreach (string path in commonSfPaths)
                {
                    if (Directory.Exists(path))
                    {
                        pathBox.Items.Add(path);
                    }
                }
                if (pathBox.Items.Count == 0)
                    pathBox.Items.Add("Select SunFile Folder");
            }
            catch
            {
            }
            if (pathBox.Items.Count < ApplicationSettings.SunFileFolderIndex + 1)
            {
                pathBox.SelectedIndex = pathBox.Items.Count - 1;
            }
            else
            {
                pathBox.SelectedIndex = ApplicationSettings.SunFileFolderIndex;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog mapleSelect = new FolderBrowserDialog()
            {
                ShowNewFolderButton = true,
                //   RootFolder = Environment.SpecialFolder.ProgramFilesX86,
                Description = "Select the MapleStory folder."
            })
            {
                if (mapleSelect.ShowDialog() != DialogResult.OK)
                    return;

                pathBox.Items.Add(mapleSelect.SelectedPath);
                pathBox.SelectedIndex = pathBox.Items.Count - 1;
            };
        }

        private void debugButton_Click(object sender, EventArgs e)
        {
            // This function iterates over all maps in the game and verifies that we recognize all their props
            // It is meant to use by the developer(s) to speed up the process of adjusting this program for different MapleStory versions
            string wzPath = pathBox.Text;
            short version = -1;
            InitializeSunFiles(wzPath);

            MultiBoard mb = new MultiBoard();
            Board b = new Board(new Microsoft.Xna.Framework.Point(), new Microsoft.Xna.Framework.Point(), mb, null, ItemTypes.None, ItemTypes.None);

            foreach (string mapid in Program.InfoManager.Maps.Keys)
            {
                MapLoader loader = new MapLoader();
                string mapcat = "Map" + mapid.Substring(0, 1);
                SunImage mapImage = (SunImage)Program.SfManager["map"]["Map"][mapcat][mapid + ".img"];
                if (mapImage == null)
                {
                    continue;
                }
                mapImage.ParseImage();
                if (mapImage["info"]["link"] != null)
                {
                    mapImage.UnparseImage();
                    continue;
                }
                loader.VerifyMapPropsKnown(mapImage, true);
                MapInfo info = new MapInfo(mapImage, null, null, null);
                loader.LoadMisc(mapImage, b);
                if (ErrorLogger.ErrorsPresent())
                {
                    ErrorLogger.SaveToFile("debug_errors.txt");
                    ErrorLogger.ClearErrors();
                }
                mapImage.UnparseImage(); // To preserve memory, since this is a very memory intensive test
            }
            MessageBox.Show("Done");
        }

        private void Initialization_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnInitializeFiles_Click(null, null);
            }
            else if (e.KeyCode == Keys.Escape)
            {
                Close();
            }
        }
    }
}