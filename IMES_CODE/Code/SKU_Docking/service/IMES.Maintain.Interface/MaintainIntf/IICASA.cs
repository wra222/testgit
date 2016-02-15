using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;
using System.Data;


namespace IMES.Maintain.Interface.MaintainIntf
{
    public interface IICASA
    {
        /// <summary>
        ///  取得ICASA的所有记录数据
        /// </summary>
        /// <param name="?"></param>
        /// <returns>IList<ChepPalletDef></returns>
        IList<ICASAInfo> GetICASAList();

        /// <summary>
        /// 保存一条ICASA的记录数据(Add)
        /// </summary>
        /// <param name="Object">ICASADef item</param>
        string AddICASAInfo(ICASAInfo item);

        /// <summary>
        /// 保存一条ICASA的记录数据(Save)
        /// </summary>
        /// <param name="Object">ICASADef item,string id</param>
        void UpdateICASAInfo(ICASAInfo item, string id);

        /// <summary>
        /// "删除一条ICASA的记录数据
        /// </summary>
        /// <param name="?">string id</param>
        void DeleteICASAInfo(string id);

    }
}
