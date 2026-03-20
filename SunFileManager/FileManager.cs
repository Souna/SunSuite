using SunLibrary.SunFileLib.Structure;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace SunFileManager
{
    public class FileManager
    {
        public List<SunFile> sunFiles = new List<SunFile>();
        private GUI.MainWindow mainWindow = null;

        public FileManager(GUI.MainWindow window)
        {
            mainWindow = window;
        }

        public FileManager() { }

        // ── Loading ───────────────────────────────────────────────────────────────
        private async Task<SunFile> OpenSunFileAsync(string path)
        {
            SunFile f = new SunFile(path);
            lock (sunFiles) { sunFiles.Add(f); }

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
            if (sunFiles.Exists(f => string.Equals(f.FilePath, path, StringComparison.OrdinalIgnoreCase)))
            {
                MessageBox.Show($"File '{Path.GetFileName(path)}' is already open.", "Duplicate File",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return null;
            }
            return await OpenSunFileAsync(path);
        }

        public void AddLoadedSunFileToTreeView(SunFile file, Dispatcher dispatcher, List<string> expansionState)
        {
            SunNode sunNode = new SunNode(file);
            sunNode.SortChildrenRecursively();

            Action addAction = () =>
            {
                mainWindow.TreeNodes.Add(sunNode);
                if (expansionState != null)
                    mainWindow.TreeNodes.SetExpansionState(expansionState);
            };

            if (dispatcher != null && !dispatcher.CheckAccess())
                dispatcher.BeginInvoke(addAction);
            else
                addAction();
        }

        // ── Saving ────────────────────────────────────────────────────────────────
        public async Task SaveToDiskAsync(SunNode node)
        {
            SunFile saveSunFile = (SunFile)node.Tag;
            var savedExpansionState = mainWindow.TreeNodes.GetExpansionState();

            var sfd = new Microsoft.Win32.SaveFileDialog
            {
                Title = "Save SunFile",
                FileName = node.Name,
                Filter = "SunFile|*.sun"
            };

            if (sfd.ShowDialog() != true) return;

            try
            {
                if (saveSunFile.FilePath != null &&
                    saveSunFile.FilePath.Equals(sfd.FileName, StringComparison.OrdinalIgnoreCase))
                {
                    saveSunFile.SaveToDisk(sfd.FileName + "$tmp");
                    node.DeleteNode();
                    File.Delete(sfd.FileName);
                    File.Move(sfd.FileName + "$tmp", sfd.FileName);
                }
                else
                {
                    saveSunFile.SaveToDisk(sfd.FileName);
                    node.DeleteNode();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error saving file", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            SunFile loadedSunFile = await LoadSunFileAsync(sfd.FileName);
            if (loadedSunFile != null)
                AddLoadedSunFileToTreeView(loadedSunFile, Dispatcher.CurrentDispatcher, savedExpansionState);
        }

        // ── Reloading ─────────────────────────────────────────────────────────────
        public async Task ReloadSunFileAsync(SunFile file, SunNode nodeToRemove, Dispatcher currentDispatcher = null)
        {
            var savedExpansionState = mainWindow.TreeNodes.GetExpansionState();
            string path = file.FilePath;

            if (currentDispatcher != null)
                currentDispatcher.Invoke(() => UnloadSunFile(file, nodeToRemove));
            else
                UnloadSunFile(file, nodeToRemove);

            SunFile loadedSunFile = await LoadSunFileAsync(path);
            if (loadedSunFile != null)
                GUI.MainWindow.manager.AddLoadedSunFileToTreeView(loadedSunFile, currentDispatcher, savedExpansionState);
        }

        // ── Unloading ─────────────────────────────────────────────────────────────
        public void UnloadSunFile(SunFile file, SunNode nodeToRemove = null)
        {
            lock (sunFiles)
            {
                try
                {
                    SunNode node = nodeToRemove ?? mainWindow.GetSelectedSunNode();
                    node?.DeleteNode();
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
                UnloadSunFile(file);
        }
    }
}
