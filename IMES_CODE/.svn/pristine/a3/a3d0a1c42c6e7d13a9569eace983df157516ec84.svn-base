using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using IMES.WS.Common;
namespace IMES.WS.MORelease
{
     public class MoItemDetail 
    {
        public string SerialNumber;
        public string MoNumber;
        public string MoItem;
        public string Reservation;
        public string ResvItem;
        public string Component;
        public string UnitReqQty;
        public string ReqQty;                  // total MO need Qty, not per unit QTY 
        public string WithdrawQty;
        public string Unit;
        public string AltGroup;
        public string Pantom;
        public string Bulk;
        public string Delete;
        public string FinalIssue;
        public string SpecialStock;
        public string MN;
        public string ParentMaterial;
        public MoItemDetail() { }
        public MoItemDetail(MoItemDetail itemdetail)
        { ObjectTool.CopyObject(itemdetail, this); }
     
        //protected List<string> NotNullItemList = new List<string> { "TxnId", "MoNumber", "MoItem", "MoNumber", "Reservation", "Component", "ReqQty", "WithdrawQty", "Unit", };
      
    }
    public class MoReleaseResponse
    {
        public string MoNumber;
        public string Result;
        public string FailDescr;
    }
    public class MoHeader 
    {
        public string SerialNumber;
        public string Plant;
        public string MoNumber;
        public string CreateDate;
        public string Status;
        public string MoType;
        public string BuildOutMtl;
        public string MaterialGroup;
        public string MaterialType;
        public string BasicStartDate;
        public string BasicFinishDate;
        public string TotalQty;
        public string DeliveredQty;
        public string Unit;
        public string ProductionVer;
        public string Priority;
        public string BOMStatus;
        public string BOMExplDate;
        public string SalesOrder;
        public string SOItem;
        public string TCode;
        public string Remark1;
        public string Remark2;
        public string Remark3;
        public string Remark4;
        public string Remark5;
        public MoHeader() { }
        public MoHeader(MoHeader header)
        {
            ObjectTool.CopyObject(header, this);
         
        }
           
    }

    public  class DBMoHeader: MoHeader
    {
        public DBMoHeader() { }
        public DBMoHeader(MoHeader header):base(header){ }
        public string IsProduct;
        private List<string> _NotNullItemList = new List<string>
               { 
                   "SerialNumber","Plant","MoNumber","CreateDate","Status","MoType",
                   "BuildOutMtl","MaterialGroup","MaterialType","BasicStartDate",
                   "BasicFinishDate","TotalQty","DeliveredQty",
                   "Unit","ProductionVer", 
                   "BOMStatus","BOMExplDate", "TCode"
                   };
        public List<string> NotNullItemList
        { get { return _NotNullItemList; } }
    }
    public  class DBMoItemDetail : MoItemDetail
    {
        public DBMoItemDetail() { }
        public DBMoItemDetail(MoItemDetail itemdetail): base(itemdetail)
        {  }
        private List<string> _NotNullItemList = new List<string> { "SerialNumber", 
                                                                                                    "MoNumber", 
                                                                                                    "MoItem", 
                                                                                                    "Reservation", 
                                                                                                    "ResvItem", 
                                                                                                    "Component", 
                                                                                                    "UnitReqQty",
                                                                                                    "ReqQty", 
                                                                                                    "WithdrawQty", 
                                                                                                    "Unit" };
         public List<string> NotNullItemList
        { get { return _NotNullItemList; } }
        public int Group = 0;
        public bool HasAltGroup=false;
        //public int UnitReqQty = 0;
    }
   

}
