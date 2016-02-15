// INVENTEC corporation (c)2009 all rights reserved. 
// Description: UnpackCombinePizza
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 
// Known issues:
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;
using IMES.Station.Interface.CommonIntf;
using System.Data;

namespace IMES.Station.Interface.StationIntf
{
    /// <summary>
    /// UnpackCombinePizza
    /// </summary>
    public interface IUnpackCombinePizza
    {

        
        ///<summary>
        /// 解除绑定
        /// 使用工作流 UnpackCombinePizza.xoml
        /// </summary>
		/// <param name="custSn"></param>
        /// <param name="cartonNo"></param>
        /// <param name="line"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        ArrayList Unpack(string custSn, string cartonNo, string line, string editor, string station, string customer);


        void Cancel(string prodSn);


    }
}
