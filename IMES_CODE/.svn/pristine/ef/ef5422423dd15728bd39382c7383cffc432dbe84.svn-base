/*
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:Web method for UploadShipmentData Page
 *             
 * UI:CI-MES12-SPEC-PAK-UI Upload Shipment Data to SAP.docx –2011/10/26 
 * UC:CI-MES12-SPEC-PAK-UC Upload Shipment Data to SAP.docx –2011/10/26            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2011-11-11  itc202017             (Reference Ebook SourceCode) Create
 * Known issues:
*/

using System;
using System.Collections.Generic;
using IMES.Station.Interface.CommonIntf;
using IMES.DataModel;
using System.Collections;


namespace IMES.Station.Interface.StationIntf
{
    
    /// <summary>
    /// Row data define of UploadShipmentData page
    /// </summary>
    [Serializable]
    public struct S_RowData_UploadShipmentData
    {
        /// <summary>
        /// Ship Date
        /// </summary>
        public DateTime m_date;
        /// <summary>
        /// Delivery No.
        /// </summary>
        public string m_dn;
        /// <summary>
        /// Po No.
        /// </summary>
        public string m_pn;
        /// <summary>
        /// Model
        /// </summary>
        public string m_model;
        /// <summary>
        /// Qty
        /// </summary>
        public int m_qty;
        /// <summary>
        /// Status
        /// </summary>
        public string m_status;
        /// <summary>
        /// Pack
        /// </summary>
        public int m_pack;
        /// <summary>
        /// PAQC
        /// </summary>
        public string m_paqc;
        /// <summary>
        /// If allow upload
        /// </summary>
        public bool m_bAllowUpload;
        /// <summary>
        /// Udt
        /// </summary>
        public DateTime m_udt;
        /// <summary>
        /// Flag=C N
        /// </summary>
        public string m_dnflag;

    };

   
    /// <summary>
    /// Data Struct DN and Udt
    /// </summary>
    [Serializable]
    public struct S_DnUdt
    {
        /// <summary>
        /// dn
        /// </summary>
        public string dn;
        /// <summary>
        /// udt
        /// </summary>
        public DateTime udt;
    };

    /// <summary>
    /// Upload Shipment Data to SAP
    /// </summary>
    public interface IUploadShipmentData
    {
        /// <summary>
        /// 获取Delivery相关信息
        /// </summary>
        /// <param name="fromDate">Shipdate from</param>
        /// <param name="toDate">Shipdate to</param>
        /// <param name="bAllData">All data</param>
        IList<S_RowData_UploadShipmentData> GetTableData(DateTime fromDate, DateTime toDate, string status);

        /// <summary>
        /// 获取Delivery相关信息 by DN list
        /// </summary>
        /// <param name="dnList">DN List</param>
        /// <param name="bAllData">All data</param>
        IList<S_RowData_UploadShipmentData> GetTableDataByDnList(string dnList, string status);

        /// <summary>
        ///  Upload
        /// </summary>
        /// <param name="dns">UploadDNInfo list</param>
        ArrayList  Upload(string station,string userId,string customer,IList<UploadDNInfo> lstUploadDNInfo);


        /// <summary>
        ///  Upload
        /// </summary>
        /// <param name="dns">UploadDNInfo list</param>
        ArrayList Upload(MpUserInfo mu, IList<UploadDNInfo> lstUploadDNInfo);
        /// <summary>
        /// 获取Delivery相关信息
        /// </summary>
        /// <param name="fromDate">Shipdate from</param>
        /// <param name="toDate">Shipdate to</param>
        /// <param name="bAllData">All data</param>
        IList<S_RowData_UploadShipmentData> GetTableData(DateTime fromDate, DateTime toDate, bool bAllData);

        /// <summary>
        /// 获取记入c:\serial.txt的信息
        /// </summary>
        /// <param name="dns">DN list</param>
        /// <param name="editor">Editor</param>
        IList<string> GetFileData(IList<S_DnUdt> dns, string editor);

        /// <summary>
        /// 成功生成文件后，每个Delivery 需要更新状态
        /// </summary>
        /// <param name="dns">DN list</param>
        void ChangeDNStatus(IList<string> dns);

        /// <summary>
        /// 获取Delivery相关信息 by DN list
        /// </summary>
        /// <param name="dnList">DN List</param>
        /// <param name="bAllData">All data</param>
        IList<S_RowData_UploadShipmentData> GetTableDataByDnList(string dnList, bool bAllData);

        void CheckDnStatus(IList<S_DnUdt> dns);

    }
}
