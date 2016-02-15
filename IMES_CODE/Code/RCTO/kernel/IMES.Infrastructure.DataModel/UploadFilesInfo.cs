using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.Repository._Metas;

namespace IMES.DataModel
{
    [ORMapping(typeof(UploadFiles))]
    [Serializable]
    public class UploadFilesInfo
    {
        [ORMapping(UploadFiles.fn_id)]
        public Int64 ID = long.MinValue;

        [ORMapping(UploadFiles.fn_approvalStatusID)]
        public Int64 ApprovalStatusID = long.MinValue;

        [ORMapping(UploadFiles.fn_uploadServerName)]
        public String UploadServerName = null;

        [ORMapping(UploadFiles.fn_uploadFileGUIDName)]
        public String UploadFileGUIDName = null;

        [ORMapping(UploadFiles.fn_uploadFileName)]
        public String UploadFileName = null;

        [ORMapping(UploadFiles.fn_editor)]
        public String Editor = null;

        [ORMapping(UploadFiles.fn_cdt)]
        public DateTime Cdt = DateTime.MinValue;

       
    }
}
