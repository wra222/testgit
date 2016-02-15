// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 用于所有检料站，传递给前台的参数
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2009-12-18   Yuan XiaoWei                 create
// Known issues:
using System;
using System.Text;

namespace IMES.DataModel
{
    /// <summary>
    /// 匹配到的料
    /// </summary>
    public class MatchedPart
    {
        /// <summary>
        /// 刷入料的实际PN,如果是Item,为ItemName
        /// </summary>
        public string pn;

        /// <summary>
        /// 实际刷入的序号
        /// </summary>
        public string sn;

       /// <summary>
        /// 刷入料如果是Part才有,如果是CheckItem没有
       /// </summary>
        public string partDescr;

        /// <summary>
        /// 刷入料如果是Part是PartType,如果是CheckItem是ItemName
        /// </summary>
        public string partType;
    }
}
