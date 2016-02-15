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
    [Table(Name = "UPSHPPO")]
    public class UPSHPPO
    {
        [Column(IsPrimaryKey=true)]
        public string HPPO;
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string Plant;
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string POType;
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string EndCustomerPO;
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string HPSKU;
         [Column(UpdateCheck = UpdateCheck.Never)]
        public int Qty;
        [Column(UpdateCheck = UpdateCheck.Never)]
        public DateTime ReceiveDate;
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string Status;
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string ErrorText;

        [Column(UpdateCheck = UpdateCheck.Never)]
        public string Editor;
        [Column(UpdateCheck = UpdateCheck.Never)]
        public DateTime Cdt;
        [Column(UpdateCheck = UpdateCheck.Never)]
        public DateTime Udt;

        //針對拉單新增訂單數量
        public int WithdrawQty=0;

    }
}
