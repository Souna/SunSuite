using SunFileManager.GUI.Input;
using SunFileManager.GUI.Input.Forms;
using SunLibrary.SunFileLib.Properties;
using SunLibrary.SunFileLib.Structure;
using SunLibrary.SunFileLib.Util;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Threading;
using System.Windows.Media;
using Wpf.Ui.Appearance;
using Wpf.Ui.Markup;

namespace SunFileManager.GUI
{
    public partial class MainWindow : SunFluentWindow
    {
        // ── Statics ───────────────────────────────────────────────────────────────
        public static string DefaultPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        public static FileManager manager = null;

        // ── Tree data ─────────────────────────────────────────────────────────────
        public ObservableCollection<SunNode> TreeNodes { get; } = new ObservableCollection<SunNode>();

        // ── Multi-select ──────────────────────────────────────────────────────────
        private HashSet<SunNode> multiSelectedNodes = new HashSet<SunNode>();
        private SunNode lastAnchorNode = null;

        // ── Context menu ──────────────────────────────────────────────────────────
        private SunContextMenuManager contextMenuManager;

        // ── WM_COPYDATA ───────────────────────────────────────────────────────────
        private const uint WM_COPYDATA = 0x004A;

        [StructLayout(LayoutKind.Sequential)]
        private struct COPYDATASTRUCT
        {
            public IntPtr dwData;
            public int cbData;
            public IntPtr lpData;
        }

        // ── Constructor ───────────────────────────────────────────────────────────
        public MainWindow(string[] sunFilesToLoad)
        {
            InitializeComponent();

            sunTreeView.ItemsSource = TreeNodes;
            manager = new FileManager(this);
            contextMenuManager = new SunContextMenuManager(this);
            panningImageViewer.ImageDoubleClicked += PanningImageViewer_ImageDoubleClicked;

            Program.SetMainFormInstance(this);
            ApplyTheme();
            ApplySettings();

            SourceInitialized += (_, __) =>
            {
                var hwndSource = HwndSource.FromHwnd(new WindowInteropHelper(this).Handle);
                hwndSource?.AddHook(WndProc);
            };


            if (sunFilesToLoad != null && sunFilesToLoad.Length > 0)
            {
                Loaded += (_, __) =>
                {
                    foreach (string file in sunFilesToLoad)
                        LoadFile(file);
                };
            }
        }

        // ── Settings ──────────────────────────────────────────────────────────────
        public void ApplyTheme()
        {
            bool dark = Program.UserSettings.DarkMode;
            ApplicationTheme theme = dark ? ApplicationTheme.Dark : ApplicationTheme.Light;

            ApplicationThemeManager.Apply(theme);

            // Replace ALL ThemesDictionary instances — ApplicationThemeManager.Apply() may
            // add a second one at the end of MergedDictionaries, and if only the first is
            // replaced the last (stale) entry wins, leaving the wrong theme applied.
            var dicts = Application.Current.Resources.MergedDictionaries;
            for (int i = 0; i < dicts.Count; i++)
            {
                if (dicts[i] is ThemesDictionary)
                    dicts[i] = new ThemesDictionary { Theme = theme };
            }

            foreach (Window w in Application.Current.Windows)
            {
                if (w is SunFluentWindow sfw)
                    sfw.ApplyDwmTheme();
            }
        }

        public void ApplySettings()
        {
            panningImageViewer.ShowOriginCross = Program.UserSettings.ShowOriginCross;
        }

        // ── WM_COPYDATA (single-instance file loading) ────────────────────────────
        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == WM_COPYDATA)
            {
                var cds = (COPYDATASTRUCT)Marshal.PtrToStructure(lParam, typeof(COPYDATASTRUCT));
                string filePath = Marshal.PtrToStringAnsi(cds.lpData);
                if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath))
                    Dispatcher.InvokeAsync(() => LoadFile(filePath));
                handled = true;
            }
            return IntPtr.Zero;
        }

        // ── File operations ───────────────────────────────────────────────────────
        public async void OpenFiles()
        {
            string sunPath = Program.UserSettings.SunFilesPath;
            var ofd = new Microsoft.Win32.OpenFileDialog
            {
                Title = "Choose SunFiles",
                Filter = "SunFile|*.sun",
                Multiselect = true,
                InitialDirectory = Directory.Exists(sunPath) ? sunPath : null
            };
            if (ofd.ShowDialog() != true) return;

            Dispatcher dispatcher = Dispatcher.CurrentDispatcher;
            SunFile[] loadedFiles = await Task.WhenAll(ofd.FileNames.Select(p => manager.LoadSunFileAsync(p)));
            foreach (SunFile sunFile in loadedFiles.Where(f => f != null))
                manager.AddLoadedSunFileToTreeView(sunFile, dispatcher, null);
        }

        public async Task SaveFileAsync(SunNode node = null)
        {
            node ??= GetSelectedSunNode();

            if (node == null)
            {
                if (TreeNodes.Count > 0)
                    node = TreeNodes[0];
                else
                    return;
            }

            if (!(node.Tag is SunFile))
                node = node.TopLevelNode;

            await manager.SaveToDiskAsync(node);
        }

        public async void LoadFile(string sunfileToLoad)
        {
            if (string.IsNullOrEmpty(sunfileToLoad) || !File.Exists(sunfileToLoad))
            {
                if (!string.IsNullOrEmpty(sunfileToLoad))
                    MessageBox.Show($"File not found: {sunfileToLoad}", "File Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            SunFile f = await manager.LoadSunFileAsync(sunfileToLoad);
            if (f != null)
                manager.AddLoadedSunFileToTreeView(f, Dispatcher.CurrentDispatcher, null);
        }

        // ── Tree node helpers ─────────────────────────────────────────────────────
        public SunNode GetSelectedSunNode() => sunTreeView.SelectedItem as SunNode;
        public IReadOnlyList<SunNode> GetMultiSelectedNodes() => multiSelectedNodes.ToList();

        private void ParseOnTreeViewSelectedItem(SunNode selectedNode, bool expand = true)
        {
            if (selectedNode?.Tag is SunImage img)
            {
                if (!img.Parsed) img.ParseImage();
                selectedNode.Reparse();
                if (expand) selectedNode.IsExpanded = true;
            }
        }

        // ── Node manipulation (called by context menu) ────────────────────────────
        public void RemoveSelectedNodes(SunNode node = null)
        {
            node ??= GetSelectedSunNode();
            if (node == null) return;
            if (!ConfirmNodeDeletion(node)) return;
            node.DeleteNode();
        }

        public void RemoveChildNodes(SunNode parent = null)
        {
            parent ??= GetSelectedSunNode();
            if (parent == null) return;
            int count = parent.Children.Count;
            if (count > 0 && Program.UserSettings.NodeWarnings)
            {
                var result = MessageBox.Show(
                    $"This will permanently delete all {count} child node{(count == 1 ? "" : "s")} of \"{parent.Name}\". Continue?",
                    "Remove All Children", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result != MessageBoxResult.Yes) return;
            }
            foreach (SunNode node in parent.Children.ToList())
                node?.DeleteNode(true);
            parent.Children.Clear();
        }

        private static bool ConfirmNodeDeletion(SunNode node)
        {
            bool isBinaryData = node.Tag is SunCanvasProperty || node.Tag is SunSoundProperty;
            int childCount = node.Children.Count;

            if (!isBinaryData && childCount == 0)
                return true;

            if (!Program.UserSettings.NodeWarnings)
                return true;

            string message;
            if (isBinaryData && childCount > 0)
                message = $"Delete \"{node.Name}\"? This will remove its binary data and {childCount} child node{(childCount == 1 ? "" : "s")}.";
            else if (isBinaryData)
                message = $"Delete \"{node.Name}\"? This will permanently remove its binary data.";
            else
                message = $"Delete \"{node.Name}\"? This will also delete {childCount} child node{(childCount == 1 ? "" : "s")}.";

            return MessageBox.Show(message, "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning)
                   == MessageBoxResult.Yes;
        }

        public void RenameSelectedNode(SunNode node = null)
        {
            node ??= GetSelectedSunNode();
            if (node == null) return;

            string title = node.Tag?.ObjectType switch
            {
                SunObjectType.File => "Rename File",
                SunObjectType.Image => "Rename Image",
                SunObjectType.Directory => "Rename Directory",
                SunObjectType.Property => "Rename Property",
                _ => "Rename"
            };

            if (!frmNameInputBox.Show(title, out string newName, node.Name))
                return;

            if (node.Parent != null && node.Parent.Children.Any(
                    c => c != node && string.Equals(c.Name, newName, StringComparison.OrdinalIgnoreCase)))
            {
                MessageBox.Show($"A node named \"{newName}\" already exists at this level.", "Duplicate Name", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            node.Rename(newName);
            txtPropertyName.Text = newName;
        }

        // ── Adding directories & properties ───────────────────────────────────────
        public void AddSunDirectoryToSelectedNode(SunNode targetNode, string name)
        {
            if (targetNode == null) return;
            if (!(targetNode.Tag is SunDirectory) && !(targetNode.Tag is SunFile))
            {
                MessageBox.Show("Error: can't add Directory to " + targetNode.GetTypeName());
                return;
            }

            string dirName = name;
            if (string.IsNullOrEmpty(name))
                if (!frmNameInputBox.Show("Add Directory", out dirName)) return;

            SunObject obj = targetNode.Tag;
            if (obj is SunFile sunFileParent)
            {
                if (sunFileParent.SunDirectory == null)
                    CreateMasterDirectory(sunFileParent);
                targetNode.AddObject(new SunDirectory(dirName, sunFileParent.SunDirectory));
            }
            else if (obj is SunDirectory sunDirParent)
            {
                targetNode.AddObject(new SunDirectory(dirName, sunDirParent));
            }
        }

        public void AddSunImageToSelectedNode(SunNode targetNode, string name)
        {
            if (targetNode == null) return;
            if (!(targetNode.Tag is SunDirectory) && !(targetNode.Tag is SunFile))
            {
                MessageBox.Show("Error: can't add Image to " + targetNode.GetTypeName());
                return;
            }

            string imgName = name;
            if (string.IsNullOrEmpty(name))
                if (!frmNameInputBox.Show("Add Image", out imgName)) return;

            if (!imgName.EndsWith(".img")) imgName += ".img";

            if (targetNode.Tag is SunFile sunFile && sunFile.SunDirectory == null)
                CreateMasterDirectory(sunFile);

            targetNode.AddObject(new SunImage(imgName) { Changed = true });
        }

        public void CreateMasterDirectory(SunFile file) =>
            file.SunDirectory = new SunDirectory(file.Name, file);

        public void AddSubPropertyToSelectedNode(SunNode targetNode, string name)
        {
            if (!(targetNode.Tag is IPropertyContainer)) return;
            string subPropName = name;
            if (string.IsNullOrEmpty(name))
                if (!frmNameInputBox.Show("Add SubProperty", out subPropName)) return;
            targetNode.AddObject(new SunSubProperty(subPropName));
        }

        public void AddDoublePropertyToSelectedNode(SunNode targetNode)
        {
            if (!(targetNode.Tag is IPropertyContainer)) return;
            if (!frmFloatInputBox.Show("Add Double Value", out string name, out double? value)) return;
            targetNode.AddObject(new SunDoubleProperty(name, (double)value));
        }

        public void AddDoublePropertyToSelectedNode(SunNode targetNode, string name, double value)
        {
            if (!(targetNode.Tag is IPropertyContainer)) return;
            targetNode.AddObject(new SunDoubleProperty(name, value));
        }

        public void AddFloatPropertyToSelectedNode(SunNode targetNode)
        {
            if (!(targetNode.Tag is IPropertyContainer)) return;
            if (!frmFloatInputBox.Show("Add Float Value", out string name, out double? value)) return;
            targetNode.AddObject(new SunFloatProperty(name, (float)value));
        }

        public void AddFloatPropertyToSelectedNode(SunNode targetNode, string name, float value)
        {
            if (targetNode == null || !(targetNode.Tag is IPropertyContainer)) return;
            targetNode.AddObject(new SunFloatProperty(name, value));
        }

        public void AddCanvasPropertyToSelectedNode(SunNode targetNode)
        {
            if (targetNode == null || !(targetNode.Tag is IPropertyContainer)) return;
            if (!frmCanvasInputBox.Show("Add Image", out string name,
                out List<Bitmap> bitmaps, out List<int> gifFrameDelays,
                out List<Bitmap> gifFrames, out bool createSubProperty)) return;

            int imageIndex = 0;
            int lastImageIndex = targetNode.LastChild != null && !createSubProperty
                ? targetNode.Children.IndexOf(targetNode.LastChild) + 1 : 0;

            if (createSubProperty)
            {
                SunSubProperty subProp = new SunSubProperty(name);
                foreach (Bitmap bmp in bitmaps)
                {
                    var pngProp = new SunPngProperty(); pngProp.SetPNG(bmp);
                    var canvasProp = new SunCanvasProperty((lastImageIndex + imageIndex).ToString()) { PNG = pngProp };
                    canvasProp.AddProperty(new SunVectorProperty("origin", new SunIntProperty("X", 0), new SunIntProperty("Y", 0)));
                    subProp.AddProperty(canvasProp);
                    imageIndex++;
                }
                imageIndex = 0;
                foreach (Bitmap frame in gifFrames)
                {
                    var canvasProp = new SunCanvasProperty(imageIndex.ToString());
                    var pngProp = new SunPngProperty(); pngProp.SetPNG(frame);
                    canvasProp.PNG = pngProp;
                    canvasProp.AddProperty(new SunIntProperty("delay", gifFrameDelays[imageIndex]));
                    canvasProp.AddProperty(new SunVectorProperty("origin", new SunIntProperty("X", 0), new SunIntProperty("Y", 0)));
                    subProp.AddProperty(canvasProp);
                    imageIndex++;
                }
                targetNode.AddObject(subProp, true);
            }
            else
            {
                if (bitmaps.Count == 1)
                {
                    var pngProp = new SunPngProperty(); pngProp.SetPNG(bitmaps[0]);
                    var canvasProp = new SunCanvasProperty(
                        targetNode.ContainsKey(name) ? (lastImageIndex + imageIndex).ToString() : name) { PNG = pngProp };
                    canvasProp.AddProperty(new SunVectorProperty("origin", new SunIntProperty("X", 0), new SunIntProperty("Y", 0)));
                    targetNode.AddObject(canvasProp, false);
                }
                else
                {
                    foreach (Bitmap bmp in bitmaps)
                    {
                        var pngProp = new SunPngProperty(); pngProp.SetPNG(bmp);
                        var canvasProp = new SunCanvasProperty((lastImageIndex + imageIndex).ToString()) { PNG = pngProp };
                        canvasProp.AddProperty(new SunVectorProperty("origin", new SunIntProperty("X", 0), new SunIntProperty("Y", 0)));
                        targetNode.AddObject(canvasProp, false);
                        imageIndex++;
                    }
                    imageIndex = 0;
                    if (gifFrames.Count > 0)
                    {
                        var gifSub = new SunSubProperty(name);
                        foreach (Bitmap frame in gifFrames)
                        {
                            var c = new SunCanvasProperty(imageIndex.ToString());
                            var p = new SunPngProperty(); p.SetPNG(frame);
                            c.PNG = p;
                            c.AddProperty(new SunIntProperty("delay", gifFrameDelays[imageIndex]));
                            c.AddProperty(new SunVectorProperty("origin", new SunIntProperty("X", 0), new SunIntProperty("Y", 0)));
                            gifSub.AddProperty(c);
                            imageIndex++;
                        }
                        targetNode.AddObject(gifSub);
                    }
                }
            }
        }

        public void AddConvexPropertyToSelectedNode(SunNode targetNode)
        {
            if (!(targetNode.Tag is IPropertyContainer)) return;
            if (!frmNameInputBox.Show("Add Convex Property", out string name)) return;
            targetNode.AddObject(new SunConvexProperty(name));
        }

        public void AddIntPropertyToSelectedNode(SunNode targetNode)
        {
            if (targetNode == null || !(targetNode.Tag is IPropertyContainer)) return;
            if (!frmDigitInputBox.Show("Add Int Value", out string name, out int? value)) return;
            targetNode.AddObject(new SunIntProperty(name, (int)value));
        }

        public void AddIntPropertyToSelectedNode(SunNode targetNode, string name, int value)
        {
            if (targetNode == null || !(targetNode.Tag is IPropertyContainer)) return;
            targetNode.AddObject(new SunIntProperty(name, value));
        }

        public void AddLongPropertyToSelectedNode(SunNode targetNode)
        {
            if (targetNode == null || !(targetNode.Tag is IPropertyContainer)) return;
            if (!frmLongInputBox.Show("Add Long Value", out string name, out long? value)) return;
            targetNode.AddObject(new SunLongProperty(name, (long)value));
        }

        public void AddLongPropertyToSelectedNode(SunNode targetNode, string name, long value)
        {
            if (targetNode == null || !(targetNode.Tag is IPropertyContainer)) return;
            targetNode.AddObject(new SunLongProperty(name, value));
        }

        public void AddLinkPropertyToSelectedNode(SunNode targetNode)
        {
            if (targetNode == null || !(targetNode.Tag is IPropertyContainer)) return;
            if (!frmNameValueInputBox.Show("Add Link", out string name, out string value)) return;
            targetNode.AddObject(new SunLinkProperty(name, value));
        }

        public void AddShortPropertyToSelectedNode(SunNode targetNode)
        {
            if (targetNode == null || !(targetNode.Tag is IPropertyContainer)) return;
            if (!frmDigitInputBox.Show("Add Short Value", out string name, out int? value)) return;
            targetNode.AddObject(new SunShortProperty(name, (short)value));
        }

        public void AddShortPropertyToSelectedNode(SunNode targetNode, string name, short value)
        {
            if (targetNode == null || !(targetNode.Tag is IPropertyContainer)) return;
            targetNode.AddObject(new SunShortProperty(name, value));
        }

        public void AddSoundPropertyToSelectedNode(SunNode targetNode)
        {
            if (targetNode == null || !(targetNode.Tag is IPropertyContainer)) return;
            if (!frmSoundInputBox.Show("Add Sound", out string name, out string path)) return;
            targetNode.AddObject(new SunSoundProperty(name, path));
        }

        public void AddStringPropertyToSelectedNode(SunNode targetNode)
        {
            if (targetNode == null || !(targetNode.Tag is IPropertyContainer)) return;
            if (!frmNameValueInputBox.Show("Add String", out string name, out string value)) return;
            targetNode.AddObject(new SunStringProperty(name, value));
        }

        public void AddStringPropertyToSelectedNode(SunNode targetNode, string name, string value)
        {
            if (targetNode == null || !(targetNode.Tag is IPropertyContainer)) return;
            targetNode.AddObject(new SunStringProperty(name, value));
        }

        public void AddVectorPropertyToSelectedNode(SunNode targetNode)
        {
            if (targetNode == null || !(targetNode.Tag is IPropertyContainer)) return;
            if (!frmVectorInputBox.Show("Add Vector", out string name, out System.Drawing.Point? value)) return;
            targetNode.AddObject(new SunVectorProperty(name,
                new SunIntProperty("X", ((System.Drawing.Point)value).X),
                new SunIntProperty("Y", ((System.Drawing.Point)value).Y)));
        }

        public void AddVectorPropertyToSelectedNode(SunNode targetNode, string name, System.Drawing.Point value)
        {
            if (targetNode == null || !(targetNode.Tag is IPropertyContainer)) return;
            targetNode.AddObject(new SunVectorProperty(name, value));
        }

        // ── Display node value ────────────────────────────────────────────────────
        public void DisplayNodeValue(SunObject obj)
        {
            // Reset all
            nameRow.Visibility = Visibility.Collapsed;
            scalarValueRow.Visibility = Visibility.Collapsed;
            multiValueRow.Visibility = Visibility.Collapsed;
            vectorRow.Visibility = Visibility.Collapsed;
            panningImageViewer.Visibility = Visibility.Collapsed;
            panningImageViewer.Canvas = null;
            panningImageViewer.OriginPoint = null;
            soundPlayer.Visibility = Visibility.Collapsed;
            soundPlayer.SoundProperty = null;
            btnApplyPropertyChanges.Visibility = Visibility.Collapsed;
            txtPropertyName.Text = "";
            txtPropertyValue.Text = "";
            txtPropertyValueMulti.Text = "";

            switch (obj)
            {
                case SunShortProperty sp:
                    ShowScalar(sp.Name, sp.Value.ToString()); break;
                case SunIntProperty ip:
                    ShowScalar(ip.Name, ip.Value.ToString()); break;
                case SunLongProperty lp:
                    ShowScalar(lp.Name, lp.Value.ToString()); break;
                case SunFloatProperty fp:
                    ShowScalar(fp.Name, fp.Value.ToString()); break;
                case SunDoubleProperty dp:
                    ShowScalar(dp.Name, dp.Value.ToString()); break;

                case SunStringProperty strp:
                    nameRow.Visibility = Visibility.Visible;
                    multiValueRow.Visibility = Visibility.Visible;
                    btnApplyPropertyChanges.Visibility = Visibility.Visible;
                    txtPropertyName.Text = strp.Name;
                    txtPropertyValueMulti.Text = strp.Value;
                    break;

                case SunLinkProperty linkp:
                    nameRow.Visibility = Visibility.Visible;
                    multiValueRow.Visibility = Visibility.Visible;
                    btnApplyPropertyChanges.Visibility = Visibility.Visible;
                    txtPropertyName.Text = linkp.Name;
                    txtPropertyValueMulti.Text = linkp.Value;
                    break;

                case SunSubProperty subp
                    when subp.SunProperties.Count == 1 && subp.SunProperties[0] is SunCanvasProperty:
                    var soloCanvas = (SunCanvasProperty)subp.SunProperties[0];
                    panningImageViewer.Visibility = Visibility.Visible;
                    panningImageViewer.Canvas = soloCanvas.GetBitmap();
                    var soloOrigin = soloCanvas["origin"] as SunLibrary.SunFileLib.Properties.SunVectorProperty;
                    panningImageViewer.OriginPoint = soloOrigin != null
                        ? new System.Drawing.Point(soloOrigin.X.Value, soloOrigin.Y.Value)
                        : (System.Drawing.Point?)null;
                    break;

                case SunCanvasProperty canvasp:
                    panningImageViewer.Visibility = Visibility.Visible;
                    panningImageViewer.Canvas = canvasp.GetBitmap();
                    var originProp = canvasp["origin"] as SunLibrary.SunFileLib.Properties.SunVectorProperty;
                    panningImageViewer.OriginPoint = originProp != null
                        ? new System.Drawing.Point(originProp.X.Value, originProp.Y.Value)
                        : (System.Drawing.Point?)null;
                    break;

                case SunVectorProperty vecp:
                    nameRow.Visibility = Visibility.Visible;
                    vectorRow.Visibility = Visibility.Visible;
                    btnApplyPropertyChanges.Visibility = Visibility.Visible;
                    txtPropertyName.Text = vecp.Name;
                    txtVectorX.Text = vecp.X.Value.ToString();
                    txtVectorY.Text = vecp.Y.Value.ToString();
                    break;

                case SunSoundProperty soundp:
                    soundPlayer.Visibility = Visibility.Visible;
                    soundPlayer.SoundProperty = soundp;
                    break;
            }

            UpdateSelectionTypeLabel();
        }

        private void ShowScalar(string name, string value)
        {
            nameRow.Visibility = Visibility.Visible;
            scalarValueRow.Visibility = Visibility.Visible;
            btnApplyPropertyChanges.Visibility = Visibility.Visible;
            txtPropertyName.Text = name;
            txtPropertyValue.Text = value;
        }

        private void PanningImageViewer_ImageDoubleClicked(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (GetSelectedSunNode()?.Tag is not SunCanvasProperty canvasp) return;

            var ofd = new Microsoft.Win32.OpenFileDialog
            {
                Title = "Replace Image",
                Filter = "Image Files|*.png;*.jpg;*.jpeg;*.bmp|All Files|*.*"
            };
            if (ofd.ShowDialog() != true) return;

            Bitmap bmp;
            try { bmp = new Bitmap(ofd.FileName); }
            catch { return; }

            var newPng = new SunPngProperty();
            newPng.SetPNG(bmp);
            canvasp.PNG = newPng;
            canvasp.SetValue(null);
            panningImageViewer.Canvas = canvasp.GetBitmap();
        }

        private void UpdateSelectionTypeLabel()
        {
            SunNode node = GetSelectedSunNode();
            lblSelectedNodeType.Text = node != null
                ? "Selection Type: " + node.GetTypeName()
                : "Selection Type: ";
        }

        // ── Apply property changes ────────────────────────────────────────────────
        private void btnApplyPropertyChanges_Click(object sender, RoutedEventArgs e)
        {
            SunNode node = GetSelectedSunNode();
            if (node == null) return;
            SunObject obj = node.Tag;

            switch (obj)
            {
                case SunShortProperty sp:
                    if (short.TryParse(txtPropertyValue.Text, out short sv)) sp.Value = sv; break;
                case SunIntProperty ip:
                    if (int.TryParse(txtPropertyValue.Text, out int iv)) ip.Value = iv; break;
                case SunLongProperty lp:
                    if (long.TryParse(txtPropertyValue.Text, out long lv)) lp.Value = lv; break;
                case SunFloatProperty fp:
                    if (float.TryParse(txtPropertyValue.Text, out float fv)) fp.Value = fv; break;
                case SunDoubleProperty dp:
                    if (double.TryParse(txtPropertyValue.Text, out double dv)) dp.Value = dv; break;
                case SunStringProperty strp:
                    strp.Value = txtPropertyValueMulti.Text; break;
                case SunLinkProperty linkp:
                    linkp.Value = txtPropertyValueMulti.Text; break;
                case SunVectorProperty vecp:
                    if (int.TryParse(txtVectorX.Text, out int xv)) vecp.X.Value = xv;
                    if (int.TryParse(txtVectorY.Text, out int yv)) vecp.Y.Value = yv;
                    break;
            }
            if (node.Tag is SunProperty changedProp && changedProp.ParentImage != null)
                changedProp.ParentImage.Changed = true;
            node.IsManuallyAdded = true;
        }

        // ── TreeView events ───────────────────────────────────────────────────────
        private void sunTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            SunNode newNode = e.NewValue as SunNode;
            bool ctrl = Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl);
            bool shift = Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift);

            if (!ctrl && !shift)
            {
                ClearHighlights();
                if (newNode != null)
                {
                    newNode.IsHighlighted = true;
                    multiSelectedNodes.Add(newNode);
                    lastAnchorNode = newNode;
                }
            }

            if (newNode?.Tag != null)
                DisplayNodeValue(newNode.Tag);
        }

        private void sunTreeView_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton != MouseButton.Left) return;
            bool ctrl = Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl);

            if (ctrl)
            {
                SunNode node = GetNodeAtMouse(e);
                if (node != null)
                {
                    if (multiSelectedNodes.Contains(node))
                    {
                        node.IsHighlighted = false;
                        multiSelectedNodes.Remove(node);
                    }
                    else
                    {
                        node.IsHighlighted = true;
                        multiSelectedNodes.Add(node);
                    }
                }
            }
        }

        private SunNode GetNodeAtMouse(MouseEventArgs e)
        {
            var result = VisualTreeHelper.HitTest(sunTreeView, e.GetPosition(sunTreeView));
            if (result == null) return null;
            var dep = result.VisualHit as DependencyObject;
            while (dep != null)
            {
                if (dep is TreeViewItem tvi)
                    return tvi.DataContext as SunNode;
                dep = VisualTreeHelper.GetParent(dep);
            }
            return null;
        }

        private void ClearHighlights()
        {
            foreach (SunNode n in multiSelectedNodes)
                n.IsHighlighted = false;
            multiSelectedNodes.Clear();
        }

        private void sunTreeView_KeyDown(object sender, KeyEventArgs e)
        {
            SunNode node = GetSelectedSunNode();
            if (node == null) return;

            if (e.Key == Key.Enter || e.Key == Key.Right)
            {
                if (node.Tag is SunImage && node.ChildCount == 0)
                    ParseOnTreeViewSelectedItem(node);
            }

            // Shift+Arrow multi-select
            if (e.KeyboardDevice.Modifiers == ModifierKeys.Shift &&
                (e.Key == Key.Up || e.Key == Key.Down))
            {
                e.Handled = true;
            }
        }

        private void sunTreeView_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            SunNode node = GetSelectedSunNode();
            if (node?.Tag is SunImage && node.ChildCount == 0)
                ParseOnTreeViewSelectedItem(node);
        }

        // Tracks which node was right-clicked for ContextMenuOpening
        private SunNode _contextMenuNode;

        private void sunTreeView_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            _contextMenuNode = GetNodeAtMouse(e);

            if (_contextMenuNode != null && !multiSelectedNodes.Contains(_contextMenuNode))
            {
                ClearHighlights();
                _contextMenuNode.IsHighlighted = true;
                multiSelectedNodes.Add(_contextMenuNode);
                lastAnchorNode = _contextMenuNode;
            }
            // Do not mark handled — let ContextMenuOpening fire naturally
        }

        private void sunTreeView_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            var selectedList = multiSelectedNodes.ToList();
            sunTreeView.ContextMenu = contextMenuManager.BuildContextMenu(_contextMenuNode, selectedList);
        }

        // ── Drag-drop ─────────────────────────────────────────────────────────────
        private void Window_DragEnter(object sender, DragEventArgs e)
        {
            e.Effects = e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.Copy : DragDropEffects.None;
            e.Handled = true;
        }

        private void Window_Drop(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return;
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (string file in files)
                if (Path.GetExtension(file).Equals(".sun", StringComparison.OrdinalIgnoreCase))
                    LoadFile(file);
            e.Handled = true;
        }

        private void sunTreeView_DragEnter(object sender, DragEventArgs e) => Window_DragEnter(sender, e);
        private void sunTreeView_Drop(object sender, DragEventArgs e) => Window_Drop(sender, e);

        // ── Menu handlers ─────────────────────────────────────────────────────────
        private void menuNew_Click(object sender, RoutedEventArgs e)
            => new frmNewFile(this).ShowDialog();

        private void menuOpen_Click(object sender, RoutedEventArgs e) => OpenFiles();

        private async void menuSave_Click(object sender, RoutedEventArgs e) => await SaveFileAsync();

        private void menuExport_Click(object sender, RoutedEventArgs e)
        {
            SunNode node = GetSelectedSunNode();
            if (node == null || !(node.Tag is SunObject obj)) return;

            string outPath = DefaultPath;
            var serializer = new SunPngMp3Serializer();
            var thread = new Thread(() => ((ISunObjectSerializer)serializer).SerializeObject(obj, outPath));
            thread.IsBackground = true;
            thread.Start();
        }

        private void menuAddDirectory_Click(object sender, RoutedEventArgs e)
            => AddSunDirectoryToSelectedNode(GetSelectedSunNode(), null);

        private void menuAddImage_Click(object sender, RoutedEventArgs e)
            => AddSunImageToSelectedNode(GetSelectedSunNode(), null);

        private void menuSettings_Click(object sender, RoutedEventArgs e)
            => new frmSettings(this).ShowDialog();

        private void menuHelp_Click(object sender, RoutedEventArgs e)
            => new frmHelp().ShowDialog();

        private void menuAbout_Click(object sender, RoutedEventArgs e)
            => new frmHelp().ShowDialog();

        // ── Keyboard shortcuts ────────────────────────────────────────────────────
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.Key == Key.S && Keyboard.Modifiers == ModifierKeys.Control)
            {
                _ = SaveFileAsync();
                e.Handled = true;
            }
            if (e.Key == Key.P &&
                Keyboard.Modifiers == (ModifierKeys.Control | ModifierKeys.Alt | ModifierKeys.Shift))
            {
                new frmPeterAlert { Owner = this }.ShowDialog();
                e.Handled = true;
            }
        }

        // ── Visual tree helper ────────────────────────────────────────────────────
        private static DependencyObject VisualTreeHelper_GetParent(DependencyObject obj)
            => System.Windows.Media.VisualTreeHelper.GetParent(obj);
    }
}
