using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMES.DataModel
{
    [Serializable]
    public class CheckItemSettingDef
    {
        public string ID;
        public string Customer;
        public string CheckCondition;
        public string CheckConditionType;
        public string Station;
        public string CheckItemID;
        public string CheckType;
        public string CheckValue;
        public string Editor;
        public string Cdt;
        public string Udt;
    }
}
