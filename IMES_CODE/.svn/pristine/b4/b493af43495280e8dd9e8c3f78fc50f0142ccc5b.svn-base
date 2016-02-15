// INVENTEC corporation (c)2010 all rights reserved. 
// Description: Switch Board Label Print interface
//                    
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2010-04-22   Yuan XiaoWei                 create
// Known issues:
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;

namespace IMES.Station.Interface.StationIntf
{
    /// <summary>
    /// Switch Board Label Print
    /// </summary>
    public interface ISwitchBoardLabelPrint
    {
        /// <summary>
        /// 取得Switch Board的Family信息列表
        /// </summary>
        /// <returns>Switch Board的Family信息列表</returns>
        IList<string> GetFamilyList();

        /// <summary>
        ///取得Family的PCB信息列表 
        /// </summary>
        /// <param name="family"></param>
        /// <returns></returns>
        IList<string> GetPCBListByFamily(string family);

        /// <summary>
        /// 取得PCB的111信息列表 
        /// </summary>
        /// <param name="pcb"></param>
        /// <param name="family"></param>
        /// <returns></returns>
        IList<string> Get111ListByPCB(string pcb, string family);

        /// <summary>
        ///  取得111的FruNo信息列表
        /// </summary>
        /// <param name="pn111"></param>
        /// <returns></returns>
        string GetFruNoBy111(string pn111);

        /// <summary>
        /// 打印
        /// </summary>
        /// <param name="fruNo"></param>
        /// <param name="qty"></param>
        /// <param name="line"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <param name="printItems"></param>
        /// <returns></returns>
        IList<PrintItem> Print(string pn111, string fruNo, int qty, string line, string editor, string station, string customer, IList<PrintItem> printItems);
    }
}
