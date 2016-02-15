/*
* INVENTEC corporation ©2011 all rights reserved. 
* Description: Combine Carton DN for 146MB
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* Known issues:
* TODO：
* 
*/


using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Workflow.Runtime;
using IMES.DataModel;
using IMES.FisObject.Common.Model;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;
using IMES.FisObject.Common.ReprintLog;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PAK.Pallet;
using IMES.FisObject.PAK.DN;
using IMES.FisObject.PAK.COA;
using IMES.FisObject.PAK.Pizza;
using IMES.FisObject.PAK.CartonSSCC;
using IMES.FisObject.PCA.MB;
using IBOMRepository = IMES.FisObject.Common.FisBOM.IBOMRepository;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.Infrastructure.UnitOfWork;
using IMES.Route;
using IMES.Station.Interface.StationIntf;
using carton = IMES.FisObject.PAK.CartonSSCC;
using log4net;
using IMES.FisObject.Common.PrintLog;
using IMES.Infrastructure.Repository._Schema;

namespace IMES.Station.Implementation
{
    /// <summary>
    /// </summary>
    public class CombineCartonDNfor146MB : MarshalByRefObject, ICombineCartonDNfor146MB
    {
        private const Session.SessionType TheType = Session.SessionType.MB;
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const Session.SessionType SessionType = Session.SessionType.MB;

        private void GetDeliveryNoList(string model, string line, string editor, string stationId, string customerId, ref IList<string> deliveryNos, ref IList<string> shipDatesOfDn, ref IList<string> modelsOfDn, ref IList<string> qtsOfDn, ref IList<string> shipwayOfDn)
        {
            FisException ex;
            List<string> erpara = new List<string>();
            try
            {
                string sessionKey = Guid.NewGuid().ToString();
                Session sessionInfo = SessionManager.GetInstance.GetSession(sessionKey, TheType);
                if (sessionInfo == null)
                {
                    sessionInfo = new Session(sessionKey, TheType, editor, stationId, "", customerId);
                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", stationId);
                    wfArguments.Add("CurrentFlowSession", sessionInfo);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("Customer", customerId);
                    wfArguments.Add("PdLine", "");
                    wfArguments.Add("SessionType", TheType);

                    sessionInfo.AddValue(Session.SessionKeys.ModelName, model);

                    //Remoting开始,调用workflow
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow("GetDeliveryListForRcto146MbCombineCarton.xoml", "", wfArguments);
                    sessionInfo.SetInstance(instance);
                    //for generate MB no
                    if (!SessionManager.GetInstance.AddSession(sessionInfo))
                    {
                        sessionInfo.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");
                        erpara.Add(sessionKey);
                        ex = new FisException("CHK020", erpara);
                        throw ex;
                    }
                    sessionInfo.WorkflowInstance.Start();
                    sessionInfo.SetHostWaitOne();
                }
                else
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK020", erpara);
                    throw ex;
                }

                //check workflow exception
                if (sessionInfo.Exception != null)
                {
                    if (sessionInfo.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        sessionInfo.ResumeWorkFlow();
                    }
                    throw sessionInfo.Exception;
                }

                //取session值.返回
                IList<DeliveryForRCTO146> lstDn = sessionInfo.GetValue("DeliveryForRCTO146List") as IList<DeliveryForRCTO146>;
                
                foreach (DeliveryForRCTO146 dn in lstDn)
                {
                    if (dn.Qty > 0)
                    {
                        deliveryNos.Add(dn.DeliveryNo);
                        shipDatesOfDn.Add(dn.ShipDate.ToString("yyyy-MM-dd"));
                        modelsOfDn.Add(dn.Model);
                        qtsOfDn.Add(dn.Qty.ToString());
                        shipwayOfDn.Add(dn.ShipWay.ToString());
                    }
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
        }

        private void CheckWin8Sps(string model)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ArrayList InputMBSN(string mbsn, bool isGetDn, string shipMode, string line, string editor, string station, string customer)
        {
            logger.Debug("(CombineCartonDNfor146MB)Inputmbsn start, shipMode:" + shipMode + " mbsn:" + mbsn + " line:" + line + " editor:" + editor + " station:" + station + " customer:" + customer);
            List<string> errpara = new List<string>();
            ArrayList retList = new ArrayList();
            string usedModel = "";
            
            try
            {
                bool shipModeIs146 = "RCTO".Equals(shipMode);
                bool shipModeIsFru = "FRU".Equals(shipMode);
                
                IMBRepository mbRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();
                IMB mb = mbRepository.Find(mbsn);
                if (mb == null || mb.MBStatus == null)
                {
                    throw new FisException("SFC001", new string[] { mbsn });
                }

                if (!shipMode.Equals(mb.ShipMode))
                {
                    // 此MBSN:%1 非 %2 板子
                    throw new FisException("CQCHK0038", new string[] { mbsn, shipMode });
                }
                
                string mbcode = mbsn.Substring(0, 2);

                if (!(string.IsNullOrEmpty(mb.CartonSN) && string.IsNullOrEmpty(mb.DeliveryNo)))
                {
                    // PCB的 CartonSN & DeliveryNo 須為空
                    throw new FisException("CQCHK0022", new string[] { });
                }

                IList<string> lstUsedModel = new List<string>();

                if (shipModeIs146)
                {
                    //string strSQL = "select Model from ModelInfo where Name='Infor' and Value=@MBCode";
                     string strSQL =@"select distinct a.Material as Model from ModelBOM a, PartInfo b 
                                    where a.Component = b.PartNo and b.InfoType='MB' and a.Material like'146%' 
                                    and b.InfoValue =@MBCode";
                    SqlParameter paraName = new SqlParameter("@MBCode", SqlDbType.VarChar, 10);
                    paraName.Direction = ParameterDirection.Input;
                    paraName.Value = mbcode;
                    DataTable tb = SqlHelper.ExecuteDataFill(SqlHelper.ConnectionString_GetData, System.Data.CommandType.Text,
                        strSQL, paraName);
                    if (tb != null)
                    {
                        foreach (DataRow dr in tb.Rows)
                        {
                            string s = dr["Model"].ToString();
                            if (s.IndexOf("146") == 0)
                            {
                                lstUsedModel.Add(s);
                            }
                        }
                    }
                    if (lstUsedModel.Count == 0)
                    {
                        // MBCode帶出的Model 不是146XXXXXX機型
                        throw new FisException("CQCHK0041", new string[] { });
                    }

                    string biosConstType = "RCTOMBBIOSVer";
                    CommonImpl cmi = new CommonImpl();
                    IList<ConstValueInfo> lstConst = cmi.GetConstValueListByType(biosConstType, "Name").Where(x => x.value.Trim() != "" && x.name == mbcode).ToList();
       
                    if (lstConst == null || lstConst.Count == 0)
                    {
                        // ConstValue 未设定 %1，请联系IE设定
                        throw new FisException("CQCHK0026", new string[] { biosConstType });
                    }

                    string mbrTestLog = "";
                    foreach (IMES.FisObject.Common.TestLog.TestLog tl in mb.TestLogs)
                    {
                        if ("MBR".Equals(tl.Type))
                        {
                            mbrTestLog = tl.Remark;
                            break;
                        }
                    }

                    if (mbrTestLog.IndexOf(lstConst[0].value) < 0)
                    {
                        // BIOS 匹配不對
                        throw new FisException("CQCHK0023", new string[] { });
                    }

                    bool isMatchMBCT = false;
                    foreach (IMES.FisObject.PCA.MB.MBInfo mbi in mb.MBInfos)
                    {
                        if ("MBCT".Equals(mbi.InfoType))
                        {
                            if (mbrTestLog.IndexOf(mbi.InfoValue) >= 0)
                            {
                                isMatchMBCT = true;
                            }
                            break;
                        }
                    }
                    if (!isMatchMBCT)
                    {
                        // MBCT 不相同
                        throw new FisException("CQCHK0024", new string[] { });
                    }
                }
                else if (shipModeIsFru)
                {
                    bool isFru = false;
                    string strSQL = @"select distinct a.Material as Model from ModelBOM a, PartInfo b 
where a.Component = b.PartNo and b.InfoType='MB' and a.Material not like 'PC%' 
and b.InfoValue =@MBCode";

                    SqlParameter paraName = new SqlParameter("@MBCode", SqlDbType.VarChar, 10);
                    paraName.Direction = ParameterDirection.Input;
                    paraName.Value = mbcode;
                    DataTable tb = SqlHelper.ExecuteDataFill(SqlHelper.ConnectionString_GetData, System.Data.CommandType.Text,
                        strSQL, paraName);
                    if (tb != null)
                    {
                        foreach (DataRow dr in tb.Rows)
                        {
                            //usedModel = dr["Model"].ToString();
                            lstUsedModel.Add(dr["Model"].ToString());
                            isFru = true;
                       }
                    }
                    if (!isFru)
                    {
                        throw new FisException("CQCHK0038", new string[] { mbsn, shipMode });
                    }

                }
                
                // 检查Process卡站
				CommonImpl.GetInstance().CheckProductBlockStation((MB)mb, line, editor, station, customer);

                IList<string> deliveryNos = new List<string>();
                IList<string> shipDatesOfDn = new List<string>();
                IList<string> modelsOfDn = new List<string>();
                IList<string> qtysOfDn = new List<string>();
                IList<string> shipwayOfDn = new List<string>();

                IList<string> lstMbsn = new List<string>();

				if (isGetDn)
                {
                    foreach (string m in lstUsedModel)
                    {
                        GetDeliveryNoList(m, line, editor, station, customer, ref deliveryNos, ref shipDatesOfDn, ref modelsOfDn, ref qtysOfDn, ref shipwayOfDn);
                    }
					
					if (deliveryNos == null || deliveryNos.Count == 0)
					{
						// DN %1 不存在!
						throw new FisException("CHK190", new string[] { "" });
					}

                    if (shipModeIsFru)
                    {
                        // 此機型下的所有MBCode清單
                        foreach (string m in lstUsedModel)
                        {
                            string strSQL = @"SELECT c.InfoValue as MBCode
FROM dbo.ModelBOM a, 
     Part b,
     PartInfo c 
WHERE a.Material=@Model and 
      a.Component = b.PartNo and
      b.PartNo =c.PartNo and
      b.Flag=1 and
      c.InfoType ='MB' and
      b.BomNodeType='MB'";
                            SqlParameter paraName = new SqlParameter("@Model", SqlDbType.VarChar, 255);
                            paraName.Direction = ParameterDirection.Input;
                            paraName.Value = m;
                            DataTable tb = SqlHelper.ExecuteDataFill(SqlHelper.ConnectionString_GetData, System.Data.CommandType.Text,
                                strSQL, paraName);
                            if (tb != null)
                            {
                                foreach (DataRow dr in tb.Rows)
                                {
                                    if (!lstMbsn.Contains(dr["MBCode"].ToString()))
                                        lstMbsn.Add(dr["MBCode"].ToString());
                                }
                            }
                        }
                    }
                }

                retList.Add(mbsn);
                retList.Add(""); // usedModel
                retList.Add(deliveryNos);
                retList.Add(shipDatesOfDn);
                retList.Add(modelsOfDn);
                retList.Add(qtysOfDn);
                retList.Add(lstMbsn);
                retList.Add(shipwayOfDn);
                return retList;
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
                logger.Debug("(CombineCartonDNfor146MB)Inputmbsn end, shipMode:" + shipMode + " mbsn:" + mbsn + " line:" + line + " editor:" + editor + " station:" + station + " customer:" + customer);
            }
        }

        public ArrayList GetDnQty(string dnsn, string usedModel, string shipMode, string line, string editor, string station, string customer)
        {
            logger.Debug("(CombineCartonDNfor146MB)GetDnQty start, dn:" + dnsn + " model:" + usedModel + " shipMode:" + shipMode + " line:" + line + " editor:" + editor + " station:" + station + " customer:" + customer);
            ArrayList retList = new ArrayList();
            // CnQty
            // DnQty
            // DnQty - DnRemainQty
            // win8sps

            string win8sps = "";

            try
            {
                if (string.IsNullOrEmpty(dnsn))
                {
                    throw new FisException("DN Can Not be Empty!");
                }

                bool shipModeIs146 = "RCTO".Equals(shipMode);
                bool shipModeIsFru = "FRU".Equals(shipMode);

                if (shipModeIsFru)
                {
                    
                        IModelRepository modelRep = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();

                        IList<IMES.FisObject.Common.Model.ModelInfo> info1 = modelRep.GetModelInfoByModelAndName(usedModel, "MB");
                        if (info1 != null && info1.Count > 0)
                        {
                            foreach (IMES.FisObject.Common.Model.ModelInfo v in info1)
                            {
                                if (!string.IsNullOrEmpty(v.Value))
                                {
                                    string val = v.Value.Trim();
                                    if (!string.IsNullOrEmpty(val) && val.Length > 3 && !"001".Equals(val.Substring(val.Length - 3, 3)))
                                    {
                                        win8sps = val;
                                        break;
                                    }
                                }
                            }
                        }
                    
                }

                IDeliveryRepository deliveryRep = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
                Delivery dn = deliveryRep.Find(dnsn);
                if (dn == null)
                {
                    // 找不到 DN %1
                    throw new FisException("CQCHK0028", new string[] { dnsn });
                }

                int CnQty = dn.DeliveryEx.QtyPerCarton;
                int DnQty = dn.Qty;

                IMBRepository mbRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();
                int DnRemainQty = mbRepository.GetCombinedMBQtyWithDeliveryNo(dnsn);

                retList.Add(CnQty.ToString());
                retList.Add(DnQty.ToString());
                retList.Add((DnQty - DnRemainQty).ToString());
                retList.Add(win8sps);

                return retList;
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
                logger.Debug("(CombineCartonDNfor146MB)GetDnQty end, dn:" + dnsn + " model:" + usedModel + " shipMode:" + shipMode + " line:" + line + " editor:" + editor + " station:" + station + " customer:" + customer);
            }
        }

        public ArrayList Save(IList<string> mbsns, string dnsn, string usedModel, IList<PrintItem> printItems, string shipMode, string line, string editor, string station, string customer)
        {
            string logmbsns = string.Join(",", mbsns.ToArray());
            logger.Debug("(CombineCartonDNfor146MB)Save start, mbsns:" + logmbsns + " model:" + usedModel + " shipMode:" + shipMode + " line:" + line + " editor:" + editor + " station:" + station + " customer:" + customer);

            try
            {
                ArrayList retList = new ArrayList();
                FisException ex;
                List<string> erpara = new List<string>();

                IModelRepository modelRep = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();
                IProductRepository productRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
                IDeliveryRepository deliveryRep = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();

                if ((null == mbsns) || (mbsns.Count == 0))
                {
                    erpara.Add("");
                    ex = new FisException("CHK021", erpara);
                    logger.Error(ex.Message, ex);
                    throw ex;
                }

                string sessionKey = mbsns[0];
                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, TheType);

                //用ProductID启动工作流，将Product放入工作流中
                if (currentSession == null)
                {
                    currentSession = new Session(sessionKey, TheType, editor, station, line, customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", currentSession);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", line);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", TheType);

                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(station, "CombineCartonDNfor146MB.xoml", "CombineCartonDNfor146MB.rules", out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    currentSession.AddValue(Session.SessionKeys.MBSNOList, mbsns);
                    currentSession.AddValue(Session.SessionKeys.DeliveryNo, dnsn);
                    currentSession.AddValue(Session.SessionKeys.ModelName, usedModel);
                    currentSession.AddValue(Session.SessionKeys.PrintItems, printItems);

                    currentSession.AddValue(Session.SessionKeys.ShipMode, shipMode);
                    currentSession.AddValue(Session.SessionKeys.Qty, mbsns.Count); // for Activity UpdateDn87Status
                    
                    currentSession.AddValue("SessionPrintLogName", printItems[0].LabelType);
                    /*currentSession.AddValue(Session.SessionKeys.PrintLogBegNo, cartonSN);
                    currentSession.AddValue(Session.SessionKeys.PrintLogEndNo, cartonSN);*/
                    currentSession.AddValue("SessionPrintLogDescr", "MBCT");
                    currentSession.AddValue(Session.SessionKeys.Reason, "");
					currentSession.AddValue(Session.SessionKeys.RCTO146Category, "MBCT");

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

                IList<PrintItem> returnList = (IList<PrintItem>)currentSession.GetValue(Session.SessionKeys.PrintItems);
                retList.Add(returnList);

                string cartonSn = currentSession.GetValue(Session.SessionKeys.Carton) as string;
                retList.Add(cartonSn);

                string palletNo = currentSession.GetValue(Session.SessionKeys.PalletNo) as string;
                if (null == palletNo)
                    palletNo = "";
                retList.Add(palletNo);

                return retList;
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
                logger.Debug("(CombineCartonDNfor146MB)Save end, mbsns:" + logmbsns + " model:" + usedModel + " shipMode:" + shipMode + " line:" + line + " editor:" + editor + " station:" + station + " customer:" + customer);
            }
        }

        public ArrayList Reprint(string mbsn, string reason, string line, string editor,
                                    string station, string customer, IList<PrintItem> printItems)
        {
            logger.Debug("(CombineCartonDNfor146MB)Reprint Start,"
                            + " [mbsn]:" + mbsn
                            + " [line]:" + line
                            + " [editor]:" + editor
                            + " [station]:" + station
                            + " [customer]:" + customer);

            try
            {
                List<string> erpara = new List<string>();
                FisException ex;

                IMBRepository mbRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();
                IMB mb = mbRepository.Find(mbsn);
                if (mb == null)
                {
                    throw new FisException("SFC001", new string[] { mbsn });
                }

                if (string.IsNullOrEmpty(mb.CartonSN))
                {
                    // 此 MB 的 CartonSN 為空
                    throw new FisException("CQCHK0029", new string[] {  });
                }

                string cartonSn = mb.CartonSN;

                //Check Print Log
                var repository = RepositoryFactory.GetInstance().GetRepository<IPrintLogRepository, PrintLog>();

                PrintLog condition = new PrintLog();
                condition.Name = printItems[0].LabelType;
                condition.BeginNo = cartonSn;
                condition.Descr = "MBCT";
                IList<PrintLog> printLogList = repository.GetPrintLogListByCondition(condition);
                if (printLogList.Count == 0)
                {
                    ex = new FisException("CHK270", erpara);
                    throw ex;
                }

                //Check Print Log
                string sessionKey = mbsn;
                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, TheType);
                if (currentSession == null)
                {
                    currentSession = new Session(sessionKey, TheType, editor, station, line, customer);
                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", currentSession);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", line);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", TheType);
                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(station, "RePrint.xoml", string.Empty, out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    currentSession.AddValue(Session.SessionKeys.PrintItems, printItems);

                    currentSession.AddValue(Session.SessionKeys.PrintLogName, printItems[0].LabelType);
                    currentSession.AddValue(Session.SessionKeys.PrintLogBegNo, cartonSn);
                    currentSession.AddValue(Session.SessionKeys.PrintLogEndNo, cartonSn);
                    currentSession.AddValue(Session.SessionKeys.PrintLogDescr, "MBCT");
                    currentSession.AddValue(Session.SessionKeys.Reason, reason);

                    //Session.AddValue(Session.SessionKeys.IsComplete, false);
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

                //check workflow exception
                if (currentSession.Exception != null)
                {
                    if (currentSession.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        currentSession.ResumeWorkFlow();
                    }
                    throw currentSession.Exception;
                }


                IList<PrintItem> returnList = (IList<PrintItem>)currentSession.GetValue(Session.SessionKeys.PrintItems);
                ArrayList arr = new ArrayList();
                arr.Add(returnList);
                arr.Add(cartonSn);
                return arr;

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
                logger.Debug("(CombineCartonDNfor146MB)Reprint End,"
                                + " [mbsn]:" + mbsn
                                + " [line]:" + line
                                + " [editor]:" + editor
                                + " [station]:" + station
                                + " [customer]:" + customer);
            }
        }

        public void CheckFRUMBOA3(string mbsn,string usedmodel)
        {
            CommonImpl cmi = new CommonImpl();
            IList<ConstValueTypeInfo> lstConst = cmi.GetConstValueTypeListByType("RMAOA3CheckModel");
            bool needcheckoa3 = lstConst.Where(x => x.value == usedmodel).Any();
            if (needcheckoa3)
            {
                IMBRepository mbRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();
                IMB mb = mbRepository.Find(mbsn);
                if (mb == null )
                {
                    throw new FisException("SFC001", new string[] { mbsn });
                }
                string mbct = (string)mb.GetExtendedProperty("MBCT");
                if (string.IsNullOrEmpty(mbct))
                {
                    throw new FisException("ICT024");
                }

                string strSQL = @"IF EXISTS(SELECT 1 from RMA_OA3Data where CT=@CT)
                                                         SELECT 1
                                                        ELSE
                                                         SELECT 0 ";
                SqlParameter sqlPara = new SqlParameter("@CT", SqlDbType.VarChar, 32);
                sqlPara.Direction = ParameterDirection.Input;
                sqlPara.Value = mbct;
                object data = SqlHelper.ExecuteScalar(SqlHelper.ConnectionString_GetData, System.Data.CommandType.Text, strSQL, sqlPara);
                if ((int)data == 0)
                {
                    throw new FisException("CQCHK50118", new string[] { mbct });
                }



               
            }

        }
        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="sn"></param>
        public void cancel(string sn)
        {
            logger.Debug("(CombinePoInCarton)Cancel start, [sn]:" + sn);

            string sessionKey = sn;
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
                logger.Debug("(CombinePoInCarton)Cancel end, [sn]:" + sn);
            }
        }
        
    }
}
