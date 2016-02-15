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
    [XmlRootAttribute("FulfillmentResponse", Namespace = "http://schemas.ms.it.oem/digitaldistribution/2010/10",
    IsNullable = false)]
    public class FulfillmentResponse
    {

        [XmlElement("Fulfillments")]
        public Fulfillment Fulfillments { get; set; }
    }
    public class Fulfillment
    {
        [XmlElement("KeyFulfillment")]
        public List<KeyFulfillment> KeyFulfillments { get; set; }
    }

}
