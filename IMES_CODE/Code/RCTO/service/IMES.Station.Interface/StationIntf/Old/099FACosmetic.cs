﻿//created by itc206070

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
    public interface IFACosmetic 
    {
        /// <summary>
        /// 输入Product Id相关信息并处理
        /// </summary>
        /// <param name="pdLine">Pd Line</param>
        /// <param name="prodId">Product Id</param>
        /// <param name="editor">operator</param>
        /// <returns>prestation</returns>
        string InputProdId(
            string pdLine,
            string custsn,
            string editor, string stationId, string customer);

        /// <summary>
        /// 不良品信息。
        /// </summary>
        /// <param name="prodId">Product Id</param>
        /// <param name="defectList">Defect IList</param>
        void InputDefectCodeList(
            string prodId,
            IList<string> defectList);

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="sessionKey"></param>
        void Cancel(string sessionKey);
    }
}
