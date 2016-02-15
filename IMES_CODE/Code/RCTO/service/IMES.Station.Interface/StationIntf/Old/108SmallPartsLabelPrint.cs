// INVENTEC corporation (c)201all rights reserved. 
// Description: OffLine Small Parts Label Print interface
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2010-05-06   Chen Xu (eB1-4)              create
// Known issues:

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using IMES.Station.Interface.CommonIntf;
using IMES.DataModel;


namespace IMES.Station.Interface.StationIntf
{
    /// <summary>
    /// 列印Small Parts Label
    /// </summary>
    public interface ISmallPartsLabelPrint
    {
        /// <summary>
        /// 列印Small Parts Label
        /// 如果用户输入的[IECPN] 在数据库中不存在，则报告错误：“无法找到TSB PN!”
        /// </summary>
        /// <param name="iecPn">iecPn</param>
        /// <param name="qty">qty</param>
        /// <param name="editor">editor</param>
        /// <param name="line">line</param>
        /// <param name="station">station</param>
        /// <param name="customer">customer</param>
        /// <param name="printItems">printItems</param>
        /// <returns>Print Items</returns>  
        IList<PrintItem> Print(string iecPn,int qty, string editor, string line, string station, string customer, IList<PrintItem> printItems);

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="iecPn"></param>
        void Cancel(string iecPn);
    }
}
