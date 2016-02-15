// INVENTEC corporation (c)2010 all rights reserved. 
// Description: 对LabelType对象的操作
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2010-01-31   Yuan XiaoWei                 create
// Known issues:
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.DataModel;
using IMES.Infrastructure.UnitOfWork;

namespace IMES.FisObject.Common.PrintItem
{
    /// <summary>
    /// 用于访问LabelType对象的Repository接口
    /// </summary>
    public interface ILabelTypeRepository : IRepository<LabelType>
    {
        #region . For CommonIntf  .
        /// <summary>
        /// 取得所有可用的打印模板
        /// </summary>
        /// <param name="stationId">站点标识</param>
        /// <param name="labelType">标签类型</param>
        /// <returns>可用的打印模板列表</returns>
        //IList<PrintTemplateInfo> GetPrintTemplateList(string stationId, string labelType);
        IList<PrintTemplateInfo> GetPrintTemplateList(string labelType);

        /// <summary>
        /// 取得LabelType的打印模式(Template/Batch: "1" / "0")
        /// </summary>
        /// <param name="stationId">站点标识</param>
        /// <param name="labelType">标签类型</param>
        /// <returns>打印模式</returns>
        //int GetPrintMode(string stationId, string labelType);
        int GetPrintMode(string labelType);

        /// <summary>
        /// 取得LabelType的Rule模式
        /// </summary>
        /// <param name="stationId">站点标识</param>
        /// <param name="labelType">标签类型</param>
        /// <returns>Rule模式</returns>
        //int GetRuleMode(string stationId, string labelType);
        int GetRuleMode(string labelType);

        /// <summary>
        /// 获取指定station可能打印的LabelType
        /// </summary>
        /// <param name="station">station</param>
        /// <returns>指定station可能打印的LabelType</returns>
        IList<string> GetPrintLabelTypeList(string pcode);//(string station);

        #endregion

        /// <summary>
        /// 根据LabelType获取Templates
        /// </summary>
        /// <param name="lblType"></param>
        /// <returns></returns>
        LabelType FillTemplates(LabelType lblType);

        /// <summary>
        /// 根据station获取LabelType列表
        /// </summary>
        /// <param name="station"></param>
        /// <returns></returns>
        IList<LabelType> GetLabelTypesByStation(string pcode);//(string station);

        /// <summary>
        /// 根据templateName获取PrintTemplate
        /// (For Both Runtime And Maintain)
        /// </summary>
        /// <param name="templateName"></param>
        /// <returns></returns>
        PrintTemplate GetPrintTemplate(string templateName);

        /// <summary>
        ///  根据LabelType aand TemplateName获取PrintTemplate
        /// </summary>
        /// <param name="labelType"></param>
        /// <param name="templateName"></param>
        /// <returns></returns>
        PrintTemplate GetPrintTemplate(string labelType, string templateName);

        /// <summary>
        /// 调用[IMES_GetData]中的存储过程IMES_GetTemplate获取模板
        /// </summary>
        /// <param name="labelType"></param>
        /// <param name="model"></param>
        /// <param name="mo"></param>
        /// <param name="dn"></param>
        /// <param name="partno"></param>
        /// <param name="rulemode"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        PrintTemplate GetPrintTemplate(string labelType, string model, string mo, string dn, string partno, int rulemode,string customer);

        /// <summary>
        /// bat打印时,根据存储过程名字和参数执行存储过程,获取主Bat
        /// </summary>
        /// <param name="currentSPName">存储过程名字</param>
        /// <param name="parameterKeys">存储过程需要的参数名字</param>
        /// <param name="parameterValues">存储过程需要的参数值</param>
        /// <returns></returns>
        string GetMainBat(string currentSPName, List<string> parameterKeys, List<List<string>> parameterValues);

        /// <summary>
        /// call bat sp 
        /// </summary>
        /// <param name="currentSPName"></param>
        /// <param name="parameterKeys"></param>
        /// <param name="parameterValues"></param>
        /// <returns></returns>
        string GetMainBat(string currentSPName, List<string> parameterKeys, List<string> parameterValues);

        #region For Maintain

        /// <summary>
        /// 取LabelType表的记录，栏位包括LabelType、PrintMode、RuleMode、Description、Editor、Cdt、Udt
        /// 按LabelType列的字符序排序
        /// </summary>
        /// <returns></returns>
        IList<LabelType> GetLabelTypeList();

        /// <summary>
        /// 返回在LabelType表中等于LabelType的数量 
        /// 参考sql如下：
        /// select count(*) from LabelType where LabelType=?
        /// </summary>
        /// <param name="strLabelType"></param>
        /// <returns></returns>
        int CheckExistedLabelType(string strLabelType);
 
        /// <summary>
        /// 新增LabelType
        /// </summary>
        /// <param name="item"></param>
        void AddLabelType(LabelType item);

        /// <summary>
        /// 修改LabelType
        /// </summary>
        /// <param name="item"></param>
        void SaveLabelType(LabelType item);

        /// <summary>
        /// 删除LabelType
        /// </summary>
        /// <param name="item"></param>
        void DeleteLabelType(LabelType item);

        /// <summary>
        /// 栏位包括Template Name、Piece、SP Name、Description、Editor、Cdt、Udt
        /// 按Template Name列的字符序排序
        /// </summary>
        /// <param name="strLabelType"></param>
        /// <returns></returns>
        IList<PrintTemplate> GetPrintTemplateListByLabelType(string strLabelType);

        /// <summary>
        /// 返回在PrintTemplate表中等于TemplateName的数量 
        /// 参考sql如下：
        /// select count(*) from PrintTemplate where TemplateName=?
        /// </summary>
        /// <param name="strTemplateName"></param>
        /// <returns></returns>
        int CheckExistedTemplateName(string strTemplateName);
      
        /// <summary>
        /// 新增PrintTemplate
        /// </summary>
        /// <param name="item"></param>
        void AddPrintTemplate(PrintTemplate item);
      
        /// <summary>
        /// 修改PrintTemplate
        /// </summary>
        /// <param name="item"></param>
        void SavePrintTemplate(PrintTemplate item);
      
        /// <summary>
        /// 删除PrintTemplate
        /// </summary>
        /// <param name="item"></param>
        void DeletePrintTemplate(PrintTemplate item);

        /// <summary>
        /// 参考sql如下：
        /// select PCode from PCode_LabelType where LabelType=?
        /// </summary>
        /// <param name="strLabelType"></param>
        /// <returns></returns>
        IList<PCodeLabelType> GetPCodeByLabelType(string strLabelType);
   
        /// <summary>
        ///  向PCode_LabelType表中保存记录
        /// </summary>
        /// <param name="item"></param>
        void SavePCode(PCodeLabelType item);

        /// <summary>
        /// 参考sql如下：
        /// select * from LabelRule where TemplateName=?
        /// </summary>
        /// <param name="strTemplateName"></param>
        /// <returns></returns>
        IList<LabelRule> GetLabelRuleByTemplateName(string strTemplateName);

        /// <summary>
        /// 新增LabelRule
        /// </summary>
        /// <param name="?"></param>
        void AddLabelRule(LabelRule item);

        /// <summary>
        /// 删除LabelRule
        /// </summary>
        /// <param name="item"></param>
        void DeleteLabelRule(LabelRule item);

        /// <summary>
        /// 栏位包括ID、Mode、Attribute、Value、Editor、Cdt、Udt
        /// 按Mode列的字符序排序
        /// 参考sql如下：
        /// select * from LabelRuleSet where RuleID=?
        /// </summary>
        /// <param name="RuleID"></param>
        /// <returns></returns>
        IList<LabelRuleSet> GetLabelRuleSetByRuleID(int RuleID);
   
        /// <summary>
        /// 返回在PrintTemplate表中等于TemplateName的数量 
        /// 参考sql如下：
        /// select count(*) from LabelRuleSet where RuleID = ? and Mode = ? and AttributeName=?
        /// </summary>
        /// <param name="strAttributeName"></param>
        /// <returns></returns>
        int CheckExistedAttributeName(int ruleId, string mode, string attributeName);
        
        /// <summary>
        /// 新增LabelRuleSet
        /// </summary>
        /// <param name="?"></param>
        void AddLabelRuleSet(LabelRuleSet item);
    
        /// <summary>
        /// 修改LabelRuleSet
        /// </summary>
        /// <param name="item"></param>
        void SaveLabelRuleSet(LabelRuleSet item);
       
        /// <summary>
        /// 删除LabelRuleSet
        /// </summary>
        /// <param name="item"></param>
        void DeleteLabelRuleSet(LabelRuleSet item);

        /// <summary>
        /// 参考sql如下：
        /// DELETE FROM PCode_LabelType WHERE LabelType=?
        /// </summary>
        /// <param name="strLabelType"></param>
        void DeletePCodeByLabelType(string strLabelType);

        /// <summary>
        /// 根据ID查找LabelRule
        /// </summary>
        /// <param name="ruleId"></param>
        /// <returns></returns>
        LabelRule GetLabelRuleByRuleID(int ruleId);

        /// <summary>
        /// 根据ID查找LabelRuleSet
        /// </summary>
        /// <param name="ruleSetId"></param>
        /// <returns></returns>
        LabelRuleSet GetLabelRuleSetByID(int ruleSetId);

        #region Defered

        void AddLabelTypeDefered(IUnitOfWork uow, LabelType item);

        void SaveLabelTypeDefered(IUnitOfWork uow, LabelType item);

        void DeleteLabelTypeDefered(IUnitOfWork uow, LabelType item);

        void AddPrintTemplateDefered(IUnitOfWork uow, PrintTemplate item);

        void SavePrintTemplateDefered(IUnitOfWork uow, PrintTemplate item);

        void DeletePrintTemplateDefered(IUnitOfWork uow, PrintTemplate item);

        void SavePCodeDefered(IUnitOfWork uow, PCodeLabelType item);

        void AddLabelRuleDefered(IUnitOfWork uow, LabelRule item);

        void DeleteLabelRuleDefered(IUnitOfWork uow, LabelRule item);

        void AddLabelRuleSetDefered(IUnitOfWork uow, LabelRuleSet item);

        void SaveLabelRuleSetDefered(IUnitOfWork uow, LabelRuleSet item);

        void DeleteLabelRuleSetDefered(IUnitOfWork uow, LabelRuleSet item);

        void DeletePCodeByLabelTypeDefered(IUnitOfWork uow, string strLabelType);

        #endregion

        #endregion

        /// <summary>
        /// 1 获取所有OfflineLabelSetting数据
        /// select * from OfflineLabelSetting order by FileName
        /// </summary>
        /// <returns></returns>
        IList<OfflineLableSettingDef> GetAllOfflineLabelSetting();

        /// <summary>
        /// 2 根据FileName获取OfflineLabelSetting数据
        /// select * from OfflineLabelSetting
        // /where FileName = FileName
        /// </summary>
        /// <param name="FileName"></param>
        /// <returns></returns>
        IList<OfflineLableSettingDef> GetOfflineLabelSetting(string FileName);

        /// <summary>
        /// 3 添加一条OfflineLabelSetting数据
        /// insert into OfflineLabelSetting(FileName,Description,LabelSpec,Param1,Param2,Param3 ,Param4,Param5,Param6,Param7,Param8,Editor,Cdt,Udt)
        // /values(@FileName,@Description,@LabelSpec,@Param1,@Param2,@Param3 ,@Param4,@Param5,@Param6,@Param7,@Param8,@Editor,@Cdt,@Udt)
        /// </summary>
        /// <param name="obj"></param>
        void AddOfflineLabelSetting(OfflineLableSettingDef obj);

        /// <summary>
        /// 4 修改OfflineLabelSetting数据
        /// update OfflineLabelSetting
        /// set
        /// FileName =@FileName ,
        /// Description=@Description,
        /// LabelSpec=@LabelSpec,
        /// Param1=@Param1,
        /// Param2=@Param2,
        /// Param3=@Param3,
        /// Param4=@Param4,
        /// Param5=@Param5,
        /// Param6=@Param6,
        /// Param7=@Param7,
        /// Param8=@Param8,
        /// Editor=@Editor,
        /// Cdt=@Cdt,
        /// Udt=@Udt
        /// where FileName = oldFileName
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="oldFileName"></param>
        void UpdateOfflineLabelSetting(OfflineLableSettingDef obj, String oldFileName);

        /// <summary>
        /// 5 根据指定条件，删除OfflineLabelSetting数据
        /// delete from OfflineLabelSetting
        /// where FileName = @FileName
        /// </summary>
        /// <param name="obj"></param>
        void DeleteOfflineLabelSetting(OfflineLableSettingDef obj);

        /// <summary>
        /// 1.获取数据时添加layout字段对应数据
        /// SELECT TemplateName,LabelType,Piece,SpName,Laytout,Description,Editor,Cdt,Udt FROM PrintTemplate WHERE 
        /// LabelType=@LabelType ORDER BY TemplateName
        /// 
        /// condition哪个字段赋值就有哪个条件,自由使用各个条件,条件间是AND关系.
        /// </summary>
        /// <param name="strLabelType"></param>
        /// <returns></returns>
        IList<PrintTemplateEntity> GetPrintTemplateListByLabelType(PrintTemplateEntity condition);

        /// <summary>
        /// setValue哪个字段赋值就有更新哪个字段
        /// condition哪个字段赋值就有哪个条件,自由使用各个条件,条件间是AND关系.
        /// </summary>
        /// <param name="setValue"></param>
        /// <param name="condition"></param>
        void UpdatePrintTemplate(PrintTemplateEntity setValue, PrintTemplateEntity condition);

        #region . Defered  .

        void AddOfflineLabelSettingDefered(IUnitOfWork uow, OfflineLableSettingDef obj);

        void UpdateOfflineLabelSettingDefered(IUnitOfWork uow, OfflineLableSettingDef obj, String oldFileName);

        void DeleteOfflineLabelSettingDefered(IUnitOfWork uow, OfflineLableSettingDef obj);

        void UpdatePrintTemplateDefered(IUnitOfWork uow, PrintTemplateEntity setValue, PrintTemplateEntity condition);

        #endregion

        #region for LabelTypeRule Table
        IList<LabelTypeRuleDef> GetLabeTypeRuleByPCode(string pCode);
        LabelTypeRuleDef GetLabeTypeRuleByLabelType(string labelType);
        IList<LabelTypeRuleDef> GetLabeTypeRuleByLabelType(IList<string> labelTypeList);
        void UpdateAndInsertLabeTypeRule(LabelTypeRuleDef item);
        void DeleteLabelTypeRule(string labelType);
        void UpdateAndInsertModelConstValue(string labelType, string modelConstValue,string editor);
        void UpdateAndInsertDeliveryConstValue(string labelType, string deliveryConstValue, string editor);
        void UpdateAndInsertPartConstValue(string labelType, int bomLevel, string partConstValue, string editor);

        void UpdateAndInsertLabeTypeRuleDefered(IUnitOfWork uow,LabelTypeRuleDef item);
        void DeleteLabelTypeRuleDefered(IUnitOfWork uow, string labelType);
        void UpdateAndInsertModelConstValueDefered(IUnitOfWork uow, string labelType, string modelConstValue, string editor);
        void UpdateAndInsertDeliveryConstValue(IUnitOfWork uow, string labelType, string deliveryConstValue, string editor);
        void UpdateAndInsertPartConstValueDefered(IUnitOfWork uow, string labelType, int bomLevel, string partConstValue, string editor);
        #endregion

        #region for MO_LabelType PO_LabelType and PrintTemplate
        IMES.FisObject.Common.PrintItem.PrintTemplate GetPrintTemplateByMo(string moNo,string labelType);
        IMES.FisObject.Common.PrintItem.PrintTemplate GetPrintTemplateByPo(string po, string labelType);
        //IMES.FisObject.Common.PrintItem.PrintTemplate GetPrintTemplateByName(string templateName);

        void InsertMOLabel(string moNo, string labelType, string templateName);
        void InsertPOLabel(string po, string labelType, string templateName);
        IList<LabelTemplateRuleDef> GetLabelTemplateRule(string labelType);
        #endregion

        #region for Bartender Label
        /// <summary>
        /// Bartendar Label call sp return Name/Value List
        /// </summary>
        /// <param name="currentSPName">sp name</param>
        /// <param name="parameterKeys">sp parameter name</param>
        /// <param name="parameterValues">sp parameter value</param>
        /// <returns></returns>
        IList<NameValueDataTypeInfo> GetBartendarNameValueInfo(string currentSPName, List<string> parameterKeys, List<List<string>> parameterValues);
        #endregion
    }
}
