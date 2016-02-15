using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;

namespace IMES.Maintain.Interface.MaintainIntf
{
    /// <summary>
    /// IBomDescrMaintain接口
    /// </summary>
    public interface IBomDescrMaintain
    {
        /// <summary>
        /// 根据Type取得对应的Bom Description数据的list(按Code栏位排序)
        /// </summary>
        /// <param name="type">过滤条件Type</param>
        /// <returns></returns>
        IList<BomDescrDef> GetBomDescrList(String type);

        /// <summary>
        /// 保存一条Bom Description的记录数据(Add), 若Code与存在记录的Code的名称相同，则提示业务异常
        /// </summary>
        /// <param name="obj">BomDescrDef结构</param>
        void AddBomDescr(BomDescrDef obj);


        /// <summary>
        /// 保存一条Bom Description的记录数据(update), 若Code与存在记录的Code的名称相同，则提示业务异常
        /// </summary>
        /// <param name="obj">更新BomDescrDef结构</param>
        /// <param name="oldCode">修改前Code</param>
        void UpdateBomDescr(BomDescrDef obj, String oldCode);

        /// <summary>
        /// 删除一条Bom Description的记录数据
        /// </summary>
        /// <param name="obj">BomDescrDef结构</param>
        void DeleteBomDescr(BomDescrDef obj);

        /// <summary>
        /// 取得DescType数据表中的所有Tp记录，按字符序排列
        /// </summary>
        /// <returns></returns>
        IList<string> GetAllDescrType();
    }
}
