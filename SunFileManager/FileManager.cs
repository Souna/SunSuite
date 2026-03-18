using SunFileManager.Comparer;
using SunLibrary.SunFileLib.Structure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Threading;

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

        public FileManager()
        {
        }

        private async Task<SunFile> OpenSunFileAsync(string path)
        {
            SunFile f = new SunFile(path);
            lock (sunFiles)
            {
                sunFiles.Add(f);
            }

            try
            {
                bool success = await Task.Run(() =>
                {
                    string ignored = string.Empty;
                    return f.ParseSunFile(out ignored, Program.UserSettings.AutoParseImages);
                });

                if (!success)
                {
                    lock (sunFiles) { sunFiles.Remove(f); }
                    return null;
                }

                return f;
            }
            catch (Exception e)
            {
                lock (sunFiles) { sunFiles.Remove(f); }
                MessageBox.Show("Error loading " + Path.GetFileName(path) + "\n" + e.Message);
                return null;
            }
        }

        public async Task<SunFile> LoadSunFileAsync(string path)
        {
            // Prevent duplicate files (case-insensitive)
            if (sunFiles.Exists(f => string.Equals(f.FilePath, path, StringComparison.OrdinalIgnoreCase)))
            {
                MessageBox.Show($"File '{Path.GetFileName(path)}' is already open.", "Duplicate File", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return null;
            }

            return await OpenSunFileAsync(path);
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
        public async Task SaveToDiskAsync(SunNode node)
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

            try
            {
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
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error saving file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //Now reload the file
            SunFile loadedSunFile = await LoadSunFileAsync(sfd.FileName);
            if (loadedSunFile != null)
                AddLoadedSunFileToTreeView(loadedSunFile, Dispatcher.CurrentDispatcher, savedExpansionState);
        }

        /// <summary>
        /// Reloads a file that already exists on the treeview.
        /// </summary>
        public async Task ReloadSunFileAsync(SunFile file, Dispatcher currentDispatcher = null)
        {
            var savedExpansionState = mainForm.sunTreeView.Nodes.GetExpansionState();
            string path = file.FilePath;
            if (currentDispatcher != null)
                currentDispatcher.Invoke(() => UnloadSunFile(file));
            else
                UnloadSunFile(file);

            SunFile loadedSunFile = await LoadSunFileAsync(path);

            if (loadedSunFile != null)
                frmFileManager.manager.AddLoadedSunFileToTreeView(loadedSunFile, currentDispatcher, savedExpansionState);
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