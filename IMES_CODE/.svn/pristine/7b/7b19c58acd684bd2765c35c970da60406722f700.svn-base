using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;
using System.Data;


namespace IMES.Maintain.Interface.MaintainIntf
{
    public interface IPartTypeAttribute
    {
        /// <summary>
        ///  取得PartType的所有记录数据
        /// </summary>
        /// <param name="?"></param>
        /// <returns>IList<PartTypeDef></returns>
        IList<PartTypeAttributeDef> GetPartTypeList();

        /// <summary>
        ///  取得PartType的所有记录数据
        /// </summary>
        /// <param name="?"></param>
        /// <returns>IList<PartTypeDef></returns>
        IList<PartTypeAttributeDef> GetPartTypeListByTp(string tp);

        /// <summary>
        ///  取得一条PartType的记录数据
        /// </summary>
        /// <param name="?">String id</param>
        /// <returns>PartTypeDef</returns>
        PartTypeAttributeDef GetPartTypeInfo(String id);

        /// <summary>
        ///  取得所有tp的数据
        /// </summary>
        /// <param name="?"></param>
        /// <returns>IList<PartTypeDef></returns>
        IList<string> GetTPList();

        /// <summary>
        ///  取得TP = tp的code数据
        /// </summary>
        /// <param name="?">string tp</param>
        /// <returns>IList<PartTypeDef></returns>
        IList<string> GetCodeListByTp(string tp);

        /// <summary>
        /// 添加一条PartType的记录数据(Add)
        /// </summary>
        /// <param name="Object">ChepPalletDef obj</param>
        string AddPartType(PartTypeAttributeDef partTypeDef);

        /// <summary>
        /// 更新一条PartType的记录数据
        /// </summary>
        /// <param name="Object">PartTypeDef PartType,string id</param>
        void UpdatePartType(PartTypeAttributeDef PartType, string id);

        /// <summary>
        /// "删除一条PartType的记录数据
        /// </summary>
        /// <param name="?">string id</param>
        void DeletePartType(string id);
    }
}
