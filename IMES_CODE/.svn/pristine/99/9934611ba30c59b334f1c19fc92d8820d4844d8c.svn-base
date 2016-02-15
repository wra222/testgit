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
    [Table(Name="ProductKeyInfo")]
    public class ProductKeyInfo
    {
        [Column(IsPrimaryKey = true)]
        public long ProductKeyID ;
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string ProductKey;
        [Column(DbType = "tinyint", UpdateCheck = UpdateCheck.Never)]
        public byte ProductKeyStateID;
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string ProductKeyState;

        [Column(CanBeNull = true, DbType = "UniqueIdentifier NULL", UpdateCheck = UpdateCheck.Never)]
        public Guid? OrderUniqueID;
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string SoldToCustomerID;
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string LicensablePartNumber;
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string OEMPONumber;
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string OEMLineItemNumber;
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string OEMPartNumber;
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string ContractNumber;
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string ReferenceNumber;
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string SKUID;
        [Column(UpdateCheck = UpdateCheck.Never)]
        public int OrderID;
        [Column(UpdateCheck = UpdateCheck.Never)]
        public int Quantity;
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

    public enum CBRStateEnum
    {
        Allocated = 1,
        ReadyUnUsed = 11,
        SendingUnUsed = 12,
        NotifiedUnUsed = 13,
        Consumed = 2,
        Bound = 3,
        ReadyCBR = 31,
        SendingCBR = 32,
        CBRAck = 33,
        CBRFail = 34,
        MSCBRFail = 35,
        NotifiedBound = 4,
        ReadyReturned = 41,
        ReadySeaReturned = 42,
        SendingReturned = 43,
        SendingSeaReturned = 44,
        CBRReturnedFail = 45,
        CBRSeaReturnedFail = 46,
        Returned = 5,
        MSCBReturnedFail = 54,
        NotifiedReturned = 6
    }


}
