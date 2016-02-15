using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;

namespace IMES.Maintain.Interface.MaintainIntf
{
   public interface IPLTStandard
    {
        /// <summary>
        /// 查询PLTStandard(PLTStandard)
        /// </summary>
        /// <returns></returns>
       IList<PltstandardInfo> GetPLTStandardList();

       /// <summary>
       /// 添加AddPLTStandard
       /// </summary>
       /// <param name="PLTStdItem"></param>
       /// <returns>返回被添加数据的ID</returns>      
       string AddPLTStandard(PltstandardInfo PLTStdItem);

        /// <summary>
        /// 删除选中的PLTStandard
        /// </summary>
        /// <param name="id"></param>
       void DeletePLTStandard(int id);

        /// <summary>
        /// 更新PalletStandard
        /// </summary>
        /// <param name="newItem"></param>
        void UpdatePLTStandard(PltstandardInfo PLTStdItem);

        /// <summary>
        /// 获取PLTSpecificationList列表
        /// </summary>
        /// <returns></returns>
        IList<IMES.DataModel.PltspecificationInfo> GetPLTSpecificationList();

    }
}
