/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: PCARepairImpl
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2009-11-03   207006     Create 
 * 
 * 
 * Known issues:Any restrictions about this file 
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMES.Station.Interface.StationIntf
{
    /// <summary>
    /// [PCA Test Station] 实现如下功能：
    /// 记录Function测试结果：如有不良,还需记录不良信息
    /// </summary>
    public interface IPCACosmetic 
    {
        /// <summary>
        /// 输入MB_SNo, 然后卡站，最后返回111阶号。
        /// </summary>
        /// <param name="pdLine">Pd Line</param>
        /// <param name="testStation">Test Station</param>
        /// <param name="MB_SNo">MB SNo</param>
        /// <param name="editor">operator</param>
        /// <returns>返回111阶号</returns>
        string InputMBSNo(
            string pdLine,
            string testStation,
            string MB_SNo,
            string editor, string customerId);
       
        /// <summary>
        /// 1.1	UC-PCA-PTS-01 PCA Test Station
        /// 记录Function测试结果:如有不良,还需记录不良信息.
        /// </summary>
        /// <param name="MB_SNo">MB SNO</param>
        /// <param name="defectList">Defect IList</param>
        void InputDefectCodeList(
            string MB_SNo,
            IList<string> defectList);

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="sessionKey"></param>
        void Cancel(string sessionKey);
    }
}
