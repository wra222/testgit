
// INVENTEC corporation (c)2010 all rights reserved. 
// Description: 用于 TPCB Collection 站，传递给前台的参数
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2010-04-14   Chen Xu (eb1-4)              create
// Known issues:

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMES.DataModel
{
    /// <summary>
    /// TPCBDet信息
    /// </summary>
    [Serializable]
    public class TPCBDet
    {
        /// <summary>
        /// TPCB
        /// </summary>
        public String Code;
        
        /// <summary>
        /// Type
        /// </summary>
        public String Type;
        
        /// <summary>
        /// PartNo
        /// </summary>
        public String PartNo;

        /// <summary>
        /// Vendor
        /// </summary>
        public String Vendor;

        /// <summary>
        /// DCode
        /// </summary>
        public String DCode;

        /// <summary>
        /// 操作员标识
        /// </summary>
        public String Editor;

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime Cdt;
    }
}
