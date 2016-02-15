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

    public interface IFAIFARelease
    {
        #region FAIFARelease
        /// <summary>
        /// getFAIModelList
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        DataTable GetFAIModelList(string department, string model, string from, string to, string state);

        DataTable GetDepartmentApproveStatus(string model);

        void InsertUpLoadFileInfo(string model, string Department, string uploadFileGUIDName, string uploadFileName, string editor);

        IList<string> GetDepartmentList(string model);

        void CheckApprovalStatusAndChengeStatus(string model, string approvalStatusID, string approvalItemID, string comment, string editor, string family, string department, string isNeedUploadFile, string filename);

        void RemoveStatus(string model, string approvalID, string comment, string editor);

        DataTable GetExeclData(string model, string department, string from, string to, string state);

        string UploadFile(string guid);
        #endregion
    }
}
