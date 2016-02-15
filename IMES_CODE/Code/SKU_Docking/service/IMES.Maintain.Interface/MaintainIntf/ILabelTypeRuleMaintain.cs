using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;
using System.Data;
using IMES.Infrastructure.UnitOfWork;

namespace IMES.Maintain.Interface.MaintainIntf
{
    /// <summary>
    /// LabelTypeRuleMaintain接口
    /// </summary>
    public interface ILabelTypeRuleMaintain
    {
        #region LabelTypeRule

        IList<LabelTypeRuleDef> GetLabeTypeRuleByPCode(string pCode);

        void UpdateAndInsertLabeTypeRule(LabelTypeRuleDef item);

        void DeleteLabelTypeRule(string labelType);

        void UpdateAndInsertModelConstValue(string labelType, string modelConstValue, string editor);

        void UpdateAndInsertDeliveryConstValue(string labelType, string deliveryConstValue, string editor);

        void UpdateAndInsertPartConstValue(string labelType, int bomLevel, string partConstValue, string editor);

        void UpdateAndInsertLabeTypeRuleDefered(IUnitOfWork uow, LabelTypeRuleDef item);

        void DeleteLabelTypeRuleDefered(IUnitOfWork uow, string labelType);

        void UpdateAndInsertModelConstValueDefered(IUnitOfWork uow, string labelType, string modelConstValue, string editor);

        void UpdateAndInsertDeliveryConstValue(IUnitOfWork uow, string labelType, string deliveryConstValue, string editor);

        void UpdateAndInsertPartConstValueDefered(IUnitOfWork uow, string labelType, int bomLevel, string partConstValue, string editor);

        #endregion
    }
}
