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
    [Table(Name = "DinnerTime")]
    public class DinnerTime
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true, UpdateCheck = UpdateCheck.Never)]
        public int ID;

        [Column(UpdateCheck = UpdateCheck.Never)]
        public string Process;

        [Column(UpdateCheck = UpdateCheck.Never)]
        public string Type;

        [Column(UpdateCheck = UpdateCheck.Never)]
        public string Class;

        [Column(UpdateCheck = UpdateCheck.Never)]
        public string PdLine;

        [Column(UpdateCheck = UpdateCheck.Never)]
        public string BeginTime;

        [Column(UpdateCheck = UpdateCheck.Never)]
        public string EndTime;

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
