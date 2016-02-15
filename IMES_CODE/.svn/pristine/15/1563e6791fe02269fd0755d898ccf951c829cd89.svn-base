/*
 * INVENTEC corporation (c)2010 all rights reserved. 
 * Description: Unpack Carton/DN/Pallet Impl
 * Update: 
 * Date         Name                Reason 
 * ==========   =================   =====================================
 * 2011-03-10   Lucy Liu            Create
 * 2011-04-12   Lucy Liu            Modify:ITC-1268-0058
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
using carton = IMES.FisObject.PAK.CartonSSCC;

namespace IMES.Station.Implementation
{
    /// <summary>
    /// UnpackForRCTO
    /// </summary>
    public class UnpackCarton_CR : MarshalByRefObject, IUnpackCarton_CR
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
            carton.ICartonSSCCRepository cartRep = RepositoryFactory.GetInstance().GetRepository<carton.ICartonSSCCRepository, IMES.FisObject.PAK.CartonSSCC.CartonSSCC>();

         
            try
            {
                CartonStatusInfo condition = new CartonStatusInfo();
                condition.cartonNo = cartonNo;
                IList<CartonStatusInfo> CartonStatus = cartRep.GetCartonStatusInfo(condition);
                if (CartonStatus.Count == 0)
                {
                    //erpara.Add("Invalid Carton No!");
                    erpara.Add(cartonNo);
                    ex = new FisException("PAC001", erpara);
                    throw ex;
                }

                var currentProduct = CommonImpl.GetProductByInput(cartonNo, CommonImpl.InputTypeEnum.Carton);
                if (currentProduct == null)
                {
                    FisException fe = new FisException("CHK079", new string[] { cartonNo });
                    throw fe;
                }         

                string sessionKey = currentProduct.ProId;
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
                    RouteManagementUtils.GetWorkflow(station, "UnpackCarton_CR.xoml", "UnpackCarton_CR.rules", out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                   
                    currentSession.AddValue(Session.SessionKeys.Carton, cartonNo);
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
                    throw new FisException("CHK290", new string[] { deliveryNo });//DN已上传
                }
                if (currentDelivery.ModelName.Substring(0, 2) == "PC")
                {
                    throw new FisException("PAK166", new string[] { deliveryNo });//请使用整机的Unpack DN by DN!”
                }
                string sessionKey = currentDelivery.DeliveryNo;


                //station = "80";

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
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow("UnpackDNByDNForRCTO.xoml", "", wfArguments);

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

                Pallet CurrentPallet = iPalletRepository.Find(DummyPalletNo);
                if (CurrentPallet == null)
                {
                    erpara.Add(DummyPalletNo);
                    ex = new FisException("PAK049", erpara);    //此Dummy Pallet No 不存在！
                    throw ex;
                }
                PalletLogInfo condition = new PalletLogInfo();
                condition.palletNo = DummyPalletNo;
                condition.station = "9A";
               // condition = "Pass Pallet Verify_RCTO";
               
                IList<PalletLogInfo> lstPalletLog =  iPalletRepository.GetPalletLogInfoList( condition);

                if (lstPalletLog == null || lstPalletLog.Count <=0)
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
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow("UnpackPalletNoForRCTO.xoml", "", wfArguments);

                    currentSession.AddValue(Session.SessionKeys.DummyPalletNo,DummyPalletNo);
                    currentSession.AddValue(Session.SessionKeys.Pallet, CurrentPallet);
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
                logger.Debug("(UnpackDummyPalletNo)Unpack PalletNo end,  DummyPalletNo:" + DummyPalletNo + "line:" + line + "editor:" + editor  + "customer:" + customer);
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


    }
}
