using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.Repository._Metas;
using mtns = IMES.Infrastructure.Repository._Metas;

namespace IMES.DataModel
{
    [ORMapping(typeof(mtns::UploadPOLog))]
    [Serializable]
    public class EDIUploadPOLogInfo
    {

        [ORMapping(mtns::UploadPOLog.fn_id)]
        public long id = 0;
        [ORMapping(mtns::UploadPOLog.fn_textFileRecordCount)]
        public int textFileRecordCount = int.MinValue;
        [ORMapping(mtns::UploadPOLog.fn_uploadOKRecordCount)]
        public int uploadOKRecordCount = int.MinValue;
        [ORMapping(mtns::UploadPOLog.fn_uploadNGDeliveryNo)]
        public string uploadNGDeliveryNo =null;
        [ORMapping(mtns::UploadPOLog.fn_editor)]
        public string editor = null;
        [ORMapping(mtns::UploadPOLog.fn_cdt)]
        public DateTime cdt = DateTime.MinValue;
    }
}
