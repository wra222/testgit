using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using IMES.Entity.Repository.Meta.IMESSKU;
namespace IMES.Query.Interface.QueryIntf
{
    public interface IFA_ECOModelQuery
    {
        IList<ECOModelInfo> GetECOModelList(string model, DateTime from, DateTime to);
        
        IList<ECOModelInfo> GetECOModelList(long id);

        IList<ECOModelInfo> SaveECOModelChange(ECOModelInfo item);

    }

    [Serializable]
    public class ECOModelInfo
    {
        public long ID;
        public string Plant;
        public string ECRNo;
        public string ECONo;
        public string Model;
        public DateTime ValidateFromDate;
        public string PreStatus;
        public string Status;
        public string Remark;
        public string Editor;
        public DateTime Cdt;
        public DateTime Udt;
    }

    [Serializable]
    public class ConstValueTypeInfo
    {
        public int ID;
        public string Type;
        public string Value;
    }
}
