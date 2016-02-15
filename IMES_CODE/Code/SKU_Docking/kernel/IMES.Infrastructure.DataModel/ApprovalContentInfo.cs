using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.Repository._Metas;

namespace IMES.DataModel
{
    [Serializable]
    public class ApprovalContentInfo
    {

        public Int64 ApprovalID=0;
        public String Module = null;      
        public String ActionName = null;      
        public String Department = null;       
        public String IsNeedApprove = null;      
        public String OwnerEmail = null;       
        public String CCEmail = null;   
        public String IsNeedUploadFile = null;      
        public String NoticeMsg = null;
        public Int64 ApprovalStatusID =0;
        public String ModuleKeyValue = null;
        public String ApprovalStatus = null;
        public String Comment = null;
        public Int64 uploadFilesIID = 0;
        public String UploadServerName = null;
        public String UploadFileGUIDName = null;
        public String UploadFileName = null;
        public DateTime UploadDate = DateTime.Now;


    }
}
