using SunLibrary.SunFileLib.Properties;
using SunLibrary.SunFileLib.Structure;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SunFileManager
{
    /// <summary>
    /// WPF-compatible tree node that wraps a SunObject and provides INotifyPropertyChanged
    /// for data binding to a WPF TreeView.
    /// </summary>
    public class SunNode : INotifyPropertyChanged
    {
        private string _name;
        private bool _isExpanded;
        private bool _isManuallyAdded;
        private bool _isHighlighted;
        private int _imageIndex;
        private SunObject _tag;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string prop = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(); }
        }

        public bool IsExpanded
        {
            get => _isExpanded;
            set { _isExpanded = value; OnPropertyChanged(); }
        }

        public bool IsManuallyAdded
        {
            get => _isManuallyAdded;
            set { _isManuallyAdded = value; OnPropertyChanged(); }
        }

        /// <summary>Used to highlight multi-selected nodes in the TreeView.</summary>
        public bool IsHighlighted
        {
            get => _isHighlighted;
            set { _isHighlighted = value; OnPropertyChanged(); }
        }

        public int ImageIndex
        {
            get => _imageIndex;
            set { _imageIndex = value; OnPropertyChanged(); OnPropertyChanged(nameof(IconSource)); }
        }

        public SunObject Tag
        {
            get => _tag;
            set { _tag = value; OnPropertyChanged(); }
        }

        public SunNode Parent { get; set; }
        public ObservableCollection<SunNode> Children { get; } = new ObservableCollection<SunNode>();

        public int Level
        {
            get
            {
                int level = 0;
                SunNode p = Parent;
                while (p != null) { level++; p = p.Parent; }
                return level;
            }
        }

        public int ChildCount => Children.Count;
        public SunNode LastChild => Children.Count > 0 ? Children[Children.Count - 1] : null;

        // ── Icon ──────────────────────────────────────────────────────────────────
        private static readonly string[] IconUris =
        {
            "pack://application:,,,/Resources/sun.ico",     // 0: File
            "pack://application:,,,/Resources/3d.png",      // 1: Image
            "pack://application:,,,/Resources/folder.png",  // 2: Directory
            "pack://application:,,,/Resources/Input.png",   // 3: Short / Int / Long
            "pack://application:,,,/Resources/Decimal.png", // 4: Float / Double
            "pack://application:,,,/Resources/String.png",  // 5: String / Link
            "pack://application:,,,/Resources/Image.png",   // 6: Canvas / Png
            "pack://application:,,,/Resources/Vector.png",  // 7: Vector
            "pack://application:,,,/Resources/Sound.png",   // 8: Sound
            "pack://application:,,,/Resources/Property.png",// 9: SubProperty / Convex
            "pack://application:,,,/Resources/Property.png",// 10: Default
        };

        private static readonly Dictionary<int, ImageSource> _iconCache = new Dictionary<int, ImageSource>();

        public ImageSource IconSource
        {
            get
            {
                int idx = Math.Clamp(ImageIndex, 0, IconUris.Length - 1);
                if (!_iconCache.TryGetValue(idx, out ImageSource src))
                {
                    src = new BitmapImage(new Uri(IconUris[idx]));
                    _iconCache[idx] = src;
                }
                return src;
            }
        }

        // ── Constructors ──────────────────────────────────────────────────────────
        public SunNode(SunObject sourceObject, bool isManuallyAdded = false)
        {
            Name = sourceObject.Name;
            IsManuallyAdded = isManuallyAdded;
            SetNodeImage(sourceObject);
            ParseChilds(sourceObject);
        }

        public SunNode() { }

        // ── Image index ───────────────────────────────────────────────────────────
        public void SetNodeImage(SunObject obj)
        {
            switch (obj.ObjectType)
            {
                case SunObjectType.File:
                    ImageIndex = 0; break;
                case SunObjectType.Image:
                    ImageIndex = 1; break;
                case SunObjectType.Directory:
                    ImageIndex = 2; break;
                case SunObjectType.Property:
                    switch (((SunProperty)obj).PropertyType)
                    {
                        case SunPropertyType.Short:
                        case SunPropertyType.Int:
                        case SunPropertyType.Long:
                            ImageIndex = 3; break;
                        case SunPropertyType.Float:
                        case SunPropertyType.Double:
                            ImageIndex = 4; break;
                        case SunPropertyType.String:
                        case SunPropertyType.Link:
                            ImageIndex = 5; break;
                        case SunPropertyType.Canvas:
                        case SunPropertyType.Png:
                            ImageIndex = 6; break;
                        case SunPropertyType.Vector:
                            ImageIndex = 7; break;
                        case SunPropertyType.Sound:
                            ImageIndex = 8; break;
                        case SunPropertyType.SubProperty:
                        case SunPropertyType.Convex:
                            ImageIndex = 9; break;
                        default:
                            ImageIndex = 10; break;
                    }
                    break;
            }
        }

        // ── Tree building ─────────────────────────────────────────────────────────
        public void ParseChilds(SunObject sourceObject)
        {
            if (sourceObject == null)
                throw new NullReferenceException("Can't create null SunNode.");

            Tag = sourceObject;

            if (sourceObject is SunFile file)
                sourceObject = file.SunDirectory;

            if (sourceObject is SunDirectory directory)
            {
                foreach (SunDirectory dir in directory.subDirs)
                    AddChild(new SunNode(dir));

                foreach (SunImage img in directory.SunImages)
                    AddChild(new SunNode(img));
            }

            if (sourceObject is SunImage image)
            {
                if (image.Parsed)
                {
                    foreach (SunProperty prop in image.SunProperties)
                        AddChild(new SunNode(prop));
                }
            }
            else if (sourceObject is IPropertyContainer container)
            {
                foreach (SunProperty prop in container.SunProperties)
                    AddChild(new SunNode(prop));
            }
        }

        private void AddChild(SunNode child)
        {
            child.Parent = this;
            Children.Add(child);
        }

        private void TryParseImage(bool reparseImage = true)
        {
            if (Tag is SunImage image)
            {
                image.ParseImage();
                if (reparseImage) Reparse();
            }
        }

        public void Reparse()
        {
            Children.Clear();
            ParseChilds(Tag);
        }

        // ── Adding objects ────────────────────────────────────────────────────────
        public SunNode AddObject(SunObject newObject, bool expandNode = true)
        {
            if (CanNodeBeInserted(this, newObject.Name))
            {
                if (Tag is SunImage)
                    TryParseImage();
                if (AddObjectInternal(newObject))
                {
                    SunNode node = new SunNode(newObject, true);
                    AddChild(node);

                    if (node.Tag is SunProperty property)
                        property.ParentImage.Changed = true;

                    IsManuallyAdded = true;

                    if (expandNode)
                        IsExpanded = true;

                    return node;
                }
                return null;
            }
            else
            {
                System.Windows.MessageBox.Show(
                    "Can't insert " + newObject.Name + " because another object with the same name already exists.");
                return null;
            }
        }

        public static bool CanNodeBeInserted(SunNode parentNode, string name)
        {
            SunObject obj = parentNode.Tag;
            if (obj is IPropertyContainer container)
                return container[name] == null;
            else if (obj is SunDirectory directory)
                return directory[name] == null;
            else if (obj is SunFile file)
                return file.SunDirectory[name] == null;
            else return false;
        }

        public static bool CanNodeBeInserted(SunNode parentNode, SunNode newNode)
        {
            SunObject obj = parentNode.Tag;
            if (obj is IPropertyContainer container)
                return container[newNode.Name] == null;
            else if (obj is SunDirectory directory)
                return directory[newNode.Name] == null;
            else if (obj is SunFile file)
                return file.SunDirectory[newNode.Name] == null;
            else return false;
        }

        private bool AddObjectInternal(SunObject newObject)
        {
            SunObject selectedObject = Tag;

            if (selectedObject is SunFile file) selectedObject = file.SunDirectory;
            if (selectedObject is SunDirectory directory)
            {
                if (newObject is SunDirectory newDir)
                    directory.AddDirectory(newDir);
                else if (newObject is SunImage image)
                    directory.AddImage(image);
                else
                    return false;
            }
            else if (selectedObject is SunImage image)
            {
                if (!image.Parsed) image.ParseImage();
                if (newObject is SunProperty newProp)
                {
                    image.AddProperty(newProp);
                    image.Changed = true;
                }
                else return false;
            }
            else if (selectedObject is IPropertyContainer propertyContainer)
            {
                if (newObject is SunProperty newProp)
                {
                    propertyContainer.AddProperty(newProp);
                    if (selectedObject is SunProperty prop)
                        prop.ParentImage.Changed = true;
                }
                else return false;
            }
            else return false;

            return true;
        }

        // ── Navigation helpers ────────────────────────────────────────────────────
        public SunNode TopLevelNode
        {
            get
            {
                SunNode parent = this;
                while (parent.Parent != null)
                    parent = parent.Parent;
                return parent;
            }
        }

        public string GetTypeName()
        {
            try
            {
                if (Parent != null && Parent.Tag is SunDirectory && Tag is SunDirectory)
                    return "Nested " + SunObjectType.Directory.ToString();
                else if (Parent != null && Tag is SunCanvasProperty img)
                {
                    if (img.IsGif) return "Animated " + SunPropertyType.Canvas.ToString();
                    else return SunPropertyType.Canvas.ToString();
                }
                return Tag.GetType().Name;
            }
            catch
            {
                return "e";
            }
        }

        // ── Mutation ──────────────────────────────────────────────────────────────
        public void DeleteNode(bool removeAllChildNodes = false)
        {
            try
            {
                if (Tag is SunProperty prop && prop.ParentImage != null)
                    prop.ParentImage.Changed = true;

                Tag.Remove();

                if (removeAllChildNodes)
                    Children.Clear();
                else
                    Parent?.Children.Remove(this);

                if (Parent != null)
                    Parent.IsManuallyAdded = true;
            }
            catch
            {
                throw new Exception("Error occurred at DeleteNode()");
            }
        }

        public void Rename(string newName)
        {
            if (Tag is SunFile && !newName.EndsWith(".sun"))
                newName += ".sun";

            if (Tag is SunImage img && !newName.EndsWith(".img"))
            {
                newName += ".img";
                img.Changed = true;
            }

            Name = newName;
            Tag.Name = newName;

            if (Tag is SunProperty prop)
                prop.ParentImage.Changed = true;

            IsManuallyAdded = true;
        }

        // ── Sorting ───────────────────────────────────────────────────────────────
        public void SortChildrenRecursively()
        {
            SortChildren();
            foreach (SunNode child in Children)
                child.SortChildrenRecursively();
        }

        private void SortChildren()
        {
            var sorted = new List<SunNode>(Children);
            sorted.Sort((a, b) =>
            {
                bool aNum = int.TryParse(a.Name, out int aVal);
                bool bNum = int.TryParse(b.Name, out int bVal);
                if (aNum && bNum) return aVal.CompareTo(bVal);
                if (aNum) return -1;
                if (bNum) return 1;
                return string.Compare(a.Name, b.Name, StringComparison.OrdinalIgnoreCase);
            });
            Children.Clear();
            foreach (SunNode node in sorted)
                Children.Add(node);
        }

        // ── Indexers ──────────────────────────────────────────────────────────────
        public SunNode this[string name]
        {
            get
            {
                foreach (SunNode node in Children)
                    if (node.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                        return node;
                return null;
            }
        }

        public bool ContainsKey(string name) => this[name] != null;
    }
}
