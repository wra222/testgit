/*
 * INVENTEC corporation ?2011 all rights reserved. 
 * Description:Implementation for Image Download Page
 * UI:CI-MES12-SPEC-FA-UI Image Download.docx –2011/10/28 
 * UC:CI-MES12-SPEC-FA-UC Image Download.docx –2011/10/28            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
* 2011-11-4   Jessica Liu           (Reference Ebook SourceCode) Create
* Known issues:
* TODO：
* ITC-1360-0020
*/

using System;
using System.Collections.Generic;
using System.Workflow.Runtime;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.Route;
using IMES.Station.Interface.StationIntf;
using log4net;

namespace IMES.Station.Implementation
{

    public class ImageDownload : MarshalByRefObject, IImageDownload
    {
        private const Session.SessionType TheType = Session.SessionType.Product;
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        #region IKPPrint Members


		/// <summary>
        /// 对输入的CPQSNO进行检查
        /// </summary>
        /// <param name="inputSn">cpqsno</param>
        /// <param name="pdLine">Pd Line</param>
        /// <param name="editor">operator</param>
        /// <param name="stationId">stationId</param>
        /// <param name="customerId">customerId</param>
        /// <returns>Model and ProdID</returns>
        public IList<String> checkCPQSNO(string cpqsno, string pdLine, string editor, string stationId, string customerId)
		{
            logger.Debug("(ImageDownload)checkCPQSNO start, inputCustsn:" + cpqsno + " pdLine:" + pdLine + " editor:" + editor + " customerId:" + customerId);

            FisException ex;
            List<string> erpara = new List<string>();

            try
            {
                string sessionKey = cpqsno;
                Session Session = SessionManager.GetInstance.GetSession(sessionKey, TheType);
                if (Session == null)
                {
                    Session = new Session(sessionKey, TheType, editor, stationId, pdLine, customerId);
                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", stationId);
                    wfArguments.Add("CurrentFlowSession", Session);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", pdLine);
                    wfArguments.Add("Customer", customerId);
                    wfArguments.Add("SessionType", TheType);

                    string wfName, rlName;

                    RouteManagementUtils.GetWorkflow(stationId, "ImageDownload.xoml", null, out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    Session.AddValue(Session.SessionKeys.CustSN, cpqsno);   //"CPQSNO", cpqsno);

                    Session.SetInstance(instance);

                    if (!SessionManager.GetInstance.AddSession(Session))
                    {
                        Session.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");
                        erpara.Add(sessionKey);
                        ex = new FisException("CHK020", erpara);	//？？？
                        throw ex;
                    }
                    
                    Session.WorkflowInstance.Start();
                    Session.SetHostWaitOne();
                }
                else
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK020", erpara);	//？？？
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

                Product getProduct = (Product)Session.GetValue(Session.SessionKeys.Product);
                string getModel = getProduct.Model;     //？？？
                string getProdID = getProduct.ProId;

                List<String> retList = new List<String>();
                retList.Add(getModel);
                retList.Add(getProdID);

                return retList;
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
                logger.Debug("(ImageDownload)checkCPQSNO end, inputCustsn:" + cpqsno + " pdLine:" + pdLine + " editor:" + editor + " customerId:" + customerId);
            }
            
        }



        /// <summary>
        /// SN Check第二次输入SN处理，返回PAQC以及ALC/NO-ALC
        /// </summary>
        /// <param name="cpqsno">cpqsno</param>
        /// <param name="bios">bios</param>
        /// <param name="image">image</param>
        /// <param name="pdLine">Pd Line</param>
        /// <param name="editor">operator</param>
        /// <param name="stationId">stationId</param>
        /// <param name="customerId">customerId</param>
        public void DoSave(string cpqsno, string bios, string image, string pdLine, string editor, string stationId, string customerId)
        {
            logger.Debug("(ImageDownload)DoSave start, cpqsno:" + cpqsno + " bios:" + bios + " image:" + image + " editor:" + editor + " customerId:" + customerId);

            FisException ex;
            List<string> erpara = new List<string>();

            try
            {
                string sessionKey = cpqsno;
                Session Session = SessionManager.GetInstance.GetSession(sessionKey, TheType);
                Session sessionInfo = SessionManager.GetInstance.GetSession(sessionKey, TheType);

                if (sessionInfo == null)
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK021", erpara);
                    throw ex;
                }
                else
                {
                    Session.AddValue("CPQSNO", cpqsno);
                    Session.AddValue("BIOS", bios);
                    Session.AddValue("Image", image);
                    Session.AddValue("Flag", "PASS");
                    sessionInfo.Exception = null;
                    sessionInfo.SwitchToWorkFlow();		
                }

                if (sessionInfo.Exception != null)
                {
                    if (sessionInfo.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        sessionInfo.ResumeWorkFlow();
                    }

                    throw sessionInfo.Exception;
                }

                return;
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
                logger.Debug("(ImageDownload)DoSave end, cpqsno:" + cpqsno + " bios:" + bios + " image:" + image + " editor:" + editor + " customerId:" + customerId);
            }    
            
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

         #endregion
    }
}
