using SunFileManager.Comparer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Windows.Threading;

namespace SunFileManager.SunFileLib
{
    public class FileManager
    {
        public static TreeViewNodeSorter sorter = new TreeViewNodeSorter();
        public List<SunFile> sunFiles = new List<SunFile>();
        private frmFileManager mainform = null;

        public FileManager(Form form)
        {
            mainform = form as frmFileManager;
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
                //bool success = f.ParseSunFile(out error);

                //if (!success ||!File.Exists(path))
                //{
                //    file = null;
                //    return false;
                //}

                file = f;
                return true;
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

        public void AddLoadedSunFileToTreeView(SunFile file, Dispatcher dispatcher)
        {
            //Make it so this can edit treeview on mainform
            SunNode sunNode = new SunNode(file);

            if (dispatcher != null)
            {
                dispatcher.BeginInvoke((Action)(() =>
                {
                    mainform.sunTreeView.BeginUpdate();
                    mainform.sunTreeView.Nodes.Add(sunNode);
                    SortNodesRecursively(sunNode);
                    mainform.sunTreeView.EndUpdate();
                }));
            }
            else
            {
                mainform.sunTreeView.BeginUpdate();
                mainform.sunTreeView.Nodes.Add(sunNode);
                SortNodesRecursively(sunNode);
                mainform.sunTreeView.EndUpdate();
            }
        }

        /// <summary>
        /// Save a SunFile to disk.
        /// </summary>
        public static void SaveToDisk(ref SunNode node)
        {
            SunFile SaveSunFile = (SunFile)node.Tag;

            SaveFileDialog sfd = new SaveFileDialog()
            {
                Title = "Save SunFile",
                FileName = node.Text,
                Filter = "SunFile|*.sun"
            };

            if (sfd.ShowDialog() != DialogResult.OK)
                return;

            /*Maybe might not even need these if/else blocks*/
            //  If saving and not changing the save destination or filename before it's saved
            if (SaveSunFile.FilePath != null && SaveSunFile.FilePath.ToLower() == sfd.FileName.ToLower())
            {
            }
            else
            {
                //  Changed something
            }

            SaveSunFile.SaveToDisk(sfd.FileName);
        }

        public void UnloadSunFile(SunFile file)
        {
            //lock(sunFiles)
            //{
            //    if (sunFiles.Contains(file))
            //    {
            //        mainform.sunTreeView.SelectedNode.Remove();
            //        sunFiles.Remove(file);
            //    }
            //}
            mainform.sunTreeView.SelectedNode.Remove();
            if (sunFiles.Contains(file)) sunFiles.Remove(file);
        }

        public void ReloadSunFile(SunFile file, Dispatcher currentDispatcher = null)
        {
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
                mainform.manager.AddLoadedSunFileToTreeView(file, currentDispatcher);
        }

        public void SortNodesRecursively(SunNode parent)
        {
            parent.TreeView.TreeViewNodeSorter = sorter;
            parent.TreeView.Sort();
        }
    }
}