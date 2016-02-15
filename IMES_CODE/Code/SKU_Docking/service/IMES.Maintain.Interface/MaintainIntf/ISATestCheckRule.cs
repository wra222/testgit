using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;

namespace IMES.Maintain.Interface.MaintainIntf
{
    public interface ISATestCheckRule
    {
         int AddSATestCheckRuleItem(SATestCheckRuleDef def);

         int UpdateSATestCheckRuleItem(SATestCheckRuleDef def);

         void RemoveSATestCheckRuleItem(int id);

         IList<SATestCheckRuleDef> GetAllSATestItems();
    }
}
