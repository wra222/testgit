using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMES.DataModel
{
    public class ModelInfoNameAndModelInfoValue
    {
        public long ID { get; set; }
        public string Name { get; set;}
        public string Description { get; set;}
        public string Value { get; set;}
        public string Editor { get; set;}
        public DateTime Cdt { get; set;}
        public DateTime Udt { get; set; }
    }
}
