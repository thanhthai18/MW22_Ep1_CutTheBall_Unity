using System;
using CsvReader;
using Runtime.Definition;

namespace Runtime.Config
{
    [Serializable]
    public class HitObjectItem
    {
        #region Members

        // public HitObjectType objectType;
        // public int valueAddScore;
        // public ResourceData requiredResourceData;
        //[CsvColumnFormat(ColumnFormat = "return_{0}")]

        #endregion Members
    }

    public class HitObjectConfig : BaseConfig<HitObjectItem> { }
}