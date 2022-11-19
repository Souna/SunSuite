using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Windows.Threading;
using SunFileManager.Comparer;
using SunLibrary.SunFileLib.Structure;

namespace SunFileManager
{
    public class FileManager
    {
        public static TreeViewNodeSorter sorter = new TreeViewNodeSorter();
        public List<SunFile> sunFiles = new List<SunFile>();
        private frmFileManager mainForm = null;

        public FileManager(Form form)
        {
            mainForm = form as frmFileManager;
        }

        private bool OpenSunFile(string path, out SunFile file)
        {
            try
            {
                SunFile f = new SunFile(path);

                lock (sunFiles)
                {
                    sunFiles.Add(f);
                }

                string error = string.Empty;
                bool success = f.ParseSunFile(out error, Program.UserSettings.AutoParseImages);

                file = f;
                return success;
            }
            catch (Exception e)
            {
                file = null;
                MessageBox.Show("Error loading " + Path.GetFileName(path));
                return false;
            }
        }

        public SunFile LoadSunFile(string path)
        {
            SunFile file;

            if (!OpenSunFile(path, out file))
            {
                return null;
            }
            return file;
        }

        public void AddLoadedSunFileToTreeView(SunFile file, Dispatcher dispatcher, List<string>? expansionState)
        {
            SunNode sunNode = new SunNode(file);

            if (dispatcher != null)
            {
                dispatcher.BeginInvoke((Action)(() =>
                {
                    mainForm.sunTreeView.BeginUpdate();
                    mainForm.sunTreeView.Nodes.Add(sunNode);
                    SortNodesRecursively(sunNode);
                    if (expansionState != null)
                        mainForm.sunTreeView.Nodes.SetExpansionState(expansionState);
                    mainForm.sunTreeView.EndUpdate();
                }));
            }
            else
            {
                mainForm.sunTreeView.BeginUpdate();
                mainForm.sunTreeView.Nodes.Add(sunNode);
                SortNodesRecursively(sunNode);
                if (expansionState != null)
                    mainForm.sunTreeView.Nodes.SetExpansionState(expansionState);
                mainForm.sunTreeView.EndUpdate();
            }
        }

        /// <summary>
        /// Save a SunFile to disk.
        /// </summary>
        public void SaveToDisk(ref SunNode node)
        {
            SunFile SaveSunFile = (SunFile)node.Tag;
            var savedExpansionState = mainForm.sunTreeView.Nodes.GetExpansionState();

            SaveFileDialog sfd = new SaveFileDialog()
            {
                Title = "Save SunFile",
                FileName = node.Text,
                Filter = "SunFile|*.sun"
            };

            if (sfd.ShowDialog() != DialogResult.OK)
                return;

            //If saving a file that exists already, creates a new temp file and replaces current file with that
            if (SaveSunFile.FilePath != null && SaveSunFile.FilePath.ToLower() == sfd.FileName.ToLower())
            {
                SaveSunFile.SaveToDisk(sfd.FileName + "$tmp");
                node.DeleteNode();
                File.Delete(sfd.FileName);
                File.Move(sfd.FileName + "$tmp", sfd.FileName);
            }
            else // We're making a new file, or the name of the file was changed in the SaveFileDialog
            {
                SaveSunFile.SaveToDisk(sfd.FileName);
                node.DeleteNode();
            }

            //Now reload the file

            SunFile loadedSunFile = LoadSunFile(sfd.FileName);
            if (loadedSunFile != null)
                AddLoadedSunFileToTreeView(loadedSunFile, Dispatcher.CurrentDispatcher, savedExpansionState);
        }

        /// <summary>
        /// Reloads a file that already exists on the treeview.
        /// </summary>
        public void ReloadSunFile(SunFile file, Dispatcher currentDispatcher = null)
        {
            var savedExpansionState = mainForm.sunTreeView.Nodes.GetExpansionState();
            string path = file.FilePath;
            if (currentDispatcher != null)
            {
                currentDispatcher.BeginInvoke((Action)(() =>
                {
                    UnloadSunFile(file);
                }));
            }
            else
                UnloadSunFile(file);

            SunFile loadedSunFile = LoadSunFile(path);

            if (loadedSunFile != null)
                mainForm.manager.AddLoadedSunFileToTreeView(loadedSunFile, currentDispatcher, savedExpansionState);
        }

        public void UnloadSunFile(SunFile file)
        {
            lock (sunFiles)
            {
                try
                {
                    ((SunNode)mainForm.sunTreeView.SelectedNode).DeleteNode();
                    sunFiles.Remove(file);
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    GC.Collect();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        public void UnloadAllSunFiles()
        {
            IReadOnlyCollection<SunFile> fileList = new List<SunFile>(sunFiles);

            foreach (SunFile file in fileList)
            {
                UnloadSunFile(file);
            }
        }

        public void SortNodesRecursively(SunNode parent)
        {
            parent.TreeView.TreeViewNodeSorter = sorter;
            parent.TreeView.Sort();
        }
    }
}