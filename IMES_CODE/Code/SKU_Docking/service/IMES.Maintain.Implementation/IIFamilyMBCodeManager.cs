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
using IMES.FisObject.Common.Line;
using IMES.FisObject.Common.Station;
using IMES.FisObject.Common.Part.PartPolicy;
namespace IMES.Maintain.Implementation
{

    public class IFamilyMBCodeManager : MarshalByRefObject, IFamilyMBCode
    {

        #region IFamilyMBCode 成员
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
        public ILineRepository lineRepository = RepositoryFactory.GetInstance().GetRepository<ILineRepository, Line>();
        IStationRepository stationRepository = RepositoryFactory.GetInstance().GetRepository<IStationRepository>();
        IPartPolicyRepository prtPcyRepository = RepositoryFactory.GetInstance().GetRepository<IPartPolicyRepository>();
        IMBRepository MBRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository>();
        public void AddCombineKPSetting(StationCheckInfo item)
        {
            try
            {
                stationRepository.InsertStationCheckInfo(item);
            }
            catch (Exception ee)
            {
                logger.Error(ee.Message);
                throw ee;
            }
        }

        public IList<string> GetLine()
        {
            IList<string> line = new List<string>();
            try
            {
                string[] lineType = new string[2];
                lineType[0] = "FA";
                lineType[1] = "PAK";
                line = lineRepository.GetLinePrefixListByStages(lineType);
            }
            catch (Exception ee)
            {
                logger.Error(ee.Message);
                throw ee;
            }
            return line;
        }

        public IList<StationInfo> GetStation()
        {
            IList<StationInfo> station = new List<StationInfo>();
            try
            {
                station = stationRepository.GetStationInfoListByNotLikeStationType("SA");
            }
            catch (Exception ee)
            {
                logger.Error(ee.Message);
                throw ee;
            }
            return station;
        }

        public IList<string> GetCheckType()
        {
            IList<string> checkType = new List<string>();
            try
            {
                CheckItemTypeInfo condition = new CheckItemTypeInfo();
                checkType = prtPcyRepository.GetNameFromCheckItemType(condition);
            }
            catch (Exception ee)
            {
                logger.Error(ee.Message);
                throw ee;
            }
            return checkType;
        }

        public IList<StationCheckInfo> GetAllCombineKPSettingItems()
        {
            IList<StationCheckInfo> dataLst = new List<StationCheckInfo>();
            try
            {
                StationCheckInfo cond = new StationCheckInfo();
                dataLst = stationRepository.GetStationCheckInfoList(cond);
            }
            catch (Exception ee)
            {
                logger.Error(ee.Message);
                throw ee;
            }
            return dataLst;
        }

        public IList<FamilyMbInfo> GetAllFamilyMbItems()
        {
            IList<FamilyMbInfo> dataLst = new List<FamilyMbInfo>();
            try
            {
                FamilyMbInfo condition = new FamilyMbInfo();
                dataLst = MBRepository.GetFamilyMbInfoList(condition);
            }
            catch (Exception ee)
            {
                logger.Error(ee.Message);
                throw ee;
            }
            return dataLst;
        }

        public void RemoveFamilyMb(FamilyMbInfo item)
        {
            try
            {
                MBRepository.DeleteFamilyMbInfo(item);
            }
            catch (Exception ee)
            {
                logger.Error(ee.Message);
                throw ee;
            }
        }

        public void UpdateFamilyMb(FamilyMbInfo item, FamilyMbInfo cond)
        {
            try
            {
                MBRepository.ModifyFamilyMbInfo(item, cond);
            }
            catch (Exception ee)
            {
                logger.Error(ee.Message);
                throw ee;
            }
        }
        public void AddFamilyMb(FamilyMbInfo item)
        {
            try
            {
                MBRepository.AddFamilyMbInfo(item);
            }
            catch (Exception ee)
            {
                logger.Error(ee.Message);
                throw ee;
            }
        }

        public void RemoveCombineKPSetting(StationCheckInfo item)
        {
            try
            {
                stationRepository.DeleteStationCheckInfo(item);
            }
            catch (Exception ee)
            {
                logger.Error(ee.Message);
                throw ee;
            }
        }

        public void UpdateCombineKPSetting(StationCheckInfo item, StationCheckInfo cond)
        {
            try
            {
                stationRepository.UpdateStationCheckInfo(item, cond);
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
