/*
* INVENTEC corporation ©2011 all rights reserved. 
* Description:Generate Customer SN
* UI:CI-MES12-SPEC-FA-UI RCTO CT Label Print.docx –2012-09-11
* UC:CI-MES12-SPEC-FA-UC RCTO CT Label Print.docx –2012-09-11        
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2011-11-01   Du.Xuan               Create   
* ITC-1360-1020 增加zzzz上限防护
* Known issues:
* TODO：
* 
*/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Station.Interface.StationIntf;
using IMES.Infrastructure;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Workflow.Runtime;
using IMES.DataModel;
using log4net;
using IMES.Route;
using IMES.FisObject.FA.Product;
using IMES.FisObject.Common.Part;
using IMES.FisObject.Common.PartSn;


namespace IMES.Station.Implementation
{
    /// <summary>
    /// IRCTOCTLabelPrint接口的实现类
    /// </summary>
    public class RCTOCTLabelPrintImpl : MarshalByRefObject, IRCTOCTLabelPrint 
    {
        private const Session.SessionType ProductSessionType = Session.SessionType.Product;
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region IRCTOCTLabelPrint members

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ctno"></param>
        /// <param name="qty"></param>
        /// <param name="printItems"></param>
        /// <param name="line"></param>
        /// <param name="editor"></param>
        /// <param name="stationId"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        public ArrayList inputCTNO( string ctno, int qty, IList<PrintItem> printItems,string line,string editor, string stationId, string customer)
        {
            logger.Debug("(RCTOCTLabelPrintImpl)InputProdId start, [CTNO]:" + ctno
                + " [line]:" + line
                + " [editor]:" + editor
                + " [station]:" + stationId
                + " [customer]:" + customer);

            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = ctno;

            IProductRepository productRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IMES.FisObject.FA.Product.IProduct>();
            IPartRepository partRep = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();


            ProductPart conf = new ProductPart();
            conf.PartSn = ctno;
            IList<ProductPart> productList = productRep.GetProductPartList(conf);
            
            try
            {
                Session session = SessionManager.GetInstance.GetSession(sessionKey, ProductSessionType);

                if (session == null)
                {
                    session = new Session(sessionKey, ProductSessionType, editor, stationId, line, customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    //一个MB_SNo对应一个workflow
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", stationId);
                    wfArguments.Add("CurrentFlowSession", session);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", line);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", ProductSessionType);


                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(stationId, "RCTOCTLabelPrint.xoml", "", out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);
                  
                    session.SetInstance(instance);

                    session.AddValue(Session.SessionKeys.IsComplete, true);
                    session.AddValue(Session.SessionKeys.PrintItems, printItems);


                    if (!SessionManager.GetInstance.AddSession(session))
                    {
                        session.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");
                        erpara.Add(sessionKey);
                        ex = new FisException("CHK020", erpara);
                        throw ex;
                    }

                    session.WorkflowInstance.Start();
                    session.SetHostWaitOne();
                }
                else
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK020", erpara);
                    throw ex;
                }

                //session.SwitchToWorkFlow();

                //check workflow exception
                if (session.Exception != null)
                {
                    if (session.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        session.ResumeWorkFlow();
                    }

                    throw session.Exception;
                }

                ArrayList retList = new ArrayList();
               
                IList<string> ctnoList = (List<string>)session.GetValue("CTNOList");
                IList<PrintItem> printList = this.getPrintList(session);


                retList.Add(ctno);
                retList.Add(qty);
                retList.Add(printList);
                retList.Add(ctnoList);
                
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
                throw e;
            }
            finally
            {
                logger.Debug("(RCTOCTLabelPrintImpl)InputProdId end, [CTNO]:" + ctno
                + " [line]:" + line
                + " [editor]:" + editor
                + " [station]:" + stationId
                + " [customer]:" + customer);
            }
        }

        public ArrayList checkCTNO(string ctno)
        {
            try
            {
                //获取Part.PartNo 和 Part.Descr
                //条件：Part.PartNo = PartInfo.PartNo and PartInfo.InfoValue=Left([CT No],5)
                //限制：Top 1
                //若Part.PartNo不存在，则报错：“错误的CT No”

                IPartRepository partRep = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
                PartInfo cond = new PartInfo();
                cond.PN = ctno.Substring(0, 5);
                IList<PartInfo> partList = partRep .GetPartInfoList(cond);                 
                string partno ="";
                if (partList.Count > 0)
                {
                    partno = partList[0].PN;
                }

                ArrayList retList = new ArrayList();
                retList.Add(partno);
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
                throw e;
            }
        }


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
                //throw new  SysException(e);
            }
        }
   
        #endregion
    }
}
