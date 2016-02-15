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
    [Table(Name = "UPSIECPO")]
    public class UPSIECPO
    {
        [Column(IsPrimaryKey=true, IsDbGenerated=true)]
        public long ID;
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string IECPO;
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string IECPOItem;
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string Model;
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string HPPO;
         [Column(UpdateCheck = UpdateCheck.Never)]
        public int Qty;       
        //[Column(UpdateCheck = UpdateCheck.Never)]
        //public string Status;
       
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string Editor;
        [Column(UpdateCheck = UpdateCheck.Never)]
        public DateTime Cdt;
        [Column(UpdateCheck = UpdateCheck.Never)]
        public DateTime Udt;   
    }
       
    
}
