using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;

namespace IMES.Maintain.Interface.MaintainIntf
{
    /// <summary>
    /// 
    /// </summary>
    public interface IRCTOMBMaintain
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="nodeType"></param>
        /// <returns></returns>
        IList<string> GetFamilyInfo(string nodeType);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="descr"></param>
        /// <returns></returns>
        IList<string> GetCodeInfo(string descr);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        void addMBMaintain(RctombmaintainInfo info);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        void deleteMBMaintain(RctombmaintainInfo info);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="family"></param>
        /// <returns></returns>
        IList<RctombmaintainInfo> getMBMaintaininfo(string family);
    }
}