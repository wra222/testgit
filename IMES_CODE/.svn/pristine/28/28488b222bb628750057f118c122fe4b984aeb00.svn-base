// created by  ies106137

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using IMES.Station.Interface.CommonIntf;
using IMES.DataModel;

namespace IMES.Station.Interface.StationIntf
{
    /// <summary>
    /// [Change MB Label] 实现如下两个功能：
    /// 1.	根据old MB，产生new MB SNo
    /// /// 这样做的目的有1个：
    /// 1.	更换MB number
    /// </summary>
    public interface IChangeMB
    {
        /// <summary>
        /// 1.1	UC-PCA-MLP-01 Print
        /// 依据旧MB
        /// 1.根据旧的MB number,产生新的MB number
        /// 2.	列印MB Label
        /// </summary>
        /// <param oldMB="oldMB">原MB号</param>
        /// <param reason="reason">重新列印的原因</param>
        /// <returns>Print Items</returns>
        IList<PrintItem> ReplaceMB(string oldMB, 
                                                       string reason, 
                                                        string editor, 
                                                        string stationId, 
                                                        string customerId,
                                                        out IList<IMES.DataModel.MBInfo> startProdIdAndEndProdId, 
                                                        IList<PrintItem> printItems);
       // List<string> GetMBInfo(string MB);
 
    }

}