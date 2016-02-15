using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;

namespace IMES.Maintain.Interface.MaintainIntf
{
    public interface IMBAssemblyCode
    {
        ///获取所有MBCFG数据
        IList<MBCFGDef> GetAllMBCFGLst();



        ///
        ///添加一条MBCFG数据
        void AddMBCFG(MBCFGDef mbcfgDef);

        /// <summary>
        /// 根据mbcode,series和Type删除一条MBCFG数据
        /// </summary>
        /// <param name="MbCode"></param>
        /// <param name="Series"></param>
        /// <param name="Type"></param>
        void DeleteMBCFG(string MbCode, string Series, string Type);

        /// <summary>
        /// 根据mbcode,series和Type修改MBCFG数据
        /// </summary>
        /// <param name="mbcfgDef"></param>
        /// <param name="MBcode"></param>
        /// <param name="Series"></param>
        /// <param name="Type"></param>
        void UpdateMBCFG(MBCFGDef mbcfgDef, string MBcode, string Series, string Type);

        /// <summary>
        /// 根据MBcode,Series,和type获取一条MBCFG数据
        /// </summary>
        IList<MBCFGDef> GetMBCFGByCodeSeriesAndType(string MBcode, string Serices, string Type);
    }
}
