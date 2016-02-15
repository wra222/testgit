﻿using System;
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
    [Table(Name = "KeyHistory")]
    public class KeyHistory
    {
        [Column(IsPrimaryKey = true, UpdateCheck = UpdateCheck.Never)]
        public long ProductKeyID;
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string ProductKey;
        [Column(IsPrimaryKey = true, DbType = "tinyint", UpdateCheck = UpdateCheck.Never)]
        public byte ProductKeyStateID;
        [Column(IsPrimaryKey = true, UpdateCheck = UpdateCheck.Never)]
        public DateTime StateChangeDate;
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
