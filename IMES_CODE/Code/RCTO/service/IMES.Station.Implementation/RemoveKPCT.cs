﻿/*
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Workflow.Runtime;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.Route;
using IMES.Station.Interface.StationIntf;
using log4net;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.Repository._Schema;
using IMES.FisObject.Common.FisBOM; 
using IMES.FisObject.Common.Part;
using IMES.FisObject.PCA.MB;
using System.Linq; 
using IMES.DataModel;

namespace IMES.Station.Implementation
{

    public class RemoveKPCT : MarshalByRefObject, IRemoveKPCT
    {
        private const Session.SessionType TheType = Session.SessionType.Product;
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
        IPartRepository partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();


        private IList<ProductPart> GetNowParts(IProduct currentProduct, string keyConstType)
        {
            List<ProductPart> nowParts = new List<ProductPart>();
            List<string> errpara = new List<string>();
            IList<ConstValueInfo> cvInfo = new List<ConstValueInfo>();

            ConstValueInfo cvCond = new ConstValueInfo();
            cvCond.type = keyConstType;
            cvCond.name = currentProduct.Model;
            cvInfo = partRepository.GetConstValueInfoList(cvCond);

            if (cvInfo == null || cvInfo.Count == 0)
            {
                cvCond = new ConstValueInfo();
                cvCond.type = keyConstType;
                cvCond.name = currentProduct.Family;
                cvInfo = partRepository.GetConstValueInfoList(cvCond);
            }
            if (cvInfo == null || cvInfo.Count == 0)
            {
                //throw new FisException("CHK1026", errpara); // 此机器没有要解的料
                return nowParts;
            }

            ConstValueInfo tmp = cvInfo[0];
            string[] wantRemovePartTypes = tmp.value.Split(',');
            foreach (string wantRemovePartType in wantRemovePartTypes)
            {
                ProductPart cond = new ProductPart();
                cond.ProductID = currentProduct.ProId;
                cond.CheckItemType = wantRemovePartType;
                IList<ProductPart> list = productRepository.GetProductPartList(cond);
                //if (list == null || list.Count == 0)
                //{
                //    errpara.Add(wantRemovePartType);
                //    throw new FisException("CHK1027", errpara); // 该机器没有结合@KP，不能过此站
                //}
                if (list != null)
                    nowParts.AddRange(list);
            }
            return nowParts;
        }

        /// <summary>
        /// GetParts
        /// </summary>
        public DataTable GetParts(string prodid, string mbct2, string pdLine, string editor, string stationId, string customerId)
        {
            logger.Debug("(RemoveKPCT)GetParts start, prodid:" + prodid + " mbct2: "+ mbct2 + ", pdLine: " + pdLine + " , editor: " + editor + " , stationId: " + stationId + " , customerId: " + customerId);
            List<string> errpara = new List<string>();

            try
            {
                IProduct currentProduct = CommonImpl.GetProductByInput(prodid, CommonImpl.InputTypeEnum.ProductIDOrCustSN);
                if (null == currentProduct)
                {
                    errpara.Add(prodid);
                    throw new FisException("SFC002", errpara);
                }

                CommonImpl.GetInstance().CheckProductBlockStation(currentProduct as Product, pdLine, editor, stationId, customerId);

                IMBRepository mbRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();
                string mbCT2 = "";
                if (!string.IsNullOrEmpty(currentProduct.PCBID))
                {
                   // mbCT2 = mbRepository.GetPCBInfoValue(currentProduct.PCBID, "MBCT2");
                    IList<string> mbCT2list = currentProduct.ProductInfoes.Where(x => (x.InfoType == "ModelCT" || x.InfoType == "SleeveCT") && x.InfoValue != "").Select(p => p.InfoValue).ToList();
                   if (mbCT2list != null && mbCT2list.Count > 0)
                   {
                       mbCT2 = mbCT2list[0];
                   }
                   else
                   {
                       mbCT2 = mbRepository.GetPCBInfoValue(currentProduct.PCBID, "MBCT2");
                   }
                }
                if (string.IsNullOrEmpty(mbCT2))
                {
                    throw new FisException("CHK1049", errpara); // 未生成MBCT2
                }
                
                if (!mbct2.Equals(mbCT2))
                {
                    throw new FisException("CHK1048", errpara); // MBCT2不匹配，请确认
                }

                DataTable ret = new DataTable();
                ret.Columns.Add(new DataColumn("PartSn"));
                ret.Columns.Add(new DataColumn("KP"));
                ret.Columns.Add(new DataColumn("ScanedPartSn"));

                IList<ProductPart> nowParts = GetNowParts(currentProduct, "JSRemoveKP");
                foreach (ProductPart p in nowParts)
                {
                    DataRow row = ret.NewRow();
                    row[0] = p.PartSn;
                    row[1] = p.CheckItemType;
                    row[2] = "";
                    ret.Rows.Add(row);
                }
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
                logger.Debug("(RemoveKPCT)GetParts end, prodid:" + prodid + " mbct2: " + mbct2 + ", pdLine: " + pdLine + " , editor: " + editor + " , stationId: " + stationId + " , customerId: " + customerId);
            }
        }


        /// <summary>
        /// CheckProduct
        /// </summary>
        public void CheckProduct(string prodid, string pdLine, string editor, string stationId, string customerId)
        {
            logger.Debug("(RemoveKPCT)CheckProduct start, prodid:" + prodid + " , pdLine: " + pdLine + " , editor: " + editor + " , stationId: " + stationId + " , customerId: " + customerId);
            List<string> errpara = new List<string>();

            try
            {
                IProduct currentProduct = CommonImpl.GetProductByInput(prodid, CommonImpl.InputTypeEnum.ProductIDOrCustSN);
                if (null == currentProduct)
                {
                    errpara.Add(prodid);
                    throw new FisException("SFC002", errpara);
                }

                CommonImpl.GetInstance().CheckProductBlockStation(currentProduct as Product, pdLine, editor, stationId, customerId);
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
                logger.Debug("(RemoveKPCT)CheckProduct end, prodid:" + prodid + " , pdLine: " + pdLine + " , editor: " + editor + " , stationId: " + stationId + " , customerId: " + customerId);
            }
        }

        /// <summary>
        /// RemoveParts
        /// </summary>
        /// <param name="prodid">prodid</param>
		/// <param name="editor">editor</param>
		/// <param name="customerId">customerId</param>
        public void RemoveParts(string prodid, string pdLine, string editor, string stationId, string customerId)
		{
            logger.Debug("(RemoveKPCT)RemoveParts start, prodid:" + prodid + " , pdLine: " + pdLine + " , editor: " + editor + " , stationId: " + stationId + " , customerId: " + customerId);

            FisException ex;
            List<string> erpara = new List<string>();

            try
            {
                string sessionKey = prodid;
                Session CurrentSession = SessionManager.GetInstance.GetSession(sessionKey, TheType);
                if (CurrentSession == null)
                {
                    CurrentSession = new Session(sessionKey, TheType, editor, stationId, pdLine, customerId);
                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", stationId);
                    wfArguments.Add("CurrentFlowSession", CurrentSession);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", pdLine);
                    wfArguments.Add("Customer", customerId);
                    wfArguments.Add("SessionType", TheType);

                    string wfName, rlName;

                    RouteManagementUtils.GetWorkflow(stationId, "RemoveKPCT_JS.xoml", null, out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    CurrentSession.AddValue(Session.SessionKeys.ProductIDOrCustSN, prodid);

                    CurrentSession.SetInstance(instance);

                    if (!SessionManager.GetInstance.AddSession(CurrentSession))
                    {
                        CurrentSession.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");
                        erpara.Add(sessionKey);
                        ex = new FisException("CHK020", erpara);
                        throw ex;
                    }

                    CurrentSession.WorkflowInstance.Start();
                    CurrentSession.SetHostWaitOne();
                }
                else
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK020", erpara);
                    throw ex;
                }

                //check workflow exception
                if (CurrentSession.Exception != null)
                {
                    if (CurrentSession.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        CurrentSession.ResumeWorkFlow();
                    }
                    throw CurrentSession.Exception;
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
                throw new SystemException(e.Message);
            }
            finally
            {
                logger.Debug("(RemoveKPCT)RemoveParts end, prodid:" + prodid + " , pdLine: " + pdLine + " , editor: " + editor + " , stationId: " + stationId + " , customerId: " + customerId);
            }
            
        }

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="sessionKey"></param>
        public void Cancel(string prodid)
        {
            string sessionKey = prodid;
            try
            {
                logger.Debug("(RemoveKPCT)Cancel start, prodid:" + prodid);

                Session session = SessionManager.GetInstance.GetSession(sessionKey, Session.SessionType.Product);

                if (session != null)
                {
                    SessionManager.GetInstance.RemoveSession(session);
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
                logger.Debug("(RemoveKPCT)Cancel end, prodid:" + prodid + " ,sessionKey:" + sessionKey);
            }
        }

        
    }
}