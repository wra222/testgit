using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Workflow.Runtime;
using System.Globalization; 
using IMES.DataModel;
using IMES.FisObject.FA.Product;
using IMES.FisObject.Common.Repair;
using IMES.FisObject.Common.MO;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.Infrastructure.UnitOfWork;
using IMES.Station.Interface.StationIntf;
using log4net;

namespace IMES.Station.Implementation
{
    /// <summary>
    /// IMES service for ProductionPlan.
    /// </summary>
    public class ProductionPlan : MarshalByRefObject, IProductionPlan
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
        private IMORepository MORepository = RepositoryFactory.GetInstance().GetRepository<IMORepository>();
        #region Implementation of IProductionPlan

        /// <summary>
        /// 获取表格内容
        /// </summary>
        public IList<ProductPlanInfo> GetProductPlanByLineAndShipDate(string line, DateTime shipDate)
        {
            logger.Debug("(ProductPlanInfo)GetProductPlanByLineAndShipDate starts");
            try
            {
                return MORepository.GetProductPlanByLineAndShipDate(line, shipDate);
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
                logger.Debug("(ProductPlanInfo)GetProductPlanByLineAndShipDate end");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public IList<ProductPlanLog> UploadProductPlan(IList<TbProductPlan> prodPlanList, string combinedPO)
        {
            logger.Debug("(ProductPlanInfo)UploadProductPlan starts");
            try
            {
                return MORepository.UploadProductPlan(prodPlanList, combinedPO);
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
                logger.Debug("(ProductPlanInfo)UploadProductPlan end");
            }
        }


        public IList<ProductPlanLog> GetProductPlanMOByLineAndShipDate(string line, DateTime shipDate, string station)
        { 
            logger.Debug("(ProductPlanInfo)GetProductPlanMOByLineAndShipDate starts");
            try
            {
                return MORepository.GetProductPlanMOByLineAndShipDate(line, shipDate, station);
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
                logger.Debug("(ProductPlanInfo)GetProductPlanMOByLineAndShipDate end");
            }
        }

        public IList<ProductPlanLog> GetProductPlanLogByLineAndShipDateAndAction(string line, DateTime shipDate, string action)
        {
            logger.Debug("(ProductPlanInfo)GetProductPlanLogByLineAndShipDateAndAction starts");
            try
            {
                return MORepository.GetProductPlanLogByLineAndShipDateAndAction(line, shipDate, action);
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
                logger.Debug("(ProductPlanInfo)GetProductPlanLogByLineAndShipDateAndAction end");
            }
        }

        public IList<ProductPlanLog> UploadProductPlan_Revise(IList<TbProductPlan> ProdPlanList, string combinedPO)
        {
            logger.Debug("(ProductPlanInfo)UploadProductPlan_Revise starts");
            try
            {
                return MORepository.UploadProductPlan_Revise(ProdPlanList, combinedPO);
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
                logger.Debug("(ProductPlanInfo)UploadProductPlan_Revise end");
            }
        
        }
        #endregion
    }
}
