using SunFileManager.GUI;
using SunLibrary.SunFileLib.Properties;
using SunLibrary.SunFileLib.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace SunFileManager
{
    /// <summary>
    /// Builds a WPF <see cref="ContextMenu"/> appropriate for the right-clicked node.
    /// </summary>
    public class SunContextMenuManager
    {
        private MainWindow mainWindow;
        private SunNode currentNode;

        public SunContextMenuManager(MainWindow window)
        {
            mainWindow = window;
        }

        // ── Public entry point ─────────────────────────────────────────────────────
        public ContextMenu BuildContextMenu(SunNode node, IReadOnlyList<SunNode> selectedNodes)
        {
            currentNode = node;
            var menu = new ContextMenu();

            if (node == null)
            {
                menu.Items.Add(MakeItem("New",  () => new frmNewFile(mainWindow).ShowDialog()));
                menu.Items.Add(MakeItem("Open", () => mainWindow.OpenFiles()));
                return menu;
            }

            var sunFileNodes = selectedNodes.Where(n => n.Tag is SunFile).ToList();

            if (sunFileNodes.Count > 1)
            {
                menu.Items.Add(MakeItem("Unload", () =>
                {
                    foreach (SunNode n in GetNodes(selectedNodes))
                        MainWindow.manager.UnloadSunFile((SunFile)n.Tag, n);
                }));
                return menu;
            }

            if (node.Tag is SunFile)
            {
                var addSub = new MenuItem { Header = "Add" };
                addSub.Items.Add(MakeItem("Directory", () => mainWindow.AddSunDirectoryToSelectedNode(node, null)));
                addSub.Items.Add(MakeItem("Image",     () => mainWindow.AddSunImageToSelectedNode(node, null)));
                menu.Items.Add(addSub);
                menu.Items.Add(new Separator());
                menu.Items.Add(MakeItem("Rename", () => mainWindow.RenameSelectedNode()));
                menu.Items.Add(MakeItem("Save",   async () => await mainWindow.SaveFileAsync()));
                menu.Items.Add(MakeItem("Unload", () => MainWindow.manager.UnloadSunFile((SunFile)node.Tag, node)));
                menu.Items.Add(MakeItem("Reload", async () =>
                    await MainWindow.manager.ReloadSunFileAsync((SunFile)node.Tag, node, System.Windows.Threading.Dispatcher.CurrentDispatcher)));

                if (node.ChildCount > 0)
                {
                    menu.Items.Add(new Separator());
                    menu.Items.Add(MakeItem("Expand All", () => ExpandRecursive(node, true)));
                    if (node.IsExpanded)
                        menu.Items.Add(MakeItem("Collapse All", () => ExpandRecursive(node, false)));
                }
            }
            else if (node.Tag is SunDirectory)
            {
                var addSub = new MenuItem { Header = "Add" };
                addSub.Items.Add(MakeItem("Directory", () => mainWindow.AddSunDirectoryToSelectedNode(node, null)));
                addSub.Items.Add(MakeItem("Image",     () => mainWindow.AddSunImageToSelectedNode(node, null)));
                menu.Items.Add(addSub);
                menu.Items.Add(new Separator());
                menu.Items.Add(MakeItem("Rename", () => mainWindow.RenameSelectedNode()));

                if (node.ChildCount > 0)
                    menu.Items.Add(MakeRemoveSubMenu());
                else
                    menu.Items.Add(MakeItem("Remove", () => mainWindow.RemoveSelectedNodes()));

                menu.Items.Add(new Separator());
                menu.Items.Add(MakeItem("Expand All",  () => ExpandRecursive(node, true)));
                if (node.IsExpanded)
                    menu.Items.Add(MakeItem("Collapse All", () => ExpandRecursive(node, false)));
            }
            else if (node.Tag is IPropertyContainer)
            {
                var addSub = new MenuItem { Header = "Add" };
                if (node.Tag is SunConvexProperty)
                {
                    addSub.Items.Add(MakeItem("SubProperty", () => mainWindow.AddSubPropertyToSelectedNode(node, null)));
                    addSub.Items.Add(MakeItem("Canvas",      () => mainWindow.AddCanvasPropertyToSelectedNode(node)));
                    addSub.Items.Add(MakeItem("Convex",      () => mainWindow.AddConvexPropertyToSelectedNode(node)));
                    addSub.Items.Add(MakeItem("Link",        () => mainWindow.AddLinkPropertyToSelectedNode(node)));
                    addSub.Items.Add(MakeItem("Sound",       () => mainWindow.AddSoundPropertyToSelectedNode(node)));
                    addSub.Items.Add(MakeItem("Vector",      () => mainWindow.AddVectorPropertyToSelectedNode(node)));
                }
                else
                {
                    addSub.Items.Add(MakeItem("SubProperty", () => mainWindow.AddSubPropertyToSelectedNode(node, null)));
                    var digitSub = new MenuItem { Header = "Digit" };
                    digitSub.Items.Add(MakeItem("Short   2 bytes", () => mainWindow.AddShortPropertyToSelectedNode(node)));
                    digitSub.Items.Add(MakeItem("Int     4 bytes", () => mainWindow.AddIntPropertyToSelectedNode(node)));
                    digitSub.Items.Add(MakeItem("Long    8 bytes", () => mainWindow.AddLongPropertyToSelectedNode(node)));
                    digitSub.Items.Add(new Separator());
                    digitSub.Items.Add(MakeItem("Float   4 bytes", () => mainWindow.AddFloatPropertyToSelectedNode(node)));
                    digitSub.Items.Add(MakeItem("Double  8 bytes", () => mainWindow.AddDoublePropertyToSelectedNode(node)));
                    addSub.Items.Add(digitSub);
                    addSub.Items.Add(MakeItem("Canvas",      () => mainWindow.AddCanvasPropertyToSelectedNode(node)));
                    addSub.Items.Add(MakeItem("Convex",      () => mainWindow.AddConvexPropertyToSelectedNode(node)));
                    addSub.Items.Add(MakeItem("Link",        () => mainWindow.AddLinkPropertyToSelectedNode(node)));
                    addSub.Items.Add(MakeItem("Sound",       () => mainWindow.AddSoundPropertyToSelectedNode(node)));
                    addSub.Items.Add(MakeItem("String",      () => mainWindow.AddStringPropertyToSelectedNode(node)));
                    addSub.Items.Add(MakeItem("Vector",      () => mainWindow.AddVectorPropertyToSelectedNode(node)));
                }
                menu.Items.Add(addSub);
                menu.Items.Add(new Separator());
                menu.Items.Add(MakeItem("Rename", () => mainWindow.RenameSelectedNode()));

                if (node.ChildCount > 0)
                    menu.Items.Add(MakeRemoveSubMenu());
                else
                    menu.Items.Add(MakeItem("Remove", () => mainWindow.RemoveSelectedNodes()));

                menu.Items.Add(new Separator());
                menu.Items.Add(MakeItem("Expand All", () => ExpandRecursive(node, true)));
                if (node.IsExpanded)
                    menu.Items.Add(MakeItem("Collapse All", () => ExpandRecursive(node, false)));
            }
            else if (node.Tag is SunProperty)
            {
                menu.Items.Add(MakeItem("Rename", () => mainWindow.RenameSelectedNode()));
                menu.Items.Add(MakeItem("Remove", () => mainWindow.RemoveSelectedNodes()));
            }

            return menu;
        }

        // ── Helpers ────────────────────────────────────────────────────────────────
        private MenuItem MakeItem(string header, Action onClick)
        {
            var item = new MenuItem { Header = header };
            item.Click += (_, __) => onClick();
            return item;
        }

        private MenuItem MakeItem(string header, Func<System.Threading.Tasks.Task> onClick)
        {
            var item = new MenuItem { Header = header };
            item.Click += async (_, __) => await onClick();
            return item;
        }

        private MenuItem MakeRemoveSubMenu()
        {
            var sub = new MenuItem { Header = "Remove..." };
            sub.Items.Add(MakeItem("This Node",       () => mainWindow.RemoveSelectedNodes()));
            sub.Items.Add(MakeItem("All Child Nodes", () => mainWindow.RemoveChildNodes()));
            return sub;
        }

        private void ExpandRecursive(SunNode node, bool expand)
        {
            node.IsExpanded = expand;
            foreach (SunNode child in node.Children)
                ExpandRecursive(child, expand);
        }

        private SunNode[] GetNodes(IReadOnlyList<SunNode> selected)
        {
            return selected.Count > 0 ? selected.ToArray() : new[] { currentNode };
        }
    }
}
