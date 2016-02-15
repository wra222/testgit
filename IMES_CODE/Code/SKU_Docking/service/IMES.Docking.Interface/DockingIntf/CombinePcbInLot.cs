/*
 * INVENTEC corporation: 2012 all rights reserved. 
 * Description: CombinePCBinLot
 * UI:CI-MES12-SPEC-SA-UI Combine PCB in Lot.docx 
 * UC:CI-MES12-SPEC-SA-UC Combine PCB in Lot..docx          
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2012-07-11   Kaisheng,Zhang        Create
 * Known issues:
 * TODO：
 * UC 具体业务：  
 *               
 * UC Revision:  3382
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;
using System.Collections;

namespace IMES.Docking.Interface.DockingIntf
{
    /// <summary>
    /// Combine PCB in Lot
    /// </summary> 
    /// 
    public interface ICombinePcbInLot
    {
        #region methods interact with the running workflow

        /// <summary>
        /// 刷mbsno，调用该方法启动工作流
        /// </summary>
        /// <param name="mbsno">mbsno</param>
        /// <param name="editor">editor</param>
        /// <param name="station">station</param>
        /// <param name="customer">customer</param>
        /// <returns>model</returns>
        ArrayList inputMBSno(string mbsno, string editor, string station, string customer);


        /// <summary>
        /// 扫描9999，结束工作流
        /// </summary>
        /// <param name="mbsno">mbsno</param>
        /// <param name="LotNo">LotNo</param>
        ArrayList save(string mbsno, string LotNo);

        #endregion


        #region methods do not interact with the running workflow


        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="mbsno"></param>
        void Cancel(string mbsno);


        #endregion

    }
}
