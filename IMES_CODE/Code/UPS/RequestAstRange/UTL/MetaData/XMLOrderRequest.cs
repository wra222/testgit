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
    
    [XmlRootAttribute("OrderRequest", Namespace = "http://schemas.ms.it.oem/digitaldistribution/2010/10",
     IsNullable = false)]
     public class OrderRequest 
    {
        public string UserRequestKey;
        public string PONumber;
        public string PODateFrom;
        public string PODateTo;
    }

 }
