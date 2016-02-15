using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.Repository._Metas;

namespace IMES.DataModel
{
    [ORMapping(typeof(Pallet_RFID))]
    [Serializable]
    public class PalletRfidInfo
    {
        [ORMapping(Pallet_RFID.fn_carrier)]
        public String carrier = null;
        [ORMapping(Pallet_RFID.fn_cdt)]
        public DateTime cdt = DateTime.MinValue;
        [ORMapping(Pallet_RFID.fn_editor)]
        public String editor = null;
        [ORMapping(Pallet_RFID.fn_id)]
        public Int32 id = int.MinValue;
        [ORMapping(Pallet_RFID.fn_plt)]
        public String plt = null;
        [ORMapping(Pallet_RFID.fn_rfidcode)]
        public String rfidcode = null;
        [ORMapping(Pallet_RFID.fn_udt)]
        public DateTime udt = DateTime.MinValue;
    }
}
