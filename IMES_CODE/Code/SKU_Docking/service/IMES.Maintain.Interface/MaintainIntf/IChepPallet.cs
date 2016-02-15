using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;
using System.Data;

namespace IMES.Maintain.Interface.MaintainIntf
{
    public interface IChepPallet
    {
        /// <summary>
        ///  取得ChepPallet的所有记录数据
        /// </summary>
        /// <param name="?"></param>
        /// <returns>IList<ChepPalletDef></returns>
        IList<ChepPalletDef> GetChepPalletList();

        /// <summary>
        ///  取得一条ChepPallet的记录数据
        /// </summary>
        /// <param name="?">String chepPalletNo</param>
        /// <returns>ChepPalletDef</returns>
        ChepPalletDef GetChepPalletInfor(String chepPalletNo);

        /// <summary>
        /// 保存一条ChepPallet的记录数据(Add)
        /// </summary>
        /// <param name="Object">ChepPalletDef obj</param>
        void AddChepPallet(ChepPalletDef obj);


        /// <summary>
        /// "删除一条ChepPallet的记录数据
        /// </summary>
        /// <param name="?">string obj</param>
        void DeleteChepPallet(string obj);
    }
}
