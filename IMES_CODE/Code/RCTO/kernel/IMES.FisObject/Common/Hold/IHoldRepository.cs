using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.UnitOfWork;
using IMES.DataModel;

namespace IMES.FisObject.Common.Hold
{
    public interface IHoldRepository : IRepository<Hold>
    {
        #region HoldRule interface
        void InsertHoldRule(HoldRuleInfo item);
        void UpdateHoldRule(HoldRuleInfo item);
        void DeleteHoldRule(int id);
        IList<HoldRuleInfo> GetHoldRule(HoldRuleInfo condition);

        void InsertMultiCustSNHoldRule(IList<string> custSnList,HoldRuleInfo item);

        IList<HoldRulePriorityInfo> GetHoldRulePriority(string line, string family, string model, string custSN, string checkInStation);

        bool ExistHoldRuleByCustSn(IList<string> custSnList);

        #endregion

        #region DefectHoldRule interface
        void InsertDefectHoldRule(DefectHoldRuleInfo item);
        void UpdateDefectHoldRule(DefectHoldRuleInfo item);
        void DeleteDefectHoldRule(int id);
        IList<DefectHoldRuleInfo> GetDefectHoldRule(DefectHoldRuleInfo condition);
        #endregion

        #region hold interface

        #endregion

    }
}
