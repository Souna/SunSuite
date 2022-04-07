﻿using SunFileManager.SunFileLib.Properties;
using System.Collections.Generic;

namespace SunFileManager.SunFileLib
{
    /// <summary>
    /// An interface providing functionality for nodes to contain child properties.
    /// </summary>
    public interface IPropertyContainer
    {
        void AddProperty(SunProperty prop);

        void AddProperties(List<SunProperty> props);

        void RemoveProperty(SunProperty prop);

        void ClearProperties();

        List<SunProperty> SunProperties { get; }

        SunProperty this[string name] { get; set; }
    }
}