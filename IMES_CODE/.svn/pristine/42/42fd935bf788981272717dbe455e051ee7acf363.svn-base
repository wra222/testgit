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
    
    public class Range
    {
        public string BeginningProductKeyID{get;set;}
        public string EndingProductKeyID { get; set; }
    }

    public class Ranges
    {
        [XmlElement("Range")]
        public List<Range> RangeList { get; set; }
    }  

}
