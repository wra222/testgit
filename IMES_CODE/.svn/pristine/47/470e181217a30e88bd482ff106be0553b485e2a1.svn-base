/*
 * INVENTEC corporation (c)2011 all rights reserved. 
 * Description: POD Label Check
 * UI:CI-MES12-SPEC-PAK-UI POD Label Check.docx 
 * UC:CI-MES12-SPEC-PAK-UC POD Label Check.docx 
 * Update: 
 * Date         Name                Reason 
 * ==========   =================   =====================================
 * 2011-11-01   Chen Xu (eB1-4)     create
 * Known issues:
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Station.Interface.CommonIntf;
using IMES.DataModel;

namespace IMES.Station.Interface.StationIntf
{
    /// <summary>
    /// POD Label Check
    /// </summary>
    /// 
    public interface IPodLabelCheck
    {
        #region methods interact with the running workflow

        /// <summary>
        /// 根据刷入的Customer S/N,SFC,获得 Product ID,Customer P/N 信息
        /// Product ID: Product.ProductID;
        /// Customer P/N: Product.Model对应的Model.CustPN
        /// </summary>
        /// <param name="custsn">custsn</param>
        /// <param name="pdLine">pdLine(Line="POD Check")</param>
        /// <param name="editor">editor</param>
        /// <param name="station">station(Station="86")</param>
        /// <param name="customer">customer</param>
        /// <returns>Product ID,Customer P/N</returns>
        ProductModel InputCustSnOnCooLabel(string custsn, string pdLine, string editor, string station, string customer, out string custpn);

        /// <summary>
        /// 保存ProductLog信息
        /// </summary>
        /// <param name="productIdValue">productIdValue</param>
        /// <param name="custSnOnCooValue">custSnOnCooValue</param>
        /// <param name="custPnOnPodValue">custPnOnPodValue</param>
        /// <param name="pdLine">pdLine(Line="POD Check")</param>
        /// <param name="editor">editor</param>
        /// <param name="station">station(Station="86")</param>
        /// <param name="customer">customer</param>
        /// <returns></returns>
        string InputCustPnOnPodLabel(string productIdValue, string custSnOnCooValue,string custPnOnPodValue, string pdLine, string editor, string station, string customer);
    


        #endregion


        #region methods do not interact with the running workflow

  
        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="productId">custsn</param>
        void Cancel(string custsn);

        #endregion

    }
}
