using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Maintain.Interface.MaintainIntf;
using IMES.FisObject.Common.TPCB;

namespace IMES.Maintain.Implementation
{
    public class TPCBMaintainManager : MarshalByRefObject,ITPCBMaintain
    {
        public  IList<TPCBInfo> GetTpcbList()
        {
            IList<TPCBInfo> retLst = null;
            try
            {
                ITPCBInfoRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<ITPCBInfoRepository>();
                retLst = itemRepository.GetTpcbList();
                return retLst;
            }
            catch(Exception)
            {
                throw;    
            }
        }

        public IList<TPCBInfo> GetTpcbList(string family)
        {
            IList<TPCBInfo> retLst = null;
            try
            {
                ITPCBInfoRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<ITPCBInfoRepository>();
                retLst = itemRepository.GetTpcbList(family);
                return retLst;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void SaveTPCBInfo(TPCBInfo item)
        {
            try
            {
                ITPCBInfoRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<ITPCBInfoRepository>();
                itemRepository.SaveTPCBInfo(item);
            }
            catch (Exception)
            {
                throw;
            }
        }

        //GetID function need mofidy
        public int GetID(string family, string PartNo)
        {
            int id = 0;
            try
            {
                ITPCBInfoRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<ITPCBInfoRepository>();
                id = itemRepository.GetID(family, PartNo);
                return id;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteTPCBInfo(string family, string partNo)
        {
            try
            {
                ITPCBInfoRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<ITPCBInfoRepository>();
                itemRepository.DeleteTPCBInfo(family, partNo);
            }
            catch(Exception)
            {
                throw;
            }
        }

        public IList<TPCBInfo> CheckHasList(string family, string partNo)
        {
            IList<TPCBInfo> retLst = null;
            try
            {
                ITPCBInfoRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<ITPCBInfoRepository>();
                retLst = itemRepository.CheckHasList(family, partNo);
                return retLst;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<TPCBInfo> CheckSameList(TPCBInfo item)
        {
            IList<TPCBInfo> retLst = null;
            try
            {
                ITPCBInfoRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<ITPCBInfoRepository>();
                retLst = itemRepository.CheckSameList(item);
                return retLst;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateTPCBInfo(TPCBInfo item)
        {
            try
            {
                ITPCBInfoRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<ITPCBInfoRepository>();
                itemRepository.UpdateTPCBInfo(item);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void InsertTPCBInfo(TPCBInfo item)
        {
            try
            {
                ITPCBInfoRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<ITPCBInfoRepository>();
                itemRepository.InsertTPCBInfo(item);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int CheckExistsRecord(string family, string partNo)
        {
            int count = 0;
            try
            {
                ITPCBInfoRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<ITPCBInfoRepository>();
                count = itemRepository.CheckExistsRecord(family, partNo);
                return count;
            }
            catch(Exception)
            {
                throw;
            }
        }

    }
}
