/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: PCARepairImpl
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2010-04-03   207006            Create 
 * 2010-05-12   207006            ITC-1122-0296
 * 2010-05-12   207006            ITC-1122-0298
 * 2010-05-12   207006            ITC-1122-0299
 * 2010-05-14   207006            ITC-1122-0300
 * 2010-05-14   207006            ITC-1122-0301
 * 2010-05-14   207006            ITC-1122-0302
 * 2010-05-14   207006            ITC-1122-0303
 * 2010-05-14   207006            ITC-1122-0304
 * 2010-05-14   207006            ITC-1122-0305
 * 2010-05-14   207006            ITC-1122-0306
 * 2010-05-14   207006            ITC-1122-0307
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
using log4net;
using IMES.DataModel;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PCA.MB;
using IMES.FisObject.Common.PartSn ;
using IMES.Route;
namespace IMES.Station.Implementation
{
    public class _105BorrowAndReturn : MarshalByRefObject, IBorrowAndReturn
    {
        private const Session.SessionType TheType = Session.SessionType.Product;
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
       
        #region IBorrowAndReturn Members

        public IList<BorrowLog> Query(string status)
        {
            var productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            if (string.IsNullOrEmpty(status))
            {
                status = null;
            }
            return productRepository.GetBorrowLogByStatus(status);
        }

        public void Borrow(string inputSN, string borrower, string editor)
        {

//2010-05-12   207006            ITC-1122-0296
//2010-05-12   207006            ITC-1122-0298
//2010-05-12   207006            ITC-1122-0299
            if (inputSN.Length == 10 && inputSN[4] == 'M')
            {
                BorrowMB(inputSN, borrower, editor);
            }
            else if (inputSN.Length == 9) 
            {
                 BorrowProduct(inputSN, borrower );
            }
            else if (inputSN.Length == 14 || inputSN.Length == 13)
            {
                BorrowCT(inputSN, borrower, editor);
            }
            else
            {
                throw new FisException("CHK129", new string[] { });
            }
        }

        public void ReturnIt(string inputSN, string borrower, string editor)
        {
//2010-05-14   207006            ITC-1122-0300
//2010-05-14   207006            ITC-1122-0301
//2010-05-14   207006            ITC-1122-0302
//2010-05-14   207006            ITC-1122-0303
            if (inputSN.Length == 10 && inputSN[4] == 'M')
            {
                ReturnMB(inputSN, borrower, editor);
            }
            else if (inputSN.Length == 9) 
            {
                ReturnProduct(inputSN, borrower);
            }
            else if (inputSN.Length == 14 || inputSN.Length == 13)
            {
                ReturnCT(inputSN, borrower, editor);
            }
            else
            {
                throw new FisException("CHK129", new string[] { });
            }
        }

        public string inputSN(string inputSN, string model, string station, string customer, string editor, out string outInput)
        {
//2010-05-14   207006            ITC-1122-0304
//2010-05-14   207006            ITC-1122-0305
//2010-05-14   207006            ITC-1122-0306
//2010-05-14   207006            ITC-1122-0307  
            outInput = string.Empty;
            if (inputSN.Length == 10 && inputSN[4] == 'M')
            {
                return inputMB(inputSN, model, station, customer, editor);
            }
            else if (inputSN.Length == 9)
            {
                return inputProduct(inputSN, model, station, customer, editor);
            }
            else if (inputSN.Length == 14)
            {
                return inputCT(inputSN, model, station, customer, editor, out outInput);
            }
            else
            {
                throw new FisException("CHK129", new string[] { });
            }
        }

        public string inputModel(string inputSN, string model)
        {
            if (!string.IsNullOrEmpty(inputSN) && inputSN.Length == 9)
            {
                FisException ex;
                List<string> erpara = new List<string>();
                string sessionKey = inputSN;
                Session Session = SessionManager.GetInstance.GetSession(sessionKey, TheType);
                if (Session == null)
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK021", erpara);
                    //ex.logErr("", "", "", "", "83");
                    //logger.Error(ex);
                    throw ex;
                }
                
                var currentProduct = (Product)Session.GetValue(Session.SessionKeys.Product);
                if (currentProduct.Model != model)
                {
                    var ex2 = new FisException("CHK128", new string[] { });
                    throw ex2;
                }
            }
            return string.Empty;
        }

        #endregion


        private string inputProduct(string inputSN, string model, string station, string customer, string editor)
        {
            logger.Debug("(BorrowAndReturn)inputProduct start, inputSN:" + inputSN + " editor:" + editor + " station:" + station + " customer:" + customer);
            string pdLine = string.Empty;
            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = inputSN;
            try
            {
                Session Session = SessionManager.GetInstance.GetSession(sessionKey, TheType);

                if (Session == null)
                {
                    Session = new Session(sessionKey, TheType, editor, station, pdLine, customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    //一个MB_SNo对应一个workflow
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", Session);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", pdLine);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", TheType);
                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(station, "105BorrowOrReturn.xoml", "105BorrowOrReturn.rules", out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    Session.AddValue(Session.SessionKeys.ModelName , model);
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
                   
                var productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
                IList<BorrowLog> borrowLog = productRepository.GetBorrowLogBySno(inputSN);
                if (!(borrowLog == null || borrowLog.Count == 0))
                {
                    return borrowLog[0].Model;
                }

                return string.Empty;
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
                logger.Debug("(BorrowAndReturn)inputProduct end, inputSN:" + inputSN + " editor:" + editor + " station:" + station + " customer:" + customer);
            }
        }

        private string inputMB(string inputSN, string model, string station, string customer, string editor)
        {
            logger.Debug("(BorrowAndReturn)inputMB start, inputSN:" + inputSN + " editor:" + editor + " station:" + station + " customer:" + customer);
            try
            {
                IMBRepository mbRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();
                var mb = mbRepository.Find(inputSN);
                if (mb == null)
                {
                    var ex = new FisException("SFC001", new string[] { inputSN });
                    throw ex;
                }

                var productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
                IList<BorrowLog> borrowLog = productRepository.GetBorrowLogBySno(inputSN);
                if (!(borrowLog == null || borrowLog.Count == 0))
                {
                    return borrowLog[0].Model;
                }

                return string.Empty; 
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
                logger.Debug("(BorrowAndReturn)inputProduct end, inputSN:" + inputSN + " editor:" + editor + " station:" + station + " customer:" + customer);
            }


        }

        private string inputCT(string inputSN, string model, string station, string customer, string editor,out string output)
        {
            logger.Debug("(BorrowAndReturn)inputCT start, inputSN:" + inputSN + " editor:" + editor + " station:" + station + " customer:" + customer);

            try
            {
              
                var partRepository = RepositoryFactory.GetInstance().GetRepository<IPartSnRepository, PartSn>();

                PartSn CT = partRepository.Find(inputSN);
                if (CT == null)
                {

                    CT = partRepository.Find(inputSN.Substring(0, 13));
                    output = inputSN.Substring(0, 13);
                    inputSN = output;
                    if (CT == null)
                    {
                        var ex = new FisException("CHK127", new string[] { inputSN });
                        throw ex;
                    }
                }
                else
                {
                    output = inputSN;
                }
                var productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
                IList<BorrowLog> borrowLog = productRepository.GetBorrowLogBySno(inputSN);
                if (!(borrowLog == null || borrowLog.Count == 0))
                {
                    return borrowLog[0].Model;
                }

                return string.Empty; 
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
                logger.Debug("(BorrowAndReturn)inputCT end, inputSN:" + inputSN + " editor:" + editor + " station:" + station + " customer:" + customer);
            }
           
        }

        private void BorrowProduct(string inputSN, string borrower)
        {
            logger.Debug("(BorrowAndReturn)BorrowProduct start, inputSN:" + inputSN);

            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = inputSN;
            Session Session = SessionManager.GetInstance.GetSession(sessionKey, TheType);
            if (Session == null)
            {
                erpara.Add(sessionKey);
                ex = new FisException("CHK021", erpara);
                //ex.logErr("", "", "", "", "83");
                //logger.Error(ex);
                throw ex;
            }
            try
            {
                Session.Exception = null;
                Session.AddValue(Session.SessionKeys.BorrowOrReturn, "B");
                Session.AddValue(Session.SessionKeys.Borrower, borrower);
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
                throw e;
            }
            catch (Exception e)
            {
                throw new SystemException(e.Message);
            }
            finally
            {
                logger.Debug("(BorrowAndReturn)BorrowProduct end, inputSN:" + inputSN);
            }
        }

        private void BorrowMB(string inputSN, string borrower, string editor)
        {
            logger.Debug("(BorrowAndReturn)BorrowMB start, inputSN:" + inputSN + " editor:" + editor + " borrower:" + borrower);

            try
            {
                checkForBorrowed(inputSN);
                var mbRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();
                              
                
                BorrowLog item = new BorrowLog();

                item.Status = "B";
                item.Sn = inputSN;
                item.Model = "MB";
                item.Borrower = borrower;
                item.Lender = editor;

                var productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
                 
                productRepository.AddBorrowLog(item);
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
                logger.Debug("(BorrowAndReturn)BorrowMB end, inputSN:" + inputSN + " editor:" + editor + " borrower:" + borrower);
            }
        }


        private void BorrowCT(string inputSN, string borrower, string editor)
        {
            logger.Debug("(BorrowAndReturn)BorrowCT start, inputSN:" + inputSN + " editor:" + editor + " borrower:" + borrower);

            try
            {
                checkForBorrowed(inputSN);
                var partRepository = RepositoryFactory.GetInstance().GetRepository<IPartSnRepository, PartSn>();

                BorrowLog item = new BorrowLog();

                item.Status = "B";
                item.Sn = inputSN;
                item.Model = "KP";
                item.Borrower = borrower;
                item.Lender = editor;

                var productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();

                productRepository.AddBorrowLog(item);
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
                logger.Debug("(BorrowAndReturn)BorrowCT end, inputSN:" + inputSN + " editor:" + editor + " borrower:" + borrower);
            }
        }

        private void ReturnProduct(string inputSN, string lender)
        {
            logger.Debug("(BorrowAndReturn)ReturnProduct start, inputSN:" + inputSN);

            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = inputSN;
            Session Session = SessionManager.GetInstance.GetSession(sessionKey, TheType);
            if (Session == null)
            {
                erpara.Add(sessionKey);
                ex = new FisException("CHK021", erpara);
                //ex.logErr("", "", "", "", "83");
                //logger.Error(ex);
                throw ex;
            }
            try
            {
                Session.Exception = null;
                Session.AddValue(Session.SessionKeys.BorrowOrReturn, "R");
                Session.AddValue(Session.SessionKeys.Lender, lender);
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
                throw e;
            }
            catch (Exception e)
            {
                throw new SystemException(e.Message);
            }
            finally
            {
                logger.Debug("(BorrowAndReturn)ReturnProduct end, inputSN:" + inputSN);
            }

        }

        private void ReturnMB(string inputSN, string lender, string editor)
        {
            logger.Debug("(BorrowAndReturn)ReturnMB start, inputSN:" + inputSN + " editor:" + editor + " lender:" + lender);

            try
            {
                checkForReturned(inputSN);
                var mbRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();
                //var mbObj = (MB)mbRepository.Find(inputSN);

                BorrowLog item = new BorrowLog();

                
                item.Status = "R";
                item.Sn = inputSN;
                item.Model = "MB";
                item.Returner = lender;
                item.Acceptor = editor;
                 

                var productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();

                productRepository.UpdateBorrowLog(item);
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
                logger.Debug("(BorrowAndReturn)ReturnMB end, inputSN:" + inputSN + " editor:" + editor + " lender:" + lender);
            }
        }

        private void ReturnCT(string inputSN, string lender, string editor)
        {

            logger.Debug("(BorrowAndReturn)ReturnCT start, inputSN:" + inputSN + " editor:" + editor + " lender:" + lender);

            try
            {
                checkForReturned(inputSN);
                var partRepository = RepositoryFactory.GetInstance().GetRepository<IPartSnRepository, PartSn>();
                //inputSNvar partObj = (PartSn)partRepository.Find(inputSN);

                BorrowLog item = new BorrowLog();

                item.Status = "R";
                item.Sn = inputSN;
                item.Model = "KP";
                item.Returner = lender;
                item.Acceptor = editor;

                var productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();

                productRepository.UpdateBorrowLog(item);
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
                logger.Debug("(BorrowAndReturn)ReturnCT end, inputSN:" + inputSN + " editor:" + editor + " lender:" + lender);

            }

        }

        private void checkForBorrowed(string inputSN)
        {
            var productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
              
            IList<BorrowLog> borrowLog = productRepository.GetBorrowLogBySno(inputSN);
            if (!(borrowLog == null || borrowLog.Count == 0))
            {
                foreach (BorrowLog bl in borrowLog)
                {
                    if (bl.Status.ToUpper() == "B")
                    {
                        var ex2 = new FisException("CHK125", new string[] { });
                        throw ex2;
                    }
                }
            }
        }


        private void checkForReturned(string inputSN)
        {
            var productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
              
            IList<BorrowLog> borrowLog = productRepository.GetBorrowLogBySno(inputSN);

            bool flag = false;
            if (!(borrowLog == null || borrowLog.Count == 0))
            {
                foreach (BorrowLog bl in borrowLog)
                {
                    if (bl.Status.ToUpper() == "B")
                    {
                        flag = true;
                        break;
                    }
                }

            }
            else
            {
                var ex2 = new FisException("CHK126", new string[] { });
                throw ex2;
            }

            if (!flag)
            {
                var ex2 = new FisException("CHK126", new string[] { });
                throw ex2;
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
    }
}