/*
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:FA MB  Return          
 * CI-MES12-SPEC-PAK-FA MB Return.docx –2012/1/10  
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2012-1-10   207003                Create  
 * Known issues:
*/

using System;
using System.Collections.Generic;
using IMES.DataModel;
using System.Collections;

namespace IMES.Docking.Interface.DockingIntf
{
   
    /// <summary>
    /// FA MB Return
    /// </summary>
    public interface IFAMBReturn
    {
        /// <summary>
        /// delete repair of MB,and save MB
        /// </summary>
        /// <param name="line">line</param>
        /// <param name="editor">editor</param>
        /// <param name="station">station</param>
        /// <param name="customer">customer</param>
        /// <param name="mbsno">mbsno</param>
        void OnMBSNSave(string line, string editor, string station, string customer, string mbsno);
        /// <summary>
        /// 界面确认后接着做
        /// </summary>
        /// <param name="mbSno"></param>
        /// <param name="continueDo"></param>
        void MakeSureSave(string mbSno, bool continueDo);
        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="sno"></param>
        void Cancel(string sno);
    }

}
