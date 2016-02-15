using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.Repository._Metas;

namespace IMES.DataModel
{
    [Serializable]
    public class ProductPlanFamily
    {
        public String Family { get; set; }
        public String Descr { get; set; }
        public String CustomerID { get; set; }
        public String Editor { get; set; }
        public DateTime Cdt { get; set; }
        public DateTime Udt { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class TbProductPlan
    {
        //public int ID { get; set; }
        public String PdLine { get; set; }
        public DateTime ShipDate { get; set; }
        public String Family { get; set; }
        public String Model { get; set; }
        public int PlanQty { get; set; }
        public int AddPrintQty { get; set; }
        public int PrePrintQty { get; set; }       
        //public string ErrDescr { get; set; }
        public String Editor { get; set; }
        public DateTime Cdt { get; set; }
        public DateTime Udt { get; set; }
        public string Pass { get; set; }
        public string PoNo { get; set; }

    }

    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class ProductPlanInfo:TbProductPlan
    {
        public int ID { get; set; }        
        public string ErrDescr { get; set; }
    }


    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class ProductPlanLog
    {
        public int ID { get; set; }
        public String Action { get; set; }
        public String PdLine { get; set; }
        public DateTime ShipDate { get; set; }
        public String Family { get; set; }
        public String Model { get; set; }
        public int PlanQty { get; set; }
        public int AddPrintQty { get; set; }
        public int RemainQty { get; set; }
        public int NonInputQty { get; set; }
        public int InputQty { get; set; }
        public string ErrDescr { get; set; }
        public String Editor { get; set; }
        public DateTime Cdt { get; set; }
        public DateTime Udt { get; set; }
        public string Pass { get; set; }
        public string PoNo { get; set; }

    }

    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class MOPlanInfo
    {
        public String MO { get; set; }
        public String Plant { get; set; }
        public String Model { get; set; }
        public DateTime CreateDate{get; set;}
        public DateTime StartDate { get; set; }
        public int Qty{get;set;}
        public String SAPStatus { get; set; }
        public int SAPQty {get; set;}
        public int Print_Qty { get; set; }
        public int Transfer_Qty { get; set; }
        public string Status { get; set; }
        public int CustomerSN_Qty { get; set; }
        public int PlanQty { get; set; }        
        public String Editor { get; set; }
        public DateTime Cdt { get; set; }
        public DateTime Udt { get; set; }
        public string PoNo { get; set; }
    }

    

}
