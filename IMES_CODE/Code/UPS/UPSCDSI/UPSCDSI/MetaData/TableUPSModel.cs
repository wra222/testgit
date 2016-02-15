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
    [Table(Name = "UPSModel")]
    public class UPSModel
    {
        [Column(IsPrimaryKey=true)]
        public string Model;
        [Column(UpdateCheck = UpdateCheck.Never)]
        public DateTime FirstReceiveDate;
        [Column(UpdateCheck = UpdateCheck.Never)]
        public DateTime LastReceiveDate;
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
