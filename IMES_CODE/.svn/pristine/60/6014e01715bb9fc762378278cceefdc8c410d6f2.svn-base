// created by itc205033

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using IMES.Station.Interface.CommonIntf;
using IMES.DataModel;

namespace IMES.Station.Interface.StationIntf
{
    public interface IPrint1397Label
    {
        /// <summary>
        /// 1.1	UC-PCA-1397-01 Print 1397 Label
        /// 打印1397Label
        /// </summary>
        /// <param name="pdLine">Pd Line</param>
        /// <param name="_1397">1397</param>
        /// <param name="VGA">VGA</param>
        /// <param name="FAN">FAN</param>
        /// <param name="qty">数量</param>
        /// <param name="editor">operator</param>
        /// <returns>Print Items</returns>
        IList<PrintItem> Print1397Label(
            string pdLine,
            string _1397,
            string VGA,
            string FAN,
            int qty,
            string editor, string stationId, string customerId, IList<PrintItem> printItems);

        /// <summary>
        /// 9.	Get CPU Assy/CPU PN/111/VGA/FAN
        /// </summary>
        /// <param name="_1397">1397</param>
        /// <returns>1397 Info</returns>
        Get1397InfoResult Get1397Info(string _1397);

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="sessionKey"></param>
        void Cancel(string sessionKey);
    }

    [Serializable]
    public struct Get1397InfoResult
    {
        public IList<string> CPUAssy;
        //public string CPUAssy;
        public string CPUPN;
        public string _111Level;
        public IList<string> VGA;
        public IList<string> FAN;
    }


}