using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;

namespace IMES.Maintain.Interface.MaintainIntf
{

    public interface IFAIInfoMaintain
    {
        /// <summary>
        /// 添加一条IFAIInfoMaintain记录.
        /// </summary>
        /// <param name="item"></param>
        void AddFAIInfoMaintain(FaiInfo item);
        
        /// <summary>
        /// 得到所有的IFAIInfoMaintain记录.
        /// </summary>
        /// <returns></returns>
        IList<FaiInfo> GetAllFAIInfoMaintainItems();
        /// <summary>
        /// 得到的IFAIInfoMaintain记录.
        /// </summary>
        /// <returns></returns>
        IList<FaiInfo> GetFAIInfoMaintainItems(DateTime finTime, string iecpnPrefix, string hpqpnPrefix, string snoPrefix);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="cond"></param>
        void UpdateFAIInfoMaintain(FaiInfo item, FaiInfo cond);

       
    }
}