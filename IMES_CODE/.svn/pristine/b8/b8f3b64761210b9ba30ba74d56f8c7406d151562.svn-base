using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;

namespace IMES.Maintain.Interface.MaintainIntf
{
    public interface ICeldata
    {
       /// <summary>
        /// 得到所有的CELDATA记录
       /// </summary>
       /// <returns></returns>
        IList<CeldataDef> GetAllCeldatas();
       /// <summary>
        /// 添加一条CELDATA
       /// 当所要添加的记录中的ZMOD与其他存在的记录重复时,抛出异常
       /// </summary>
       /// <param name="item"></param>
       /// <returns>添加成功后返回此记录的ID</returns>
        string AddCeldataItem(CeldataDef item);
       /// <summary>
       /// 删除一条CELDATA
       /// </summary>
       /// <param name="id"></param>
        void DeleteCeldataItem(string zmod);
    }
}
