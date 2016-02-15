using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;
using System.Data;

namespace IMES.Maintain.Interface.MaintainIntf
{
    /// <summary>
    /// ICheckItemTypeListMaintain接口
    /// </summary>
    public interface IModuleApprovalItem
    {
        #region ModuleApprovalItem

        IList<string> GetFamilyList();

        DataTable GetApprovalItemAttrList(string approvalItemID);

        IList<string> GetModuleListTop();

        IList<string> GetModuleList();

        IList<string> GetActionNAmeList(string module);

        IList<string> GetDepartmentList(string module);

        DataTable GetModuleList(string module);

        IList<ApprovalItemInfo> GetModuleList(ApprovalItemInfo condition);

        void UpdateApprovalItem(ApprovalItemInfo item);

        void InsertApprovalItem(ApprovalItemInfo item);

        void DeleteApprovalItem(int id);

        void InsertApprovalItemAttr(IList<ApprovalItemAttrInfo> condition);

        void DeleteApprovalItemAttr(long id);

        
        
        #endregion
    }
}
