﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PCA.MB;
using IMES.FisObject.Common.Part;
using IMES.FisObject.PAK.DN;

namespace IMES.CheckItemModule.Utility
{
    public class TagData
    {
        public IProduct Product;
        public IMB PCB;
        public Part MBPart;
        public Delivery DN;
        public Hashtable GroupValueList = new Hashtable();
    }

    public class KPVendorCode
    {
        public string PartNo;
        public string VendorCode;
    }
}
