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
    public interface IPNOPLabelPrint
    {
        /// <summary>
        /// InputModel
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        ArrayList InputModel(string Model);
        /// <summary>
        /// InitDCode
        /// </summary>
        /// <returns></returns>
        IList<WarrantyDef> InitDCode();
        /// <summary>
        /// InputMBSN
        /// </summary>
        /// <param name="MBSN"></param>
        /// <param name="PdLine"></param>
        /// <param name="DCode"></param>
        /// <param name="Model"></param>
        /// <param name="InfoValue"></param>
        /// <param name="printItems"></param>
        /// <param name="editor"></param>
        /// <param name="stationId"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        ArrayList InputMBSN(
            string MBSN,
            string PdLine,
            string DCode,
            string Model,
            string InfoValue,
            IList<PrintItem> printItems,
            string editor,
            string stationId,
            string customer);

        /// <summary>
        /// Cancel
        /// </summary>
        /// <param name="prodId"></param>
        void Cancel(string prodId);
    }
}
