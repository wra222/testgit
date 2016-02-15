using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace IMES.Entity.Repository.Meta.OA3Lenovo
{
    [Table(Name = "CBRInfo_Returned")]
    public class CBRInfo_Returned
    {
        [Column(IsPrimaryKey = true,UpdateCheck = UpdateCheck.Never)]
        public int CBRInfoID;
        [Column(UpdateCheck = UpdateCheck.Never)]
        public long ProductKeyID;
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string HardwareID;
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string SerialNumber;
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string PartNumber;
        [Column(UpdateCheck = UpdateCheck.Never)]
        public bool DeleteFlag;
        [Column(UpdateCheck = UpdateCheck.Never)]
        public String ActionCode;
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string CreatedBy;
        [Column(UpdateCheck = UpdateCheck.Never)]
        public DateTime CreatedDate;
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string ModifiedBy;
        [Column(UpdateCheck = UpdateCheck.Never)]
        public DateTime ModifiedDate;
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string PCBID;
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string Reditor;
        [Column(UpdateCheck = UpdateCheck.Never)]
        public DateTime Rdt;
    }
}
