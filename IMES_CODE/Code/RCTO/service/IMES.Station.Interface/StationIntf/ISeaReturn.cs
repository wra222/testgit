using System.Collections.Generic;
using IMES.Station.Interface.CommonIntf;
using IMES.DataModel;
using System.Collections;

namespace IMES.Station.Interface.StationIntf
{

    /// </summary>
    public interface ISeaReturn
    {

        /// <summary>
        /// InputCT
        /// </summary>
        /// <param name="ct">string</param>
        /// <param name="returnType">string</param>
		/// <param name="pdLine">string</param>
        /// <param name="editor">string</param>
        /// <param name="stationId">string</param>
        /// <param name="customerId">string</param>
        /// <param name="printItems">PrintItem</param>
        /// <returns></returns>
        ArrayList InputCT(string ct, string returnType, string pdLine, string editor, string stationId, string customerId, IList<PrintItem> printItems);
        
        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="sessionKey"></param>
        void Cancel(string sessionKey);
   }
}
