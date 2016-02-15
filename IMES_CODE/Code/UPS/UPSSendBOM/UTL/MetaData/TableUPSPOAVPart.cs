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
    [Table(Name = "UPSPOAVPart")]
    public class UPSPOAVPart
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true)]
        public long ID;
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string HPPO;
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string AVPartNo;
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string IECPartNo;
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string IECPartType;
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string Remark;       

        [Column(UpdateCheck = UpdateCheck.Never)]
        public string Editor;
        [Column(UpdateCheck = UpdateCheck.Never)]
        public DateTime Cdt;
        [Column(UpdateCheck = UpdateCheck.Never)]
        public DateTime Udt;
    }

  

}
