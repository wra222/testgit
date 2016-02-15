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
    public class OrderResponse 
    {
        [XmlElement]
        public string PONumber { get; set; }

        [XmlElement]
        public string POLineItemNumber { get; set; }

        [XmlElement]
        public string LicensablePartNumber { get; set; }

        [XmlElement]
        public string LicensableName { get; set; }

        [XmlElement]
        public string PartNumber { get; set; }
        
        [XmlElement]
        public string Quantity { get; set; }

        [XmlElement("OrderUniqueID")]
        public string OrderUniqueID { get; set; }

        public Guid OrderGUID
        {
            get { return string.IsNullOrEmpty(OrderUniqueID) ? new Guid() : new Guid(OrderUniqueID); }
            set {}
        }
        [XmlElement]
        public string PODate { get; set; }
        [XmlElement]
        public string POType { get; set; }
        [XmlElement]
        public string POStatus { get; set; }
        [XmlElement]
        public string PLANTFROM { get; set; }
        [XmlElement]
        public string PLANTTO { get; set; }        
    }

    [Serializable]
    [XmlRootAttribute("ArrayOrderResponse", Namespace = "http://schemas.ms.it.oem/digitaldistribution/2010/10",
    IsNullable = false)]
    public class ArrayOrderResponse
    {        
        [XmlElement("OrderResponse")]
        public List<OrderResponse> OrderResponseItems { get; set; }
    }    

}
