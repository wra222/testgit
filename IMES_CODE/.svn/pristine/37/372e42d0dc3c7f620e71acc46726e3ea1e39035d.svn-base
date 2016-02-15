// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2010-04-15   Lucy Liu                create
// Known issues:
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Station.Interface.CommonIntf;
using IMES.DataModel;
using System.Collections;

namespace IMES.Station.Interface.StationIntf
{
    public interface ILightGuide
    {

      
        /// <summary>
        /// 根据KittingCode获取料位信息
        /// </summary>
        /// <param name="code">Kitting code</param>
        /// <returns>料位信息列表</returns>
        IList<LightBomInfo> getBomByCode(string code);


        /// <summary>
        /// 获取到料位信息是根据model和code一块查询出来的
        /// </summary>
        /// <param name="model">model</param>
        /// <param name="code">code</param>
        /// <returns>料位信息列表</returns>
        IList<LightBomInfo> getBomByModel(string model, out string code);

        /// <summary>
        /// 对输入的ctNo进行partMatch,返回partNo
        /// </summary>
        /// <param name="ctNo">ct no</param>
        /// <returns>part no</returns>
        string checkCTNo(string ctNo);


    }
}

