// INVENTEC corporation (c)2012 all rights reserved. 
// Description: Change Model Interface
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// Known issues:
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Station.Interface.CommonIntf;
using IMES.DataModel;

namespace IMES.Station.Interface.StationIntf
{
    /// <summary>
    /// 
    /// </summary>
    public interface IChangeSamplePO
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sn1"></param>
        /// <param name="editor"></param>
        /// <param name="line"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        ArrayList InputSN1(string sn1, string editor, string line, string station, string customer);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sn1"></param>
        /// <param name="sn2"></param>
        /// <returns></returns>
        ArrayList InputSN2(string sn1, string sn2);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sn1"></param>
        /// <param name="selectStation"></param>
        /// <returns></returns>
        ArrayList Change(string sn1, string sn2);

        /// <summary>
        /// 取消workflow
        /// </summary>
        /// <param name="sessionKey"></param>
        void Cancel(string sessionKey);
    }
	
	[Serializable]
    public struct S_ChangeSamplePO_Prod
    {
        public string ProdId;
		public string CustSN;
		public string Model;
		public string PoNo;
		public string MO;
		public string Station;
	}
}
