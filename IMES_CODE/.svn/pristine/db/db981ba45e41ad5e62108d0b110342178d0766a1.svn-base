using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;

namespace IMES.Maintain.Interface.MaintainIntf
{
    public interface IUniteMB
    {

        /// <summary>
        /// 获取所有Unite MB记录
        /// </summary>
        /// <returns></returns>
        IList<MBCodeDef> getAllUniteMB();

        /// <summary>
        /// 根据MBCode获取Unite MB记录
        /// </summary>
        /// <param name="MBCode"></param>
        /// <returns></returns>
        IList<MBCodeDef> getLstByMB(string MBCode);

        /// <summary>
        /// 添加一条Unite MB记录
        /// </summary>
        /// <param name="obj"></param>
        void addUniteMB(MBCodeDef obj);

        /// <summary>
        /// 删除一条Unite MB记录
        /// </summary>
        /// <param name="MBCode"></param>
        void deleteUniteMB(string MBCode);

        /// <summary>
        /// 修改一条Unite MB记录
        /// </summary>
        /// <param name="obj"></param>
        void updateUniteMB(MBCodeDef obj, string MBCode);
    }
}
