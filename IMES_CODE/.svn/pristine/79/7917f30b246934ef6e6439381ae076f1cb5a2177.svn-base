using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;
using IMES.Station.Interface.CommonIntf;
using IMES.Station.Interface.BSamIntf;
using System.Workflow.Runtime;  
using IMES.Infrastructure;
using IMES.Infrastructure.WorkflowRuntime;
using System.Collections;
using IMES.FisObject.Common;
using IMES.FisObject.FA.Product; 
using IMES.Infrastructure.FisObjectRepositoryFramework;
using log4net;
using IMES.Route;
using IMES.FisObject.PAK.Pallet;
using IMES.FisObject.PAK.DN;
using IMES.FisObject.Common.Part;
using IMES.FisObject.PAK.Carton;
using System.Data;
using IMES.Infrastructure.Extend;
using IMES.FisObject.PAK.Pizza;
using IMES.FisObject.Common.FisBOM;
namespace IMES.Station.Implementation
{   /// <summary>
    ///Carton Weight
    /// station :
    /// 本站实现的功能：
    ///    
    ///     
    ///    
    /// </summary> 

    public class CartonWeight : MarshalByRefObject, ICartonWeight
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public ArrayList InputCustSn(string custSn, string line, string editor, string station, string customer,string weight)
        {
            logger.Debug("(CartonWeight)InputFirstCustSn start, custsn:" + custSn + "pdLine:" + line + "editor:" + editor + "station:" + station + "customer:" + customer);

            FisException ex;
            List<string> erpara = new List<string>();
            try
            {

                string sessionKey = custSn;
               // CheckQCStatus(custSn);
                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, Session.SessionType.Product);
                if (currentSession == null)
                {
                    currentSession = new Session(sessionKey, Session.SessionType.Product, editor, station, line, customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", currentSession);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", line);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", Session.SessionType.Product);
                    currentSession.AddValue(Session.SessionKeys.CustSN, custSn);
                    currentSession.AddValue(ExtendSession.SessionKeys.ActualCartonWeight, decimal.Parse(weight));
                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(station, "CartonWeight.xoml", "CartonWeight.rules", out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

           
                    currentSession.SetInstance(instance);

                    if (!SessionManager.GetInstance.AddSession(currentSession))
                    {
                        currentSession.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");
                        erpara.Add(sessionKey);
                        ex = new FisException("CHK020", erpara);
                        throw ex;
                    }

                    currentSession.WorkflowInstance.Start();
                    currentSession.SetHostWaitOne();
                }
                else
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK020", erpara);
                    throw ex;
                }


                if (currentSession.Exception != null)
                {
                    if (currentSession.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        currentSession.ResumeWorkFlow();
                    }

                    throw currentSession.Exception;
                }
                Carton carton = (Carton)currentSession.GetValue(Session.SessionKeys.Carton);
                IList<IMES.FisObject.FA.Product.IProduct> prodList = currentSession.GetValue(Session.SessionKeys.ProdList) as IList<IMES.FisObject.FA.Product.IProduct>;
                string boxid = carton.BoxId;
                ArrayList retLst = new ArrayList();
                retLst.Add(carton.CartonSN);
                string dnNo = prodList[0].DeliveryNo;
                IDeliveryRepository deliveryRep = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
                Delivery dnObj= deliveryRep.Find(dnNo);
                string flag = (string)dnObj.GetExtendedProperty("Flag");
                if (!string.IsNullOrEmpty(flag) && flag== "N")
                { retLst.Add(boxid); }
                else
                { retLst.Add(""); }
               
                retLst.Add(carton.Model);
             
                object obj = currentSession.GetValue(Session.SessionKeys.StandardWeight);
                string w = "";
                if (obj != null)
                { w = obj.ToString(); }
                retLst.Add(w);//Standard Weight
                List<string> prdLst = new List<string>();
               
                List<S_BSam_Product> lstSprd = new List<S_BSam_Product>();
                foreach (IMES.FisObject.FA.Product.IProduct product in prodList)
                {
                    S_BSam_Product sProduct = new S_BSam_Product();
                    sProduct.CustomerSN = product.CUSTSN;
                    sProduct.ProductID = product.ProId;
                    sProduct.Model = product.Model;
                    sProduct.Location = (string)product.GetAttributeValue("CartonLocation") ?? "";
                    lstSprd.Add(sProduct);
                }
                retLst.Add(lstSprd);
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
                throw new SystemException(e.Message);
            }
            finally
            {
                //logger.Debug("(CombineCartonInDN)InputFirstSN, firstSn:" + firstSn );
            }


        }
        private bool CheckNOMLabel(string model)
        {
            IBOMRepository bomRep = RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();
            IHierarchicalBOM curBom = bomRep.GetHierarchicalBOMByModel(model);
            IList<IBOMNode> PLBomNodeList = curBom.GetFirstLevelNodesByNodeType("PL");
            return PLBomNodeList.Any(x => x.Part.PN == "60LANOM00001"); //NOM Label: ModelBOM下带60LANOM00001料
        }
        public ArrayList SaveForTablet(string custSn, IList<PrintItem> printItems)
        {
            logger.Debug("(CartonWeight)SaveForTablet Start,Key:" + custSn);
            ArrayList retLst = new ArrayList();
            try
            {
                FisException ex;
                List<string> erpara = new List<string>();
                string sessionKey = custSn;
                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, Session.SessionType.Product);
                if (currentSession == null)
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK021", erpara);
                    throw ex;
                }

                currentSession.AddValue(Session.SessionKeys.PrintItems, printItems);
                currentSession.Exception = null;
                currentSession.SwitchToWorkFlow();

                if (currentSession.Exception != null)
                {
                    if (currentSession.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        currentSession.ResumeWorkFlow();
                    }
                    throw currentSession.Exception;
                }
                IMES.FisObject.FA.Product.IProduct product = (IMES.FisObject.FA.Product.IProduct)currentSession.GetValue(Session.SessionKeys.Product);

                IList<PrintItem> lst = (IList<PrintItem>)currentSession.GetValue(Session.SessionKeys.PrintItems);
                ArrayList arr = new ArrayList();
                arr.Add(CheckNOMLabel(product.Model));
                arr.Add(lst);
                return arr;

            }

            catch (FisException e)
            {
                logger.Error(e.mErrmsg);
                throw e;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw e;
            }
            finally
            {
                logger.Debug("(CartonWeight)InputFirstSN, Custsn:" + custSn);
            }
        }
        public void Save(string custSn)
        {
            logger.Debug("(CartonWeight)Save Start,Key:" + custSn);
           ArrayList retLst = new ArrayList();
            try
            {
                FisException ex;
                List<string> erpara = new List<string>();
                string sessionKey = custSn;
                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, Session.SessionType.Product);
                if (currentSession == null)
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK021", erpara);
                    throw ex;
                }
         

                currentSession.Exception = null;
                currentSession.SwitchToWorkFlow();

                if (currentSession.Exception != null)
                {
                    if (currentSession.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        currentSession.ResumeWorkFlow();
                    }
                    throw currentSession.Exception;
                }
            
            }

            catch (FisException e)
            {
                logger.Error(e.mErrmsg);
                throw e;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw e;
            }
            finally
            {
                logger.Debug("(CartonWeight)InputFirstSN, Custsn:" + custSn );
            }
        }
        private void CheckQCStatus(string sn)
        {
            IMES.FisObject.FA.Product.IProduct product = CommonImpl.GetProductByInput(sn, CommonImpl.InputTypeEnum.ProductIDOrCustSN);
            string paqcStatus = product.GetAttributeValue("PAQC_QCStatus") ?? "";
            switch (paqcStatus)
            {
                case "":
                    throw new FisException("PAK014", new string[] { });
                case "8":
                    throw new FisException("PAK014", new string[] { });
                case "A":
                    throw new FisException("PAK015", new string[] { });
                default:
                    break;
            }
        }

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="sessionKey"></param>
        public void Cancel(string custSn)
        {
            logger.Debug("(CartonWeight)Cancel start, [custSN]:" + custSn);
            List<string> erpara = new List<string>();
            string sessionKey = custSn;

            try
            {
                Session session = SessionManager.GetInstance.GetSession(sessionKey,Session.SessionType.Product);

                if (session != null)
                {
                    SessionManager.GetInstance.RemoveSession(session);
                }
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg);
                throw e;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw e;
            }
            finally
            {
                logger.Debug("(CartonWeight)Cancel end, [custSN]:" + custSn);
            }
        }

      
    }
}
