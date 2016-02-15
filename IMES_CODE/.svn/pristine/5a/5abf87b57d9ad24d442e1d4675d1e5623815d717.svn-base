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
    public interface IGenerateCustomerSN
    {
        /// <summary>
        /// 输入Product Id和相关信息
        /// </summary>
        /// <param name="pdLine">Pd Line</param>
        /// <param name="prodId">Product Id</param>
        /// <param name="editor">operator</param>
        ArrayList InputProdId(string pdLine, string prodId, string editor, string stationId, string customer);

        /// <summary>
        /// 输入DDRCT和相关信息
        /// </summary>
        /// <param name="pdLine">Pd Line</param>
        /// <param name="prodId">Product Id</param>
        /// <param name="editor">operator</param>
        ArrayList InputDDRCT(string prodId, string ddrct);
        /// <summary>
        /// 输入MBSno和相关信息
        /// </summary>
        /// <param name="pdLine">Pd Line</param>
        /// <param name="prodId">Product Id</param>
        /// <param name="editor">operator</param>
        ArrayList InputMBSno(string prodId, string mbsno);
        /// <summary>
        /// 保存信息
        /// </summary>
        /// <param name="pdLine">Pd Line</param>
        /// <param name="prodId">Product Id</param>
        /// <param name="editor">operator</param>
        ArrayList save(string prodId);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="prodId"></param>
        /// <param name="printItems"></param>
        /// <returns></returns>
        IList<PrintItem> Print(string prodId, IList<PrintItem> printItems);

        /// <summary>
        /// 重新列印Customer SN
        /// </summary>
        /// <param name="prodId"></param>
        /// <param name="editor"></param>
        /// <param name="stationId"></param>
        /// <param name="customer"></param>
        /// <param name="printItems"></param>
        /// <param name="reason"></param>
        /// <param name="customerSN"></param>
        /// <returns></returns>
        ArrayList Reprint(
            string prodId,
            string editor, string stationId, string customer, IList<PrintItem> printItems, string reason,out string customerSN);

        /// <summary>
        /// Cancel
        /// </summary>
        void Cancel(string prodId);
    }
}
