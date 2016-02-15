using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Station.Interface.CommonIntf;
using IMES.DataModel;
using System.Collections;
using System.Data;

namespace IMES.Station.Interface.StationIntf
{

    public interface IFAIPAKRelease
    {
        #region FAIPAKRelease

        //IList<FAIModelInfo> GetFAIModelList(FAIModelInfo condition);

        DataTable GetDepartmentApproveStatus(string model,string department,string state);

        void InsertUpLoadFileInfo(string model, string Department, string uploadFileGUIDName, string uploadFileName, string editor);

        //IList<string> GetDepartmentList(string model);

        void SaveStatus(string model, string approvalID, int pakQty, string shipdate, string editor);

        void ReleaseStatus(string model, string approvalID, string editor);
        #endregion
    }
}
