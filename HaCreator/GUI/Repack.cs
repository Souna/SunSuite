﻿/* Copyright (C) 2015 haha01haha01

* This Source Code Form is subject to the terms of the Mozilla Public
* License, v. 2.0. If a copy of the MPL was not distributed with this
* file, You can obtain one at http://mozilla.org/MPL/2.0/. */

using SunLibrary.SunFileLib.Structure;
using SunLibrary.SunFileLib.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace HaCreator.GUI
{
    public partial class Repack : Form
    {
        private List<SunFile> toRepack = new List<SunFile>();

        public Repack()
        {
            InitializeComponent();
            infoLabel.Text = "Files to repack:\r\n";
            foreach (SunFile sunf in Program.SfManager.SunFiles.Values)
            {
                if (Program.SfManager.SunFilesUpdated[sunf])
                {
                    toRepack.Add(sunf);
                    infoLabel.Text += sunf.Name + "\r\n";
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;

            Thread t = new Thread(new ThreadStart(RepackerThread));
            t.Start();
        }

        private void ShowErrorMessage(string data)
        {
            MessageBox.Show(data, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void ChangeRepackState(string state)
        {
            stateLabel.Text = state;
        }

        private void FinishSuccess()
        {
            MessageBox.Show("Repacked successfully, press OK to restart.");
            Program.Restarting = true;
            Close();
        }

        private void ShowErrorMessageThreadSafe(Exception e, string saveStage)
        {
            Invoke((Action)delegate
            {
                ChangeRepackState("ERROR While saving " + saveStage + ", aborted.");
                button1.Enabled = true;
                ShowErrorMessage("There has been an error while saving, it is likely because you do not have permissions to the destination folder or the files are in use.\r\n\r\nPress OK to see the error details.");
                ShowErrorMessage(e.Message + "\r\n" + e.StackTrace);
            });
        }

        private void RepackerThread()
        {
            Invoke((Action)delegate { ChangeRepackState("Deleting old backups..."); });
            // Prepare directories
            string rootDir = Path.Combine(Program.SfManager.BaseDir, "HaCreator");
            string backupDir = Path.Combine(rootDir, "Backup");
            string orgBackupDir = Path.Combine(rootDir, "Original");
            string XMLDir = Path.Combine(rootDir, "XML");
            try
            {
                if (!Directory.Exists(backupDir))
                    Directory.CreateDirectory(backupDir);
                if (!Directory.Exists(orgBackupDir))
                    Directory.CreateDirectory(orgBackupDir);
                if (!Directory.Exists(XMLDir))
                    Directory.CreateDirectory(XMLDir);
                foreach (FileInfo fi in new DirectoryInfo(backupDir).GetFiles())
                {
                    fi.Delete();
                }
            }
            catch (Exception e)
            {
                ShowErrorMessageThreadSafe(e, "backup files");
                return;
            }

            // Save XMLs
            // We have to save XMLs first, otherwise the SunImages will already be disposed when we reach this code
            //Invoke((Action)delegate { ChangeRepackState("Saving XMLs..."); });
            //foreach (SunImage img in Program.SfManager.updatedImages)
            //{
            //    try
            //    {
            //        string xmlPath = Path.Combine(XMLDir, img.FullPath);
            //        string xmlPathDir = Path.GetDirectoryName(xmlPath);
            //        if (!Directory.Exists(xmlPathDir))
            //            Directory.CreateDirectory(xmlPathDir);
            //        SunClassicXmlSerializer xmlSer = new SunClassicXmlSerializer(0, LineBreak.None, false);
            //        xmlSer.SerializeImage(img, xmlPath);
            //    }
            //    catch (Exception e)
            //    {
            //        ShowErrorMessageThreadSafe(e, "XMLs");
            //        return;
            //    }
            //}

            // Save WZ Files
            foreach (SunFile sf in toRepack)
            {
                Invoke((Action)delegate { ChangeRepackState("Saving " + sf.Name + "..."); });
                string orgFile = sf.FilePath;
                string tmpFile = orgFile + "$tmp";
                try
                {
                    sf.SaveToDisk(tmpFile);
                    sf.Dispose();
                    string buPath = Path.Combine(orgBackupDir, Path.GetFileName(orgFile));
                    // Try to backup to /Originals/ First, if there is already a file there, we are not original, so just backup to /Backup/
                    if (File.Exists(buPath))
                    {
                        buPath = Path.Combine(backupDir, Path.GetFileName(orgFile));
                    }
                    File.Move(orgFile, buPath);
                    File.Move(tmpFile, orgFile);
                }
                catch (Exception e)
                {
                    ShowErrorMessageThreadSafe(e, sf.Name);
                    return;
                }
            }

            Invoke((Action)delegate { ChangeRepackState("Finished"); FinishSuccess(); });
        }

        private void Repack_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!button1.Enabled && !Program.Restarting)
            { //Do not let the user close the form while saving
                e.Cancel = true;
            }
        }

        private void Repack_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Close();
            }
            else if (e.KeyCode == Keys.Enter)
            {
                button1_Click(null, null);
            }
        }
    }
}