using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace IMES.Entity.Repository.Meta.OA3Lenovo
{
    [Serializable]
    [Table(Name = "IESOrder")]
    public class IESOrder// : IOrderResponse
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true, UpdateCheck = UpdateCheck.Never)]
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

        [Column(DbType = "UniqueIdentifier", Name = "OrderUniqueID", UpdateCheck = UpdateCheck.Never)]
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
       
       
    }

  

}
