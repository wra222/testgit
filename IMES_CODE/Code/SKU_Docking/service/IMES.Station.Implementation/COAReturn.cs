/*
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:COA Return          
 * CI-MES12-SPEC-PAK-COA Return.docx –2012/1/10  
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2012-1-10   207003                Create  
 * Known issues:
*/

using System;
using System.Collections;
using System.Collections.Generic;
using IMES.DataModel;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Station.Interface.StationIntf;
using log4net;
using IMES.Infrastructure.WorkflowRuntime;
using System.Workflow.Runtime;
using IMES.Route;
using IMES.FisObject.FA.Product;
using IMES.FisObject.Common.Part;
using IMES.FisObject.PAK.Pizza;
using IMES.FisObject.PAK.COA;
using IMES.Infrastructure.UnitOfWork;
namespace IMES.Station.Implementation
{
    /// <summary>
    /// IMES service for COAReturn.
    /// </summary>
    public class _COAReturn : MarshalByRefObject, ICOAReturn
    { 
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const Session.SessionType SessionType = Session.SessionType.Common;
        private IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
        private IPartRepository ipartRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
        IPizzaRepository repPizza = RepositoryFactory.GetInstance().GetRepository<IPizzaRepository, Pizza>();
        private ICOAStatusRepository coaRepository = RepositoryFactory.GetInstance().GetRepository<ICOAStatusRepository, COAStatus>();
        #region ICOAReturn Members
        /// <summary>
        /// Get Product Table
        /// </summary>
        /// <param name="line">line</param>
        /// <param name="editor">editor</param>
        /// <param name="station">station</param>
        /// <param name="customer">customer</param>
        /// <param name="key">key</param>
        /// <param name="SN">SN</param> 
        /// <param name="complete">complete</param>  
        public S_COAReturn GetProductTable(string line, string editor, string station, string customer, string key, List<string> SN, bool complete)
        {
            string keyStr = "";
            try
            {
                bool first = false ;
                S_COAReturn ret = new S_COAReturn();
                ret.reValue = "false";
                string sessionKey = key;
                keyStr = sessionKey;
                List<string> validTemp = new List<string>() ;
                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, SessionType);
                if (null == currentSession)
                {
                    first = true;
                    currentSession = new Session(sessionKey, SessionType, editor, station, line, customer);
                    currentSession.AddValue("SNListForCOAReturn", validTemp);
                }
                validTemp = (List<string>)currentSession.GetValue("SNListForCOAReturn");
                
                foreach (string tmp in SN)
                {
                    if (validTemp.Contains(tmp))
                    {
                    }
                    else
                    {
                        validTemp.Add(tmp);
                    }
                }
                currentSession.AddValue("SNListForCOAReturn", validTemp);
                
                validTemp = (List<string>)currentSession.GetValue("SNListForCOAReturn");
                List<S_RowData_COAReturn> inValidProduct = new List<S_RowData_COAReturn>();
                List<S_RowData_COAReturn> validProduct = new List<S_RowData_COAReturn>();
                foreach (string tmp in validTemp)
                {
                    S_RowData_COAReturn aRow = new S_RowData_COAReturn();
                    aRow.SN = tmp;
                    aRow.COAorError = "";
                    IProduct temp = productRepository.GetProductByCustomSn(tmp);
                    if (null != temp)
                    {
                        // IMES_FA..Product_Part 表中与当前Product 绑定的Parts 
                        IList<IProductPart> productParts = new List<IProductPart>();
                        productParts = temp.ProductParts;
                        /*if (productParts == null || productParts.Count <= 0)
                        {
                            aRow.COAorError = "未结合COA / OOA";
                            inValidProduct.Add(aRow);
                            continue;
                        }*/
                        COAReturnInfo cond = new COAReturnInfo();
                        cond.custsn = tmp;
                        COAReturnInfo empty = new COAReturnInfo();
                        empty.status = "";
                        IList<COAReturnInfo> reCOAReturnInfo =  coaRepository.GetCOAReturnInfoList(cond, empty);
                        if (null != reCOAReturnInfo && reCOAReturnInfo.Count > 0)
                        {
                           aRow.COAorError = "已经完成本站!";
                           inValidProduct.Add(aRow);
                           continue;
                        }
                        foreach (ProductPart iprodpart in productParts)
                        {
                            IPart curPart = ipartRepository.GetPartByPartNo(iprodpart.PartID);
                            if (curPart.BOMNodeType == "P1" && curPart.Descr.IndexOf("COA") == 0)
                            {
                                COAStatus reCOA = coaRepository.Find(iprodpart.PartSn);
                                if (null == reCOA)
                                {
                                    aRow.COAorError = "不存在COA";
                                    inValidProduct.Add(aRow);
                                }
                                else
                                {
                                    aRow.COAorError = iprodpart.PartSn;
                                    validProduct.Add(aRow);
                                }
                                break;
                            }
                        }
                        string[] pizzIds;
                        pizzIds = new string[] { temp.PizzaID };
                        IList<string> partsn = repPizza.GetPartNoListFromPizzaPart(pizzIds, "P1", "DESC", "OOA");
                        foreach (string aPartSn in partsn)
                        {
                            COAStatus reCOA = coaRepository.Find(aPartSn);
                            if (null == reCOA)
                            {
                                aRow.COAorError = "不存在OOA";
                                inValidProduct.Add(aRow);
                            }
                            else
                            {
                                aRow.COAorError = aPartSn;
                                aRow.OOA = "true";
                                validProduct.Add(aRow);
                            }
                            break;
                        }
                        if (aRow.COAorError == "")
                        {
                            aRow.COAorError = "未结合COA / OOA";
                            inValidProduct.Add(aRow);
                        }
                        continue;
                    }
                    else
                    {
                        aRow.COAorError = "Invalid Customer S/N!";
                        inValidProduct.Add(aRow);
                        continue;
                    }
                }
                ret.reValue = "true";
                ret.validProduct = validProduct;
                ret.inValidProduct = inValidProduct;
                currentSession.AddValue("ValidProduct", validProduct);
                if (first == true && inValidProduct.Count == 0)
                {
                    SessionManager.GetInstance.AddSession(currentSession);
                }
                return ret;
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg, e);
                throw new Exception(e.mErrmsg);
            }
            catch (Exception ee)
            {
                throw ee;
            }
        }
        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="key"></param>
        public void Cancel(string key)
        {
            logger.Debug("(_COAReturn)Cancel start, [key]:" + key);
            List<string> erpara = new List<string>();
            string sessionKey = key;

            try
            {
                Session session = SessionManager.GetInstance.GetSession(sessionKey, SessionType);
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
                logger.Debug("(_COAReturn)Cancel end, [key]:" + key);
            }
        }
        /// <summary>
        /// 接着做
        /// </summary>
        /// <param name="key"></param>
        public void MakeSureSave(string key)
        {
            logger.Debug("(_COAReturn)MakeSureSave start, [key]: " + key);
            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = key;
            try
            {
                Session session = SessionManager.GetInstance.GetSession(sessionKey, SessionType);
                if (session == null)
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK021", erpara);
                    throw ex;
                }
                else
                {

                    List<S_RowData_COAReturn> validProduct = (List < S_RowData_COAReturn >)session.GetValue("ValidProduct");
                    if (null == validProduct || validProduct.Count == 0)
                    {
                        return;
                    }
                    IUnitOfWork uow = new UnitOfWork();
                    foreach (S_RowData_COAReturn tmp in validProduct)
                    {
                        COAStatus reCOA = coaRepository.Find(tmp.COAorError);
                        COAReturnInfo item = new COAReturnInfo();
                        item.custsn = tmp.SN;
                        item.coasn = tmp.COAorError;
                        item.originalStatus = reCOA.Status;
                        item.status = "";
                        item.line = reCOA.LineID;
                        item.editor = reCOA.Editor;
                        item.cdt = DateTime.Now;
                        item.udt = DateTime.Now;
                        coaRepository.InsertCOAReturnDefered(uow, item);
                    }
                    uow.Commit();
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
                Session sessionDelete = SessionManager.GetInstance.GetSession(key, SessionType);
                if (sessionDelete != null)
                {
                    SessionManager.GetInstance.RemoveSession(sessionDelete);
                }
                logger.Debug("(_COAReturn)MakeSureSave end, key:" + key);
            }
        }
        #endregion
    }
}
