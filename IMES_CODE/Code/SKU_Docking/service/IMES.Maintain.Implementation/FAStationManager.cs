/*
 * INVENTEC corporation (c)2010 all rights reserved. 
 * Description: 
 *              
 * Update: 
 * Date         Name            Reason 
 * ========== ================= =====================================
 * 2010-6-23   itc210001        create
 * 
 * Known issues:Any restrictions about this file 
 *          
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Maintain.Interface.MaintainIntf;
using IMES.DataModel;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.FA.Product;

namespace IMES.Maintain.Implementation 
{
    public class FAStationManager : MarshalByRefObject,IFAStation
    {
       

        public IList<string> GetLineList()
        {
            IList<string> retLst = null;
            try
            {
                
                IProductRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository>();
                retLst = itemRepository.GetLineList();
                return retLst;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<FaStationInfo> GetFaStationInfoList()
        {
            IList<FaStationInfo> retLst = null;
            try
            {
                IProductRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository>();
                retLst = itemRepository.GetFaStationInfoList();
                return retLst;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public IList<FaStationInfo> GetFaStationInfoList(string line)
        {
            IList<FaStationInfo> retLst = null;
            try
            {
                IProductRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository>();
                retLst = itemRepository.GetFaStationInfoList(line);
                return retLst;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int CheckExistsRecord(string line, string station)
        {
            int count = 0;
            try
            {
                IProductRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository>();
                count = itemRepository.CheckExistsRecord(line, station);
                return count;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int GetID(string line, string station)
        {
            int id = 0;
            try
            {
                IProductRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository>();
                id = itemRepository.GetID(line, station);
                return id;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateFaStation(FaStationInfo item)
        {
            try
            {
                IProductRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository>();
                itemRepository.UpdateFaStation(item);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void InsertFaStation(FaStationInfo item)
        {
            try
            {
                IProductRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository>();
                itemRepository.InsertFaStation(item);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteFaStationInfo(string line, string station)
        {
            try
            {
                IProductRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository>();
                itemRepository.DeleteFaStationInfo(line, station);
            }
            catch (Exception)
            {
                throw;
            }
        }

       
    }
}
