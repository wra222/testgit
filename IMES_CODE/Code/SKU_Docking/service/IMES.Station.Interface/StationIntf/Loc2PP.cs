// INVENTEC corporation ©2011 all rights reserved. 
// Description:FA 2PP
// UI:
// UC:CI-MES12-SPEC-FA-UC 2PP Input.docx
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2013-1-30    Frank                        Create 
// Known issues:
// TODO：

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;

namespace IMES.Station.Interface.StationIntf
{
    /// <summary>
    /// 更换主板器件后，记录更换信息，主板刷出窗口
    /// </summary>
    public interface ILoc2PP
    {
        #region "methods interact with the running workflow"


       /// <summary>
        /// 刷ProdId，检查输入的prodId
       /// </summary>
       /// <param name="prodId"></param>
        string ChkProdId(string prodId);

        /// <summary>
        /// 刷库位后，存数据
        /// </summary>
		/// <param name="prodId"></param>
        /// <param name="loc"></param>
		/// <param name="line"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <returns>loc</returns>
        string InputLoc(string prodId, string loc, string line, string editor, string station, string customer);

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="prodId"></param>
        void cancel(string prodId);
        #endregion

        #region "methods do not interact with the running workflow"

        #endregion
    }
}
