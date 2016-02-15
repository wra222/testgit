using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;

namespace IMES.Maintain.Interface.MaintainIntf
{
    public interface IACAdaptor
    {
        /// <summary> 
        ///取得所有的ACAdaptor记录.
        /// </summary>
        /// <returns>返回数据库中所有存在的ACAdaptor记录</returns>
        IList<ACAdaptor> GetAllAdaptorInfo();
        /// <summary>
        /// 根据输入的assembly模糊查询(assembly%)
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns>返回符合条件的ACAdaptor记录</returns>
        IList<ACAdaptor> GetAdaptorByAssembly(string assembly);
        /// <summary>
        /// 删除选中的一条ACAdaptor记录
        /// </summary>
        /// <param name="item">被选中的ACAdaptor</param>
        void DeleteOneAcAdaptor(ACAdaptor item);
        /// <summary>
        /// 添加一条符合条件的ACAdaptor记录,
        /// 当所添加的记录中的assembly在其他记录中存在时,抛出异常
        /// </summary>
        /// <param name="item"></param>
        /// <returns>返回添加成功后此记录的id</returns>
        string AddOneAcAdaptor(ACAdaptor item);
        /// <summary>
        /// 更新选中记录,
        /// 当此记录中assembly在其他的记录中存在时,抛出异常
        /// </summary>
        /// <param name="newItem"></param>
        void UpdateOneAcAdaptor(ACAdaptor newItem);
    }
}
