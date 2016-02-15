using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace IMES.Station.Interface.StationIntf
{
    /// <summary>
    /// 刷入OQC检验完成的整机，记录良品/不良品信息（免检模式不走此站）。
    /// 目的：实现整机到Image D/L的移转
    /// </summary>
    public interface SpecialModelForItcnd
    {
        /// <summary>
        /// 输入Product Id相关信息并处理
        /// </summary>
        /// <param name="Family">Family</param>
        /// <param name="Model">Model</param>
        /// <param name="Type">Type</param>

        ArrayList Query(
            string Family,
            string Model,
            string Type);

        /// <summary>
        /// 记录良品/不良品信息（免检模式不走此站）。
        /// </summary>
        /// <param name="Family">Family</param>
        /// <param name="Model">Model</param>
        /// <param name="Type">Type</param>
        /// <param name="user">user</param>
        void Insert(
            string Family,
            string Model,
            string Type, 
            string user,out string qcStatus);

        /// <summary>
        /// </summary>
        /// <param name="Family">Family</param>
        /// <param name="Model">Model</param>
        /// <param name="Type">Type</param>
        void Delete(string Family,
            string Model,
            string Type);
    }
}
