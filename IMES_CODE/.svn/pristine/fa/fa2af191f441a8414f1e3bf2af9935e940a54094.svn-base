// INVENTEC corporation (c)2010 all rights reserved. 
// Description: Maintain T/P AND TPCB Vendor Code 的绑定数据，并列印Label
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2010-03-22   Chen Xu (eB1-4)              create
// Known issues:

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Station.Interface.CommonIntf;
using IMES.DataModel;


namespace IMES.Station.Interface.StationIntf
{
    /// <summary>
    /// Maintain T/P AND TPCB Vendor Code 的绑定数据，并列印Label
    /// </summary>
    public interface ITPCBTPLabel
    {

        /// <summary>
        /// 通过tpcb和tp，获得VCode
        /// </summary>
        /// <param name="tpcb">tpcb</param>
        /// <param name="tp">tp</param>
        /// <param name="editor">editor</param>
        /// <param name="station">station</param>      
        /// <param name="line">line</param>
        /// <param name="customer">customer</param>
        /// <returns>vcode</returns>
        string GetVCode(string tpcb, string tp, string editor, string station,string line, string customer);

       
        
         /// <summary>
        /// Maintain T/P AND TPCB Vendor Code 的绑定数据
        /// 保存 VCode Info
        /// </summary>
        /// <param name="tpcb">tpcb</param>
        /// <param name="tp">tp</param>
        /// <param name="vcode">vcode</param> 
        /// <param name="editor">editor</param>
        /// <param name="station">station</param>      
        /// <param name="line">line</param>
        /// <param name="customer">customer</param>
        /// <returns></returns>
        void Save(string tpcb, string tp, string vcode, string editor, string station, string line, string customer);


        /// <summary>
        /// T/P AND TPCB Label Print
        /// 检查通过后，获取PrintItem
        /// </summary>
        /// <param name="tpcb">tpcb</param>
        /// <param name="tp">tp</param>
        /// <param name="vcode">vcode</param> 
        /// <param name="qty">qty</param>
        /// <param name="printItems">printItems</param>
        /// <returns>list of PrintItem</returns>  
        /// <param name="editor">editor</param>
        /// <param name="station">station</param>      
        /// <param name="line">line</param>
        /// <param name="customer">customer</param>
        IList<PrintItem> Print(string tpcb, string tp, string vcode, string qty, IList<PrintItem> printItems, string editor, string station, string line, string customer);

        
        /// <summary>
        /// Maintain T/P AND TPCB Vendor Code 的绑定数据
        /// Delete
        /// </summary>
        /// <param name="vcode">vcode</param>
        /// <param name="editor">editor</param>
        /// <param name="station">station</param>      
        /// <param name="line">line</param>
        /// <param name="customer">customer</param>
        /// <returns></returns>
        void DeleteVcode(string vcode, string editor, string station, string line, string customer);


        /// <summary>
        /// Maintain T/P AND TPCB Vendor Code 的绑定数据
        /// Query
        /// </summary>
        IList<VCodeInfo> Query();

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="sessionKey"></param>
        void Cancel(string sessionKey);


    }
}
