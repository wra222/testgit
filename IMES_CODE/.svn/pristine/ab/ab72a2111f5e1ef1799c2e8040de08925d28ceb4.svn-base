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
    [Table(Name = "IESOrder")]
    public class IESOrder// : IOrderResponse
    {
        [Column(IsPrimaryKey=true,IsDbGenerated=true)]
        public int orderID;
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string actionCode;
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string CreatedBy;
        [Column(UpdateCheck = UpdateCheck.Never)]
        public DateTime CreatedDate;

        [Column(UpdateCheck = UpdateCheck.Never)]
        public string PONumber { get; set; }

        [Column(UpdateCheck = UpdateCheck.Never)]
        public string POLineItemNumber { get; set; }
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string LicensablePartNumber { get; set; }
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string LicensableName { get; set; }
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string PartNumber { get; set; }
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string Quantity { get; set; }

        //string StrOrderUniqueID { get; set; }

        [Column(DbType = "UniqueIdentifier", Name="OrderUniqueID")]
        public Guid OrderUniqueID { get; set; }

        [Column(UpdateCheck = UpdateCheck.Never)]
        public string PODate { get; set; }
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string POType { get; set; }
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string POStatus { get; set; }
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string PLANTFROM { get; set; }
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string PLANTTO { get; set; }

        public IESOrder() { }
        public IESOrder(OrderResponse order)
        {
            this.actionCode = "0";
            this.CreatedDate = DateTime.Now;
            this.LicensableName = order.LicensableName;
            this.LicensablePartNumber = order.LicensablePartNumber;
            //this.orderID = string.IsNullOrEmpty(order.PartNumber) ? 0 : int.Parse(order.PartNumber);
            this.OrderUniqueID = order.OrderGUID;
            this.PartNumber = order.PartNumber;
            this.PLANTFROM = order.PLANTFROM;
            this.PLANTTO = order.PLANTTO;
            this.PODate = order.PODate;
            this.POLineItemNumber = order.POLineItemNumber;
            this.PONumber = order.PONumber;
            this.POStatus = order.POStatus;
            this.POType = order.POType;
            this.Quantity = order.Quantity;            
        }
       
    }

  

}
