﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace IMES.Entity.Repository.Meta.IMESSKU
{
    [Serializable]
    [Table(Name = "SMT_Dashboard_Line_Time_Qty")]
    public class SMT_Dashboard_Line_Time_Qty
    {
      [Column(IsPrimaryKey = true, IsDbGenerated = true, UpdateCheck = UpdateCheck.Never)]
      public int ID;
      [Column(UpdateCheck = UpdateCheck.Never)]
      public string Line;
      [Column(UpdateCheck = UpdateCheck.Never)]
      public string DurTime;
      [Column(UpdateCheck = UpdateCheck.Never)]
      public int Qty;
      [Column(UpdateCheck = UpdateCheck.Never)]
      public string Editor;
      [Column(UpdateCheck = UpdateCheck.Never)]
      public DateTime Cdt;
      [Column(UpdateCheck = UpdateCheck.Never)]
      public DateTime Udt;
    }
}