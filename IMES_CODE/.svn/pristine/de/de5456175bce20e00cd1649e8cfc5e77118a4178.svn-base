using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Maintain.Interface.MaintainIntf;
using IMES.FisObject.PCA.MB;
using log4net;
using IMES.DataModel;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure;
using IMES.FisObject.FA.Product;

namespace IMES.Maintain.Implementation
{
    public class ITCNDCheckSettingManager : MarshalByRefObject, IITCNDCheckSetting
    {

        #region IITCNDCheckSetting 成员
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();

        public void AddITCNDCheckSetting(IMES.DataModel.ITCNDCheckSettingDef item)
        {
            try
            {
                //ITC-1361-0159
                ITCNDCheckSettingDef cond = new ITCNDCheckSettingDef();
                cond.line = item.line;
                cond.station = item.station;
                cond.checkItem = item.checkItem;

                IList<ITCNDCheckSettingDef> dataLst = new List<ITCNDCheckSettingDef>();
                dataLst = productRepository.GetExistITCNDCheckSetting(cond);
                if (dataLst != null && dataLst.Count > 0)
                {
                    List<string> erpara = new List<string>();
                    FisException ex;
                    ex = new FisException("DMT151", erpara);
                    throw ex;
                }
                productRepository.AddITCNDCheckSetting(item);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<IMES.DataModel.ITCNDCheckSettingDef> GetAllITCNDCheckSettingItems()
        {
            IList<ITCNDCheckSettingDef> dataLst = new List<ITCNDCheckSettingDef>();
            try
            {
                ITCNDCheckSettingDef cond = new ITCNDCheckSettingDef();
                dataLst = productRepository.GetExistITCNDCheckSetting(cond);
            }
            catch (Exception ee)
            {
                logger.Error(ee.Message);
                throw ee;
            }
            return dataLst;
        }

        public void RemoveITCNDCheckSetting(IMES.DataModel.ITCNDCheckSettingDef item)
        {
            try
            {
                productRepository.RemoveITCNDCheckSetting(item);
            }
            catch (Exception ee)
            {
                logger.Error(ee.Message);
                throw ee;
            }
        }

        public void UpdateITCNDCheckSetting(ITCNDCheckSettingDef item, ITCNDCheckSettingDef cond)
        {
            try
            {
                productRepository.ChangeITCNDCheckSetting(item, cond);
            }
            catch (Exception ee)
            {
                logger.Error(ee.Message);
                throw ee;
            }
        }

        #endregion
    }
}
