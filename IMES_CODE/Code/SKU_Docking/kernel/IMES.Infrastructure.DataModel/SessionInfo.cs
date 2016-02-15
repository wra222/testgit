using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMES.DataModel
{
    [Serializable]
    public class SessionInfo
    {
        public string SessionKey;
        public string StationId;
        public string PdLine;
        public string Operator;
        public DateTime  Cdt;
        public SessionType sessiontype ;
   
    }
    public enum SessionType
    {
        Common = 0,
        MB,
        Product
    }    

}
