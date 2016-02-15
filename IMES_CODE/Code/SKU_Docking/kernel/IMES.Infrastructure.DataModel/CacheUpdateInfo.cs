using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMES.DataModel
{
    public class CacheUpdateInfo
    {
        public int ID = int.MinValue;
        public string Type = null;
        public string Item = string.Empty;
        public bool Updated = default(bool);
        public string CacheServerIP = null;
        public string AppName = null;
        public DateTime Cdt = DateTime.MinValue;
        public DateTime Udt = DateTime.MinValue;
    }
}
