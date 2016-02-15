using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using IMES.DataModel;

namespace IMES.Docking.Interface.DockingIntf
{
    /// <summary>
    /// 
    /// </summary>
    public interface RegenerateCustSNForDocking
    {
        /// <summary>
        /// 输入Product Id相关信息并处理
        /// </summary>
        /// <param name="pdLine">Pd Line</param>
        /// <param name="prodId">Product Id</param>
        /// <param name="editor">operator</param>
        /// <returns>prestation</returns>
        ArrayList InputProdId(
            string pdLine,
            string prodId,
            string editor, string stationId, string customer,
            IList<PrintItem> printItems);

       
    }
}
