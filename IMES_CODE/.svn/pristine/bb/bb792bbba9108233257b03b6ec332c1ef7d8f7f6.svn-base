// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2009-11-12   Yuan XiaoWei                 create
// Known issues:
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;

namespace IMES.Station.Interface.StationIntf
{
    /// <summary>
    /// POData站接口定义，用于Delivery，Pallet信息的上传修改
    /// </summary>
    public interface IPOData
    {

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="uploadID"></param>
        /// <param name="editor"></param>
        /// <returns></returns>
        IList<DNForUI> UploadDNFile(string uploadID, string editor);

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="uploadID"></param>
        /// <param name="editor"></param>
        /// <returns></returns>
        IList<DNForUI> UpdateDNFile(string uploadID, string editor);

        string GetPAKConnectionString();

        /// <summary>
        /// 修改一条选中的DN
        /// </summary>
        /// <param name="UpdateDN"></param>
        /// <param name="editor"></param>
        void ModifyDN(DNUpdateCondition UpdateDN, string editor);

        /// <summary>
        /// 修改一条选中的DNPallet信息
        /// </summary>
        /// <param name="deliveryPalletID"></param>
        /// <param name="deliveryQty"></param>
        /// <param name="editor"></param>
        void ModifyDNPallet(int deliveryPalletID, short deliveryQty, string editor);

        /// <summary>
        /// 修改一条选中的DeliveryInfo信息
        /// </summary>
        /// <param name="deliverInfoID"></param>
        /// <param name="infoValue"></param>
        /// <param name="editor"></param>
        void ModifyDNInfo(int deliverInfoID, string infoValue, string editor);

        /// <summary>
        /// 保存修改后的DN信息
        /// </summary>
        /// <param name="deliveryNo"></param>
        /// <param name="deliverInfoID"></param>
        /// <param name="infoValue"></param>
        /// <param name="UpdateDN"></param>
        /// <param name="deliveryPalletID"></param>
        /// <param name="deliveryQty"></param>
        /// <param name="editor"></param>
        void Save(string deliveryNo,DNUpdateCondition UpdateDN, int deliverInfoID, string infoValue, int deliveryPalletID, short deliveryQty, string editor);

        /// <summary>
        /// 根据查询条件获取符合条件的Delivery列表
        /// </summary>
        /// <param name="MyCondition"></param>
        /// <returns></returns>
        IList<DNForUI> getDNList(DNQueryCondition MyCondition);

        /// <summary>
        /// 根据DN获取DNInfoForUI列表
        /// </summary>
        /// <param name="dn"></param>
        /// <returns></returns>
        IList<DNInfoForUI> getDNInfoList(string dn);

        /// <summary>
        /// 根据DN获取Pallet列表
        /// </summary>
        /// <param name="dn"></param>
        /// <returns></returns>
        IList<DNPalletQty> getPalletList(string dn);

        /// <summary>
        /// 根据DN删除Delivery,DeliveryInfo,Delivery_Pallet表的相关记录
        /// </summary>
        /// <param name="dn"></param>
        /// <returns></returns>
        void deleteDN(string dn, string line, string editor, string station, string customer);


    }
}
