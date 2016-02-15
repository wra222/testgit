using System.Collections.Generic;
using IMES.Station.Interface.CommonIntf;
using IMES.DataModel;
using System.Collections;

namespace IMES.Station.Interface.StationIntf
{
    /// <summary>
    /// 
    /// 
    /// </summary>
    public interface ITravelCardWithCollectKP
    {
        /// <summary>
        /// 输入ProdId和相关信息
        /// </summary>
        /// <param name="pdLine">Pd Line</param>
        /// <param name="prodId">Product Id</param>
        /// <param name="editor">operator</param>
        ArrayList InputCT(
            string ctNo,
            string pdLine,
            string editor, string stationId, string customerId, string model, IList<PrintItem> printItems);



        IList<PrintItem> PrintCustsnLabel(IList<PrintItem> printItems, string custsn);
      
        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="sessionKey"></param>
        void Cancel(string sessionKey);

        /// <summary>
        /// </summary>
        ArrayList RePrint(string sn, string reason, string line, string editor, string station, string customer, IList<PrintItem> printItems);

    }
}
