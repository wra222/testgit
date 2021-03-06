﻿/*
 * INVENTEC corporation (c)2010 all rights reserved. 
 * Description: Unpack Carton/DN/Pallet Impl
 * Update: 
 * Date         Name                Reason 
 * ==========   =================   =====================================
 * 2011-03-10   Lucy Liu            Create
 * 2011-04-12   Lucy Liu            Modify:ITC-1268-0058
 * 2011-09-05  Vincent Lee       add multi-unpackage function
 * 2011-10-20   itc202017           Add UnpackDNByDN implementation
 * Known issues:
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Workflow.Runtime;
using IMES.DataModel;
using IMES.FisObject.Common.MO;
using IMES.FisObject.Common.Model;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.Station.Interface.CommonIntf;
using IMES.Station.Interface.StationIntf;
using log4net;
using IMES.FisObject.PAK.DN;
using IMES.Route;
using System.Data.SqlClient;
using System.Data;
using IMES.Infrastructure.Repository._Schema;
using IMES.Infrastructure.UnitOfWork;
using IMES.FisObject.PAK.Pallet;

namespace IMES.Station.Implementation
{
    /// <summary>
    /// CombinePOInCarton
    /// </summary>
    public class Unpack : MarshalByRefObject, IUnpack
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const Session.SessionType currentSessionType = Session.SessionType.Common;
        private const Session.SessionType ProductSessionType = Session.SessionType.Product;

        #region IUnpack Members

       
        ///<summary>
        /// 解除绑定
        /// 使用工作流070UnpackCarton.xoml
        /// </summary>
        public void UnpackCarton(string cartonNo, string line, string editor, string station, string customer)
        {
            logger.Debug("(UnpackCarton)Unpack start, cartonNo:" + cartonNo + "line:" + line + "editor:" + editor + "station:" + station + "customer:" + customer);

            FisException ex;
            List<string> erpara = new List<string>();

         
            try
            {
                var currentProduct = CommonImpl.GetProductByInput(cartonNo, CommonImpl.InputTypeEnum.Carton);
                //if (currentProduct == null)
                //{
                //    FisException fe = new FisException("CHK079", new string[] { cartonNo });
                //    throw fe;
                //}


                if (!(currentProduct.DeliveryNo == "" || currentProduct.DeliveryNo == null))
                {
                    throw new FisException("CHK141", new string[] { cartonNo });//请先Unpack DN!
                }

                string sessionKey = currentProduct.ProId;
                

                station = "82U";

                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, ProductSessionType);

                if (currentSession == null)
                {
                    currentSession = new Session(sessionKey, ProductSessionType, editor, station, line, customer);


                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", currentSession);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", line);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", ProductSessionType);
                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(station, "070UnpackCarton.xoml", "", out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    currentSession.AddValue(Session.SessionKeys.Product, currentProduct);
                    currentSession.AddValue(Session.SessionKeys.Carton, currentProduct.CartonSN);
                    currentSession.AddValue(Session.SessionKeys.CustSN, currentProduct.CUSTSN);
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
                logger.Debug("(UnpackCarton)Unpack end,  cartonNo:" + cartonNo + "line:" + line + "editor:" + editor + "station:" + station + "customer:" + customer);
            }
        }

        ///<summary>
        /// 解除绑定
        /// 使用工作流070UnpackDN.xoml
        /// </summary>
        public void UnpackDN(string cartonNo, string line, string editor, string station, string customer)
        {
            logger.Debug("(UnpackDN)Unpack start, cartonNo:" + cartonNo + "line:" + line + "editor:" + editor + "station:" + station + "customer:" + customer);

            FisException ex;
            List<string> erpara = new List<string>();


            try
            {
                var currentProduct = CommonImpl.GetProductByInput(cartonNo, CommonImpl.InputTypeEnum.Carton);
                //if (currentProduct == null)
                //{
                //    FisException fe = new FisException("CHK079", new string[] { cartonNo });
                //    throw fe;
                //}


                if (!(currentProduct.PalletNo == "" || currentProduct.PalletNo == null))
                {
                    throw new FisException("CHK142", new string[] { cartonNo });//请先Unpack Pallet!
                }


                if ((currentProduct.DeliveryNo == "" || currentProduct.DeliveryNo == null))
                {
                    throw new FisException("CHK143", new string[] { cartonNo });//该Carton No还未与DN绑定!
                }

                string sessionKey = currentProduct.ProId;


                station = "85U";

                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, ProductSessionType);

                if (currentSession == null)
                {
                    currentSession = new Session(sessionKey, ProductSessionType, editor, station, line, customer);


                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", currentSession);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", line);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", ProductSessionType);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow("070UnpackDN.xoml", "", wfArguments);

                    currentSession.AddValue(Session.SessionKeys.Product, currentProduct);
                    //ITC-1268-0058
                    //由于输入有校验位，所以应该用结构里的值作为session变量
                    currentSession.AddValue(Session.SessionKeys.Carton, currentProduct.CartonSN);
                    currentSession.AddValue(Session.SessionKeys.DeliveryNo, currentProduct.DeliveryNo);
                    //currentSession.AddValue(Session.SessionKeys.CustSN, currentProduct.CUSTSN);
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
                logger.Debug("(UnpackDN)Unpack end,  cartonNo:" + cartonNo + "line:" + line + "editor:" + editor + "station:" + station + "customer:" + customer);
            }
        }

        ///<summary>
        /// 解除绑定
        /// 使用工作流070UnpackPallet.xoml
        /// </summary>
        public void UnpackPallet(string palletNo, string line, string editor, string station, string customer)
        {
            logger.Debug("(UnpackPallet)Unpack start, palletNo:" + palletNo + "line:" + line + "editor:" + editor + "station:" + station + "customer:" + customer);

            FisException ex;
            List<string> erpara = new List<string>();


            try
            {
                var currentProduct = CommonImpl.GetProductByInput(palletNo, CommonImpl.InputTypeEnum.ProductIDOrCustSNOrPallet);
                //if (currentProduct == null)
                //{
                //    FisException fe = new FisException("CHK079", new string[] { cartonNo });
                //    throw fe;
                //}


                string sessionKey = currentProduct.ProId;


                station = "87U";

                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, ProductSessionType);

                if (currentSession == null)
                {
                    currentSession = new Session(sessionKey, ProductSessionType, editor, station, line, customer);


                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", currentSession);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", line);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", ProductSessionType);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow("070UnpackPallet.xoml", "", wfArguments);

                    currentSession.AddValue(Session.SessionKeys.Product, currentProduct);
                   
                    currentSession.AddValue(Session.SessionKeys.PalletNo, currentProduct.PalletNo);
                    //Pallet NO存在,Delivery No肯定不为空
                    currentSession.AddValue(Session.SessionKeys.DeliveryNo, currentProduct.DeliveryNo);
                    currentSession.AddValue(Session.SessionKeys.CustSN, currentProduct.CUSTSN);
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
                logger.Debug("(UnpackPallet)Unpack end,  palletNo:" + palletNo + "line:" + line + "editor:" + editor + "station:" + station + "customer:" + customer);
            }
        }

        ///<summary>
        /// 根据指定的DeliveryNo解除绑定
        /// 使用工作流070UnpackDNByDN.xoml
        /// </summary>
        public void UnpackDNByDN(string deliveryNo, bool bSuperUI, string line, string editor, string station, string customer)
        {
            if (bSuperUI)
            {
                logger.Debug("(UnpackDNByDN[Super])Unpack start, deliveryNo:" + deliveryNo + "line:" + line + "editor:" + editor + "station:" + station + "customer:" + customer);
            }
            else
            {
                logger.Debug("(UnpackDNByDN)Unpack start, deliveryNo:" + deliveryNo + "line:" + line + "editor:" + editor + "station:" + station + "customer:" + customer);
            }

            FisException ex;
            List<string> erpara = new List<string>();


            try
            {
                var currentDelivery = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, IMES.FisObject.PAK.DN.Delivery>().Find(deliveryNo);
                
                if (null == currentDelivery)
                {
                    throw new FisException("CHK190", new string[] { deliveryNo });//DN不存在
                }

                if (!bSuperUI && "98" == currentDelivery.Status)
                {
                    throw new FisException("CHK191", new string[] { deliveryNo });//DN已上传
                }

                string sessionKey = currentDelivery.DeliveryNo;


                station = "80";

                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, ProductSessionType);

                if (currentSession == null)
                {
                    currentSession = new Session(sessionKey, ProductSessionType, editor, station, line, customer);


                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", currentSession);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", line);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", ProductSessionType);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow("UnpackDNByDN.xoml", "", wfArguments);

                    currentSession.AddValue(Session.SessionKeys.DeliveryNo, currentDelivery.DeliveryNo);
                    currentSession.AddValue(Session.SessionKeys.Delivery, currentDelivery);
                    currentSession.SetInstance(instance);

                    if (!SessionManager.GetInstance.AddSession(currentSession))
                    {
                        currentSession.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");
                        erpara.Add(sessionKey);
                        ex = new FisException("CHK192", erpara);
                        throw ex;
                    }

                    currentSession.WorkflowInstance.Start();
                    currentSession.SetHostWaitOne();
                }
                else
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK193", erpara);
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
                if (bSuperUI)
                {
                    logger.Debug("(UnpackDNByDN[Super])Unpack end,  deliveryNo:" + deliveryNo + "line:" + line + "editor:" + editor + "station:" + station + "customer:" + customer);
                }
                else
                {
                    logger.Debug("(UnpackDNByDN)Unpack end,  deliveryNo:" + deliveryNo + "line:" + line + "editor:" + editor + "station:" + station + "customer:" + customer);
                }
            }
        }

        ///<summary>
        /// 根据指定的Custom Sn解除绑定
        /// 使用工作流070UnpackDNBySN.xoml
        /// </summary>
        public void UnpackDNbySNCheck(string prodSn, string pdline, string editor, string station, string customer)
        {
            logger.Debug("(UnpackDNbySNCheck)UnpackDNbySNCheck start, prodSn:" + prodSn + "line:" + pdline + "editor:" + editor + "station:" + station + "customer:" + customer);

            string currentSessionKey = prodSn;
            try
            {
                if (station == "91")
                {
                    station = "9U";
                }
                else
                {
                    station = "9P";
                }
                //Session currentCommonSession = SessionManager.GetInstance.GetSession(currentSessionKey, ProductSessionType);
                Session currentCommonSession = SessionManager.GetInstance.GetSession(currentSessionKey, ProductSessionType);


                if (currentCommonSession == null)
                {
                    //currentCommonSession = new Session(currentSessionKey, Session.SessionType.Common, editor, station, pdline, customer);
                    currentCommonSession = new Session(currentSessionKey, ProductSessionType, editor, station, pdline, customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", currentSessionKey);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", currentCommonSession);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", pdline);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", Session.SessionType.Product);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow("070UnpackDNBySN.xoml", "", wfArguments);

                    currentCommonSession.AddValue(Session.SessionKeys.CN, "DN");
                    currentCommonSession.AddValue(Session.SessionKeys.IsComplete, false);
                    currentCommonSession.SetInstance(instance);

                    if (!SessionManager.GetInstance.AddSession(currentCommonSession))
                    {
                        currentCommonSession.WorkflowInstance.Terminate("Session:" + currentSessionKey + " Exists.");
                        FisException ex;
                        List<string> erpara = new List<string>();
                        erpara.Add(currentSessionKey);
                        ex = new FisException("CHK020", erpara);
                        throw ex;
                    }

                    currentCommonSession.WorkflowInstance.Start();
                    currentCommonSession.SetHostWaitOne();
                }
                else
                {
                    FisException ex;
                    List<string> erpara = new List<string>();
                    erpara.Add(currentSessionKey);
                    ex = new FisException("CHK020", erpara);
                    throw ex;
                }

                if (currentCommonSession.Exception != null)
                {
                    if (currentCommonSession.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        currentCommonSession.ResumeWorkFlow();
                    }

                    throw currentCommonSession.Exception;
                }
            /*    
                Product newProduct = (Product)currentCommonSession.GetValue(Session.SessionKeys.Product);
                if (newProduct.PalletNo == null)
                {
                    FisException ex;
                    List<string> erpara = new List<string>();
                    erpara.Add("此机器尚未结合栈板，不能进行Unpack！");
                    ex = new FisException("CHK292", erpara);
                    throw ex;
                }
                currentCommonSession.AddValue(Session.SessionKeys.DeliveryNo, newProduct.DeliveryNo);
                */
               

                return ;
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
                logger.Debug("(UnpackDNbySN)UnpackDNbySNCheck end,  cartonNo:" + prodSn + "line:" + pdline + "editor:" + editor + "station:" + station + "customer:" + customer);
            }
        }

        public string UnpackDNbySNSave(string prodId,bool isPallet)
        {
            logger.Debug("(UnpackDNbySN)UnpackDNbySNSave start,"
                + " [prodId]: " + prodId);

            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = prodId;

            try
            {
                Session session = SessionManager.GetInstance.GetSession(sessionKey, ProductSessionType);

                if (session == null)
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK021", erpara);
                    throw ex;
                }
                else
                {
                    session.AddValue(Session.SessionKeys.Pallet, isPallet);
                    session.Exception = null;
                    session.SwitchToWorkFlow();

                    //check workflow exception
                    if (session.Exception != null)
                    {
                        if (session.GetValue(Session.SessionKeys.WFTerminated) != null)
                        {
                            session.ResumeWorkFlow();
                        }

                        throw session.Exception;
                    }
                    string ret =(string ) session.GetValue(Session.SessionKeys.DCode );
                    return ret;
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
                logger.Debug("(UnpackDNbySN)UnpackDNbySNSave end,"
                   + " [prodId]: " + prodId);
            }
        }


        ///<summary>
        /// 根据指定的sn or dn解除绑定
        /// 使用工作流070UnpackDNByall.xoml
        /// </summary>
        public void UnpackAllBySNCheck(string prodSn, string pdline, string editor, string station, string customer)
        {
            logger.Debug("(UnpackAllBySN)Unpack start, prodid:" + prodSn + "line:" + pdline + "editor:" + editor + "station:" + station + "customer:" + customer);

            string currentSessionKey = prodSn;
            try
            {
                station = "SP";
                Session currentCommonSession = SessionManager.GetInstance.GetSession(currentSessionKey, ProductSessionType);
                if (currentCommonSession == null)
                {
                    currentCommonSession = new Session(currentSessionKey, ProductSessionType, editor, station, pdline, customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", currentSessionKey);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", currentCommonSession);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", pdline);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", Session.SessionType.Product);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow("070UnpackAllBySN.xoml", "070unpackallbysn.rules", wfArguments);

                    currentCommonSession.AddValue(Session.SessionKeys.CN, "ALL");
                    currentCommonSession.AddValue(Session.SessionKeys.Pallet, false);
                    currentCommonSession.AddValue(Session.SessionKeys.IsComplete, false);
                    currentCommonSession.SetInstance(instance);

                    if (!SessionManager.GetInstance.AddSession(currentCommonSession))
                    {
                        currentCommonSession.WorkflowInstance.Terminate("Session:" + currentSessionKey + " Exists.");
                        FisException ex;
                        List<string> erpara = new List<string>();
                        erpara.Add(currentSessionKey);
                        ex = new FisException("CHK020", erpara);
                        throw ex;
                    }

                    currentCommonSession.WorkflowInstance.Start();
                    currentCommonSession.SetHostWaitOne();
                }
                else
                {
                    FisException ex;
                    List<string> erpara = new List<string>();
                    erpara.Add(currentSessionKey);
                    ex = new FisException("CHK020", erpara);
                    throw ex;
                }


                if (currentCommonSession.Exception != null)
                {
                    if (currentCommonSession.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        currentCommonSession.ResumeWorkFlow();
                    }

                    throw currentCommonSession.Exception;
                }

                return;
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
                logger.Debug("(UnpackAllBySN)Unpack end,  prodid:" + prodSn + "line:" + pdline + "editor:" + editor + "station:" + station + "customer:" + customer);
            }

        }

        public string UnpackAllbySNSave(string prodId)
        {
            logger.Debug("(UnpackAllBySN)UnpackAllbySNSave start,"
                + " [prodId]: " + prodId);

            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = prodId;

            try
            {
                Session session = SessionManager.GetInstance.GetSession(sessionKey, ProductSessionType);

                if (session == null)
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK021", erpara);
                    throw ex;
                }
                else
                {
                    session.Exception = null;
                    session.SwitchToWorkFlow();

                    //check workflow exception
                    if (session.Exception != null)
                    {
                        if (session.GetValue(Session.SessionKeys.WFTerminated) != null)
                        {
                            session.ResumeWorkFlow();
                        }
                        throw session.Exception;
                        
                    }
                    return "ok";
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
                logger.Debug("(UnpackAllBySN)UnpackAllbySNSave end,"
                   + " [prodId]: " + prodId);
            }
        }


        ///<summary>
        /// 根据指定的DummyPalletNo解除绑定
        /// 使用工作流UnpackDummyPalletNo.xoml
        /// </summary>
        public void UnpackDummyPalletNo(string DummyPalletNo, string line, string editor, string customer)
        {
            logger.Debug("(UnpackDummyPalletNo)Unpack DummyPalletNo start, DummyPalletNo:" + DummyPalletNo + "line:" + line + "editor:" + editor +  "customer:" + customer);

            FisException ex;
            List<string> erpara = new List<string>();

            try
            {
                IPalletRepository iPalletRepository = RepositoryFactory.GetInstance().GetRepository<IPalletRepository, Pallet>();
                
                IList<DummyShipDetInfo> dummyshipdetList = null;
                dummyshipdetList = iPalletRepository.GetDummyShipDetListByPlt(DummyPalletNo);
              

                if (dummyshipdetList == null || dummyshipdetList.Count<=0)
                {
                    erpara.Add(DummyPalletNo);
                    ex = new FisException("PAK049", erpara);    //此Dummy Pallet No 不存在！
                    throw ex;
                }

                WhPltLogInfo WHPLTLogLog = iPalletRepository.GetWhPltLogInfoNewestly(DummyPalletNo);
                if (WHPLTLogLog == null || !(WHPLTLogLog.wc == "9A" || WHPLTLogLog.wc == "RD"))
                {
                    erpara.Add(DummyPalletNo);
                    ex = new FisException("PAK119", erpara);    //此Dummy Pallet No %1  状态错误，不能解绑！
                    throw ex;
                }

                string sessionKey = DummyPalletNo;
                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, currentSessionType);

                if (currentSession == null)
                {
                    string station = "UD"; //UC 更新：本站站号：UD;
                    currentSession = new Session(sessionKey, currentSessionType, editor, station, line, customer);                  

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", currentSession);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", line);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", currentSessionType);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow("UnpackDummyPalletNo.xoml", "", wfArguments);

                    currentSession.AddValue(Session.SessionKeys.DummyPalletNo,DummyPalletNo);
                    
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
                logger.Debug("(UnpackDummyPalletNo)Unpack DummyPalletNo end,  DummyPalletNo:" + DummyPalletNo + "line:" + line + "editor:" + editor  + "customer:" + customer);
            }
        }




        /// <summary>
        /// Cancel
        /// </summary>
        public void Cancel(string prodId)
        {
            logger.Debug("(Unpack)Cancel start, [prodId]:" + prodId);
            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = prodId;

            try
            {
                Session session = SessionManager.GetInstance.GetSession(sessionKey, ProductSessionType);

                if (session != null)
                {
                    SessionManager.GetInstance.RemoveSession(session);
                    //SessionManager.GetInstance.Rem
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
                logger.Debug("(Unpack)Cancel end, [prodId]:" + prodId);
            }
        }




        #endregion

        #region enhance unpack function
        public DataTable CheckUnpackCarton(IList<string> cartonSNList)
        {
            string strSQL = @"select a.* , b.ShipDate, b.Status as DeliveryStatus,
                                                     (case when LTRIM(RTRIM(a.PalletNo)) ='' and 
                                                                         LTRIM(RTRIM(a.DeliveryNo))='' and 
                                                                         b.Status ='00'   then
                                                                                   1
                                                                   else
                                                                                    0
                                                                   end) as IsAllowUnpack           
                                            from (select CartonSN, DeliveryNo, PalletNo, COUNT(ProductID) as ProdQTY 
	                                                  from Product  
                                                      where CartonSN in ('{0}')
                                                      group by CartonSN, DeliveryNo, PalletNo) a 
                                            left join Delivery b on (a.DeliveryNo = b.DeliveryNo)";

            string inSection = string.Join("','", cartonSNList.ToArray());

            strSQL = string.Format(strSQL, inSection);

            DataTable tb = SqlHelper.ExecuteDataFill(SqlHelper.ConnectionString_GetData,
                                                                                 System.Data.CommandType.Text,
                                                                                strSQL);

            return tb;
        }

        public DataTable CheckUnpackDN(IList<string> cartonSNList)
        {
            string strSQL = @"select a.* , b.ShipDate, b.Status as DeliveryStatus,
                                                     (case when LTRIM(RTRIM(a.PalletNo))='' and
                                                                         LTRIM(RTRIM(a.DeliveryNo)) !='' and    
                                                                         b.Status in ('00', '82')   then
                                                                                   1
                                                                   else
                                                                                    0
                                                                   end) as IsAllowUnpack       
                                            from (select CartonSN, DeliveryNo, PalletNo, COUNT(ProductID) as ProdQTY 
	                                                  from Product  
                                                      where CartonSN in ('{0}')
                                                      group by CartonSN, DeliveryNo, PalletNo) a 
                                            left join Delivery b on (a.DeliveryNo = b.DeliveryNo)";

            string inSection = string.Join("','", cartonSNList.ToArray());

            strSQL = string.Format(strSQL, inSection);

            DataTable tb = SqlHelper.ExecuteDataFill(SqlHelper.ConnectionString_GetData,
                                                                                 System.Data.CommandType.Text,
                                                                                strSQL);
            return tb;
        }
        public DataTable CheckUnpackDN(string deliveryNo)
        {
            string strSQL = @"select a.* , b.ShipDate, b.Status as DeliveryStatus,
                                                     (case when LTRIM(RTRIM(a.PalletNo))='' and 
                                                                         b.Status in ('00', '82')   then
                                                                                   1
                                                                   else
                                                                                    0
                                                                   end) as IsAllowUnpack       
                                            from (select CartonSN, DeliveryNo, PalletNo, COUNT(ProductID) as ProdQTY 
	                                                  from Product  
                                                      where DeliveryNo=@DN')
                                                      group by CartonSN, DeliveryNo, PalletNo) a 
                                            left join Delivery b on (a.DeliveryNo = b.DeliveryNo)";

            SqlParameter paraName = new SqlParameter("@DN", SqlDbType.VarChar, 32);
            paraName.Direction = ParameterDirection.Input;
            paraName.Value = deliveryNo;

            DataTable tb = SqlHelper.ExecuteDataFill(SqlHelper.ConnectionString_GetData,
                                                                                 System.Data.CommandType.Text,
                                                                                strSQL,
                                                                                paraName);
            return tb;
        }
        public DataTable CheckUnpackPallet(IList<string> palletIdList)
        {
            string strSQL = @"select a.* , b.ShipDate, b.Status as DeliveryStatus, c.Status as PalletStatus,
                                                      (case when b.Status in ('00', '82','88')  and  c.Status ='1'  then
                                                                                   1
                                                                   else
                                                                                    0
                                                                   end) as IsAllowUnpack              
                                            from (select a.DeliveryNo, a.PalletNo, b.PalletId, COUNT(ProductID) as ProdQTY 
	                                                    from Product a left join PalletId b on (a.PalletNo = b.PalletNo)     
                                                      where b.PalletId in ('{0}')
                                                      group by a.DeliveryNo, a.PalletNo, b.PalletId) a 
                                                  left join Delivery b on (a.DeliveryNo = b.DeliveryNo)
                                                  left join Delivery_Pallet c on (a.DeliveryNo = c.DeliveryNo and
                                                                                                 a.PalletNo = c.PalletNo)";

            string inSection = string.Join("','", palletIdList.ToArray());

            strSQL = string.Format(strSQL, inSection);

            DataTable tb = SqlHelper.ExecuteDataFill(SqlHelper.ConnectionString_GetData,
                                                                                 System.Data.CommandType.Text,
                                                                                strSQL);
            return tb;
        }

        public DataTable CheckUnpackPallet(string deliveryNo)
        {
            string strSQL = @"select a.* , b.ShipDate, b.Status as DeliveryStatus, c.Status as PalletStatus,
                                                                (case when b.Status in ('00', '82','88') and LTRIM(RTRIM(a.PalletNo)) ! =''   then
                                                                                   1
                                                                   else
                                                                                    0
                                                                   end) as IsAllowUnpack           
                                            from (select a.DeliveryNo, a.PalletNo, b.PalletId, COUNT(ProductID) as ProdQTY 
	                                                    from Product a left join PalletId b on (a.PalletNo = b.PalletNo)     
                                                      where a.DeliveryNo =@DN
                                                      group by a.DeliveryNo, a.PalletNo, b.PalletId) a 
                                                  left join Delivery b on (a.DeliveryNo = b.DeliveryNo)
                                                  left join Delivery_Pallet c on (a.DeliveryNo = c.DeliveryNo and
                                                                                                 a.PalletNo = c.PalletNo)";

            SqlParameter paraName = new SqlParameter("@DN", SqlDbType.VarChar, 32);
            paraName.Direction = ParameterDirection.Input;
            paraName.Value = deliveryNo;

            DataTable tb = SqlHelper.ExecuteDataFill(SqlHelper.ConnectionString_GetData,
                                                                                 System.Data.CommandType.Text,
                                                                                strSQL,
                                                                                paraName);
            return tb;
        }

        public void UpackMultiCarton(IList<string> cartonSNList, string line, string editor, string station, string customer)
        {
            try
            {

                SqlTransactionManager.Begin();
                DateTime now = DateTime.Now;
                BackupUnpackProduct(cartonSNList, editor,now);
                BackupUnpackProductStatus(cartonSNList, editor, now);

                updateProductStatus(cartonSNList, line, editor, "68", 1, now);
                //writeProductLog(cartonSNList, line, editor, "68", 1, now);
                writeProductLog(cartonSNList, line, editor, "82U", 1, now);

                clearCartonOnTestDataBoxLog(cartonSNList);

                unBindCartonOnProduct(cartonSNList); 
                SqlTransactionManager.Commit();
            }
            catch (Exception e)
            {
                SqlTransactionManager.Rollback();
                throw e;
            }
            finally
            {
                SqlTransactionManager.Dispose();
                SqlTransactionManager.End();
            }

        }

        

        public void UpackDNByMutiCarton(IList<string> cartonSNList, string line, string editor, string station, string customer)
        {
            try
            {

                SqlTransactionManager.Begin();
                DateTime now = DateTime.Now;

                BackupUnpackProduct(cartonSNList, editor, now);
                BackupUnpackProductStatus(cartonSNList, editor, now);
                
                updateDeliveryStatus(cartonSNList, editor, "00", now);

                updateProductStatus(cartonSNList, line, editor, "83", 1, now);
                writeProductLog(cartonSNList, line, editor, "85U", 1, now);

                unBindDNOnProduct(cartonSNList);
                deleteCartonSSCC(cartonSNList);
                SqlTransactionManager.Commit();
            }
            catch (Exception e)
            {
                SqlTransactionManager.Rollback();
                throw e;
            }
            finally
            {
                SqlTransactionManager.Dispose();
                SqlTransactionManager.End();
            }
        }

        public void UpackDNByDeliveryNo(string deliveryNo, string line, string editor, string station, string customer)
        {
            try
            {   
                SqlTransactionManager.Begin();
                DateTime now = DateTime.Now;

                BackupUnpackProduct(deliveryNo, editor, now);
                BackupUnpackProductStatus(deliveryNo, editor, now);

                updateDeliveryStatus(deliveryNo, editor, "00", now);

                updateProductStatus(deliveryNo, line, editor, "83", 1, now);
                writeProductLog(deliveryNo, line, editor, "85U", 1, now);

                unBindDNOnProduct(deliveryNo);
                deleteCartonSSCC(deliveryNo);
                SqlTransactionManager.Commit();
            }
            catch (Exception e)
            {
                SqlTransactionManager.Rollback();
                throw e;
            }
            finally
            {
                SqlTransactionManager.Dispose();
                SqlTransactionManager.End();
            }
        }

        public void UpackMultiPallet(IList<string> palletIdList, string line, string editor, string station, string customer)
        {
            try
            {
                SqlTransactionManager.Begin();
                DateTime now = DateTime.Now;
               
                BackupUnpackProduct(palletIdList, editor, now, false);
                BackupUnpackProductStatus(palletIdList, editor, now, false);

                // Delivery Status  '88' -> '82'   
                updateDeliveryStatusInUpackPallet(palletIdList, editor, now);
                updateDeliveryPalletStatus(palletIdList, editor, "0", now);

                updateProductStatus(palletIdList, line, editor, "85", 1, now, false);
                writeProductLog(palletIdList, line, editor, "87U", 1, now, false);

                writePalletLog(palletIdList, line, editor, "87U", now);
                updatePalletStation(palletIdList, editor, "87", now);
                

                clearPalletOnTestDataBoxLog(palletIdList);

                unBindPalletOnProduct(palletIdList);
                deletePalletId(palletIdList);
                SqlTransactionManager.Commit();
            }
            catch (Exception e)
            {
                SqlTransactionManager.Rollback();
                throw e;
            }
            finally
            {
                SqlTransactionManager.Dispose();
                SqlTransactionManager.End();
            }
        }

        public void UpackMultiPallet(string deliveryNo, string line, string editor, string station, string customer)
        {
            try
            {
                SqlTransactionManager.Begin();
                DateTime now = DateTime.Now;
                BackupUnpackProduct(deliveryNo, editor, now);
                BackupUnpackProductStatus(deliveryNo, editor, now);

                // Delivery Status '88' -> '82'   other status don't change
                updateDeliveryStatusInUpackPallet(deliveryNo, editor, now);
                updateDeliveryPalletStatus(deliveryNo, editor, "0", now);
               
                updateProductStatus(deliveryNo, line, editor, "85", 1, now);
                writeProductLog(deliveryNo, line, editor, "87U", 1, now);

                writePalletLog(deliveryNo, line, editor, "87U", now);
                updatePalletStation(deliveryNo, editor, "87", now);

                clearPalletOnTestDataBoxLog(deliveryNo);

                unBindPalletOnProduct(deliveryNo);
                deletePalletId(deliveryNo);
                SqlTransactionManager.Commit();
            }
            catch (Exception e)
            {
                SqlTransactionManager.Rollback();
                throw e;
            }
            finally
            {
                SqlTransactionManager.Dispose();
                SqlTransactionManager.End();
            }
        }

        #endregion


        #region  private function for unpack multi-carton/DN/Pallet
        void BackupUnpackProduct(IList<string> cartonSNList, string editor, DateTime now)
        {
            string strSQL = @" insert UnpackProduct (ProductID, Model, PCBID, PCBModel, MAC, UUID, MBECR, 
	                                                       CVSN, CUSTSN, ECR, BIOS, IMGVER, WMAC, IMEI, MEID, ICCID, 
	                                                       COAID, PizzaID, MO, UnitWeight, CartonSN, CartonWeight, 
	                                                       DeliveryNo, PalletNo, HDVD, BLMAC, TVTuner,
	                                                       UEditor, UPdt) 
                                            select ProductID, Model, PCBID, PCBModel, MAC, UUID, MBECR, 
	                                                       CVSN, CUSTSN, ECR, BIOS, IMGVER, WMAC, IMEI, MEID, ICCID, 
	                                                       COAID, PizzaID, MO, UnitWeight, CartonSN, CartonWeight, 
	                                                       DeliveryNo, PalletNo, HDVD, BLMAC, TVTuner,
	                                                       @editor as UEditor, @now  as UPdt  
                                            from Product
                                            where CartonSN in ('{0}')";
            string inSection = string.Join("','", cartonSNList.ToArray());
            strSQL = string.Format(strSQL, inSection);

            SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString_GetData,
                                                           CommandType.Text,
                                                           strSQL);
            SqlParameter paraEditor = new SqlParameter("@editor", SqlDbType.VarChar, 32);
            paraEditor.Direction = ParameterDirection.Input;
            paraEditor.Value = editor;

            SqlParameter paraNow = new SqlParameter("@now", SqlDbType.DateTime);
            paraNow.Direction = ParameterDirection.Input;
            paraNow.Value = now;

            SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString_GetData,
                                                           CommandType.Text,
                                                           strSQL,
                                                           paraEditor,
                                                           paraNow);
        }


        void BackupUnpackProduct(IList<string> idList, string editor, DateTime now, bool isCarton)
        {
            if (isCarton)
            {
                BackupUnpackProduct(idList, editor, now);
                return;
            }

            string strSQL = @" insert UnpackProduct (ProductID, Model, PCBID, PCBModel, MAC, UUID, MBECR, 
	                                                       CVSN, CUSTSN, ECR, BIOS, IMGVER, WMAC, IMEI, MEID, ICCID, 
	                                                       COAID, PizzaID, MO, UnitWeight, CartonSN, CartonWeight, 
	                                                       DeliveryNo, PalletNo, HDVD, BLMAC, TVTuner,
	                                                       UEditor, UPdt) 
                                            select ProductID, Model, PCBID, PCBModel, MAC, UUID, MBECR, 
	                                                       CVSN, CUSTSN, ECR, BIOS, IMGVER, WMAC, IMEI, MEID, ICCID, 
	                                                       COAID, PizzaID, MO, UnitWeight, CartonSN, CartonWeight, 
	                                                       DeliveryNo, PalletNo, HDVD, BLMAC, TVTuner,
	                                                       @editor as UEditor, @now  as UPdt  
                                            from Product
                                            where  PalletNo in (select PalletNo
                                                                                from PalletId 
                                                                               where PalletId in ('{0}') )";
            string inSection = string.Join("','", idList.ToArray());
            strSQL = string.Format(strSQL, inSection);

            SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString_GetData,
                                                           CommandType.Text,
                                                           strSQL);
            SqlParameter paraEditor = new SqlParameter("@editor", SqlDbType.VarChar, 32);
            paraEditor.Direction = ParameterDirection.Input;
            paraEditor.Value = editor;

            SqlParameter paraNow = new SqlParameter("@now", SqlDbType.DateTime);
            paraNow.Direction = ParameterDirection.Input;
            paraNow.Value = now;

            SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString_GetData,
                                                           CommandType.Text,
                                                           strSQL,
                                                           paraEditor,
                                                           paraNow);
        }

        void BackupUnpackProduct(string deliveryNo, string editor, DateTime now)
        {
            string strSQL = @" insert UnpackProduct (ProductID, Model, PCBID, PCBModel, MAC, UUID, MBECR, 
	                                                       CVSN, CUSTSN, ECR, BIOS, IMGVER, WMAC, IMEI, MEID, ICCID, 
	                                                       COAID, PizzaID, MO, UnitWeight, CartonSN, CartonWeight, 
	                                                       DeliveryNo, PalletNo, HDVD, BLMAC, TVTuner,
	                                                       UEditor, UPdt) 
                                            select ProductID, Model, PCBID, PCBModel, MAC, UUID, MBECR, 
	                                                       CVSN, CUSTSN, ECR, BIOS, IMGVER, WMAC, IMEI, MEID, ICCID, 
	                                                       COAID, PizzaID, MO, UnitWeight, CartonSN, CartonWeight, 
	                                                       DeliveryNo, PalletNo, HDVD, BLMAC, TVTuner,
	                                                       @editor as UEditor, @now  as UPdt  
                                            from Product
                                            where DeliveryNo = @DN ";
             SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString_GetData,
                                                           CommandType.Text,
                                                           strSQL);
            SqlParameter paraEditor = new SqlParameter("@editor", SqlDbType.VarChar, 32);
            paraEditor.Direction = ParameterDirection.Input;
            paraEditor.Value = editor;

            SqlParameter paraNow = new SqlParameter("@now", SqlDbType.DateTime);
            paraNow.Direction = ParameterDirection.Input;
            paraNow.Value = now;

            SqlParameter paraDN = new SqlParameter("@DN", SqlDbType.DateTime);
            paraDN.Direction = ParameterDirection.Input;
            paraDN.Value = deliveryNo;


            SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString_GetData,
                                                           CommandType.Text,
                                                           strSQL,
                                                           paraEditor,
                                                           paraNow,
                                                           paraDN);
        }


        void BackupUnpackProductStatus(IList<string> cartonSNList, string editor, DateTime now)
        {

            string strSQL = @"Insert UnpackProductStatus(ProductID, Station, Status, ReworkCode, Line, 
                                                       Editor, Cdt, Udt, UEditor, UPdt)
                                            select a.ProductID, b.Station, b.Status, b.ReworkCode, b.Line, 
                                                       b.Editor, b.Cdt, b.Udt, @editor as UEditor, @now as UPdt
                                             from Product a, ProductStatus b
                                             where a.ProductID = b.ProductID and 
                                                       a.CartonSN in ('{0}') ";
            string inSection = string.Join("','", cartonSNList.ToArray());
            strSQL = string.Format(strSQL, inSection);

            SqlParameter paraEditor = new SqlParameter("@editor", SqlDbType.VarChar, 32);
            paraEditor.Direction = ParameterDirection.Input;
            paraEditor.Value = editor;

            SqlParameter paraNow = new SqlParameter("@now", SqlDbType.DateTime);
            paraNow.Direction = ParameterDirection.Input;
            paraNow.Value = now;

            SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString_GetData,
                                                           CommandType.Text,
                                                           strSQL,
                                                           paraEditor,
                                                           paraNow);
        }

        void BackupUnpackProductStatus(IList<string> idList, string editor, DateTime now, bool isCarton)
        {
            if (isCarton)
            {
                BackupUnpackProductStatus(idList, editor, now);
                return;
            }
            string strSQL = @"Insert UnpackProductStatus(ProductID, Station, Status, ReworkCode, Line, 
                                                       Editor, Cdt, Udt, UEditor, UPdt)
                                            select a.ProductID, b.Station, b.Status, b.ReworkCode, b.Line, 
                                                       b.Editor, b.Cdt, b.Udt, @editor as UEditor, @now as UPdt
                                             from Product a, ProductStatus b
                                             where a.ProductID = b.ProductID and 
                                                       a.PalletNo in (select distinct PalletNo
                                                                                from PalletId 
                                                                               where PalletId in ('{0}') ) ";
            string inSection = string.Join("','", idList.ToArray());
            strSQL = string.Format(strSQL, inSection);

            SqlParameter paraEditor = new SqlParameter("@editor", SqlDbType.VarChar, 32);
            paraEditor.Direction = ParameterDirection.Input;
            paraEditor.Value = editor;

            SqlParameter paraNow = new SqlParameter("@now", SqlDbType.DateTime);
            paraNow.Direction = ParameterDirection.Input;
            paraNow.Value = now;

            SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString_GetData,
                                                           CommandType.Text,
                                                           strSQL,
                                                           paraEditor,
                                                           paraNow);
        }

        void BackupUnpackProductStatus( string deliveryNo, string editor, DateTime now)
        {
            string strSQL = @"Insert UnpackProductStatus(ProductID, Station, Status, ReworkCode, Line, 
                                                       Editor, Cdt, Udt, UEditor, UPdt)
                                            select a.ProductID, b.Station, b.Status, b.ReworkCode, b.Line, 
                                                       b.Editor, b.Cdt, b.Udt, @editor as UEditor, @now as UPdt
                                             from Product a, ProductStatus b
                                             where a.ProductID = b.ProductID and 
                                                       a.DeliveryNo =@DN) ";
          
            SqlParameter paraEditor = new SqlParameter("@editor", SqlDbType.VarChar, 32);
            paraEditor.Direction = ParameterDirection.Input;
            paraEditor.Value = editor;

            SqlParameter paraNow = new SqlParameter("@now", SqlDbType.DateTime);
            paraNow.Direction = ParameterDirection.Input;
            paraNow.Value = now;

            SqlParameter paraDN = new SqlParameter("@DN", SqlDbType.DateTime);
            paraDN.Direction = ParameterDirection.Input;
            paraDN.Value = deliveryNo;


            SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString_GetData,
                                                           CommandType.Text,
                                                           strSQL,
                                                           paraEditor,
                                                           paraNow,
                                                           paraDN);
        }

        void updateProductStatus(IList<string> cartonSNList, string line, string editor, string station, int status, DateTime udt)
        {
            string strSQL = @"update ProductStatus
                                             set Station=@station,
                                                 Status = @status,
                                                 Line = @line
                                                 TestFailCount=0,
                                                 Editor =@editor,
                                                 Udt = @now
                                            where ProductID in (select ProductID from Product where CartonSN in ('{0}') ) ";

            string inSection = string.Join("','", cartonSNList.ToArray());

            strSQL = string.Format(strSQL, inSection);

            SqlParameter paraStation = new SqlParameter("@station", SqlDbType.VarChar, 32);
            paraStation.Direction = ParameterDirection.Input;
            paraStation.Value = station;
            SqlParameter paraStatus = new SqlParameter("@status", SqlDbType.Int);
            paraStatus.Direction = ParameterDirection.Input;
            paraStatus.Value = status;
            SqlParameter paraEditor = new SqlParameter("@editor", SqlDbType.VarChar, 32);
            paraEditor.Direction = ParameterDirection.Input;
            paraEditor.Value = editor;

            SqlParameter paraLine = new SqlParameter("@line", SqlDbType.VarChar, 32);
            paraLine.Direction = ParameterDirection.Input;
            paraLine.Value = line;

            SqlParameter paraNow = new SqlParameter("@now", SqlDbType.DateTime);
            paraNow.Direction = ParameterDirection.Input;
            paraNow.Value = udt;

            SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString_GetData,
                                                           CommandType.Text,
                                                           strSQL,
                                                           paraStation,
                                                           paraStatus,
                                                           paraLine,
                                                           paraEditor,
                                                           paraNow);
        }

        void updateProductStatus(IList<string> idList, string line, string editor, string station, int status, DateTime udt, bool isCarton)
        {
            if (isCarton)
            {
                updateProductStatus(idList, line, editor, station, status, udt);
                return;
            }

            string strSQL = @"update ProductStatus
                                             set Station=@station,
                                                 Status = @status,
                                                 Line = @line
                                                 TestFailCount=0,
                                                 Editor =@editor,
                                                 Udt = @now
                                            where ProductID in (select ProductID 
                                                                               from Product 
                                                                              where  PalletNo in (select distinct PalletNo
                                                                                                                from PalletId 
                                                                                                               where PalletId in ('{0}'))) ";

            string inSection = string.Join("','", idList.ToArray());

            strSQL = string.Format(strSQL, inSection);

            SqlParameter paraStation = new SqlParameter("@station", SqlDbType.VarChar, 32);
            paraStation.Direction = ParameterDirection.Input;
            paraStation.Value = station;
            SqlParameter paraStatus = new SqlParameter("@status", SqlDbType.Int);
            paraStatus.Direction = ParameterDirection.Input;
            paraStatus.Value = status;
            SqlParameter paraEditor = new SqlParameter("@editor", SqlDbType.VarChar, 32);
            paraEditor.Direction = ParameterDirection.Input;
            paraEditor.Value = editor;

            SqlParameter paraLine = new SqlParameter("@line", SqlDbType.VarChar, 32);
            paraLine.Direction = ParameterDirection.Input;
            paraLine.Value = line;

            SqlParameter paraNow = new SqlParameter("@now", SqlDbType.DateTime);
            paraNow.Direction = ParameterDirection.Input;
            paraNow.Value = udt;

            SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString_GetData,
                                                           CommandType.Text,
                                                           strSQL,
                                                           paraStation,
                                                           paraStatus,
                                                           paraLine,
                                                           paraEditor,
                                                           paraNow);
        }

        void updateProductStatus(string deliveryNo, string line, string editor, string station, int status, DateTime udt)
        {
            string strSQL = @"update ProductStatus
                                             set Station=@station,
                                                 Status = @status,
                                                 Line = @line
                                                 TestFailCount=0,
                                                 Editor =@editor,
                                                 Udt = @now
                                            where ProductID in (select ProductID from Product where DeliveryNo=@DN ) ";

            SqlParameter paraStation = new SqlParameter("@station", SqlDbType.VarChar, 32);
            paraStation.Direction = ParameterDirection.Input;
            paraStation.Value = station;
            SqlParameter paraStatus = new SqlParameter("@status", SqlDbType.Int);
            paraStatus.Direction = ParameterDirection.Input;
            paraStatus.Value = status;
            SqlParameter paraEditor = new SqlParameter("@editor", SqlDbType.VarChar, 32);
            paraEditor.Direction = ParameterDirection.Input;
            paraEditor.Value = editor;

            SqlParameter paraLine = new SqlParameter("@line", SqlDbType.VarChar, 32);
            paraLine.Direction = ParameterDirection.Input;
            paraLine.Value = line;

            SqlParameter paraNow = new SqlParameter("@now", SqlDbType.DateTime);
            paraNow.Direction = ParameterDirection.Input;
            paraNow.Value = udt;

            SqlParameter paraDN = new SqlParameter("@DN", SqlDbType.DateTime);
            paraDN.Direction = ParameterDirection.Input;
            paraDN.Value = deliveryNo;

            SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString_GetData,
                                                           CommandType.Text,
                                                           strSQL,
                                                           paraStation,
                                                           paraStatus,
                                                           paraLine,
                                                           paraEditor,
                                                           paraNow,
                                                           paraDN);
        }

        void writeProductLog(IList<string> cartonSNList, string line, string editor, string station, int status, DateTime now)
        {
            string strSQL = @"insert ProductLog (ProductID, Model, Station, Status, Line, Editor, Cdt)
                                           select ProductID, Model, @station,@status,@line,@editor,@now 
                                           from Product 
                                           where CartonSN in ('{0}')";

            string inSection = string.Join("','", cartonSNList.ToArray());

            strSQL = string.Format(strSQL, inSection);

            SqlParameter paraStation = new SqlParameter("@station", SqlDbType.VarChar, 32);
            paraStation.Direction = ParameterDirection.Input;
            paraStation.Value = station;
            SqlParameter paraStatus = new SqlParameter("@status", SqlDbType.Int);
            paraStatus.Direction = ParameterDirection.Input;
            paraStatus.Value = status;
            SqlParameter paraEditor = new SqlParameter("@editor", SqlDbType.VarChar, 32);
            paraEditor.Direction = ParameterDirection.Input;
            paraEditor.Value = editor;

            SqlParameter paraLine = new SqlParameter("@line", SqlDbType.VarChar, 32);
            paraLine.Direction = ParameterDirection.Input;
            paraLine.Value = line;

            SqlParameter paraNow = new SqlParameter("@now", SqlDbType.DateTime);
            paraNow.Direction = ParameterDirection.Input;
            paraNow.Value = now;

            SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString_GetData,
                                                           CommandType.Text,
                                                           strSQL,
                                                           paraStation,
                                                           paraStatus,
                                                           paraLine,
                                                           paraEditor,
                                                           paraNow);

        }

        void writeProductLog(IList<string> idList, string line, string editor, string station, int status, DateTime now, bool isCarton)
        {
            if (isCarton)
            {
                writeProductLog(idList, line, editor, station, status, now);
                return;
            }
            string strSQL = @"insert ProductLog (ProductID, Model, Station, Status, Line, Editor, Cdt)
                                           select ProductID, Model, @station,@status,@line,@editor,@now 
                                           from Product 
                                           where  PalletNo in (select PalletNo
                                                                             from PalletId 
                                                                           where PalletId in ('{0}'))";

            string inSection = string.Join("','", idList.ToArray());

            strSQL = string.Format(strSQL, inSection);

            SqlParameter paraStation = new SqlParameter("@station", SqlDbType.VarChar, 32);
            paraStation.Direction = ParameterDirection.Input;
            paraStation.Value = station;
            SqlParameter paraStatus = new SqlParameter("@status", SqlDbType.Int);
            paraStatus.Direction = ParameterDirection.Input;
            paraStatus.Value = status;
            SqlParameter paraEditor = new SqlParameter("@editor", SqlDbType.VarChar, 32);
            paraEditor.Direction = ParameterDirection.Input;
            paraEditor.Value = editor;

            SqlParameter paraLine = new SqlParameter("@line", SqlDbType.VarChar, 32);
            paraLine.Direction = ParameterDirection.Input;
            paraLine.Value = line;

            SqlParameter paraNow = new SqlParameter("@now", SqlDbType.DateTime);
            paraNow.Direction = ParameterDirection.Input;
            paraNow.Value = now;

            SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString_GetData,
                                                           CommandType.Text,
                                                           strSQL,
                                                           paraStation,
                                                           paraStatus,
                                                           paraLine,
                                                           paraEditor,
                                                           paraNow);

        }

        void writeProductLog(string deliveryNo, string line, string editor, string station, int status, DateTime now)
        {
            string strSQL = @"insert ProductLog (ProductID, Model, Station, Status, Line, Editor, Cdt)
                                           select ProductID, Model, @station,@status,@line,@editor,@now 
                                           from Product 
                                           where DeliveryNo =@DN";

           

            SqlParameter paraStation = new SqlParameter("@station", SqlDbType.VarChar, 32);
            paraStation.Direction = ParameterDirection.Input;
            paraStation.Value = station;
            SqlParameter paraStatus = new SqlParameter("@status", SqlDbType.Int);
            paraStatus.Direction = ParameterDirection.Input;
            paraStatus.Value = status;
            SqlParameter paraEditor = new SqlParameter("@editor", SqlDbType.VarChar, 32);
            paraEditor.Direction = ParameterDirection.Input;
            paraEditor.Value = editor;

            SqlParameter paraLine = new SqlParameter("@line", SqlDbType.VarChar, 32);
            paraLine.Direction = ParameterDirection.Input;
            paraLine.Value = line;

            SqlParameter paraNow = new SqlParameter("@now", SqlDbType.DateTime);
            paraNow.Direction = ParameterDirection.Input;
            paraNow.Value = now;

            SqlParameter paraDN = new SqlParameter("@DN", SqlDbType.DateTime);
            paraDN.Direction = ParameterDirection.Input;
            paraDN.Value = deliveryNo;

            SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString_GetData,
                                                           CommandType.Text,
                                                           strSQL,
                                                           paraStation,
                                                           paraStatus,
                                                           paraLine,
                                                           paraEditor,
                                                           paraNow,
                                                           paraDN);

        }
        
       


        void updateDeliveryStatus(IList<string> cartonSNList, string editor, string status, DateTime now)
        {
            string strSQL = @"update Delivery
                                            set Status=@status,
                                                  Editor=@editor,
                                                  Udt =@now
                                            where DeliveryNo in ( select distinct DeliveryNo 
                                                                                 from Product 
                                                                                 where CartonSN in ('{0}'))  ";

            string inSection = string.Join("','", cartonSNList.ToArray());

            strSQL = string.Format(strSQL, inSection);


            SqlParameter paraStatus = new SqlParameter("@status", SqlDbType.VarChar, 8);
            paraStatus.Direction = ParameterDirection.Input;
            paraStatus.Value = status;

            SqlParameter paraEditor = new SqlParameter("@editor", SqlDbType.VarChar, 32);
            paraEditor.Direction = ParameterDirection.Input;
            paraEditor.Value = editor;

            SqlParameter paraNow = new SqlParameter("@now", SqlDbType.DateTime);
            paraNow.Direction = ParameterDirection.Input;
            paraNow.Value = now;

            SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString_GetData,
                                                           CommandType.Text,
                                                           strSQL,
                                                           paraStatus,
                                                           paraEditor,
                                                           paraNow);

        }

        void updateDeliveryStatus(string deliveryNo, string editor, string status, DateTime now)
        {
            string strSQL = @"update Delivery
                                            set Status=@status,
                                                  Editor=@editor,
                                                  Udt =@now
                                            where DeliveryNo =@DN ";


            SqlParameter paraStatus = new SqlParameter("@status", SqlDbType.VarChar, 8);
            paraStatus.Direction = ParameterDirection.Input;
            paraStatus.Value = status;

            SqlParameter paraEditor = new SqlParameter("@editor", SqlDbType.VarChar, 32);
            paraEditor.Direction = ParameterDirection.Input;
            paraEditor.Value = editor;

            SqlParameter paraNow = new SqlParameter("@now", SqlDbType.DateTime);
            paraNow.Direction = ParameterDirection.Input;
            paraNow.Value = now;

            SqlParameter paraDN = new SqlParameter("@DN", SqlDbType.DateTime);
            paraDN.Direction = ParameterDirection.Input;
            paraDN.Value = deliveryNo;

            SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString_GetData,
                                                           CommandType.Text,
                                                           strSQL,
                                                           paraStatus,
                                                           paraEditor,
                                                           paraNow,
                                                           paraDN);

        }

        void updateDeliveryStatusInUpackPallet(IList<string> palletIdList, string editor, DateTime now)
        {
            string strSQL = @"update Delivery
                                            set Status= (case Status when '88' then '82' else Status end),
                                                  Editor=@editor,
                                                  Udt =@now  
                                            where DeliveryNo in ( select distinct a.DeliveryNo
					                                                             from Delivery_Pallet a, PalletId b
                                                                                where a.PalletNo = b.PalletNo and
                                                                                            b.PalletId in ('{0}')) ";

            string inSection = string.Join("','", palletIdList.ToArray());

            strSQL = string.Format(strSQL, inSection);           

            SqlParameter paraEditor = new SqlParameter("@editor", SqlDbType.VarChar, 32);
            paraEditor.Direction = ParameterDirection.Input;
            paraEditor.Value = editor;

            SqlParameter paraNow = new SqlParameter("@now", SqlDbType.DateTime);
            paraNow.Direction = ParameterDirection.Input;
            paraNow.Value = now;

            SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString_GetData,
                                                           CommandType.Text,
                                                           strSQL,                                                          
                                                           paraEditor,
                                                           paraNow);

        }

        void updateDeliveryStatusInUpackPallet(string deliveryNo, string editor, DateTime now)
        {
            string strSQL = @"update Delivery
                                            set Status= (case Status when '88' then '82' else Status end),
                                                  Editor=@editor,
                                                  Udt =@now  
                                            where DeliveryNo=@DN ";

            SqlParameter paraEditor = new SqlParameter("@editor", SqlDbType.VarChar, 32);
            paraEditor.Direction = ParameterDirection.Input;
            paraEditor.Value = editor;

            SqlParameter paraNow = new SqlParameter("@now", SqlDbType.DateTime);
            paraNow.Direction = ParameterDirection.Input;
            paraNow.Value = now;

            SqlParameter paraDN = new SqlParameter("@DN", SqlDbType.VarChar, 32);
            paraDN.Direction = ParameterDirection.Input;
            paraDN.Value = deliveryNo;

            SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString_GetData,
                                                           CommandType.Text,
                                                           strSQL,
                                                           paraEditor,
                                                           paraNow,
                                                           paraDN);

        }

        void updateDeliveryPalletStatus(IList<string> palletIdList, string editor, string status, DateTime now)
        {
            string strSQL = @"update Delivery_Pallet
                                                set Status=@status,
                                                      Editor =@editor,
                                                      Udt =@now
                                                where PalletNo = (select distinct PalletNo
                                                                                from PalletId
                                                                                where PalletId in ('{0}'))  ";

            string inSection = string.Join("','", palletIdList.ToArray());

            strSQL = string.Format(strSQL, inSection);


            SqlParameter paraStatus = new SqlParameter("@status", SqlDbType.VarChar, 8);
            paraStatus.Direction = ParameterDirection.Input;
            paraStatus.Value = status;

            SqlParameter paraEditor = new SqlParameter("@editor", SqlDbType.VarChar, 32);
            paraEditor.Direction = ParameterDirection.Input;
            paraEditor.Value = editor;

            SqlParameter paraNow = new SqlParameter("@now", SqlDbType.DateTime);
            paraNow.Direction = ParameterDirection.Input;
            paraNow.Value = now;

            SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString_GetData,
                                                           CommandType.Text,
                                                           strSQL,
                                                           paraStatus,
                                                           paraEditor,
                                                           paraNow);

        }

        void updateDeliveryPalletStatus(string deliveryNo, string editor, string status, DateTime now)
        {
            string strSQL = @"update Delivery_Pallet
                                                set Status=@status,
                                                      Editor =@editor,
                                                      Udt =@now
                                                where PalletNo = (select PalletNo
                                                                                from Delivery_Pallet 
                                                                               where DeliveryNo = @DN  ";

            SqlParameter paraStatus = new SqlParameter("@status", SqlDbType.VarChar, 8);
            paraStatus.Direction = ParameterDirection.Input;
            paraStatus.Value = status;

            SqlParameter paraEditor = new SqlParameter("@editor", SqlDbType.VarChar, 32);
            paraEditor.Direction = ParameterDirection.Input;
            paraEditor.Value = editor;

            SqlParameter paraNow = new SqlParameter("@now", SqlDbType.DateTime);
            paraNow.Direction = ParameterDirection.Input;
            paraNow.Value = now;

            SqlParameter paraDN = new SqlParameter("@DN", SqlDbType.DateTime);
            paraDN.Direction = ParameterDirection.Input;
            paraDN.Value = deliveryNo;

            SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString_GetData,
                                                           CommandType.Text,
                                                           strSQL,
                                                           paraStatus,
                                                           paraEditor,
                                                           paraNow,
                                                           paraDN);
        }

        void updatePalletStation(IList<string> palletIdList, string editor, string station, DateTime now)
        {
            string strSQL = @"update Pallet
                                                set Station=@station,
                                                      Editor =@editor,
                                                      Udt =@now
                                                where PalletNo = (select distinct PalletNo
                                                                                from PalletId
                                                                                where PalletId in ('{0}')) 
                                                   and  Station='89'";

            string inSection = string.Join("','", palletIdList.ToArray());

            strSQL = string.Format(strSQL, inSection);


            SqlParameter paraStatus = new SqlParameter("@station", SqlDbType.VarChar, 8);
            paraStatus.Direction = ParameterDirection.Input;
            paraStatus.Value = station;

            SqlParameter paraEditor = new SqlParameter("@editor", SqlDbType.VarChar, 32);
            paraEditor.Direction = ParameterDirection.Input;
            paraEditor.Value = editor;

            SqlParameter paraNow = new SqlParameter("@now", SqlDbType.DateTime);
            paraNow.Direction = ParameterDirection.Input;
            paraNow.Value = now;

            SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString_GetData,
                                                           CommandType.Text,
                                                           strSQL,
                                                           paraStatus,
                                                           paraEditor,
                                                           paraNow);

        }

        void updatePalletStation(string deliveryNo, string editor, string station, DateTime now)
        {
            string strSQL = @"update Pallet
                                                set Station=@station,
                                                      Editor =@editor,
                                                      Udt =@now
                                                where PalletNo = (select distinct PalletNo
                                                                                from Delivery_Pallet
                                                                                where DeliveryNo=@DN) 
                                                   and  Station='89'";
           

            SqlParameter paraStatus = new SqlParameter("@station", SqlDbType.VarChar, 8);
            paraStatus.Direction = ParameterDirection.Input;
            paraStatus.Value = station;

            SqlParameter paraEditor = new SqlParameter("@editor", SqlDbType.VarChar, 32);
            paraEditor.Direction = ParameterDirection.Input;
            paraEditor.Value = editor;

            SqlParameter paraNow = new SqlParameter("@now", SqlDbType.DateTime);
            paraNow.Direction = ParameterDirection.Input;
            paraNow.Value = now;

            SqlParameter paraDN = new SqlParameter("@DN", SqlDbType.VarChar, 32);
            paraDN.Direction = ParameterDirection.Input;
            paraDN.Value = deliveryNo;

            SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString_GetData,
                                                           CommandType.Text,
                                                           strSQL,
                                                           paraStatus,
                                                           paraEditor,
                                                           paraNow,
                                                           paraDN);
        }

        void writePalletLog(IList<string> palletIdList, string line, string editor, string station, DateTime now)
        {
            string strSQL = @"insert PalletLog (PalletNo, Station, Line, Editor, Cdt)
                                           select PalletNo, @station,@line,@editor,@now 
                                           from Pallet 
                                          where PalletNo = (select distinct PalletNo
                                                                                from PalletId
                                                                                where PalletId in ('{0}')) 
                                                   and  Station='89'";

            string inSection = string.Join("','", palletIdList.ToArray());

            strSQL = string.Format(strSQL, inSection);

            SqlParameter paraStation = new SqlParameter("@station", SqlDbType.VarChar, 32);
            paraStation.Direction = ParameterDirection.Input;
            paraStation.Value = station;
          
            SqlParameter paraEditor = new SqlParameter("@editor", SqlDbType.VarChar, 32);
            paraEditor.Direction = ParameterDirection.Input;
            paraEditor.Value = editor;

            SqlParameter paraLine = new SqlParameter("@line", SqlDbType.VarChar, 32);
            paraLine.Direction = ParameterDirection.Input;
            paraLine.Value = line;

            SqlParameter paraNow = new SqlParameter("@now", SqlDbType.DateTime);
            paraNow.Direction = ParameterDirection.Input;
            paraNow.Value = now;

            SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString_GetData,
                                                           CommandType.Text,
                                                           strSQL,
                                                           paraStation,
                                                           paraLine,
                                                           paraEditor,
                                                           paraNow);

        }

        void writePalletLog(string deliveryNo, string line, string editor, string station, DateTime now)
        {
            string strSQL = @"insert PalletLog (PalletNo, Station, Line, Editor, Cdt)
                                           select PalletNo, @station,@line,@editor,@now 
                                           from Pallet 
                                          where PalletNo = ((select distinct PalletNo
                                                                                from Delivery_Pallet
                                                                                where DeliveryNo=@DN) 
                                                   and  Station='89'";
           
            SqlParameter paraStation = new SqlParameter("@station", SqlDbType.VarChar, 32);
            paraStation.Direction = ParameterDirection.Input;
            paraStation.Value = station;

            SqlParameter paraEditor = new SqlParameter("@editor", SqlDbType.VarChar, 32);
            paraEditor.Direction = ParameterDirection.Input;
            paraEditor.Value = editor;

            SqlParameter paraLine = new SqlParameter("@line", SqlDbType.VarChar, 32);
            paraLine.Direction = ParameterDirection.Input;
            paraLine.Value = line;

            SqlParameter paraNow = new SqlParameter("@now", SqlDbType.DateTime);
            paraNow.Direction = ParameterDirection.Input;
            paraNow.Value = now;

            SqlParameter paraDN = new SqlParameter("@DN", SqlDbType.VarChar, 32);
            paraDN.Direction = ParameterDirection.Input;
            paraDN.Value = deliveryNo;

            SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString_GetData,
                                                           CommandType.Text,
                                                           strSQL,
                                                           paraStation,
                                                           paraLine,
                                                           paraEditor,
                                                           paraNow,
                                                           paraDN);

        }


        void unBindCartonOnProduct(IList<string> cartonSNList)
        {
            string strSQL = @"update Product
                                             set CartonSN=''                                                
                                            where  CartonSN in ('{0}') ";

            string inSection = string.Join("','", cartonSNList.ToArray());

            strSQL = string.Format(strSQL, inSection);
            SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString_GetData,
                                                           CommandType.Text,
                                                           strSQL);

        }

        void unBindDNOnProduct(IList<string> cartonSNList)
        {
            string strSQL = @"update Product
                                             set DeliveryNo=''                                                
                                            where  CartonSN in ('{0}') ";

            string inSection = string.Join("','", cartonSNList.ToArray());

            strSQL = string.Format(strSQL, inSection);
            SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString_GetData,
                                                           CommandType.Text,
                                                           strSQL);

        }

        void unBindDNOnProduct(string deliveryNo)
        {
            string strSQL = @"update Product
                                             set DeliveryNo=''                                                
                                            where  DeliveryNo=@DN) ";

            SqlParameter paraDN = new SqlParameter("@DN", SqlDbType.DateTime);
            paraDN.Direction = ParameterDirection.Input;
            paraDN.Value = deliveryNo;

            SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString_GetData,
                                                           CommandType.Text,
                                                           strSQL,
                                                           paraDN);

        }

        void unBindPalletOnProduct(IList<string> palletIdList)
        {
            string strSQL = @"update Product
                                             set PalletNo=''                                                
                                            where  PalletNo in (select distinct PalletNo
                                                                                from PalletId 
                                                                               where PalletId in ('{0}') )  ";

            string inSection = string.Join("','", palletIdList.ToArray());

            strSQL = string.Format(strSQL, inSection);
            SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString_GetData,
                                                           CommandType.Text,
                                                           strSQL);

        }

        void unBindPalletOnProduct(string deliveryNo)
        {
            string strSQL = @"update Product
                                             set PalletNo=''                                                
                                            where  DeliveryNo = @DN  ";

            SqlParameter paraDN = new SqlParameter("@DN", SqlDbType.DateTime);
            paraDN.Direction = ParameterDirection.Input;
            paraDN.Value = deliveryNo;

            SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString_GetData,
                                                           CommandType.Text,
                                                           strSQL,
                                                           paraDN);
        }

        void clearCartonOnTestDataBoxLog(IList<string> cartonSNList)
        {
            string strSQL = @"update TestDataBoxLog
                                             set CartonSn=''                                                
                                            where  PCBNo in (select PCBID 
                                                                           from Product    
                                                                           where CartonSN in('{0}'))";


            string inSection = string.Join("','", cartonSNList.ToArray());

            strSQL = string.Format(strSQL, inSection);
            SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString_GetData,
                                                           CommandType.Text,
                                                           strSQL);

        }
        void clearPalletOnTestDataBoxLog(IList<string> palletIdList)
        {
            string strSQL = @"update TestDataBoxLog
                                             set PalletSerialNo=''                                                
                                            where  PCBNo in (select a.PCBID 
                                                                           from Product a, PalletId b   
                                                                           where a.PalletNo = b.PalletNo
                                                                               and b.PalletId in('{0}'))";


            string inSection = string.Join("','", palletIdList.ToArray());

            strSQL = string.Format(strSQL, inSection);
            SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString_GetData,
                                                           CommandType.Text,
                                                           strSQL);

        }
        void clearPalletOnTestDataBoxLog(string deliveryNo)
        {
            string strSQL = @"update TestDataBoxLog
                                             set PalletSerialNo=''                                                
                                            where  PCBNo in (select PCBID 
                                                                           from Product
                                                                           where DeliveryNo =@DN)";

            SqlParameter paraDN = new SqlParameter("@DN", SqlDbType.DateTime);
            paraDN.Direction = ParameterDirection.Input;
            paraDN.Value = deliveryNo;

            SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString_GetData,
                                                           CommandType.Text,
                                                           strSQL,
                                                           paraDN);
        }

        void deleteCartonSSCC(IList<string> cartonSNList)
        {
            string strSQL = @"delete from CartonSSCC where CartonSN in ('{0}') ";

            string inSection = string.Join("','", cartonSNList.ToArray());
            strSQL = string.Format(strSQL, inSection);
            SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString_GetData,
                                                           CommandType.Text,
                                                           strSQL);
        }

        void deleteCartonSSCC(string deliveryNo)
        {
            string strSQL = @"delete from CartonSSCC 
                                            where CartonSN in (select distinct CartonSN 
                                                                               from Product 
                                                                               where DeliveryNo=@DN) ";

            SqlParameter paraDN = new SqlParameter("@DN", SqlDbType.DateTime);
            paraDN.Direction = ParameterDirection.Input;
            paraDN.Value = deliveryNo;

            SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString_GetData,
                                                           CommandType.Text,
                                                           strSQL,
                                                           paraDN);
        }

        void deletePalletId(IList<string> palletIdList)
        {
            string strSQL = @"delete from PalletId where PalletId in ('{0}') ";

            string inSection = string.Join("','", palletIdList.ToArray());
            strSQL = string.Format(strSQL, inSection);
            SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString_GetData,
                                                           CommandType.Text,
                                                           strSQL);
        }

        void deletePalletId(string  deliveryNo)
        {
            string strSQL = @"delete from PalletId where PalletNo in (select PalletNo
                                                                                                         from Delivery_Pallet
                                                                                                         where DeliveryNo = @DN) ";

            SqlParameter paraDN = new SqlParameter("@DN", SqlDbType.DateTime);
            paraDN.Direction = ParameterDirection.Input;
            paraDN.Value = deliveryNo;

            SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString_GetData,
                                                           CommandType.Text,
                                                           strSQL,
                                                           paraDN);
        }
        

        #endregion


    }
}
