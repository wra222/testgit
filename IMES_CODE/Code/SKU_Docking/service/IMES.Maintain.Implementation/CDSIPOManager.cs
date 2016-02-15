/*
 * INVENTEC corporation ©2012 all rights reserved. 
 * Description:UI for CDSI PO Page
 *             
 * UI:CI-MES12-SPEC-PAK-DATA MAINTAIN(II).docx –2012/05/18
 * UC:CI-MES12-SPEC-PAK-DATA MAINTAIN(II).docx –2012/05/18            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2012-05-18  itc202017             (Reference Ebook SourceCode) Create
 * Known issues:
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Maintain.Interface.MaintainIntf;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using log4net;
using IMES.DataModel;
using IMES.Infrastructure;

namespace IMES.Maintain.Implementation
{
    public class CDSIPOManager : MarshalByRefObject,ICDSIPO
    {

        #region ICDSIPO Members
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        IProductRepository productRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();

        public class IcpSnoDetPoMoInfo : IComparer<SnoDetPoMoInfo>
        {
            public int Compare(SnoDetPoMoInfo x, SnoDetPoMoInfo y)
            {
                return x.snoId.CompareTo(y.snoId);
            }
        }

        public IList<SnoDetPoMoInfo> GetList()
        {
            logger.Debug("(CDSIPOManager) GetList begins.");
            try
            {
                return productRep.GetSnoDetPoMoInfoList_NOT98DN();
                /*SnoDetPoMoInfo cond = new SnoDetPoMoInfo();
                List<SnoDetPoMoInfo> ret = productRep.GetSnoDetPoMoInfoList(cond).ToList();
                ret.Sort(new IcpSnoDetPoMoInfo());
                return ret;
                */
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg, e);
                throw new Exception(e.mErrmsg);
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                throw new SystemException(e.Message);
            }
            finally
            {
                logger.Debug("(CDSIPOManager) GetList ends");
            }
        }

        public void Add(SnoDetPoMoInfo item)
        {
            logger.Debug("(CDSIPOManager) Add begins.");
            try
            {
                productRep.AddSnoDetPoMoInfo(item);
                return;
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg, e);
                throw new Exception(e.mErrmsg);
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                throw new SystemException(e.Message);
            }
            finally
            {
                logger.Debug("(CDSIPOManager) Add ends");
            }
        }

        public void Delete(string id)
        {
            logger.Debug("(CDSIPOManager) Delete begins.");
            try
            {
                SnoDetPoMoInfo cond = new SnoDetPoMoInfo();
                cond.snoId = id;
                productRep.DeleteSnoDetPoMoInfo(cond);
                return;
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg, e);
                throw new Exception(e.mErrmsg);
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                throw new SystemException(e.Message);
            }
            finally
            {
                logger.Debug("(CDSIPOManager) Delete ends");
            }
        }

        public void Update(string id, SnoDetPoMoInfo item)
        {
            logger.Debug("(CDSIPOManager) Update begins.");
            try
            {
                SnoDetPoMoInfo cond = new SnoDetPoMoInfo();
                cond.snoId = id;
                productRep.UpdateSnoDetPoMoInfo(item, cond);
                return;
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg, e);
                throw new Exception(e.mErrmsg);
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                throw new SystemException(e.Message);
            }
            finally
            {
                logger.Debug("(CDSIPOManager) Update ends");
            }
        }

        public bool CheckIfExist(string id)
        {
            logger.Debug("(CDSIPOManager) CheckIfExist begins.");
            try
            {
                SnoDetPoMoInfo cond = new SnoDetPoMoInfo();
                cond.snoId = id;
                return productRep.CheckExistSnoDetPoMoInfo(cond); ;
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg, e);
                throw new Exception(e.mErrmsg);
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                throw new SystemException(e.Message);
            }
            finally
            {
                logger.Debug("(CDSIPOManager) CheckIfExist ends");
            }
        }

        public bool CheckIfExistProduct(string id)
        {
            logger.Debug("(CDSIPOManager) CheckIfExistProduct begins.");
            try
            {
                return (productRep.Find(id) != null);
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg, e);
                throw new Exception(e.mErrmsg);
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                throw new SystemException(e.Message);
            }
            finally
            {
                logger.Debug("(CDSIPOManager) CheckIfExistProduct ends");
            }
        }

        public string GetMOByProductID(string id)
        {
            logger.Debug("(CDSIPOManager) GetMOByProductID begins.");
            try
            {
                return productRep.GetProductByIdOrSn(id).MO;
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg, e);
                throw new Exception(e.mErrmsg);
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                throw new SystemException(e.Message);
            }
            finally
            {
                logger.Debug("(CDSIPOManager) GetMOByProductID ends");
            }
        }

        #endregion
    }
}
