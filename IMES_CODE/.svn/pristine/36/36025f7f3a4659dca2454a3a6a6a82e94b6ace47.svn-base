/*
* INVENTEC corporation ?2011 all rights reserved. 
* Description:Implementation for Combine AST Page
* UI:CI-MES12-SPEC-FA-UI Combine AST .docx –2011/12/5 
* UC:CI-MES12-SPEC-FA-UC Combine AST .docx –2011/12/5            
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2011-12-2   Jessica Liu           (Reference Ebook SourceCode) Create
* Known issues:
* TODO：
* ITC-1360-1649, Jessica Liu, 2012-4-10
*/


using System;
using System.Collections;
using System.Collections.Generic;
using System.Workflow.Runtime;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.Route;
using log4net;
using IMES.Station.Interface.StationIntf;
using IMES.DataModel;
using System.Data;
using System.Data.SqlClient;
using IMES.Infrastructure.Repository._Schema;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.Part;

namespace IMES.Station.Implementation
{

    public class CombineAST : MarshalByRefObject, ICombineAST
    {
        private const Session.SessionType TheType = Session.SessionType.Product;
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        #region IKPPrint Members


        /// <summary>
        /// 卡站处理
        /// </summary>
        /// <param name="prodid">prodid</param>
        /// <param name="length">AST length</param>
        /// <param name="pdline">product line</param>
        /// <param name="status">status</param>
        /// <param name="stationId">station</param>
        /// <param name="editor">editor</param>
        /// <param name="customerId">customerId</param>
        /// <returns>AST值、errorFlag、imageUrl</returns> 
        //public string BlockCheck(string prodid, int length, string pdline, string status, string stationId, string editor, string customerId)
        public List<string> BlockCheck(string prodid, int length, string pdline, string status, string stationId, string editor, string customerId)
        {
            return BlockCheck(prodid, length, pdline, status, stationId, editor, customerId, false);
        }

        public List<string> BlockCheck(string prodid, int length, string pdline, string status, string stationId, string editor, string customerId, bool isCombineAndGenerateAST)
        {
/*//test，测试流程是否通，需去掉========
return prodid;
//test，测试流程是否通，需去掉========*/

            logger.Debug("(CombineAST)BlockCheck start, inputCustsnOrProdid:" + prodid + " length:" + length + " pdline:" + pdline + " status:" + status + " stationId:" + stationId + " editor:" + editor + " customerId:" + customerId);

            FisException ex;
            List<string> erpara = new List<string>();

            try
            {
                string sessionKey = prodid;
                Session Session = SessionManager.GetInstance.GetSession(sessionKey, TheType);
                if (Session == null)
                {
                    Session = new Session(sessionKey, TheType, editor, stationId, pdline, customerId);
                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", stationId);
                    wfArguments.Add("CurrentFlowSession", Session);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", pdline);
                    wfArguments.Add("Customer", customerId);
                    wfArguments.Add("SessionType", TheType);

                    string wfName, rlName;

                    string wfFileName = "CombineAST";
                    if (isCombineAndGenerateAST)
                        wfFileName = "CombineASTandPart";

                    RouteManagementUtils.GetWorkflow(stationId, wfFileName+".xoml", wfFileName+".rules", out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    Session.AddValue("ProdidOrCustsn", prodid);
                    Session.AddValue("ASTLength", length);
                    Session.AddValue("DESCR", "ATSN1");

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

                string WantFinishFlow_when_CombineAndGenerateAST = Session.GetValue("WantFinishFlow_when_CombineAndGenerateAST") as string;
                if (isCombineAndGenerateAST)
                {
                    if (string.IsNullOrEmpty(WantFinishFlow_when_CombineAndGenerateAST))
                        isCombineAndGenerateAST = false;
                }

                //check workflow exception
                if (Session.Exception != null && !isCombineAndGenerateAST)
                {
                    if (Session.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        Session.ResumeWorkFlow();
                    }
                    throw Session.Exception;
                }

                //ITC-1360-1649, Jessica Liu, 2012-4-10
                string AST = (string)Session.GetValue("AST");
                string errorFlag = (string)Session.GetValue("ErrorFlag");
                string imageUrl = (string)Session.GetValue("ImageURL");

                if (isCombineAndGenerateAST)
                    errorFlag = WantFinishFlow_when_CombineAndGenerateAST;

                List<string> ret = new List<string>();
                ret.Add(AST);
                ret.Add(errorFlag);
                ret.Add(imageUrl);

                return ret;     //AST;
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
                logger.Debug("(CombineAST)BlockCheck end, inputCustsnOrProdid:" + prodid + " length:" + length + " pdline:" + pdline + " status:" + status + " stationId:" + stationId + " editor:" + editor + " customerId:" + customerId);
            }

        }


        /// <summary>
        /// 删除已有标签及Check CDSI绑定等处理
        /// </summary>
        /// <param name="prodid">prodid</param>
        /// <param name="deleteflag">delete flag</param>
        /// <param name="stationId">station</param>
        /// <param name="editor">editor</param>
        /// <param name="customerId">customerId</param>
        /// <param name="printItems">Print Item鍒楄〃</param>
        /// <returns>CustomSN、ProductID、Model值、CDSI标志、Print Item鍒楄〃、AST</returns>
        public IList<PrintItem> DoPrint(string prodid, bool deleteflag, string stationId, string editor, string customerId, IList<PrintItem> printItems, out string prodId, out string custsn, out string model, out string CDSIFlag, out string ast)
        {
            logger.Debug("(CombineAST)DoPrint start, inputCustsnOrProdid:" + prodid + " deleteflag:" + deleteflag + " stationId:" + stationId + " editor:" + editor + " customerId:" + customerId);

            FisException ex;
            List<string> erpara = new List<string>();

            try
            {
                string sessionKey = prodid;
                Session Session = SessionManager.GetInstance.GetSession(sessionKey, TheType);
                Session sessionInfo = SessionManager.GetInstance.GetSession(sessionKey, TheType);
                Product gProduct = (Product)sessionInfo.GetValue(Session.SessionKeys.Product);
                bool isCDSI = gProduct.IsCDSI;

                if (sessionInfo == null)
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK021", erpara);
                    throw ex;
                }
                else
                {
                    sessionInfo.AddValue("DeleteFlag", deleteflag);                    
                    sessionInfo.AddValue("IsCDSI", isCDSI);
                    Session.AddValue(Session.SessionKeys.PrintItems, printItems);

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

                Product getProduct = (Product)Session.GetValue(Session.SessionKeys.Product);
                prodId = getProduct.ProId;
                custsn = getProduct.CUSTSN;
                model = getProduct.Model;
                
                if (isCDSI == true)
                {
                    CDSIFlag = "TRUE";
                }
                else
                {
                    CDSIFlag = "FALSE";
                }

                //2012-5-2
                ast = (string)Session.GetValue("ASTInfo");

                //2012-4-28
                //IList<PrintItem> returnList = this.getPrintList(Session);
                IList<PrintItem> returnList = null;

                return returnList;
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
                logger.Debug("(CombineAST)DoPrint end, inputCustsnOrProdid:" + prodid + " deleteflag:" + deleteflag + " stationId:" + stationId + " editor:" + editor + " customerId:" + customerId);
            }

        }
        /*
        /// <summary>
        /// 删除已有标签及Check CDSI绑定等处理
        /// </summary>
        /// <param name="prodid">prodid</param>
        /// <param name="deleteflag">delete flag</param>
        /// <param name="stationId">station</param>
        /// <param name="editor">editor</param>
        /// <param name="customerId">customerId</param>
        /// <returns>CustomSN、ProductID、Model值</returns>
        public List<string> DoPrint(string prodid, bool deleteflag, string stationId, string editor, string customerId)
        {
            logger.Debug("(CombineAST)DoPrint start, inputCustsnOrProdid:" + prodid + " deleteflag:" + deleteflag + " stationId:" + stationId + " editor:" + editor + " customerId:" + customerId);

            FisException ex;
            List<string> erpara = new List<string>();

            try
            {
                string sessionKey = prodid;
                Session Session = SessionManager.GetInstance.GetSession(sessionKey, TheType);
                Session sessionInfo = SessionManager.GetInstance.GetSession(sessionKey, TheType);
                Product gProduct = (Product)sessionInfo.GetValue(Session.SessionKeys.Product);
                bool isCDSI = gProduct.IsCDSI;

                if (sessionInfo == null)
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK021", erpara);
                    throw ex;
                }
                else
                {
                    sessionInfo.AddValue("DeleteFlag", deleteflag);

                    
                    sessionInfo.AddValue("IsCDSI", isCDSI);

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

                Product getProduct = (Product)Session.GetValue(Session.SessionKeys.Product);
                string prodId = getProduct.ProId;
                string custsn = getProduct.CUSTSN;
                string model = getProduct.Model;
                string CDSIFlag = "TRUE";
                if (isCDSI == false)
                {
                    CDSIFlag = "FALSE";
                }

                List<string> ret = new List<string>();
                ret.Add(prodId);
                ret.Add(custsn);
                ret.Add(model);
                ret.Add(CDSIFlag);

                return ret;
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
                logger.Debug("(CombineAST)DoPrint end, inputCustsnOrProdid:" + prodid + " deleteflag:" + deleteflag + " stationId:" + stationId + " editor:" + editor + " customerId:" + customerId);
            }
            
        }
        */

        /// <summary>
        /// 对AST进行保存处理
        /// </summary>
        /// <param name="prodid">prodid</param>
        /// <param name="ast">AST</param>
        /// <param name="stationId">station</param>
        /// <param name="editor">editor</param>
        /// <param name="customerId">customerId</param>
        public ArrayList DoSave(string prodid, string ast, string stationId, string editor, string customerId)
        {
/*//test，测试流程是否通，需去掉========
return;
//test，测试流程是否通，需去掉========*/

            logger.Debug("(CombineAST)DoSave start, inputCustsnOrProdid:" + prodid + " ast:" + ast + " stationId:" + stationId + " editor:" + editor + " customerId:" + customerId);

            FisException ex;
            List<string> erpara = new List<string>();
            ArrayList ret = new ArrayList();

            try
            {
                string sessionKey = prodid;
                //Session Session = SessionManager.GetInstance.GetSession(sessionKey, TheType);
                Session sessionInfo = SessionManager.GetInstance.GetSession(sessionKey, TheType);

                if (sessionInfo == null)
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK021", erpara);
                    throw ex;
                }
                else
                {
                    // mantis 1548
                    List<string> errpara = ChkAst(ast);
                    if (errpara != null)
                    {
                        Cancel(sessionKey);
                        throw new FisException("CHK967", errpara);  //此资产标签已结合+'ProductID:'+@ProductID+’CustSN：‘+@CustSN
                    }

                    sessionInfo.AddValue("AST", ast);

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

                string promptATSN9 = "@ATSN9";
                string partSnATSN9 = "";
                IPart needPartATSN9 = sessionInfo.GetValue("NeedPartTypeATSN9") as IPart;
                if (null != needPartATSN9)
                {
                    partSnATSN9 = needPartATSN9.PN;

                    foreach (PartInfo pi in needPartATSN9.Attributes)
                    {
                        if ("Descr".Equals(pi.InfoType))
                        {
                            promptATSN9 = pi.InfoValue;
                            break;
                        }
                    }
                }
                ret.Add(partSnATSN9);
                ret.Add(promptATSN9);

                return ret;
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
                logger.Debug("(CombineAST)DoSave end, inputCustsnOrProdid:" + prodid + " ast:" + ast + " stationId:" + stationId + " editor:" + editor + " customerId:" + customerId);
            }
        }

        public void DoSaveAfterCheckATSN9(string prodid, string stationId, string editor, string customerId)
        {
            /*//test，测试流程是否通，需去掉========
            return;
            //test，测试流程是否通，需去掉========*/

            logger.Debug("(CombineAST)DoSaveAfterCheckATSN9 start, inputCustsnOrProdid:" + prodid + " stationId:" + stationId + " editor:" + editor + " customerId:" + customerId);

            FisException ex;
            List<string> erpara = new List<string>();
            ArrayList ret = new ArrayList();

            try
            {
                string sessionKey = prodid;
                Session sessionInfo = SessionManager.GetInstance.GetSession(sessionKey, TheType);

                if (sessionInfo == null)
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK021", erpara);
                    throw ex;
                }
                else
                {
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
                logger.Debug("(CombineAST)DoSaveAfterCheckATSN9 end, inputCustsnOrProdid:" + prodid + " stationId:" + stationId + " editor:" + editor + " customerId:" + customerId);
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

        // mantis 1548
        private List<string> ChkAst(string ast)
        {
            if (!string.IsNullOrEmpty(ast))
            {
                string strSQL = @"select p.ProductID, p.CUSTSN
from Product p inner join Product_Part pp on p.ProductID=pp.ProductID
where pp.PartSn=@PartSn and pp.PartType like 'ATSN%'";
                SqlParameter paraName = new SqlParameter("@PartSn", SqlDbType.VarChar, 20);
                paraName.Direction = ParameterDirection.Input;
                paraName.Value = ast;
                DataTable tb = SqlHelper.ExecuteDataFill(SqlHelper.ConnectionString_FA, System.Data.CommandType.Text,
                    strSQL, paraName);
                if (tb != null && tb.Rows.Count > 0)
                {
                    string ProductID = tb.Rows[0][0].ToString();
                    string CUSTSN = tb.Rows[0][1].ToString();
                    List<string> errpara = new List<string>();
                    errpara.Add(ProductID);
                    errpara.Add(CUSTSN);
                    return errpara;
                }
            }
            return null;
        }

        #endregion
    }
}
