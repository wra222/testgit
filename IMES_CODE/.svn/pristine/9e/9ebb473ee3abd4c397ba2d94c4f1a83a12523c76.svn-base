using System.Text;
using IMES.DataModel;
using System.Data;
using System.Collections.Generic;
using System;

namespace IMES.Maintain.Interface.MaintainIntf
{
    public interface IPartForbidRule
    {
        IList<string> GetCustomer();

        IList<string> GetCategory();

        IList<string> GetLine(string customer);

        IList<PartForbidRuleInfo> GetPartForbidRule(PartForbidRuleInfo condition);

        void InsertPartForbidRule(PartForbidRuleInfo item);

        void UpdatePartForbidRule(PartForbidRuleInfo item);

        void DeletePartForbidRule(int id);
    }
}
