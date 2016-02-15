using System;
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
    [Table(Name = "ConstValueType")]
    public class ConstValueType
    {
        [Column(IsPrimaryKey = true, UpdateCheck = UpdateCheck.Never )]
        public int ID;
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string Type;
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string Value;       

    }

}
