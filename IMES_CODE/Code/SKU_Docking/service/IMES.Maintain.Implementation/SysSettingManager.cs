using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.FisObject.PCA.MBModel;
using IMES.FisObject.Common.Line;
using IMES.FisObject.PCA.MBMO;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectBase;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.Station;
using IMES.FisObject.Common.Model;
using IMES.FisObject.Common.Part;
using IMES.FisObject.Common.Defect;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PAK.DN;
using IMES.FisObject.PAK.Pallet;
using IMES.FisObject.Common.CheckItem;
using IMES.FisObject.PCA.MB;
using IMES.FisObject.Common.Repair;
using IMES.FisObject.Common.Misc;
using IMES.FisObject.Common.MO;
using IMES.DataModel;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Warranty;
using IMES.Maintain.Interface.MaintainIntf;
using IMES.Infrastructure.UnitOfWork;
using IMES.FisObject.Common.NumControl;
using System.Data;

namespace IMES.Maintain.Implementation
{
    
    public class SysSettingManager : MarshalByRefObject,ISysSetting
    {
        public IList<SysSettingInfo> GetSysSettingListByCondition(SysSettingInfo sysSettingCondition)
        {
            IList<SysSettingInfo> ret = new List<SysSettingInfo>();
            try
            {
                IPartRepository iPartRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository>();
                ret = iPartRepository.GetSysSettingInfoes(sysSettingCondition);
            }
            catch (Exception)
            {
                throw;
            }
            return ret;
        }

        public void AddSysSettingInfo(SysSettingInfo item)
        {
            IList<SysSettingInfo> ret = new List<SysSettingInfo>();
            try
            {
                IPartRepository iPartRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository>();
                iPartRepository.AddSysSettingInfo(item);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateSysSettingInfo(SysSettingInfo setValue, SysSettingInfo condition)
        {
            IList<SysSettingInfo> ret = new List<SysSettingInfo>();
            try
            {
                IPartRepository iPartRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository>();
                iPartRepository.UpdateSysSettingInfo(setValue, condition);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteSysSettingInfo(int id)
        {
            try
            {
                IPartRepository iPartRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository>();
                iPartRepository.DeleteSysSettingInfo(id);
            }
            catch (Exception)
            {
                throw;
            } 
        }
    }
}
