using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;

namespace IMES.Maintain.Interface.MaintainIntf
{
    public interface IITCNDDefectCheck
    {
        /// <summary>
        /// 添加一条ITCND Defect Check记录,当family与DB重复时,抛出异常.
        /// </summary>
        /// <param name="?"></param>
        void AddITCNDDefectCheck(ITCNDDefectCheckDef item);
        /// <summary>
        /// 删除选中的ITCND Defect Check记录.
        /// </summary>
        /// <param name="item"></param>
        void RemoveITCNDDefectCheck(ITCNDDefectCheckDef item);
        /// <summary>
        /// 得到所有的ITCND Defect Check记录.
        /// </summary>
        /// <returns></returns>
        IList<ITCNDDefectCheckDef> GetAllITCNDDefectCheckItems();
    }
}
