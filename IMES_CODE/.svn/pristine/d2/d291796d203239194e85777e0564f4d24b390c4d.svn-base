/*
 * INVENTEC corporation ?2011 all rights reserved. 
 * Description:Interface for REV Label Print For Docking Page
 * UI:CI-MES12-SPEC-FA-UI REV Label Print For Docking.docx –2012/5/28 
 * UC:CI-MES12-SPEC-FA-UC REV Label Print For Docking.docx –2012/5/28            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
* 2012-5-30   Jessica Liu           (Reference Ebook SourceCode) Create
* Known issues:
* TODO：
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using IMES.Station.Interface.CommonIntf;
using IMES.DataModel;


namespace IMES.Docking.Interface.DockingIntf
{
    public interface IREVLabelPrintForDocking
    {

        /// <summary>
        /// 打印REV Label For Docking
        /// </summary>
        /// <param name="family">Family</param>
        /// <param name="dcode">DataCode</param>
        /// <param name="qty">PrintQty</param>
        /// <param name="stationId">product station</param>
        /// <param name="editor">editor</param>
        /// <param name="customerId">customer sn</param>
        /// <param name="printItems">Print Item列表</param>
        /// <returns>DCode、Print Item列表</returns>
        ArrayList Print(string family, string dcode, int qty, string stationId, string editor, string customerId, IList<PrintItem> printItems);


        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="sessionKey"></param>
        void Cancel(string sessionKey);
    }
}