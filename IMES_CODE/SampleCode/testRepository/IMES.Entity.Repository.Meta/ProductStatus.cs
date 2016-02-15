using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq.Mapping;

namespace IMES.Entity.Repository.Meta
{
    [Table(Name = "ProductStatus")]
    public class ProductStatus
    {
        [Column(IsPrimaryKey = true,UpdateCheck = UpdateCheck.Never )]
        public string ProductID;

        [Column(UpdateCheck = UpdateCheck.Never,AutoSync=AutoSync.Never)]
        //[Column]
        public string Station;      

        [Column(UpdateCheck = UpdateCheck.Never)]
        public DateTime Udt;     


    }

}
