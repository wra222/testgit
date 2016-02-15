using System.Text;
using IMES.DataModel;
using System.Data;
using System.Collections.Generic;
using System;

namespace IMES.Maintain.Interface.MaintainIntf
{
    public interface IHoldRule
    {
        IList<string> GetLineTop();

        IList<string> GetFamilyTop();

        IList<string> GetModelTop(string family);

        IList<string> GetLine();

        IList<string> GetFamily();

        IList<string> GetCheckInStation();

        IList<string> GetHoldStation();

        IList<string> GetHoldCode();

        IList<HoldRuleInfo> GetHoldRule(HoldRuleInfo condition);

        void InsertMultiCustSNHoldRule(IList<string> custSnList, HoldRuleInfo item);

        void InsertHoldRule(HoldRuleInfo item);

        void UpdateHoldRule(HoldRuleInfo item);

        void DeleteHoldRule(int id);
    }
}
