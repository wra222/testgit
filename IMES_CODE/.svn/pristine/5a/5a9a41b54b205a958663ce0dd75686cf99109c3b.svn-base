using System;
using System.Linq;
using System.Text;
using IMES.DataModel;
using System.Collections.Generic;

namespace IMES.Maintain.Interface.MaintainIntf
{
    /// <summary>
    /// IKittingCodeMaintain接口
    /// </summary>
    public interface IKittingCodeMaintain
    {
        /// <summary>
        /// 根据Type取得对应的Kitting Code数据的list(按Code栏位排序)
        /// </summary>
        /// <param name="type">过滤条件Type</param>
        /// <returns></returns>
        IList<KittingCodeDef> GetKittingCodeList(String type);

        /// <summary>
        /// 保存一条kitting Code的记录数据(Add), 若Code与相同Type存在记录的Code的名称相同，则提示业务异常
        /// </summary>
        /// <param name="obj">KittingCodeDef结构</param>
        void AddKittingCode(KittingCodeDef obj);


        /// <summary>
        /// 保存一条kitting Code的记录数据(update), 若Code与相同Type存在记录的Code的名称相同，则提示业务异常
        /// </summary>
        /// <param name="obj">更新KittingCodeDef结构</param>
        /// <param name="oldCode">修改前Code</param>
        void UpdateKittingCode(KittingCodeDef obj, String oldCode);

        /// <summary>
        /// 删除一条Kitting Code的记录数据
        /// </summary>
        /// <param name="obj">KittingCodeDef结构</param>
        void DeleteKittingCode(KittingCodeDef obj);
    }
}
