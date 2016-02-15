/*
 * INVENTEC corporation (c)2010 all rights reserved. 
 * Description:  ICT input interface implement
 *             
 * Update: 
 * Date       Name                  Reason 
 * ========== ===================== =====================================
 * 2010-1-20  liu xiaoling          Create 
 * Known issues:
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;
using IMES.FisObject.Common.Model;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.UnitOfWork;
using IMES.Maintain.Interface.MaintainIntf;
using log4net;
using IMES.FisObject.Common.FisBOM;
namespace IMES.Maintain.Implementation
{

    class FamilyManagerEx : MarshalByRefObject, IFamilyManagerEx
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public IList<FamilyInfo> FindFamiliesByCustomerOrderByFamily(string customer)
        {
            try
            {
                IFamilyRepository familyRepository = RepositoryFactory.GetInstance().GetRepository<IFamilyRepository, Family>();
                IList<FamilyInfo> retLst = null;
                retLst = familyRepository.FindFamiliesByCustomerOrderByFamily(customer);
                return retLst;
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg);
                throw e;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw;
            }
       
        }

        public IList<string> GetFamilyByCustomer(string customer)
        {
            try
            {
                IFamilyRepositoryEx familyRepository = RepositoryFactory.GetInstance().GetRepository<IFamilyRepositoryEx>();
                IList<string> retLst = null;
                retLst = familyRepository.GetFamilyByCustomer(customer);
                return retLst;
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg);
                throw e;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw;
            }
        }

        public IList<IMES.DataModel.ModelInfo> GetModelByFamily(string family)
        {
            try
            {
                var currentModelRepository = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();
                IList<IMES.DataModel.ModelInfo> lst = currentModelRepository.GetModelListByFamilyAndStatus(family, 1);
                return lst;
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg);
                throw e;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw;
            }
        }

        public void DeleteFamily(string family, string customer, string editor)
        {
            try
            {
                logger.Info("DeleteFamily family=" + family + ", customer=" + customer + ", editor=" + editor);
                IFamilyRepositoryEx familyRepository = RepositoryFactory.GetInstance().GetRepository<IFamilyRepositoryEx>();
                familyRepository.DeleteFamilyEx(family, customer);
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg);
                throw e;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw;
            }
        }

    }

}
