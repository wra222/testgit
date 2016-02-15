/*
 * INVENTEC corporation (c)2010 all rights reserved. 

 * Known issues:
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Maintain.Interface.MaintainIntf;
using log4net;
using IMES.DataModel;
using IMES.FisObject.Common.Part;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure;
using IMES.FisObject.PCA.EcrVersion;
using IMES.Infrastructure.UnitOfWork;

namespace IMES.Maintain.Implementation
{
    class FRUMBVersionManager : MarshalByRefObject, IFRUMBVersion
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region IFRUMBVersion members

        public IList<string> GetPartNoInFruMBVer()
        {
     
            IEcrVersionRepository ipr = RepositoryFactory.GetInstance().GetRepository<IEcrVersionRepository, EcrVersion>();
            try
            {
                IList<string> lstPartNoInFruMBVer = ipr.GetPartNoInFruMBVer();
                return lstPartNoInFruMBVer;
          
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


        public IList<FruMBVerInfo> GetFruMBVer(string partNo)
        {
        
            IEcrVersionRepository ipr = RepositoryFactory.GetInstance().GetRepository<IEcrVersionRepository, EcrVersion>();
         
            try
            {
                FruMBVerInfo condition = new FruMBVerInfo() { partNo = partNo };
                return ipr.GetFruMBVer(condition);
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


        public void InsertFruMBVer(FruMBVerInfo item)
        {
            logger.Debug("(FRUMBVersionManager)InsertFruMBVer, [PartNo]:" + item.partNo);
        
            IEcrVersionRepository ier = RepositoryFactory.GetInstance().GetRepository<IEcrVersionRepository, EcrVersion>();

            try
            {
                FruMBVerInfo con = new FruMBVerInfo { partNo = item.partNo, mbCode = item.mbCode, ver = item.ver };

                IList<FruMBVerInfo> lst = ier.GetFruMBVer(con);
                if (lst.Count > 0)
                { throw new FisException("Duplicate data"); }

                ier.InsertFruMBVer(item);
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
            finally
            {
                logger.Debug("(FRUMBVersionManager)InsertFruMBVer, [PartNo]:" + item.partNo);
            
            }

        
        }

        public void UpdateFruMBVer(FruMBVerInfo item)
        {
            logger.Debug("(FRUMBVersionManager)SaveECRVersion start, [PartNo]:" + item.partNo);
            IEcrVersionRepository ier = RepositoryFactory.GetInstance().GetRepository<IEcrVersionRepository, EcrVersion>();
            try
            {
                FruMBVerInfo con = new FruMBVerInfo { partNo = item.partNo, mbCode = item.mbCode, ver = item.ver };

                IList<FruMBVerInfo> lst = ier.GetFruMBVer(con);
                if (lst.Count > 1 ||  (lst.Count==1 && lst[0].id!=item.id)   )
                { throw new FisException("Duplicate data"); }
                ier.UpdateFruMBVer(item);
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
            finally
            {
                logger.Debug("(FRUMBVersionManager)SaveECRVersion end, [PartNo]:" + item.partNo);
            }            
        }



        public void RemoveFruMBVer(int id)
        {
            logger.Debug("(FRUMBVersionManager)RemoveFruMBVer start, [id]:" + id.ToString());
            IEcrVersionRepository ier = RepositoryFactory.GetInstance().GetRepository<IEcrVersionRepository, EcrVersion>();
            try
            {
                ier.RemoveFruMBVer(id);
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
            finally
            {
                logger.Debug("(FRUMBVersionManager)RemoveFruMBVer start, [id]:" + id.ToString());
            } 
        }

        #endregion

       


      
    }
}
