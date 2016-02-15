using System;
using System.Data;
using System.Collections.Generic;
using IMES.DataModel;

namespace IMES.Station.Interface.StationIntf
{
    /// <summary>
    /// IProductionPlan接口
    /// </summary>
    public interface IProductionPlan
    {
        /// <summary>
        /// select ID, PdLine, ShipDate, Family, Model, PlanQty, AddPrintQty, PrePrintQty, Editor, Cdt, Udt 
        ///from ProductPlan
        ///where PdLine = @line and
        ///ShipDate =@shipdate
        /// </summary>
        /// <param name="Line">Phyical Line</param>
        /// <param name="ShipDate">ShipDate</param>
        /// <returns></returns>
        IList<ProductPlanInfo> GetProductPlanByLineAndShipDate(string line, DateTime shipDate);

        /// <summary>
        /// declare @ProductPlanUpload TbProductPlan 
        ///  exec IMES_ProductPlanCheckInsert @ProductPlanUpload 
        /// </summary>
        /// <param name="ProdPlanList"> TVP parameter structure</param>
        IList<ProductPlanLog> UploadProductPlan(IList<TbProductPlan> prodPlanList, string combinedPO);

        IList<ProductPlanLog> GetProductPlanMOByLineAndShipDate(string line, DateTime shipDate, string station);

        IList<ProductPlanLog> GetProductPlanLogByLineAndShipDateAndAction(string line, DateTime shipDate, string action);

        IList<ProductPlanLog> UploadProductPlan_Revise(IList<TbProductPlan> ProdPlanList, string combinedPO);
    }
}
