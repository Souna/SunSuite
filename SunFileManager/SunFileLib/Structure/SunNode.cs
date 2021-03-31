using SunFileManager.SunFileLib.Properties;
using System;
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
        public SunNode(SunObject sourceObject) : base(sourceObject.Name)
        {
            Name = sourceObject.Name;
            ParseChilds(sourceObject);
        }

        public void ParseChilds(SunObject sourceObject)
        {
            if (sourceObject == null)
                throw new NullReferenceException("Can't create null SunNode.");

            Tag = sourceObject;
            //  WHEN LOADING, SourceObject becomes null because the SunFile doesn't have a sundirectory yet.
            if (sourceObject is SunFile file)
                sourceObject = file.SunDirectory;

            if (sourceObject is SunDirectory directory)
            {
                foreach (SunDirectory dir in directory.subDirectoryList)
                {
                    Nodes.Add(new SunNode(dir));
                }

                foreach (SunProperty property in directory.sunPropertyList)
                {
                    Nodes.Add(new SunNode(property));
                }
            }

            // another check for image and anything else that applies here ?
        }

        /// <summary>
        /// Creates a new node on the tree with the <b>SunObject</b> type.
        /// </summary>
        public SunNode AddObject(SunObject newObject, bool expandNode = true)
        {
            //  Original HaRepacker tries to parse the wzimage before adding object.
            if (AddObjectInternal(newObject))
            {
                SunNode node = new SunNode(newObject);
                Nodes.Add(node);
                if (expandNode)
                    node.TreeView.SelectedNode = node;
                return node;
            }
            else return null;
        }

        /// <summary>
        /// Adds a <b>SunObject</b> as a child of the selected node.
        /// </summary>
        private bool AddObjectInternal(SunObject newObject)
        {
            // Work more on this. There's a lot more to add
            SunObject selectedObject = (SunObject)Tag;  // Selected node.

            // If selectedObject is a SunFile, selectedObject becomes the master SunDirectory of that SunFile.
            if (selectedObject is SunFile file) selectedObject = file.SunDirectory;

            if (selectedObject is IPropertyContainer)
            {
                //  We're adding something to an existing directory
                if (selectedObject is SunDirectory directory)
                {
                    //  We're adding a subdirectory
                    if (newObject is SunDirectory dir)
                    {
                        // Cannot create a directory with a duplicate name under the same directory.
                        foreach (SunDirectory d in directory.SubDirectories)
                        {
                            if (d.Name == newObject.Name) return false;
                        }
                        directory.AddDirectory(dir);
                    }
                    //  We're adding a property to the directory
                    else if (newObject is SunProperty prop)
                    {
                        // Cannot create a property with a duplicate name under the same directory.
                        foreach (SunProperty p in directory.SunProperties)
                        {
                            if (p.Name == prop.Name) return false;
                        }
                        directory.AddProperty(prop);
                    }
                    else return false;
                }
                //  We're adding something to an existing image
                else if (selectedObject is SunImageProperty image)
                {
                    //  We're adding a gif to its own gif parent node (containing all the frames)
                    if (image.IsGif)
                    {
                        if (newObject is SunImageProperty frame)
                            //image.AddProperty(frame);
                            image.AddFrame(frame);
                        else return false;  // error
                    }
                    //  We're adding a property to the image
                    else if (newObject is SunProperty prop)
                    {
                        // Cannot create a property with a duplicate name under the same image.
                        foreach (SunProperty p in image.SunProperties)
                        {
                            if (p.Name == prop.Name) return false;
                        }
                        image.AddProperty(prop);
                    }
                    else return false;
                }
                return true;
            }
            return false;
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
                //  Selected an image node.
                else if (Parent != null && Tag is SunImageProperty img)
                {
                    // Selected a gif node.
                    if (img.IsGif) return "Animated " + SunPropertyType.Image.ToString();
                    else return SunPropertyType.Image.ToString();
                }
                switch (((SunObject)Tag).ObjectType)
                {
                    case SunObjectType.File:
                        return SunObjectType.File.ToString();

                    case SunObjectType.Directory:
                        return SunObjectType.Directory.ToString();

                    case SunObjectType.Property:
                        return ((SunProperty)Tag).PropertyType.ToString() + " " + SunObjectType.Property.ToString();

                    default:    // Selected anything else.
                        return Tag.GetType().Name;
                }
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