// INVENTEC corporation (c)2012 all rights reserved. 
// Description: PAK UnitWeight Interface
// UI:CI-MES12-SPEC-PAK-DATA MAINTAIN2.docx
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2012-04-18   Chen Xu itc208014            create
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
    /// <summary>
    ///  UC 具体业务： 根据刷入的Model号或者CustSN号，获得当前标准重量，并支持修改该重量
    /// </summary>
    public interface IModelWeight
    {
        /// <summary>
        /// 获取ModelWeight
        /// </summary>
        /// <param name="inputData">inputData</param>
        /// <returns></returns>
        ArrayList GetModelWeightByModelorCustSN(string inputData);

        /// <summary>
        /// 保存修改的ModelWeight
        /// </summary>
        /// <param name="item">item</param>
        /// <returns></returns>
        void SaveModelWeightItem(ModelWeightDef item);
        IList<string> GetPrIDListInHoldByModel(string model);
        void SaveModelWeightItemAndHold(ModelWeightDef item, IList<string> lstPrdID,string holdStation,string defectCode);
        List<string> GetDefectCodeList(string type);
        IList<HoldInfo> GetHoldProductList(string model);
        void ReleaseHold(IList<string> lstPrdID, IList<int> lstHoldID, string defectCode, string editor);
    }
}
