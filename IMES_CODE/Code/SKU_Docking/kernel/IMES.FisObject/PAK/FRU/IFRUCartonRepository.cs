using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.FisObject.PAK.FRU
{
    public interface IFRUCartonRepository : IRepository<FRUCarton>
    {
        /// <summary>
        /// �����Part
        /// </summary>
        /// <param name="carton"></param>
        /// <returns></returns>
        FRUCarton FillCartonParts(FRUCarton carton);

        /// <summary>
        /// �����Gift
        /// </summary>
        /// <param name="carton"></param>
        /// <returns></returns>
        FRUCarton FillCartonGifts(FRUCarton carton);

        /// <summary>
        /// ���FRUCarton_FRUGift�Ƿ����
        /// </summary>
        /// <param name="giftId"></param>
        /// <returns></returns>
        bool CheckExistForFRUCarton_FRUGift(string giftId);
    }
}