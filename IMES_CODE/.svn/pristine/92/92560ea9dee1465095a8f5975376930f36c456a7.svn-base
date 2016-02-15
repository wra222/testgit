/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: OQCOutputImpl
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2012-08-21   itc202017     Create 
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
    public interface IOQCOutput
    {
        /// <summary>
        /// 输入Product Id/CustSN相关信息并处理
        /// </summary>
        /// <param name="input">input(ProductId/CustSN)</param>
        /// <param name="editor">operator</param>
        List<string> InputProduct(
            string input, bool isID,
            string editor, string station, string customer);

        /// <summary>
        /// 记录良品/不良品信息
        /// </summary>
        /// <param name="prodId">Product Id</param>
        /// <param name="defectList">Defect IList</param>
        void InputDefectCodeList(
            string prodId,
            IList<string> defectList);

        /// <summary>
        /// Cancel
        /// </summary>
        void Cancel(string prodId);
    }
}
