using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.DataModel;
using System.Collections.Generic;
using IMES.Infrastructure.UnitOfWork;

namespace IMES.FisObject.PAK.FRU
{
    public interface IGiftRepository : IRepository<FRUGift>
    {
        /// <summary>
        /// ÕÌº”‘ÿGiftPart
        /// </summary>
        /// <param name="gift"></param>
        /// <returns></returns>
        FRUGift FillGiftParts(FRUGift gift);

        /// <summary>
        /// select count(*) from IMES_PAK..FRUGift where ID=''
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        int GetCountOfFRUGift(string id);

        #region Defered

        #endregion
    }
}