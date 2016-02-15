/*
 * INVENTEC corporation (c)2011 all rights reserved. 
 * Description: FinalScan interface
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2011-10-10  Zhang.Kai-sheng       (Reference Ebook SourceCode) Create
 * TODO:
 * UI/UC Update (2010/10/20),ADD "Chep Pallet" 
 * UC --"5. Check Pallet"/"7. Check Chep Pallet"/"6. Save Data" -add "Chep Pallet Check" process (P7-P10)
 * Known issues:
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;
using System.Collections;


namespace IMES.Station.Interface.StationIntf
{
    
    /// <summary>
    /// 
    /// </summary>
    ///[Serializable]
    ///public struct FwdPltInfo11
    ///{
    ///    public string Date;
    ///    public string Operator;
    ///    public string PickID;
    ///    public string Plt;
    ///    public int Qty;
    ///    public string Status;        
    ///}
    
    /// <summary>
    /// 刷入Pick ID以及Pallet No，实现Final Scan作业。
    /// 目的：Empty
    /// </summary>
    public interface IFinalScan
    {
        /// <summary>
        /// 输入pickID相关信息并处理
        /// </summary>
        /// <param name="pdLine">Pd Line</param>
        /// <param name="pickID">Pick ID</param>
        /// <param name="editor">operator</param>
        /// <param name="stationId">stationId</param>
        /// <param name="customer">customer</param>
        /// <returns>Remain Pallet and Remain Qty</returns>
        IList InputPickID(
            string pdLine,
            string pickID,
            string editor, string stationId, string customer);
       
        /// <summary>
        /// 输入UCCID相关信息并处理
        /// </summary>
        /// <param name="pdLine">Pd Line</param>
        /// <param name="pltNo">UCCID</param>
        /// <param name="editor">operator</param>
        /// <param name="stationId">stationId</param>
        /// <param name="customer"customer></param>
        IList InputUCCID(
            string pdLine,
            string pickID,
            string UCCID,
            string editor, string stationId, string customer);

        /// <summary>
        /// 输入UCCID相关信息并处理
        /// </summary>
        /// <param name="pdLine">Pd Line</param>
        /// <param name="pltNo">Pallet No</param>
        /// <param name="editor">operator</param>
        /// <param name="stationId">stationId</param>
        /// <param name="customer"customer></param>
        IList InputPalletNo(
            string pdLine,
            string pickID,
            string pltNo,
            string editor, string stationId, string customer);

        /// <summary>
        /// InputChepPalletNo
        /// </summary>
        /// <param name="pdLine"></param>
        /// <param name="pickID"></param>
        /// <param name="pltNo"></param>
        /// <param name="cheppltNo"></param>
        /// <param name="editor"></param>
        /// <param name="stationId"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        IList InputChepPalletNo(
           string pdLine,
           string pickID,
           string pltNo,
           string chepPltNo,
           string editor, string stationId, string customer);
        


        IList<FwdPltInfo> GetFwdPltList(
            string pdLine,
            string pickID,
            string pltNo,
            string editor, string stationId, string customer);

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="sessionKey"></param>
        void Cancel(string sessionKey);
    }
}
