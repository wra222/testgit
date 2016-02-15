/*
 * INVENTEC corporation (c)2010 all rights reserved. 
 * Description: 
 *              
 * Update: 
 * Date         Name            Reason 
 * ========== ================= =====================================
 * 2010-2-20   itc210001        create
 * 
 * Known issues:Any restrictions about this file 
 *          
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Maintain.Interface;
using IMES.DataModel;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectBase;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.Model;

namespace IMES.Maintain.Implementation
{
    public class AllKindsOfTypeManager : MarshalByRefObject,IAllKindsOfType
    {

        public IList<AreaDef> GetAreaList()
        {
            IList<AreaDef> retLst = null;

            try
            {
                IProductRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository>();
                retLst = itemRepository.GetAreaList();
                return retLst;
            }
            catch (Exception)
            {
                throw;
            }
           
        }

        public IList<string> GetFamilyList()
        {
            IList<string> retLst = null;

            try
            {
                IModelRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IModelRepository>();
                retLst = itemRepository.GetFamilyList();
                return retLst;
            }
            catch(Exception)
            {
                throw;
            }
        }

        public IList<TraceStdInfo> GetTraceStdList(string family, string area)
        {
            IList<TraceStdInfo> retLst = null;
            try
            {
                IProductRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository>();
                retLst = itemRepository.GetTraceStdList(family, area);
                return retLst;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void SaveAllKindsOfTypeInfo(TraceStdInfo item)
        {
            try
            {
                IProductRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository>();
                itemRepository.SaveAllKindsOfTypeInfo(item);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteResult(TraceStdInfo item)
        {
            try
            {
                IProductRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository>();
                itemRepository.DeleteResult(item);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int GetId(TraceStdInfo item)
        {
            int retId = 0;
            try
            {
                IProductRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository>();
                retId = itemRepository.GetId(item);
                return retId;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateAllKindsOfTypeInfo(TraceStdInfo item)
        {
            try
            {
                IProductRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository>();
                itemRepository.UpdateAllKindsOfTypeInfo(item);
            }
            catch(Exception)
            {
                throw;
            }
        }

        //public void SaveAllKindsOfTypeInfo(IMES.DataModel.TraceStdInfo item)
        //{
        //    try
        //    {
        //        IProductRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository>();
        //        itemRepository.SaveAllKindsOfTypeInfo(item);
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        public int CheckExistsRecord(TraceStdInfo item)
        {
            try
            {
                int count = 0;
                IProductRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository>();
                count = itemRepository.CheckExistsRecord(item);
                return count;
            }
            catch (Exception)
            {
                throw;
            }
        }
        //public IList<SelectInfoDef> GetCustomerFamilyList()
        //{

 
        //    List<SelectInfoDef> result = new List<SelectInfoDef>();
        //    if (customer == "")
        //    {
        //        return result;
        //    }

        //    try
        //    {
        //        IFamilyRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IFamilyRepository>();
        //        IList<Family> getData = itemRepository.GetFamilyList(customer);
        //        for (int i = 0; i < getData.Count; i++)
        //        {
        //            SelectInfoDef item = new SelectInfoDef();
        //            item.Text = getData[i].FamilyName;
        //            item.Value = getData[i].FamilyName;
        //            result.Add(item);
        //        }

        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }

        //    return result;
        //}
    }
}
