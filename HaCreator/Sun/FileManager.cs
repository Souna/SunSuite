/* Copyright (C) 2015 haha01haha01

* This Source Code Form is subject to the terms of the Mozilla Public
* License, v. 2.0. If a copy of the MPL was not distributed with this
* file, You can obtain one at http://mozilla.org/MPL/2.0/. */

using HaCreator.MapEditor.Info;
using SunLibrary.SunFileLib.Properties;
using SunLibrary.SunFileLib.Structure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace HaCreator.Wz
{
    public class FileManager
    {
        private string baseDir;
        public Dictionary<string, SunFile> SunFiles = new Dictionary<string, SunFile>();
        public Dictionary<SunFile, bool> SunFilesUpdated = new Dictionary<SunFile, bool>();
        public HashSet<SunImage> updatedImages = new HashSet<SunImage>();
        public Dictionary<string, SunFileMainDirectory> sunDirs = new Dictionary<string, SunFileMainDirectory>();

        public FileManager(string directory)
        {
            baseDir = directory;
        }

        private string Capitalize(string x)
        {
            if (x.Length > 0 && char.IsLower(x[0]))
            {
                return new string(new char[] { char.ToUpper(x[0]) }) + x.Substring(1);
            }
            else
            {
                return x;
            }
        }

        public bool LoadSunFile(string name)
        {
            try
            {
                SunFile sunf = new SunFile(Path.Combine(baseDir, Capitalize(name) + ".sun"));

                string parseErrorMessage = string.Empty;
                bool parseSuccess = sunf.ParseSunFile(out parseErrorMessage, false);

                name = name.ToLower();
                SunFiles[name] = sunf;
                SunFilesUpdated[sunf] = false;
                sunDirs[name] = new SunFileMainDirectory(sunf);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool LoadDataSunFile(string name)
        {
            try
            {
                SunFile sunf = new SunFile(Path.Combine(baseDir, Capitalize(name) + ".sun"));

                string parseErrorMessage = string.Empty;
                bool parseSuccess = sunf.ParseSunFile(out parseErrorMessage, false);

                name = name.ToLower();
                SunFiles[name] = sunf;
                SunFilesUpdated[sunf] = false;
                sunDirs[name] = new SunFileMainDirectory(sunf);
                foreach (SunDirectory mainDir in sunf.SunDirectory.SubDirectories)
                {
                    sunDirs[mainDir.Name.ToLower()] = new SunFileMainDirectory(sunf, mainDir);
                }
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("Error initializing " + name + ".sun (" + e.Message + ").\r\nCheck that the directory is valid and the file is not in use.");
                return false;
            }
        }

        public void SetUpdated(string name, SunImage img)
        {
            img.Changed = true;
            updatedImages.Add(img);
            SunFilesUpdated[GetMainDirectoryByName(name).File] = true;
        }

        public SunFileMainDirectory GetMainDirectoryByName(string name)
        {
            return sunDirs[name.ToLower()];
        }

        public SunDirectory this[string name]
        {
            get { return sunDirs.ContainsKey(name.ToLower()) ? sunDirs[name.ToLower()].MainDir : null; }    //really not very useful to return null in this case
        }

        public SunDirectory String
        {
            get { return GetMainDirectoryByName("string").MainDir; }
        }

        public string BaseDir
        {
            get { return baseDir; }
        }

        public void ExtractMobFile()
        {
            SunImage mobImage = (SunImage)String["Mob.img"];
            if (!mobImage.Parsed) mobImage.ParseImage();
            foreach (SunSubProperty mob in mobImage.SunProperties)
            {
                SunStringProperty nameProp = (SunStringProperty)mob["name"];
                string name = nameProp == null ? "" : nameProp.Value;
                Program.InfoManager.Mobs.Add(SunInfoTools.AddLeadingZeros(mob.Name, 7), name);
            }
            /*   SunImage mobImage2 = (SunImage)String["Mob001.img"];
               if (mobImage2 != null)
               {
                   if (!mobImage2.Parsed) mobImage2.ParseImage();
                   foreach (SunSubProperty mob in mobImage2.SunProperties)
                   {
                       SunStringProperty nameProp = (SunStringProperty)mob["name"];
                       string name = nameProp == null ? "" : nameProp.Value;
                       Program.InfoManager.Mobs.Add(SunInfoTools.AddLeadingZeros(mob.Name, 7), name);
                   }
               }
               SunImage mobImage3 = (SunImage)String["Mob2.img"];
               if (mobImage3 != null)
               {
                   if (!mobImage3.Parsed) mobImage3.ParseImage();
                   foreach (SunSubProperty mob in mobImage3.SunProperties)
                   {
                       SunStringProperty nameProp = (SunStringProperty)mob["name"];
                       string name = nameProp == null ? "" : nameProp.Value;
                       Program.InfoManager.Mobs.Add(SunInfoTools.AddLeadingZeros(mob.Name, 7), name);
                   }
               }*/
        }

        public void ExtractNpcFile()
        {
            SunImage npcImage = (SunImage)String["Npc.img"];
            if (!npcImage.Parsed) npcImage.ParseImage();
            foreach (SunSubProperty npc in npcImage.SunProperties)
            {
                SunStringProperty nameProp = (SunStringProperty)npc["name"];
                string name = nameProp == null ? "" : nameProp.Value;
                Program.InfoManager.NPCs.Add(SunInfoTools.AddLeadingZeros(npc.Name, 7), name);
            }
        }

        public void ExtractReactorFile()
        {
            foreach (SunImage reactorImage in this["reactor"].SunImages)
            {
                ReactorInfo reactor = ReactorInfo.Load(reactorImage);
                Program.InfoManager.Reactors[reactor.ID] = reactor;
            }
        }

        public void ExtractSoundFile()
        {
            foreach (SunImage soundImage in this["sound"].SunImages)
            {
                if (!soundImage.Name.ToLower().Contains("bgm")) continue;
                if (!soundImage.Parsed) soundImage.ParseImage();
                try
                {
                    foreach (SunSoundProperty bgm in soundImage.SunProperties)
                    {
                        Program.InfoManager.BGMs[SunInfoTools.RemoveExtension(soundImage.Name) + @"/" + bgm.Name] = bgm;
                    }
                }
                catch (Exception e) { continue; }
            }
        }

        public void ExtractMapMarks()
        {
            SunImage mapHelper = (SunImage)this["map"]["MapHelper.img"];
            foreach (SunCanvasProperty mark in mapHelper["mark"].SunProperties)
                Program.InfoManager.MapMarks[mark.Name] = mark.PNG.GetPNG(false);
        }

        public void ExtractTileSets()
        {
            SunDirectory tileParent = (SunDirectory)this["map"]["Tile"];
            foreach (SunImage tileset in tileParent.SunImages)
                Program.InfoManager.TileSets[SunInfoTools.RemoveExtension(tileset.Name)] = tileset;
        }

        public void ExtractObjSets()
        {
            SunDirectory objParent1 = (SunDirectory)this["map"]["Obj"];
            if (objParent1 != null)
            {
                foreach (SunImage objset in objParent1.SunImages)
                    Program.InfoManager.ObjectSets[SunInfoTools.RemoveExtension(objset.Name)] = objset;
            }

            if (this.SunFiles.ContainsKey("map001"))
            {
                SunDirectory objParent2 = (SunDirectory)this["map001"]["Obj"];
                if (objParent2 != null)
                {
                    foreach (SunImage objset in objParent2.SunImages)
                        Program.InfoManager.ObjectSets[SunInfoTools.RemoveExtension(objset.Name)] = objset;
                }
            }
            if (this.SunFiles.ContainsKey("map2"))
            {
                SunDirectory objParent3 = (SunDirectory)this["map2"]["Obj"];
                if (objParent3 != null)
                {
                    foreach (SunImage objset in objParent3.SunImages)
                        Program.InfoManager.ObjectSets[SunInfoTools.RemoveExtension(objset.Name)] = objset;
                }
            }
        }

        //this handling sucks but nexon naming is not consistent enough to handle much better idk
        public void ExtractBackgroundSets()
        {
            SunDirectory bgParent1 = (SunDirectory)this["map"]["Back"];
            if (bgParent1 != null)
            {
                foreach (SunImage bgset in bgParent1.SunImages)
                    Program.InfoManager.BackgroundSets[SunInfoTools.RemoveExtension(bgset.Name)] = bgset;
            }
            if (this.SunFiles.ContainsKey("map001"))
            {
                SunDirectory bgParent2 = (SunDirectory)this["map001"]["Back"];
                if (bgParent2 != null)
                {
                    foreach (SunImage bgset in bgParent2.SunImages)
                        Program.InfoManager.BackgroundSets[SunInfoTools.RemoveExtension(bgset.Name)] = bgset;
                }
            }
            if (this.SunFiles.ContainsKey("map2"))
            {
                SunDirectory bgParent3 = (SunDirectory)this["map2"]["Back"];
                if (bgParent3 != null)
                {
                    foreach (SunImage bgset in bgParent3.SunImages)
                        Program.InfoManager.BackgroundSets[SunInfoTools.RemoveExtension(bgset.Name)] = bgset;
                }
            }
        }

        public void ExtractMaps()
        {
            SunImage mapStringsParent = (SunImage)String["Map.img"];
            if (!mapStringsParent.Parsed) mapStringsParent.ParseImage();
            foreach (SunSubProperty mapCat in mapStringsParent.SunProperties)
            {
                foreach (SunSubProperty map in mapCat.SunProperties)
                {
                    SunStringProperty mapName = (SunStringProperty)map["mapName"];
                    string id;
                    if (map.Name.Length == 9)
                        id = map.Name;
                    else
                        id = SunInfoTools.AddLeadingZeros(map.Name, 9);

                    if (mapName == null)
                        Program.InfoManager.Maps[id] = "";
                    else
                        Program.InfoManager.Maps[id] = mapName.Value;
                }
            }
        }

        public void ExtractPortals()
        {
            SunSubProperty portalParent = (SunSubProperty)this["map"]["MapHelper.img"]["portal"];
            SunSubProperty editorParent = (SunSubProperty)portalParent["editor"];
            for (int i = 0; i < editorParent.SunProperties.Count; i++)
            {
                SunCanvasProperty portal = (SunCanvasProperty)editorParent.SunProperties[i];
                Program.InfoManager.PortalTypeById.Add(portal.Name);
                PortalInfo.Load(portal);
            }
            SunSubProperty gameParent = (SunSubProperty)portalParent["game"]["pv"];
            foreach (SunProperty portal in gameParent.SunProperties)
            {
                if (portal.SunProperties[0] is SunSubProperty)
                {
                    Dictionary<string, Bitmap> images = new Dictionary<string, Bitmap>();
                    Bitmap defaultImage = null;
                    foreach (SunSubProperty image in portal.SunProperties)
                    {
                        //SunSubProperty portalContinue = (SunSubProperty)image["portalContinue"];
                        //if (portalContinue == null) continue;
                        Bitmap portalImage = image["0"].GetBitmap();
                        if (image.Name == "default")
                            defaultImage = portalImage;
                        else
                            images.Add(image.Name, portalImage);
                    }
                    Program.InfoManager.GamePortals.Add(portal.Name, new PortalGameImageInfo(defaultImage, images));
                }
                else if (portal.SunProperties[0] is SunCanvasProperty)
                {
                    Dictionary<string, Bitmap> images = new Dictionary<string, Bitmap>();
                    Bitmap defaultImage = null;
                    try
                    {
                        foreach (SunCanvasProperty image in portal.SunProperties)
                        {
                            //SunSubProperty portalContinue = (SunSubProperty)image["portalContinue"];
                            //if (portalContinue == null) continue;
                            Bitmap portalImage = image.GetBitmap();
                            defaultImage = portalImage;
                            images.Add(image.Name, portalImage);
                        }
                        Program.InfoManager.GamePortals.Add(portal.Name, new PortalGameImageInfo(defaultImage, images));
                    }
                    catch (InvalidCastException) { continue; } //nexon likes to toss ints in here zType etc
                }
            }

            for (int i = 0; i < Program.InfoManager.PortalTypeById.Count; i++)
            {
                Program.InfoManager.PortalIdByType[Program.InfoManager.PortalTypeById[i]] = i;
            }
        }

        /*        public void ExtractItems()
                {
                    SunImage consImage = (SunImage)String["Consume.img"];
                    if (!consImage.Parsed) consImage.ParseImage();
                    foreach (SunSubProperty item in consImage.SunProperties)
                    {
                        SunStringProperty nameProp = (SunStringProperty)item["name"];
                        string name = nameProp == null ? "" : nameProp.Value;
                        Program.InfoManager.Items.Add(SunInfoTools.AddLeadingZeros(item.Name, 7), name);
                    }
                }*/
    }
}