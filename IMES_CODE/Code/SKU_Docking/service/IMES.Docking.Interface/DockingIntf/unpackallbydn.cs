/*
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:Web method for CombineCOAandDN Page            
 * CI-MES12-SPEC-PAK Combine COA and DN.docx
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2011-12-20  itc207003              Create
 * Known issues:
*/

using System;
using System.Collections.Generic;
using IMES.DataModel;
using System.Collections;


namespace IMES.Docking.Interface.DockingIntf
{
    
    
    /// <summary>
    /// UnpackAllbyDN
    /// </summary>
    public interface IUnpackAllbyDN
    {
        /// <summary>
        /// 获取GetDN表相关信息
        /// </summary>
        /// <param name="DN">DN</param> 
        void GetDN(string DN);
        /// <summary>
        /// DoUnpackByDN
        /// </summary>
        /// <param name="editor">editor</param>
        /// <param name="station">station</param>
        /// <param name="customer">customer</param>
        /// <param name="DN">DN</param> 
        void DoUnpackByDN(string editor, string station, string customer, string DN);
    }
}
