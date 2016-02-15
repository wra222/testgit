using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using IMES.Station.Interface.StationIntf;
using IMES.Infrastructure;
using IMES.Infrastructure.WorkflowRuntime;
//using IMES.Station.Interface.CommonIntf;
using System.Workflow.Runtime;
using IMES.DataModel;
using log4net;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.PCA.MBMO;
using IMES.FisObject.Common.Model;
using IMES.Infrastructure.Extend;
using IMES.FisObject.Common.PrintLog;
using IMES.Route;
using IMES.FisObject.Common.Station;
using IMES.FisObject.FA.Product;
using IMES.FisObject.Common.Defect;
using IMES.FisObject.PCA.MB;
using IMES.FisObject.Common.Material;
using IMES.FisObject.PAK.DN;
using System.Data.SqlClient;
using System.Data;
using IMES.Infrastructure.Repository._Schema;


namespace IMES.Station.Implementation
{

    /// <summary>
    /// </summary>
    public partial class CombineCartonAndDNfor146 : MarshalByRefObject, ICombineCartonAndDNfor146
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region IRCTOLabelReprint Members

        public ArrayList Save(IList<string> snList, string model, string dnNo, string qty, string line, string editor, string station, string customer, string materialType,IList<PrintItem> printItems)
        {
            ArrayList retrunValue = new ArrayList();
            string cartonSN = "";
            FisException ex;
            List<string> erpara = new List<string>();
            try
            {
                logger.Debug("(CombineCartonAndDNfor146)Reprint start, start dn:" + dnNo + " editor:" + editor + " customerId:" + customer);
                string sessionKey = snList[0];
                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, Session.SessionType.Product);
                if (currentSession == null)
                {
                    currentSession = new Session(sessionKey, Session.SessionType.Product, editor, station, "", customer);
                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();

                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", currentSession);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", line);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", Session.SessionType.Product);
                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(station, "CombineCartonAndDNfor146.xoml", "CombineCartonAndDNfor146.rules", out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);
                    currentSession.AddValue(Session.SessionKeys.MaterialType,materialType);
                    if (materialType.Contains("LCM" ))
                    {
                        IProductRepository iProductRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository>();
                        IList<IProduct> lstPrd = new List<IProduct>();
                        foreach(string sn in snList)
                        {
                            IProduct product = iProductRepository.GetProductByCustomSn(sn);
                            lstPrd.Add(product);
                        }
                        currentSession.AddValue(Session.SessionKeys.ProdList, lstPrd);
                    }
                  
                    currentSession.AddValue(Session.SessionKeys.DeliveryNo, dnNo);
                    currentSession.AddValue(Session.SessionKeys.Qty, int.Parse(qty));
                    currentSession.AddValue(Session.SessionKeys.NewScanedProductIDList, snList);
                    currentSession.AddValue(Session.SessionKeys.ModelName,model);
                    currentSession.AddValue(Session.SessionKeys.MaterialCTList, snList);
                    currentSession.AddValue(Session.SessionKeys.PrintLogName,"146 Carton Label");
                    currentSession.AddValue(Session.SessionKeys.PrintLogDescr,"MaterialCT");
                    currentSession.AddValue(Session.SessionKeys.PrintItems, printItems);
                    currentSession.SetInstance(instance);
                    currentSession.AddValue(Session.SessionKeys.ShipMode, "RCTO");
                    currentSession.AddValue("MaterialStage","PAK");
                    //string shipMode = (string)CurrentSession.GetValue(Session.SessionKeys.ShipMode) ?? "";
                   // string materialStage = (string)CurrentSession.GetValue("MaterialStage") ?? "";

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

                //check workflow exception
                if (currentSession.Exception != null)
                {
                    if (currentSession.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        currentSession.ResumeWorkFlow();
                    }
                    throw currentSession.Exception;
                }
                IList<PrintItem> printItem = (IList<PrintItem>)currentSession.GetValue(Session.SessionKeys.PrintItems);
                cartonSN = currentSession.GetValue(Session.SessionKeys.Carton).ToString();
                retrunValue.Add(cartonSN);
                retrunValue.Add(printItem);
                return retrunValue;
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
                logger.Debug("(CombineCartonAndDNfor146)Reprint end, dn:" + dnNo + " editor:" + editor + " station:" + station + " customerId:" + customer);

            }
        }
        public S_DNfor146 GetDnInfo(string dnNo, string materialType)
        {
            var materialRep = RepositoryFactory.GetInstance().GetRepository<IMaterialRepository>();
            var remainQ = 0;
            IDeliveryRepository DeliveryRepository = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
            Delivery dnObj= DeliveryRepository.GetDelivery(dnNo);
            remainQ = dnObj.Qty - materialRep.GetCombinedMaterialQty(materialType, null, dnNo, null);
            return new S_DNfor146 { DnNo = dnNo, CartonQty = dnObj.DeliveryEx.QtyPerCarton, RemainQty = remainQ, Qty = dnObj.Qty };
         
        }

        public ArrayList GetDeliveryAndVendorCodeList(string model,string materialType, int beginDay, int endDay)
        {
            ArrayList arr = new ArrayList();
            List<string> lstVendorCode = new List<string>();
            IList<DeliveryForRCTO146> dnList = GetDnList(model, beginDay, endDay).ToList();        
            List<S_DNfor146> lstDn146 = new List<S_DNfor146>();
            var materialRep = RepositoryFactory.GetInstance().GetRepository<IMaterialRepository>();
           
            int remainQ = 0;
            if (materialType == "DockingMB" || materialType == "RMAMB")
            {
                lstVendorCode = GetMBCodeList(model);
            }
            else
            {
                lstVendorCode = GetVendorCodeList(model);
            }

            if (dnList.Count > 0)
            {
                
               foreach (DeliveryForRCTO146 dn146 in dnList)
               {
                   remainQ = dn146.Qty - materialRep.GetCombinedMaterialQty(materialType, null, dn146.DeliveryNo, null);
                  lstDn146.Add(new S_DNfor146 { DnNo = dn146.DeliveryNo, CartonQty = dn146.QtyPerCarton,
                                        RemainQty = remainQ, Qty = dn146.Qty,ShipDate=dn146.ShipDate.ToString("yyyyMMdd"),ShipWay=dn146.ShipWay });

               }
               
            }
            arr.Add(lstDn146);
            arr.Add(lstVendorCode);
            return arr;
        }

        private List<string> GetMBCodeList(string model)
        {
            string strSQL = @"select b.InfoValue as VendorCode
                                                    from ModelBOM a,
                                                         PartInfo b,   
                                                         Part c 
                                                    where 
                                                         a.Component=b.PartNo and 
                                                         b.PartNo=c.PartNo and 
                                                         c.BomNodeType='MB'and 
                                                         b.InfoType='MB'and a.Material=@model";
            SqlParameter paraName = new SqlParameter("@model ", SqlDbType.VarChar, 64);
            paraName.Direction = ParameterDirection.Input;
            paraName.Value = model;
            DataTable tb = SqlHelper.ExecuteDataFill(SqlHelper.ConnectionString_GetData,
                                    System.Data.CommandType.Text,
                                  strSQL, paraName);
            List<string> lst = new List<string>();
            foreach (DataRow dr in tb.Rows)
            {
                lst.Add(dr[0].ToString().Trim());
            }
            return lst;
        }

        private List<string> GetVendorCodeList(string model)
        {
            string strSQL = @"select  c.Component as VendorCode
                                                      from ModelBOM a,
                                                           ModelBOM b,
                                                           ModelBOM c
                                                    where
                                                          a.Material=@model and
                                                          a.Component = b.Material and
                                                          b.Component =c.Material ";
            SqlParameter paraName = new SqlParameter("@model ", SqlDbType.VarChar, 64);
            paraName.Direction = ParameterDirection.Input;
            paraName.Value = model;
            DataTable tb = SqlHelper.ExecuteDataFill(SqlHelper.ConnectionString_GetData,
                                    System.Data.CommandType.Text,
                                  strSQL, paraName);
            List<string> lst = new List<string>();
            foreach (DataRow dr in tb.Rows)
            {
                lst.Add(dr[0].ToString().Trim());
            }
            return lst;
        }


        private IList<DeliveryForRCTO146> GetDnList(string model, int beginDay, int endDay)
        {
          
            DateTime beginDate
               = new DateTime(DateTime.Now.AddDays(beginDay).Year, DateTime.Now.AddDays(beginDay).Month,
                                         DateTime.Now.AddDays(beginDay).Day);

            DateTime endDate
              = new DateTime(DateTime.Now.AddDays(endDay).Year, DateTime.Now.AddDays(endDay).Month,
                                        DateTime.Now.AddDays(endDay).Day);

            IDeliveryRepository DeliveryRepository = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
           
            IList<DeliveryForRCTO146> lstDn =
              DeliveryRepository.GetDeliveryForRCTO146(model.Trim(), "00", beginDate, endDate);
            return lstDn;
        
        }
        public void CheckMaSn(string sn,string station)
        {
            var materialRep = RepositoryFactory.GetInstance().GetRepository<IMES.FisObject.Common.Material.IMaterialRepository>();
            Material material = materialRep.Find(sn);
            if (material!=null && !material.CheckMaterailStatus(station))
            {
                throw new FisException("CQCHK1068", new string[] { material.MaterialCT});
            }
        }

        public void CheckCrSn(string sn,string line, string editor, string customer,string materialType,string station,string model)
        {
                IProductRepository iProductRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository>();
                IProduct product = iProductRepository.GetProductByCustomSn(sn);
                if (product == null)
                {
                    FisException fe = new FisException("CHK079", new string[] { sn });
                    throw fe;
                }
                if (product.Model != model)
                { throw new FisException("The model of this product does not match input model!"); }
                CheckMaSn(sn,station);
                CommonImpl cmm = new CommonImpl();
                if (materialType == "146LCM")
                { cmm.CheckProductBlockStation((Product)product, line, editor, station, customer); }
                else
                { cmm.CheckProductBlockStation((Product)product, line, editor, "CR32", customer); }
           }

        public ArrayList RePrint(string ct, string reason, IList<PrintItem> printItems, string editor, string stationId, string customer)
        {
         
            FisException ex;
            List<string> erpara = new List<string>();
            ArrayList retvaluelist = new ArrayList();
            var materialRep = RepositoryFactory.GetInstance().GetRepository<IMaterialRepository>();
            Material material = materialRep.Find(ct);
            if (material == null || string.IsNullOrEmpty(material.CartonSN))
            {
                throw new FisException("The CT or CartonSN is null");
            }
            string sessionKey = ct;

            try
            {
                Session session = SessionManager.GetInstance.GetSession(sessionKey, Session.SessionType.Product);
                if (session == null)
                {
                    session = new Session(sessionKey, Session.SessionType.Product, editor, stationId, ct, customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    //一个MB_SNo对应一个workflow
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", stationId);
                    wfArguments.Add("CurrentFlowSession", session);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", "");
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", Session.SessionType.Product);

                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(stationId, "ReprintCartonLabel.xoml", "", out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    session.AddValue(Session.SessionKeys.PrintItems, printItems);
                    session.AddValue(Session.SessionKeys.Reason, reason);
                    session.AddValue(Session.SessionKeys.PrintLogName, "146 Carton Label");
                    session.AddValue(Session.SessionKeys.PrintLogBegNo, material.CartonSN);
                    session.AddValue(Session.SessionKeys.PrintLogEndNo, material.CartonSN);

                    session.AddValue(Session.SessionKeys.IsComplete, false);
                    session.SetInstance(instance);

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

                //check workflow exception
                if (session.Exception != null)
                {
                    if (session.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        session.ResumeWorkFlow();
                    }

                    throw session.Exception;
                }


                IList<PrintItem> resultPrintItems = session.GetValue(Session.SessionKeys.PrintItems) as IList<PrintItem>;

                retvaluelist.Add(resultPrintItems);

                string printLabel = (string)session.GetValue(Session.SessionKeys.PrintLogName);
           
                retvaluelist.Add(material.CartonSN);
               
                return retvaluelist;

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
                logger.Debug("RCTOLabelPrint(Cancel) start, sessionKey:" + sessionKey);
                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, Session.SessionType.Product);
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
                logger.Debug("RCTOLabelPrint(Cancel) end, sessionKey:" + sessionKey);
            }

        }

		
	
        #endregion
    }
}
