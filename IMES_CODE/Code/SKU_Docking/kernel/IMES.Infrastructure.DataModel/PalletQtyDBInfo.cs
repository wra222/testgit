using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMES.DataModel
{
    [Serializable]
    public struct PalletQtyDBInfo
    {
        public int id;
        public int fullQty;
        public int tireQty;
        public int mediumQty;
        public int litterQty;
        public String editor;
        public DateTime cdt;
        public DateTime udt;
        //更新主键时使用
        public int oldFullQty;
    }
}
