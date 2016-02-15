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
    [Serializable]
    public class KeyFulfillment
    {
        public Guid OrderUniqueID{get;set;}
        public string SoldToCustomerID {get;set;}
        public string ShipToCustomerID{get;set;}
        public string PONumber{get;set;}
        public string PODate {get;set;}
        public string POLineItemNumber {get;set;}
        public string LicensablePartNumber {get;set;}
        public string LicensableName { get; set; }
        public string PartNumber{get;set;}
        public string FulfillmentNumber{get;set;}        
        public string FulfilledDateUTC{get;set;}
        public string FulfillmentCreateDateUTC{get;set;}
        public string Quantity{get;set;}

        [XmlElement("Ranges")]
        public Ranges RangeItem { get; set; }
        [XmlElement("Keys")]
        public Keys KeyItem { get; set; }
    }  
}
