/*
 * INVENTEC corporation ?2011 all rights reserved. 
 * Description:Interface for Asset Tag Label Print Page
 * UI:CI-MES12-SPEC-PAK-UI Asset Tag Label Print.docx –2011/10/10 
 * UC:CI-MES12-SPEC-PAK-UC Asset Tag Label Print.docx –2011/10/10            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
* 2011-10-10   Jessica Liu           (Reference Ebook SourceCode) Create
* Known issues:
* TODO：
* UC 具体业务：Product.DeliveryNo 在DeliveryInfo分别查找InfoType=‘CustPo’/‘IECSo’-数据接口已提供，需进一步调试（in Activity：GetProductByCustomerSN）
* UC 具体业务：在ProductLog不存在Station='81' and Status=1 and Line= 'ATSN Print'的记录-数据接口已提供，需进一步调试（in Activity：CheckAssetSNReprint）
* UC 具体业务：Product_Part.Value where ProductID=@prdid and PartNo in (bom中BomNodeType=’AT’对应的Pn)-数据接口已提供，需进一步调试（in Activity：GetProductByCustomerSN）
* UC 具体业务：保存product和Asset SN的绑定关系-- Insert Product_Part values(@prdid,@partpn,@astsn,’’,’AT’,@user,getdate(),getdate()) -数据接口已提供，需进一步调试（in Activity：GenerateAssetTagLabel））
* UC 具体业务：得到IE维护的Asset SN可用范围select @descr=rtrim(Descr) from Maintain (nolock) where Tp='AST' and Code=@cust ，其中@cust是Product.Model对应的ModelInfo中Name=’Cust’对应的值-数据接口尚未定义（in Activity：GenerateAssetTagLabel）
* UC 具体业务：查找CUST（其中@cust是Product.Model对应的ModelInfo中Name=’Cust’对应的值）已用过的最大号-数据接口存在协商问题（in Activity：GenerateAssetTagLabel））
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Station.Interface.CommonIntf;
using IMES.DataModel;

namespace IMES.Station.Interface.StationIntf
{
    public interface IAssetTagLabelPrint
    {

        /// <summary>
        /// 打印Asset Tag Label
        /// </summary>
        /// <param name="inputSn">custsn</param>
        /// <param name="pdLine">product line</param>
        /// <param name="stationId">product station</param>
        /// <param name="editor">editor</param>
        /// <param name="customerId">customer sn</param>
        /// <param name="printItems">Print Item列表</param>
        /// <returns>Print Item列表</returns>
        IList<PrintItem> Print(string custsn, string pdLine, string stationId, string editor, string customerId, IList<PrintItem> printItems, out string astCodeList);

        /// <summary>
        /// 重新打印Asset Tag Label
        /// </summary>
        /// <param name="inputSn">custsn</param>
        /// <param name="pdLine">product line</param>
        /// <param name="stationId">product station</param>
        /// <param name="editor">editor</param>
        /// <param name="customerId">customer sn</param>
        /// <param name="reason">reason</param>
        /// <param name="printItems">Print Item列表</param>
        /// <returns>Print Item列表</returns>
        /// <param name="prodid">ErrorFlag、imageURL</param>
        IList<PrintItem> RePrint(string custsn, string pdLine, string stationId, string editor, string customerId, string reason, IList<PrintItem> printItems, out string ErrorFlag, out string imageURL, out string astCodeList);


        /// <summary>
        /// 检查Customer SN合法性并获得对应Product ID
        /// </summary>
        /// <param name="inputSn">custsn</param>
        /// <param name="pdLine">product line</param>
        /// <param name="stationId">product station</param>
        /// <param name="editor">editor</param>
        /// <param name="customerId">customer sn</param>
        /// <returns>CustomSN和ProductID值</returns>
        List<string> CheckCustomerSN(string custsn, string pdLine, string stationId, string editor, string customerId);
        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="sessionKey"></param>
        void Cancel(string sessionKey);
    }
}