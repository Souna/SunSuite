using SunFileManager.SunFileLib.Properties;
using SunFileManager.SunFileLib.Structure;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace SunFileManager.SunFileLib
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

            ParseChilds(sourceObject);
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

            // another check for image and anything else that applies here ?
        }

        private void TryParseImage(bool reparseImage = true)
        {
            if (Tag is SunImage)
            {
                ((SunImage)Tag).ParseImage();
                if (reparseImage)
                {
                    Reparse();
                }
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
            //  Original HaRepacker tries to parse the wzimage before adding object.
            if (CanNodeBeInserted(this, newObject.Name))
            {
                TryParseImage();
                if (AddObjectInternal(newObject))
                {
                    SunNode node = new SunNode(newObject, true);
                    Nodes.Add(node);
                    if (node.Tag is SunProperty)
                    {
                        ((SunProperty)node.Tag).ParentImage.Changed = true;
                    }
                    node.EnsureVisible();
                    //if (expandNode) node.TreeView.SelectedNode = node;
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
        /// Adds a <b>SunObject</b> as a child of the selected node.
        /// </summary>
        private bool AddObjectInternal(SunObject newObject)
        {
            SunObject selectedObject = (SunObject)Tag;  // Selected node.

            // If selectedObject is a SunFile, selectedObject becomes the master SunDirectory of that SunFile.
            if (selectedObject is SunFile file) selectedObject = file.SunDirectory;
            if (selectedObject is SunDirectory directory)
            {
                if (newObject is SunDirectory)
                {
                    directory.AddDirectory((SunDirectory)newObject);
                }
                else if (newObject is SunImage)
                {
                    directory.AddImage((SunImage)newObject);
                }
                else
                {
                    return false;
                }
            }
            else if (selectedObject is SunImage image)
            {
                if (!image.Parsed) image.ParseImage();
                if (newObject is SunProperty)
                {
                    image.AddProperty((SunProperty)newObject);
                    image.Changed = true;
                }
                else
                {
                    return false;
                }
            }
            else if (selectedObject is IPropertyContainer propertyContainer)
            {
                if (newObject is SunProperty)
                {
                    propertyContainer.AddProperty((SunProperty)newObject);
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

            //  We're adding something to an existing image
            //    else if (selectedObject is SunCanvasProperty image)
            //    {
            //        //  We're adding a gif to its own gif parent node (containing all the frames)
            //        if (image.IsGif)
            //        {
            //            if (newObject is SunCanvasProperty frame)
            //                //image.AddProperty(frame);
            //                image.AddFrame(frame);
            //            else return false;  // error
            //        }
            //        //  We're adding a property to the image
            //        else if (newObject is SunProperty prop)
            //        {
            //            // Cannot create a property with a duplicate name under the same image.
            //            foreach (SunProperty p in image.SunProperties)
            //            {
            //                if (p.Name == prop.Name) return false;
            //            }
            //            image.AddProperty(prop);
            //        }
            //        else return false;
            //    }
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
                //switch (((SunObject)Tag).ObjectType)
                //{
                //    case SunObjectType.File:
                //        return SunObjectType.File.ToString();

                //    case SunObjectType.Directory:
                //        return SunObjectType.Directory.ToString();

                //    case SunObjectType.Property:
                //        return ((SunProperty)Tag).PropertyType.ToString() + " " + SunObjectType.Property.ToString();

                //    default:    // Selected anything else.
                //        return Tag.GetType().Name;
                //}
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
        public void DeleteNode()
        {
            ((SunObject)Tag).Remove();
            Remove();   // Delete from Tree.
        }

        /// <summary>
        /// Changes the name of a selected node.
        /// </summary>
        public void Rename(string newName)
        {
            Text = newName; // The displayed name.
            if (Tag is SunFile) Text += ".sun";
            ((SunObject)Tag).Name = newName;    // Change internal name.
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
                foreach (SunNode node in Nodes)
                    if (node.Name.ToLower() == name.ToLower())
                        return null;
                // wtf. flesh this out later
                switch (type)
                {
                    case SunObjectType.Directory:
                        return AddObject(new SunDirectory(name, (SunDirectory)Tag));

                    case SunObjectType.File:
                        break;

                    case SunObjectType.Property:
                        break;

                    default: break;
                }
                return null;
            }
        }
    }
}