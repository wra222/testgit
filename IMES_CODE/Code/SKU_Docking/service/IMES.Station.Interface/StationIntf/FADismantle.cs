// INVENTEC corporation (c)2010 all rights reserved. 
// Description:  MBReflow Interface
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2010-03-15   itc207013                    create
// Known issues:
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using IMES.Station.Interface.CommonIntf;
using IMES.DataModel;

namespace IMES.Station.Interface.StationIntf
{
    /// <summary>
    /// DismantleFA Interface
    /// </summary>
    public interface IDismantleFA
    {

        #region IDismantleFA

        /// <summary>
        /// DismantleFA
        /// </summary>
        /// <param name="snorproid"></param>
        /// <param name="sDismantleType"></param>
        /// <param name="sKeyparts"></param>
        /// <param name="sReturnStation"></param>
        /// <param name="line"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        void Dismantle(string snorproid, string sDismantleType, string sKeyparts, string sReturnStation, string line, string pCode, string editor, string station, string customer);
        /// <summary>
        /// DismantleBatch
        /// </summary>
        /// <param name="snorproidlist"></param>
        /// <param name="sDismantleType"></param>
        /// <param name="sKeyparts"></param>
        /// <param name="sReturnStation"></param>
        /// <param name="line"></param>
        /// <param name="pCode"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        void DismantleBatch(IList<string> snorproidlist, string sDismantleType, string sKeyparts, string sReturnStation, string line, string pCode, string editor, string station, string customer);
        /// <summary>
        /// CheckPrdIDorSNList
        /// </summary>
        /// <param name="snorproidlist"></param>
        /// <param name="sDismantleType"></param>
        /// <param name="sKeyparts"></param>
        /// <param name="sReturnStation"></param>
        /// <param name="line"></param>
        /// <param name="pCode"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        string CheckPrdIDorSNList(IList<string> snorproidlist, string sDismantleType, string sKeyparts, string sReturnStation, string line, string pCode, string editor, string station, string customer);
        /// <summary>
        /// InputProdId
        /// </summary>
        /// <param name="snorproid"></param>
        /// <param name="line"></param>
        /// <param name="pCode"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        IList<string> InputProdId(string snorproid, string line, string pCode, string editor, string station, string customer); 
        #endregion


    }
}
