/*  MapleLib - A general-purpose MapleStory library
 * Copyright (C) 2009, 2010, 2015 Snow and haha01haha01

 * This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

 * This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

 * You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.*/

using SunLibrary.SunFileLib.Properties;

//using static System.Net.WebRequestMethods;
using SunLibrary.SunFileLib.Structure;
using System;
using System.IO;
using System.Reflection;

namespace MapleLib.WzLib
{
    public class WzSettingsManager
    {
        private string sfPath;
        private Type userSettingsType;
        private Type appSettingsType;
        private Type xnaColorType = null;

        public WzSettingsManager(string sfPath, Type userSettingsType, Type appSettingsType)
        {
            this.sfPath = sfPath;
            this.userSettingsType = userSettingsType;
            this.appSettingsType = appSettingsType;
        }

        public WzSettingsManager(string sfPath, Type userSettingsType, Type appSettingsType, Type xnaColorType)
            : this(sfPath, userSettingsType, appSettingsType)
        {
            this.xnaColorType = xnaColorType;
        }

        private void ExtractSettingsImage(SunImage settingsImage, Type settingsHolderType)
        {
            if (!settingsImage.Parsed) settingsImage.ParseImage();
            foreach (FieldInfo fieldInfo in settingsHolderType.GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                string settingName = fieldInfo.Name;
                SunProperty settingProp = settingsImage[settingName];
                byte[] argb;
                if (settingProp == null)
                    SaveField(settingsImage, fieldInfo);
                else if (fieldInfo.FieldType.BaseType != null && fieldInfo.FieldType.BaseType.FullName == "System.Enum")
                    fieldInfo.SetValue(null, InfoTool.GetInt(settingProp));
                else switch (fieldInfo.FieldType.FullName)
                    {
                        //case "Microsoft.Xna.Framework.Graphics.Color":
                        case "Microsoft.Xna.Framework.Color":
                            if (xnaColorType == null) throw new InvalidDataException("XNA color detected, but XNA type activator is null");
                            argb = BitConverter.GetBytes((uint)((SunDoubleProperty)settingProp).Value);
                            fieldInfo.SetValue(null, Activator.CreateInstance(xnaColorType, argb[0], argb[1], argb[2], argb[3]));
                            break;

                        case "System.Drawing.Color":
                            argb = BitConverter.GetBytes((uint)((SunDoubleProperty)settingProp).Value);
                            fieldInfo.SetValue(null, System.Drawing.Color.FromArgb(argb[3], argb[2], argb[1], argb[0]));
                            break;

                        case "System.Int32":
                            fieldInfo.SetValue(null, InfoTool.GetInt(settingProp));
                            break;

                        case "System.Double":
                            fieldInfo.SetValue(null, ((SunDoubleProperty)settingProp).Value);
                            break;

                        case "System.Boolean":
                            fieldInfo.SetValue(null, InfoTool.GetBool(settingProp));
                            //bool a = (bool)fieldInfo.GetValue(null);
                            break;

                        case "System.Single":
                            fieldInfo.SetValue(null, ((SunFloatProperty)settingProp).Value);
                            break;
                        /*case "WzMapleVersion":
                            fieldInfo.SetValue(null, (WzMapleVersion)InfoTool.GetInt(settingProp));
                            break;

                        case "ItemTypes":
                            fieldInfo.SetValue(null, (ItemTypes)InfoTool.GetInt(settingProp));
                            break;*/
                        case "System.Drawing.Size":
                            fieldInfo.SetValue(null, new System.Drawing.Size(((SunVectorProperty)settingProp).X.Value, ((SunVectorProperty)settingProp).Y.Value));
                            break;

                        case "System.String":
                            fieldInfo.SetValue(null, InfoTool.GetString(settingProp));
                            break;

                        case "System.Drawing.Bitmap":
                            fieldInfo.SetValue(null, ((SunCanvasProperty)settingProp).PNG.GetPNG(false));
                            break;

                        default:
                            throw new Exception("unrecognized setting type");
                    }
            }
        }

        private void CreateSunProp(IPropertyContainer parent, SunPropertyType propType, string propName, object value)
        {
            SunProperty addedProp;
            switch (propType)
            {
                case SunPropertyType.Float:
                    addedProp = new SunFloatProperty(propName);
                    break;

                case SunPropertyType.Canvas:
                    addedProp = new SunCanvasProperty(propName);
                    ((SunCanvasProperty)addedProp).PNG = new SunPngProperty();
                    break;

                case SunPropertyType.Int:
                    addedProp = new SunIntProperty(propName);
                    break;

                case SunPropertyType.Double:
                    addedProp = new SunDoubleProperty(propName);
                    break;
                /*case WzPropertyType.Sound:
                    addedProp = new WzSoundProperty(propName);
                    break;*/
                case SunPropertyType.String:
                    addedProp = new SunStringProperty(propName);
                    break;

                case SunPropertyType.Short:
                    addedProp = new SunShortProperty(propName);
                    break;

                case SunPropertyType.Vector:
                    addedProp = new SunVectorProperty(propName);
                    if (addedProp.Parent == null) addedProp.Parent = (SunImage)parent;
                    ((SunVectorProperty)addedProp).X = new SunIntProperty("X") { Parent = (SunImage)parent };
                    ((SunVectorProperty)addedProp).Y = new SunIntProperty("Y") { Parent = (SunImage)parent };
                    break;
                /*case WzPropertyType.Lua: // probably dont allow the user to create custom Lua for now..
                    {
                        addedProp = new WzLuaProperty(propName, new byte[] { });
                        break;
                    }*/
                default:
                    throw new NotSupportedException("Not supported type");
            }
            if (addedProp.Parent == null)
            {
                if (parent is SunImage)
                {
                    addedProp.Parent = (SunImage)parent;
                }
            }
            addedProp.SetValue(value);
            parent.AddProperty(addedProp);
        }

        private void SetSunProperty(SunImage parentImage, string propName, SunPropertyType propType, object value)
        {
            SunProperty property = parentImage[propName];
            if (property != null)
            {
                if (property.PropertyType == propType)
                    property.SetValue(value);
                else
                {
                    property.Remove();
                    CreateSunProp(parentImage, propType, propName, value);
                }
            }
            else
                CreateSunProp(parentImage, propType, propName, value);
        }

        private void SaveSettingsImage(SunImage settingsImage, Type settingsHolderType)
        {
            if (!settingsImage.Parsed) settingsImage.ParseImage();
            foreach (FieldInfo fieldInfo in settingsHolderType.GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                SaveField(settingsImage, fieldInfo);
            }
            settingsImage.Changed = true;
        }

        private void SaveField(SunImage settingsImage, FieldInfo fieldInfo)
        {
            string settingName = fieldInfo.Name;
            if (fieldInfo.FieldType.BaseType != null && fieldInfo.FieldType.BaseType.FullName == "System.Enum")
                SetSunProperty(settingsImage, settingName, SunPropertyType.Int, fieldInfo.GetValue(null));
            else switch (fieldInfo.FieldType.FullName)
                {
                    //case "Microsoft.Xna.Framework.Graphics.Color":
                    case "Microsoft.Xna.Framework.Color":
                        object xnaColor = fieldInfo.GetValue(null);
                        //for some odd reason .NET requires casting the result to uint before it can be
                        //casted to double
                        SetSunProperty(settingsImage, settingName, SunPropertyType.Double, (double)(uint)xnaColor.GetType().GetProperty("PackedValue").GetValue(xnaColor, null));
                        break;

                    case "System.Drawing.Color":
                        SetSunProperty(settingsImage, settingName, SunPropertyType.Double, (double)((System.Drawing.Color)fieldInfo.GetValue(null)).ToArgb());
                        break;

                    case "System.Int32":
                        SetSunProperty(settingsImage, settingName, SunPropertyType.Int, fieldInfo.GetValue(null));
                        break;

                    case "System.Double":
                        SetSunProperty(settingsImage, settingName, SunPropertyType.Double, fieldInfo.GetValue(null));
                        break;

                    case "Single":
                        SetSunProperty(settingsImage, settingName, SunPropertyType.Float, fieldInfo.GetValue(null));
                        break;

                    case "System.Drawing.Size":
                        SetSunProperty(settingsImage, settingName, SunPropertyType.Vector, fieldInfo.GetValue(null));
                        break;

                    case "System.String":
                        SetSunProperty(settingsImage, settingName, SunPropertyType.String, fieldInfo.GetValue(null));
                        break;

                    case "System.Drawing.Bitmap":
                        SetSunProperty(settingsImage, settingName, SunPropertyType.Canvas, fieldInfo.GetValue(null));
                        break;

                    case "System.Boolean":
                        SetSunProperty(settingsImage, settingName, SunPropertyType.Int, (bool)fieldInfo.GetValue(null) ? 1 : 0);
                        break;
                }
        }

        /// <summary>
        /// to-do
        /// </summary>
        public void Load()
        {
            if (File.Exists(sfPath))
            {
                File.Delete(sfPath); return;
                SunFile SunFile = new SunFile(sfPath);
                try
                {
                    string parseErrorMessage = string.Empty;
                    bool success = SunFile.ParseSunFile(out parseErrorMessage, true);

                    ExtractSettingsImage((SunImage)SunFile["UserSettings.img"], userSettingsType);
                    ExtractSettingsImage((SunImage)SunFile["ApplicationSettings.img"], appSettingsType);
                    SunFile.Dispose();
                }
                catch
                {
                    SunFile.Dispose();
                    throw;
                }
            }
        }

        public void Save()
        {
            bool settingsExist = File.Exists(sfPath);
            SunFile SunFile;
            if (settingsExist)
            {
                SunFile = new SunFile(sfPath);

                string parseErrorMessage = string.Empty;
                bool success = SunFile.ParseSunFile(out parseErrorMessage, true);
            }
            else
            {
                SunFile = new SunFile();
                SunFile.Header.Copyright = "Based on settings file generated by MapleLib's WzSettings module created by haha01haha01";
                SunFile.Header.RecalculateFileStart();  //make sure this is good
                SunImage UserSettings = new SunImage("UserSettings.img") { Changed = true, Parsed = true };
                SunImage AppSettings = new SunImage("ApplicationSettings.img") { Changed = true, Parsed = true };
                SunFile.SunDirectory.SunImages.Add(UserSettings);
                SunFile.SunDirectory.SunImages.Add(AppSettings);
            }
            SaveSettingsImage((SunImage)SunFile["UserSettings.img"], userSettingsType);
            SaveSettingsImage((SunImage)SunFile["ApplicationSettings.img"], appSettingsType);
            if (settingsExist)
            {
                string tempFile = Path.GetTempFileName();
                string settingsPath = SunFile.FilePath;
                SunFile.SaveToDisk(tempFile);
                SunFile.Dispose();
                File.Delete(settingsPath);
                File.Move(tempFile, settingsPath);
            }
            else
                SunFile.SaveToDisk(sfPath);
        }
    }
}