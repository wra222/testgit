using System.Collections.Generic;

namespace IMES.FisObject.Common.Part
{
    /// <summary>
    /// 所有与Part进行绑定的对象需要实现此接口，e.g. Product, MB
    /// </summary>
    public interface IPartOwner
    {
        /// <summary>
        /// key of Owner (productId for Product; mbsn for MB)
        /// </summary>
        string OwnerId { get; }

        /// <summary>
        /// Family
        /// </summary>
        string Family { get; }

        /// <summary>
        /// Model
        /// </summary>
        string Model { get; }

        /// <summary>
        /// 检查指定part是否已经和其它PartOwner绑定, 只按照通用规则检查，
        /// 即认为已绑定Part都存放在Product_Part, PCB_Part,Pizza_Part
        /// </summary>
        /// <param name="pn">pn</param>
        /// <param name="sn">sn</param>
        void PartUniqueCheck(string pn, string sn);
        /// <summary>
        /// 检查指定part是否已经和any PartOwner绑定(including current owner), 只按照通用规则检查，
        /// 即认为已绑定Part都存放在Product_Part, PCB_Part, Pizza_Part
        /// </summary>
        /// <param name="pn">pn</param>
        /// <param name="sn">sn</param>
        void PartUniqueCheckWithoutOwner(string pn, string sn);
        
        /// <summary>
        /// 浪琩PartNo/PartType/CT/CheckItemType 琌Μ栋筁,璝ゼΜ栋筁厨岿
        /// </summary>
        /// <param name="partType"></param>
        /// <param name="partNoList"></param>
        /// <param name="BomNodeType"></param>
        /// <param name="checkItemType"></param>
        /// <param name="sn"></param>
        /// <returns></returns>
        bool CheckPartBinded(string partType, IList<string> partNoList, string BomNodeType, string checkItemType, string sn);

        /// <summary>
        /// 将Part绑定到PartOwner对象
        /// 根据Part类型获取相应的IPartStrategy实现，调用IPartStrategy.BindTo()
        /// 以实现不同类型Part的绑定逻辑
        /// </summary>
        /// <param name="parts">要绑定的part列表</param>
        void BindPart(List<IProductPart> parts);

        /// <summary>
        /// 在ProductPart列表中添加指定part
        /// 这是PartOwner的默认行为，只将
        /// Part单纯绑定到Owner不会针对不
        /// 同的Part类型进行特殊的处理
        /// </summary>
        /// <param name="part">part</param>
        void AddPart(IProductPart part);

        /// <summary>
        /// 在Part列表中删除指定part
        /// </summary>
        /// <param name="partSn">partSn</param>
        /// <param name="partPn">partPn</param>
        void RemovePart(string partSn, string partPn);

        /// <summary>
        /// 删除所有的Parts
        /// </summary>
        void RemoveAllParts();

        /// <summary>
        /// 删除指定CheckItemType的Part
        /// </summary>
        void RemovePartsByType(string type);

        /// <summary>
        /// 删除指定BomNodeType类型的Part
        /// </summary>
        void RemovePartsByBomNodeType(string bomNodeType);

        /// <summary>
        /// 获取指定站收集的Part
        /// </summary>
        /// <param name="station">station</param>
        /// <returns>Part列表</returns>
        IList<IProductPart> GetProductPartByStation(string station);
    }
}