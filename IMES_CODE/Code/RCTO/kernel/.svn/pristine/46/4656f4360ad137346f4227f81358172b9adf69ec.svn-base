using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.UnitOfWork;

namespace IMES.FisObject.PCA.MB
{
    public interface IMODismantleLogRepository : IRepository<MODismantleLog>
    {
        /// <summary>
        /// 插入一组MoDismantleLog记录
        /// </summary>
        /// <param name="items"></param>
        void AddBatch(IList<MODismantleLog> items);

        #region Defered

        void AddBatchDefered(IUnitOfWork uow, IList<MODismantleLog> items);

        #endregion
    }
}
