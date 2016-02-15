/*
 * INVENTEC corporation ?2011 all rights reserved. 
 * Description:Interface for SN Check Page
 * UI:CI-MES12-SPEC-PAK-UC Combine DN & Pallet for BSam –2013/02/19    
 * UC:CI-MES12-SPEC-PAK-UC Combine DN & Pallet for BSam –2013/02/19            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
* 2013-02-19   Benson  Chen   
* Known issues:
* TODO：
* UC 具体业务：
* UC 具体业务：
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Station.Interface.CommonIntf;
using IMES.DataModel;

namespace IMES.Station.Interface.BSamIntf
{
    //[Serializable]
    //public struct S_BSam_Product
    //{
    //    public string ProductID;
    //    public string CustomerSN;
    //    public string Model;
    //    public string Location;

    //}
    /// <summary>
    /// ICartonWeight
    /// </summary>
    public interface ICartonWeight
    {
        /// <summary>
        /// 刷第一个SN时，调用该方法启动工作流，根据输入CustSN获取DN List,Carton No,成功后调用InputCustSn
        /// 将custSN放到Session.CustSN中(string)
        /// 返回ArrayList
        /// </summary>
        /// <param name="firstSn">firstSn</param>
        /// <param name="line">line</param>
        /// <param name="editor">editor</param>
        /// <param name="station">station(Station="85A")</param>
        /// <param name="customer">customer</param>
        /// <returns>ArrayList</returns>
        ArrayList InputCustSn(string custSn, string line, string editor, string station, string customer,string weight);

        void Save(string custSn);
        ArrayList SaveForTablet(string custSn, IList<PrintItem> printItems);
        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="sessionKey"></param>
        void Cancel(string custSn);
    }
   

}