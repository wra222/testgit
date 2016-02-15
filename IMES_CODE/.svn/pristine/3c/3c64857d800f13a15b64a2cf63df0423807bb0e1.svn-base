using System;
using System.Collections.Generic;
using IMES.DataModel;
using System.Collections;

namespace IMES.Station.Interface.StationIntf
{
    /// <summary>
    /// </summary>
    /// 
    [Serializable]
    public class SerialNumber2012
    {
        public string beginNumber { get; set; }
        public string endNumber { get; set; }
    }


    public interface ITravelCardPrint2012
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pdLine"></param>
        /// <param name="mo"></param>
        /// <param name="qty"></param>
        /// <param name="IsNextMonth"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <param name="startProdIdAndEndProdId"></param>
        /// <param name="printItems"></param>
        /// <param name="battery"></param>
        /// <param name="lcm"></param>
        /// <param name="deliveryDate"></param>
        /// <param name="bomremark"></param>
        /// <param name="remark"></param>
        /// <param name="exception"></param>
        /// <returns></returns>
        IList<PrintItem> PrintTCWithProductIDForBN(
            string pdLine, string model, string mo, int qty, bool IsNextMonth, string editor, string station, string customer,
            out IList<string> startProdIdAndEndProdId, IList<PrintItem> printItems, out string battery, out string lcm, string deliveryDate,
            string bomremark, string remark, string exception);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="prodid"></param>
        /// <param name="reason"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <param name="pCode"></param>
        /// <param name="printItems"></param>
        /// <returns></returns>
        ArrayList ReprintTravelCard(string prodid, string reason, string editor, string station, string customer, string pCode, IList<PrintItem> printItems);


        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="sessionKey"></param>
        void Cancel(string sessionKey);

        /// <summary>
        /// 获取Model列表
        /// </summary>
        /// <param name="family"></param>
        /// <returns></returns>
        IList<ModelInfo> GetModelList(string family);
        IList<MOInfo> GetMOList(string modelId);

        

        /// <summary>
        /// 获取Family列表
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        IList<IMES.DataModel.FamilyInfo> GetFamilyList(string customer);
        
        //  IList<string> GetPrintLogProIdListByList(string mo);

        List<SerialNumber2012> GetPrintLogProidList(string mo);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IList<ConstValueInfo> GetExList();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool CheckBTByModel(string model);

        bool CheckModel(string family, string model);

        string GetProductID(string LCMCT);

    }
}
