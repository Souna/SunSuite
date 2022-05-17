using SunFileManager.GUI;
using SunFileManager.Properties;
using SunLibrary.SunFileLib.Properties;
using SunLibrary.SunFileLib.Structure;
using System;
using System.Windows.Forms;

namespace SunFileManager
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

        #region Menu Item Declarations

        private readonly ToolStripMenuItem New;
        private readonly ToolStripMenuItem Open;
        private readonly ToolStripMenuItem Save;
        private readonly ToolStripMenuItem Unload;
        private readonly ToolStripMenuItem Reload;
        private readonly ToolStripMenuItem Rename;
        private readonly ToolStripMenuItem Remove;
        private readonly ToolStripMenuItem RemoveThis;
        private readonly ToolStripMenuItem RemoveChildren;
        private readonly ToolStripMenuItem Collapse;
        private readonly ToolStripMenuItem Expand;
        private readonly ToolStripMenuItem AddSunDirectory;
        private readonly ToolStripMenuItem AddSunImage;
        private readonly ToolStripMenuItem AddSubProperty;
        private readonly ToolStripMenuItem AddDoubleProperty;
        private readonly ToolStripMenuItem AddFloatProperty;
        private readonly ToolStripMenuItem AddCanvasProperty;
        private readonly ToolStripMenuItem AddIntProperty;
        private readonly ToolStripMenuItem AddLongProperty;
        private readonly ToolStripMenuItem AddShortProperty;
        private readonly ToolStripMenuItem AddSoundProperty;
        private readonly ToolStripMenuItem AddStringProperty;
        private readonly ToolStripMenuItem AddVectorProperty;

        #endregion Menu Item Declarations

        #region SubMenus

        //  Submenu which contains the Add Property submenu and directory button.
        private ToolStripMenuItem AddSubMenu = new ToolStripMenuItem("Add", Resources.Add);

        //  Submenu which contains the Remove submenu
        private ToolStripMenuItem RemoveSubMenu = new ToolStripMenuItem("Remove...", Resources.Remove);

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

            RemoveThis = new ToolStripMenuItem("This Node", Resources.Remove, new EventHandler(
                delegate (object sender, EventArgs e)
                {
                    mainform.RemoveSelectedNodes();
                }));

            RemoveChildren = new ToolStripMenuItem("All Child Nodes", Resources.Remove, new EventHandler(
                delegate (object sender, EventArgs e)
                {
                    mainform.RemoveChildNodes();
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

            AddSunDirectory = new ToolStripMenuItem("Directory", Resources.addfolder, new EventHandler(
                delegate (object sender, EventArgs e)
                {
                    mainform.AddSunDirectoryToSelectedNode((SunNode)mainform.sunTreeView.SelectedNode, null);
                }));

            AddSunImage = new ToolStripMenuItem("Image", Resources._3d, new EventHandler(
                delegate (object sender, EventArgs e)
                {
                    mainform.AddSunImageToSelectedNode((SunNode)mainform.sunTreeView.SelectedNode, null);
                }));

            AddSubProperty = new ToolStripMenuItem("SubProperty", Resources.Directory, new EventHandler(
                delegate (object sender, EventArgs e)
                {
                    mainform.AddSubPropertyToSelectedNode((SunNode)mainform.sunTreeView.SelectedNode, null);
                }));

            AddDoubleProperty = new ToolStripMenuItem("Double       8 bytes", Resources.Decimal, new EventHandler(
                delegate (object sender, EventArgs e)
                {
                    mainform.AddDoublePropertyToSelectedNode((SunNode)mainform.sunTreeView.SelectedNode);
                }));

            AddFloatProperty = new ToolStripMenuItem("Float           4 bytes", Resources.Decimal, new EventHandler(
                delegate (object sender, EventArgs e)
                {
                    mainform.AddFloatPropertyToSelectedNode((SunNode)mainform.sunTreeView.SelectedNode);
                }));

            AddCanvasProperty = new ToolStripMenuItem("Canvas", Resources.Canvas, new EventHandler(
                delegate (object sender, EventArgs e)
                {
                    mainform.AddCanvasPropertyToSelectedNode((SunNode)mainform.sunTreeView.SelectedNode);
                }));

            AddIntProperty = new ToolStripMenuItem("Int               4 bytes", Resources.Input, new EventHandler(
                delegate (object sender, EventArgs e)
                {
                    mainform.AddIntPropertyToSelectedNode((SunNode)mainform.sunTreeView.SelectedNode);
                }));

            AddLongProperty = new ToolStripMenuItem("Long           8 bytes", Resources.Input, new EventHandler(
                delegate (object sender, EventArgs e)
                {
                    mainform.AddLongPropertyToSelectedNode((SunNode)mainform.sunTreeView.SelectedNode);
                }));

            AddShortProperty = new ToolStripMenuItem("Short          2 bytes", Resources.Input, new EventHandler(
                delegate (object sender, EventArgs e)
                {
                    mainform.AddShortPropertyToSelectedNode((SunNode)mainform.sunTreeView.SelectedNode);
                }));

            AddSoundProperty = new ToolStripMenuItem("Sound", Resources.Sound, new EventHandler(
                delegate (object sender, EventArgs e)
                {
                    mainform.AddSoundPropertyToSelectedNode((SunNode)mainform.sunTreeView.SelectedNode);
                }));

            AddStringProperty = new ToolStripMenuItem("String", Resources.String, new EventHandler(
                delegate (object sender, EventArgs e)
                {
                    mainform.AddStringPropertyToSelectedNode((SunNode)mainform.sunTreeView.SelectedNode);
                }));

            AddVectorProperty = new ToolStripMenuItem("Vector", Resources.Vector, new EventHandler(
                delegate (object sender, EventArgs e)
                {
                    mainform.AddVectorPropertyToSelectedNode((SunNode)mainform.sunTreeView.SelectedNode);
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
            RemoveSubMenu.DropDownItems.Clear();
            AddDigitPropertySubMenu.DropDownItems.Clear();

            if (node == null)
            {
                PopupMenu.Items.AddRange(new ToolStripItem[] { New, Open });
                PopupMenu.Show(mainform.sunTreeView, e.X, e.Y);
            }
            else
            {
                if (node.Tag is SunFile fileNode)
                {
                    //  Populate "Add" menu.
                    AddSubMenu.DropDownItems.AddRange(new ToolStripItem[] { AddSunDirectory, AddSunImage });

                    PopupMenu.Items.AddRange(new ToolStripItem[] { AddSubMenu, new ToolStripSeparator(), Rename, Save, Unload, Reload });

                    if (fileNode.SunDirectory != null && fileNode.SunDirectory.TopLevelEntryCount > 0)
                    {
                        PopupMenu.Items.AddRange(new ToolStripItem[] { Expand, Collapse });
                    }
                    PopupMenu.Show(mainform.sunTreeView, e.X, e.Y);
                }
                else if (node.Tag is SunDirectory dir)
                {
                    //  Populate "Add" menu.
                    AddSubMenu.DropDownItems.AddRange(new ToolStripItem[] { AddSunDirectory, AddSunImage });

                    PopupMenu.Items.AddRange(new ToolStripItem[] { AddSubMenu, new ToolStripSeparator(), Rename });

                    if (node.Nodes.Count > 0/*dir.TopLevelEntryCount > 0*/)
                    {
                        // Populate "Remove" menu.
                        RemoveSubMenu.DropDownItems.AddRange(new ToolStripMenuItem[] { RemoveThis, RemoveChildren });
                        PopupMenu.Items.AddRange(new ToolStripMenuItem[] { RemoveSubMenu, Expand, Collapse });
                    }
                    else
                    {
                        PopupMenu.Items.Add(Remove);
                    }
                    PopupMenu.Show(mainform.sunTreeView, e.X, e.Y);
                }
                else if (node.Tag is IPropertyContainer)
                {
                    AddSubMenu.DropDownItems.AddRange(new ToolStripItem[] { AddSubProperty, AddDigitPropertySubMenu, AddCanvasProperty, AddStringProperty, AddSoundProperty, AddVectorProperty });

                    //  Populate "Digit" add menu.
                    AddDigitPropertySubMenu.DropDownItems.AddRange(new ToolStripItem[] { AddShortProperty, AddIntProperty, AddLongProperty, new ToolStripSeparator(), AddFloatProperty, AddDoubleProperty });

                    PopupMenu.Items.AddRange(new ToolStripItem[] { AddSubMenu, new ToolStripSeparator(), Rename });

                    if (node.Nodes.Count > 0/*container.SunProperties.Count > 0*/)
                    {
                        // Populate "Remove" menu.
                        RemoveSubMenu.DropDownItems.AddRange(new ToolStripMenuItem[] { RemoveThis, RemoveChildren });
                        PopupMenu.Items.AddRange(new ToolStripMenuItem[] { RemoveSubMenu, Expand, Collapse });
                    }
                    else
                    {
                        PopupMenu.Items.Add(Remove);
                    }
                    PopupMenu.Show(mainform.sunTreeView, e.X, e.Y);
                }
                else if (node.Tag is SunProperty)
                {
                    PopupMenu.Items.AddRange(new ToolStripItem[] { Rename, Remove });
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