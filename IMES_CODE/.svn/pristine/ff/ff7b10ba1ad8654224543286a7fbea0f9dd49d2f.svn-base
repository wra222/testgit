/*
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:Web method for WHPalletControl Page            
 * CI-MES12-SPEC-PAK W/H Pallet Control.docx
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
    /// check input re
    /// </summary>
    public struct S_CheckInput_Re
    {
        /// <summary>
        /// plt
        /// </summary>
        public string plt;
        /// <summary>
        /// found
        /// </summary>
        public bool found;
    };
    [Serializable]
    /// <summary>
    /// Row data define of DN Data Extended Property
    /// </summary>
    public struct S_DN_Extended
    {
        /// <summary>
        /// success  
        /// </summary>
        public int success;
        /// <summary>
        /// Consolidated
        /// </summary>
        public string consolidated;
        /// <summary>
        /// BOL
        /// </summary>
        public string bol;
        /// <summary>
        /// EmeaCarrier
        /// </summary>
        public string emeaCarrier;
        /// <summary>
        /// Carrier
        /// </summary>
        public string carrier;
        /// <summary>
        /// ModelName
        /// </summary>
        public string modelName;
        /// <summary>
        /// regId
        /// </summary>
        public string regId;
    };
    [Serializable]
    /// <summary>
    /// table data
    /// </summary>
    public struct S_Table_Data
    {
        /// <summary>
        /// DeliveryNO  
        /// </summary>
        public string DeliveryNO;
        /// <summary>
        /// Model
        /// </summary>
        public string Model;
        /// <summary>
        /// PalletNO
        /// </summary>
        public string PalletNO;
        /// <summary>
        /// Qty
        /// </summary>
        public string Qty;
        /// <summary>
        /// Forwarder
        /// </summary>
        public string Forwarder;
        /// <summary>
        /// HAWB
        /// </summary>
        public string HAWB;
        /// <summary>
        /// Satus
        /// </summary>
        public string Satus;
        /// <summary>
        /// LOC
        /// </summary>
        public string LOC;
        
    };
    [Serializable]
    /// <summary>
    /// common ret
    /// </summary>
    public struct S_common_ret
    {
        /// <summary>
        /// state 1 0 %,success,2 0 % fail,3 no data
        /// </summary>
        public int state;
        /// <summary>
        /// describe
        /// </summary>
        public string describe;
    };
    /// <summary>
    /// WHPalletControl
    /// </summary>
    public interface IWHPalletControl
    {
        /// <summary>
        /// 获取plt信息
        /// </summary>
        /// <param name="input">input</param>
        S_CheckInput_Re CheckInput(string input);
        /// <summary>
        /// 获取plt in信息 plt=pltno
        /// </summary>
        /// <param name="plt">plt</param>
        bool CheckIn(string plt);
        /// <summary>
        /// 获取plt in信息 plt=pltno
        /// </summary>
        /// <param name="plt">plt</param>
        bool CheckRW(string plt);
        /// <summary>
        /// 获取plt in信息 plt=pltno
        /// </summary>
        /// <param name="plt">plt</param>
        bool CheckExist(string plt);
         /// <summary>
        /// 获取plt out信息 plt=pltno
        /// </summary>
        /// <param name="plt">plt</param>
        bool CheckOut(string plt);
        /// <summary>
        /// 获取Qty信息
        /// </summary>
        /// <param name="plt">plt</param>
        int GetQty(string plt);
        /// <summary>
        /// 获取DN信息
        /// </summary>
        /// <param name="plt">plt</param>
        S_DN_Extended GetDN(string plt);
        /// <summary>
        /// not AssignDelivery return > 0
        /// </summary>
        /// <param name="plt">plt</param>
        /// <param name="editor">editor</param>
        int NonAssignDelivery(string plt, string editor);
        /// <summary>
        /// OrderPltType
        /// </summary>
        /// <param name="plt">plt</param>
        S_common_ret OrderPltType(string plt);
        /// <summary>
        /// AssignBol
        /// </summary>
        /// <param name="plt">plt</param>
        /// <param name="editor">editor</param>
        S_common_ret AssignBol(string plt, string editor);
        /// <summary>
        /// AssignPallet
        /// </summary>
        /// <param name="plt">plt</param>
        /// <param name="editor">editor</param>
        S_common_ret AssignPallet(string plt, string editor);
        /// <summary>
        /// RemovePallet
        /// </summary>
        /// <param name="plt">plt</param>
        /// <param name="editor">editor</param>
        void RemovePallet(string plt, string editor);
        /// <summary>
        /// GetPalletCount
        /// </summary>
        int GetPalletCount();
        /// <summary>
        /// Get7Days
        /// </summary>
        IList<S_Table_Data> Get7Days();
        /// <summary>
        /// GetDateFromTo
        /// </summary>
        /// <param name="begin">begin</param> 
        /// <param name="end">end</param>  
        IList<S_Table_Data> GetDateFromTo(DateTime begin, DateTime end);
        /// <summary>
        /// GetDateFromToNotIN
        /// </summary>
        /// <param name="begin">begin</param> 
        /// <param name="end">end</param>  
        IList<S_Table_Data> GetDateFromToNotIN(DateTime begin, DateTime end);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="palletNo"></param>
        /// <param name="editor"></param>
        /// <param name="line"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        void UpdatePalletWH(string palletNo, string editor, string line, string station, string customer);
        /// <summary>
        /// 获取plt in信息 plt=pltno
        /// </summary>
        /// <param name="plt">plt</param>
        bool CheckDT(string plt);
    }
   
}
