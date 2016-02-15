using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMES.DataModel
{
    [Serializable]
    public class AssetRangeDef
    {
       public int id;
       public string code;
       public string begin;
       public string end;
       public string remark;
       public string editor;
       public string cdt;
       public string udt;
       public string status;
    }
}
