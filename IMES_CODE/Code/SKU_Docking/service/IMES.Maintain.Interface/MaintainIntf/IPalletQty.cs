using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;
using System.Data;

namespace IMES.Maintain.Interface.MaintainIntf
{
    public interface IPalletQty
    {
        /// <summary>
        /// 取得Pallet Qty List下的Quantity数据的list(按fullQty排序)
        /// </summary>
        /// <param name="?"></param>
        /// <returns>IList<PalletQtyDef></returns>
        IList<PalletQtyDef> GetQtyInfoList();

        /// <summary>
        /// 保存一条Qty的记录数据(Add)
        /// </summary>
        /// <param name="Object">alletQtyDef pqInfo</param>
        string AddQtyInfo(PalletQtyDef pqInfo);


        /// <summary>
        /// 更新一条Qty的记录数据(update),
        /// </summary>
        /// <param name="Object">PalletQtyDef pqInfo, string oldFullQty</param>
        void UpdateQtyInfo(PalletQtyDef pqInfo, string oldFullQty);

        /// <summary>
        /// "删除一条Qty的记录数据
        /// </summary>
        /// <param name="?">String fullQty</param>
        void DeleteQtyInfo(String fullQty);
    }
}
