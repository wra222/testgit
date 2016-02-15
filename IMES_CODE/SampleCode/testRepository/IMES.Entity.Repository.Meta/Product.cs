using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq.Mapping;

namespace IMES.Entity.Repository.Meta
{
    [Table(Name = "Product")]
    public class Product
    {
        [Column(IsPrimaryKey = true,DbType="varchar(9) not null",UpdateCheck = UpdateCheck.Never )]
        public string ProductID;

        [Column(UpdateCheck = UpdateCheck.Never,AutoSync=AutoSync.Never)]
        //[Column]
        public string CUSTSN;
        // [Column]
        [Column(UpdateCheck = UpdateCheck.Never, AutoSync = AutoSync.Never)]
        public string PCBID;
        [Column(UpdateCheck = UpdateCheck.Never, AutoSync = AutoSync.Never)]
       // [Column]
        public string Model;

        [Column(UpdateCheck = UpdateCheck.Never)]
        public DateTime Udt;

      


    }

}
