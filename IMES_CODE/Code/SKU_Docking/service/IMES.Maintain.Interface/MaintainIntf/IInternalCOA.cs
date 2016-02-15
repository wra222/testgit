using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;

namespace IMES.Maintain.Interface.MaintainIntf
{
    /// <summary>
    /// InternalCOA接口
    /// </summary>
    public interface IInternalCOA
    {
        /// <summary>
        /// 取得所有InternalCOA数据的list(按Type和Code排序)
        /// </summary>
        /// <returns>返回InternalCOADef列表</returns>
        IList<InternalCOADef> GetAllInternalCOAInfoList();

        /// <summary>
        /// 保存一条InternalCOA的记录数据(Add)
        /// </summary>
        /// <param name="obj">InternalCOADef结构</param>
        void AddInternalCOA(InternalCOADef obj);

        /// <summary>
        /// 删除一条InternalCOA的记录数据
        /// </summary>
        /// <param name="obj">InternalCOADef结构</param>
        void DeleteInternalCOA(InternalCOADef obj);
        
    }
}
