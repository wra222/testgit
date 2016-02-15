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
    [Table(Name = "UPH")]
    public class UPH
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true, UpdateCheck = UpdateCheck.Never)]
        public int ID;

        [Column(UpdateCheck = UpdateCheck.Never)]
        public string Process;

        [Column(UpdateCheck = UpdateCheck.Never)]
        public int Attend_normal;

        [Column(UpdateCheck = UpdateCheck.Never)]
        public string Family;

        [Column(UpdateCheck = UpdateCheck.Never)]
        public string ST;

        [Column(UpdateCheck = UpdateCheck.Never)]
        public int NormalUPH;

        [Column(UpdateCheck = UpdateCheck.Never)]
        public string Cycle;

        [Column(UpdateCheck = UpdateCheck.Never)]
        public string Remark;

        [Column(UpdateCheck = UpdateCheck.Never)]
        public string Special;

        [Column(UpdateCheck = UpdateCheck.Never)]
        public string Editor;

        [Column(UpdateCheck = UpdateCheck.Never)]
        public DateTime Cdt;

        [Column(UpdateCheck = UpdateCheck.Never)]
        public DateTime Udt;
    }
}

