// INVENTEC corporation (c)2012 all rights reserved. 
// Description: Change Model Interface
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2012-06-15   Yuan XiaoWei                 create
// Known issues:
using System.Collections.Generic;
using IMES.DataModel;
using System.Data;

namespace IMES.Station.Interface.StationIntf
{
    /// <summary>
    /// 
    /// </summary>
    public interface IChangeModel
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model1"></param>
        /// <param name="editor"></param>
        /// <param name="line"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        List<StationDescrQty> InputModel1(string model1, string editor, string line, string station, string customer);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="model1"></param>
        /// <param name="model2"></param>
        /// <returns></returns>
        void InputModel2(string model1, string model2);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="model1"></param>
        /// <param name="selectStation"></param>
        /// <returns></returns>
        List<ProductModel> Change(string model1, string selectStation, int changeQty);

        /// <summary>
        /// 取消workflow
        /// </summary>
        /// <param name="sessionKey"></param>
        void Cancel(string sessionKey);
    }
}
