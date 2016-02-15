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
    [Serializable]
    [Table(Name = "ProductKeyState")]
    public class ProductKeyState
    {
        //ProductKeyStateID, ProductKeyStateName, StateArea, Description, ActionCode, CreatedBy, CreatedDate, ModifiedBy, ModifiedDate
        [Column(DbType = "tinyint", IsPrimaryKey = true, IsDbGenerated = true, UpdateCheck = UpdateCheck.Never)]
        public byte ProductKeyStateID { get; set; }
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string ProductKeyStateName;
        [Column(UpdateCheck = UpdateCheck.Never)]
        public int StateArea;
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string Description;
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string ActionCode;
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string CreatedBy;
        [Column(UpdateCheck = UpdateCheck.Never)]
        public DateTime CreatedDate;
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string ModifiedBy;
        [Column(UpdateCheck = UpdateCheck.Never)]
        public DateTime ModifiedDate;
    }
}
