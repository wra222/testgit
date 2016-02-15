using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMES.DataModel
{
    /// <summary>
    ///    All    全部
    ///    Empty 空庫位
    ///    Partial 未滿庫位
    ///    Full 滿庫位
    ///    Occupy 使用中的庫位
    ///    Hold 禁用庫位
    ///    HoldIn 禁用入庫庫位
    ///    HoldOut 禁用出庫庫位
    ///    Model By機型查詢
    /// </summary>
    public enum BSamLocationQueryType
    {
        All = 0,
        Empty,
        Partial,
        Full,
        Occupy,
        Hold,
        HoldIn,
        HoldOut,
        Model
    }
}
