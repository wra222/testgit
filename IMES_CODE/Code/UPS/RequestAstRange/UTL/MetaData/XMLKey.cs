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
    public class Key
    {
          public string ProductKey{get;set;}
          public string ProductKeyID{get;set;}
          public string SKUID{get;set;}
          public string PLANT{get;set;}
    }

    public class Keys
    {
        [XmlElement("Key")]
        public List<Key> KeyList { get; set; }
    }
}
