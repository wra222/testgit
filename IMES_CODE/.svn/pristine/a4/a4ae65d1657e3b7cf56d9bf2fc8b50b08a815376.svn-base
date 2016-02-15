using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Station.Interface.StationIntf;
using IMES.Infrastructure;
using IMES.Infrastructure.WorkflowRuntime;
//using IMES.Station.Interface.CommonIntf;
using IMES.FisObject.FA.Product;
using System.Workflow.Runtime;
using IMES.DataModel;
using log4net;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.PCA.MBMO;
using IMES.FisObject.Common.Model;
using IMES.FisObject.Common.Part;
using IMES.Infrastructure.Extend;
using IMES.FisObject.Common.PrintLog;
using IMES.Route;

namespace IMES.Station.Implementation
{

    public partial class PrDelete : MarshalByRefObject, IPrDelete
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        #region IPrDelete Members

        public IList<string> FindInfoForPrSN(string prsn)
        {
            IList<string> result = new List<string>();
            IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            IPartRepository partRep = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            try
            {
                logger.Debug("start Get Product ID & SN By PrSN");

                bool bFind = false;
                string proId = string.Empty;
                string customSn = string.Empty;
                string model = string.Empty;
                string partno = string.Empty;
                IList<IProductPart> product_parts = new List<IProductPart>();
                product_parts = productRepository.GetProductPartsByBomNodeTypeAndPartSn("PS", prsn);
                if (product_parts != null && product_parts.Count > 0)
                {
                    foreach (IProductPart temp in product_parts)
                    {
                        IProduct currentProduct = null;
                        currentProduct = productRepository.Find(temp.ProductID);
                        if (currentProduct == null)
                        {
                            continue;
                        }
                        else
                        {
                            proId = temp.ProductID;
                            customSn = currentProduct.CUSTSN;
                            model = currentProduct.Model;
                            partno = temp.PartID;
                            bFind = true;
                            break;
                        }
                    }
                    if (bFind == true)
                    {
                        result.Add(proId);
                        result.Add(customSn);
                        result.Add(model);
                        result.Add(partno);
                    }
                    else
                    {
                        FisException ex;
                        List<string> erpara = new List<string>();
                        ex = new FisException("CHK808", erpara);
                        throw ex;
                    }
                }
                else
                {
                    FisException ex;
                    List<string> erpara = new List<string>();
                    ex = new FisException("CHK808", erpara);
                    throw ex;
                }                
            }
            catch (FisException ex)
            {
                result.Add(ex.Message);
                throw ex;
            }
            catch (Exception e)
            {
                result.Add(e.Message);
                throw new SystemException(e.Message);
            }
            finally
            {
                logger.Debug("end Get Product ID & SN By PrSN");
            }
            return result;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputSN"></param>
        /// <param name="productID"></param>
        /// <param name="productSN"></param>
        /// <param name="checkSN"></param>
        /// <param name="partNo"></param>
        /// <param name="model"></param>
        /// <param name="station"></param>
        /// <param name="editor"></param>
        /// <param name="pdline"></param>
        /// <param name="customer"></param>
        public void DelPrSN(string inputSN, string productID, string productSN, string checkSN, string partNo, 
                            string model, string station, string editor, string pdline, string customer)
        {
            logger.Debug("Delete Product SN:" + inputSN + "," + productID + "," + productSN);
            string currentSessionKey = productID;
            
            try
            {
                if (productSN != checkSN)
                {
                    FisException ex;
                    List<string> erpara = new List<string>();                    
                    ex = new FisException("CHK835", erpara);
                    throw ex;
                }
                Session currentProductSession = SessionManager.GetInstance.GetSession(currentSessionKey, Session.SessionType.Product);
                if (currentProductSession == null)
                {
                    currentProductSession = new Session(currentSessionKey, Session.SessionType.Product, editor, station, pdline, customer);
                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", currentSessionKey);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", currentProductSession);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", "");
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", Session.SessionType.Product);
                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(station, "PrDelete.xoml", "", out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    currentProductSession.AddValue(Session.SessionKeys.PartNo, partNo);
                    currentProductSession.AddValue(Session.SessionKeys.ProductIDOrCustSN, productID);
                    currentProductSession.AddValue(Session.SessionKeys.MONO, model);
                    currentProductSession.AddValue("PRSN", inputSN);
                    currentProductSession.SetInstance(instance);
                    if (!SessionManager.GetInstance.AddSession(currentProductSession))
                    {
                        currentProductSession.WorkflowInstance.Terminate("Session:" + currentSessionKey + " Exists.");
                        FisException ex;
                        List<string> erpara = new List<string>();
                        erpara.Add(currentSessionKey);
                        ex = new FisException("CHK020", erpara);
                        throw ex;
                    }

                    currentProductSession.WorkflowInstance.Start();
                    currentProductSession.SetHostWaitOne();
                }
                else
                {
                    FisException ex;
                    List<string> erpara = new List<string>();
                    erpara.Add(currentSessionKey);
                    ex = new FisException("CHK020", erpara);
                    throw ex;
                }
                if (currentProductSession.Exception != null)
                {
                    if (currentProductSession.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        currentProductSession.ResumeWorkFlow();
                    }

                    throw currentProductSession.Exception;
                }
                return;
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
                logger.Debug("Del PrSN Success!");
            }
        }

        #endregion
    }
}
