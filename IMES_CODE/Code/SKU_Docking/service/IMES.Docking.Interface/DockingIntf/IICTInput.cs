﻿// INVENTEC corporation (c)2012 all rights reserved. 
// Description: ICT Input Interface
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2012-01-16   Yuan XiaoWei                 create
// 2012-01-17   Yang Weihua                  add methods for MBReinput, ECRReprint
// Known issues:
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;
using System.Collections;

namespace IMES.Docking.Interface.DockingIntf
{
    /// <summary>
    /// 
    /// </summary>
    public interface IICTInput
    {
        /// <summary>
        /// scan mbsno
        /// </summary>
        /// <param name="MBSno"></param>
        /// <param name="ecr"></param>
        /// <param name="warrantyID"></param>
        /// <param name="editor"></param>
        /// <param name="line"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        void InputMBSno(string MBSno, string ecr, int warrantyID, string editor, string line, string station, string customer);

        /// <summary>
        /// save
        /// </summary>
        /// <param name="MBSno"></param>
        /// <param name="aoi"></param>
        /// <param name="defectList"></param>
        /// <param name="printItems"></param>
        /// <returns></returns>
        ArrayList Save(string MBSno, string aoi, IList<string> defectList, IList<PrintItem> printItems);


        /// <summary>
        /// cancel workflow
        /// </summary>
        /// <param name="sessionKey"></param>
        void Cancel(string sessionKey);

        #region ECR Label Reprint

        /// <summary>
        /// Reprint Input mbSno, Whether this mb can be reprinted.
        /// </summary>
        /// <returns>MBInfo</returns>
        MBInfo EcrReprintInputMBSno(string mbSno, string editor, string line, string station, string customer);

        /// <summary>
        /// reprint ecr label
        /// </summary>
        /// <param name="mbSno"></param>
        /// <param name="reason"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <param name="printItems"></param>
        /// <param name="line"></param>
        /// <returns></returns>
        IList<PrintItem> EcrReprint(
            string mbSno,
            string reason,
            string editor, 
            string line,
            string station,
            string customer,
            IList<PrintItem> printItems);
        #endregion

        #region MB Reinput
        /// <summary>
        /// input mbSno for MB reinput
        /// </summary>
        /// <returns>MBInfo</returns>
        MBInfo MBReinputInputMBSno(string mbSno, string editor, string line, string station, string customer);

        /// <summary>
        /// save for MB reinput
        /// </summary>
        /// <param name="mbSno"></param>
        /// <param name="editor"></param>
        /// <param name="line"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        void MBReinputSave(string mbSno, string editor, string line, string station, string customer);



        #endregion
    }
}
