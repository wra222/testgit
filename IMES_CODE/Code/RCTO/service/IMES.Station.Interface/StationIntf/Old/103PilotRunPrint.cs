/*
 * INVENTEC corporation (c)2010 all rights reserved. 
 * Description: Pilot Run Print
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2010-04-21   Tong.Zhi-Yong     Create 
 * Known issues:Any restrictions about this file 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMES.Station.Interface.StationIntf
{
    /// <summary>
    /// 试产打印
    /// </summary>
    public interface IPilotRunPrint
    {
        /// <summary>
        /// 打印
        /// </summary>
        /// <param name="customerSN">客户序列号</param>
        /// <param name="offsetX">offsetX</param>
        /// <param name="offsetY">offsetY</param>
        /// <param name="batchFile">批处理文件名称</param>
        /// <returns>返回要执行打印的bat字符串</returns>
        string PrintPioltRun(string customerSN, int offsetX, int offsetY, string batchFile);
    }
}
