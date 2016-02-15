/*
 * INVENTEC corporation: 2012 all rights reserved. 
 * Description: PCA OQC Input
 * UI:CI-MES12-SPEC-SA-UI PCA OQC Input.docx 
 * UC:CI-MES12-SPEC-SA-UC PCA OQC Input.docx          
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2012-05-30   Kaisheng              Create
 * Known issues:
 * TODO：
 * UC 具体业务：  1.	刷入Lot相关数据，做Lot的Lock/Undo/Pass操作
 *               
 * UC Revision:  
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
    /// PCA OQC Output
    /// 连板切割入口，实现连板的切割,并打印子板标签
    /// </summary> 
    /// 
    public interface IPCAOQCInput
    {
        #region methods interact with the running workflow

       /// <summary>
        /// inputMBSnoORLotNo
       /// </summary>
       /// <param name="InputStr"></param>
       /// <param name="InputType"></param>
       /// <param name="editor"></param>
       /// <param name="station"></param>
       /// <param name="customer"></param>
       /// <returns></returns>
        ArrayList inputMBSnoORLotNo(string InputStr, string InputType, string editor, string station, string customer);

        /// <summary>
        /// InsertPcbLotCheck
        /// </summary>
        /// <param name="lotNo"></param>
        /// <param name="mbSno"></param>
        /// <param name="editor"></param>
        /// <param name="line"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        ArrayList InsertPcbLotCheck(string lotNo, String mbSno, string editor, string line, string customer);

        /// <summary>
        /// save
        /// </summary>
        /// <param name="LotNo"></param>
        /// <param name="strCMD"></param>
        string save(string LotNo, String strCMD, string editor, string line, string customer);

        #endregion


        #region methods do not interact with the running workflow


        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="LotNo"></param>
        void Cancel(string LotNo);


        #endregion

    }
}
