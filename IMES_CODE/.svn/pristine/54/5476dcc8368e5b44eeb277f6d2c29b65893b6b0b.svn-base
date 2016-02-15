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
    /// UnitWeight Interface
    /// </summary>
    public interface IMBReflow
    {

        #region MBReflow

        /// <summary>
        /// mb reflow
        /// </summary>
        /// <param name="MBSN"></param>
        /// <param name="ECR"></param>
        /// <param name="DateCode"></param>
        /// <param name="line"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        IList<PrintItem> Reflow(string MBSN, string ECR, string DateCode, string line, string editor, string station, string customer, IList<PrintItem> printItems, out bool IsQuantity, out string iecVer, out string CustVer,out string MECR);

        /// <summary>
        /// cancel mbsn
        /// </summary>
        /// <param name="MBSN"></param>
        void Cancel(string MBSN);

        /// <summary>
        /// save data
        /// </summary>
        /// <param name="MBSN"></param>
        /// <param name="IECVerson"></param>
        /// <param name="CustVersion"></param>
        /// <returns></returns>
        IList<PrintItem> Save(string MBSN, string ECR, string IECVerson, string CustVersion, string DateCode);
        #endregion


    }
}
