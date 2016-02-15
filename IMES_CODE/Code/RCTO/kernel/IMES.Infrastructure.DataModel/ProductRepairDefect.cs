// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 用于Repair站，传递给前台的参数
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2009-11-11   Yuan XiaoWei                 create
// Known issues:
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMES.DataModel
{
    [Serializable]
    public class ProductRepairDefect
    {
       public int ID;
       public int ProductRepairID;
       public string Type;
       public string DefectCode;
       public string Cause;
       public string Obligation;
       public string Compoment;
       public string Site;
       public string MajorPart;
       public string Remark;
       //未完待续
    }
}
