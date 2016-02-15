/*
 * INVENTEC corporation ?2011 all rights reserved. 
 * Description:Interface for SN Check Page
 * UI:CI-MES12-SPEC-PAK-UI SN Check.docx –2011/10/20 
 * UC:CI-MES12-SPEC-PAK-UC SN Check.docx –2011/10/20            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
* 2011-10-20   Jessica Liu           (Reference Ebook SourceCode) Create
* Known issues:
* TODO：
* UC 具体业务：判断是否需要检查Asset Tag SN Check：	Select b.Tp,Type,@message=Message,Sno1, @lwc=L_WC, @mwc=M_WC from Special_Det a,Special_Maintain b where a.SnoId=@Productid and b.Type=a.Tp and b.SWC=”68”得到的Tp=”K”的记录时，表示需要检查Asset Tag SN Check是否做过 (目前只考虑得到一条记录的情况??)-数据接口尚未定义（in Activity：CheckAssetTagSN）
* UC 具体业务：当BOM(存在PartType=ALC and BomNodeType=PL的part) 且model<>PC4941AAAAAY时，表示有ALC，这时没有真正的Pizza盒-数据接口尚未定义（in Activity：CheckSNIdentical）
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Station.Interface.CommonIntf;
using IMES.DataModel;

namespace IMES.Station.Interface.StationIntf
{
    public interface ISNCheck
    {
        /// <summary>
        /// SN Check初次输入SN处理
        /// </summary>
        /// <param name="inputSn">custsn</param>
        /// <param name="pdLine">Pd Line</param>
        /// <param name="editor">operator</param>
        /// <param name="stationId">stationId</param>
        /// <param name="customerId">customerId</param>
        /// <returns>ProductID</returns>
        String InputCustSNOnProduct(string custsn, string pdLine, string editor, string stationId, string customerId);
        ArrayList InputCustSNOnProduct2(string custsn, string pdLine, string editor, string stationId, string customerId);
		


        /// <summary>
        /// SN Check第二次输入SN处理
        /// </summary>
        /// <param name="custsnOnPizza">Customer SN On Pizza</param>
        /// <param name="custsnOnProduct">Customer SN On Product</param>
        /// <param name="pdLine">Pd Line</param>
        /// <param name="editor">operator</param>
        /// <param name="stationId">stationId</param>
        /// <param name="customerId">customerId</param>
        void InputCustSNOnPizza(string custsnOnPizza, string custsnOnProduct, string pdLine, string editor, string stationId, string customerId);


        //2011-10-24，新增，为了打印信息
        /// <summary>
        /// SN Check第二次输入SN处理，返回PAQC以及ALC/NO-ALC
        /// </summary>
        /// <param name="custsnOnPizza">Customer SN On Pizza</param>
        /// <param name="custsnOnProduct">Customer SN On Product</param>
        /// <param name="pdLine">Pd Line</param>
        /// <param name="editor">operator</param>
        /// <param name="stationId">stationId</param>
        /// <param name="customerId">customerId</param>
        /// <returns>SN Info</returns>
        string InputCustSNOnPizzaReturn(string custsnOnPizza, string custsnOnProduct, string pdLine, string editor, string stationId, string customerId,string cartonPn);

        IList<PrintItem> InputCustSNOnPizzaReturn(string custsnOnPizza, string custsnOnProduct, string pdLine, string editor, string stationId, string customerId,string cartonPn, IList<PrintItem> printItems);

        /// <summary>
        /// SN Check第二次输入SN后判断是否一致的处理
        /// </summary>
        /// <param name="custsnOnPizza">Customer SN On Pizza</param>
        /// <param name="custsnOnProduct">Customer SN On Product</param>
        /// <param name="pdLine">Pd Line</param>
        /// <param name="editor">operator</param>
        /// <param name="stationId">stationId</param>
        /// <param name="customerId">customerId</param>
        void CheckTwoSNIdentical(string custsnOnPizza, string custsnOnProduct, string pdLine, string editor, string stationId, string customerId);

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="sessionKey"></param>
        void Cancel(string sessionKey);

        /// <summary>
        /// 重印标签
        /// </summary>
        /// <param name="custSN"></param>
        /// <param name="reason"></param>
        /// <param name="line"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <param name="printItems"></param>
        /// <returns></returns>
        IList<PrintItem> ReprintEnergyLabel(string custSN, string reason, string line, string editor,
                                    string station, string customer, IList<PrintItem> printItems);


        String InputCustsnForCQ(string custsn, string pdLine, string editor, string stationId, string customerId,bool isNeedEnergyLabel);
        ArrayList InputCustsnOnPizzaForCQ(string custsnOnPizza, string custsnOnProduct, string pdLine, string editor, string stationId, string customerId, IList<PrintItem> printItems);

    }
}