using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectRepositoryFramework;
//
using IMES.DataModel;
using IMES.Infrastructure.UnitOfWork;

namespace IMES.FisObject.PCA.MBModel
{
    ///<summary>
    ///</summary>
    public interface IMBModelRepository : IRepository<IMBModel>
    {
        /// <summary>
        /// 取得同一主板代码的所有主板型号对象
        /// </summary>
        /// <param name="mbCode">主板代码</param>
        /// <returns>主板型号对象集合</returns>
        IList<IMBModel> GetMBModelByMBCode(string mbCode);
        
        /// <summary>
        /// 根据part no获得part表中的part type
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        string getTypeByModel(string model);

        /// <summary>
        /// Get [Cust Version]/[IEC Version] by ECR, MB_Code, then Display (WHERE MBCode = @MBCode AND RIGHT(ECR, 2) = RIGHT(@UIEcr, 2))
        /// </summary>
        /// <param name="ecr"></param>
        /// <param name="mbcode"></param>
        /// <returns></returns>
        IList<EcrVersionInfo> getEcrVersionsByEcrAndMbcode(string ecr, string mbcode);

        /// <summary>
        /// 从EcrVersion中获得MBCode列表
        /// </summary>
        /// <returns></returns>
        IList<string> GetMBCodeListFromEcrVersion();

        /// <summary>
        /// 判断EcrVersion是否存在
        /// </summary>
        /// <param name="ecr"></param>
        /// <returns></returns>
        bool IsEcrExistInEcrVersion(string ecr);

        /// <summary>
        /// Lazy load of MBCode of MBModel
        /// </summary>
        /// <param name="mbModel"></param>
        /// <returns></returns>
        MBModel FillMBCodeObj(MBModel mbModel);

        /// <summary>
        /// insert into SMTCT
        /// </summary>
        /// <param name="item"></param>
        void AddSmtctInfo(SmtctInfo item);

        /// <summary>
        /// delete  SMTCT
        /// </summary>
        /// <param name="condition"></param>
        void DeleteSmtctInfo(SmtctInfo condition);

        /// <summary>
        /// update SMTCT
        /// </summary>
        /// <param name="setValue"></param>
        /// <param name="condition"></param>
        void UpdateSmtctInfo(SmtctInfo setValue, SmtctInfo condition);

        /// <summary>
        /// select * from SMTCT  order by Family
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        IList<SmtctInfo> GetSmtctInfoList(SmtctInfo condition);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mbModel"></param>
        /// <returns></returns>
        IMES.FisObject.Common.Model.Family FillFamilyObj(MBModel mbModel);

        #region . Defered .

        void AddSmtctInfoDefered(IUnitOfWork uow, SmtctInfo item);

        void DeleteSmtctInfoDefered(IUnitOfWork uow, SmtctInfo condition);

        void UpdateSmtctInfoDefered(IUnitOfWork uow, SmtctInfo setValue, SmtctInfo condition);

        #endregion

        #region For Maintain

        /// <summary>
        /// 获得MBFamily列表
        /// </summary>
        /// <returns></returns>
        IList<string> GetMBFamilyList();

        /// <summary>
        /// 获得MBModel列表
        /// </summary>
        /// <returns></returns>
        IList<string> GetMBModelList();

        /// <summary>
        /// 1.获取所有Unite MB记录
        /// select * from MBCode
        /// </summary>
        /// <returns></returns>
        IList<MBCode> GetAllUniteMB();

        /// <summary>
        /// 2.根据MBCode获取Unite MB记录
        /// </summary>
        /// <param name="MBCode"></param>
        /// <returns></returns>
        IList<MBCode> GetLstByMB(string mbCode);

        /// <summary>
        /// 3.添加一条Unite MB记录
        /// </summary>
        /// <param name="obj"></param>
        void AddUniteMB(MBCode obj);

        /// <summary>
        /// 4.删除一条Unite MB记录
        /// </summary>
        /// <param name="mbCode"></param>
        void DeleteUniteMB(string mbCode);

        /// <summary>
        /// 5.修改一条Unite MB记录
        /// </summary>
        /// <param name="obj"></param>
        void UpdateUniteMB(MBCode obj, string mbCode);

        #region . Defered .

        void AddUniteMBDefered(IUnitOfWork uow, MBCode obj);

        void DeleteUniteMBDefered(IUnitOfWork uow, string mbCode);

        void UpdateUniteMBDefered(IUnitOfWork uow, MBCode obj, string mbCode);

        #endregion

        #endregion

        #region . For CommonIntf  .

        /// <summary>
        /// 取得主板代码集合
        /// </summary>
        /// <returns>主板代码集合</returns>
        IList<MB_CODEInfo> GetMBCodeList();

        /// <summary>
        /// 取得主板代码和MDL組合集合
        /// </summary>
        /// <returns></returns>
        IList<MB_CODEAndMDLInfo> GetMBCodeAndMdlList();

        /// <summary>
        /// 取得主板代码和MDL組合集合,不算打印完毕的
        /// </summary>
        /// <returns></returns>
        IList<MB_CODEAndMDLInfo> GetMBCodeAndMdlListExceptPrinted();

        /// <summary>
        /// Get 111 Level List from database
        /// </summary>
        /// <param name="mbCodeId">MB_CODE Identifier</param>
        /// <returns>111 Level Info List</returns>
        IList<_111LevelInfo> Get111LevelList(string mbCodeId);

        /// <summary>
        /// Get 111 Level List from database, not including printed
        /// </summary>
        /// <param name="mbCodeId"></param>
        /// <returns></returns>
        IList<_111LevelInfo> Get111LevelListExceptPrinted(string mbCodeId);

        /// <summary>
        /// 取得1397阶信息列表
        /// </summary>
        /// <param name="familyId">Family标识</param>
        /// <returns>1397阶信息列表</returns>
        IList<_1397LevelInfo> Get1397LevelList(string familyId);

        #endregion
    }
}
