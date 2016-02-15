// INVENTEC corporation (c)2009 all rights reserved. 
// Description: TravelCard Print Interface
// 根据产线生产安排，打印travel card
// 分两种模式：
// A.不做管控，按照选定的Model 打印；将打印出来的ProdId label 贴在travel card上，开始online作业
// B.在输入时需要根据MO进行管控，Travel card上会同时打印ProdId，开始online作业 
//               
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2009-10-22   Yuan XiaoWei                 create
// Known issues:
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Station.Interface.CommonIntf;
using IMES.DataModel;
using System.Collections;
using System.Data;
using IMES.FisObject.Common;
using IMES.FisObject.Common.PrintLog;
namespace IMES.Station.Interface.StationIntf
{
    /// <summary>
    /// 根据产线生产安排，打印travel card
    /// 分两种模式：
    /// A.	不做管控，按照选定的Model 打印；将打印出来的ProdId label 贴在travel card上，开始online作业
    /// B.	在输入时需要根据MO进行管控，Travel card上会同时打印ProdId，开始online作业
    /// 目的：打印travel card，作流线管控使用
    /// </summary>
    /// 
    [Serializable]
    public class SerialNumber
    {
        public string beginNumber { get; set; }
        public string endNumber { get; set; }
    }


    public interface ITravelCardPrint
    {
        /// <summary>
        /// 根据产线生产安排，打印travel card
        /// A.	不做管控，按照选定的Model 打印；将打印出来的ProdId label 贴在travel card上，开始online作业
        /// 目的：打印travel card，作流线管控使用
        /// </summary>
        /// <param name="pdLine">Pd Line</param>
        /// <param name="model">Product Model</param>
        /// <param name="qty">打印数量</param>
        /// <param name="editor">operator</param>
        /// <param name="station">station</param>
        /// <param name="customer">customer</param>
        /// <returns>Print Items</returns>
        IList<PrintItem> PrintTCNoProductID(
            string pdLine,
            string model,
            int qty,
            string editor, string stationId, string customer, IList<PrintItem> printItems);

        //IList<PrintItem> PrintTCWithProductIDForBN(
        //    string pdLine,
        //    string mo,
        //    int qty,
        //    bool IsNextMonth,
        //    string editor, string station, string customer,
        //    out IList<string> startProdIdAndEndProdId, IList<PrintItem> printItems);




        IList<PrintItem> PrintTCWithProductIDForBN(
            string pdLine,
            string mo,
            int qty,
            bool IsNextMonth,
            string editor, string station, string customer,
            out IList<string> startProdIdAndEndProdId, IList<PrintItem> printItems, out string battery, out string lcm, string deliveryDate, string sku);
        


        /// <summary>
        /// 根据产线生产安排，打印travel card
        /// B.	在输入时需要根据MO进行管控，Travel card上会同时打印ProdId，开始online作业
        /// 目的：打印travel card，作流线管控使用
        /// </summary>
        /// <param name="pdLine">Pd Line</param>
        /// <param name="mo">Product MO</param>
        /// <param name="qty">打印数量</param>
        /// <param name="ecr">ECR</param>
        /// <param name="month">月</param>
        /// <param name="editor">operator</param>
        /// <param name="station">station</param>
        /// <param name="customer">customer</param>
        /// <param name="startProdIdAndEndProdId">返回起始和结束的ProdId</param>
        /// <returns>Print Items</returns>
        IList<PrintItem> PrintTCWithProductID(
            string pdLine,
            string mo,
            int qty,
            string ecr,
            bool IsNextMonth,
            string editor, string station, string customer,
            out IList<string> startProdIdAndEndProdId, IList<PrintItem> printItems);

        /// <summary>
        /// 重印带ProdId的Travel Card
        /// </summary>
        /// <param name="mo">mo</param>
        /// <param name="startProdId">Start ProdId</param>
        /// <param name="endProdId">End ProdId</param>
        /// <param name="editor">operator</param>
        /// <param name="station">station</param>
        /// <param name="customer">customer</param>
        /// <returns>Print Items</returns>
        ArrayList ReprintLabel(
            string mo,
            string startProdId,
            string endProdId,
            string pCode,
            string editor, string station, string customer,
            string reason, IList<PrintItem> printItems);

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

        /// <summary>
        /// 获取Family列表
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        IList<IMES.DataModel.FamilyInfo> GetFamilyList(string customer);
        DataTable GetPrintLogProIdListByDataTable(string mo);
      //  IList<string> GetPrintLogProIdListByList(string mo);
        IList<IMES.DataModel.ProdIdRangeInfo> GetPrintLogProdIdRangeList(string mo);
        List<SerialNumber> GetPrintLogProidList(string mo);
        string GetSKU(string model);
        ArrayList ReprintTravelCard(string prodid, string editor, string station, string customer, string reason, string pCode, IList<PrintItem> printItems);
    }
}
