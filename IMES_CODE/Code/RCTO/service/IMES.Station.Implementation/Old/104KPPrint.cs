/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: KP Print Impl
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2010-04-22   LuycLiu     Create 

 * 
 * Known issues:Any restrictions about this file 
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Workflow.Runtime;
using IMES.DataModel;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.Route;
using IMES.Station.Interface.StationIntf;
using log4net;

namespace IMES.Station.Implementation
{

    public class KPPrint : MarshalByRefObject, IKPPrint
    {
        private const Session.SessionType TheType = Session.SessionType.Product;
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        #region IKPPrint Members

        /// <summary>
        /// 打印KP标签
        /// </summary>
        /// <param name="inputSn">sn</param>
        /// <param name="pdLine">product line</param>
        /// <param name="stationId">product station</param>
        /// <param name="editor">editor</param>
        /// <param name="customerId">customer sn</param>
        /// <param name="outputSn">输出sn</param>
        /// <param name="printItems">Print Item列表</param>
        /// <returns>Print Item列表</returns>
        public IList<PrintItem> Print(string inputSn, string pdLine, string stationId, string editor, string customerId, out string outputSn, IList<PrintItem> printItems)
        {
            logger.Debug("(KPPrint)Print start, inputSn:" + inputSn + " pdLine:" + pdLine + " stationId:" + stationId + " editor:" + editor + " customerId:" + customerId);

            FisException ex;
            List<string> erpara = new List<string>();
            string temp = string.Empty;
            try
            {
                var productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IMES.FisObject.FA.Product.IProduct>();
                IMES.FisObject.FA.Product.IProduct currentProduct = null;

                //输入为CustSn或ProductId
                if (inputSn.Length == 9)
                {
                    temp = inputSn;
                    if (inputSn.Substring(inputSn.Length - 1, 1) == "Q")
                    {
                        //如果输入项为9位且最后一个字符为Q,则根据CustSN条件查找
                        currentProduct = productRepository.GetProductByCustomSn(inputSn);
                    }
                    else
                    {
                        //否则根据ProductId条件查找
                        currentProduct = productRepository.Find(inputSn);
                    }
                }
                else if (inputSn.Length == 10)
                {

                    if (inputSn.Substring(0, 1) == "S")
                    {
                        //如果输入项为10位，且第一位为S,则把后面的9位作为CustSn，进行查找
                        temp = inputSn.Substring(1, 9);
                        currentProduct = productRepository.GetProductByCustomSn(temp);
                    }
                    else
                    {
                        //将前9位作为ProductId，进行查找
                        temp = inputSn.Substring(0, 9);
                        currentProduct = productRepository.Find(temp);
                    }
                }
                if (currentProduct == null)
                {
                    FisException fe = new FisException("CHK079", new string[] { inputSn });
                    throw fe;
                }

                //如果没抛异常，证明输入合法，将输入的CustSn/ProductId作为sessionKey
                string sessionKey = inputSn;
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
                    RouteManagementUtils.GetWorkflow(stationId, "104KPPrint.xoml", null, out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    Session.SetInstance(instance);

                    if (!SessionManager.GetInstance.AddSession(Session))
                    {
                        Session.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");
                        erpara.Add(sessionKey);
                        ex = new FisException("CHK020", erpara);
                        throw ex;
                    }

                    
                    Session.AddValue(Session.SessionKeys.PrintItems, printItems);
                    Session.AddValue(Session.SessionKeys.PrintLogBegNo, inputSn);
                    Session.AddValue(Session.SessionKeys.PrintLogEndNo, inputSn);
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

                IList<PrintItem> returnList = this.getPrintList(Session);
                outputSn = temp;
                return returnList;
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
                logger.Debug("(KPPrint)Print end, inputSn:" + inputSn + " pdLine:" + pdLine + " stationId:" + stationId + " editor:" + editor + " customerId:" + customerId);
            }
        }

        /// <summary>
        /// 从Sessin里获取打印列表
        /// </summary>
        /// <param name="session">session</param>
        /// <returns></returns>
        private IList<PrintItem> getPrintList(Session session)
        {

            try
            {
                object printObject = session.GetValue(Session.SessionKeys.PrintItems);
                session.RemoveValue(Session.SessionKeys.PrintItems);
                if (printObject == null)
                {
                    return null;
                }

                IList<PrintItem> printItems = (IList<PrintItem>)printObject;

                return printItems;
            }
            catch (FisException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw e;

            }
        }
        #endregion
    }
}
