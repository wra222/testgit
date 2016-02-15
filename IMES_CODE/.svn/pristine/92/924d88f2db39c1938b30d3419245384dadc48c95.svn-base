using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;
using System.Collections;

namespace IMES.Station.Interface.StationIntf
{
    /// <summary>
    ///     
    /// </summary>
    public interface IHPPNLabelforRCTO
    {
        /// <summary>
        /// 输入Product Id相关信息并处理
        /// </summary>
        /// <param name="prodId">Product Id</param>
        /// <param name="editor">operator</param>
        /// <returns>prestation</returns>
        ArrayList InputProdId(
            string prodId,string model, string HPPN,
            IList<PrintItem> printItems,
            string editor, string stationId, string customer);

        ArrayList GetProductInfo(
            string prodId,
            string editor, string stationId, string customer);

        /// <summary>
        /// Cancel
        /// </summary>
        void Cancel(string prodId);

    }
}
