using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Station.Interface.CommonIntf;
using IMES.DataModel;

namespace IMES.Station.Interface.StationIntf
{
    /// <summary>
    /// 根据机型和日期产生Service Tag（客户序号），并列印label
    /// 目的：产生客户序号，并建立客户序号与ProdId的一一对应关系，完成厂内管控与客户管控的衔接
    /// </summary>
    public interface IEEPLabelPrint
    {
        /// <summary>
        /// 输入Product Id和相关信息
        /// </summary>
        /// <param name="pdLine">Pd Line</param>
        /// <param name="prodId">Product Id</param>
        /// <param name="editor">operator</param>
        ArrayList InputProdId(string pdLine, string prodId, string editor, string stationId, string PrintLogName, string customer, IList<PrintItem> printItems);

        /// <summary>
        /// Reprint
        /// </summary>
        /// <param name="prodid"></param>
        /// <param name="reason"></param>
        /// <param name="line"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <param name="pCode"></param>
        /// <param name="printItems"></param>
        /// <returns></returns>
        ArrayList Reprint(string prodid, string reason, string line, string editor, string station, string customer, IList<PrintItem> printItems);

        /// <summary>
        /// Cancel
        /// </summary>
        void Cancel(string prodId);
    }
}
