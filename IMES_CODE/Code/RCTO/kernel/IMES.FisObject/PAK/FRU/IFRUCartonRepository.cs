using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.FisObject.PAK.FRU
{
    public interface IFRUCartonRepository : IRepository<FRUCarton>
    {
        /// <summary>
        /// 晚加载Part
        /// </summary>
        /// <param name="carton"></param>
        /// <returns></returns>
        FRUCarton FillCartonParts(FRUCarton carton);

        /// <summary>
        /// 晚加载Gift
        /// </summary>
        /// <param name="carton"></param>
        /// <returns></returns>
        FRUCarton FillCartonGifts(FRUCarton carton);

        /// <summary>
        /// 检查FRUCarton_FRUGift是否存在
        /// </summary>
        /// <param name="giftId"></param>
        /// <returns></returns>
        bool CheckExistForFRUCarton_FRUGift(string giftId);
    }
}