
using System.Collections.Generic;
using IMES.DataModel;
using System.Collections;

namespace IMES.Station.Interface.StationIntf
{
    /// <summary>
    /// IMaterialReturn
    /// </summary>
    public interface IMaterialReturn
    {
        /// <summary>
        /// GetMaterialType
        /// </summary>
        /// <param name="type">string</param>
        /// <param name="value">string</param>
        /// <returns></returns>
        IList<ConstValueTypeInfo> GetMaterialType(string type, string value);

        /// <summary>
        /// InputMaterialCTFirst
        /// </summary>
        /// <param name="materialCT">string</param>
        /// <param name="materialType">string</param>
        /// <param name="line">string</param>
        /// <param name="editor">string</param>
        /// <param name="station">string</param>
        /// <param name="customer">string</param>
        /// <returns></returns>
        ArrayList InputMaterialCTFirst(string materialCT, string materialType, string line, string editor, string station, string customer);

        /// <summary>
        /// InputMaterialCT
        /// </summary>
        /// <param name="materialCT">string</param>
        /// <param name="sessionKey">string</param>
        /// <returns></returns>
        ArrayList InputMaterialCT(string materialCT, string sessionKey);

        /// <summary>
        /// 扫描9999，结束工作流
        /// 如果没有Defect，即defectCodeList为null或cout为0
        /// 将Session.AddValue(Session.SessionKeys.HasDefect,false)
        /// 否则Session.AddValue(Session.SessionKeys.HasDefect,true)
        /// </summary>
        /// <param name="prodId"></param>
        void Save(string prodId);

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="prodId"></param>
        void Cancel(string prodId);


    }
}
