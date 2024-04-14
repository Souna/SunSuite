using MaterialSkin;
using MaterialSkin.Controls;
using SunFileManager.GUI;
using SunFileManager.GUI.Input;
using SunFileManager.GUI.Input.Forms;
using SunLibrary.SunFileLib.Properties;
using SunLibrary.SunFileLib.Structure;
using SunLibrary.SunFileLib.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Threading;
using Path = System.IO.Path;

namespace SunFileManager
{
    public partial class frmFileManager : MaterialForm
    {
        //Declare a variable to point to a folder called "New .Sun Files" on the desktop

        public static string DefaultPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        public static FileManager manager = null;
        public SunContextMenuManager contextMenuManager = null;
        public bool AnimateGifs = false;
        public Size defaultTextBoxSize = new Size(205, 29);
        public SoundPlayer mp3Player = null;
        public frmSettings settingsForm = null;
        private MaterialSkinManager materialSkinManager = MaterialSkinManager.Instance;

        public TextBox temporaryYbox = new TextBox();
        private Label lblVectorYVal = new Label();

        // Node being dragged
        private SunNode dragNode = null;

        // Temporary drop node for selection
        private SunNode tempDropNode = null;

        // Timer for scrolling
        private System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();

        private bool threadDone = false;
        private System.Threading.Thread runningThread = null;

        public frmFileManager(string sunfileToLoad)
        {
            InitializeComponent();
            chkAnimateGif.Visible = false;
            contextMenuManager = new SunContextMenuManager(this);
            lblPropertyName.Visible = false;
            lblVectorXVal.Visible = false;
            lblValue.Visible = false;
            btnApplyPropertyChanges.Visible = false;
            txtPropertyName.Visible = false;
            txtPropertyValue.Visible = false;
            panning_PictureBox.Visible = false;
            elementHost1.Visible = false;
            manager = new FileManager(this);
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.BlueGrey800, Primary.BlueGrey900, Primary.BlueGrey500, Accent.LightBlue200, TextShade.WHITE);
            Program.UserSettings.Load(Program.settingsPath);
            ApplySettings();

            //materialSkinManager.ColorScheme = new ColorScheme(Primary.BlueGrey800, Primary.BlueGrey900, Primary.BlueGrey500, Accent.LightBlue200, TextShade.WHITE);

            //Load a SunFile from double clicking it
            LoadFile(sunfileToLoad);
        }

        public static void LoadFile(string sunfileToLoad)
        {
            if (sunfileToLoad != null && File.Exists(sunfileToLoad))
            {
                SunFile f = manager.LoadSunFile(sunfileToLoad);
                if (f != null)
                {
                    manager.AddLoadedSunFileToTreeView(f, null, null);
                }
            }
        }

        public void ApplySettings()
        {
            materialSkinManager.Theme = Program.UserSettings.DarkMode ? MaterialSkinManager.Themes.DARK : MaterialSkinManager.Themes.LIGHT;
            //AutomaticallyParseImages is handled in FileManager.OpenSunFile()
            //DisplayWarnings
            sunTreeView.ShowRootLines = Program.UserSettings.FileBoxes;
            sunTreeView.ShowLines = Program.UserSettings.NodeLines;
            sunTreeView.FullRowSelect = Program.UserSettings.HighlightLine;
        }

        #region Toolstrip

        /// <summary>
        /// Display SunFile creation form.
        /// </summary>
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new frmNewFile(this).ShowDialog();
        }

        /// <summary>
        /// Click event for Toolstrip add directory button.
        /// </summary>
        private void sunDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddSunDirectoryToSelectedNode((SunNode)sunTreeView.SelectedNode, null);
        }

        /// <summary>
        /// Opens settings menu
        /// </summary>
        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            settingsForm = new frmSettings(this);
            materialSkinManager.AddFormToManage(settingsForm);
            settingsForm.ShowDialog();
        }

        /// <summary>
        /// Displays help menu
        /// </summary>
        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new frmHelp().ShowDialog();
        }

        /// <summary>
        /// Allows for exporting of images and sound straight to desktop
        /// </summary>
        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string outPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            SunNode node = (SunNode)sunTreeView.SelectedNode;
            if (!(node.Tag is SunObject)) return;
            List<SunObject> objs = new List<SunObject>
            {
                (SunObject)node.Tag
            };

            SunPngMp3Serializer serializer = new SunPngMp3Serializer();
            threadDone = false;
            runningThread = new Thread(new ParameterizedThreadStart(RunSunObjExtraction));
            runningThread.Start((object)new object[] { objs, outPath, serializer });
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
                sunFilePathsToLoad.Add(path/*.ToLower()*/);

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
                manager.AddLoadedSunFileToTreeView(sunFile, dispatcher, null);
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
                    sunTreeView.SelectedNode = ((SunNode)sunTreeView.SelectedNode).TopLevelNode;
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

        public void RemoveChildNodes()
        {
            SunNode parent = (SunNode)sunTreeView.SelectedNode;
            foreach (SunNode node in parent.Nodes)
            {
                if (node != null) node.DeleteNode(true);
            }
            parent.Nodes.Clear();
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

        private void RunSunObjExtraction(object param)
        {
            List<SunObject> objsToDump = (List<SunObject>)((object[])param)[0];
            string path = (string)((object[])param)[1];
            ProgressingSunSerializer serializer = (ProgressingSunSerializer)((object[])param)[2];

            if (serializer is ISunObjectSerializer)
            {
                foreach (SunObject obj in objsToDump)
                {
                    ((ISunObjectSerializer)serializer).SerializeObject(obj, path);
                }
            }
            threadDone = true;
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
            txtPropertyName.Text = newName;
        }

        #endregion Treeview Node Manipulation

        #region Adding Directories & Properties

        /// <summary>
        /// Creates a new Directory under the selected node.
        /// <br>A directory may be created as a top-level directory underneath the SunFile.</br>
        /// </summary>
        public void AddSunDirectoryToSelectedNode(SunNode targetNode, string name)
        {
            bool added = false;
            if (targetNode == null) return;
            SunObject obj = (SunObject)targetNode.Tag;

            if (!(targetNode.Tag is SunDirectory) && !(targetNode.Tag is SunFile))
            {
                MessageBox.Show("Error: Selected node is " + targetNode.GetTypeName() + ", can't add Directory to this type");
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
                    CreateMasterDirectory(sunFileParent);
                targetNode.AddObject(new SunDirectory(dirName, sunFileParent.SunDirectory));
                added = true;
            }
            else if (obj is SunDirectory sunDirectoryParent)
            {
                targetNode.AddObject(new SunDirectory(dirName, sunDirectoryParent));
                added = true;
            }
            if (!added)
            {
                MessageBox.Show("Error adding directory.");
            }
        }

        public void AddSunImageToSelectedNode(SunNode targetNode, string name)
        {
            if (targetNode == null) return;

            if (!(targetNode.Tag is SunDirectory) && !(targetNode.Tag is SunFile))
            {
                MessageBox.Show("Error: Selected node is " + targetNode.GetTypeName() + ", can't add Image to this type");
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

            if ((SunObject)targetNode.Tag is SunFile sunFileParent)
            {
                if (sunFileParent.SunDirectory == null)
                    CreateMasterDirectory(sunFileParent);
            }

            targetNode.AddObject(new SunImage(imgName) { Changed = true });
        }

        public void CreateMasterDirectory(SunFile file)
        {
            file.SunDirectory = new SunDirectory(file.Name, file);
        }

        public void AddSubPropertyToSelectedNode(SunNode targetNode, string name)
        {
            if (!(targetNode.Tag is IPropertyContainer)) return;

            string subPropName = name;
            if (name == string.Empty || name == null)
            {
                if (!frmNameInputBox.Show("Add SubProperty", out subPropName))
                    return;
            }

            targetNode.AddObject(new SunSubProperty(subPropName));
        }

        /// <summary>
        /// Creates a new Double property node under a selected node.
        /// </summary>
        public void AddDoublePropertyToSelectedNode(SunNode targetNode)
        {
            if (!(targetNode.Tag is IPropertyContainer)) return;
            if (!frmFloatInputBox.Show("Add Double Value", out string name, out double? value))
                return;
            targetNode.AddObject(new SunDoubleProperty(name, (double)value));
        }

        /// <summary>
        /// Creates a new Double property node under a selected node, with a provided name and value.
        /// </summary>
        public void AddDoublePropertyToSelectedNode(SunNode targetNode, string name, double value)
        {
            if (!(targetNode.Tag is IPropertyContainer)) return;
            targetNode.AddObject(new SunDoubleProperty(name, value));
        }

        /// <summary>
        /// Creates a new Float property node under a selected node.
        /// </summary>
        public void AddFloatPropertyToSelectedNode(SunNode targetNode)
        {
            if (!(targetNode.Tag is IPropertyContainer)) return;
            if (!frmFloatInputBox.Show("Add Float Value", out string name, out double? value))
                return;
            targetNode.AddObject(new SunFloatProperty(name, (float)value));
        }

        /// <summary>
        /// Creates a new Float property node under a selected node, with a provided name and value.
        /// </summary>
        public void AddFloatPropertyToSelectedNode(SunNode targetNode, string name, float value)
        {
            if (targetNode == null || !(targetNode.Tag is IPropertyContainer)) return;
            targetNode.AddObject(new SunFloatProperty(name, value));
        }

        /// <summary>
        /// Creates nodes containing a bitmap image or animated gif under a selected node.
        /// </summary>
        public void AddCanvasPropertyToSelectedNode(SunNode targetNode)
        {
            /*
             * Either way when you add a .gif, a subproperty is created and it's placed inside.
             * The code for this method can very much be improved but for now it works as needed
             * If you're adding a single canvas you can choose its name
             * If you're adding multiple canvases at once the names start at 0 and increment
             */
            if (targetNode == null || !(targetNode.Tag is IPropertyContainer)) return;

            if (!frmCanvasInputBox.Show("Add Image", out string name, out List<Bitmap> bitmaps, out List<int> gifFrameDelays, out List<Bitmap> gifFrames, out bool createSubProperty))
                return;

            int imageIndex = 0;
            // If this is 0 then there's no existing nodes under target node
            int lastImageIndex = targetNode.LastNode != null ?  targetNode.LastNode.Index + 1 : 0;

            if (createSubProperty)
            {
                SunSubProperty subProp = new SunSubProperty(name);

                foreach (Bitmap bmp in bitmaps)
                {
                    SunPngProperty pngProp = new SunPngProperty();
                    pngProp.SetPNG(bmp);
                    SunCanvasProperty canvasProp = new SunCanvasProperty((lastImageIndex + imageIndex).ToString())
                    {
                        PNG = pngProp 
                    };
                    canvasProp.AddProperty(new SunVectorProperty("origin", new SunIntProperty("X", 0), new SunIntProperty("Y", 0)));    //Create default origin prop
                    subProp.AddProperty(canvasProp);

                    imageIndex++;
                }

                imageIndex = 0; // reset image index

                foreach (Bitmap frame in gifFrames)
                {
                    SunCanvasProperty canvasProp = new SunCanvasProperty(imageIndex.ToString());
                    SunPngProperty pngProp = new SunPngProperty();
                    pngProp.SetPNG(frame);
                    canvasProp.PNG = pngProp;
                    int delay = gifFrameDelays[imageIndex];
                    canvasProp.AddProperty(new SunIntProperty("delay", delay));
                    canvasProp.AddProperty(new SunVectorProperty("origin", new SunIntProperty("X", 0), new SunIntProperty("Y", 0)));    //Create default origin prop
                    subProp.AddProperty(canvasProp);

                    imageIndex++;
                }
                targetNode.AddObject(subProp, true);
                sunTreeView.SelectedNode = targetNode[subProp.Name];
            }
            else
            {
                if (bitmaps.Count == 1)
                {
                    SunPngProperty pngProp = new SunPngProperty();
                    pngProp.SetPNG(bitmaps[0]);
                    SunCanvasProperty canvasProp = new SunCanvasProperty(name)
                    { 
                        PNG = pngProp 
                    };
                    if (targetNode.Nodes.ContainsKey(name))
                    {
                        canvasProp.Name = (lastImageIndex + imageIndex).ToString();
                    }
                    canvasProp.AddProperty(new SunVectorProperty("origin", new SunIntProperty("X", 0), new SunIntProperty("Y", 0)));    //Create default origin prop
                    targetNode.AddObject(canvasProp, false);
                }
                else foreach (Bitmap bmp in bitmaps)
                {
                    SunPngProperty pngProp = new SunPngProperty();
                    pngProp.SetPNG(bmp);
                    SunCanvasProperty canvasProp = new SunCanvasProperty((lastImageIndex + imageIndex).ToString())
                    {
                        PNG = pngProp 
                    };
                    canvasProp.AddProperty(new SunVectorProperty("origin", new SunIntProperty("X", 0), new SunIntProperty("Y", 0)));    //Create default origin prop
                    targetNode.AddObject(canvasProp, false);

                    imageIndex++;
                }

                imageIndex = 0; // reset image index

                if (gifFrames.Count > 0)
                {
                    SunSubProperty gifSubProp = new SunSubProperty(name);
                    foreach (Bitmap frame in gifFrames)
                    {
                        SunCanvasProperty canvasProp = new SunCanvasProperty(imageIndex.ToString());
                        SunPngProperty pngProp = new SunPngProperty();
                        pngProp.SetPNG(frame);
                        canvasProp.PNG = pngProp;
                        int delay = gifFrameDelays[imageIndex];
                        canvasProp.AddProperty(new SunIntProperty("delay", delay));
                        canvasProp.AddProperty(new SunVectorProperty("origin", new SunIntProperty("X", 0), new SunIntProperty("Y", 0)));    //Create default origin prop
                        gifSubProp.AddProperty(canvasProp);

                        imageIndex++;
                    }
                    targetNode.AddObject(gifSubProp);
                }
            }

            bitmaps.Clear();
            gifFrameDelays.Clear();
            gifFrames.Clear();
        }

        /// <summary>
        /// Creates a new Convex property under a selected node.
        /// </summary>
        public void AddConvexPropertyToSelectedNode(SunNode targetNode)
        {
            if (!(targetNode.Tag is IPropertyContainer)) return;

            if (!frmNameInputBox.Show("Add Convex Property", out string convexPropName))
                return;

            targetNode.AddObject(new SunConvexProperty(convexPropName));
        }

        /// <summary>
        /// Creates a new Integer property node under a selected node.
        /// </summary>
        public void AddIntPropertyToSelectedNode(SunNode targetNode)
        {
            if (targetNode == null || !(targetNode.Tag is IPropertyContainer)) return;
            if (!frmDigitInputBox.Show("Add Int Value", out string name, out int? value))
                return;
            targetNode.AddObject(new SunIntProperty(name, (int)value));
        }

        /// <summary>
        /// Creates a new Integer property node under a selected node, with a provided name and value.
        /// </summary>
        public void AddIntPropertyToSelectedNode(SunNode targetNode, string name, int value)
        {
            if (targetNode == null || !(targetNode.Tag is IPropertyContainer)) return;
            targetNode.AddObject(new SunIntProperty(name, value));
        }

        /// <summary>
        /// Creates a new Long property node under a selected node.
        /// </summary>
        public void AddLongPropertyToSelectedNode(SunNode targetNode)
        {
            if (targetNode == null || !(targetNode.Tag is IPropertyContainer)) return;
            if (!frmLongInputBox.Show("Add Long Value", out string name, out long? value))
                return;
            targetNode.AddObject(new SunLongProperty(name, (long)value));
        }

        /// <summary>
        /// Creates a new Long property node under a selected node, with a provided name and value.
        /// </summary>
        public void AddLongPropertyToSelectedNode(SunNode targetNode, string name, long value)
        {
            if (targetNode == null || !(targetNode.Tag is IPropertyContainer)) return;
            targetNode.AddObject(new SunLongProperty(name, value));
        }

        /// <summary>
        /// Creates a new Link property node under a selected node.
        /// </summary>
        public void AddLinkPropertyToSelectedNode(SunNode targetNode)
        {
            if (targetNode == null || !(targetNode.Tag is IPropertyContainer)) return;
            if (!frmNameValueInputBox.Show("Add Link", out string name, out string value)) return;
            targetNode.AddObject(new SunLinkProperty(name, value));
        }

        /// <summary>
        /// Creates a new Short property node under a selected node.
        /// </summary>
        public void AddShortPropertyToSelectedNode(SunNode targetNode)
        {
            if (targetNode == null || !(targetNode.Tag is IPropertyContainer)) return;
            if (!frmDigitInputBox.Show("Add Short Value", out string name, out int? value))
                return;
            targetNode.AddObject(new SunShortProperty(name, (short)value));
        }

        /// <summary>
        /// Creates a new Short property node under a selected node, with a provided name and value.
        /// </summary>
        public void AddShortPropertyToSelectedNode(SunNode targetNode, string name, short value)
        {
            if (targetNode == null || !(targetNode.Tag is IPropertyContainer)) return;
            targetNode.AddObject(new SunShortProperty(name, value));
        }

        /// <summary>
        /// Creates a new Sound property node under a selected node.
        /// </summary>
        public void AddSoundPropertyToSelectedNode(SunNode targetNode)
        {
            if (targetNode == null || !(targetNode.Tag is IPropertyContainer)) return;
            if (!frmSoundInputBox.Show("Add Sound", out string name, out string path))
                return;
            targetNode.AddObject(new SunSoundProperty(name, path));
        }

        /// <summary>
        /// Creates a new String property node under a selected node.
        /// </summary>
        public void AddStringPropertyToSelectedNode(SunNode targetNode)
        {
            if (targetNode == null || !(targetNode.Tag is IPropertyContainer)) return;
            if (!frmNameValueInputBox.Show("Add String", out string name, out string value)) return;
            targetNode.AddObject(new SunStringProperty(name, value));
        }

        /// <summary>
        /// Creates a new String property node under a selected node, with a provided name and value.
        /// </summary>
        public void AddStringPropertyToSelectedNode(SunNode targetNode, string name, string value)
        {
            if (targetNode == null || !(targetNode.Tag is IPropertyContainer)) return;
            targetNode.AddObject(new SunStringProperty(name, value));
        }

        /// <summary>
        /// Creates a new Vector property node under a selected node.
        /// </summary>
        public void AddVectorPropertyToSelectedNode(SunNode targetNode)
        {
            if (targetNode == null || !(targetNode.Tag is IPropertyContainer)) return;
            if (!frmVectorInputBox.Show("Add Vector", out string name, out Point? value)) return;
            targetNode.AddObject(new SunVectorProperty(name, new SunIntProperty("X", ((Point)value).X), new SunIntProperty("Y", ((Point)value).Y)));
        }

        /// <summary>
        /// Creates a new Vector property node under a selected node, with a provided name and value.
        /// </summary>
        public void AddVectorPropertyToSelectedNode(SunNode targetNode, string name, Point value)
        {
            if (targetNode == null || !(targetNode.Tag is IPropertyContainer)) return;
            targetNode.AddObject(new SunVectorProperty(name, value));
        }

        #endregion Adding Directories & Properties

        #region Treeview Input Events

        private void sunTreeView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Right)
            {
                if (sunTreeView.SelectedNode != null && sunTreeView.SelectedNode.Tag is SunImage
                    && sunTreeView.SelectedNode.Nodes.Count == 0)
                    ParseOnTreeViewSelectedItem((SunNode)sunTreeView.SelectedNode);
            }
        }

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

        public void DisplayNodeValue(SunObject obj)
        {
            //  Return controls to default state when selection changes.
            lblPropertyName.Visible = false;
            lblValue.Visible = false;
            if (Controls.ContainsKey("temp"))
                Controls.RemoveByKey("temp");
            //temporaryYbox.Visible = false;
            lblVectorYVal.Visible = false;
            btnApplyPropertyChanges.Visible = false;
            txtPropertyName.Visible = false;
            txtPropertyName.Text = "";
            txtPropertyValue.Visible = false;
            txtPropertyValue.Text = "";
            lblVectorXVal.Visible = false;
            txtPropertyValue.Size = defaultTextBoxSize;
            txtPropertyValue.ScrollBars = ScrollBars.None;
            txtPropertyValue.TextAlign = HorizontalAlignment.Center;
            txtPropertyValue.Multiline = false;
            elementHost1.Visible = false;
            soundPlayer.Visibility = System.Windows.Visibility.Collapsed;

            panning_PictureBox.Visible = false;
            panning_PictureBox.Canvas = null;

            chkAnimateGif.Visible = false;

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
                    btnApplyPropertyChanges.Visible = true;
                    break;

                case SunIntProperty intProperty:
                    lblPropertyName.Visible = true;
                    txtPropertyName.Visible = true;
                    lblValue.Visible = true;
                    txtPropertyValue.Visible = true;
                    txtPropertyName.Text = intProperty.Name;
                    txtPropertyValue.Text = intProperty.Value.ToString();
                    btnApplyPropertyChanges.Visible = true;
                    break;

                case SunLongProperty longProperty:
                    lblPropertyName.Visible = true;
                    txtPropertyName.Visible = true;
                    lblValue.Visible = true;
                    txtPropertyValue.Visible = true;
                    txtPropertyName.Text = longProperty.Name;
                    txtPropertyValue.Text = longProperty.Value.ToString();
                    btnApplyPropertyChanges.Visible = true;
                    break;

                case SunFloatProperty floatProperty:
                    lblPropertyName.Visible = true;
                    txtPropertyName.Visible = true;
                    lblValue.Visible = true;
                    txtPropertyValue.Visible = true;
                    txtPropertyName.Text = floatProperty.Name;
                    txtPropertyValue.Text = floatProperty.Value.ToString();
                    btnApplyPropertyChanges.Visible = true;
                    break;

                case SunDoubleProperty doubleProperty:
                    lblPropertyName.Visible = true;
                    txtPropertyName.Visible = true;
                    lblValue.Visible = true;
                    txtPropertyValue.Visible = true;
                    txtPropertyName.Text = doubleProperty.Name;
                    txtPropertyValue.Text = doubleProperty.Value.ToString();
                    btnApplyPropertyChanges.Visible = true;
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
                    txtPropertyValue.ScrollBars = ScrollBars.Vertical;
                    btnApplyPropertyChanges.Visible = true;
                    break;

                case SunCanvasProperty canvasProperty:
                    //  Display the image/gif with its attributes.
                    panning_PictureBox.Visible = true;

                    //mainfrm_panning_PictureBox.Canvas = BitmapToImageSource.ToWinFormsBitmap(canvasProperty.GetBitmap());

                    panning_PictureBox.Canvas = canvasProperty.GetBitmap();

                    //  Check if selected node is of a gif, if so act accordingly
                    //if (canvasProperty.IsGif/* && canvasProperty.Frames.Count > 0*/)
                    //{
                    //    chkAnimateGif.Visible = true;
                    //    mainfrm_panning_PictureBox.Canvas = canvasProperty.Frames[0].PNG; // as a form of thumbnail
                    //    //if (AnimateGifs)    // find out how to animate the gif here - not only that but make it efficient or whatever
                    //    //{
                    //    //    do
                    //    //    {
                    //    //        //mainfrm_panning_PictureBox.Image
                    //    //    }
                    //    //    while (AnimateGifs);
                    //    //}
                    //}
                    //else
                    //{
                    //    mainfrm_panning_PictureBox.Canvas = canvasProperty.PNG;
                    //}

                    //btnApplyPropertyChanges.Visible = true;
                    break;

                case SunVectorProperty vectorProperty:
                    lblPropertyName.Visible = true;
                    lblVectorXVal.Visible = true;
                    lblVectorYVal.Visible = true;
                    lblValue.Visible = true;
                    txtPropertyName.Visible = true;
                    txtPropertyValue.Visible = true;
                    txtPropertyName.Text = vectorProperty.Name;

                    // Resize the original value textbox to hold the X-value.
                    txtPropertyValue.Size = new Size(txtPropertyValue.Size.Width / 2, txtPropertyValue.Height);

                    // Temporary textbox to hold Y-value of the vector.
                    temporaryYbox.Name = "temp";
                    temporaryYbox.Size = txtPropertyValue.Size;
                    temporaryYbox.Font = txtPropertyValue.Font;
                    temporaryYbox.TextAlign = txtPropertyValue.TextAlign;
                    temporaryYbox.Location = new Point(txtPropertyValue.Location.X + txtPropertyValue.Size.Width + 1, txtPropertyValue.Location.Y);
                    temporaryYbox.TabIndex = txtPropertyValue.TabIndex + 1;
                    Controls.Add(temporaryYbox);

                    // Temporary label to say "Y-Value" under second vector value box
                    lblVectorYVal.Size = lblVectorXVal.Size;
                    lblVectorYVal.Font = lblVectorXVal.Font;
                    lblVectorYVal.ForeColor = lblVectorXVal.ForeColor;
                    lblVectorYVal.Text = "Y-Value";
                    lblVectorYVal.Location = new Point(lblVectorXVal.Location.X + txtPropertyValue.Size.Width + 1, lblVectorXVal.Location.Y);
                    Controls.Add(lblVectorYVal);

                    // Add vector values to both textboxes.
                    txtPropertyValue.Text = vectorProperty.X.ToString();
                    temporaryYbox.Text = vectorProperty.Y.ToString();

                    btnApplyPropertyChanges.Visible = true;
                    break;

                case SunSoundProperty soundProperty:
                    elementHost1.Visible = true;
                    soundPlayer.Visibility = System.Windows.Visibility.Visible;
                    soundPlayer.SoundProperty = (SunSoundProperty)obj;
                    break;

                case SunSubProperty subProperty:
                    break;

                case SunLinkProperty linkProperty:
                    lblPropertyName.Visible = true;
                    txtPropertyName.Visible = true;
                    lblValue.Visible = true;

                    txtPropertyName.Text = linkProperty.Name;
                    txtPropertyValue.Visible = true;
                    txtPropertyValue.Multiline = true;
                    txtPropertyValue.Size = new Size(205, 62);
                    txtPropertyValue.TextAlign = HorizontalAlignment.Left;
                    txtPropertyValue.Text = linkProperty.Value;
                    txtPropertyValue.ScrollBars = ScrollBars.Vertical;
                    btnApplyPropertyChanges.Visible = true;
                    break;

                default:
                    break;
            }
            UpdateSelectedNodeTypeLabel();
        }

        public void UpdateSelectedNodeTypeLabel()
        {
            lblSelectedNodeType.Text = "Selection Type: " + ((SunNode)sunTreeView.SelectedNode).GetTypeName();
        }

        private void chkAnimateGif_CheckedChanged(object sender, EventArgs e)
        {
            AnimateGifs = chkAnimateGif.Checked;
        }

        private void btnApplyPropertyChanges_Click(object sender, EventArgs e)
        {
            if (sunTreeView.SelectedNode == null) return;

            SunObject obj = (SunObject)sunTreeView.SelectedNode.Tag;
            //I know enclosing each case in its own block to declare local variables for the TryParse methods is ugly
            //I'd rather do this than have a bunch of if/else statements. Don't know if it's better =)
            switch (obj)
            {
                case SunNullProperty nullProperty:
                    break;

                case SunShortProperty shortProperty:
                    {
                        if (!short.TryParse(txtPropertyValue.Text, out short newValue)) return;
                        ((SunShortProperty)obj).Value = newValue;
                    }
                    break;

                case SunIntProperty intProperty:
                    {
                        if (!int.TryParse(txtPropertyValue.Text, out int newValue)) return;
                        ((SunIntProperty)obj).Value = newValue;
                    }
                    break;

                case SunLongProperty longProperty:
                    {
                        if (!long.TryParse(txtPropertyValue.Text, out long newValue)) return;
                        ((SunLongProperty)obj).Value = newValue;
                    }
                    break;

                case SunFloatProperty floatProperty:
                    {
                        if (!float.TryParse(txtPropertyValue.Text, out float newValue)) return;
                        ((SunFloatProperty)obj).Value = newValue;
                    }
                    break;

                case SunDoubleProperty doubleProperty:
                    {
                        if (!double.TryParse(txtPropertyValue.Text, out double newValue)) return;
                        ((SunDoubleProperty)obj).Value = newValue;
                    }
                    break;

                case SunStringProperty stringProperty:
                    ((SunStringProperty)obj).Value = txtPropertyValue.Text;
                    break;

                case SunLinkProperty linkProperty:
                    ((SunLinkProperty)obj).Value = txtPropertyValue.Text;
                    break;

                case SunCanvasProperty canvasProperty:
                    // It already changes it internally when you double click the picturebox.
                    // picPan_MouseDoubleClick
                    break;

                case SunVectorProperty vectorProperty:
                    {
                        if (!int.TryParse(txtPropertyValue.Text, out int newXValue)) return;
                        ((SunVectorProperty)obj).X.Value = newXValue;
                        if (!int.TryParse(temporaryYbox.Text, out int newYValue)) return;
                        ((SunVectorProperty)obj).Y.Value = newYValue;
                    }
                    break;
            }
            sunTreeView.SelectedNode.ForeColor = SunNode.NewObjectForeColor;
        }

        #region DragDrop

        /// <summary>
        /// Occurs when a drag-and-drop operation is completed.
        /// </summary>
        private void sunTreeView_DragDrop(object sender, DragEventArgs e)
        {
            return;
            // Unlock updates
            TreeViewDragHelper.ImageList_DragLeave(this.sunTreeView.Handle);

            // Get drop node
            SunNode dropNode = (SunNode)this.sunTreeView.GetNodeAt(this.sunTreeView.PointToClient(new Point(e.X, e.Y)));

            if (CanDragDropNode(dragNode, dropNode))
            {
                // If drop node isn't equal to drag node, add drag node as child of drop node
                if (this.dragNode != dropNode)
                {
                    if (!SunNode.CanNodeBeInserted(dropNode, dragNode))
                        return;

                    //SunNode newNode = SunNode.CloneJson(dragNode);
                    //SunNode newNode = SunNode.DeepCopy(dragNode);

                    dragNode.ForeColor = SunNode.NewObjectForeColor;
                    dropNode.AddObject((SunObject)dragNode.Tag, false);

                    // Remove OG drag node from parent
                    // We need to be able to do this without removing the node from its new parent
                    // ATM I am struggling to work this out because any changes made to the old node also affect the new one
                    // I learned this is because by default C# makes reference types for objects
                    dragNode.DeleteNode();

                    // Set drag node to null
                    dragNode = null;

                    // Disable scroll timer
                    timer.Enabled = false;
                }
            }
        }

        /// <summary>
        /// Determines if the node you are dragging is compatible with the node you are moving it under.
        /// </summary>
        /// <param name="drag">The node you are moving</param>
        /// <param name="receiver">The new parent node for the one you are dragging</param>
        /// <returns>True if you move a node under a 'compatible' node.</returns>
        private bool CanDragDropNode(SunNode drag, SunNode receiver)
        {
            if (drag.Tag is SunFile)    // SunFiles cannot be moved
            {
                return false;
            }

            if (drag.Tag is SunDirectory)   // Directories only go inside SunFiles or other Directories
            {
                if (!(receiver.Tag is SunFile || receiver.Tag is SunDirectory))
                    return false;
            }

            if (drag.Tag is SunImage)   // Images only go inside Directories
            {
                if (!(receiver.Tag is SunDirectory))
                    return false;
            }

            if (drag.Tag is SunProperty)
            {
                if (drag.Tag is IPropertyContainer) // If we are dragging a special property, i.e SubProperty/SunCanvasProperty, they can only go inside an IPropertyContainer
                    if (!(receiver.Tag is IPropertyContainer))
                        return false;
                    else if (!(receiver.Tag is IPropertyContainer)) // If we are dragging a normal property
                        return false;
            }

            return true;
        }

        /// <summary>
        /// Occurs when the mouse drags an item into the client area for this Control.
        /// </summary>
        private void sunTreeView_DragEnter(object sender, DragEventArgs e)
        {
            return;
            TreeViewDragHelper.ImageList_DragEnter(this.sunTreeView.Handle, e.X - this.sunTreeView.Left,
                e.Y - this.sunTreeView.Top);
            // Enable timer for scrolling dragged item
            this.timer.Enabled = true;

            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }

        /// <summary>
        /// Occurs when an object is dragged out of the control's bounds.
        /// </summary>
        private void sunTreeView_DragLeave(object sender, EventArgs e)
        {
            return;
            TreeViewDragHelper.ImageList_DragLeave(this.sunTreeView.Handle);

            // Disable timer for scrolling dragged item
            this.timer.Enabled = false;
        }

        /// <summary>
        /// Occurs when an object is dragged over the control's bounds.
        /// </summary>
        private void sunTreeView_DragOver(object sender, DragEventArgs e)
        {
            return;
            // Compute drag position and move image
            Point formP = this.PointToClient(new Point(e.X, e.Y));
            TreeViewDragHelper.ImageList_DragMove(formP.X - this.sunTreeView.Left, formP.Y - this.sunTreeView.Top);

            // Get actual drop node
            SunNode dropNode = (SunNode)this.sunTreeView.GetNodeAt(this.sunTreeView.PointToClient(new Point(e.X, e.Y)));
            if (dropNode == null)
            {
                e.Effect = DragDropEffects.None;
                return;
            }

            e.Effect = DragDropEffects.Move;

            // if mouse is on a new node select it
            if (this.tempDropNode != dropNode)
            {
                TreeViewDragHelper.ImageList_DragShowNolock(false);
                this.sunTreeView.SelectedNode = dropNode;
                TreeViewDragHelper.ImageList_DragShowNolock(true);
                tempDropNode = dropNode;
            }

            // Avoid that drop node is child of drag node
            TreeNode tmpNode = dropNode;
            while (tmpNode.Parent != null)
            {
                if (tmpNode.Parent == this.dragNode) e.Effect = DragDropEffects.None;
                tmpNode = tmpNode.Parent;
            }
        }

        /// <summary>
        /// Occurs when the mouse drags an item.
        /// The system requests that the Control provides feedback to that effect.
        /// </summary>
        private void sunTreeView_GiveFeedback(object sender, GiveFeedbackEventArgs e)
        {
            if (e.Effect == DragDropEffects.Move)
            {
                // Show pointer cursor while dragging
                e.UseDefaultCursors = false;
                this.sunTreeView.Cursor = Cursors.Default;
            }
            else e.UseDefaultCursors = true;
        }

        /// <summary>
        /// Occurs when the user begins dragging an item.
        /// </summary>
        private void sunTreeView_ItemDrag(object sender, ItemDragEventArgs e)
        {
            // Get drag node and select it
            this.dragNode = (SunNode)e.Item;
            this.sunTreeView.SelectedNode = this.dragNode;
            return;

            // Reset image list used for drag image
            this.imageListDrag.Images.Clear();
            this.imageListDrag.ImageSize = new Size(this.dragNode.Bounds.Size.Width + this.sunTreeView.Indent, this.dragNode.Bounds.Height);

            // Create new bitmap
            // This bitmap will contain the tree node image to be dragged
            Bitmap bmp = new Bitmap(this.dragNode.Bounds.Width + this.sunTreeView.Indent, this.dragNode.Bounds.Height);

            // Get graphics from bitmap
            Graphics gfx = Graphics.FromImage(bmp);

            // Draw node icon into the bitmap
            gfx.DrawImage(imageList1.Images[dragNode.ImageIndex], 0, 0);

            // Draw node label into bitmap
            gfx.DrawString(this.dragNode.Text,
                this.sunTreeView.Font,
                new SolidBrush(this.sunTreeView.ForeColor),
                (float)this.sunTreeView.Indent, 1.0f);

            // Add bitmap to imagelist
            this.imageListDrag.Images.Add(bmp);

            // Get mouse position in client coordinates
            Point p = this.sunTreeView.PointToClient(Control.MousePosition);

            // Compute delta between mouse position and node bounds
            int dx = p.X + this.sunTreeView.Indent - this.dragNode.Bounds.Left;
            int dy = p.Y - this.dragNode.Bounds.Top;

            // Begin dragging image
            if (TreeViewDragHelper.ImageList_BeginDrag(this.imageListDrag.Handle, 0, dx, dy))
            {
                // Begin dragging
                this.sunTreeView.DoDragDrop(bmp, DragDropEffects.Move);
                // End dragging image
                TreeViewDragHelper.ImageList_EndDrag();
            }
        }

        #endregion DragDrop

        #endregion Treeview Input Events

        #region Form Input Events

        /// <summary>
        /// For key combinations
        /// </summary>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                // ... Process Shift+Ctrl+Alt+B ...
                case Keys.B | Keys.Control | Keys.Alt | Keys.Shift:
                    ShowPeterAlert();
                    return true; // signal that we've processed this key
            }

            // run base implementation
            return base.ProcessCmdKey(ref msg, keyData);
        }

        public void ShowPeterAlert()
        {
            new frmPeterAlert().ShowDialog();
        }

        #endregion Form Input Events

        #region Debug

        private void btnCreateTestFile_Click(object sender, EventArgs e)
        {
            //sunTreeView.Focus();
            //string name = "test.sun";
            //var fullpath = Path.Combine(DefaultPath, name);
            //SunFile file = new SunFile(name, fullpath);
            //manager.sunFiles.Add(file);
            //sunTreeView.Nodes.Add(new SunNode(file));

            //AddSunImageToSelectedNode((SunNode)sunTreeView.Nodes[file.Name], "image1");

            SunFile file = manager.LoadSunFile(Path.Combine(DefaultPath + "\\stuff" + "\\NewSunFiles", "Map.sun"));
            manager.sunFiles.Add(file);
            sunTreeView.Nodes.Add(new SunNode(file));
        }
        private void btnOpenStringFile_Click(object sender, EventArgs e)
        {
            SunFile file = manager.LoadSunFile(Path.Combine(DefaultPath + "\\stuff" + "\\NewSunFiles", "String.sun"));
            manager.sunFiles.Add(file);
            sunTreeView.Nodes.Add(new SunNode(file));
        }

        #endregion Debug

    }
}