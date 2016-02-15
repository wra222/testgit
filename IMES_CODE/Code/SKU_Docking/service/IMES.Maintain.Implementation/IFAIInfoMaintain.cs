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
using IMES.FisObject.Common.Misc;
namespace IMES.Maintain.Implementation
{

    public class IFAIInfoMaintainManager : MarshalByRefObject, IFAIInfoMaintain
    {

        #region IFAIInfoMaintain 成员
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        IMiscRepository miscRepository = RepositoryFactory.GetInstance().GetRepository<IMiscRepository>();
        public void AddFAIInfoMaintain(FaiInfo item)
        {
            try
            {
                miscRepository.AddFaiInfo(item);
            }
            catch (Exception ee)
            {
                logger.Error(ee.Message);
                throw ee;
            }
        }

        public IList<FaiInfo> GetAllFAIInfoMaintainItems()
        {
            IList<FaiInfo> dataLst = new List<FaiInfo>();
            try
            {
                FaiInfo  cond = new FaiInfo ();
                dataLst = miscRepository.GetFaiInfoList(cond);
            }
            catch (Exception ee)
            {
                logger.Error(ee.Message);
                throw ee;
            }
            return dataLst;
        }

        public void UpdateFAIInfoMaintain(FaiInfo item, FaiInfo cond)
        {
            try
            {
                miscRepository.ModifyFaiInfo(item, cond);
            }
            catch (Exception ee)
            {
                logger.Error(ee.Message);
                throw ee;
            }
        }
        public IList<FaiInfo> GetFAIInfoMaintainItems(DateTime finTime, string iecpnPrefix, string hpqpnPrefix, string snoPrefix)
        {
            IList<FaiInfo> dataLst = new List<FaiInfo>();
            try
            {
                dataLst = miscRepository.GetFaiInfoByLikes( finTime,  iecpnPrefix,  hpqpnPrefix,  snoPrefix);
            }
            catch (Exception ee)
            {
                logger.Error(ee.Message);
                throw ee;
            }
            return dataLst;
        }
        #endregion
    }
}
