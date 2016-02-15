// INVENTEC corporation (c)2010 all rights reserved. 
// Description: Switch Board Label Print interface
//                    
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2010-04-22   207006                       create
// Known issues:
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;

namespace IMES.Station.Interface.StationIntf
{
    /// <summary>
    /// Borrow And Return实现借还功能
    /// </summary>
    public interface IBorrowAndReturn
    {
        /// <summary>
        /// 根据状态查询记录
        /// </summary>
        /// <param name="status"></param>
        /// <returns>返回查询结果</returns>
        IList<BorrowLog> Query(string status);

        /// <summary>
        /// 借出
        /// </summary>
        /// <param name="inputSN">借出的货号</param>
        /// <param name="borrower">借者</param>
        /// <param name="editor">借出人</param>
        void Borrow(string inputSN, string borrower,string editor);

        /// <summary>
        /// 归还
        /// </summary>
        /// <param name="inputSN">归还的货号</param>
        /// <param name="borrower">归还者</param>
        /// <param name="editor">接受者</param>
        void ReturnIt(string inputSN, string borrower,string editor);


        /// <summary>
        /// 输入货号
        /// </summary>
        /// <param name="inputSN">货号</param>
        /// <param name="model">model号</param>
        /// <param name="station">站号</param>
        /// <param name="customer">客户</param>
        /// <param name="editor">使用者</param>
        /// <param name="outInput">去掉校验位的货号</param>
        /// <returns>log中的model号</returns>
        string inputSN(string inputSN, string model, string station, string customer, string editor, out string outInput);

        /// <summary>
        /// 输入model后的检查
        /// </summary>
        /// <param name="inputSN">货号</param>
        /// <param name="model">model号</param>
        /// <returns>log中的model号</returns>
        string inputModel(string inputSN, string model);
        
        /// <summary>
        /// 取消workflow
        /// </summary>
        /// <param name="sessionKey"></param>
        void Cancel(string sessionKey);
    }
}
