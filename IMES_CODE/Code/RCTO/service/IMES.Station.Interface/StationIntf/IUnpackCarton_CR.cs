﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;
using IMES.Station.Interface.CommonIntf;
using System.Data;

namespace IMES.Station.Interface.StationIntf
{
    /// <summary>
    /// Unpack Carton
    /// </summary>
    public interface IUnpackCarton_CR
    {

        #region methods interact with the running workflow

        
        ///<summary>
        /// 解除Carton的绑定
        /// 使用工作流070UnpackCarton.xoml
        /// </summary>
        /// <param name="cartonNo"></param>
        /// <param name="line"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        void UnpackCarton(string cartonNo, string line, string editor, string station, string customer);

        ///<summary>
        /// 解除DN的绑定
        /// 使用工作流070UnpackDN.xoml
        /// </summary>
        /// <param name="cartonNo"></param>
        /// <param name="line"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        void UnpackDN(string cartonNo, string line, string editor, string station, string customer);

         ///<summary>
        /// 解除Pallet的绑定
        /// 使用工作流070UnpackPallet.xoml
        /// </summary>
        /// <param name="pallet"></param>
        /// <param name="line"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        void UnpackPallet(string pallet, string line, string editor, string station, string customer);
       
        ///<summary>
        /// 根据指定的DeliveryNo解除绑定
        /// 使用工作流070UnpackDNByDN.xoml
        /// </summary>
        /// <param name="deliveryNo"></param>
        /// <param name="bSuperUI"></param>
        /// <param name="line"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        void UnpackDNByDN(string deliveryNo, bool bSuperUI, string line, string editor, string station, string customer);

        /// 使用工作流UnpackSNByAll.xoml  tmp 
        /// </summary>
        /// <param name="prodSn"></param>
        /// <param name="line"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
         void  UnpackAllBySNCheck(string prodSn, string line, string editor, string station, string customer);

        ///<summary>
        /// 解除Dn的绑定通过Sn or Product id
        /// 使用工作流070UnpackDNbySN.xoml
        /// </summary>
        /// <param name="prodSn"></param>

        string UnpackAllbySNSave(string prodSn);

        ///<summary>
        /// 解除Dn的绑定通过Sn or Product id
        /// 使用工作流070UnpackDNbySN.xoml
        /// </summary>
            /// <param name="prodSn"></param>
        /// <param name="line"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        void UnpackDNbySNCheck(string prodSn, string line, string editor, string station, string customer);

        ///<summary>
        /// 解除Dn的绑定通过Sn or Product id
        /// 使用工作流070UnpackDNbySN.xoml
        /// </summary>
        /// <param name="prodSn"></param>

        string UnpackDNbySNSave(string prodSn,bool isPallet);
        ///<summary>
        /// 解除Dn的绑定通过Sn or Product id
        /// 使用工作流070UnpackDNbySN.xoml
        /// </summary>
        /// <param name="prodSn"></param>

        ///<summary>
        /// 根据指定的DummyPalletNo解除绑定
        /// 使用工作流UnpackDummyPalletNo.xoml
        /// </summary>
        /// <param name="DummyPalletNo"></param>
        /// <param name="line"></param>
        /// <param name="editor"></param>
        /// <param name="customer"></param>
        void UnpackDummyPalletNo(string DummyPalletNo, string line, string editor, string customer);


        void Cancel(string prodSn);

        #endregion


    }
}