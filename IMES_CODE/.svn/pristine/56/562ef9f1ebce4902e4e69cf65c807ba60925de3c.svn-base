using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;
using System.Data;


namespace IMES.Maintain.Interface.MaintainIntf
{
    public interface IFAFloatLocation
    {
        /// <summary>
        ///  取得KitLoc的所有记录数据
        /// </summary>
        /// <param name="?"></param>
        /// <returns>IList<FAFloatLocationDef> chepList</returns>
        IList<FAFloatLocationDef> GetKitLocList();

        /// <summary>
        ///  根据family取得KitLoc的记录数据
        /// </summary>
        /// <param name="?">string family</param>
        /// <returns>IList<FAFloatLocationDef> chepList</returns>
        IList<FAFloatLocationDef> GetKitLocListByFamily(string family);

        /// <summary>
        ///  获取所有family记录数据
        /// </summary>
        /// <param name="?"></param>
        /// <returns>IList<string> familyLst</returns>
        IList<string> GetAllFamilys();

        /// <summary>
        ///  获取PdLine下拉框记录数据
        /// </summary>
        /// <param name="?"></param>
        /// <returns>IList<string> familyObjs</returns>
        IList<LineInfo> GetAllPdLines();

        /// <summary>
        /// 保存一条KitLoc的记录数据(Add)
        /// </summary>
        /// <param name="Object"> FAFloatLocationDef item</param>
        void AddKitLoc(FAFloatLocationDef item);

        /// <summary>
        /// 更新一条KitLoc的记录数据
        /// </summary>
        /// <param name="Object">FAFloatLocationDef item,string itemId</param>
        void UpdateKitLoc(FAFloatLocationDef kitLocDef,string itemId);

        /// <summary>
        /// "删除一条记录
        /// </summary>
        /// <param name="?">FAFloatLocationDef item</param>
        void DeleteKitLoc(FAFloatLocationDef kitLocDef);
    }
}
