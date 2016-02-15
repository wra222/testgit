/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: PCARepairImpl
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2009-11-03   207006     Create 
 * 
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
using IMES.Station.Interface.CommonIntf;
using IMES.FisObject.Common.Model;
using IMES.FisObject.Common.MO;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.DataModel;
using log4net;
using IMES.Infrastructure.Extend;
using IMES.FisObject.Common.Line;

using System.Collections;

using IMES.Route;


namespace IMES.Station.Implementation
{
    public class _030IMAGEOutput : MarshalByRefObject, IIMAGEOutput
    {
        private static readonly Session.SessionType theType = Session.SessionType.Product;
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly ILog log = LogManager.GetLogger("fisLog");

        #region IIMAGEOutput Members
        public void InputDownloadImageResult(string prodId, string resultCode)
        {
            logger.Debug("(_ImageOutput)InputDownloadImageResult start, prodId:" + prodId);
           // string result = "";
            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = prodId;
            Session Session = SessionManager.GetInstance.GetSession(sessionKey, theType);
            if (Session == null)
            {
                erpara.Add(sessionKey);
                ex = new FisException("CHK021", erpara);
                throw ex;

            }
        }
        public ArrayList GetProdidLineVersion(string custsn, string editor, string stationId, string customerId)
        {
            string prodId = "";
            string version = "";
            try
            {
                    CommonImpl cmm = new CommonImpl();
                    IMES.FisObject.FA.Product.IProduct iProduct = CommonImpl.GetProductByInput(custsn, CommonImpl.InputTypeEnum.CustSN);
             
                    List<string> erpara = new List<string>();
                    prodId = iProduct.ProId;
                    // Add by Benson --For Get Pass code & Fail Code //IMPasscode/IMFailcode
                    string model = iProduct.Model;
                    IModelRepository myRepository = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();
                    Model ModelObj = myRepository.Find(model);
                    string passcode = ModelObj.GetAttribute("IMPasscode");
                    string failcode = ModelObj.GetAttribute("IMFailcode");
                    if (string.IsNullOrEmpty(passcode) || string.IsNullOrEmpty(failcode))
                    {
                        erpara.Add(custsn);
                        throw new FisException("CHK179", erpara);
                    }

                    if (string.IsNullOrEmpty(prodId))
                    {
                        erpara.Add(custsn);
                        throw new FisException("SFC011", erpara);
                    }
                    IMES.DataModel.ProductStatusInfo prsinfo;
                    try
                    {
                        prsinfo = cmm.GetProductStatusInfo(prodId);
                    }
                    catch
                    {
                        logger.Error("No Product Status Data");
                        throw new Exception("No Product Status Data");

                    }
                    string pdLine = prsinfo.pdLine;
                    if (string.IsNullOrEmpty(pdLine))
                    {
                        erpara.Add(custsn);
                        throw new FisException("SFC011", erpara);
                    }
                    ILineRepository iline = RepositoryFactory.GetInstance().GetRepository<ILineRepository, Line>();
                    Line line = iline.Find(pdLine);
                    version = GetVersion(iProduct);
                    if (version == "")
                    {
                        erpara.Add(iProduct.Model);
                        throw new FisException("CHK167", erpara);
                    }

                    ArrayList retArray = new ArrayList();
                    retArray.Add(prodId);
                    retArray.Add(pdLine);
                    retArray.Add(line.Descr);
                    retArray.Add(version);
                    retArray.Add(passcode);
                    retArray.Add(failcode);
                     return retArray;
                // reList[0] : prodid    reList[1] : pdLine   reList[2] : line.Descr    reList[3] : version 
                   
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg, e);
                throw e;
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                throw new SystemException(e.Message);
            }
            finally
            {
                logger.Debug("(_BoardInput)Save end, prodId:" + prodId);
            }
       
        }

            //  ArrayList retrunValue = new ArrayList();
        public void SaveDownloadImgResult(string prodId, bool IsSuccessDownload, IList<string> defectList)
        {
            logger.Debug("(_ImageOutput)SaveDownloadImgResult start, prodId:" + prodId);
            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = prodId;
            Session Session = SessionManager.GetInstance.GetSession(sessionKey, theType);
            if (Session == null)
            {
                erpara.Add(sessionKey);
                ex = new FisException("CHK021", erpara);
                throw ex;

            }
            try 
            {
                Product iProduct = (Product)Session.GetValue(Session.SessionKeys.Product);
                if (!(defectList == null || defectList.Count == 0))
                {
                    Session.AddValue(Session.SessionKeys.DefectList, defectList);
                }
                Session.AddValue(IMES.Infrastructure.Extend.ExtendSession.SessionKeys.IsSuccessDownloadImage, IsSuccessDownload);
                Session.AddValue(IMES.Infrastructure.Extend.ExtendSession.SessionKeys.IsCorrectVersion, "1");
                Session.Exception = null;
                Session.SwitchToWorkFlow();
                
                if (Session.Exception != null)
                {
                    if (Session.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        Session.ResumeWorkFlow();
                    }

                    throw Session.Exception;
                }
            }
            catch (FisException e)
            {
                logger.Error(e.Message, e);
                throw e;
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                throw new SystemException(e.Message);
            }
            finally
            {
                logger.Debug("(_BoardInput)SaveDownloadImgResult end, prodId:" + prodId);
            }
        
        }




        public string Save(string prodId, string version)
        {

            logger.Debug("(_ImageOutput)Save start, prodId:" + prodId);
            string result = "";
            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = prodId;
            Session Session = SessionManager.GetInstance.GetSession(sessionKey, theType);
            if (Session == null)
            {
                erpara.Add(sessionKey);
                ex = new FisException("CHK021", erpara);
                throw ex;

            }
            try
            {
                Product iProduct = (Product)Session.GetValue(Session.SessionKeys.Product);
          
                string modelId = iProduct.Model;
                IModelRepository myRepository = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();
                string ModelVersion = null;
                Model _Model = myRepository.Find(modelId);
                if (_Model == null)
                {
                    result = "2";
                }
                else
                {
                    ModelVersion = _Model.GetAttribute("Version");
                    if (ModelVersion == null)
                    {
                        result = "2"; 
                    }

                    else
                    { 
                        if (ModelVersion.ToUpper().Trim() != version.ToUpper().Trim())
                        {
                            result = "0";//Wrong Version
                           List<string> lst = new List<string>();
                           lst.Add("4118");
                           Session.AddValue(Session.SessionKeys.DefectList, lst);
                        }
                        else
                        {
                            result = "1"; //Correct Version 
                        }
                
                    }

                } //   if (_Model == null)
           
                Session.AddValue(IMES.Infrastructure.Extend.ExtendSession.SessionKeys.IsCorrectVersion, result);
                Session.Exception = null;
                Session.SwitchToWorkFlow();
                if (Session.Exception != null)
                {
                    if (Session.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        Session.ResumeWorkFlow();
                    }

                    throw Session.Exception;
                }
                  return result;
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg, e);
                throw e;
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                throw new SystemException(e.Message);
            }
            finally
            {
                logger.Debug("(_BoardInput)Save end, prodId:" + prodId);
            }
        
        
        
        
        
        
        }

        private string GetVersion(IMES.FisObject.FA.Product.IProduct product)
        {
            string version = "";
            string modelId = product.Model;
            IModelRepository myRepository = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();
            Model _Model = myRepository.Find(modelId);
            if (_Model!= null & !string.IsNullOrEmpty(_Model.GetAttribute("Version")))
            {
                version = _Model.GetAttribute("Version");
            }
            return version;
        }
        public ArrayList InputCustsn(string custsn, string editor, string stationId, string customerId)
        {
            if (editor.Trim() == "")
                log.Error("Editor from bll is empty!");

            if (customerId.Trim() == "")
                log.Error("Customer from bll is empty!");

            //   ArrayList GetProdidLineVersion(string custsn, string editor, string stationId, string customerId)
               // ***************************************** Get Product ID & pdLine & Version *****************************************
                ArrayList retArray = new ArrayList();
                retArray = GetProdidLineVersion(custsn, editor, stationId, customerId); //  reList[0] : prodid    reList[1] : pdLine   reList[2] : line.Descr    reList[3] : version  
                string prodId = retArray[0].ToString();
                string pdLine = retArray[1].ToString();
             
            // ***************************************** Get Product ID & pdLine & Version *****************************************
               logger.Debug("(_030IMAGEOutput)Input CUSTSN start, pdLine:" + pdLine + " prodId:" + prodId + " editor:" + editor + " stationId:" + stationId + " customerId:" + customerId);
               FisException ex;
               List<string> erpara = new List<string>(); 
            try
            {
                string sessionKey = prodId; 
                Session Session = SessionManager.GetInstance.GetSession(sessionKey, theType);
                if (Session == null)
                {
                    Session = new Session(sessionKey, theType, editor, stationId, pdLine, customerId);
                    Session.AddValue(Session.SessionKeys.CustSN, custsn);
                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", stationId);
                    wfArguments.Add("CurrentFlowSession", Session);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", pdLine);
                    wfArguments.Add("Customer", customerId);
                    wfArguments.Add("SessionType", theType);
                    Session.AddValue(Session.SessionKeys.IsComplete, false);
                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(stationId, "030IMAGEOutput.xoml", "030IMAGEOutput.rules", out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);
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

          
                if (Session.Exception != null)
                {
                    if (Session.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        Session.ResumeWorkFlow();
                    }
                    throw Session.Exception;
                }
          
                return retArray;
               
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg, e);
                throw e;

            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                throw new SystemException(e.Message);
            }
            finally
            {
                logger.Debug("(_030IMAGEOutput)InputProdId end, pdLine:" + pdLine + " prodId:" + prodId + " editor:" + editor + " stationId:" + stationId + " customerId:" + customerId);

            }
  
        }
       

        public void InputProdId(string pdLine, string prodId, string editor, string stationId, string customerId)
        {
            logger.Debug("(_030IMAGEOutput)InputProdId start, pdLine:" + pdLine + " prodId:" + prodId + " editor:" + editor + " stationId:" + stationId + " customerId:" + customerId);

            FisException ex;
            List<string> erpara = new List<string>();
           
            try
            {
                string sessionKey = prodId;
                Session Session = SessionManager.GetInstance.GetSession(sessionKey, theType);

                if (Session == null)
                {
                    Session = new Session(sessionKey, theType, editor, stationId, pdLine, customerId);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();

                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", stationId);
                    wfArguments.Add("CurrentFlowSession", Session);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", pdLine);
                    wfArguments.Add("Customer", customerId);
                    wfArguments.Add("SessionType", theType);
                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(stationId, "030IMAGEOutput.xoml", null, out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    //Session.AddValue(Session.SessionKeys.IsNextMonth, month);
                    //Session.AddValue(Session.SessionKeys.Qty, qty);
                    //Session.AddValue(Session.SessionKeys.ECR, ecr);

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

                //IList<PrintItem> returnList = this.getPrintList(Session);
                //return returnList;
            }
            catch (FisException e)
            {
                logger.Error(e.Message, e);
                throw e;
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                throw new SystemException(e.Message);
            }
            finally
            {
                logger.Debug("(_030IMAGEOutput)InputProdId end, pdLine:" + pdLine + " prodId:" + prodId + " editor:" + editor + " stationId:" + stationId + " customerId:" + customerId);

            }
        }

        public IList<ModelPassQty> Query(string pdLine, string editor, string stationId, string customerId)
        {
            DateTime currentTime = DateTime.Now;
            string currentDayStr = currentTime.ToString("yyyy-MM-dd");
            string lastDayStr = currentTime.AddDays(-1).ToString("yyyy-MM-dd");

            DateTime startTime;
            DateTime endTime;


            if (currentTime.Hour > 7 || (currentTime.Hour == 7 && currentTime.Minute > 50))
            {
                startTime = DateTime.Parse(currentDayStr + " 07:50");
                endTime = DateTime.Parse(currentTime.AddDays(1).ToString("yyyy-MM-dd"));
            }
            else
            {
                startTime = DateTime.Parse(lastDayStr + " 07:50");
                endTime = DateTime.Parse(currentDayStr + " 07:50");
            }


            var currentProductRepository = IMES.Infrastructure.FisObjectRepositoryFramework.RepositoryFactory.GetInstance().GetRepository<IMES.FisObject.FA.Product.IProductRepository, IMES.FisObject.FA.Product.IProduct>();
            return currentProductRepository.GetModelPassQty(pdLine, stationId, startTime, endTime);
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

                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, theType);

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

        #endregion
    }
}
