using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.DataModel;
using IMES.Infrastructure.UnitOfWork;
//
namespace IMES.FisObject.Common.Defect
{
    public interface IDefectRepository : IRepository<Defect>
    {
        #region . For CommonIntf  .

        /// <summary>
        /// 根据type获得DefectCode列表
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        IList<IMES.DataModel.DefectInfo> GetDefectList(string type);

        /// <summary>
        /// 根据defectId获得DefectCode
        /// </summary>
        /// <param name="defectId"></param>
        /// <returns></returns>
        IMES.DataModel.DefectInfo GetDefectInfo(string defectId);

        /// <summary>
        /// 获得SubDefectCode列表
        /// </summary>
        /// <returns></returns>
        IList<IMES.DataModel.SubDefectInfo> GetSubDefectList();

        IList<IMES.DataModel.SubDefectInfo> GetSubDefectList(string type);

        #endregion

        /// <summary>
        /// 获得DefectCode的描述信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        string TransToDesc(string code);

        #region For Maintain
       
        /// <summary>
        /// 参数为字符串类型，对应DefectCode表中的Type字段。
        /// SQL语句：
        /// SELECT * 
        /// FROM IMES_GetData..DefectCode
        ///          WHERE Type = @Type
        ///          ORDER BY Type, Defect
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        IList<DefectCodeInfo> GetDefectCodeList(string type);

        /// <summary>
        /// 返回值为字符串类型，对应DefectCode表中的主键Defect
        /// 参数为两个字符串类型，分别对应数据库DefectCode表中的type ,defect
        /// SQL语句：
        /// SELECT Defect
        /// FROM IMES_GetData..DefectCode
        /// WHERE Type = @Type AND Defect = @Defect
        /// </summary>
        /// <param name="type"></param>
        /// <param name="defect"></param>
        /// <returns></returns>
        string GetDefect(string type, string defect);

        /// <summary>
        /// 参数为两个字符串类型，分别数据库DefectCode表中的Type和Defect字段
        /// SQL语句：
        /// DELETE FROM IMES_GetData..DefectCode WHERE Type = @Type AND Defect = @Defect
        /// </summary>
        /// <param name="type"></param>
        /// <param name="defect"></param>
        void DeleteDefectCode(string type, string defect);

        /// <summary>
        /// 返回值和参数说明：
        /// 无返回值；
        /// 参数为IMES.DataModel.DefectCodeInfo结构
        /// SQL语句：
        /// IF EXISTS(SELECT * FROM IMES_GetData..DefectCode WHERE Defect = @Defect)
        ///          UPDATE IMES_GetData..DefectCode SET Descr = @Descr, EngDescr=@EngDescr, Editor = @Editor, Udt = GETDATE()
        ///                    WHERE Defect = @Defect
        /// </summary>
        /// <param name="dfc"></param>
        void UpdateDefectCode(DefectCodeInfo dfc);

        /// <summary>
        /// 返回值和参数说明：
        /// 无返回值；
        /// 参数为IMES.DataModel.DefectCodeInfo结构
        /// SQsL语句：
        /// INSERT IMES_GetData..DefectCode (Defect, Type, Descr, Editor, Cdt, Udt)
        ///                    VALUES(@Defect, @Type, @Descr, @Editor, GETDATE(), GETDATE())
        /// </summary>
        /// <param name="dfc"></param>
        void InsertDefectCode(DefectCodeInfo dfc);

        /// <summary>
        /// 返回值和参数说明：
        /// 返回值为代表记录条数的Int值
        /// 参数为字符串类型，对应数据库DefectCode表中的[Defect]字段
        /// SQL语句：
        /// SELECT COUNT(*)  FROM [IMES_GetData_Datamaintain].[dbo].[DefectCode]
        ///   WHERE  [Defect]=@Defect
        /// </summary>
        /// <param name="Defect"></param>
        /// <returns></returns>
        int CheckExistsRecord(string Defect);

        /// <summary>
        /// 返回值和参数说明:
        /// 返回值为IMES.DataMoedl.DefectInfo的列表,无参数;
        /// SQL语句:
        /// SELECT * 
        /// FROM IMES_GetData..DefectCode
        /// ORDER BY Type, Defect
        /// </summary>
        /// <returns></returns>
        IList<DefectCodeInfo> GetDefectCodeList();

        /// <summary>
        /// 1.取得所输CTNo相关的不良记录信息
        /// select a.Line,b.Defect,b.Cause,b.Cdt,b.Udt
        /// from KeyPartRepair a(nolock), KeyPartRepair_DefectInfo b(nolock) 
        /// where a.ID=b.ProductRepairID and b.VendorCT=left(@sn,14)
        /// union
        /// select '',MpDefect,'',Udt,Udt from IqcCause1(nolock) where CtLabel=@sn
        /// </summary>
        /// <param name="sn"></param>
        /// <returns></returns>
        IList<SQEDefectCTNoInfo> GetSQEDefectCTNoInfo(string sn);

        /// <summary>
        /// 2.取得partType的方法
        /// select left(rtrim(b.Descr),3) as Descr from KeyPartRepair_DefectInfo a(nolock), Part b(nolock),ModelBom c(nolock) 
        /// where left(a.VendorCT,5)=c.Component and a.VendorCT=left(@sn,14) and c.Material=b.PartNo
        /// union
        /// select left(rtrim(b.Descr),3) as Descr from IqcCause1 a(nolock), Part b(nolock),ModelBom c(nolock) where left
        /// (a.CtLabel,5)=c.Component and a.CtLabel=@sn and c.Material=b.PartNo
        /// </summary>
        /// <param name="sn"></param>
        /// <returns></returns>
        IList<string> GetPartTypeSQEDefectReport(string sn);

        /// <summary>
        /// 3.获取ProductRepair_DefectInfo 数据
        /// select
        /// DefectCode,Cause,Obligation,Component,MajorPart,Remark,VendorCT,
        /// Editor,Cdt,Udt
        /// where VendorCT=@sn
        /// </summary>
        /// <param name="sn"></param>
        /// <returns></returns>
        IList<SQEDefectProductRepairReportInfo> GetSQEDefectProductRepairInfo(string sn);

        /// <summary>
        /// 4.添加一条IqcKp数据
        /// </summary>
        /// <param name="iqcKp"></param>
        void AddIqcKp(IqcKpDef iqcKp);

        /// <summary>
        /// 5.查找IqcKp数据，返回其list
        /// select * from IqcKp 
        /// where tp=@tp and CtLabel=@CtLabel and Defect=@Defect
        /// </summary>
        /// <param name="tp"></param>
        /// <param name="ctLabel"></param>
        /// <param name="defect"></param>
        /// <returns></returns>
        IList<IqcKpDef> GetIqcKpByTypeCtLabelAndDefect(string tp, string ctLabel, string defect);

        /// <summary>
        /// 6.查找IqcKp数据，返回其list
        /// select * from IqcKp 
        /// where tp=@tp and CtLabel=@CtLabel and Defect=@Defect and Cause = @Cause
        /// </summary>
        /// <param name="tp"></param>
        /// <param name="ctLabel"></param>
        /// <param name="defect"></param>
        /// <param name="cause"></param>
        /// <returns></returns>
        IList<IqcKpDef> GetIqcKpByTypeCtLabelAndDefect(string tp, string ctLabel, string defect, string cause);

        /// <summary>
        /// 7.更新IqcKp数据；
        /// UPDATE IMES2012_FA.dbo.IqcKp
        /// SET CtLabel = @CtLabel
        /// ,Model = @Model
        /// ,Tp = @Tp
        /// ,Defect = @Defect
        /// ,Cause = @Cause,
        /// ,Location = @Location
        /// ,Obligation = @Obligation
        /// ,Remark = @Remark
        /// ,Result = @Result
        /// ,Editor = @Editor
        /// ,Cdt = @Cdt
        /// ,Udt = @Udt
        /// WHERE CtLabel = CtLabel and Tp = tp and Defect = defect
        /// 
        /// （setValue哪个字段赋值就有更新哪个字段;condition哪个字段赋值就有哪个条件,自由使用各个条件,条件间是AND关系.）
        /// </summary>
        /// <param name="setValue"></param>
        /// <param name="condition"></param>
        void UpdateIqcKp(IqcKpDef setValue, IqcKpDef condition);

        /// <summary>
        /// 8.根据type获取Defect数据
        /// Select * from DefectCode where Type=@type order by Defect
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        IList<DefectCodeInfo> GetDefecSQEDefectInfo(string type);

        /// <summary>
        /// 1.获取defectCode的列表
        /// select distinct Defect from DefectCode order by Defect
        /// </summary>
        /// <returns></returns>
        IList<string> GetDefectCode();

        /// <summary>
        /// 3.获取defect列表
        /// select * from DefectCode order by Defect
        /// </summary>
        /// <returns></returns>
        IList<DefectCodeInfo> GetDefectCodeLst();

        /// <summary>
        /// 4.根据defect删除defect数据
        /// delete from defectCode where Defect = @defect
        /// </summary>
        /// <param name="defect"></param>
        void DeleteDefectCode(string defect);

        /// <summary>
        /// select * from IqcCause1(nolock) where CtLabel=<输入ctLabel> and MpDefect=<输入mpDefect>
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        bool CheckIfIqcCauseExist(IqcCause1Info condition);

        /// <summary>
        /// update IqcCause1 set Udt = GETDATE() where CtLabel=<输入ctLabel> and MpDefect=<输入mpDefect>
        /// </summary>
        /// <param name="setValue"></param>
        /// <param name="condition"></param>
        void UpdateUDTofIqcCause(IqcCause1Info setValue, IqcCause1Info condition);

        /// <summary>
        /// insert into IqcCause1(CtLabel,MpDefect,Udt) values(<输入ctLabel>,<输入mpDefect>,GETDATE())
        /// </summary>
        /// <param name="item"></param>
        void AddIqcCause(IqcCause1Info item);

        IList<DefectCodeInfo> GetDefectCodeInfoList(DefectCodeInfo condition);

        /// <summary>
        /// select * from IqcCause1 where CtLabel = '<ctno>'
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        IList<IqcCause1Info> GetIqcCause1InfoList(IqcCause1Info condition);

        IList<IqcCause1Info> GetIqcCause1InfoList(IqcCause1Info eqCondition, IqcCause1Info neqCondition);

        bool CheckIfIqcCauseExist(IqcCause1Info eqCondition, IqcCause1Info neqCondition);

        #region Defered

        void DeleteDefectCodeDefered(IUnitOfWork uow, string type, string defect);

        void UpdateDefectCodeDefered(IUnitOfWork uow, DefectCodeInfo dfc);

        void InsertDefectCodeDefered(IUnitOfWork uow, DefectCodeInfo dfc);

        void AddIqcKpDefered(IUnitOfWork uow, IqcKpDef iqcKp);

        void UpdateIqcKpDefered(IUnitOfWork uow, IqcKpDef setValue, IqcKpDef condition);

        void DeleteDefectCodeDefered(IUnitOfWork uow, string defect);

        void UpdateUDTofIqcCauseDefered(IUnitOfWork uow, IqcCause1Info setValue, IqcCause1Info condition);

        void AddIqcCauseDefered(IUnitOfWork uow, IqcCause1Info item);

        #endregion

        #endregion
    }
}
