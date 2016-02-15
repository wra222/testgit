using System.Collections.Generic;
using IMES.Station.Interface.CommonIntf;
using IMES.DataModel;
using System.Collections;

namespace IMES.Station.Interface.StationIntf
{

    /// </summary>
    public interface ICombineCartonandDNfor146_CommonParts
    {
        /// <summary>
        /// GetModelList
        /// </summary>
        /// <returns></returns>
        IList<string> GetModelList();

        /// <summary>
        /// GetDeliveryList
        /// </summary>
        /// <param name="model">string</param>
        /// <returns></returns>
        ArrayList GetDeliveryList(string model);

        /// <summary>
        /// GetRemainQtyList
        /// </summary>
        /// <param name="dn">string</param>
        /// <returns></returns>
        string GetRemainQty(string dn);

        /// <summary>
        /// InputPCSinCarton
        /// </summary>
        /// <param name="pdLine">string</param>
        /// <param name="input">string</param>
        /// <param name="dnno">string</param>
        /// <param name="model">string</param>
        /// <param name="editor">string</param>
        /// <param name="stationId">string</param>
        /// <param name="customerId">string</param>
        /// <param name="printItems">PrintItem</param>
        /// <returns></returns>
        ArrayList InputPCSinCarton(string pdLine, string input, string dnno, string model, string editor, string stationId, string customerId, IList<PrintItem> printItems);
        /// <summary>
        /// Reprint
        /// </summary>
        /// <param name="CartonNo"></param>
        /// <param name="reason"></param>
        /// <param name="printItems"></param>
        /// <param name="editor"></param>
        /// <param name="stationId"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        ArrayList RePrint(string CartonNo, string reason, IList<PrintItem> printItems, string editor, string stationId, string customer);
        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="sessionKey"></param>
        void Cancel(string sessionKey);
   }
}
