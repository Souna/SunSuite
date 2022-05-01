﻿namespace SunFileManager.SunFileLib
{
    public enum SunPropertyType
    {
        #region Regular

        Null,
        Short = 2,
        Int = 3,
        Long = 4,
        Float = 5,
        Double = 6,
        String = 7,

        #endregion Regular

        #region Extended

        // Properties that can contain other properties
        Canvas = 8,

        Vector = 9,
        Sound = 10,
        SubProperty = 11,
        Extended = 12,

        #endregion Extended
    }
}