/*
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:Web method for Update Ship Date Page            
 * CI-MES12-SPEC-PAK Update Ship Date.docx
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2011-12-20  itc207003              Create
 * Known issues:
*/

using System;
using System.Collections.Generic;
using IMES.Station.Interface.CommonIntf;
using IMES.DataModel;
using System.Collections;


namespace IMES.Station.Interface.StationIntf
{
    [Serializable]
    /// <summary>
    /// Row data define of UpdateShipDate page
    /// </summary>
    public struct S_RowData_ShipDate
    {
        /// <summary>
        /// dn
        /// </summary>
        public string dn;
        /// <summary>
        /// Status
        /// </summary>
        public string Status;
        /// <summary>
        /// Qty
        /// </summary>
        public string Qty;
        /// <summary>
        /// ShipDate
        /// </summary>
        public string ShipDate;
    };
    /// <summary>
    /// Update Ship Date
    /// </summary>
    public interface IUpdateShipDate
    {
        /// <summary>
        /// 获取Delivery表相关信息
        /// </summary>
        /// <param name="dn">dn</param>
        S_RowData_ShipDate GetDN(string dn);
        /// <summary>
        /// 更改Delivery表相关信息
        /// </summary>
        /// <param name="dn">dn</param>
        /// <param name="dnDate">dnDate</param>
        /// <param name="editor">editor</param>
        void UpDN(string dn, string dnDate, string editor);
    }
}
