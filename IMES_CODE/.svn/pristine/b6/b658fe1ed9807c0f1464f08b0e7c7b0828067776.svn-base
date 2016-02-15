using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace UTL.MetaData
{   
    
    [Table(Name = "ReceivedKeyRange")]
    public class ReceivedKeyRange
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true)]
        public long ID;
        [Column(DbType = "UniqueIdentifier", UpdateCheck = UpdateCheck.Never)]
        public Guid OrderUniqueID { get; set; }
        [Column(UpdateCheck = UpdateCheck.Never)]
        public long BeginningProductKeyID { get; set; }
        [Column(UpdateCheck = UpdateCheck.Never)]
        public long EndingProductKeyID { get; set; }
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string Remark { get; set; }
        [Column(UpdateCheck = UpdateCheck.Never)]
        public DateTime Cdt { get; set; }
    }   

}
