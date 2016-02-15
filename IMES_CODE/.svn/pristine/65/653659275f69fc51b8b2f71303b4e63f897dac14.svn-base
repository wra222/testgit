using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;
using IMES.Station.Interface.CommonIntf;
using IMES.Station.Interface.BSamIntf;
using System.Workflow.Runtime;  
using IMES.Infrastructure;
using IMES.Infrastructure.WorkflowRuntime;
using System.Collections;
using IMES.FisObject.Common;
using IMES.FisObject.FA.Product; 
using IMES.Infrastructure.FisObjectRepositoryFramework;
using log4net;
using IMES.Route;
using IMES.FisObject.PAK.Pallet;
using IMES.FisObject.PAK.DN;
using IMES.FisObject.Common.Part;
using IMES.FisObject.PAK.Carton;
using System.Data;
using IMES.Infrastructure.Extend;
using IMES.FisObject.PAK.Pizza;
using IFAPrd = IMES.FisObject.FA.Product; 
namespace IMES.Station.Implementation
{   /// <summary>
    ///UnpackDNByCarton
    /// station :
    /// 本站实现的功能：
    ///    
    ///     
    ///    
    /// </summary> 

    public class UnpackDNByCarton : MarshalByRefObject, IUnpackDNByCarton
    {
        private const Session.SessionType currentSessionType = Session.SessionType.Common;
        private const Session.SessionType ProductSessionType = Session.SessionType.Product;
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public ArrayList GetDNListByInput(string sn)
        {
            logger.Debug("(UnpackDNByCarton)GetDNListByInput start, sn:" + sn);

            List<string> erpara = new List<string>();
            ArrayList retLst = new ArrayList();
            string key = "";
            List<string> dnLst = new List<string>();
            try
            {
              
                IProductRepository productRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IFAPrd.IProduct>();
                IDeliveryRepository deliveryRepository = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
                ICartonRepository cartonRep = RepositoryFactory.GetInstance().GetRepository<ICartonRepository, Carton>();
                Carton objCarton = cartonRep.GetCartonByBoxId(sn);
                if (objCarton == null)
                {
                    objCarton = cartonRep.Find(sn);
                }

                if (objCarton == null)
                {
                    throw new FisException("CHK109", new string[] { });
                }
                foreach (DeliveryCarton item in objCarton.DeliveryCartons)
                {
                    Delivery dn = deliveryRepository.Find(item.DeliveryNo);
                    if (dn.Status == "98")
                    { dnLst.Add(dn.DeliveryNo); }
                
                }
                IList<CartonProduct> lstCartonPrd = objCarton.CartonProducts;
                key = lstCartonPrd[0].ProductID;
                retLst.Add(key);
                retLst.Add(dnLst);

                return retLst;
              
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
                //logger.Debug("(CombineCartonInDN)InputFirstSN, firstSn:" + firstSn );
            }


        }
        public void Unpack(string proid, string line, string editor, string station, string customer)
        {

            logger.Debug("(UnpackDNByCarton)Unpack start, ProdID:" + proid + "line:" + line + "editor:" + editor + "station:" + station + "customer:" + customer);
            FisException ex;
            List<string> erpara = new List<string>();
            try
            {
                string sessionKey = proid;
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
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow("UnpackDNByCarton.xoml", "UnpackDNByCarton.rules", wfArguments);

                    // ********** Add by Benson at 2013/10/09
                    CommonImpl cmi = new CommonImpl();
                    IList<ConstValueInfo> lstConst = cmi.GetConstValueListByType("SAP", "Name").Where(x => x.value.Trim() != "").ToList(); ;
                    string isExcuteDeleteSAPsn = "";
                    string allowUnpackCode = "";
                    foreach (ConstValueInfo constV in lstConst)
                    {
                        if (constV.name == "ExcuteDeleteSNonSAP")
                        {
                            isExcuteDeleteSAPsn = constV.value;
                        }
                        if (constV.name == "AllowUnpackCode")
                        {
                            allowUnpackCode = constV.value;
                        }
                    }
                    string plant = System.Configuration.ConfigurationManager.AppSettings["PlantCode"];
                    currentSession.AddValue("ExcuteDeleteSNonSAP", isExcuteDeleteSAPsn);
                    currentSession.AddValue("AllowUnpackCode", allowUnpackCode);
                    currentSession.AddValue("PlantCode", plant);
                    // ********** Add by Benson at 2013/10/09

                    
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
                logger.Debug("(UnpackAllByDN)Unpack end, ProdID:" + proid + "line:" + line + "editor:" + editor + "station:" + station + "customer:" + customer);
            }
        }

      
    }
}
