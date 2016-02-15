using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;

namespace IMES.Maintain.Interface.MaintainIntf
{
    /// <summary>
    /// IVendorCode接口
    /// </summary>
    public interface IVendorCode
    {
        /// <summary>
        /// 取得IqcPnoBom表中的Vendor值
        /// </summary>
        /// <returns></returns>
        IList<string> GetVendorFromIqcPnoBom();

         /// <summary>
         /// 取得VendorCode数据列表(按Vendor、Priority栏位排序)
         /// </summary>
         /// <returns></returns>
        IList<VendorCodeDef> GetAllVendorCodeList();

        /// <summary>
        /// 保存一条Vendor Code的记录数据(Add)
        /// </summary>
        /// <param name="obj">VendorCodeDef结构</param>
        void AddVendorCode(VendorCodeDef obj,string SelVender);

        /// <summary>
        /// 删除一条Vendor Code的记录数据
        /// </summary>
        /// <param name="obj">VendorCodeDef结构</param>
        void DeleteVendorCode(VendorCodeDef obj);
    }
}
