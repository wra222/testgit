﻿// created by itc205033
// modify by zhulei(itc211026) 2012-01-12
// modify by zhulei(itc211026) 2012-01-18

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;


namespace IMES.Station.Interface.CommonIntf
{
    /// <summary>
    /// MB_CODE接口
    /// </summary>
    public interface IMB_CODE
    {
        /// <summary>
        /// 获取全部MBCode信息
        /// </summary>
        /// <returns>MBCode信息列表</returns>
        IList<MB_CODEAndMDLInfo> GetMbCodeAndMdlList();

        /// <summary>
        /// 获取全部未打印的MBCode信息
        /// </summary>
        /// <returns>MBCode信息列表</returns>
        IList<MB_CODEAndMDLInfo> GetMBCodeAndMdlListExceptPrinted();

        /// <summary>
        /// 获取全部MBCode信息(返回值为string,而非结构)
        /// </summary>
        /// <returns>MBCode信息(string)</returns>
        IList<string> GetMbCodeList();

        //add by zhulei
        /// <summary>
        /// 获取全部MBCode信息(MB Label Print)
        /// </summary>
        /// <param name="bomNodeType"></param>
        /// <param name="mbType"></param>
        /// <param name="mdlType"></param>
        /// <param name="mdlPostfix"></param>
        /// <returns>MBCode信息列表</returns>
        IList<MB_CODEAndMDLInfo> GetMbCodeAndMdlInfoList(string bomNodeType, string mbType, string mdlType, string mdlPostfix);

        /// <summary>
        /// 获取全部MBCode信息(VGA Label Print)
        /// </summary>
        /// <param name="bomNodeType"></param>
        /// <param name="mbType"></param>
        /// <param name="mdlType"></param>
        /// <param name="mdlPostfix"></param>
        /// <returns>MBCode信息列表</returns>
        IList<MB_CODEAndMDLInfo> GetMbCodeAndMdlInfoList(string bomNodeType, string mbType, string mdlType, string mdlPostfix, string vgaType, string vgaValue);
    }

    /// <summary>
    /// 111 Level接口
    /// </summary>
    public interface I111Level
    {
        /// <summary>
        /// 根据MBCode获取对应的111 Level信息列表
        /// </summary>
        /// <param name="MB_CODEid">MB_CODE id</param>
        /// <returns>111 Level信息列表</returns>
        IList<_111LevelInfo> Get111LevelList(string MB_CODEid);

        /// <summary>
        /// 根据MBCode获取对应的所有未打印的111 Level信息
        /// </summary>
        /// <param name="MB_CODEid">MB_CODE id</param>
        /// <returns>111 Level信息列表</returns>
        IList<_111LevelInfo> Get111LevelListExceptPrinted(string MB_CODEid);

        //add by zhulei
        /// <summary>
        /// 根据MBCode获取对应的111 Level信息列表(MB and VGA)
        /// </summary>
        /// <param name="bomNodeType"></param>
        /// <param name="mbType"></param>
        /// <param name="mbCode"></param>
        /// <returns>111 Level信息列表</returns>
        IList<_111LevelInfo> GetPartNoListByInfo(string bomNodeType, string mbType, string mbCode);

    }

    /// <summary>
    /// PdLine接口
    /// </summary>
    public interface IPdLine
    {
        /// <summary>
        /// 根据station,customer获取对应的PdLine信息列表
        /// </summary>
        /// <param name="stationId">Station Identifier</param>
        /// <param name="customerId">customer</param>
        /// <returns>PdLine信息列表</returns>
        IList<PdLineInfo> GetPdLineList(string stationId, string customerId);

        /// <summary>
        /// 根据customer获取对应的PdLine信息列表
        /// </summary>
        /// <param name="customerId">customer</param>
        /// <returns>PdLine信息列表</returns>
        IList<PdLineInfo> GetPdLineList(string customerId);

        /// <summary>
        /// 根据stage,customer获取对应的PdLine信息列表
        /// </summary>
        /// <param name="stage">Stage</param>
        /// <param name="customerId">customer</param>
        /// <returns>PdLine信息列表</returns>
        IList<PdLineInfo> GetPdLineListByStageAndCustomer(string stage, string customerId);

        /// <summary>
        /// 根据PdLine获取对应的PdLine详细信息
        /// </summary>
        /// <param name="id">PdLineID</param>
        /// <returns>PdLine详细信息</returns>
        PdLineInfo GetPdLine(string id);
    }


    /// <summary>
    /// sample_a interface
    /// </summary>
    public interface ISamplea
    {
        /// <summary>
        /// GetsampleaList
        /// </summary>
        /// <returns></returns>
        IList<OfflineLableSettingDef> GetSampleaList();
    }




    /// <summary>
    /// 打印模板接口
    /// </summary>
    public interface IPrintTemplate
    {
        /// <summary>
        /// 取得所有可用的打印模板
        /// </summary>
        /// <param name="labelType">标签类型</param>
        /// <returns>可用的打印模板列表</returns>
        IList<PrintTemplateInfo> GetPrintTemplateList(string labelType);

        /// <summary>
        /// 取得LabelType的打印模式(Template/Batch: "1" / "0")
        /// </summary>
        /// <param name="labelType">标签类型</param>
        /// <returns>打印模式</returns>
        string GetPrintMode(string labelType);

        /// <summary>
        /// 取得LabelType的Rule模式
        /// </summary>
        /// <param name="labelType">标签类型</param>
        /// <returns>Rule模式</returns>
        string GetRuleMode(string labelType);

        /// <summary>
        /// 获取指定pcode可能打印的pcode
        /// </summary>
        /// <param name="pcode">pcode</param>
        /// <returns>指定pcode可能打印的LabelType</returns>
        IList<string> GetPrintLabelTypeList(string pcode);

        /// <summary>
        ///   select * from SysSetting a, ConstValueType b where a.Name ='RemoteBatPath' and b.Type='RemoteBatPCode' and b.Value =@PCode
        /// </summary>
        /// <param name="pcode"></param>
        /// <returns></returns>
        string GetRemoteBatPath(string pcode);

        /// <summary>
        ///   select * from SysSetting a, ConstValueType b where a.Name ='RemoteBartenderPath' and b.Type='RemoteBartenderPCode' and b.Value =@PCode
        /// </summary>
        /// <param name="pcode"></param>
        /// <returns></returns>
        string GetRemoteBartenderPath(string pcode);
    }

    /// <summary>
    /// SMTMO接口
    /// </summary>
    public interface ISMTMO
    {
        /// <summary>
        /// 根据111阶码取得SMTMO信息列表
        /// </summary>
        /// <param name="_111LevelId">111阶码</param>
        /// <returns>SMTMO信息列表</returns>
        IList<SMTMOInfo> GetSMTMOList(string _111LevelId);

        /// <summary>
        /// 根据SMTMO标识，取得SMTMO信息
        /// </summary>
        /// <param name="SMTMOId">SMTMO标识</param>
        /// <returns>SMTMO信息</returns>
        SMTMOInfo GetSMTMOInfo(string SMTMOId);

        //add by zhulei
        /// <summary>
        /// 根据111阶码取得SMTMO信息列表(MB and VGA)
        /// </summary>
        /// <param name="partNo"></param>
        /// <returns>SMTMO信息列表</returns>
        IList<SMTMOInfo> GetSmtMoListByPno(string partNo);

        /// <summary>
        /// 根据SMTMO标识，取得SMTMO信息(MB and VGA)
        /// </summary>
        /// <param name="SMTMO"></param>
        /// <returns>SMTMO信息</returns>
        SMTMOInfo GetSmtmoInfoList(string SMTMO);

    }

    /// <summary>
    /// MO接口
    /// </summary>
    public interface IMO
    {
        /// <summary>
        /// 根据机器Model号，取得MO信息列表
        /// </summary>
        /// <param name="modelId">机器Model号码</param>
        /// <returns>MO信息列表</returns>
        IList<MOInfo> GetMOList(string modelId);

        /// <summary>
        /// 根据MO标识，取得MO信息
        /// </summary>
        /// <param name="MOId">MO标识</param>
        /// <returns>MO信息</returns>
        MOInfo GetMOInfo(string MOId);
    }

    /// <summary>
    /// 文档类型接口
    /// </summary>
    public interface IDocType
    {
        /// <summary>
        /// 取得文档类型列表
        /// </summary>
        /// <returns>文档类型列表</returns>
        IList<DocTypeInfo> GetDocTypeList();
    }

    /// <summary>
    /// 测试站接口
    /// </summary>
    public interface ITestStation
    {
        /// <summary>
        /// 取得FA测试站信息列表
        /// </summary>
        /// <returns>测试站信息列表</returns>
        IList<TestStationInfo> GetFATestStationList();

        /// <summary>
        /// 取得SA测试站信息列表
        /// </summary>
        /// <returns>测试站信息列表</returns>
        IList<TestStationInfo> GetSATestStationList();
            
        /// <summary>
        /// 根据站号取得站名称
        /// </summary>
        /// <param name="stationId">station id</param>
        /// <returns>站名称</returns>
        string GeStationDescr(string stationId);

        /// <summary>
        /// 根据station type来获取站信息列表
        /// </summary>
        /// <param name="type">type</param>
        /// <returns>站信息列表</returns>
        IList<StationInfo> GetStationListByType(string type);

        IList<StationInfo> GetStationListByLineAndType(string pdline, string type); 

    }

    /// <summary>
    /// Family接口
    /// </summary>
    public interface IFamily
    {   
        /// <summary>
        /// 取得Product的Family信息列表
        /// </summary>
        /// <param name="customerId">customer</param>
        /// <returns>Family信息列表</returns>
  
        IList<FamilyInfo> GetFamilyList(string customerId);


        /// <summary>
        /// 取得Product的Family信息列表,Orderby Family
        /// </summary>
        /// <param name="customer">customer</param>
        /// <returns>Family信息列表</returns>
        IList<FamilyInfo> FindFamiliesByCustomerOrderByFamily(string customer);
       
    }

    /// <summary>
    /// 1397阶接口
    /// </summary>
    public interface I1397Level
    {
        /// <summary>
        /// 取得1397阶信息列表
        /// </summary>
        /// <param name="familyId">Family标识</param>
        /// <returns>1397阶信息列表</returns>
        IList<_1397LevelInfo> Get1397LevelList(string familyId);
    }

    /// <summary>
    /// VGA接口
    /// </summary>
    public interface IVGA
    {
        /// <summary>
        /// 取得VGA列表
        /// </summary>
        /// <returns>VGA信息列表</returns>
        IList<VGAInfo> GetVGAList();
    }

    /// <summary>
    /// FAN接口
    /// </summary>
    public interface IFAN
    {
        /// <summary>
        /// 取得FAN列表
        /// </summary>
        /// <returns>FAN信息列表</returns>
        IList<FANInfo> GetFANList();
    }

    /// <summary>
    /// Defect接口
    /// </summary>
    public interface IDefect
    {
        /// <summary>
        /// 取得Defect列表
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>Defect信息列表</returns>
        IList<DefectInfo> GetDefectList(string type);

        /// <summary>
        /// 取得Defect信息
        /// </summary>
        /// <param name="defectId">Defect标识</param>
        /// <returns>Defect信息</returns>
        DefectInfo GetDefectInfo(string defectId);

        /// <summary>
        /// 根据type,customer获取对应的Defect信息列表
        /// </summary>
        /// <param name="type">type</param>
        /// <param name="customer">customer</param>
        /// <returns>Defect信息列表</returns>
        IList<IMES.DataModel.DefectInfo> GetDefectInfoByTypeAndCustomer(string type, string customer);
    }

    /// <summary>
    /// Cause接口
    /// </summary>
    public interface ICause
    {
        /// <summary>
        /// 取得Cause信息列表
        /// </summary>
        /// <param name="customerId">customer</param>
        /// <param name="stage">customer</param>
        /// <returns>Cause信息列表</returns>
        IList<CauseInfo> GetCauseList(string customerId, string stage);
    }

    /// <summary>
    /// MajorPart接口
    /// </summary>
    public interface IMajorPart
    {
        /// <summary>
        /// 取得MajorPart信息列表
        /// </summary>
        /// <param name="customerId">customer</param>
        /// <returns>MajorPart信息列表</returns>
        IList<MajorPartInfo> GetMajorPartList(string customerId);
    }

    /// <summary>
    /// Component接口
    /// </summary>
    public interface IComponent
    {
        /// <summary>
        /// 取得Component信息列表
        /// </summary>
        /// <param name="customerId">customer</param>
        /// <returns>Component信息列表</returns>
        IList<ComponentInfo> GetComponentList(string customerId);
    }

    /// <summary>
    /// Obligation接口
    /// </summary>
    public interface IObligation
    {
        /// <summary>
        /// 取得Obligation信息列表
        /// </summary>
        /// <param name="customerId">customer</param>
        /// <returns>Obligation信息列表</returns>
        IList<ObligationInfo> GetObligationList(string customerId);
    }

    /// <summary>
    /// Mark接口
    /// </summary>
    public interface IMark
    {
        /// <summary>
        /// 取得Mark信息列表
        /// </summary>
        /// <param name="customerId">customer</param>
        /// <returns>Mark信息列表</returns>
        IList<MarkInfo> GetMarkList(string customerId);
    }

    /// <summary>
    /// Floor接口
    /// </summary>
    public interface IFloor
    {
        /// <summary>
        /// 取得Floor信息列表
        /// </summary>
        /// <returns>Floor信息列表</returns>
        IList<FloorInfo> GetFloorList();
    }

    /// <summary>
    /// PPID类型接口
    /// </summary>
    public interface IPPIDType
    {
        /// <summary>
        /// 取得PPID类型信息列表
        /// </summary>
        /// <returns>PPID类型信息列表</returns>
        IList<PPIDTypeInfo> GetPPIDTypeList();
    }

    /// <summary>
    /// PPID描述接口
    /// </summary>
    public interface IPPIDDescription
    {
        /// <summary>
        /// 取得PPID描述信息列表
        /// </summary>
        /// <param name="PPIDTypeId">PPIDType id</param>
        /// <returns>PPID描述信息列表</returns>
        IList<PPIDDescriptionInfo> GetPPIDDescriptionList(string PPIDTypeId);
    }

    /// <summary>
    /// PartNo接口
    /// </summary>
    public interface IPartNo
    {
        /// <summary>
        /// 取得PartNo信息列表
        /// </summary>
        /// <param name="PPIDTypeId">PPID类型标识</param>
        /// <param name="PPIDDescrptionId">PPID描述标识</param>
        /// <returns>PartNo信息列表</returns>
        IList<PartNoInfo> GetPartNoList(string PPIDTypeId, string PPIDDescrptionId);
    }

    /// <summary>
    /// Sub-Defect接口
    /// </summary>
    public interface ISubDefect
    {
        /// <summary>
        /// 取得Sub-Defect信息列表
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>Sub-Defect信息列表</returns>
        IList<SubDefectInfo> GetSubDefectList(string type);
    }

    /// <summary>
    /// Responsibility接口
    /// </summary>
    public interface IResponsibility
    {
        /// <summary>
        /// 取得Responsibility列表
        /// </summary>
        /// <param name="customer">customer</param>
        /// <returns>Responsibility列表</returns>
        IList<ResponsibilityInfo> GetResponsibilityList(string customerId);
    }

    /// <summary>
    /// 4M接口
    /// </summary>
    public interface I4M
    {
        /// <summary>
        /// 取得4M列表
        /// </summary>
        /// <param name="customer">customer</param>
        /// <returns>4M列表</returns>
        IList<_4MInfo> Get4MList(string customerId);
    }

    /// <summary>
    /// Cover接口
    /// </summary>
    public interface ICover
    {
        /// <summary>
        /// 取得Cover列表
        /// </summary>
        /// <param name="customer">customer</param>
        /// <returns>Cover列表</returns>
        IList<CoverInfo> GetCoverList(string customerId);
    }

    /// <summary>
    /// Uncover接口
    /// </summary>
    public interface IUncover
    {
        /// <summary>
        /// 取得Uncover列表
        /// </summary>
        /// <param name="customer">customer</param>
        /// <returns>取得Uncover列表</returns>
        IList<UncoverInfo> GetUncoverList(string customerId);
    }

    /// <summary>
    /// Tracking Status接口
    /// </summary>
    public interface ITrackingStatus
    {
        /// <summary>
        /// 取得Tracking Status列表
        /// </summary>
        /// <param name="customer">customer</param>
        /// <returns>Tracking Status列表</returns>
        IList<TrackingStatusInfo> GetTrackingStatusList(string customerId);
    }

    /// <summary>
    /// Distribution接口
    /// </summary>
    public interface IDistribution
    {
        /// <summary>
        /// 取得Distribution列表
        /// </summary>
        /// <param name="customer">customer</param>
        /// <returns>Distribution列表</returns>
        IList<DistributionInfo> GetDistributionList(string customerId);
    }

    /// <summary>
    /// Model接口
    /// </summary>
    public interface IModel
    {
        /// <summary>
        /// 取得Model列表
        /// </summary>
        /// <param name="familyId">Family标识</param>
        /// <returns>Model列表</returns>
        IList<ModelInfo> GetModelList(string familyId);

        /// <summary>
        /// 检查用户输入的Model在Model表中是否存在
        /// </summary>
        /// <param name="model">model</param>
        void checkModel(string model);


        /// <summary>
        /// 检查用户输入的Model在是否属于该Family
        /// </summary>
        /// <param name="family">family</param>
        /// <param name="model">model</param>
        void checkModelinFamily(string family, string model);

    }

    
    /// <summary>
    /// ProdIdRange接口
    /// </summary>
    public interface IProdIdRange
    {
        /// <summary>
        /// 取得ProdIdRange列表
        /// </summary>
        /// <param name="MOId">MO标识</param>
        /// <returns>ProdIdRange列表</returns>
        IList<ProdIdRangeInfo> GetProdIdRangeList(string MOId);
    }

    /// <summary>
    /// KP类型接口
    /// </summary>
    public interface IKPType
    {
        /// <summary>
        /// 取得KP类型列表
        /// </summary>
        /// <returns>KP类型列表</returns>
        IList<KPTypeInfo> GetKPTypeList();
    }

    /// <summary>
    /// ChangeKPType接口
    /// </summary>
    public interface IChangeKPType
    {
        /// <summary>
        /// 取得ChangeKPType列表
        /// </summary>
        /// <returns>ChangeKPType列表</returns>
        IList<ConstValueInfo> GetChangeKPTypeList();
    }

    /// <summary>
    /// BOLNo接口
    /// </summary>
    public interface IBOLNo
    {
        /// <summary>
        /// 取得BOLNo列表
        /// </summary>
        /// <returns>BOLNo列表</returns>
        IList<BOLNoInfo> GetBOLNoList();
    }

    /// <summary>
    /// Pallet接口
    /// </summary>
    public interface IPallet
    {
        /// <summary>
        /// 取得Pallet列表
        /// </summary>
        /// <param name="DNId">DN标识</param>
        /// <returns>Pallet列表</returns>
        IList<PalletInfo> GetPalletList(string DNId);
    }


    /// <summary>
    /// Repair接口
    /// </summary>
    public interface IRepair
    {
        /// <summary>
        /// 取得MB Repair列表
        /// </summary>
        /// <param name="MBId">MB标识</param>
        /// <returns>MB Repair列表</returns>
        IList<RepairInfo> GetMBRepairList(string MBId);

        /// <summary>
        /// 取得Product Repair列表
        /// </summary>
        /// <param name="ProdId">Product标识</param>
        /// <returns>Product Repair列表</returns>
        IList<RepairInfo> GetProdRepairList(string ProdId);

        IList<RepairInfo> GetProdRepairList(string ProdId,out bool IsOQCRepair);
    }


    /// <summary>
    /// MB接口
    /// </summary>
    public interface IMB
    {
        /// <summary>
        /// 根据MBSNO取得MB相关信息
        /// </summary>
        /// <param name="MBId">MB SNO</param>
        /// <returns>MB相关信息</returns>
        MBInfo GetMBInfo(string MBId);

                
        /// <summary>
        /// 根据MBSNO取得MB相关信息
        /// </summary>
        /// <param name="MBId">MB SNO</param>
        /// <returns>MB相关信息</returns>
        MBInfo GetMBInfo(string MBId, out int MultiQTY);//Dean 20110329
    }

    /// <summary>
    /// Product接口
    /// </summary>
    public interface IProduct
    {
        /// <summary>
        /// 根据ProdId取得Product相关信息
        /// </summary>
        /// <param name="productId">ProdId</param>
        /// <returns>Product相关信息</returns>
        ProductInfo GetProductInfo(string productId);

        /// <summary>
        /// 根据ProdId取得Product相关信息
        /// </summary>
        /// <param name="customerSn">Customer SN</param>
        /// <returns>Product相关信息</returns>
        ProductInfo GetProductInfoByCustomSn(string customerSn);
 
        /// <summary>
        /// 根据ProdId取得Product Status相关信息
        /// </summary>
        /// <param name="productId">Product id</param>
        /// <returns>Product Status相关信息</returns>
        ProductStatusInfo GetProductStatusInfo(string productId);
    }

    /// <summary>
    /// PartType接口
    /// </summary>
    public interface IPartType
    {
        /// <summary>
        /// 获取PartType列表
        /// </summary>
        /// <returns>获取PartType列表</returns>
        IList<PartTypeInfo> GetPartTypeList();
    }

    /// <summary>
    /// QC统计接口
    /// </summary>
    public interface IQCStatistic
    {
        /// <summary>
        /// 获取QC统计信息列表
        /// </summary>
        /// <param name="pdLine">PdLine</param>
        /// <returns>QC统计信息列表</returns>
        IList<QCStatisticInfo> GetQCStatisticList(string pdLine, string type);
    }

    /// <summary>
    /// Session接口(直接Session操作，仅在异常情况下使用)
    /// </summary>
    public interface ISession
    {
        /// <summary>
        /// 强制结束一个指定Session
        /// </summary>
        /// <param name="sessionKey">MBSN或ProdId等</param>
        /// <param name="type">type包括MB,Product,Common</param>
        void TerminateSession(string sessionKey, IMES.DataModel.SessionType type);

        /// <summary>
        /// 根据type获取Session信息
        /// </summary>
        /// <param name="type">type</param>
        /// <returns>Session信息</returns>
        IList<SessionInfo> GetSessionByType(SessionType type);

        /// <summary>
        /// 获取全部站信息列表
        /// </summary>
        /// <returns>站信息列表</returns>
        IList<StationInfo> GetAllStationInfo();

       

    }

    /// <summary>
    /// DCode接口
    /// </summary>
    public interface IDCode
    {
        /// <summary>
        /// 取得DCode信息列表
        /// </summary>
        /// <returns>DCode信息列表</returns>
        //IList<DCodeInfo> GetDCodeList();

        /// <summary>
        /// 获取DCode信息列表
        /// </summary>
        /// <param name="isFRU">是否为FRU</param>
        /// <returns></returns>
        IList<DCodeInfo> GetDCodeRuleListForMB(bool isFRU, string customer);

        /// <summary>
        /// 获取DCode信息列表
        /// </summary>
        /// <returns></returns>
        IList<DCodeInfo> GetDCodeRuleListForMB(string customer);

        /// <summary>
        /// 获取DCode信息列表
        /// </summary>
        /// <returns></returns>
        IList<DCodeInfo> GetDCodeRuleListForVB(string customer);

        /// <summary>
        /// 获取DCode信息列表
        /// </summary>
        /// <returns></returns>
        IList<DCodeInfo> GetDCodeRuleListForKP(string customer);

        /// <summary>
        /// 获取DCode信息列表
        /// </summary>
        /// <returns></returns>
        IList<DCodeInfo> GetDCodeRuleListForDK(string customer);

    }

    /// <summary>
    /// 打印模板接口
    /// </summary>
    public interface IPrintItem
    {
        /// <summary>
        /// bat打印时,根据存储过程名字和参数执行存储过程,获取主Bat
        /// </summary>
        /// <param name="currentSPName">存储过程名字</param>
        /// <param name="parameterKeys">存储过程需要的参数名字</param>
        /// <param name="parameterValues">存储过程需要的参数值</param>
        /// <returns>主bat名称</returns>
        string GetMainBat(string currentSPName, List<string> parameterKeys, List<List<string>> parameterValues);
		
		/// <summary>
        /// Bartender打印时,根据存储过程名字和参数执行存储过程,获取主Bartender Name/Value
        /// </summary>
        /// <param name="currentSPName">存储过程名字</param>
        /// <param name="parameterKeys">存储过程需要的参数名字</param>
        /// <param name="parameterValues">存储过程需要的参数值</param>
        /// <returns>主bat名称</returns>
        IList<NameValueDataTypeInfo> GetBartenderNameValueList(string currentSPName, List<string> parameterKeys, List<List<string>> parameterValues);

    }

    /// <summary>
    /// Shipment接口
    /// </summary>
    public interface IShipment
    {
        /// <summary>
        /// 获取Shipment信息列表
        /// </summary>
        /// <param name="shipDate">Shipment date</param>
        /// <returns>Shipment信息列</returns>
        IList<ShipmentInfo> GetShipmentList(DateTime shipDate);
    }

    /// <summary>
    /// Cache接口
    /// </summary>
    public interface ICache
    {
        /// <summary>
        /// 刷新所有Cache
        /// </summary>
        void RefreshAllCache();

        /// <summary>
        /// 更新所有Cache
        /// </summary>
        void UpdateCacheNow();

        /// <summary>
        /// 更新PartMatch dll load into MEF cache 
        /// </summary>
        void RefreshPartMatchAssembly();

        /// <summary>
        /// 刪除Cache Item by Key
        /// </summary>
        /// <param name="cacheType">Bom, Part, Model, Family</param>
        /// <param name="key"></param>
        void RemoveCacheItem(string cacheType, string key);
        /// <summary>
        /// 刪除Cache Item by KeyList
        /// </summary>
        /// <param name="cacheType">Bom, Part, Model, Family</param>
        /// <param name="keyList"></param>
        void RemoveCacheItem(string cacheType, string[] keyList);
    }

    /// <summary>
    /// KittingCode接口
    /// </summary>
    public interface IKittingCode
    {
        /// <summary>
        /// 获取全部KittingCode信息列表
        /// </summary>
        /// <returns>KittingCode信息列表</returns>
        IList<KittingCodeInfo> GetKittingCodeList();
    }

    /// <summary>
    /// LightCode接口
    /// </summary>
    public interface ILightCode
    {
        /// <summary>
        /// 获取全部LightCode列表
        /// </summary>
        /// <returns>LightCode列表</returns>
        IList<string> GetLightCodeList();
    }

    ///<summary>
    /// Data migration from imes to old fis
    ///</summary>
    public interface  IDataMigration
    {
        ///<summary>
        /// Data migration from imes to old fis
        ///</summary>
        ///<param name="keyid">ProductID， CartonID，PalletID</param>
        ///<param name="pcode">UI PCode</param>
        ///<param name="no1">reserved parameter</param>
        ///<param name="no2">reserved parameter</param>
        void ImesToFis(string keyid, string pcode, string no1, string no2);
    }

    /// add by Dorothy 2011-12-16 
    /// <summary>
    /// ConstValue接口
    /// </summary>
    public interface IConstValue
    {
        /// <summary>
        /// 根据type从ConstValue获取对应的信息列表
        /// </summary>
        /// <param name="type"></param>
        /// <returns>信息列表</returns>
        IList<ConstValueInfo> GetConstValueListByType(string type,string orderby);

    }
    /// add by Dorothy 2012-1-6 
    /// <summary>
    /// LabelKittingCode接口
    /// </summary>
    public interface ILabelKittingCode
    {
        /// <summary>
        /// 根据type获取对应的LabelKittingCode信息列表
        /// </summary>
        /// <param name="type"></param>
        /// <returns>PalletType信息列表</returns>
        IList<LabelKittingCode> GetLabelKittingCodeList(string type);

    }
    
    public interface IStationByType
    {
        /// <summary>
        /// 根据type获取对应的Station信息列表按照Descr排序
        /// </summary>
        /// <param name="type"></param>
        /// <returns>StationType信息列表</returns>
        IList<StationInfo> GetStationByTypeList(string type);
    }
    public interface IDeliveryByModel
    {
        /// <summary>
        /// 根据model获取对应的delivery信息列表按照ShipDate排序
        /// </summary>
        /// <param name="type"></param>
        /// <returns>Delivery信息列表</returns>
        IList<DNForUI> GetDeliveryListByModel(string model, string status);
    }

    public interface IDismantleType{
      /// <summary>
        /// 取得ConstValue列表
        /// </summary>
        /// <returns>ConstValue列表</returns>
        IList<ConstValueInfo> GetDismantleTypeList(string type);
    }

    //add by zhulei
    public interface IQty
    {
        /// <summary>
        /// Get Qty
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        int GetQty(string line);

        /// <summary>
        /// Clear Qty
        /// </summary>
        /// <param name="line"></param>
        /// <param name="qty"></param>
        void ClearQty(string line, string qty);

    }

    /// <summary>
    /// CauseItem接口
    /// </summary>
    public interface ICauseItem
    {
        /// <summary>
        /// 取得Cause Item WHERE Type='COAStatus' 
        /// </summary>
        /// <param name="type">type</param>
        /// <returns>Cause Item</returns>
        IList<string> GetCauseItemByType(string type);
    }

    public interface IPartQuery
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="models"></param>
        /// <param name="stations"></param>
        /// <returns></returns>
        IList<PartQty> QueryPartByModel(IList<ModelQty> models, IList<string> stations);
    }

    public interface IBOMQuery
    {
        IList<StationInfo> GetAllPartCollectionStation();
        IDictionary<string, IList<IMES.DataModel.BomItemInfo>> GetBOM(string customer, string model, string line);
    }

    public interface IExecSP
    {
        System.Data.DataTable GetSPResult(string editor,string dbName, string spName, string[] ParameterNameArray, string[] ParameterValueArray);
        System.Data.DataSet GetSQLResult(string editor,string dbName, string sqlText, string[] ParameterNameArray, string[] ParameterValueArray);
    }

    public interface IDeliveryByCarton
    {
        /// <summary>
        /// Get Delivery by Carton
        /// </summary>
        /// <param name="type"></param>
        /// <returns>Delivery信息列表</returns>
        IList<DNForUI> GetDeliveryListByCarton(string model, string status);
    }


}

