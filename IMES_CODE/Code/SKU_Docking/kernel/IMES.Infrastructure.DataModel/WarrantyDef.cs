using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMES.DataModel
{
    [Serializable]
    public class WarrantyDef
    {
        public String id;
        public String Customer;
        public String Type;
        public String DateCodeType;
        public String WarrantyFormat;
        public String ShipTypeCode;
        public String WarrantyCode;
        public String Descr;
        public String Editor;
        //对应下拉框文字
        public String TypeText;
        public String WarrantyFormatText;
        public String ShipTypeCodeText;
        public String cdt;
        public String udt;
    }

}
