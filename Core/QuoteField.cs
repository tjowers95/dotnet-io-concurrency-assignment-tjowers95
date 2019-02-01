using System;
using System.ComponentModel;

namespace Core
{
    [Serializable]
    public enum QuoteField
    {
        [Description("open")]
        Open,
        [Description("close")]
        Close,
        [Description("high")]
        High,
        [Description("low")]
        Low,
        [Description("latestPrice")]
        LatestPrice,
        [Description("change")]
        Change,
        [Description("changePercent")]
        ChangePercent
    }
}