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
            foreach (string commonPath in commonMaplePaths)
            {
                if (commonPath == path)
                    return true;
            }
            return false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ApplicationSettings.MapleVersionIndex = versionBox.SelectedIndex;
            ApplicationSettings.MapleFolderIndex = pathBox.SelectedIndex;
            string wzPath = pathBox.Text;

            if (wzPath == "Select Maple Folder")
            {
                MessageBox.Show("Please select the maple folder.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!ApplicationSettings.MapleFolder.Contains(wzPath) && !IsPathCommon(wzPath))
            {
                ApplicationSettings.MapleFolder = ApplicationSettings.MapleFolder == "" ? wzPath : (ApplicationSettings.MapleFolder + "," + wzPath);
            }

            /* short version = -1;
             if (versionBox.SelectedIndex != 3)
             {
                 string testFile = File.Exists(Path.Combine(wzPath, "Data.wz")) ? "Data.wz" : "Item.wz";
                 try
                 {
                     fileVersion = WzTool.DetectMapleVersion(Path.Combine(wzPath, testFile), out version);
                 }
                 catch (Exception ex)
                 {
                     HaRepackerLib.Warning.Error("Error initializing " + testFile + " (" + ex.Message + ").\r\nCheck that the directory is valid and the file is not in use.");
                     return;
                 }
             }
             else
             {*/
            //  }

            InitializeSunFiles(wzPath);

            Hide();
            Application.DoEvents();
            editor = new HaEditor();
            editor.ShowDialog();
            Application.Exit();
        }

        private void InitializeSunFiles(string wzPath)
        {
            Program.WzManager = new FileManager(wzPath);
            if (Program.WzManager.HasDataFile)//currently always false
            {
                textBox2.Text = "Initializing Data.wz...";
                Application.DoEvents();
                Program.WzManager.LoadDataSunFile("data");
                Program.WzManager.ExtractMaps();
                //Program.WzManager.ExtractItems();
                Program.WzManager.ExtractMobFile();
                Program.WzManager.ExtractNpcFile();
                Program.WzManager.ExtractReactorFile();
                Program.WzManager.ExtractSoundFile();
                Program.WzManager.ExtractMapMarks();
                Program.WzManager.ExtractPortals();
                Program.WzManager.ExtractTileSets();
                Program.WzManager.ExtractObjSets();
                Program.WzManager.ExtractBackgroundSets();
            }
            else
            {
                textBox2.Text = "Initializing String.wz...";
                Application.DoEvents();
                Program.WzManager.LoadSunFile("string");
                Program.WzManager.ExtractMaps();

                textBox2.Text = "Initializing Mob.wz...";
                Application.DoEvents();
                Program.WzManager.LoadSunFile("mob");
                Program.WzManager.ExtractMobFile();

                textBox2.Text = "Initializing Npc.wz...";
                Application.DoEvents();
                Program.WzManager.LoadSunFile("npc");
                Program.WzManager.ExtractNpcFile();

                textBox2.Text = "Initializing Reactor.wz...";
                Application.DoEvents();
                Program.WzManager.LoadSunFile("reactor");
                Program.WzManager.ExtractReactorFile();

                textBox2.Text = "Initializing Sound.wz...";
                Application.DoEvents();
                Program.WzManager.LoadSunFile("sound");
                Program.WzManager.ExtractSoundFile();

                textBox2.Text = "Initializing Map.wz...";
                Application.DoEvents();
                Program.WzManager.LoadSunFile("map");
                Program.WzManager.ExtractMapMarks();
                Program.WzManager.ExtractPortals();
                Program.WzManager.ExtractTileSets();
                Program.WzManager.ExtractObjSets();
                Program.WzManager.ExtractBackgroundSets();

                if (Program.WzManager.LoadSunFile("map001"))
                {
                    textBox2.Text = "Initializing Map001.wz...";
                    Application.DoEvents();
                    Program.WzManager.ExtractBackgroundSets();
                    Program.WzManager.ExtractObjSets();
                }

                if (Program.WzManager.LoadSunFile("map002")) //kms now stores main map key here
                {
                    textBox2.Text = "Initializing Map002.wz...";
                    Application.DoEvents();
                    Program.WzManager.ExtractBackgroundSets();
                    Program.WzManager.ExtractObjSets();
                }

                if (Program.WzManager.LoadSunFile("map2"))
                {
                    textBox2.Text = "Initializing Map2.wz...";
                    Application.DoEvents();
                    Program.WzManager.ExtractBackgroundSets();
                    Program.WzManager.ExtractObjSets();
                }
                textBox2.Text = "Initializing UI.wz...";
                Application.DoEvents();
                Program.WzManager.LoadSunFile("ui");
            }
        }

        private static readonly string[] commonMaplePaths = new string[] { @"C:\Nexon\MapleStory", @"C:\Program Files\WIZET\MapleStory", @"C:\MapleStory" };

        private void Initialization_Load(object sender, EventArgs e)
        {
            versionBox.SelectedIndex = 0;
            try
            {
                string[] paths = ApplicationSettings.MapleFolder.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                foreach (string x in paths)
                {
                    pathBox.Items.Add(x);
                }
                foreach (string path in commonMaplePaths)
                {
                    if (Directory.Exists(path))
                    {
                        pathBox.Items.Add(path);
                    }
                }
                if (pathBox.Items.Count == 0)
                    pathBox.Items.Add("Select Maple Folder");
            }
            catch
            {
            }
            versionBox.SelectedIndex = ApplicationSettings.MapleVersionIndex;
            if (pathBox.Items.Count < ApplicationSettings.MapleFolderIndex + 1)
            {
                pathBox.SelectedIndex = pathBox.Items.Count - 1;
            }
            else
            {
                pathBox.SelectedIndex = ApplicationSettings.MapleFolderIndex;
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
                SunImage mapImage = (SunImage)Program.WzManager["map"]["Map"][mapcat][mapid + ".img"];
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
                button1_Click(null, null);
            }
            else if (e.KeyCode == Keys.Escape)
            {
                Close();
            }
        }
    }
}