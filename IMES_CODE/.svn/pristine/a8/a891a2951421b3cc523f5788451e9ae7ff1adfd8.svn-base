// INVENTEC corporation (c)2009 all rights reserved. 
// Description: FinalPVS站使用的BLL接口
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2009-10-23   Yuan XiaoWei                 create
// Known issues:
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;

namespace IMES.Station.Interface.StationIntf
{
    /// <summary>
    /// 重印Pizza标签的接口
    /// </summary>
    public interface IPVSReprint
    {

        /// <summary>
        /// 重印Pizza标签
        /// </summary>
        /// <param name="custSN"></param>
        /// <param name="reason"></param>
        /// <param name="printItems"></param>
        /// <param name="editor"></param>
        /// <param name="line"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        IList<PrintItem> ReprintLabel(ref string custSN, string reason, IList<PrintItem> printItems, string editor, string line, string station, string customer);

    }
}
