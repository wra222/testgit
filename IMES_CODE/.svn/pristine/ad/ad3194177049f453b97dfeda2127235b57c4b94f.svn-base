using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;
using System.Collections;

namespace IMES.Station.Interface.StationIntf
{
    /// <summary>
    /// 刷入OQC检验完成的整机，记录良品/不良品信息（免检模式不走此站）。
    /// 目的：实现整机到Image D/L的移转
    /// </summary>
    public interface MasterLabelPrint
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
            IList<PrintItem> printItems,
            string editor, string stationId, string customer);

        /// <summary>
        /// Cancel
        /// </summary>
        void Cancel(string prodId);

        /// <summary>
        /// 输入Product Id相关信息并处理
        /// </summary>
        /// <param name="pdLine">Pd Line</param>
        /// <param name="prodId">Product Id</param>
        /// <param name="editor">operator</param>
        /// <returns>prestation</returns>
        ArrayList rePrint(
            string pdLine,
            string prodId,
            string reason,
            IList<PrintItem> printItems,
            string editor,
            string stationId,
            string customer);

        /// <summary>
        /// CheckJamestown
        /// </summary>
        /// <param name="pdLine">Pd Line</param>
        /// <param name="prodId">Product Id</param>
        /// <returns>prestation</returns>
        ArrayList CheckJamestown(
            string prodId,
            string pdLine, string editor, string stationId, string customer);

        /// <summary>
        /// CheckDDRCT
        /// </summary>
        /// <param name="pdLine">Pd Line</param>
        /// <param name="ddrct">ddrct</param>
        /// <param name="prodId">Product Id</param>
        /// <returns>prestation</returns>
        ArrayList CheckDDRCT(
            string prodId,string ddrct,
            string pdLine, string editor, string stationId, string customer);
        
        /// <summary>
        /// CheckCustsn
        /// </summary>
        /// <param name="pdLine">Pd Line</param>
        /// <param name="prodId">Product Id</param>
        /// <param name="sn">sn</param>
        /// <returns>prestation</returns>
        bool CheckCustsn(
            string prodId,
            string sn,
            string pdLine, string editor, string stationId, string customer);

		/// <summary>
        /// GetCustsn
        /// </summary>
        string GetCustsn(
            string prodId,
            string pdLine, string editor, string stationId, string customer);
    }
}
