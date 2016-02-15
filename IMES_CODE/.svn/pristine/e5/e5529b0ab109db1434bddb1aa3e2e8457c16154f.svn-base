using System.Text;
using IMES.DataModel;
using System.Data;
using System.Collections.Generic;

namespace IMES.Maintain.Interface.MaintainIntf
{
    public interface IAssetRule
    {

        DataTable GetAssetRuleList();

        IList<AstRuleInfo> GetAsSetRuleList();

        void DeleteAssetRule(AstRuleInfo item);

        void DeleteAssetRule(int id);

        string AddAssetRule(AstRuleInfo item);

        void AddAsSetRule(AstRuleInfo item);

        List<SelectInfoDef> GetAstTypeList();

        IList<ConstValueInfo> GetCheckItemValue(string SelectTypes, string name);

        IList<string> GetAVPartNoValue(string ASTTypes, string Tp);
    }
}
