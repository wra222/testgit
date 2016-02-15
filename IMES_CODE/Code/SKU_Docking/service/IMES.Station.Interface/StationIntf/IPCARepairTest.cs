//created by itc206070

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMES.Station.Interface.StationIntf
{
    /// <summary>
    /// [FA Cosmetic] 实现如下功能：
    /// 处理Product手动确认不良的业务
    /// </summary>
    public interface IPCARepairTest 
    {
        /// <summary>
        /// 输入Product Id相关信息并处理
        /// </summary>
        /// <param name="pdLine">Pd Line</param>
        /// <param name="mbSno">MBSno</param>
        /// <param name="editor">Editor</param>
        /// <param name="stationId">Station</param>
        /// <param name="customer">Customer</param>
        void InputMBSno(
            string pdLine,
            string mbSno,
            string editor, string stationId, string customer);

        /// <summary>
        /// 不良品信息。
        /// </summary>
        /// <param name="mbSno">MBSno</param>
        /// <param name="defectList">Defect IList</param>
        /// <param name="scanCode">scanCode</param>
        /// <returns>string</returns>
        string InputDefectCodeList(
            string mbSno,
            IList<string> defectList,
            string scanCode);

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="sessionKey"></param>
        void Cancel(string sessionKey);
    }
}
