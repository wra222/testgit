using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.DataModel;
using IMES.Infrastructure.UnitOfWork;

namespace IMES.FisObject.PAK.WeightLog
{
    /// <summary>
    /// WeightLog对象Repository接口
    /// </summary>
    public interface IWeightLogRepository : IRepository<WeightLog>
    {
        /// <summary>
        /// select count(*) from IMES_PAK..FRUWeightLog where SN=''
        /// </summary>
        /// <param name="sn"></param>
        /// <returns></returns>
        int GetCountOfFRUWeightLog(string sn);

        /// <summary>
        /// 将记录插入或者update
        /// 没有就插入，有就更新
        /// insert into IMES_PAK..FRUWeightLog (SN,Weight,Line,Station,Editor,Cdt) Values('','','','','',getdate())
        /// update IMES_PAK..FRUWeightLog Set Weight='',Cdt=getdate() where SN=''
        /// </summary>
        /// <param name="item"></param>
        void AddOrModifyFRUWeightLog(FRUWeightLog item);

        /// <summary>
        /// select Weight from IMES_PAK..FRUWeightLog where SN=''
        /// </summary>
        /// <param name="sn"></param>
        /// <returns></returns>
        decimal GetWeightOfFRUWeightLog(string sn);

        #region Defered

        void AddOrModifyFRUWeightLogDefered(IUnitOfWork uow, FRUWeightLog item);

        #endregion
    }
}
