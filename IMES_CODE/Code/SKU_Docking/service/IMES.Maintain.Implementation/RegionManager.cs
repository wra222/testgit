using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using IMES.FisObject.Common.Part;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Maintain.Interface.MaintainIntf;
using IMES.DataModel;
using IMES.FisObject.Common.Model;
using IMES.Infrastructure.UnitOfWork;
using IMES.FisObject.Common.Misc;

namespace IMES.Maintain.Implementation
{
    public class RegionManager : MarshalByRefObject, IRegion
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public IPartRepository partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
        public IModelRepository modelRepository = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();
        public IMiscRepository iMiscRepository = RepositoryFactory.GetInstance().GetRepository<IMiscRepository>();

        public IList<RegionInfo> GetRegionList()
        {
            IList<RegionInfo> regionList = new List<RegionInfo>();
            IList<Region> fisObjectList = partRepository.GetRegionList();
            if (fisObjectList != null)
            {
                foreach (Region temp in fisObjectList)
                {
                    RegionInfo region = new RegionInfo();
                    region.Region = temp.region;
                    region.Description = temp.Description;
                    region.Editor = temp.Editor;
                    region.Cdt = temp.Cdt;
                    region.Udt = temp.Udt;

                    regionList.Add(region);
                }
            }
            return regionList;
        }

        public void AddRegion(RegionInfo region)
        {
            Region fisObject = new Region();
            fisObject.region = region.Region;
            fisObject.Description = region.Description;
            fisObject.Editor = region.Editor;
            fisObject.Cdt = region.Cdt;
            fisObject.Udt = region.Udt;
            partRepository.AddRegion(fisObject);
        }

        public void UpdateRegion(RegionInfo region, string regionName)
        {
            Region fisObject = new Region();
            fisObject.region = region.Region;
            fisObject.Description = region.Description;
            fisObject.Editor = region.Editor;
            fisObject.Cdt = region.Cdt;
            fisObject.Udt = region.Udt;
            partRepository.UpdateRegion(fisObject, regionName);
        }

        public void DeleteRegion(string region)
        {
            UnitOfWork unit = new UnitOfWork();
            modelRepository.DeleteModelInfoNameByRegionDefered(unit, region);
            partRepository.DeleteRegionByNameDefered(unit, region);
            unit.Commit();
        }

        public bool IFRegionIsExists(string region)
        {
            return partRepository.IFRegionIsExists(region);
        }

        public bool IFRegionInUSE(string region)
        {
            return partRepository.IFRegionIsInUse(region);
        }

        #region new Region

        public IList<RegionInfo> GetRegionList(RegionInfo condition)
        {
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                IList<RegionInfo> ret = iMiscRepository.GetData<IMES.Infrastructure.Repository._Metas.Region, RegionInfo>(condition);
                return ret;
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                throw;
            }
            finally
            {
                logger.DebugFormat("END: {0}()", methodName);
            }


        }

        public void InsertRegion(RegionInfo item)
        {
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                iMiscRepository.InsertData<IMES.Infrastructure.Repository._Metas.Region, RegionInfo>(item);
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                throw;
            }
            finally
            {
                logger.DebugFormat("END: {0}()", methodName);
            }
        }

        public void UpdateRegion(RegionInfo condition, RegionInfo item)
        {
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                iMiscRepository.UpdateData<IMES.Infrastructure.Repository._Metas.Region, RegionInfo>(condition, item);
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                throw;
            }
            finally
            {
                logger.DebugFormat("END: {0}()", methodName);
            }
        }

        public void DeleteRegion(RegionInfo condition)
        {
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                iMiscRepository.DeleteData<IMES.Infrastructure.Repository._Metas.Region, RegionInfo>(condition);
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                throw;
            }
            finally
            {
                logger.DebugFormat("END: {0}()", methodName);
            }
        }

        #endregion 
    }
}
