// created by itc205033

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.FisObject.Common.Model;
using IMES.DataModel;
using IMES.FisObject.Common.Part;
//using IMES.Station.Interface.CommonIntf;

namespace IMES.Maintain.Interface.MaintainIntf
{
    /// <summary>
    /// [Model] 实现如下功能：
    /// 使用此界面來维护Model资料.
    /// </summary>
    public interface IProcessManager
    {

        string ProcessSaveAs(ProcessMaintainInfo processInfo, string oldProcess);

        /// <summary>
        /// 取得全部Process List
        /// </summary>
        /// <returns>Process List</returns>
        IList<ProcessMaintainInfo> getProcessList();

        /// <summary>
        /// 取process表中等于process的纪录，支持like '%Process%'
        /// </summary>
        /// <param name="Process">Process</param>
        /// <returns>ProcessMaintainInfo List</returns>
        IList<ProcessMaintainInfo> getProcessList(string process);
        
        /// <summary>
        /// 取model_process表中等于model的纪录，支持like '%model%'
        /// </summary>
        /// <param name="Model">model</param>
        /// <returns>ModelProcessMaintainInfo List</returns>
        IList<ModelProcessMaintainInfo> getModelProcessListByModel(string model);

        /// <summary>
        /// 取palletprocess表中等于customer的纪录，支持like '%customer%'
        /// </summary>
        /// <param name="Customer">customer</param>
        /// <returns>PalletProcessMaintainInfo list</returns>
        IList<PalletProcessMaintainInfo> getPalletProcessListByCustomer(string customer);

        /// <summary>
        /// PCB, 取partprocess表中等于mbFaily的纪录，支持like '%mbFamily%'
        /// </summary>
        /// <param name="ModelId">mbFamily</param>
        /// <param name="Object">PartProcessMaintainInfo list</param>
        IList<PartProcessMaintainInfo> getPartProcessListByMBFamily(string mbFamily);

        /// <summary>
        /// 取rework_process表中等于reworkCode的纪录，支持like '%reworkCode%'
        /// </summary>
        /// <param name="ModelId">reworkCode</param>
        /// <param name="Object">ReworkProcessMaintainInfo list</param>
        IList<ReworkProcessMaintainInfo> getReworkProcessListByReworkCode(string reworkCode);

        /// <summary>
        /// 取palletprocess表中等于process的纪录
        /// </summary>
        /// <param name="ModelId">process</param>
        /// <param name="Object">PalletProcessMaintainInfo list</param>
        IList<PalletProcessMaintainInfo> getPalletProcessListByProcess(string process);

        /// <summary>
        /// 取partprocess表中等于process的纪录
        /// </summary>
        /// <param name="ModelId">process</param>
        /// <param name="Object">PartProcessMaintainInfo list</param>
        IList<PartProcessMaintainInfo> getPCBProcessListByProcess(string process);

        /// <summary>
        /// 取rework_process表中等于process的纪录
        /// </summary>
        /// <param name="ModelId">process</param>
        /// <param name="Object">ReworkProcessMaintainInfo list</param>
        IList<ReworkProcessMaintainInfo> getReworkProcessListByProcess(string process);

        /// <summary>
        /// 取rework_releasetype表中等于process的纪录
        /// </summary>
        /// <param name="ModelId">process</param>
        /// <param name="Object">ReworkReleaseTypeMaintainInfo list</param>
        IList<ReworkReleaseTypeMaintainInfo> getReworkReleaseTypeListByProcess(string process);

        /// <summary>
        /// 取palletprocess表中所有纪录
        /// </summary>
        /// <param name="Object">PalletProcessMaintainInfo list</param>
        IList<PalletProcessMaintainInfo> getPalletProcessList();

        /// <summary>
        /// 取customer, PCB Family, PCB Model表中所有纪录的集合
        /// </summary>
        /// <param name=""></param>
        /// <param name="Object">customer, PCB Family, PCB Model list</param>
        IList<PartProcessMaintainInfo> getPCBProcessList();

        /// <summary>
        /// BWEIGHT、CN、COA、CPQSNO、Delivery、KIT ID、MB、MMI、PCMAC、PLT、WEIGHT和PartCheck和CheckItem中所有type(Distinct)
        /// </summary>
        /// <param name=""></param>
        /// <param name="Object">BWEIGHT、CN、COA、CPQSNO、Delivery、KIT ID、MB、MMI、PCMAC、PLT、WEIGHT和PartCheck和CheckItem中所有type(Distinct) list</param>
        IList<ReworkProcessMaintainInfo> getReworkProcessList();

        /// <summary>
        /// BWEIGHT、CN、COA、CPQSNO、Delivery、KIT ID、MB、MMI、PCMAC、PLT、WEIGHT和PartCheck和CheckItem中所有type(Distinct)
        /// </summary>
        /// <param name=""></param>
        /// <param name="Object">BWEIGHT、CN、COA、CPQSNO、Delivery、KIT ID、MB、MMI、PCMAC、PLT、WEIGHT和PartCheck和CheckItem中所有type(Distinct) list</param>
        IList<ReworkReleaseTypeMaintainInfo> getReworkReleaseTypeList();


        /// <summary>
        /// 从process取得一条记录
        /// </summary>
        /// <param name="ModelId">process</param>
        /// <param name="Object">ProcessMaintainInfo</param>
        ProcessMaintainInfo getProcess(string process);


        /// <summary>
        /// 向process新增一条记录
        /// </summary>
        /// <param name="MB_SNo">Object</param>
        void addProcess(ProcessMaintainInfo Object);

        /// <summary>
        /// 向process保存一条记录
        /// </summary>
        /// <param name="MB_SNo">Object</param>
        void saveProcess(string strOldProcessName, ProcessMaintainInfo Object);                 


        /// <summary>
        /// 删除processs表中等于process的记录
        /// </summary>
        /// <param name="process">process</param>
        void deleteProcess(string process);                  


        /// <summary>
        /// 取得processstation表中等于process的记录。
        /// </summary>
        /// <param name="FamilyId">process</param>
        /// <returns>ProcessStationMaintainInfo List</returns>
        IList<ProcessStationMaintainInfo> getProcessStationList(string process);


        /// <summary>
        /// 从processstation表中取得一条记录。
        /// </summary>
        /// <param name="FamilyId">id</param>
        /// <returns>ProcessStationMaintainInfo</returns>
        ProcessStationMaintainInfo getProcessStation(int id);
        
        /// <summary>
        /// 删除processstation表中等于id的记录
        /// </summary>
        /// <param name="FamilyId">id</param>
        /// <returns></returns>
        void deleteProcessStation(int id);



        /// <summary>
        /// 新增一条ProcessStation的记录数据,在processstation表中保存一条记录。
        /// </summary>
        /// <param name="FamilyId">Object</param>
        /// <returns></returns>
        int saveProcessStation(ProcessStationMaintainInfo Object);          



        /// <summary>
        /// 取station表中所有纪录
        /// </summary>
        /// <param name="FamilyId"></param>
        /// <returns>StationMaintainInfo list</returns>
        IList<StationMaintainInfo> getStationList();


        /// <summary>
        /// 向partprocess新增一条记录
        /// 过程包括：先删除等于process的所有记录，然后新增所有最新check的mbFamily
        /// </summary>
        /// <param name="process">process</param>
        /// <param name="mbFamily">mbFamily</param>
        /// <returns></returns>
        void addPartProcesses(IList<PartProcessMaintainInfo> arrCheckedMBFamily, PartProcessMaintainInfo partProcessInfo); 


        /// <summary>
        /// 向reworkprocess新增一条记录,过程包括：先删除等于process的所有记录，然后新增所有最新check的reworkCode
        /// </summary>
        /// <param name="FamilyId">reworkCode</param>
        /// <param name="FamilyId">process</param>
        /// <returns></returns>
        void addReworkProcesses(IList<string> arrCheckedReworkCode, ReworkProcessMaintainInfo reworkProcessInfo);

        /// <summary>
        /// 向reworkReleaseType新增一条记录,过程包括：先删除等于process的所有记录，然后新增所有最新check的reworkReleaseType
        /// </summary>
        /// <param name="FamilyId">reworkCode</param>
        /// <param name="FamilyId">process</param>
        /// <returns></returns>
        void addReworkReleaseType(IList<string> arrCheckedReworkReleaseType, ReworkReleaseTypeMaintainInfo reworkReleaseTypeProcessInfo);

        /// <summary>
        /// 向palletprocess新增一条记录，过程包括：先删除等于process的所有记录，然后新增所有最新check的customer
        /// </summary>
        /// <param name="FamilyId">customer</param>
        /// <param name="FamilyId">process</param>
        /// <returns></returns>
        void addPalletProcesses(IList<string> arrCheckedCustomer, PalletProcessMaintainInfo palletProcessInfo);

        /// <summary>
        /// 取rework表中所有纪录
        /// </summary>
        /// <param name="Object">Rework list</param>
        IList<ReworkMaintainInfo> getReworkList();

        /// <summary>
        /// 取customer表中所有纪录
        /// </summary>
        /// <param name="Object">CustomerInfo list</param>
        IList<CustomerMaintainInfo> GetMyCustomerList();

        /// <summary>
        /// 取MBFamily表中所有纪录
        /// </summary>
        /// <param name="Object">string list</param>
        IList<string> getMBFamilyAndMBModelList();
        /// <summary>
        /// 按照Name, Model, Pallet, Rework, PCB查询process list, 如果都为空，则取process表全部process list
        /// </summary>
        /// <param name="FamilyId"></param>
        /// <returns>process list</returns>
        ///List<StationMaintainInfo> query();
        ///

        ///导入
        string UploadProcess(ProcessMaintainInfo processInfo, List<ProcessStationMaintainInfo> processStationList);

        //导出
        ProcessInfoDef ExportProcess(string process);

        IList<ConstValueTypeInfo> getallProcessbyMaterial(string type);

        IList<MaterialProcess> GetMaterialProcessByProcess(string process);

        void AddMaterialProcess(string materialType, string process, string editor);

        void RemoveMaterialProcessByType(string materialType);

    }

}
