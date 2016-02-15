using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMES.DataModel
{
    [Serializable]
    public class RepairInfoMaintainDef
    {
        public int id;
        public string code;
        public string description;
        public string engDescr;
        public string type;
        public string customerID;
        public string editor;
        public string cdt;
        public string udt;
    }
}
