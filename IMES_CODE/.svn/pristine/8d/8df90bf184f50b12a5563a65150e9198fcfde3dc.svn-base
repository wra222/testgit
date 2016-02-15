// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 070 Unpack Carton/DN/Pallet
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-03-10   Lucy Liu                create
// 2011-10-20   itc202017               Add UnpackDNByDN interface.
// Known issues:
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;
using System.Data;

namespace IMES.Docking.Interface.DockingIntf
{
    /// <summary>
    /// Unpack Carton
    /// </summary>
    public interface IUnpackForDocking
    {

        #region methods interact with the running workflow

        
        /// 使用工作流UnpackSNByAll.xoml  tmp 
        /// </summary>
        /// <param name="prodSn"></param>
        /// <param name="line"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
         void  UnpackBySNCheck(string prodSn, string line, string editor, string station, string customer);

        ///<summary>
        /// 解除Dn的绑定通过Sn or Product id
        /// 使用工作流070UnpackDNbySN.xoml
        /// </summary>
        /// <param name="prodSn"></param>

        string UnpackbySNSave(string prodSn);

        ///<summary>
        /// 解除Dn的绑定通过Sn or Product id
        /// 使用工作流070UnpackDNbySN.xoml



        void Cancel(string prodSn);

        #endregion

      
    }
}
