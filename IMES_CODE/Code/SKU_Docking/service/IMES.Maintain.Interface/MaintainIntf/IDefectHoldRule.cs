using System.Text;
using IMES.DataModel;
using System.Data;
using System.Collections.Generic;
using System;

namespace IMES.Maintain.Interface.MaintainIntf
{
    public interface IDefectHoldRule
    {
        IList<string> GetLineTop();

        IList<string> GetLine();

        IList<string> GetCheckInStation();

        IList<string> GetHoldStation();

        IList<string> GetHoldCode();

        bool CheckFamilyAndModel(string inputDate);

        IList<DefectHoldRuleInfo> GetDefectHoldRule(DefectHoldRuleInfo condition);

        void InsertDefectHoldRule(DefectHoldRuleInfo item);

        void UpdateDefectHoldRule(DefectHoldRuleInfo item);

        void DeleteDefectHoldRule(int id);
    }
}
