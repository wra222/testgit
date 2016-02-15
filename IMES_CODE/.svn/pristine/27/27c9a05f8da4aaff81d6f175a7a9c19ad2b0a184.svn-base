using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;
using IMES.Maintain.Interface.MaintainIntf;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure;
using System.Data;
using IMES.FisObject.Common.PrintItem;
using IMES.Infrastructure.UnitOfWork;
using IMES.FisObject.Common.Part;

namespace IMES.Maintain.Implementation
{
    public class LabelTypeRuleMaintainImpl : MarshalByRefObject, ILabelTypeRuleMaintain
    {

        ILabelTypeRepository iILabelTypeRepository = RepositoryFactory.GetInstance().GetRepository<ILabelTypeRepository>();
        IPartRepository iPartRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository , IPart>();
        #region LabelTypeRule

        public IList<LabelTypeRuleDef> GetLabeTypeRuleByPCode(string pcode)
        {
            try
            {
                return iILabelTypeRepository.GetLabeTypeRuleByPCode(pcode);
            }
            catch (Exception)
            {
                throw;
            } 
        }

        public void UpdateAndInsertLabeTypeRule(LabelTypeRuleDef item)
        {
            try
            {
                iILabelTypeRepository.UpdateAndInsertLabeTypeRule(item);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteLabelTypeRule(string labelType)
        {
            try
            {
                iILabelTypeRepository.DeleteLabelTypeRule(labelType);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateAndInsertModelConstValue(string labelType, string modelConstValue, string editor)
        {
            try
            {
                iILabelTypeRepository.UpdateAndInsertModelConstValue(labelType, modelConstValue, editor);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateAndInsertDeliveryConstValue(string labelType, string deliveryConstValue, string editor)
        {
            try
            {
                iILabelTypeRepository.UpdateAndInsertDeliveryConstValue(labelType, deliveryConstValue, editor);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateAndInsertPartConstValue(string labelType, int bomLevel, string partConstValue, string editor)
        {
            try
            {
                iILabelTypeRepository.UpdateAndInsertPartConstValue(labelType, bomLevel, partConstValue, editor);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateAndInsertLabeTypeRuleDefered(IUnitOfWork uow, LabelTypeRuleDef item)
        {
            try
            {
                iILabelTypeRepository.UpdateAndInsertLabeTypeRuleDefered(uow, item);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteLabelTypeRuleDefered(IUnitOfWork uow, string labelType)
        {
            try
            {
                iILabelTypeRepository.DeleteLabelTypeRuleDefered(uow, labelType);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateAndInsertModelConstValueDefered(IUnitOfWork uow, string labelType, string modelConstValue, string editor)
        {
            try
            {
                iILabelTypeRepository.UpdateAndInsertModelConstValueDefered(uow, labelType, modelConstValue, editor);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateAndInsertDeliveryConstValue(IUnitOfWork uow, string labelType, string deliveryConstValue, string editor)
        {
            try
            {
                iILabelTypeRepository.UpdateAndInsertDeliveryConstValue(uow, labelType, deliveryConstValue, editor);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateAndInsertPartConstValueDefered(IUnitOfWork uow, string labelType, int bomLevel, string partConstValue, string editor)
        {
            try
            {
                iILabelTypeRepository.UpdateAndInsertPartConstValueDefered(uow, labelType, bomLevel, partConstValue, editor);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        public static String Null2String(Object _input)
        {
            if (_input == null)
            {
                return "";
            }
            return _input.ToString().Trim();
        }


    }
}
