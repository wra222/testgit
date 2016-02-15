using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.Repository._Metas;

namespace IMES.DataModel
{
    [ORMapping(typeof(Pakodmsession))]
    [Serializable]
    public class PakOdmSessionInfo
    {
        [ORMapping(Pakodmsession.fn_id)]
        public Int32 id = int.MinValue;
        [ORMapping(Pakodmsession.fn_serial_num)]
        public String serial_num = null;
        [ORMapping(Pakodmsession.fn_show_order)]
        public Int32 show_order = int.MinValue;
        [ORMapping(Pakodmsession.fn_udf_key_detail)]
        public String udf_key_detail = null;
        [ORMapping(Pakodmsession.fn_udf_key_header)]
        public String udf_key_header = null;
        [ORMapping(Pakodmsession.fn_udf_value_detail)]
        public String udf_value_detail = null;
        [ORMapping(Pakodmsession.fn_udf_value_header)]
        public String udf_value_header = null;
    }
}
