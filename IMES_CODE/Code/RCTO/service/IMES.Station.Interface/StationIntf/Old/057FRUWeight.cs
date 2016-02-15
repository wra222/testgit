
/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: FRU Weight Interface
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2010-03-09   LuycLiu     Create 
 * 该实现文件不需要编写工作流，直接掉Repositroy就可以
 * 
 * Known issues:Any restrictions about this file 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMES.Station.Interface.StationIntf
{
    public interface IFRUWeight
    {
        /// <summary>
        /// 称重
        /// </summary>
        /// <param name="pno">PNO</param>
        /// <param name="weight">重量</param>
        /// <param name="editor">操作人员</param>
        /// <param name="stationId">站ID</param>
        /// <param name="customerId">客户ID</param>
        /// <param name="line">product line</param>
        void Weight(string pno, decimal weight,string line,
             string editor, string stationId, string customerId);

        /// <summary>
        /// 检查扫入Pno的合法性
        /// </summary>
        /// <param name="pno">Pno</param>
        /// <returns>bool</returns>
        bool ValidatePNO(string pno);

        /// <summary>
        /// 根据扫入的pno获取重量
        /// </summary>
        /// <param name="pno">pno</param>
        /// <returns>重量</returns>
        decimal GetWeight(string pno);
    }
}
