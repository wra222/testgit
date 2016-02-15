using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace UTL.MetaData
{       
    [Table(Name = "ReceivedKeyInfo")]
    public class ReceivedKeyInfo
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true)]
        public long ID;
        [Column(DbType = "UniqueIdentifier", UpdateCheck = UpdateCheck.Never)]
        public Guid OrderUniqueID { get; set; }
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string SoldToCustomerID { get; set; }
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string ShipToCustomerID { get; set; }
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string PONumber { get; set; }
        [Column(UpdateCheck = UpdateCheck.Never)]
        public DateTime PODate { get; set; }
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string POLineItemNumber { get; set; }
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string LicensablePartNumber { get; set; }
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string LicensableName { get; set; }
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string PartNumber { get; set; }
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string FulfillmentNumber { get; set; }
        [Column(UpdateCheck = UpdateCheck.Never)]
        public DateTime FulfilledDateUTC { get; set; }
        [Column(UpdateCheck = UpdateCheck.Never)]
        public DateTime FulfillmentCreateDateUTC { get; set; }
        [Column(UpdateCheck = UpdateCheck.Never)]
        public int Quantity { get; set; }
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string Status {get;set;}
        [Column(UpdateCheck = UpdateCheck.Never)]
	    public  string Remark {get;set;}
        [Column(UpdateCheck = UpdateCheck.Never)]
	    public DateTime Cdt {get;set;}
        [Column(UpdateCheck = UpdateCheck.Never)]
	    public DateTime Udt {get;set;}

        public ReceivedKeyInfo() { }
        public ReceivedKeyInfo(KeyFulfillment keyFulfillment)
        {
            this.OrderUniqueID = keyFulfillment.OrderUniqueID;
            this.FulfilledDateUTC = DateTime.ParseExact(keyFulfillment.FulfilledDateUTC, "yyyy-MM-ddTHH:mm:sszzz", null);
            this.FulfillmentCreateDateUTC = DateTime.ParseExact(keyFulfillment.FulfillmentCreateDateUTC, "yyyy-MM-ddTHH:mm:sszzz", null);
            this.FulfillmentNumber = keyFulfillment.FulfillmentNumber;
            this.LicensableName = keyFulfillment.LicensableName;
            this.LicensablePartNumber = keyFulfillment.LicensablePartNumber;
            this.PartNumber = keyFulfillment.PartNumber;
            this.PODate = DateTime.ParseExact(keyFulfillment.PODate, "yyyy-MM-ddTHH:mm:sszzz", null);
            this.POLineItemNumber = keyFulfillment.POLineItemNumber;
            this.PONumber = keyFulfillment.PONumber;
            this.Quantity = int.Parse(keyFulfillment.Quantity);
            this.ShipToCustomerID = keyFulfillment.ShipToCustomerID;
            this.SoldToCustomerID = keyFulfillment.SoldToCustomerID;
            this.Status = "Created";  // Create
            this.Remark = "";
            this.Cdt = DateTime.Now;
            this.Udt = DateTime.Now;
             
        }

    }   

}
