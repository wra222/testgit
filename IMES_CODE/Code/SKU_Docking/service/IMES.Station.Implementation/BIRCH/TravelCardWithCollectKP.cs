/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: PCARepairImpl
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2009-11-03   207006     Create 
 * 2010-02-03   207006     ITC-1122-0079
 * 
 * Known issues:Any restrictions about this file 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Workflow.Runtime;
using IMES.Station.Interface.StationIntf;
using IMES.Infrastructure;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.DataModel;
using IMES.FisObject.Common.CheckItem;
using log4net;
using System.Collections;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure.Repository._Schema;
using System.Data.SqlClient;
using System.Data;
using IMES.FisObject.Common.MO;
using IMES.Station.Interface.CommonIntf;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Route;
using IMES.FisObject.Common.Model;
using IMES.Infrastructure.Repository.Common;
using IMES.FisObject.Common.Part;
namespace IMES.Station.Implementation
{
    public class TravelCardWithCollectKP : MarshalByRefObject, ITravelCardWithCollectKP
    {


        private const Session.SessionType TheType = Session.SessionType.Product;
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //    private string OriSessionkey;

        #region TravelCardAndInputCT Members
        public ArrayList InputCT(string ctNo, string pdLine, string editor, string stationId, string customerId, string model, IList<PrintItem> printItems)
        {
            logger.Debug("(TravelCardAndInputCT)InputProdId start, pdLine:" + pdLine + " input:" + ctNo + " editor:" + editor + " stationId:" + stationId + " customerId:" + customerId);
         
            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = ctNo;
       
            try
            {
                Session Session = SessionManager.GetInstance.GetSession(sessionKey, TheType);

                if (Session == null)
                {
                       
                   Session = new Session(sessionKey, TheType, editor, stationId, pdLine, customerId);
                   Session.AddValue(Session.SessionKeys.PrintItems, printItems);
                   Session.AddValue(Session.SessionKeys.ModelName, model);
                   Session.AddValue(Session.SessionKeys.ValueToCheck, ctNo); //         Session.AddValue(Session.SessionKeys.ValueToCheck, ctNo);
                   Session.AddValue("IsNewPart", true);
                   IProductRepository productRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IMES.FisObject.FA.Product.IProduct>();
                   IList<IMES.FisObject.Common.Part.IProductPart>  lstPart= 
                       productRep.GetProductPartsByValue(ctNo).Where(x=>x.PartType=="Tablet LCM").ToList();

                   if (lstPart.Count > 0)
                   {
                       Session.AddValue("IsNewPart", false);
                       Session.AddValue(Session.SessionKeys.Reason, "");
                       IMES.FisObject.FA.Product.IProduct product = productRep.Find(lstPart[0].ProductID);
                       Session.AddValue(Session.SessionKeys.Product, product);
                       Session.AddValue(Session.SessionKeys.PrintLogBegNo, ctNo);
                       Session.AddValue(Session.SessionKeys.PrintLogEndNo, ctNo);
                       Session.AddValue(Session.SessionKeys.PrintLogName, printItems[0].LabelType);
                       Session.AddValue(Session.SessionKeys.PrintLogDescr, ctNo);
                       Session.AddValue(Session.SessionKeys.Reason, "");
                   }
         
                   //Session.AddValue(Session.SessionKeys.PrintLogBegNo, ctNo);
                   //Session.AddValue(Session.SessionKeys.PrintLogEndNo, ctNo);
                   //Session.AddValue(Session.SessionKeys.PrintLogName, printItems[0].LabelType);
                   //Session.AddValue(Session.SessionKeys.PrintLogDescr, "");
                   //Session.AddValue(Session.SessionKeys.Reason, "");
                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                  //  Session.SessionKeys.Qty
                    Session.AddValue(Session.SessionKeys.Qty, 1);
                    Session.AddValue(Session.SessionKeys.IsNextMonth, false);
                    IPartRepository partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
                    IList<string> valueList = partRepository.GetValueFromSysSettingByName("Site");
                    if (valueList.Count == 0)
                    { throw new FisException("PAK095", new string[] { "Site" }); }
                    if (valueList[0] == "ICC")
                    { Session.AddValue("CityType", "cq"); }
                    else
                    { Session.AddValue("CityType", "sh"); }
     

                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", stationId);
                    wfArguments.Add("CurrentFlowSession", Session);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", pdLine);
                    wfArguments.Add("Customer", customerId);
                    wfArguments.Add("SessionType", TheType);
                
                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(stationId, "TravelCardWithCollectKP.xoml", "TravelCardWithCollectKP.rules", out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);
             
                    Session.AddValue(Session.SessionKeys.IsComplete, false);
                    Session.SetInstance(instance);
                    if (!SessionManager.GetInstance.AddSession(Session))
                    {
                        Session.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");

                        erpara.Add(ctNo);
                        erpara.Add(sessionKey);
                        ex = new FisException("CHK020", erpara);
                        throw ex;
                    }
                    Session.WorkflowInstance.Start();
                    Session.SetHostWaitOne();
                              
                }
                else
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK020", erpara);
                    throw ex;
                }


                //check workflow exception
                if (Session.Exception != null)
                {
                    if (Session.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        Session.ResumeWorkFlow();
                    }
                    throw Session.Exception;
                }
                ArrayList arr = new ArrayList();

                IMES.FisObject.FA.Product.IProduct prd =(IMES.FisObject.FA.Product.IProduct) Session.GetValue(Session.SessionKeys.Product);
                IList<PrintItem> pr = (IList<PrintItem>)Session.GetValue(Session.SessionKeys.PrintItems);
                arr.Add(prd.CUSTSN);
                arr.Add(prd.ProId);
                arr.Add(pr);
                return arr;
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
                logger.Debug("(TravelCardAndInputCT)InputProdId end, pdLine:" + pdLine + " input:" + ctNo + " editor:" + editor + " stationId:" + stationId + " customerId:" + customerId);

            }
        }

        public IList<PrintItem> PrintCustsnLabel(IList<PrintItem> printItems, string prodId)
        {
            string sessionKey = prodId;
            Session Session = SessionManager.GetInstance.GetSession(sessionKey, TheType);
            Session.AddValue(Session.SessionKeys.PrintItems, printItems);
            if (Session == null)
            {

                throw new Exception("Print Error");
            }
            try
            {
                Session.AddValue(Session.SessionKeys.IsComplete, true);
                Session.SwitchToWorkFlow();
            
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
                logger.Debug("(_BoardInput)Print Custsn Label, prodId:" + prodId);
            }

            return (IList<PrintItem>)Session.GetValue(Session.SessionKeys.PrintItems);
        }
        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="sessionKey"></param>
        public void Cancel(string sessionKey)
        {
            try
            {
                logger.Debug("Cancel start, sessionKey:" + sessionKey);

                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, TheType);

                if (currentSession != null)
                {
                    SessionManager.GetInstance.RemoveSession(currentSession);
                }
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
                logger.Debug("Cancel end, sessionKey:" + sessionKey);
            }
        }


        public ArrayList RePrint(string sn, string reason, string line, string editor, string station, string customer, IList<PrintItem> printItems)
        {
            try
            {
                logger.Debug("(TravelCardAndInputCT)Reprint start, ProdId:" + sn + " line:" + line + " editor:" + editor + " customerId:" + customer);
                List<string> erpara = new List<string>();
                FisException ex;
                var productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IMES.FisObject.FA.Product.IProduct>();
                IMES.FisObject.FA.Product.IProduct currentProduct = productRepository.GetProductByIdOrSn(sn);

                if (currentProduct == null)
                {
                    List<string> errpara = new List<string>();
                    errpara.Add(sn);
                    throw new FisException("SFC002", errpara);
                }

                string sessionKey = currentProduct.ProId;
                Session Session = SessionManager.GetInstance.GetSession(sessionKey, Session.SessionType.Product);
                if (Session == null)
                {
                    Session = new Session(sessionKey, Session.SessionType.Product, editor, station, line, customer);
                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", Session);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", line);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", Session.SessionType.Product);
                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(station, "RePrintEx.xoml", string.Empty, out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    Session.AddValue(Session.SessionKeys.PrintItems, printItems);

                    //Get Product data

                    Session.AddValue(Session.SessionKeys.ProductIDOrCustSN, sessionKey);
                    Session.AddValue(Session.SessionKeys.Reason, reason);

                    Session.AddValue(Session.SessionKeys.PrintLogName, printItems[0].LabelType);

                    Session.AddValue(Session.SessionKeys.PrintLogBegNo, sessionKey);
                    Session.AddValue(Session.SessionKeys.PrintLogEndNo, sessionKey);
                    Session.AddValue(Session.SessionKeys.PrintLogDescr, sessionKey);



                    Session.SetInstance(instance);

                    if (!SessionManager.GetInstance.AddSession(Session))
                    {
                        Session.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");
                        erpara.Add(sessionKey);
                        ex = new FisException("CHK020", erpara);
                        throw ex;
                    }
                    Session.WorkflowInstance.Start();
                    Session.SetHostWaitOne();
                }
                else
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK020", erpara);
                    throw ex;
                }

                //check workflow exception
                if (Session.Exception != null)
                {
                    if (Session.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        Session.ResumeWorkFlow();
                    }
                    throw Session.Exception;
                }


                IList<PrintItem> returnList = (IList<PrintItem>)Session.GetValue(Session.SessionKeys.PrintItems);
                ArrayList arr = new ArrayList();
                arr.Add(returnList);
                return arr;
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
                logger.Debug("(CombineKeyParts)Reprint end, ProdId:" + sn + " line:" + line + " editor:" + editor + " customerId:" + customer);
            }
        }

      
        #endregion
    }
}
