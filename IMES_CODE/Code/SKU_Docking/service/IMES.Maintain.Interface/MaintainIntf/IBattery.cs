using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;

namespace IMES.Maintain.Interface.MaintainIntf
{
    /// <summary>
    /// IBattery接口
    /// </summary>
    public interface IBattery
    {
        /// <summary>
        /// 取得所有Battery数据的list(按Battery列的字母序排序)
        /// </summary>
        /// <returns>返回BatteryDef列表</returns>
        IList<BatteryDef> GetAllBatteryInfoList(); //HPPN

        /// <summary>
        /// 根据batteryVC取得对应的Battery数据的list(左匹配原则，按Battery列的字母序排序)
        /// <param name="batteryVC">过滤条件batteryVC，对应数据库HPPN字段</param>
        /// <returns>返回BatteryDef列表</returns>
        IList<BatteryDef> GetBatteryInfoList(String batteryVC); //HPPN
 
        /// <summary>
        /// 保存一条Battery的记录数据(Add)，若BatteryVC名称与其他存在的BatteryVC的名称相同，则提示业务异常
        /// </summary>
        /// <param name="obj">Battery结构</param>
        void AddBattery(BatteryDef obj);
        
        /// <summary>
        /// 保存一条Battery的记录数据(update), 若BatteryVC名称与其他存在的BatteryVC的名称相同，则提示业务异常
        /// </summary>
        /// <param name="obj">更新的Battery结构</param>
        /// <param name="oldBattery">修改前BatteryVC</param>
        void UpdateBattery(BatteryDef obj, String oldBattery);

        /// <summary>
        /// 删除一条Battery的记录数据
        /// </summary>
        /// <param name="obj">删除的Battery结构，关键传入BatteryVC信息</param>    
        void DeleteBattery(BatteryDef obj);        
    }
}
