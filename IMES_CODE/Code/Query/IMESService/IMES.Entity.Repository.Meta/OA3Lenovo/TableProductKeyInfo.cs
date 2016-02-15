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
    [Table(Name="ProductKeyInfo")]
    public class ProductKeyInfo
    {
        [Column(IsPrimaryKey = true, UpdateCheck = UpdateCheck.Never)]
        public long ProductKeyID ;
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string ProductKey;
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string HardwareID;
        [Column(DbType = "tinyint", UpdateCheck = UpdateCheck.Never)]
        public byte ProductKeyStateID;  //31:ReadyCBR 32:SendingCBR 33:CBRAck 34:CBRFail 35:MSCBRFail 4:NotifiedBound
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string ProductKeyState;
        [Column(UpdateCheck = UpdateCheck.Never )]
        public Nullable<Guid> OrderUniqueID;
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string SoldToCustomerID;
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string ReferenceNumber;
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string OEMPONumber;
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string ReturnReasonCode;      
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
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string OEMPartNumber;


    }


    public enum CBRStateEnum
    {
        Allocated=1,
        Lock=10,
        ReadyUnUsed=11,
        SendingUnUsed=12,
        NotifiedUnUsed=13,
        Consumed=2,
        Bound =3,
        ReadyCBR=31,
        SendingCBR=32,
        CBRAck=33,
        CBRFail=34,
        MSCBRFail=35,
        NotifiedBound=4,
        ReadyReturned=41,
        ReadySeaReturned = 42,
        SendingReturned=43,
        SendingSeaReturned = 44,
        CBRReturnedFail=45,
        CBRSeaReturnedFail = 46,
        Returned=5,       
        MSCBReturnedFail=54,
        NotifiedReturned=6
    }

}
