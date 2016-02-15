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
    [Serializable]
    [Table(Name = "SMT_Dashboard_Result")]
    public class SMT_Dashboard_Result
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true, UpdateCheck = UpdateCheck.Never)]
        public int ID;
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string Line;
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string DurTime;
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string StandardOutput;
    }
}
