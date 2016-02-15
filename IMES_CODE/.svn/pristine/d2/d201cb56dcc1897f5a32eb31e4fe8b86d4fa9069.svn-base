using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMES.Station.Interface.StationIntf
{
    public interface ICombineLCMandBTDL_TPDL
    {
        /// <summary>
        /// Check TpdlLcmCT#
        /// </summary>
        /// <param name="lcmCTNum"></param>
        /// <param name="editor"></param>
        /// <param name="stationId"></param>
        /// <param name="customerId"></param>
        void CheckTpdlLcmCT(string lcmCTNum, string editor, string stationId, string customerId);
      
        /// <summary>
        /// Check BtdlLcmCT#
        /// </summary>
        /// <param name="lcmCTNum"></param>
        /// <param name="editor"></param>
        /// <param name="stationId"></param>
        /// <param name="customerId"></param>
        void CheckBtdlLcmCT(string lcmCTNum, string editor, string stationId, string customerId);
        /// <summary>
        /// Check Lcm VendorSn
        /// </summary>
        /// <param name="lcmCTNum"></param>
        /// <param name="lcmVendorSn"></param>
        void CheckLcmVendorSn(string lcmCTNum,
            string lcmVendorSn);
        //void CheckBtdlSn(string lcmCTNum, string btdlSn);
        //void CheckTpdlSn(string lcmCTNum, string tpdlSn);

        /// <summary>
        /// Combine BTDL
        /// </summary>
        /// <param name="lcmCTNum"></param>
        /// <param name="btdlSn"></param>
        void CombineBTDL(string lcmCTNum, string btdlSn, string pCode);

        /// <summary>
        /// Combine TPDL
        /// </summary>
        /// <param name="lcmCTNum"></param>
        /// <param name="tpdlSn"></param>
        void CombineTPDL(string lcmCTNum, string tpdlSn, string pCode);

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="sessionKey"></param>
        void Cancel(string sessionKey);
    }
}
