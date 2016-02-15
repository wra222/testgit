using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMES.DataModel
{
    [Serializable]
    public class COMSettingDef
    {
        public int id;
        public string name;
        public string commport;
        public string rthreshold;
        public string baudRate;
        public string handshaking;
        public string sthreshold;
        public string editor;
        public string cdt;
        public string udt;
    }
}
