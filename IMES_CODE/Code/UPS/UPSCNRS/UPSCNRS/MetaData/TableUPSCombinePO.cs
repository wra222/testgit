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
    [Table(Name = "UPSCombinePO")]
    public class UPSCombinePO
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true)]
        public long ID;
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string HPPO;
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string IECPO;
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string IECPOItem;
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string Model;
        [Column(UpdateCheck = UpdateCheck.Never)]
        public DateTime ReceiveDate;
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string ProductID;
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string CUSTSN;
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string IsShipPO;
        [Column(UpdateCheck = UpdateCheck.Never)]        
        public string Station;
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string Status;
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
