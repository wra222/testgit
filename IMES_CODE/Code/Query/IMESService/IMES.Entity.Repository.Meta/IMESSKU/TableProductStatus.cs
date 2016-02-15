﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace IMES.Entity.Repository.Meta.IMESSKU
{
    [Table(Name = "ProductStatus")]
    public class ProductStatus
    {
        [Column(IsPrimaryKey = true, UpdateCheck = UpdateCheck.Never)]
        public string ProductID;
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string Station;
        [Column(UpdateCheck = UpdateCheck.Never)]
        public int Status;
    }

}
