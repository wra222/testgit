﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace IMES.Query.Interface.QueryIntf
{
    public interface IPAK_BsamShipSnList
    {
        DataTable GetBsamShipSnListByDN(string Connection, string ShipDate);
        DataTable GetBsamShipSnListByConsolidate(string Connection, string ShipDate);
    }
}