// created by itc205033

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.FisObject.Common.Model;
using IMES.DataModel;
using IMES.FisObject.Common.Part;
//using IMES.Station.Interface.CommonIntf;

namespace IMES.Maintain.Interface.MaintainIntf
{
    /// <summary>
    /// [Model] 实现如下功能：
    /// 使用此界面來维护PartType资料.
    /// </summary>
    public interface IPartForbiddenManager
    {
        /// <summary>
        /// 取得全部PartForbidden List
        /// </summary>
        /// <param name="strFamily"></param>
        /// <returns>PartForbidden List</returns>
        IList<PartForbiddenMaintainInfo> getPartForbiddenListByFamily(string strFamily);

        
        /// <summary>
        /// 新增/保存PartForbidden
        /// </summary>
        /// <param name="PartForbidden">PartForbidden</param>
        /// <returns></returns>
        int SavePartForbidden(PartForbiddenMaintainInfo objPartForbidden);

        /// <summary>
        /// 删除PartForbidden
        /// </summary>
        /// <param name="PartForbidden">PartForbidden</param>
        /// <returns></returns>
        void DeletePartForbidden(int partForbiddenId);
    }

}
