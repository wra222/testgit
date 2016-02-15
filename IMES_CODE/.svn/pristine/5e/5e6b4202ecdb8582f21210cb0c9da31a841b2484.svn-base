// INVENTEC corporation (c)2010 all rights reserved. 
// Description:  ITPCBCollection Station Interface
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2010-04-17   Chen Xu (eB1-4)              create
// Known issues:

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Station.Interface.CommonIntf;
using IMES.DataModel;


namespace IMES.Station.Interface.StationIntf
{
    /// <summary>
    /// TPCB Collection
    /// </summary>
    public interface ITPCBCollection
    {
        #region methods interact with the running workflow

        /// <summary>
        /// 【保存】或【更新】TPCBInfo信息
        /// </summary>
        /// <param name="family">family</param>
        /// <param name="pdline">pdline</param>
        /// <param name="type">type</param>
        /// <param name="partno">partno</param>
        /// <param name="vendor">vendor</param>
        /// <param name="dcode">dcode</param>
        /// <param name="editor">editor</param>
        /// <param name="station">station</param>      
        /// <param name="customer">customer</param>
        /// <returns>返回TPCBInfo信息</returns>
        IList<TPCBInfo> SaveTPCB(string family, string pdline, string type, string partno, string vendor, string dcode, string editor, string station, string customer);


        /// <summary>
        /// 删除TPCB数据相关信息
        /// </summary>
        /// <param name="family">family</param>
        /// <param name="pdline">pdline</param>
        /// <param name="partno">partno</param>
        /// <param name="editor">editor</param>
        /// <param name="station">station</param>      
        /// <param name="customer">customer</param>
        void DeleteTPCB(string family, string pdline, string partno, string editor, string station, string customer);   


        /// <summary>
        /// 【保存】TPCBDet信息
        /// </summary>
        /// <param name="tpcbCode">tpcbCode</param>
        /// <param name="family">family</param>
        /// <param name="pdline">pdline</param>
        /// <param name="editor">editor</param>
        /// <param name="station">station</param>      
        /// <param name="customer">customer</param>
        void SaveTPCBDet(string tpcbCode, string family, string pdline, string editor, string station, string customer);    

        #endregion


        #region methods do not interact with the running workflow

        /// <summary>
        /// 获得FamilyInfo
        /// </summary>
        IList<FamilyInfo> GetFamilyList();

        /// <summary>
        /// 根据下拉框选择的family，取得Type下拉框信息
        /// </summary>
        /// <param name="family">family</param>
        /// <returns>Type</returns>
        IList<string> GetTypeList(string family);

        /// <summary>
        /// 根据下拉框选择的family和type, 取得PartNo下拉框信息
        /// </summary>
        /// <param name="family">family</param>
        /// <param name="type">type</param>
        /// <returns>PartNo</returns>
        IList<string> GetPartNoList(string family, string type);

        /// <summary>
        /// 根据下拉框选择的family和partno, 取得DCode信息
        /// </summary>
        /// <param name="family">family</param>
        /// <param name="partno">partno</param>
        /// <returns>DCode</returns>
        string GetDCode(string family, string partno);

        /// <summary>
        /// 根据下拉框选择的family和partno, 取得VendorSN信息
        /// </summary>
        /// <param name="family">family</param>
        /// <param name="partno">partno</param>
        /// <returns>VendorSN</returns>
        string GetVendorSN(string family, string partno);

        /// <summary>
        /// 显示全部TPCBInfo数据相关信息
        /// </summary>
        /// <param name="family">family</param>
        /// <param name="pdline">pdline</param>
        /// <returns>返回TPCBInfo信息</returns>
        IList<TPCBInfo> Query(string family, string pdline);
       
        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="sessionKey"></param>
        void Cancel(string sessionKey);

        #endregion

    }
}
