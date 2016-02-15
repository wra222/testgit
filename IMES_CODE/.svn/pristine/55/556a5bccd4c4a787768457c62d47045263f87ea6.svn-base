using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace UPH.Entity.Repository.Meta.IMESSKU
{
    [Table(Name = "ProductUPH")]
    public class ProductUPH
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true, UpdateCheck = UpdateCheck.Never)]
        public int ID;
      
        [Column(UpdateCheck = UpdateCheck.Never)]
        public DateTime Date;

        [Column(UpdateCheck = UpdateCheck.Never)]
        public string Line;

        [Column(UpdateCheck = UpdateCheck.Never)]
        public string Lesson;

        [Column(UpdateCheck = UpdateCheck.Never)]
        public string TimeRange;

        [Column(UpdateCheck = UpdateCheck.Never)]
        public string Family;

        [Column(UpdateCheck = UpdateCheck.Never)]
        public Decimal ProductRatio;

       

    }
}
