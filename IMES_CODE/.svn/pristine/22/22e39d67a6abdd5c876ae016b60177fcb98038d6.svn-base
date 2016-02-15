/*
 * INVENTEC corporation ?2011 all rights reserved. 
 * Description:Interface for BT Change Page
 * UI:CI-MES12-SPEC-PAK-BT_CHANGE.docx –2011/10/28 
 * UC:CI-MES12-SPEC-PAK-BT_CHANGE.docx –2011/10/28            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
* 2011-10-28   Jessica Liu           (Reference Ebook SourceCode) Create
* Known issues:
* TODO：
* UC 具体业务：确认不是一个已存在的Model[数据表ProductBT：栏位：ProductID（char（10）），BT（varchar（50）），Editor，Cdt,Udt]-接口待测试（in Activity：CheckModelValid）
* UC 具体业务：查找符合条件的Product：1） ProductStatus.Line的Stage=“FA” 2） ProductStatus.Line的Stage=“PAK”并且ProductStatus.Station=“69”-接口待测试（in Activity：GetProductByProductStatus）
* UC 具体业务：BT 转 非BT：删除ProductBT表中符合条件的Product-接口待测试（in Activity：BTChangeToUnBT）
* UC 具体业务：非BT 转 BT：将符合条件的Product添加到ProductBT表中ProductBT.BT ='BT'+convert(nvarchar(20),getdate(),112)-接口待测试（in Activity：unBTChangeToBT）
*/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Station.Interface.CommonIntf;
using IMES.DataModel;

namespace IMES.Station.Interface.StationIntf
{
    public interface IBTChange
    {
        /// <summary>
        /// 检查输入Model的合法性
        /// </summary>
        /// <param name="model">model</param>
        /// <param name="pdLine">Pd Line</param>
        /// <param name="editor">operator</param>
        /// <param name="stationId">stationId</param>
        /// <param name="customerId">customerId</param> 
        void InputModel(string model, string pdLine, string editor, string stationId, string customerId);



        /// <summary>
        /// 将属于所输Model的 符合条件的Product转成BT类型
        /// </summary>
        /// <param name="model">model</param>
        /// <param name="BTToUnBT">BT To unBT Flag</param>
        /// <param name="pdLine">Pd Line</param>
        /// <param name="editor">operator</param>
        /// <param name="stationId">stationId</param>
        /// <param name="customerId">customerId</param>
        void DoBTChange(string model, bool BTToUnBT, string pdLine, string editor, string stationId, string customerId);



        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="sessionKey"></param>
        void Cancel(string sessionKey);
    }
}