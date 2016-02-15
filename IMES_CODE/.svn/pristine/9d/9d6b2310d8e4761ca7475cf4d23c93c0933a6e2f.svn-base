// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 对Process对象的操作
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2009-12-09   Yuan XiaoWei                 create
// Known issues:
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.UnitOfWork;
using IMES.DataModel;
using System.Data;

namespace IMES.FisObject.Common.Process
{
    /// <summary>
    /// 用于访问Process对象的Repository接口
    /// </summary>
    public interface IProcessRepository : IRepository<Process>
    {
        /// <summary>
        /// 读取流程站序列
        /// </summary>
        /// <param name="proc"></param>
        /// <returns></returns>
        Process FillProcessStations(Process proc);

        /// <summary>
        /// 根据机型获取Process对象
        /// </summary>
        /// <param name="model">指定机型</param>
        /// <returns>Process对象</returns>
        IList<Process> GetProcessByModel(string model);

        /// <summary>
        /// SFC卡站
        /// </summary>
        /// <param name="Line"></param>
        /// <param name="Customer"></param>
        /// <param name="CurrentStation"></param>
        /// <param name="key"></param>
        /// <param name="ProcessType"></param>
        void SFC(string Line, string Customer, string CurrentStation, string key, string ProcessType);

        /// <summary>
        /// 建立ModelProcess
        /// </summary>
        /// <param name="model"></param>
        /// <param name="editor"></param>
        /// <param name="line"></param>
        void CreateModelProcess(string model, string editor,string line);

        /// <summary>
        /// 找到DISMANTLE Process，得到对应的Release Type：
        /// select ReleaseType from Rework_ReleaseType where Process=(select Process from Rework_Process where ReworkCode=@code)
        /// </summary>
        /// <param name="reworkCode"></param>
        /// <returns></returns>
        IList<string> GetReleaseType(string reworkCode);

        /// <summary>
        /// 获得ModelProcess
        /// </summary>
        /// <param name="model"></param>
        /// <param name="line"></param>
        /// <returns></returns>
        IList<ModelProcess> GetModelProcessByModelLine(string model, string line);

        #region For Maintain

        /// <summary>
        /// 取process表中等于process的纪录 order by Process
        /// </summary>
        /// <param name="process_like"></param>
        /// <returns></returns>
        IList<Process> getProcessList(string process_like);

        /// <summary>
        /// 取model_process表中等于model的纪录
        /// </summary>
        /// <param name="model_like"></param>
        /// <returns></returns>
        IList<ModelProcess> GetModelProcessListByModel(string model_like);

        /// <summary>
        /// 取palletprocess表中等于customer的纪录
        /// </summary>
        /// <param name="customer_like"></param>
        /// <returns></returns>
        IList<PalletProcess> GetPalletProcessListByCustomer(string customer_like);

        /// <summary>
        /// 取partprocess表中等于mbFaily的纪录
        /// </summary>
        /// <param name="mbFamily_like"></param>
        /// <returns></returns>
        IList<PartProcess> GetPartProcessListByMBFamily(string mbFamily_like);

        /// <summary>
        /// 取rework_process表中等于reworkCode的纪录
        /// </summary>
        /// <param name="reworkCode_like"></param>
        /// <returns></returns>
        IList<ReworkProcess> GetReworkProcessListByReworkCode(string reworkCode_like);

        /// <summary>
        /// 取palletprocess表中等于process的纪录
        /// </summary>
        /// <param name="process"></param>
        /// <returns></returns>
        IList<PalletProcess> GetPalletProcessListByProcess(string process);

        /// <summary>
        /// 取partprocess表中等于process的纪录
        /// </summary>
        /// <param name="process"></param>
        /// <returns></returns>
        IList<PartProcess> GetPartProcessListByProcess(string process);

        /// <summary>
        /// 取rework_process表中等于process的纪录
        /// </summary>
        /// <param name="process"></param>
        /// <returns></returns>
        IList<ReworkProcess> GetReworkProcessListByProcess(string process);

        /// <summary>
        /// 取palletprocess表中所有纪录
        /// </summary>
        /// <returns></returns>
        IList<PalletProcess> GetPalletProcessList();

        /// <summary>
        /// 取partprocess表中所有纪录
        /// </summary>
        /// <returns></returns>
        IList<PartProcess> GetPartProcessList();

        /// <summary>
        /// 取rework_process表中所有纪录
        /// </summary>
        /// <returns></returns>
        IList<ReworkProcess> GetReworkProcessList();

        /// <summary>
        /// 删除palletprocess表中等于process的记录
        /// </summary>
        /// <param name="process"></param>
        void DeletePalletProcessByProcess(string process);

        /// <summary>
        /// 删除partprocess表中等于process的记录
        /// </summary>
        /// <param name="process"></param>
        void DeletePartProcessByProcess(string process);

        /// <summary>
        /// 删除reworkprocess表中等于process的记录
        /// </summary>
        /// <param name="process"></param>
        void DeleteReworkProcessByProcess(string process);

        /// <summary>
        /// 取得processstation表中等于process的记录
        /// 按Pre Station, Status, Station排序
        /// </summary>
        /// <param name="process"></param>
        /// <returns></returns>
        IList<ProcessStation> GetProcessStationList(string process);

        /// <summary>
        /// 从processstation表中取得一条记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ProcessStation GetProcessStation(int id);

        /// <summary>
        /// 删除processstation表中等于id的记录
        /// </summary>
        /// <param name="id"></param>
        void DeleteProcessStation(int id);

        /// <summary>
        /// 在processstation表中新增一条记录
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        int AddProcessStation(ProcessStation obj);

        /// <summary>
        /// 在processstation表中保存一条记录
        /// </summary>
        /// <param name="obj"></param>
        void SaveProcessStation(ProcessStation obj);

        /// <summary>
        /// 向partprocess新增一条记录
        /// </summary>
        /// <param name="obj"></param>
        void AddPartProcess(PartProcess obj);

        /// <summary>
        /// 向reworkprocess新增一条记录
        /// </summary>
        /// <param name="obj"></param>
        void AddReworkProcess(ReworkProcess obj);

        /// <summary>
        /// 向palletprocess新增一条记录
        /// </summary>
        /// <param name="obj"></param>
        void AddPalletProcess(PalletProcess obj);

        /// <summary>
        /// select * from ProcessRuleset order by Priority desc
        /// </summary>
        /// <returns></returns>
        IList<ProcessRuleSet> GetAllProcessRuleset();

        /// <summary>
        /// update ProcessRuleset 
        /// set Priority=ruleSet.Priority 
        /// where ID=ruleSet.ID
        /// </summary>
        /// <param name="ruleSet"></param>
        void UpdateRuleSetPriority(ProcessRuleSet ruleSet);

        /// <summary>
        /// 删除当前被选Rule Set及所有应用该Rule Set的所有Rule
        /// 表ProcessRuleset，表ProcessRule
        /// </summary>
        /// <param name="rule_set_id"></param>
        void DeleteProcessRulesetByID(int rule_set_id);

        /// <summary>
        /// insert ProcessRuleset(Priority,Condition1, Condition2, Condition3, Condition4, Condition5) 
        /// select ISNULL(max(Priority),'0')+1,' ruleset .Condition1' , ' ruleset .Condition2', ' ruleset .Condition3', ' ruleset .Condition4', ' ruleset .Condition5'
        /// from ProcessRuleset
        /// </summary>
        /// <param name="ruleset"></param>
        void AddProcessRuleSet(ProcessRuleSet ruleset);

        /// <summary>
        /// update ProcessRuleset 
        /// set Condition1='singleRuleSet.Condition1' ,
        /// Condition2='singleRuleSet.Condition2' ,
        /// Condition3='singleRuleSet.Condition3' ,
        /// Condition4='singleRuleSet.Condition4' ,
        /// Condition5='singleRuleSet.Condition5' ,
        /// where ID='singleRuleSet.ID'
        /// </summary>
        /// <param name="singleRuleSet"></param>
        void UpdateProcessRuleSet(ProcessRuleSet singleRuleSet);

        /// <summary>
        /// select * from ProcessRule where RuleSetID='rule_set_id' order by Process
        /// </summary>
        /// <param name="rule_set_id"></param>
        /// <returns></returns>
        IList<ProcessRule> GetAllRuleByRuleSetID(int rule_set_id);

        /// <summary>
        /// 返回插入数据的ID
        /// insert ProcessRule(RuleSetID,Process, Value1, Value2, Value 3, Value 4, Value 5) 
        /// values(‘processrule.RuleSetID’,’ processrule.Process’,’ processrule.Value1’,’ processrule.Value2’,’ processrule.Value3’,’ processrule.Value4’,’ processrule.Value5’)
        /// </summary>
        /// <param name="processrule"></param>
        void AddProcessRule(ProcessRule processrule);

        /// <summary>
        /// Update  ProcessRule
        /// Set RuleSetID='processrule.RuleSetID',
        /// Process='processrule.Process', 
        /// Value1='processrule.Value1',
        /// Value2='processrule.Value2',
        /// Value3='processrule.Value3',
        /// Value4='processrule.Value4', 
        /// Value5='processrule.Value5' 
        /// WhereID='processrule.ID'
        /// </summary>
        /// <param name="processrule"></param>
        void UpdateProcessRule(ProcessRule processrule);

        /// <summary>
        /// 删除ProcessRule
        /// </summary>
        /// <param name="id"></param>
        void DeleteProcessRuleByID(int id);

        /// <summary>
        /// 取得process表中等于processName的记录数
        /// </summary>
        /// <param name="processName"></param>
        /// <returns></returns>
        int CheckExistedProcess(string processName);

        /// <summary>
        /// 删除一条Model_Process表的记录，where Model=?
        /// </summary>
        /// <param name="modelName"></param>
        void DeleteModelProcessByModel(string modelName);

        /// <summary>
        /// 保存Process
        /// </summary>
        /// <param name="strOldProcessName"></param>
        /// <param name="obj"></param>
        void SaveProcess(string strOldProcessName, Process obj);

        /// <summary>
        /// select b.ID from ProcessRuleSet a inner join ProcessRule b on a.ID = b.RuleSetID where a.Condition1='Model' 
        /// and ISNULL(a.Condition2, '')='' and b.Value1 = ? and ISNULL(b.Value2, '')=''
        /// </summary>
        /// <param name="modelName"></param>
        /// <returns></returns>
        IList<int> GetProcessRuleIDByModel(string modelName);

        /// <summary>
        /// Delete ProcessRule Where RuleSetID='ruleSetId'
        /// </summary>
        /// <param name="ruleSetId"></param>
        void DeleteRuleByRuleSetID(int ruleSetId);

        /// <summary>
        /// 根据ID获得ProcessRule
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ProcessRule GetProcessRuleById(int id);

        /// <summary>
        /// 根据ID获得ProcessRuleSet
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ProcessRuleSet GetRuleSetById(int id);

        /// <summary>
        /// Truncate table Model_Process
        /// </summary>
        void TruncateModelProcess();

        ////Delete Model_Process where Model=?
        //void DeleteModelProcessByModel(string Model);

        /// <summary>
        /// 取得Rework_ReleaseType表中的所有Process,按字符序排列
        /// 参考sql
        /// select distinct a.* 
        /// from Process a
        ///      join Rework_ReleaseType b on a.Process=b.Process 
        /// order by a.Process
        /// </summary>
        /// <returns></returns>
        IList<Process> GetAllProcessForRework();

        /// <summary>
        /// delete from Model_Process
        /// </summary>
        void DeleteAllModelProcess();

        /// <summary>
        /// 判断RuleSet是否已经存在 
        /// 参考sql:
        /// Select * from ProcessRuleset where Condition1=? and Condition2=? and Condition3=? and Condition4=? and Condition5=? And ID <> ?
        /// </summary>
        /// <param name="ruleset"></param>
        /// <returns></returns>
        bool IFRuleSetIsExists(ProcessRuleSet ruleset);

        /// <summary>
        /// 判断Rule是否已经存在
        /// 参考sql：
        /// Select * from ProcessRule where RuleSetId=? And Process=? And Value1=? And Value2=? And Value3=? And Value4=? And Value5=?  And ID <> ?
        /// </summary>
        /// <param name="rule"></param>
        /// <returns></returns>
        bool IFRuleIsExists(ProcessRule rule);

        /// <summary>
        /// 判断ProcessRuleset是否是Model
        /// 参考sql：
        /// Select * from ProcessRuleset where ID=? And Condition1=’Model’ and isnull(Condition2,’’)==’’ and isnull(Condition3,’’)==’’ and isnull(Condition4,’’)==’’ and isnull(Condition5,’’)==’’
        /// </summary>
        /// <param name="rulesetID"></param>
        /// <returns></returns>
        bool IFConditionIsModel(int rulesetID);

        /// <summary>
        /// 取rework_releasetype表中等于process的纪录
        /// </summary>
        /// <param name="process"></param>
        /// <returns></returns>
        IList<ReworkReleaseType> GetReworkReleaseTypeListByProcess(string process);

        /// <summary>
        /// 选项包括：BWEIGHT、CN、COA、CPQSNO、Delivery、KIT ID、MB、MMI、PCMAC、PLT、WEIGHT和PartCheck和CheckItem中所有type(Distinct)
        /// 参考sql如下：
        /// select distinct ReleaseType from 
        /// (     (select 'BWEIGHT' as ReleaseType)
        /// union (select 'CN' as ReleaseType)
        /// union (select 'COA' as ReleaseType)
        /// union (select 'CPQSNO' as ReleaseType)
        /// union (select 'Delivery' as ReleaseType)
        /// union (select 'KIT ID' as ReleaseType)
        /// union (select 'MB' as ReleaseType)
        /// union (select 'MMI' as ReleaseType)
        /// union (select 'PCMAC' as ReleaseType)
        /// union (select 'PLT' as ReleaseType)
        /// union (select 'WEIGHT' as ReleaseType)
        /// union (select 'CN' as ReleaseType)
        /// union
        /// (select distinct PartType as ReleaseType from PartCheck)
        /// union
        /// (select distinct ItemName as ReleaseType from CheckItem)) as A
        /// order by ReleaseType
        /// </summary>
        /// <returns></returns>
        IList<string> GetReworkReleaseTypeList();

        /// <summary>
        /// 删除rework_releasetype表中等于process的记录
        /// </summary>
        /// <param name="process"></param>
        void DeleteReworkReleaseTypeByProcess(string process);

        /// <summary>
        /// 向rework_releasetype表新增一条记录
        /// </summary>
        /// <param name="item"></param>
        void AddReworkReleaseType(ReworkReleaseType item);

        /// <summary>
        /// 返回在Process_Station表中等于Process，Station和PreStation的值对的数量
        /// 参考sql如下：
        /// select count(*) from Process_Station where Process=? and Station = ? and PreStation = ?
        /// </summary>
        /// <param name="process"></param>
        /// <param name="station"></param>
        /// <param name="preStation"></param>
        /// <returns></returns>
        int CheckExistedProcessStation(string process, string station, string preStation);

        /// <summary>
        /// 参考sql:
        /// select distinct Descr as Family from Part where BomNodeType='MB'
        /// </summary>
        /// <returns></returns>
        IList<string> GetPCBFamilyList();

        /// <summary>
        /// 参考sql:
        /// select distinct PCBModelID as PCBModel from IMES_PCA..PCB order by PCBModel
        /// </summary>
        /// <returns></returns>
        IList<string> GetPCBModelList();

        IList<ProcessRule> GetRuleListByRuleSetIdAndProcess(int ruleSetId, string processName);

        /// <summary>
        /// 1）取得process列表
        /// SELECT [Type]
        /// ,[Process]
        /// ,[Descr]
        /// ,[Editor]
        /// ,[Cdt]
        /// ,[Udt]
        /// FROM [IMES2012_GetData].[dbo].[Process] ORDER BY [Type],[Process]
        /// </summary>
        /// <returns></returns>
        IList<ProcessMaintainInfo> GetProcessList();

        /// <summary>
        /// 2）
        /// if Exists
        /// (SELECT [ID]
        /// ,[RuleSetID]
        /// ,[Process]
        /// ,[Value1]
        /// ,[Value2]
        /// ,[Value3]
        /// ,[Value4]
        /// ,[Value5]
        /// ,[Value6]
        /// ,[Editor]
        /// ,[Cdt]
        /// ,[Udt]
        /// FROM [IMES2012_GetData].[dbo].[ProcessRule] where [Process]=@Process)
        /// select true
        /// else
        /// select false
        /// </summary>
        /// <param name="process"></param>
        /// <returns></returns>
        bool ExistProcessRule(string process);

        /// <summary>
        /// 3)批量添加ProcessStation
        /// </summary>
        /// <param name="stationList"></param>
        void AddProcessStations(List<ProcessStation> stationList);

        /// <summary>
        /// SELECT [MBFamily],Process 
        /// FROM PartProcess
        /// where [MBFamily]='MBFamily'
        /// </summary>
        /// <param name="mbFamily"></param>
        /// <returns></returns>
        DataTable GetExistPartProcess(string mbFamily);

        #region Defered

        void DeletePalletProcessByProcessDefered(IUnitOfWork uow, string process);

        void DeletePartProcessByProcessDefered(IUnitOfWork uow, string process);

        void DeleteReworkProcessByProcessDefered(IUnitOfWork uow, string process);

        void DeleteProcessStationDefered(IUnitOfWork uow, int id);

        void AddProcessStationDefered(IUnitOfWork uow, ProcessStation obj);

        void SaveProcessStationDefered(IUnitOfWork uow, ProcessStation obj);

        void AddPartProcessDefered(IUnitOfWork uow, PartProcess obj);

        void AddReworkProcessDefered(IUnitOfWork uow, ReworkProcess obj);

        void AddPalletProcessDefered(IUnitOfWork uow, PalletProcess obj);

        void UpdateRuleSetPriorityDefered(IUnitOfWork uow, ProcessRuleSet ruleSet);

        void DeleteProcessRulesetByIDDefered(IUnitOfWork uow, int rule_set_id);

        void AddProcessRuleSetDefered(IUnitOfWork uow, string condition);

        void UpdateProcessRuleSetDefered(IUnitOfWork uow, ProcessRuleSet singleRuleSet);

        void AddProcessRuleDefered(IUnitOfWork uow, ProcessRule processrule);

        void UpdateProcessRuleDefered(IUnitOfWork uow, ProcessRule processrule);

        void DeleteProcessRuleByIDDefered(IUnitOfWork uow, int id);

        void DeleteModelProcessByModelDefered(IUnitOfWork uow, string modelName);

        void SaveProcessDefered(IUnitOfWork uow, string strOldProcessName, Process obj);

        void DeleteRuleByRuleSetIDDefered(IUnitOfWork uow, int ruleSetId);

        //void TruncateModelProcessDefered(IUnitOfWork uow);

        //void DeleteModelProcessByModelDefered(IUnitOfWork uow, string Model);

        void DeleteAllModelProcessDefered(IUnitOfWork uow);

        void DeleteReworkReleaseTypeByProcessDefered(IUnitOfWork uow, string process);

        void AddReworkReleaseTypeDefered(IUnitOfWork uow, ReworkReleaseType item);

        void AddProcessStationsDefered(IUnitOfWork uow, List<ProcessStation> stationList);

        #endregion

        #region for material Process

        void AddMaterialProcess(string materialType, string process, string editor);
        void RemoveMaterialProcess(string process);
        void RemoveMaterialProcessByType(string materialType);

        void AddMaterialProcessDefered(IUnitOfWork uow,string materialType, string process, string editor);
        void RemoveMaterialProcessDefered(IUnitOfWork uow,string process);
        void RemoveMaterialProcessByTypeDefered(IUnitOfWork uow,string materialType);

        IList<MaterialProcess> GetMaterialProcessByProcess(string process);
        MaterialProcess GetMaterialProcessByType(string materialType);
        IList<ProcessStation> GetMaterialProcessNextStatus(string materialType, string curStatus);
        bool CheckMaterialProcessStatus(string materialType, string curStatus, string nextStatus);

        #endregion

        #region Model_Process table operation

        void DeleteModelProcessByProcess(string process);
        void DeleteModelProcessByProcessDefered(IUnitOfWork uow, string process);

        #endregion

        #endregion

        #region ProcessRule
        IList<ProcessRule> GetAllProcessRule();
        IList<int> GetAllRuleSetIDInProcessRule();
        void AddModelProcess(string modelName, string processName, string firstLine, string editor);

        #endregion
    }

}
