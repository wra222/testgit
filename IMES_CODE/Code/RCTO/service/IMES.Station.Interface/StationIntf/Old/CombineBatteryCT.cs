using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Station.Interface.CommonIntf;
using IMES.DataModel;



namespace IMES.Station.Interface.StationIntf
{
    public interface ICombineBatteryCT
    {
        /// <summary>
        /// 输入CustSN和相关信息
        /// </summary>
        /// <param name="pdLine">PdLine</param>
        /// <param name="custsn">CustSN</param>
        /// <param name="editor">operator</param>
        string InputCustSN(
            string pdLine,
            string custsn,
            string stationId, string editor, string customer, out string model, out string ActualLine);

        /// <summary>
        /// 输入Battery CT
        /// </summary>
        /// <param name="prodId">Product Id</param>
        /// <param name="custsn">Cust SN</param>
        /// <param name="batteryct">Battery CT</param>
        void InputBatteryCT(
            string prodId,
            string custsn,
            string batteryct);

        /// <summary>
        /// Cancel
        /// </summary>
        void Cancel(string prodId);
    }
}
