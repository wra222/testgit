using System;
using System.Collections.Generic;
using IMES.DataModel;
using System.Collections;

namespace IMES.Station.Interface.StationIntf
{

    public interface ITravelCardPrintProductPlan
    {
        /// <summary>
        /// SELECT distinct a.[Family]
        /// ,a.[Descr]
        ///,a.[CustomerID]
        ///FROM [Family] a, ProductPlan b, MO c
        ///Where a.Family = b.Family 
        /// and b.PdLine = left(@PdLine,1) 
        /// and b.ShipDate=@ShipDate
        /// and GetPlanMONo(b.ID, b.PdLine) = c.MO 
        ///and c.Qty > c.Print_Qty 
        /// and c.[Status] = 'H'
        /// </summary>
        /// <param name="line"></param>
        /// <param name="shipdate"></param>
        IList<ProductPlanFamily> GetProductPlanFamily(string line, DateTime shipdate);

        /// <summary>
        /// SELECT a.[Model], b.[ID]
        ///FROM [Model] a, ProductPlan b, MO c
        ///Where a.Family = @Family 
        ///      and a.Model = b.Model
        ///      and b.PdLine = left(@PdLine,1) 
        ///      and b.ShipDate=@ShipDate
        ///      and GetPlanMONo(b.ID, b.PdLine) = c.MO 
        ///      and c.Qty > c.Print_Qty 
        ///      and c.[Status] = 'H' 
        ///Order by a.[Model]
        /// </summary>
        /// <param name="line"></param>
        /// <param name="shipdate"></param>
        /// <param name="Model"></param>
        /// <returns></returns>

        IList<ProductPlanInfo> GetProductPlanModel(string line, DateTime shipdate, string family);

        /// <summary>
        /// SELECT a.*, b.PlanQty 
        ///FROM  MO a, ProductPlan b
        ///Where b.[ID] = @ID and 
        ///      a.MO= dbo. GetPlanMONo(b.ID, b.PdLine)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        MOPlanInfo GetPlanMO(int id);


        /// <summary>
        ///
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        bool CheckedBSamModel(string Model);

        bool CheckedC_SKU(string Model, string Name);

        bool CheckedBSamModelAndC_SKU(string Model, string Name);

        IList<PilotMoInfo> GetPilotMo(PilotMoInfo condition, IList<string> combinedState);

    }
}
