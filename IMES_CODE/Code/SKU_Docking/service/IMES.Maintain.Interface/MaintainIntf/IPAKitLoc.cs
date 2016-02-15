using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;

namespace IMES.Maintain.Interface.MaintainIntf
{
   public interface IPAKitLoc
    {
       
       /// <summary>
       /// 查询所有的pdLine信息
       /// </summary>
       /// <returns></returns>
        IList<string> GetAllPdLine();
       /// <summary>
       /// 按照用户所选的pdLine,显示符合条件的信息
       /// </summary>
       /// <param name="pdline"></param>
       /// <returns></returns>
        IList<PAKitLocDef> GetPAKitlocByPdLine(string pdline);
       /// <summary>
       /// 添加一条PAKitLoc
       /// 当PDLine,PartNo与已经存在数据库中的数据重复时,抛出异常
       /// </summary>
       /// <param name="item"></param>
       /// <returns></returns>
        string AddPAKitLoc(PAKitLocDef item);
       /// <summary>
       /// 更新一条记录
        ///  当PDLine,PartNo与已经存在数据库中的数据重复时,抛出异常
        ///  当所要更新的记录在数据库中不存在时,抛出异常
       /// </summary>
       /// <param name="newItem"></param>
        void UpdatePAKitLoc(PAKitLocDef newItem);
       /// <summary>
       /// 根据id删除所选的记录
       /// </summary>
       /// <param name="oldItem"></param>
        void DeletePAKitLoc(PAKitLocDef oldItem);
       /// <summary>
       /// 查询所有符合条件的TypeDescr
       /// </summary>
       /// <returns></returns>
        IList<string> GetAllTypeDescr();
       /// <summary>
       /// 根据所选的TypeDescr,加载与其对应的PartNo
       /// </summary>
       /// <param name="typeDescr"></param>
       /// <returns></returns>
        IList<string> GetPartNoByTypeDescr(string typeDescr);
       /// <summary>
       /// 根据查询Family表中的type为PAKKitting的family字段
       /// </summary>
       /// <param name="stationtype"></param>
       /// <returns></returns>
        IList<string> GetAllPAKikittingStationName();
        IList<string> FindCodeByFamily(string family);
    }
}
