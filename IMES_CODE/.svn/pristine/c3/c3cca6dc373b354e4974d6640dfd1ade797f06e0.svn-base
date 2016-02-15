using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.UnitOfWork;

namespace IMES.FisObject.Common.Part
{
    public interface IAssemblyCodeRepository : IRepository<AssemblyCode>
    {
        /// <summary>
        /// 根据AssemblyCode查找AssemblyCode对象列表
        /// </summary>
        /// <param name="assCode"></param>
        /// <returns></returns>
        IList<AssemblyCode> FindAssemblyCode(string assCode);

        /// <summary>
        /// 根据AssemblyCode和InfoType查找AssemblyCode
        /// </summary>
        /// <param name="assemblyCode"></param>
        /// <param name="infoType"></param>
        /// <returns></returns>
        string GetAssemblyCodeInfo(string assemblyCode, string infoType);

        /// <summary>
        /// select distinct AssemblyCode from IMES_GetData..AssemblyCode where PartNo IN (Select Value from IMES_GetData..ModelInfo where Name='PN' and Model='')
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        IList<string> GetAssemblyCodesByModel(string model);

        /// <summary>
        /// select distinct AssemblyCode from AssemblyCode where PartNo=''
        /// </summary>
        /// <param name="partNo"></param>
        /// <returns></returns>
        IList<string> GetAssemblyCodesByPartNo(string partNo);

        /// <summary>
        /// 根据partNo获得AssemblyCode对象列表
        /// </summary>
        /// <param name="partNo"></param>
        /// <returns></returns>
        IList<AssemblyCode> GetAssemblyCodeListByPartNo(string partNo);

        #region For Maintain

        /// <summary>
        /// 取得PartNo下的所有AssemblyCode数据的list(按Family、Model、Region列的字符序排序)
        /// </summary>
        /// <param name="partNo"></param>
        /// <returns></returns>
        IList<AssemblyCode> GetAssemblyCodeList(string partNo);

        /// <summary>
        /// 根据partNo和assemblyCode获得AssemblyCode ID列表
        /// </summary>
        /// <param name="partNo"></param>
        /// <param name="assemblyCode"></param>
        /// <returns></returns>
        IList<int> CheckExistedAssemblyCode(string partNo, string assemblyCode);

        /// <summary>
        /// 取得model, PartNo下的所有AssemblyCode数据的list(按字符序排序)
        /// 规则:
        /// 1、	取得该Model的Family和Region栏位的数据。
        /// 2、	在AssemblyCode表中搜索此PartNo和Model直接对应的AssemblyCode。若记录集不为空，则返回此结果集，放弃后续步骤。
        /// 3、	在AssemblyCode表中搜索此PartNo、Family、Region和空Model直接对应的AssemblyCode。若记录集不为空，则返回此结果集，放弃后续步骤。
        /// 4、	在AssemblyCode表中搜索此PartNo、Family和空Region、Model直接对应的AssemblyCode。若记录集不为空，则返回此结果集，放弃后续步骤。
        /// 5、	在AssemblyCode表中搜索此PartNo和空Family、Region、Model直接对应的AssemblyCode。返回此结果集。
        /// </summary>
        /// <param name="model"></param>
        /// <param name="partNo"></param>
        /// <returns></returns>
        IList<AssemblyCode> GetAssemblyCodeList(string model, string partNo);

        /// <summary>
        /// 根据partNo, family, model, region,并且不等于assemblyCodeId,查询AssemblyCode的数目
        /// </summary>
        /// <param name="partNo"></param>
        /// <param name="family"></param>
        /// <param name="model"></param>
        /// <param name="region"></param>
        /// <param name="assemblyCodeId"></param>
        /// <returns></returns>
        int CheckExistedAssemblyCode(string partNo, string family, string model, string region, string assemblyCodeId);

        /// <summary>
        /// 新增一条AssemblyCodeInfo的记录数据
        /// </summary>
        /// <param name="item"></param>
        void AddAssemblyCodeInfo(AssemblyCodeInfo item);

        /// <summary>
        /// 修改一条AssemblyCodeInfo的记录数据
        /// </summary>
        /// <param name="item"></param>
        void SaveAssemblyCodeInfo(AssemblyCodeInfo item);

        /// <summary>
        /// 删除一条AssemblyCodeInfo的记录
        /// </summary>
        /// <param name="item"></param>
        void DeleteAssemblyCodeInfo(AssemblyCodeInfo item);

        /// <summary>
        /// 除当前传入参数partNo外,检查在其他的partNo中是否有相同的assemblyCode
        /// 参考sql:where PartNo <> ? and AssemblyCode = ?
        /// </summary>
        /// <param name="partNo"></param>
        /// <param name="assemblyCode"></param>
        /// <returns></returns>
        int CheckSameAssemblyCode(string partNo, string assemblyCode);

        /// <summary>
        /// 取得Part的Type Description
        /// 参考sql：
        /// SELECT DISTINCT InfoValue as [Type Description]
        /// FROM IMES_GetData..AssemblyCodeInfo
        /// WHERE InfoType = ?
        ///       AND ISNULL(InfoValue, '') <> ''
        ///       AND AssemblyCode IN (SELECT AssemblyCode FROM IMES_GetData..AssemblyCode WHERE PartNo = ?)
        /// ORDER BY [Type Description]  
        /// </summary>
        /// <param name="infoType"></param>
        /// <param name="partNo"></param>
        /// <returns></returns>
        IList<string> GetPartTypeDescr(string infoType, string partNo);

        void DeleteAssemblyCodeInfoByPN(string partNo);

        #region Defered

        void AddAssemblyCodeInfoDefered(IUnitOfWork uow, AssemblyCodeInfo item);

        void SaveAssemblyCodeInfoDefered(IUnitOfWork uow, AssemblyCodeInfo item);

        void DeleteAssemblyCodeInfoDefered(IUnitOfWork uow, AssemblyCodeInfo item);

        void DeleteAssemblyCodeInfoByPNDefered(IUnitOfWork uow, string partNo);

        #endregion

        #endregion
    }
}
