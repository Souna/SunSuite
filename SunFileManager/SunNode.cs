using System;
using System.Drawing;
using System.Windows.Forms;
using SunLibrary.SunFileLib.Properties;
using SunLibrary.SunFileLib.Structure;
using SunFileManager.GUI;

namespace SunFileManager
{
    /// <summary>
    /// Class which inherits from TreeNode.
    /// <para>Provides methods for parsing child SunNodes, adding and removing SunNodes,
    /// <br>returning top level node of current selection, and returning a SunNode's underlying type.</br></para>
    /// </summary>
    public class SunNode : TreeNode
    {
        private bool isSunObjectAddedManually = false;
        public static Color NewObjectForeColor = Color.Red;

        public SunNode(SunObject sourceObject, bool isSunObjectAddedManually = false) : base(sourceObject.Name)
        {
            Name = sourceObject.Name;
            this.isSunObjectAddedManually = isSunObjectAddedManually;
            if (isSunObjectAddedManually)
            {
                ForeColor = NewObjectForeColor;
            }

            SetNodeImage(sourceObject);
            ParseChilds(sourceObject);
        }

        public void SetNodeImage(SunObject obj)
        {
            switch (obj.ObjectType)
            {
                case SunObjectType.File:
                    ImageIndex = 0;
                    SelectedImageIndex = 0;
                    break;

                case SunObjectType.Image:
                    ImageIndex = 1;
                    SelectedImageIndex = 1;
                    break;

                case SunObjectType.Directory:
                    ImageIndex = 2;
                    SelectedImageIndex = 2;
                    break;

                case SunObjectType.Property:
                    switch (((SunProperty)obj).PropertyType)
                    {
                        case SunPropertyType.Short:
                        case SunPropertyType.Int:
                        case SunPropertyType.Long:
                            ImageIndex = 3;
                            SelectedImageIndex = 3;
                            break;

                        case SunPropertyType.Float:
                        case SunPropertyType.Double:
                            ImageIndex = 4;
                            SelectedImageIndex = 4;
                            break;

                        case SunPropertyType.String:
                            ImageIndex = 5;
                            SelectedImageIndex = 5;
                            break;

                        case SunPropertyType.Canvas:
                        case SunPropertyType.Png:
                            ImageIndex = 6;
                            SelectedImageIndex = 6;
                            break;

                        case SunPropertyType.Vector:
                            ImageIndex = 7;
                            SelectedImageIndex = 7;
                            break;

                        case SunPropertyType.Sound:
                            ImageIndex = 8;
                            SelectedImageIndex = 8;
                            break;

                        case SunPropertyType.SubProperty:
                            ImageIndex = 9;
                            SelectedImageIndex = 9;
                            break;

                        default:
                            ImageIndex = 10;
                            SelectedImageIndex = 10;
                            break;
                    }
                    break;
            }
        }

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
                {
                    Nodes.Add(new SunNode(dir));
                }

                foreach (SunImage img in directory.SunImages)
                {
                    Nodes.Add(new SunNode(img));
                }
            }

            if (sourceObject is SunImage image)
            {
                if (image.Parsed)
                {
                    foreach (SunProperty prop in image.SunProperties)
                    {
                        Nodes.Add(new SunNode(prop));
                    }
                }
            }
            else if (sourceObject is IPropertyContainer container)
            {
                foreach (SunProperty prop in container.SunProperties)
                {
                    Nodes.Add(new SunNode(prop));
                }
            }
        }

        private void TryParseImage(bool reparseImage = true)
        {
            if (Tag is SunImage image)
            {
                image.ParseImage();
                if (reparseImage)
                    Reparse();
            }
        }

        public void Reparse()
        {
            Nodes.Clear();
            ParseChilds((SunObject)Tag);
        }

        /// <summary>
        /// Creates a new node on the tree with the <b>SunObject</b> type.
        /// </summary>
        public SunNode AddObject(SunObject newObject, bool expandNode = true)
        {
            if (CanNodeBeInserted(this, newObject.Name))
            {
                if (Tag is SunImage)
                    TryParseImage();
                if (AddObjectInternal(newObject))
                {
                    SunNode node = new SunNode(newObject, true);
                    Nodes.Add(node);
                    if (node.Tag is SunProperty property)
                    {
                        property.ParentImage.Changed = true;
                    }
                    node.EnsureVisible();

                    if (expandNode)
                    {
                        node.Expand();
                    }

                    return node;
                }
                else return null;
            }
            else
            {
                MessageBox.Show("Can't insert " + newObject.Name + " because another object with the same name already exists.");
                return null;
            }
        }

        /// <summary>
        /// Checks for duplicates.
        /// </summary>
        public static bool CanNodeBeInserted(SunNode parentNode, string name)
        {
            SunObject obj = (SunObject)parentNode.Tag;
            if (obj is IPropertyContainer container)
                return container[name] == null;
            else if (obj is SunDirectory directory)
                return directory[name] == null;
            else if (obj is SunFile file)
                return file.SunDirectory[name] == null;
            else return false;
        }

        /// <summary>
        /// Checks for duplicates
        /// </summary>
        public static bool CanNodeBeInserted(SunNode parentNode, SunNode newNode)
        {
            SunObject obj = (SunObject)parentNode.Tag;
            if (obj is IPropertyContainer container)
                return container[newNode.Name] == null;
            else if (obj is SunDirectory directory)
                return directory[newNode.Name] == null;
            else if (obj is SunFile file)
                return file.SunDirectory[newNode.Name] == null;
            else return false;
        }

        /// <summary>
        /// Adds a <b>SunObject</b> as a child of the selected node.
        /// </summary>
        private bool AddObjectInternal(SunObject newObject)
        {
            SunObject selectedObject = (SunObject)Tag;  // Selected node.

            // If selectedObject is a SunFile, selectedObject becomes the master SunDirectory of that SunFile.
            if (selectedObject is SunFile file) selectedObject = file.SunDirectory;
            if (selectedObject is SunDirectory directory)
            {
                if (newObject is SunDirectory newDir)
                {
                    directory.AddDirectory(newDir);
                }
                else if (newObject is SunImage image)
                {
                    directory.AddImage(image);
                }
                else
                {
                    return false;
                }
            }
            else if (selectedObject is SunImage image)
            {
                if (!image.Parsed) image.ParseImage();
                if (newObject is SunProperty newProp)
                {
                    image.AddProperty(newProp);
                    image.Changed = true;
                }
                else
                {
                    return false;
                }
            }
            else if (selectedObject is IPropertyContainer propertyContainer)
            {
                if (newObject is SunProperty newProp)
                {
                    propertyContainer.AddProperty(newProp);
                    if (selectedObject is SunProperty prop)
                    {
                        prop.ParentImage.Changed = true;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Returns the top-level parent node of the selected node.
        /// </summary>
        public SunNode TopLevelNode
        {
            get
            {
                SunNode parent = this;
                while (parent.Level > 0)
                {
                    parent = (SunNode)parent.Parent;
                }
                return parent;
            }
        }

        /// <summary>
        /// Returns a string of the selected node's underlying type.
        /// </summary>
        public string GetTypeName()
        {
            try
            {
                // Selected a nested directory.
                if (Parent != null && Parent.Tag is SunDirectory && Tag is SunDirectory)
                    return "Nested " + SunObjectType.Directory.ToString();
                //  Selected a node for a canvas.
                else if (Parent != null && Tag is SunCanvasProperty img)
                {
                    // Selected a gif node.
                    if (img.IsGif) return "Animated " + SunPropertyType.Canvas.ToString();
                    else return SunPropertyType.Canvas.ToString();
                }

                return Tag.GetType().Name;
            }
            catch (Exception)
            {
                return "e";
            }
        }

        /// <summary>
        /// Calls the <b>Remove()</b> method for whichever type
        /// <br>the selected node belongs.</br>
        /// </summary>
        public void DeleteNode(bool removeAllChildNodes = false)
        {
            try
            {
                if (Tag is SunProperty prop)
                {
                    prop.ParentImage.Changed = true;
                }
                ((SunObject)Tag).Remove();
                if (removeAllChildNodes)
                    Nodes.Clear();
                else
                    Remove(); //Delete from Tree.
            }
            catch (Exception e)
            {
                throw new Exception(message: "Error occured at DeleteNode()");
            }
        }

        /// <summary>
        /// Changes the name of a selected node.
        /// </summary>
        public void Rename(string newName)
        {
            if (Tag is SunFile && !newName.EndsWith(".sun"))
                newName += ".sun";

            if (Tag is SunImage img && !newName.EndsWith(".img"))
            {
                newName += ".img";
                img.Changed = true;
            }

            Text = newName; // Change the displayed name.

            ((SunObject)Tag).Name = newName;    // Change internal name.

            if (Tag is SunProperty prop)
            {
                prop.ParentImage.Changed = true;
            }
            isSunObjectAddedManually = true;
            ForeColor = NewObjectForeColor;
        }

        /// <summary>
        /// Get a SunNode by name.
        /// </summary>
        public SunNode this[string name]
        {
            get
            {
                foreach (SunNode node in Nodes)
                    if (node.Name.ToLower() == name.ToLower())
                        return node;
                return null;
            }
        }

        public SunNode this[string name, SunObjectType type]
        {   // Using a getter to create something new feels disgusting
            get
            {
                //foreach (SunNode node in Nodes)
                //    if (node.Name.ToLower() == name.ToLower())
                //        return null;
                //// wtf. flesh this out later
                //switch (type)
                //{
                //    case SunObjectType.Directory:
                //        return AddObject(new SunDirectory(name, (SunDirectory)Tag));

                //    case SunObjectType.File:
                //        break;

                //    case SunObjectType.Property:
                //        break;

                //    default: break;
                //}
                return null;
            }
        }
    }
}