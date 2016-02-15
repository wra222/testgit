// INVENTEC corporation (c)2011 all rights reserved. 
// Description: PAK UnitWeight Interface
// UI:CI-MES12-SPEC-PAK-UI Unit Weight.docx
// UC:CI-MES12-SPEC-PAK-UC Unit Weight.docx                           
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-11-29   Chen Xu itc208014            create
// Known issues:
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using IMES.Station.Interface.CommonIntf;
using IMES.DataModel;

namespace IMES.Station.Interface.StationIntf
{
    [Serializable]
    public struct S_UnitWeightNew
    {
        public string ProductID;
        public string Model;
        public decimal StandardWeight;
        public string PAQC;
        public string FAI;
        public string IndiaLabel;
        public string ConfigLabel;
        public string PODLabel;
        public string EditsFISAddr;
        public IList<BomItemInfo> BomItemList;
    }


         //retLst.Add(productID);              //  [0] IMES_FA..ProductID
         //       retLst.Add(model);                       //  [1] IMES_FA..Product.Model
         //       retLst.Add(standardWeight);      //  [2] (decimal) IMES_GetData..ModelWeight.PakUnitWeightNew 
         //       retLst.Add(labelType1);             //  [3] PAQC 抽检
         //       retLst.Add(labelType2);             //  [4] Adaptor Label / India Label 判定
         //       retLst.Add(printlabeltype1);       //  [5] print Config Label
         //       retLst.Add(printlabeltype2);       //  [6] print POD Label
         //       retLst.Add(EditsFISAddr);           //  [7]  （\\hp-iis\OUT\）需要查询SysSetting 表获取, 参考方法：select Value from SysSetting nolock where Name = 'EditsFISAddr'
         //       IList<string> printexepathLst = ipartRepository.GetValueFromSysSettingByName("PDFPrintPath");
         //       retLst.Add(bomItemList);  // [8]

    /// <summary>
    ///  UC 具体业务：  本站站号：85
    ///                1. Unit 称重；
    ///                2. 列印Unit Weight Label；
    ///                3. 上传数据至SAP
    /// </summary>
    public interface IPakUnitWeightNew
    {

        #region PakUnitWeight

        /// <summary>
        /// 此站输入的是SN，需要在BLL中先根据SN获取Product调用CommonImpl.GetProductByInput()
        /// 如果获取不到，报CHK079: 找不到与此序号匹配的Product! 
        /// 用ProductID启动工作流
        /// 将获得的Product放到Session.Product中
        /// 将actualWeight放到Session.ActuralWeight中
        /// 获取Model和标准重量和ProductID
        /// </summary>
        /// <param name="custSN"></param>
        /// <param name="actualWeight"></param>
        /// <param name="line"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <param name="configParams"></param>
        /// <returns>ArrayList对象</returns>
        ArrayList InputCustsn(string custSN, decimal actualWeight, string line, string editor, string station, string customer, out List<string> configParams);

      

        /// <summary>
        /// 保存机器重量，更新机器状态，返回打印重量标签的PrintItem，结束工作流。
        /// 将printItems放到Session.PrintItems中
        /// </summary>
        /// <param name="productID"></param>
        /// <param name="printItems"></param>
        /// <returns></returns>
        IList<PrintItem> Save(string productID,  IList<PrintItem> printItems);
        string GetCqPodLabelColor(string model);
        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="custSN"></param>
        void Cancel(string custSN);
        #endregion

        #region Reprint

        /// <summary>
        /// 重印Unit Weight Label
        /// </summary>
        /// <param name="custSN"></param>
        /// <param name="reason"></param>
        /// <param name="line"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <param name="printItemList"></param>
        /// <param name="printlabeltype"></param>
        /// <param name="model"></param>
        /// <param name="printexepath"></param>
        /// <returns></returns>
        IList<PrintItem> ReprintLabel(string custSN, string reason, string line, string editor, string station, string customer, IList<PrintItem> printItemList, out string printlabeltype, out string model, out string printexepath);

        #endregion


        #region GetWeightSettingInfo

        /// <summary>
        /// 获取WeightSettingInfo
        /// </summary>
        /// <param name="hostname">hostname</param>
        /// <returns></returns>
        IList<COMSettingDef> GetWeightSettingInfo(string hostname);

        #endregion


        /// <summary>
        /// 获取ModelWeight
        /// </summary>
        /// <param name="inputData">inputData</param>
        /// <returns></returns>
        ModelWeightDef GetModelWeightByModelorCustSN(string inputData);

        /// <summary>
        /// 保存修改的ModelWeight
        /// </summary>
        /// <param name="item">item</param>
        /// <returns></returns>
        void SaveModelWeightItem(ModelWeightDef item);
        ArrayList GetPODLabelPathAndSite();
        MatchedPartOrCheckItem TryPartMatchCheck(string sessionKey, string checkValue);
    }
}
