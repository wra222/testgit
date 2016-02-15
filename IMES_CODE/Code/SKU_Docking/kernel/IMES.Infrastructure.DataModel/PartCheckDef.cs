using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMES.DataModel
{
    [Serializable]
    public class PartCheckDef
    {
        public string ID;
        public string Customer;
        public string PartType;
        public string ValueType;
        public string NeedSave;
        public string NeedCheck;
        public string Editor;
    }
}
