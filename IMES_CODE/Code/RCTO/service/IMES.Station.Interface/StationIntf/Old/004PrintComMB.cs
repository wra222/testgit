// INVENTEC corporation (c)2009 all rights reserved. 
// Description: OffLine Print ComMB interface
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
using System.Drawing;
using IMES.Station.Interface.CommonIntf;
using IMES.DataModel;

namespace IMES.Station.Interface.StationIntf
{
    /// <summary>
    /// 为每个小板子产生CHILD MB SNO，并列印Label
    /// </summary>
    public interface IPrintComMB
    {
        /// <summary>
        /// 1.1	UC-PCA-PCM-01 Print ComMB
        /// 为每个小板子产生CHILD MB SNO，并列印Label
        /// 异常情况：
        /// a.	如果没有输入[Q’ty]，则报告错误“Please Entry Q’ty first!!”
        /// b.	如果没有选择[Batch File/Template]，则报告错误“请选择Batch File!!”
        /// </summary>
        /// <param name="PCBNo">PCBNo</param>
        /// <param name="multiQty">连板数量</param>
        /// <param name="editor">operator</param>
        /// <param name="line"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <param name="printItems"></param>
        /// <returns>Print Items</returns>
        IList<PrintItem> Print(
            string PCBNo,
            int multiQty, string editor,
            string line, string station, string customer, IList<PrintItem> printItems);
    }
}
