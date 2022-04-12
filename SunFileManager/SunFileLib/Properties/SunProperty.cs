using SunFileManager.SunFileLib.Structure;
using SunFileManager.SunFileLib.Util;
using System;
using System.Collections.Generic;

namespace SunFileManager.SunFileLib.Properties
{
    /// <summary>
    /// Base class for defining the properties associated with different Nodes.
    /// </summary>
    public abstract class SunProperty : SunObject
    {
        #region Members

        public virtual new SunProperty this[string name] { get { return null; } set { } }

        public abstract SunPropertyType PropertyType { get; }

        /// <summary>
        /// The SunDirectory this Property is contained within.
        /// </summary>
        public SunDirectory ParentDirectory
        {
            get
            {
                SunObject parent = Parent;
                while (parent != null)
                {
                    if (parent is SunDirectory directory) return directory;
                    else parent = parent.Parent;
                }
                return null;
            }
        }

        //                            Object so we can use different data types later.
        public abstract void SetValue(object value);

        public abstract void WriteValue(SunBinaryWriter writer);

        #endregion Members

        #region Inherited Members

        /// <summary>
        /// Returns the byte-value type of the SunProperty.
        /// </summary>
        public override SunObjectType ObjectType { get { return SunObjectType.Property; } }

        /// <summary>
        /// Returns the SunFile this property is a part of.
        /// </summary>
        public override SunFile SunFileParent { get { return ParentDirectory.SunFileParent; } }

        /// <summary>
        /// Removes the property from its parent in the file structure.
        /// </summary>
        public override void Remove()
        {
            ((IPropertyContainer)Parent).RemoveProperty(this);
        }

        #endregion Inherited Members

        #region Custom Members

        public static List<SunProperty> ParsePropertyList(uint offset, SunBinaryReader reader, SunObject parent, SunImage parentImg)
        {
            int entryCount = reader.ReadCompressedInt();
            List<SunProperty> properties = new List<SunProperty>(entryCount);
            for (int i = 0; i < entryCount; i++)
            {
                string name = reader.ReadString();
                byte propertyType = reader.ReadByte();
                switch (propertyType)
                {
                    case 0:
                        properties.Add(new SunNullProperty(name) { Parent = parent });
                        break;
                    case 2:
                        properties.Add(new SunShortProperty(name, reader.ReadInt16()) { Parent = parent });
                        break;
                    case 3:
                        properties.Add(new SunIntProperty(name, reader.ReadCompressedInt()) { Parent = parent });
                        break;
                    case 4:
                        properties.Add(new SunLongProperty(name, reader.ReadLong()) { Parent = parent });   // Max value is int only?
                        break;
                    case 5:
                        properties.Add(new SunFloatProperty(name, reader.ReadSingle()) { Parent = parent });
                        break;
                    case 6:
                        properties.Add(new SunDoubleProperty(name, reader.ReadDouble()) { Parent = parent });
                        break;
                    case 7:
                        properties.Add(new SunStringProperty(name, reader.ReadString()){ Parent = parent });
                        //properties.Add(new SunStringProperty(name, reader.ReadStringBlock(offset)) { Parent = parent });
                        break;
                    case 8:
                        reader.BaseStream.Position += 5;    // To skip to Gif_Bool  
                        properties.Add(new SunCanvasProperty(name, parent, reader.ReadBoolean()));
                        break;
                    default:
                        throw new Exception("Unknown property type at ParsePropertyList, Type = " + propertyType);
                }
            }
            return properties;
        }

        public static void WritePropertyList(SunBinaryWriter writer, List<SunProperty> properties)
        {
            //if (properties.Count == 0) return;
            writer.Write((byte)SunObjectType.Property);
            writer.WriteCompressedInt(properties.Count);
            for(int i = 0; i < properties.Count; i++)
            {
                properties[i].WriteValue(writer);
            }
        }

        public SunImage ParentImage
        {
            get
            {
                SunObject parent = Parent;
                while (parent != null)
                {
                    if (parent is SunImage) return (SunImage)parent;
                    else parent = parent.Parent;
                }
                return null;
            }
        }
        #endregion Custom Members
    }
}