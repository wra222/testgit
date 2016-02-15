// created by itc98079

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.FisObject.Common.Model;
using IMES.DataModel;

namespace IMES.Maintain.Interface.MaintainIntf
{
    /// <summary>
    /// </summary>
    public interface ILabelSettingManager
    {
        
        /// <summary>
        /// 取得全部LabelType List
        /// </summary>
        /// <returns>LabelType List</returns>
        IList<LabelTypeMaintainInfo> getLableTypeList();

        /// <summary>
        /// 新增LabelType
        /// </summary>
        /// <param name="LabelType">LabelType</param>
        /// <returns></returns>
        void AddLabelType(LabelTypeMaintainInfo objLabelType);

        /// <summary>
        /// 保存LabelType
        /// </summary>
        /// <param name="objLabelType">objLabelType</param>
        /// <param name="strOldLabelType">strOldLabelType</param>
        /// <returns></returns>
        void SaveLabelType(LabelTypeMaintainInfo objLabelType);

        /// <summary>
        /// 删除LabelType
        /// </summary>
        /// <param name="strLabelType">strLabelType</param>
        /// <returns></returns>
        void DeleteLabelType(string strLabelType);


        /// <summary>
        /// 取得全部PrintTemplate List
        /// </summary>
        /// <returns>PrintTemplate List</returns>
        IList<PrintTemplateMaintainInfo> getPrintTemplateListByLableType(string strLableType);

        /// <summary>
        /// 新增PrintTemplate
        /// </summary>
        /// <param name="PrintTemplate">PrintTemplate</param>
        /// <returns></returns>
        void AddPrintTemplate(PrintTemplateMaintainInfo objPrintTemplate);

        /// <summary>
        /// 保存PrintTemplate
        /// </summary>
        /// <param name="PrintTemplate">PrintTemplate</param>
        /// <returns></returns>
        void SavePrintTemplate(PrintTemplateMaintainInfo objPrintTemplate);

        /// <summary>
        /// 删除PrintTemplate
        /// </summary>
        /// <param name="strTemplateName">strTemplateName</param>
        /// <returns></returns>
        void DeletePrintTemplate(string strTemplateName);


        /// <summary>
        /// 取得全部PCodeLabelType List
        /// </summary>
        /// <param name="strLabelType">strLabelType</param>
        /// <returns>PCodeLabelType List</returns>
        IList<PCodeLabelTypeMaintainInfo> getPCodeByLabelType(string strLabelType);


        /// <summary>
        /// 保存PCodeLabelType
        /// 过程包括：先删除等于labeltype的所有记录，然后新增所有最新check的PCode
        /// </summary>
        /// <param name="arrCheckedPCode">arrCheckedPCode</param>
        /// <param name="PCodeLabelTypeInfo">PCodeLabelTypeInfo</param>
        /// <returns></returns>
        void SavePCode(IList<string> arrCheckedPCode, PCodeLabelTypeMaintainInfo PCodeLabelTypeInfo);


        /// <summary>
        /// 取得LabelRule List
        /// </summary>
        /// <param name="strTemplateName">strTemplateName</param>
        /// <returns>LabelRuleMaintainInfo List</returns>
        IList<LabelRuleMaintainInfo> getLabelRuleByTemplateName(string strTemplateName);

        /// <summary>
        /// 新增LabelRule
        /// </summary>
        /// <param name="objLabelRule">objLabelRule</param>
        /// <returns>LabelRuleID</returns>
        int AddLabelRule(LabelRuleMaintainInfo objLabelRule);

        /// <summary>
        /// 删除LabelRule
        /// </summary>
        /// <param name="RuleID">RuleID</param>
        /// <returns></returns>
        void DeleteLabelRule(int RuleID);

        /// <summary>
        /// 取得LabelRuleSet List
        /// </summary>
        /// <param name="RuleID">RuleID</param>
        /// <returns>LabelRuleSet List</returns>
        IList<LabelRuleSetMaintainInfo> GetLabelRuleSetByRuleID(int RuleID);


        /// <summary>
        /// 取得Model List，只取Model字段
        /// 按Model 列的字符序排序
        /// </summary>
        /// <returns>Model List</returns>
        IList<string> GetModelList();

        /// <summary>
        /// 取得ModelInfo List，只取ModelInfo表的Name字段
        /// 按Name 列的字符序排序
        /// </summary>
        /// <returns>ModelInfo List</returns>
        IList<string> GetModelInfoNameList();

        /// <summary>
        /// 取得Delivery List，只取Delivery表的DeliveryNo字段
        /// 按DeliveryNo 列的字符序排序
        /// </summary>
        /// <returns>Delivery List</returns>
        IList<string> GetDeliveryNoList();

        /// <summary>
        /// 取得DeliveryInfoType List，只取DeliveryInfo表的InfoType字段
        /// 按InfoType列的字符序排序
        /// </summary>
        /// <returns>DeliveryInfoType List</returns>
        IList<string> GetDeliveryInfoTypeList();

        /// <summary>
        /// 取得Part List，只取Part表的PartNo字段
        /// 按PartNo列的字符序排序
        /// </summary>
        /// <returns>Part List</returns>
        IList<string> GetPartList();

        /// <summary>
        /// 取得PartInfoType List，只取PartInfo表的InfoType字段
        /// 按InfoType列的字符序排序
        /// </summary>
        /// <returns>PartInfo List</returns>
        IList<string> GetPartInfoTypeList();

        /// <summary>
        /// 新增/保存LabelRuleSet
        /// </summary>
        /// <param name="LabelRuleSet">LabelRuleSet</param>
        /// <returns></returns>
        int SaveLabelRuleSet(LabelRuleSetMaintainInfo objLabelRuleSet);

        /// <summary>
        /// 删除LabelRuleSet
        /// </summary>
        /// <param name="LabelRuleSetID">iLabelRuleSetID</param>
        /// <returns></returns>
        void DeleteLabelRuleSet(int iLabelRuleSetID);

    }

}
