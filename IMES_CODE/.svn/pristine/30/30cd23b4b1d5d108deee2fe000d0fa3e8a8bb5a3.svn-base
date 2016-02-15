using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.UnitOfWork;
using IMES.DataModel;

namespace IMES.FisObject.FA.LCM
{
    public interface ILCMRepository : IRepository<LCM>
    {
        /// <summary>
        /// 3.—query LCM by LCMSno and METype
        /// declare @CTNO varchar(20)
        /// declare @metype varchar(5)
        /// select count(*) from IMES_FA..LCMBind where LCMSno=@CTNO and METype=@metype
        /// </summary>
        /// <param name="lcmSno"></param>
        /// <param name="meType"></param>
        /// <returns></returns>
        int GetLCMBindCount(string lcmSno, string meType);

        /// <summary>
        /// select count(*) from IMES_FA..LCMBind where MESno=@btdlsn
        /// </summary>
        /// <param name="meSno"></param>
        /// <returns></returns>
        int GetLCMBindCount(string meSno);

        /// <summary>
        /// Insert LCMBind: LCMSno=LCM CT#, MESno=BTDL SN#, METype='BTDL'
        /// </summary>
        /// <param name="item"></param>
        void InsertLCMBind(LCMME item);

        /// <summary>
        /// 1.获取ICASA的所有信息
        /// select * from ICASA ORDER BY VC
        /// </summary>
        /// <returns></returns>
        IList<ICASADef> GetICASAList();

        /// <summary>
        /// 2.用过id获取一条信息
        /// select * from ICASA where ID=id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ICASADef GetICASAInfoById(int id);

        /// <summary>
        /// 3.用过vc获取一条信息
        /// select TOP 1 * from ICASA where VC=vc
        /// </summary>
        /// <param name="vc"></param>
        /// <returns></returns>
        ICASADef GetICASAInfoByVC(string vc);

        /// <summary>
        /// 4.添加一条记录
        /// </summary>
        /// <param name="item"></param>
        void AddICASAInfo(ICASADef item);

        /// <summary>
        /// 5.更新指定id的记录 全字段where ID=id
        /// </summary>
        /// <param name="item"></param>
        /// <param name="id"></param>
        void UpdateICASAInfo(ICASADef item, int id);

        /// <summary>
        /// 6.删除指定id的记录 where ID=id
        /// </summary>
        /// <param name="id"></param>
        void DeleteICASAInfo(int id);

        /// <summary>
        /// 3) ICASA  where VC=@VC  取得 [Antel1],[ICASA] 不能全为空
        /// NOT (ISNULL([Antel1],'')='' AND ISNULL([ICASA],'')='')
        /// </summary>
        /// <param name="vc"></param>
        /// <returns></returns>
        bool CheckExistICASAByVC(string vc);

        IList<ICASADef> GetICASAInfoByVCs(string[] vcs);

        #region Defered

        void InsertLCMBindDefered(IUnitOfWork uow, LCMME item);

        void AddICASAInfoDefered(IUnitOfWork uow, ICASADef item);

        void UpdateICASAInfoDefered(IUnitOfWork uow, ICASADef item, int id);

        void DeleteICASAInfoDefered(IUnitOfWork uow, int id);

        #endregion
    }
}
