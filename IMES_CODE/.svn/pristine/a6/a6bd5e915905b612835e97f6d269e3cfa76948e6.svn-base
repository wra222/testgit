using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.Repository._Metas;
using System.Reflection;

namespace IMES.DataModel
{
    [ORMapping(typeof(UPSHPPO))]
    [Serializable]
    public class UPSHPPOInfo
    {
        [ORMapping(UPSHPPO.fn_hppo)]
        public String HPPO = null;

        [ORMapping(UPSHPPO.fn_plant)]
        public String Plant = null;

        [ORMapping(UPSHPPO.fn_potype)]
        public String POType = null;

        [ORMapping(UPSHPPO.fn_endCustomerPO)]
        public String EndCustomerPO = null;

        [ORMapping(UPSHPPO.fn_hpsku)]
        public String HPSKU = null;

        [ORMapping(UPSHPPO.fn_qty)]
        public int Qty = int.MinValue;

        [ORMapping(UPSHPPO.fn_receiveDate)]
        public DateTime ReceiveDate = DateTime.MinValue;

        [ORMapping(UPSHPPO.fn_status)]
        public String Etatus = null;

        [ORMapping(UPSHPPO.fn_errorText)]
        public String ErrorText = null;

        [ORMapping(UPSHPPO.fn_editor)]
        public String Editor = null;

        [ORMapping(UPSHPPO.fn_cdt)]
        public DateTime Cdt = DateTime.MinValue;

        [ORMapping(UPSHPPO.fn_udt)]
        public DateTime Udt = DateTime.MinValue;

        public object GetProperty(string name)
        {
            FieldInfo prop = GetType().GetField(name);
            if (prop != null)
            {
                return prop.GetValue(this);
            }
            return null;
        }
    }
}
