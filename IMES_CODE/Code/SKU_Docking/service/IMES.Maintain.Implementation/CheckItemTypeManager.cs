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

    public class CheckItemTypeManager : MarshalByRefObject, ICheckItemType
    {
        public IList<CheckItemTypeInfo> GetCheckItemTypeByCondition(CheckItemTypeInfo condition)
        {
            IList<CheckItemTypeInfo> ret = new List<CheckItemTypeInfo>();
            try
            {
                IPartRepository iPartRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository>();
                ret = iPartRepository.GetCheckItemType(condition);
            }
            catch (Exception)
            {
                throw;
            }
            return ret;
        }

        public void AddCheckItemTypeInfo(CheckItemTypeInfo item)
        {
            IList<CheckItemTypeInfo> ret = new List<CheckItemTypeInfo>();
            try
            {
                IPartRepository iPartRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository>();
                iPartRepository.InsertCheckItemType(item);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateCheckItemTypeInfo(CheckItemTypeInfo item)
        {
            IList<CheckItemTypeInfo> ret = new List<CheckItemTypeInfo>();
            try
            {
                IPartRepository iPartRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository>();
                iPartRepository.UpdateCheckItemType(item);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteCheckItemTypeInfo(string name)
        {
            try
            {
                IPartRepository iPartRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository>();
                iPartRepository.DeleteCheckItemType(name);
            }
            catch (Exception)
            {
                throw;
            } 
        }
    }
}
