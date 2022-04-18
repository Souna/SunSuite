using System;
using System.Windows.Forms;
using SunFileManager.GUI;
using SunFileManager.Properties;
using SunFileManager.SunFileLib.Properties;
using SunFileManager.SunFileLib.Structure;

namespace SunFileManager.SunFileLib
{
    /// <summary>
    /// Class that manages the right-click menu for the SunTreeView and its nodes.
    /// </summary>
    public class SunContextMenuManager
    {
        // The main form that we're referencing.
        private frmFileManager mainform = null;

        // The menu shown when right-clicking.
        // Populated with different options depending on context.
        public ContextMenuStrip PopupMenu = new ContextMenuStrip();

        private SunNode currentNode = null;

        #region Menu Items

        //  Right-click button to create a new SunFile.
        private ToolStripMenuItem New;

        //  Right-click button to open SunFiles.
        private ToolStripMenuItem Open;

        //  Right-click button to save selected SunFile.
        private ToolStripMenuItem Save;

        //  Right-click button to unload selected SunFile.
        private ToolStripMenuItem Unload;

        //  Right-click button to reload selected SunFile.
        private ToolStripMenuItem Reload;

        //  Right-click button to rename selected node.
        private ToolStripMenuItem Rename;

        //  Right-click button to remove a node from a parent node.
        private ToolStripMenuItem Remove;

        //  Right-click button to collapse all child nodes of selected node.
        private ToolStripMenuItem Collapse;

        //  Right-click button to expand all child nodes of selected node.
        private ToolStripMenuItem Expand;

        //  Right-click button to add a SunDirectory to a SunFile node.
        private ToolStripMenuItem AddSunDirectory;

        //  Right-click button to add a SunImage to a SunNode.
        private ToolStripMenuItem AddSunImage;

        //  Right-click button to add a Sub Property to a node.
        private ToolStripMenuItem AddSubProperty;

        //  Right-click button to add a Double property to a node.
        private ToolStripMenuItem AddDoubleProperty;

        //  Right-click button to add a Float property to a node.
        private ToolStripMenuItem AddFloatProperty;

        //  Right-click button to add an Image property to a node.
        private ToolStripMenuItem AddCanvasProperty;

        //  Right-click button to add an Int property to a node.
        private ToolStripMenuItem AddIntProperty;

        //  Right-click button to add a Long property to a node.
        private ToolStripMenuItem AddLongProperty;

        //  Right-click button to add a Short property to a node.
        private ToolStripMenuItem AddShortProperty;

        //  Right-click button to add a Sound property to a node.
        private ToolStripMenuItem AddSoundProperty;

        //  Right-click button to add a String property to a node.
        private ToolStripMenuItem AddStringProperty;

        //  Right-click button to add a Vector property to a node.
        private ToolStripMenuItem AddVectorProperty;

        #endregion Menu Items

        #region SubMenus

        //  Submenu which contains the Add Property submenu and directory button.
        private ToolStripMenuItem AddSubMenu = new ToolStripMenuItem("Add", Resources.Add);

        //  Submenu which contains methods for adding different attributes to the nodes.
        private ToolStripMenuItem AddPropertySubMenu = new ToolStripMenuItem("Property", Resources.Property);

        //  Submenu which contains methods for adding numerical values to nodes.
        private ToolStripMenuItem AddDigitPropertySubMenu = new ToolStripMenuItem("Digit", Resources.Input);

        #endregion SubMenus

        /// <summary>
        /// Context menu constructor.
        /// <br>All of the menu items are defined here.</br>
        /// </summary>
        public SunContextMenuManager(Form form)
        {
            mainform = form as frmFileManager;

            #region Menu Item Definitions

            #region General Node Functions

            New = new ToolStripMenuItem("New", Resources.New, new EventHandler(
                delegate (object sender, EventArgs e)
                {
                    new frmNewFile(mainform).ShowDialog();
                }));

            Open = new ToolStripMenuItem("Open", Resources.Open, new EventHandler(
                delegate (object sender, EventArgs e)
                {
                    mainform.openToolStripMenuItem_Click(sender, e);
                }));

            Save = new ToolStripMenuItem("Save", Resources.Save, new EventHandler(
                delegate (object sender, EventArgs e)
                {
                    mainform.SaveFile();
                }));

            Unload = new ToolStripMenuItem("Unload", Resources.Remove, new EventHandler(
                delegate (object sender, EventArgs e)
                {
                    foreach (SunNode node in GetNodes(sender))
                    {
                        mainform.manager.UnloadSunFile((SunFile)node.Tag);
                    }
                }));

            Reload = new ToolStripMenuItem("Reload", Resources.Refresh, new EventHandler(
                delegate (object sender, EventArgs e)
                {
                    foreach (SunNode node in GetNodes(sender))
                    {
                        mainform.manager.ReloadSunFile((SunFile)node.Tag);
                    }
                }));

            Rename = new ToolStripMenuItem("Rename", Resources.Edit, new EventHandler(
                delegate (object sender, EventArgs e)
                {
                    mainform.RenameSelectedNode();
                }));

            Remove = new ToolStripMenuItem("Remove", Resources.Remove, new EventHandler(
                delegate (object sender, EventArgs e)
                {
                    mainform.RemoveSelectedNodes();
                }));

            Collapse = new ToolStripMenuItem("Collapse All", Resources.Collapse, new EventHandler(
                delegate (object sender, EventArgs e)
                {
                    foreach (SunNode node in GetNodes(sender))
                        node.Collapse();
                }));

            Expand = new ToolStripMenuItem("Expand All", Resources.Expand, new EventHandler(
                delegate (object sender, EventArgs e)
                {
                    foreach (SunNode node in GetNodes(sender))
                        node.ExpandAll();
                }));

            #endregion General Node Functions

            #region Properties

            AddSunDirectory = new ToolStripMenuItem("Directory", Resources.Directory, new EventHandler(
                delegate (object sender, EventArgs e)
                {
                    mainform.AddSunDirectoryToSelectedNode(mainform.sunTreeView.SelectedNode, null);
                }));

            AddSunImage = new ToolStripMenuItem("Image", Resources.Directory, new EventHandler(
                delegate (object sender, EventArgs e)
                {
                    mainform.AddSunImageToSelectedNode(mainform.sunTreeView.SelectedNode, null);
                }));

            AddSubProperty = new ToolStripMenuItem("SubProperty", Resources.Directory, new EventHandler(
                delegate (object sender, EventArgs e)
                {
                    mainform.AddSubPropertyToSelectedNode(mainform.sunTreeView.SelectedNode, null);
                }));

            AddDoubleProperty = new ToolStripMenuItem("Double       8 bytes", Resources.Decimal, new EventHandler(
                delegate (object sender, EventArgs e)
                {
                    mainform.AddDoublePropertyToSelectedNode(mainform.sunTreeView.SelectedNode);
                }));

            AddFloatProperty = new ToolStripMenuItem("Float           4 bytes", Resources.Decimal, new EventHandler(
                delegate (object sender, EventArgs e)
                {
                    mainform.AddFloatPropertyToSelectedNode(mainform.sunTreeView.SelectedNode);
                }));

            AddCanvasProperty = new ToolStripMenuItem("Canvas", Resources.Canvas, new EventHandler(
                delegate (object sender, EventArgs e)
                {
                    mainform.AddCanvasPropertyToSelectedNode(mainform.sunTreeView.SelectedNode);
                }));

            AddIntProperty = new ToolStripMenuItem("Int               4 bytes", Resources.Input, new EventHandler(
                delegate (object sender, EventArgs e)
                {
                    mainform.AddIntPropertyToSelectedNode(mainform.sunTreeView.SelectedNode);
                }));

            AddLongProperty = new ToolStripMenuItem("Long           8 bytes", Resources.Input, new EventHandler(
                delegate (object sender, EventArgs e)
                {
                    mainform.AddLongPropertyToSelectedNode(mainform.sunTreeView.SelectedNode);
                }));

            AddShortProperty = new ToolStripMenuItem("Short          2 bytes", Resources.Input, new EventHandler(
                delegate (object sender, EventArgs e)
                {
                    mainform.AddShortPropertyToSelectedNode(mainform.sunTreeView.SelectedNode);
                }));

            AddSoundProperty = new ToolStripMenuItem("Sound", Resources.Sound, new EventHandler(
                delegate (object sender, EventArgs e)
                {
                    mainform.AddSoundPropertyToSelectedNode(mainform.sunTreeView.SelectedNode);
                }));

            AddStringProperty = new ToolStripMenuItem("String", Resources.String, new EventHandler(
                delegate (object sender, EventArgs e)
                {
                    mainform.AddStringPropertyToSelectedNode(mainform.sunTreeView.SelectedNode);
                }));

            AddVectorProperty = new ToolStripMenuItem("Vector", Resources.Vector, new EventHandler(
                delegate (object sender, EventArgs e)
                {
                    mainform.AddVectorPropertyToSelectedNode(mainform.sunTreeView.SelectedNode);
                }));

            #endregion Properties

            #endregion Menu Item Definitions
        }

        /// <summary>
        /// Populates the context menu with the appropriate options depending on context.
        /// <br>pun intended</br>
        /// </summary>
        public void CreateContextMenu(SunNode node, MouseEventArgs e)
        {
            PopupMenu.Items.Clear();
            AddSubMenu.DropDownItems.Clear();
            AddPropertySubMenu.DropDownItems.Clear();
            AddDigitPropertySubMenu.DropDownItems.Clear();

            // No nodes in list yet or chose white space. Show basic menu.
            if (node == null)
            {
                PopupMenu.Items.AddRange(new ToolStripItem[] { New, Open });
                PopupMenu.Show(mainform.sunTreeView, e.X, e.Y);
            }
            else    // List is populated with nodes, show menu populated with options.
            {
                //  If selecting a SunFile node.
                if (node.Tag is SunFile fileNode)
                {
                    //  We're at top level rn
                    AddSubMenu.DropDownItems.Add(AddSunDirectory);
                    AddSubMenu.DropDownItems.Add(AddSunImage);

                    PopupMenu.Items.Add(AddSubMenu);
                    PopupMenu.Items.Add(new ToolStripSeparator());
                    PopupMenu.Items.Add(Rename);
                    PopupMenu.Items.Add(Save);
                    PopupMenu.Items.Add(Unload);
                    PopupMenu.Items.Add(Reload);
                    if (fileNode.SunDirectory != null && fileNode.SunDirectory.SubDirectories.Count > 0)
                    {
                        PopupMenu.Items.Add(Expand);
                        PopupMenu.Items.Add(Collapse);
                    }
                    PopupMenu.Show(mainform.sunTreeView, e.X, e.Y);
                }
                //  If selecting a SunDirectory node.
                else if (node.Tag is SunDirectory)
                {
                    //  Populate "Add" menu.
                    AddSubMenu.DropDownItems.Add(AddSunDirectory);
                    AddSubMenu.DropDownItems.Add(AddSunImage);

                    PopupMenu.Items.Add(AddSubMenu);
                    PopupMenu.Items.Add(new ToolStripSeparator());
                    PopupMenu.Items.Add(Rename);
                    PopupMenu.Items.Add(Remove);
                    PopupMenu.Items.Add(Expand);
                    PopupMenu.Items.Add(Collapse);
                    PopupMenu.Show(mainform.sunTreeView, e.X, e.Y);
                }
                else if (node.Tag is SunImage)
                {
                    //  Populate "Add" menu.
                    AddSubMenu.DropDownItems.Add(AddPropertySubMenu);

                    //  Populate "Digit" add menu.
                    AddDigitPropertySubMenu.DropDownItems.AddRange(new ToolStripMenuItem[] { AddShortProperty, AddIntProperty, AddLongProperty });
                    AddDigitPropertySubMenu.DropDownItems.Add(new ToolStripSeparator());
                    AddDigitPropertySubMenu.DropDownItems.AddRange(new ToolStripMenuItem[] { AddFloatProperty, AddDoubleProperty });

                    // Populate "Property" add menu.
                    AddPropertySubMenu.DropDownItems.Add(AddDigitPropertySubMenu);
                    AddPropertySubMenu.DropDownItems.Add(AddSubProperty);
                    AddPropertySubMenu.DropDownItems.Add(AddCanvasProperty);
                    AddPropertySubMenu.DropDownItems.Add(AddSoundProperty);
                    AddPropertySubMenu.DropDownItems.Add(AddStringProperty);
                    AddPropertySubMenu.DropDownItems.Add(AddVectorProperty);

                    PopupMenu.Items.Add(AddSubMenu);
                    PopupMenu.Items.Add(new ToolStripSeparator());
                    PopupMenu.Items.Add(Rename);
                    PopupMenu.Items.Add(Remove);
                    PopupMenu.Items.Add(Expand);
                    PopupMenu.Items.Add(Collapse);
                    PopupMenu.Show(mainform.sunTreeView, e.X, e.Y);
                }
                //  If selecting a Property node.
                else if (node.Tag is SunProperty)
                {
                    if (node.Tag is SunCanvasProperty)
                    {
                        AddDigitPropertySubMenu.DropDownItems.AddRange(new ToolStripMenuItem[] { AddShortProperty, AddIntProperty, AddLongProperty });
                        AddDigitPropertySubMenu.DropDownItems.Add(new ToolStripSeparator());
                        AddDigitPropertySubMenu.DropDownItems.AddRange(new ToolStripMenuItem[] { AddFloatProperty, AddDoubleProperty });

                        AddSubMenu.DropDownItems.AddRange(new ToolStripMenuItem[] { AddDigitPropertySubMenu, AddStringProperty, AddVectorProperty });
                        PopupMenu.Items.Add(AddSubMenu);
                        PopupMenu.Items.Add(new ToolStripSeparator());
                    }
                    else if (node.Tag is SunSubProperty)
                    {
                        AddSubMenu.DropDownItems.Add(AddPropertySubMenu);

                        //  Populate "Digit" add menu.
                        AddDigitPropertySubMenu.DropDownItems.AddRange(new ToolStripMenuItem[] { AddShortProperty, AddIntProperty, AddLongProperty });
                        AddDigitPropertySubMenu.DropDownItems.Add(new ToolStripSeparator());
                        AddDigitPropertySubMenu.DropDownItems.AddRange(new ToolStripMenuItem[] { AddFloatProperty, AddDoubleProperty });

                        // Populate "Property" add menu.
                        AddPropertySubMenu.DropDownItems.Add(AddDigitPropertySubMenu);
                        AddPropertySubMenu.DropDownItems.Add(AddSubProperty);
                        AddPropertySubMenu.DropDownItems.Add(AddCanvasProperty);
                        AddPropertySubMenu.DropDownItems.Add(AddSoundProperty);
                        AddPropertySubMenu.DropDownItems.Add(AddStringProperty);
                        AddPropertySubMenu.DropDownItems.Add(AddVectorProperty);
                        PopupMenu.Items.Add(AddSubMenu);
                        PopupMenu.Items.Add(new ToolStripSeparator());
                    }
                    PopupMenu.Items.Add(Rename);
                    PopupMenu.Items.Add(Remove);
                    PopupMenu.Items.Add(Expand);
                    PopupMenu.Items.Add(Collapse);
                    PopupMenu.Show(mainform.sunTreeView, e.X, e.Y);
                }
            }
            currentNode = node;
        }

        private SunNode[] GetNodes(object sender)
        {
            return new SunNode[] { currentNode };
        }
    }
}