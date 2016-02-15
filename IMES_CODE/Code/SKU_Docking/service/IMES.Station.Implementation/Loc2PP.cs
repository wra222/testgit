// INVENTEC corporation ©2011 all rights reserved. 
// Description:PAQC Input 
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// Known issues:
// TODO：

using log4net;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Workflow.Runtime;
using IMES.Station.Interface.StationIntf;
using IMES.Infrastructure;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.Route;
using IMES.DataModel;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.Station;

namespace IMES.Station.Implementation
{
    /// <summary>
    /// BGAOutput接口的实现类
    /// </summary>
    public class Loc2PP : MarshalByRefObject, ILoc2PP 
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const Session.SessionType SessionType = Session.SessionType.Product;

        #region members

        /// <summary>
        /// 刷prodId，检查输入的prodId
        /// </summary>
        /// <param name="prodId"></param>
        public string ChkProdId(string prodId)
        {
            logger.Debug("(Loc2PP)InputprodId start, prodId:" + prodId);
            try
            {
				IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
                IProduct currentProduct = productRepository.GetProductByIdOrSn(prodId);
                if (currentProduct == null)
					return "";
                return prodId;
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
                logger.Debug("(Loc2PP)InputprodId end, prodId:" + prodId);
            }
        }

        /// <summary>
        /// 刷库位后，存数据
        /// </summary>
		/// <param name="prodId"></param>
        /// <param name="loc"></param>
		/// <param name="line"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        public string InputLoc(string prodId, string loc, string line, string editor, string station, string customer)
        {
            logger.Debug("(Loc2PP)InputLoc start,prodId:" + prodId
                + " [loc]: " + loc);
            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = prodId;
            var productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            try
            {
                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, SessionType);

                if (currentSession == null)
                {
                    currentSession = new Session(sessionKey, SessionType, editor, station, line, customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", currentSession);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", line);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", SessionType);
                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(station, "Loc2PP.xoml", "loc2pp.rules", out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    currentSession.AddValue(Session.SessionKeys.InfoValue, loc);
					currentSession.AddValue(Session.SessionKeys.AttributeValue, loc);

                    string infoTypeLocType = "LocType";
                    currentSession.AddValue(infoTypeLocType, "");
                    string NeedInsert66 = "ADD66";
                    currentSession.AddValue(NeedInsert66,true);

                    if (loc.Length >= 3)
                    {
                        currentSession.AddValue(infoTypeLocType, loc.Substring(0, 3));
                    }
                    else
                    {
                        currentSession.AddValue(infoTypeLocType, loc);//altodownload
                    
                    }
                    if (loc.Trim() == "AD")
                    {
                        var currentProduct = productRepository.GetProductByIdOrSn(sessionKey);
                        if (LastLogIs66Pass(currentProduct))
                        {
                            currentSession.AddValue(NeedInsert66, false);//自动download 如果机器最后有66 log,需提示直接进PAK,不需要insert 过站记录.
                        }
                    
                    }

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
                if (loc.Trim() == "AD")
                {
                    return GetNextDownLoad(sessionKey);
                }
                else
                {
                    return loc;
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
                logger.Debug("(Loc2PP)InputLoc end,prodId:" + prodId
                   + " [loc]: " + loc);
            }
        }

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="prodId"></param>
        public void cancel(string prodId)
        {
            logger.Debug("(Loc2PP)Cancel start, [prodId]:" + prodId);
            //FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = prodId;

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
                logger.Debug("(Loc2PP)Cancel end, [prodId]:" + prodId);
            } 
        }

        private string GetNextDownLoad(string prodId)
        {
            var productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            var currentProduct = productRepository.GetProductByIdOrSn(prodId);
            string type = currentProduct.GetExtendedProperty("PTYPE");
            if (LastLogIs66Pass(currentProduct))
            {
                type = "66";
            }
            return type;
        }
        private bool LastLogIs66Pass(IProduct p)
        {
            ProductLog plog=p.ProductLogs.OrderBy(x => x.Cdt).Last();
            if (plog!=null)
            {
                if (plog.Station == "66" && plog.Status == StationStatus.Pass)
                {
                    return true;
                }
                return false;
            }
            return false;
        }
        #endregion
    }
}
