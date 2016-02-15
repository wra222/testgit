using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Workflow.Runtime;
using System.Globalization;
using IMES.DataModel;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PAK.BSam;
using IMES.FisObject.Common.Repair;
using IMES.FisObject.Common.MO;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.Infrastructure.UnitOfWork;
using IMES.Infrastructure.Repository.PAK;
using IMES.Infrastructure.Repository.Common;
using IMES.Station.Interface.StationIntf;
using log4net;
using IMES.FisObject.Common.Model;


namespace IMES.Station.Implementation
{
    /// <summary>
    /// IMES service for TravelCardPrintProductPlan.
    /// </summary>
    public partial class TravelCardPrintProductPlan : MarshalByRefObject, ITravelCardPrintProductPlan
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //private static readonly Session.SessionType theType = Session.SessionType.Common;
        //private IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
        private IMORepository MORepository = RepositoryFactory.GetInstance().GetRepository<IMORepository>();
        private IBSamRepository BSamRepository = RepositoryFactory.GetInstance().GetRepository<IBSamRepository>();
        private IModelRepository ModelRepository = RepositoryFactory.GetInstance().GetRepository<IModelRepository>();

        //BSamRepository
        #region ITravelCardPrint Members
        /// <summary>
        /// GetProductPlanFamily
        /// </summary>
        public IList<ProductPlanFamily> GetProductPlanFamily(string line, DateTime shipdate)
        {

            logger.Debug("(TravelCardPrintProductPlan)GetProductPlanFamily starts");
            try
            {
                
                return MORepository.GetProductPlanFamily(line, shipdate);
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
                logger.Debug("(TravelCardPrintProductPlan)GetProductPlanFamily end");
            }
        }

        /// <summary>
        /// GetProductPlanModel
        /// </summary>
        public IList<ProductPlanInfo> GetProductPlanModel(string line, DateTime shipdate, string family)
        {

            logger.Debug("(TravelCardPrintProductPlan)GetProductPlanModel starts");
            try
            {
                return MORepository.GetProductPlanModel(line, shipdate, family);
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
                logger.Debug("(TravelCardPrintProductPlan)GetProductPlanModel end");
            }
        }

        /// <summary>
        /// GetPlanMO
        /// </summary>
        public MOPlanInfo GetPlanMO(int id)
        {
            logger.Debug("(TravelCardPrintProductPlan)GetPlanMO starts");
            try
            {
                return MORepository.GetPlanMO(id);
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
                logger.Debug("(TravelCardPrintProductPlan)GetPlanMO end");
            }
        }

        /// <summary>
        /// CheckedBSamModel
        /// </summary>
        public bool CheckedBSamModel(string Model)
        {
            logger.Debug("(TravelCardPrintProductPlan)CheckedBSamModel starts");
            try
            {
                BSamModel BSamModel = BSamRepository.GetBSamModel(Model);
                if (BSamModel != null)
                {
                    return true;
                }
                return false;
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
                logger.Debug("(TravelCardPrintProductPlan)CheckedBSamModel end");
            }

        }

        //GetC_SKUInfo
        public bool CheckedC_SKU(string Model, string Name)
        {
            logger.Debug("(TravelCardPrintProductPlan)CheckedC_SKU starts");
            try
            {
                IList<IMES.FisObject.Common.Model.ModelInfo> ModelList = ModelRepository.GetModelInfoByModelAndName(Model, Name);
                if (ModelList.Count > 0)
                {
                    return true;
                }
                return false;
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
                logger.Debug("(TravelCardPrintProductPlan)CheckedC_SKU end");
            }

        }


        public bool CheckedBSamModelAndC_SKU(string Model, string Name)
        {
            logger.Debug("(TravelCardPrintProductPlan)CheckedBSamModelAndC_SKU starts");
            try
            {
                BSamModel BSamModel = BSamRepository.GetBSamModel(Model);
                IList<IMES.FisObject.Common.Model.ModelInfo> ModelList = ModelRepository.GetModelInfoByModelAndName(Model, Name);

                if (ModelList != null || ModelList.Count != 0)
                {
                    if (BSamModel.HP_C_SKU.ToString() == ModelList[0].Value.ToString())
                    {
                        return true;
                    }
                }
                return false;
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
                logger.Debug("(TravelCardPrintProductPlan)CheckedBSamModelAndC_SKU end");
            }

        }

        public IList<PilotMoInfo> GetPilotMo(PilotMoInfo condition, IList<string> combinedState)
        {
            logger.Debug("(TravelCardPrintProductPlan)GetPilotMo start, model:" + condition.model);
            try
            {
                IMORepository iMORepository = RepositoryFactory.GetInstance().GetRepository<IMORepository>();
                IList<PilotMoInfo> tempPilotMo = iMORepository.SearchPilotMo(condition, combinedState);

                return tempPilotMo;
            }
            catch (FisException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new SystemException(e.Message);
            }
            finally
            {
                logger.Debug("(TravelCardPrintProductPlan)GetPilotMo end, model:" + condition.model);
            }
        }

        #endregion 


    }
}
