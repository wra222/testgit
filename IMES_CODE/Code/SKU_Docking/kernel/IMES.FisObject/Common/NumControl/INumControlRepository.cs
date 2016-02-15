// 2010-04-28 Liu Dong(eB1-4)         Modify 生成号: 不再将每个生成的号码都存NumControl表了.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.UnitOfWork;
using System.Data;
using IMES.DataModel;

namespace IMES.FisObject.Common.NumControl
{
    public interface INumControlRepository : IRepository<NumControl>
    {
        /// <summary>
        /// 根据Type,Customer, 前缀来查找NumControl里的最大值.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="preStr"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        string GetMaxNumber(string type, string preStr, string customer);

        NumControl GetMaxNumberObj(string type, string preStr, string customer);

        /// <summary>
        /// 根据Type， 前缀来查找NumControl里的最大值.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="preStr"></param>
        /// <returns></returns>
        string GetMaxNumber(string type, string preStr);

        NumControl GetMaxNumberObj(string type, string preStr);

        /// <summary>
        /// select max(convert(int,Value)) from NumControl where NoType='UCCID'
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        string GetMaxUCCIDNumber(string type);

        // 2010-04-28 Liu Dong(eB1-4)         Modify 生成号: 不再将每个生成的号码都存NumControl表了.
        /// <summary>
        /// 保存NumControl, 新增或修改.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="insertOrUpdate"></param>
        /// <param name="preStr"></param>
        void SaveMaxNumber(NumControl item, bool insertOrUpdate, string preStr);

        void SaveMaxNumberWithOutByCustomer(NumControl item, bool insertOrUpdate, string preStr);

        void SaveMaxNumber(NumControl item, bool insertOrUpdate);

        /// <summary>
        /// 获得指定状态,111 Part Code的地址范围及其使用到的位置.
        /// </summary>
        /// <param name="_111PartCode"></param>
        /// <param name="rangeStatus"></param>
        /// <param name="maxNumber"></param>
        /// <returns></returns>
        MACRange GetMaxMACRange(string _111PartCode, string rangeStatus, out string maxNumber);

        /// <summary>
        /// 获得指定状态,111 Part Code的可用的地址范围
        /// </summary>
        /// <param name="_111PartCode"></param>
        /// <param name="rangeStatus"></param>
        /// <returns></returns>
        MACRange GetAvailableRange(string _111PartCode, string rangeStatus);

        /// <summary>
        /// 更新地址范围的状态.
        /// </summary>
        /// <param name="macRangeId"></param>
        /// <param name="rangeStatus"></param>
        void SetMACRangeStatus(int macRangeId, string rangeStatus);

        /// <summary>
        /// 保存最大的MAC地址.
        /// </summary>
        /// <param name="item"></param>
        void SaveMaxMAC(NumControl item);

        /// <summary>
        /// 根据Notype和Value取得NumControl的记录数据
        /// </summary>
        /// <param name="noType"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        IList<NumControl> GetNumControlByNoTypeAndValue(string noType, string value);

        /// <summary>
        /// A.	得到IE维护的Asset SN可用范围：AssetRange表里的得到Begin和End，按照Code=@cust作查询
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        IList<AssetRangeInfo> GetAssetRangeInfo(string code);

        string GetMaxAssetNumber(string noType, string noName, string customer);

        /// <summary>
        /// B.	生成序号：当没有找到该CUST已用过的最大号时，使用定义范围的最小号，否则作+1处理。字符包括”0123456789”
        /// </summary>
        /// <param name="item"></param>
        /// <param name="insertOrUpdate"></param>
        void SaveMaxAssetNumber(NumControl item, bool insertOrUpdate);

        /// <summary>
        /// select top 1 * from MACRange where Code = @code and Status IN (‘R’,’A’) order by Cdt
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        MACRange GetMACRange(string code, string[] statuses);

        /// <summary>
        /// select top 1 * from NumControl where NoName=@NoName and NoType=@NoType order by Value desc
        /// </summary>
        /// <param name="noType"></param>
        /// <param name="noName"></param>
        /// <returns></returns>
        NumControl GetMaxValue(string noType, string noName);

        /// <summary>
        /// select top 1 * from NumControl where NoName=@NoName and NoType=@NoType and Customer=@Customer order by Value desc
        /// </summary>
        /// <param name="noType"></param>
        /// <param name="noName"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        NumControl GetMaxValue(string noType, string noName, string customer);

        #region . Defered  .

        // 2010-04-28 Liu Dong(eB1-4)         Modify 生成号: 不再将每个生成的号码都存NumControl表了.
        void SaveMaxNumberDefered(IUnitOfWork uow, NumControl item, bool insertOrUpdate, string preStr);

        void SaveMaxNumberWithOutByCustomerDefered(IUnitOfWork uow, NumControl item, bool insertOrUpdate, string preStr);

        void SetMACRangeStatusDefered(IUnitOfWork uow, int macRangeId, string rangeStatus);

        void SaveMaxMACDefered(IUnitOfWork uow, NumControl item);

        void SaveMaxAssetNumberDefered(IUnitOfWork uow, NumControl item, bool insertOrUpdate);

        #endregion

        #region For Maintain

        /// <summary>
        ///  取得MACRange List列表（按“Code”列的字母序排序+Cdt排序）
        /// </summary>
        IList<MACRange> GetMACRangeList();

        /// <summary>
        /// 取得一条MACRang的记录数据
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        MACRange GetMACRange(int macRangeId);

        /// <summary>
        /// 保存一条MACRang的记录数据(Add)
        /// </summary>
        /// <param name="Object"></param>
        void AddMACRange(MACRange Object);

        /// <summary>
        /// 保存一条MACRang的记录数据(update)
        /// </summary>
        /// <param name="Object"></param>
        void UpdateMACRange(MACRange Object);

        /// <summary>
        /// 删除一条MACRang数据
        /// </summary>
        /// <param name="?"></param>
        void DeleteMACRange(int macRangeId);

        /// <summary>
        /// 取得code下的Mac总数
        /// 指定Code对应的所有MAC Range的CodeEndNo-BeginNo+1的合计值
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        long GetMACRangeTotalByCode(string code);

        /// <summary>
        /// 取得code下的MacRange使用的Mac总数
        /// 指定Code对应的所有Status为Close的MAC Range的CodeEndNo-BeginNo+1的合计值加上NumControl表中对应该
        /// Code的Value减去该Code对应的Status为Active的MAC Range的BeginNo再加1
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        int GetMACRangeTotalUsedByCode(string code);

        /// <summary>
        /// SELECT COUNT(*) FROM HDCPKey WHERE Status = 'R'
        /// </summary>
        /// <returns></returns>
        DataTable GetHDCPQuery();

        void InsertNumControl(NumControl item);

        #region Defered

        void AddMACRangeDefered(IUnitOfWork uow, MACRange Object);

        void UpdateMACRangeDefered(IUnitOfWork uow, MACRange Object);

        void DeleteMACRangeDefered(IUnitOfWork uow, int macRangeId);

        void InsertNumControlDefered(IUnitOfWork uow, NumControl item);

        #endregion

        #endregion
    }
}
