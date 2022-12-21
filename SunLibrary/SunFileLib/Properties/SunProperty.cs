using System;
using System.Collections.Generic;
using SunLibrary.SunFileLib.Structure;
using SunLibrary.SunFileLib.Util;

namespace SunLibrary.SunFileLib.Properties
{
    /// <summary>
    /// Base class for defining the properties within the SunFiles.
    /// </summary>
    public abstract class SunProperty : SunObject
    {
        #region Members

        public virtual new SunProperty this[string name]
        { get { return null; } set { } }

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

        public abstract SunProperty DeepClone();

        #endregion Members

        #region Inherited Members

        /// <summary>
        /// Returns the byte-value type of the SunProperty.
        /// </summary>
        public override SunObjectType ObjectType
        { get { return SunObjectType.Property; } }

        /// <summary>
        /// Returns the SunFile this property is a part of.
        /// </summary>
        public override SunFile SunFileParent
        { get { return ParentDirectory.SunFileParent; } }

        /// <summary>
        /// Removes the property from its parent in the file structure.
        /// </summary>
        public override void Remove()
        {
            ((IPropertyContainer)Parent).RemoveProperty(this);
        }

        #endregion Inherited Members

        #region Custom Members

        public virtual List<SunProperty> SunProperties
        { get { return null; } }

        public static List<SunProperty> ParsePropertyList(SunBinaryReader reader, SunObject parent, SunImage parentImg)
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
                        properties.Add(new SunLongProperty(name, reader.ReadLong()) { Parent = parent });
                        break;

                    case 5:
                        properties.Add(new SunFloatProperty(name, reader.ReadSingle()) { Parent = parent });
                        break;

                    case 6:
                        properties.Add(new SunDoubleProperty(name, reader.ReadDouble()) { Parent = parent });
                        break;

                    case 7:
                        properties.Add(new SunStringProperty(name, reader.ReadString()) { Parent = parent });
                        break;

                    case 12:    // Extended Properties (Canvas/Vector/Sound/Sub)
                        int endOfBlock = (int)(reader.ReadUInt32() + reader.BaseStream.Position);
                        SunProperty extendedProperty = ParseExtendedProperty(reader, name, parent);
                        properties.Add(extendedProperty);
                        if (reader.BaseStream.Position != endOfBlock)
                            reader.BaseStream.Position = endOfBlock;
                        break;

                    default:
                        throw new Exception("Unknown property type at ParsePropertyList, Type = " + propertyType);
                }
            }
            return properties;
        }

        public static SunPropertyExtended ParseExtendedProperty(SunBinaryReader reader, string name, SunObject parent)
        {
            // Here read the different extended property bytes
            switch (reader.ReadByte())
            {
                case 8:
                    SunCanvasProperty canvasProperty = new SunCanvasProperty(name) { Parent = parent };
                    if (reader.ReadByte() == 1)
                    {
                        // There are properties
                        reader.BaseStream.Position++;   //To jump over 04 (bc canvas is a propertycontainer)
                        canvasProperty.AddProperties(ParsePropertyList(reader, canvasProperty, canvasProperty.ParentImage));
                    }
                    canvasProperty.PNG = new SunPngProperty(reader, false);
                    return canvasProperty;

                case 9:
                    SunVectorProperty vectorProperty = new SunVectorProperty(name) { Parent = parent };
                    vectorProperty.X = new SunIntProperty("X", reader.ReadCompressedInt()) { Parent = vectorProperty };
                    vectorProperty.Y = new SunIntProperty("Y", reader.ReadCompressedInt()) { Parent = vectorProperty };
                    return vectorProperty;

                case 10:
                    SunSoundProperty soundProperty = new SunSoundProperty(name, reader, false) { Parent = parent };
                    return soundProperty;

                case 11:
                    SunSubProperty subProp = new SunSubProperty(name) { Parent = parent };
                    reader.BaseStream.Position++;   //To jump over 04 (bc sub is a propertycontainer)
                    subProp.AddProperties(ParsePropertyList(reader, subProp, subProp.ParentImage));
                    return subProp;

                case 14:
                    SunConvexProperty convexProperty = new SunConvexProperty(name) { Parent = parent};
                    reader.BaseStream.Position++; //To jump over 04 (bc convex is also a IPropertyContainer)
                    convexProperty.AddProperties(ParsePropertyList(reader, convexProperty, convexProperty.ParentImage));
                    return convexProperty;

                default:
                    throw new Exception("Error occured parsing extended property");
            }
        }

        public static void WritePropertyList(SunBinaryWriter writer, List<SunProperty> properties)
        {
            writer.Write((byte)SunObjectType.Property);
            writer.WriteCompressedInt(properties.Count);
            for (int i = 0; i < properties.Count; i++)
            {
                writer.Write(properties[i].Name);
                if (properties[i] is SunPropertyExtended extendedProp)
                {
                    WriteExtendedPropertyValue(writer, extendedProp);
                }
                else
                {
                    properties[i].WriteValue(writer);
                }
            }
        }

        internal static void WriteExtendedPropertyValue(SunBinaryWriter writer, SunPropertyExtended extProp)
        {
            writer.Write((byte)SunPropertyType.Extended);
            long beforePos = writer.BaseStream.Position;
            writer.Write((Int32)0); // Placeholder
            extProp.WriteValue(writer);

            int len = (int)(writer.BaseStream.Position - beforePos);
            long newPos = writer.BaseStream.Position;
            writer.BaseStream.Position = beforePos;
            writer.Write(len - 4);
            writer.BaseStream.Position = newPos;
        }

        public SunImage ParentImage
        {
            get
            {
                SunObject parent = Parent;
                while (parent != null)
                {
                    if (parent is SunImage image) return image;
                    else parent = parent.Parent;
                }
                return null;
            }
        }

        #endregion Custom Members
    }
}