using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Windows.Threading;
using SunFileManager.GUI;
using SunFileManager.GUI.Input;
using SunFileManager.GUI.Input.Forms;
using SunFileManager.SunFileLib;
using SunFileManager.SunFileLib.Properties;
using SunFileManager.SunFileLib.Structure;

namespace SunFileManager
{
    public partial class frmFileManager : Form
    {
        public static string DefaultPath = "C:\\Users\\SOUND\\Desktop\\New .Sun Files";
        public FileManager manager = null;
        public SunContextMenuManager contextMenuManager = null;
        public bool AnimateGifs = false;
        public Size defaultTextBoxSize = new Size(205, 29);

        public frmFileManager()
        {
            InitializeComponent();
            chkAnimateGif.Visible = false;
            contextMenuManager = new SunContextMenuManager(this);
            lblPropertyName.Visible = false;
            lblValue.Visible = false;
            txtPropertyName.Visible = false;
            txtPropertyValue.Visible = false;
            mainfrm_panning_PictureBox.Visible = false;
            manager = new FileManager(this);
        }

        #region Toolstrip

        /// <summary>
        /// Display SunFile creation form.
        /// </summary>
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmNewFile newfile = new frmNewFile(this);
            newfile.ShowDialog();
        }

        /// <summary>
        /// Click event for Toolstrip add directory button.
        /// </summary>
        private void sunDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddSunDirectoryToSelectedNode(sunTreeView.SelectedNode, null);
        }

        #endregion Toolstrip

        #region Loading, Unloading & Saving

        /// <summary>
        /// Load an existing SunFile.
        /// </summary>
        public void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Dispatcher dispatcher = Dispatcher.CurrentDispatcher;
            OpenFileDialog ofd = new OpenFileDialog()
            {
                Title = "Choose SunFiles",
                Filter = "SunFile|*.sun",
                Multiselect = true
            };

            if (ofd.ShowDialog() != DialogResult.OK)
                return;

            // List to hold all selected SunFile paths.
            List<string> sunFilePathsToLoad = new List<string>();

            // Fill list with selected filepaths.
            foreach (string path in ofd.FileNames)
                sunFilePathsToLoad.Add(path.ToLower());

            List<SunFile> loadedSunFiles = new List<SunFile>();

            foreach (string filepath in sunFilePathsToLoad)
            {
                SunFile f = manager.LoadSunFile(filepath);
                if (f == null)
                {
                    MessageBox.Show("Error");
                }
                else
                {
                    lock (loadedSunFiles)
                    {
                        loadedSunFiles.Add(f);
                    }
                }
            }

            foreach (SunFile sunFile in loadedSunFiles)
            {
                // Add to tree.
                manager.AddLoadedSunFileToTreeView(sunFile, dispatcher);
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFile();
        }

        public void SaveFile()
        {
            SunNode node;

            if (sunTreeView.SelectedNode == null)
            {
                if (sunTreeView.Nodes.Count > 0)
                    node = (SunNode)sunTreeView.Nodes[0]; // If there's only 1 node, node = just the base node.
                else
                {   // No nodes.
                    return;
                }
            }
            else
            {
                if (sunTreeView.SelectedNode.Tag is SunFile)
                    node = (SunNode)sunTreeView.SelectedNode;
                else
                {
                    // If selected node is something other than a top level SunFile,
                    // set node = to the top level SunFile node.
                    node = ((SunNode)sunTreeView.SelectedNode).TopLevelNode;
                }
            }
            manager.SaveToDisk(ref node);
        }

        /// <summary>
        /// Removes selected nodes from the TreeView.
        /// <br>All child nodes, if any, are gone too.</br>
        /// </summary>
        public void RemoveSelectedNodes()
        {
            SunNode node = (SunNode)sunTreeView.SelectedNode;
            //right now only able to select one node
            node.DeleteNode();
        }

        /// <summary>
        /// Parse the data tree selected item on double clicking, or copy pasting into it.
        /// </summary>
        /// <param name="selectedNode"></param>
        private static void ParseOnTreeViewSelectedItem(SunNode selectedNode, bool expandDataTree = true)
        {
            SunImage img = (SunImage)selectedNode.Tag;

            if (!img.Parsed)
                img.ParseImage();
            selectedNode.Reparse();
            if (expandDataTree)
            {
                selectedNode.Expand();
            }
        }

        #endregion Loading, Unloading & Saving

        #region Treeview Node Manipulation

        /// <summary>
        /// Rename the selected node.
        /// </summary>
        public void RenameSelectedNode()
        {
            string namestring = "";
            SunNode node = (SunNode)sunTreeView.SelectedNode;
            switch (((SunObject)node.Tag).ObjectType)
            {
                case SunObjectType.File:
                    namestring = "Rename File";
                    break;

                case SunObjectType.Image:
                    namestring = "Rename Image";
                    break;

                case SunObjectType.Directory:
                    namestring = "Rename Directory";
                    break;

                case SunObjectType.Property:
                    namestring = "Rename Property";
                    break;

                default:
                    break;
            }

            if (!frmNameInputBox.Show(namestring, out string newName, node.Text))
                return;
            node.Rename(newName);
            // Update property name textbox without having to 'refresh' the node.
            if (node.Tag is SunProperty && txtPropertyName.Visible) txtPropertyName.Text = newName;
        }

        #endregion Treeview Node Manipulation

        #region Adding Directories & Properties

        /// <summary>
        /// Creates a new Directory under the selected node.
        /// <br>A directory may be created as a top-level directory underneath the SunFile.</br>
        /// </summary>
        public void AddSunDirectoryToSelectedNode(TreeNode selectedNode, string name)
        {
            bool added = false;
            if (selectedNode == null) return;
            SunObject obj = (SunObject)selectedNode.Tag;

            if (!(selectedNode.Tag is SunDirectory) && !(selectedNode.Tag is SunFile))
            {
                MessageBox.Show("Tag is " + selectedNode.Tag.ToString());
                return;
            }

            string dirName = name;
            if (name == string.Empty || name == null)
            {
                if (!frmNameInputBox.Show("Add Directory", out dirName))
                    return;
            }

            if (obj is SunFile sunFileParent) // Selected parent node.
            {
                if (sunFileParent.SunDirectory == null)
                    sunFileParent.SunDirectory = new SunDirectory(sunFileParent.Name, sunFileParent);   // If master sundirectory is null make a new one
                ((SunNode)selectedNode).AddObject(new SunDirectory(dirName, sunFileParent.SunDirectory));
                added = true;
            }
            else if (obj is SunDirectory sunDirectoryParent)
            {
                ((SunNode)selectedNode).AddObject(new SunDirectory(dirName, sunDirectoryParent));
                added = true;
            }
            if (!added)
            {
                MessageBox.Show("Error adding directory.");
            }
        }

        public void AddSunImageToSelectedNode(TreeNode targetNode, string name)
        {
            if (targetNode == null) return;

            if (!(targetNode.Tag is SunDirectory) && !(targetNode.Tag is SunFile))
            {
                MessageBox.Show("Tag is " + targetNode.Tag.ToString());
                return;
            }

            string imgName = name;
            if (name == string.Empty || name == null)
            {
                if (!frmNameInputBox.Show("Add Image", out imgName))
                    return;
            }

            if (!imgName.EndsWith(".img"))
                imgName += ".img";

            ((SunNode)targetNode).AddObject(new SunImage(imgName) { Changed = true });
        }

        public void AddSubPropertyToSelectedNode(TreeNode targetNode, string name)
        {
            if (!(targetNode.Tag is IPropertyContainer)) return;

            if (!(targetNode.Tag is SunDirectory) && !(targetNode.Tag is SunFile) && !(targetNode.Tag is SunImage) && !(targetNode.Tag is SunSubProperty))
            {
                MessageBox.Show("Can't add a SubProperty to directory or sunfile.Tag is " + targetNode.Tag.ToString());
                return;
            }

            string subPropName = name;
            if (name == string.Empty || name == null)
            {
                if (!frmNameInputBox.Show("Add SubProperty", out subPropName))
                    return;
            }

            ((SunNode)targetNode).AddObject(new SunSubProperty(subPropName));
        }

        /// <summary>
        /// Creates a new Integer property node under a selected node.
        /// </summary>
        public void AddDoublePropertyToSelectedNode(TreeNode targetNode)
        {
            if (!(targetNode.Tag is IPropertyContainer)) return;
            if (!frmFloatInputBox.Show("Add Double Value", out string name, out double? value))
                return;
            ((SunNode)targetNode).AddObject(new SunDoubleProperty(name, (double)value));
        }

        /// <summary>
        /// Creates a new Float property node under a selected node.
        /// </summary>
        public void AddFloatPropertyToSelectedNode(TreeNode targetNode)
        {
            if (!(targetNode.Tag is IPropertyContainer)) return;
            if (!frmFloatInputBox.Show("Add Float Value", out string name, out double? value))
                return;
            ((SunNode)targetNode).AddObject(new SunFloatProperty(name, (float)value));
        }

        /// <summary>
        /// Creates a new Float property node under a selected node, with a provided name and value.
        /// </summary>
        public void AddFloatPropertyToSelectedNode(TreeNode targetNode, string name, float value)
        {
            if (targetNode == null || !(targetNode.Tag is IPropertyContainer)) return;
            ((SunNode)targetNode).AddObject(new SunFloatProperty(name, value));
        }

        /// <summary>
        /// Creates a node containing a bitmap image or animated gif under a selected node.
        /// </summary>
        public void AddCanvasPropertyToSelectedNode(TreeNode targetNode)
        {
            if (targetNode == null || !(targetNode.Tag is IPropertyContainer)) return;
            // List for ability to use gifs.
            if (!frmCanvasInputBox.Show("Add Image", out string name, out List<Bitmap> bitmapList, out bool isGif))
                return;

            SunNode target = (SunNode)targetNode;

            if (isGif)
            {
                //  Create the parent node containing all of the gif frames.
                SunNode gifParentNode = target.AddObject(new SunCanvasProperty(name, (SunObject)target.Tag, true));
                if (gifParentNode == null) return;  //improve this duplicate node checking

                for (int i = 0; i < bitmapList.Count; i++)
                {
                    SunCanvasProperty frame = new SunCanvasProperty(i.ToString(), (SunObject)gifParentNode.Tag)
                    {
                        PNG = bitmapList[i]
                    };
                    gifParentNode.AddObject(frame, false);

                    // Add default image properties here. To do so change gifParentNode.AddObject(frame) to
                    // SunNode newNode = gifParentNode.AddObject(frame), then do
                    // newNode.AddObject(whatever_default_property_you_want_to_add)
                    // Examples: image origin, frame delay
                }
                //  To force Treeview AfterSelect to fire
                sunTreeView.SelectedNode = null;
                sunTreeView.SelectedNode = gifParentNode;
            }
            else
            {
                foreach (Bitmap bmp in bitmapList)
                {
                    SunCanvasProperty image = new SunCanvasProperty(name, (SunObject)target.Tag)
                    {
                        PNG = bmp
                    };
                    target.AddObject(image);

                    // Add default image properties here. To do so change target.AddObject(image) to
                    // SunNode newNode = target.AddObject(image), then do
                    // newNode.AddObject(whatever_default_property_you_want_to_add)
                }
            }
        }

        /// <summary>
        /// Creates a new Integer property node under a selected node.
        /// </summary>
        public void AddIntPropertyToSelectedNode(TreeNode targetNode)
        {
            if (targetNode == null || !(targetNode.Tag is IPropertyContainer)) return;
            if (!frmDigitInputBox.Show("Add Int Value", out string name, out int? value))
                return;
            ((SunNode)targetNode).AddObject(new SunIntProperty(name, (int)value));
        }

        /// <summary>
        /// Creates a new Integer property node under a selected node, with a provided name and value.
        /// </summary>
        public void AddIntPropertyToSelectedNode(TreeNode targetNode, string name, int value)
        {
            if (targetNode == null || !(targetNode.Tag is IPropertyContainer)) return;
            ((SunNode)targetNode).AddObject(new SunIntProperty(name, value));
        }

        /// <summary>
        /// Creates a new Long property node under a selected node.
        /// </summary>
        public void AddLongPropertyToSelectedNode(TreeNode targetNode)
        {
            if (targetNode == null || !(targetNode.Tag is IPropertyContainer)) return;
            if (!frmLongInputBox.Show("Add Long Value", out string name, out long? value))
                return;
            ((SunNode)targetNode).AddObject(new SunLongProperty(name, (long)value));
        }

        /// <summary>
        /// Creates a new Long property node under a selected node, with a provided name and value.
        /// </summary>
        public void AddLongPropertyToSelectedNode(TreeNode targetNode, string name, long value)
        {
            if (targetNode == null || !(targetNode.Tag is IPropertyContainer)) return;
            ((SunNode)targetNode).AddObject(new SunLongProperty(name, value));
        }

        /// <summary>
        /// Creates a new Short property node under a selected node.
        /// </summary>
        public void AddShortPropertyToSelectedNode(TreeNode targetNode)
        {
            if (targetNode == null || !(targetNode.Tag is IPropertyContainer)) return;
            if (!frmDigitInputBox.Show("Add Short Value", out string name, out int? value))
                return;
            ((SunNode)targetNode).AddObject(new SunShortProperty(name, (short)value));
        }

        /// <summary>
        /// Creates a new Short property node under a selected node, with a provided name and value.
        /// </summary>
        public void AddShortPropertyToSelectedNode(TreeNode targetNode, string name, short value)
        {
            if (targetNode == null || !(targetNode.Tag is IPropertyContainer)) return;
            ((SunNode)targetNode).AddObject(new SunShortProperty(name, value));
        }

        /// <summary>
        /// Creates a new Sound property node under a selected node.
        /// </summary>
        public void AddSoundPropertyToSelectedNode(TreeNode targetNode)
        {
            //if (targetNode == null || !(targetNode.Tag is IPropertyContainer)) return;
            //if (!frmSoundInputBox.Show("Add Sound", out string name, out string path)) return;
            //((SunNode)targetNode).AddObject(new SunSoundProperty(name, path));
        }

        public void AddStringPropertyToSelectedNode(TreeNode targetNode)
        {
            if (targetNode == null || !(targetNode.Tag is IPropertyContainer)) return;
            if (!frmNameValueInputBox.Show("Add String", out string name, out string value)) return;
            ((SunNode)targetNode).AddObject(new SunStringProperty(name, (string)value));
        }

        public void AddStringPropertyToSelectedNode(TreeNode targetNode, string name, string value)
        {
            if (targetNode == null || !(targetNode.Tag is IPropertyContainer)) return;
            ((SunNode)targetNode).AddObject(new SunStringProperty(name, value));
        }

        /// <summary>
        /// Creates a new Vector property node under a selected node.
        /// </summary>
        public void AddVectorPropertyToSelectedNode(TreeNode targetNode)
        {
            if (targetNode == null || !(targetNode.Tag is IPropertyContainer)) return;
            if (!frmVectorInputBox.Show("Add Vector", out string name, out Point? value)) return;
            ((SunNode)targetNode).AddObject(new SunVectorProperty(name, new SunIntProperty("X", ((Point)value).X), new SunIntProperty("Y", ((Point)value).Y)));
        }

        /// <summary>
        /// Creates a new Integer property node under a selected node, with a provided name and value.
        /// </summary>
        public void AddVectorPropertyToSelectedNode(TreeNode targetNode, string name, Point value)
        {
            if (targetNode == null || !(targetNode.Tag is IPropertyContainer)) return;
            ((SunNode)targetNode).AddObject(new SunVectorProperty(name, value));
        }

        #endregion Adding Directories & Properties

        #region Treeview Click Events

        private void sunTreeView_DoubleClick(object sender, EventArgs e)
        {
            if (sunTreeView.SelectedNode != null && sunTreeView.SelectedNode.Tag is SunImage && sunTreeView.SelectedNode.Nodes.Count == 0)
            {
                ParseOnTreeViewSelectedItem((SunNode)sunTreeView.SelectedNode);
            }
        }

        //  Updates the selected node type label on selection of a node
        //  and shows necessary controls/values.
        private void sunTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            DisplayNodeValue((SunObject)sunTreeView.SelectedNode.Tag);
            lblSelectedNodeType.Text = "Selection Type: " + ((SunNode)sunTreeView.SelectedNode).GetTypeName();
        }

        //  Right-clicking SunTreeView to show context menu.
        private void sunTreeView_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right) return;

            //  Get selected node.
            SunNode selection = (SunNode)sunTreeView.GetNodeAt(e.X, e.Y);
            sunTreeView.SelectedNode = selection;
            cmsNodes.Items.Clear();

            //  There aren't any nodes or we right-clicked blank space.
            if (sunTreeView.Nodes.Count == 0 || selection == null)
            {
                contextMenuManager.CreateContextMenu(null, e);
            }
            //  There are nodes in the SunTreeView.
            else if (sunTreeView.Nodes.Count != 0 && selection != null)
            {
                contextMenuManager.CreateContextMenu(selection, e);
            }
        }

        private void DisplayNodeValue(SunObject obj)
        {
            //  Return controls to default state when selection changes.
            lblPropertyName.Visible = false;
            lblValue.Visible = false;
            if (Controls.ContainsKey("temp"))
                Controls.RemoveByKey("temp");
            txtPropertyName.Visible = false;
            txtPropertyName.Text = "";
            txtPropertyValue.Visible = false;
            txtPropertyValue.Text = "";
            txtPropertyValue.Size = defaultTextBoxSize;
            txtPropertyValue.ScrollBars = ScrollBars.None;
            txtPropertyValue.TextAlign = HorizontalAlignment.Center;
            txtPropertyValue.Multiline = false;

            mainfrm_panning_PictureBox.Visible = false;
            mainfrm_panning_PictureBox.Canvas = null;

            chkAnimateGif.Visible = false;
            // Do something here about cpu usage

            switch (obj)
            {
                case SunNullProperty nullProperty:
                    break;

                case SunShortProperty shortProperty:
                    lblPropertyName.Visible = true;
                    txtPropertyName.Visible = true;
                    lblValue.Visible = true;
                    txtPropertyValue.Visible = true;
                    txtPropertyName.Text = shortProperty.Name;
                    txtPropertyValue.Text = shortProperty.Value.ToString();
                    break;

                case SunIntProperty intProperty:
                    lblPropertyName.Visible = true;
                    txtPropertyName.Visible = true;
                    lblValue.Visible = true;
                    txtPropertyValue.Visible = true;
                    txtPropertyName.Text = intProperty.Name;
                    txtPropertyValue.Text = intProperty.Value.ToString();
                    break;

                case SunLongProperty longProperty:
                    lblPropertyName.Visible = true;
                    txtPropertyName.Visible = true;
                    lblValue.Visible = true;
                    txtPropertyValue.Visible = true;
                    txtPropertyName.Text = longProperty.Name;
                    txtPropertyValue.Text = longProperty.Value.ToString();
                    break;

                case SunFloatProperty floatProperty:
                    lblPropertyName.Visible = true;
                    txtPropertyName.Visible = true;
                    lblValue.Visible = true;
                    txtPropertyValue.Visible = true;
                    txtPropertyName.Text = floatProperty.Name;
                    txtPropertyValue.Text = floatProperty.Value.ToString();
                    break;

                case SunDoubleProperty doubleProperty:
                    lblPropertyName.Visible = true;
                    txtPropertyName.Visible = true;
                    lblValue.Visible = true;
                    txtPropertyValue.Visible = true;
                    txtPropertyName.Text = doubleProperty.Name;
                    txtPropertyValue.Text = doubleProperty.Value.ToString();
                    break;

                case SunStringProperty stringProperty:
                    lblPropertyName.Visible = true;
                    txtPropertyName.Visible = true;
                    lblValue.Visible = true;

                    txtPropertyName.Text = stringProperty.Name;
                    txtPropertyValue.Visible = true;
                    txtPropertyValue.Multiline = true;
                    txtPropertyValue.Size = new Size(205, 62);
                    txtPropertyValue.TextAlign = HorizontalAlignment.Left;
                    txtPropertyValue.Text = stringProperty.Value;
                    break;

                case SunCanvasProperty canvasProperty:
                    //  Display the image/gif with its attributes.
                    mainfrm_panning_PictureBox.Visible = true;
                    //  Check if selected node is of a gif, if so act accordingly
                    if (canvasProperty.IsGif/* && canvasProperty.Frames.Count > 0*/)
                    {
                        chkAnimateGif.Visible = true;
                        mainfrm_panning_PictureBox.Canvas = canvasProperty.Frames[0].PNG; // as a form of thumbnail
                        //if (AnimateGifs)    // find out how to animate the gif here - not only that but make it efficient or whatever
                        //{
                        //    do
                        //    {
                        //        //mainfrm_panning_PictureBox.Image
                        //    }
                        //    while (AnimateGifs);
                        //}
                    }
                    else
                    {
                        mainfrm_panning_PictureBox.Canvas = canvasProperty.PNG;
                    }
                    break;

                case SunVectorProperty vectorProperty:
                    lblPropertyName.Visible = true;
                    lblValue.Visible = true;
                    txtPropertyName.Visible = true;
                    txtPropertyValue.Visible = true;
                    txtPropertyName.Text = vectorProperty.Name;

                    // Resize the original value textbox to hold the X-value.
                    txtPropertyValue.Size = new Size(txtPropertyValue.Size.Width / 2, txtPropertyValue.Height);

                    // Create temporary textbox to hold Y-value of the vector.
                    TextBox temporaryYbox = new TextBox();
                    temporaryYbox.Name = "temp";
                    temporaryYbox.Size = txtPropertyValue.Size;
                    temporaryYbox.Font = txtPropertyValue.Font;
                    temporaryYbox.TextAlign = txtPropertyValue.TextAlign;
                    temporaryYbox.Location = new Point(txtPropertyValue.Location.X + txtPropertyValue.Size.Width + 1, txtPropertyValue.Location.Y);
                    Controls.Add(temporaryYbox);

                    // Add vector values to both textboxes.
                    txtPropertyValue.Text = "X: " + vectorProperty.X.ToString();
                    temporaryYbox.Text = "Y: " + vectorProperty.Y.ToString();
                    break;

                //case SunSoundProperty soundProperty:
                //    break;

                default:
                    break;
            }
        }

        private void chkAnimateGif_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAnimateGif.Checked) AnimateGifs = true;
            else AnimateGifs = false;
        }

        #endregion Treeview Click Events

        #region Temporary

        private void btnCreateMapSun_Click(object sender, EventArgs e)
        {
            sunTreeView.Focus();
            string name = "map.sun";
            var fullpath = Path.Combine(DefaultPath, name);
            SunFile file = new SunFile(name, fullpath);
            manager.sunFiles.Add(file);
            sunTreeView.Nodes.Add(new SunNode(file));

            AddSunDirectoryToSelectedNode(sunTreeView.Nodes[file.Name], "Back");
            AddSunDirectoryToSelectedNode(sunTreeView.Nodes[file.Name].LastNode, "Test");
            AddSunDirectoryToSelectedNode(sunTreeView.Nodes[file.Name].LastNode.LastNode, "still");
        }

        private void btnQuickImageInt_Click(object sender, EventArgs e)
        {
            sunTreeView.Focus();
            string name = Path.GetRandomFileName() + ".sun";
            var fullpath = Path.Combine(DefaultPath, name);
            SunFile file = new SunFile(name, fullpath);
            sunTreeView.Nodes.Add(new SunNode(file));

            AddSunDirectoryToSelectedNode(sunTreeView.Nodes[file.Name], "directory1");
            AddSunImageToSelectedNode(sunTreeView.Nodes[file.Name].LastNode, "image1");
        }

        #endregion Temporary
    }
}