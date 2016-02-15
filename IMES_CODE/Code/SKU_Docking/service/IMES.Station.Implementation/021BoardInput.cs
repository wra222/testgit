/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: PCARepairImpl
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2009-11-03   207006     Create 
 * 2010-02-03   207006     ITC-1122-0079
 * 
 * Known issues:Any restrictions about this file 
 */

using System;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Workflow.Runtime;
using IMES.DataModel;
using IMES.FisObject.Common.CheckItem;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.Route;
using IMES.Station.Interface.StationIntf;
using log4net;
using IMES.FisObject.Common.Model;
using IMES.FisObject.Common.Warranty;
using IMES.FisObject.PCA.MB;
using IMES.FisObject.Common.Part;
using IMES.Infrastructure.Repository._Schema;

namespace IMES.Station.Implementation
{
    public class _BoardInput : MarshalByRefObject, IBoardInput
    {


        private const Session.SessionType TheType = Session.SessionType.Product;
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    //    private string OriSessionkey;
    
        #region IBoardInput Members
        public IList<BomItemInfo> InputProdIdorCustsn(string pdLine, string input, string editor, string stationId, string customerId, out string realProdID, out string model)
        {
            logger.Debug("(_BoardInput)InputProdId start, pdLine:" + pdLine + " input:" + input + " editor:" + editor + " stationId:" + stationId + " customerId:" + customerId);

            var productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            
            var currentProduct = productRepository.GetProductByIdOrSn(input);

            //var currentProduct = CommonImpl.GetProductByInput(input, CommonImpl.InputTypeEnum.ProductIDOrCustSN);

            FisException ex;
            List<string> erpara = new List<string>();
            if (currentProduct == null)
            {

                erpara.Add(input);
                throw new FisException("SFC002", erpara);
            }

            string sessionKey = currentProduct.ProId;
            realProdID = sessionKey;
            model = currentProduct.Model;

            try
            {
                //ArrayList retLst = new ArrayList();
                Session Session = SessionManager.GetInstance.GetSession(sessionKey, TheType);

                if (Session == null)
                {


                    Session = new Session(sessionKey, TheType, editor, stationId, pdLine, customerId);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    //一个MB_SNo对应一个workflow
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", stationId);
                    wfArguments.Add("CurrentFlowSession", Session);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", pdLine);
                    wfArguments.Add("Customer", customerId);
                    wfArguments.Add("SessionType", TheType);

                    // WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow("_TEST.xoml", "_TEST.rules", wfArguments);
                //    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow("021BoardInput.xoml", "021BoardInput.rules", wfArguments); 
                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(stationId, "021BoardInput.xoml", "021BoardInput.rules", out wfName, out rlName);
               //   RouteManagementUtils.GetWorkflow(stationId, "021BoardInput_TEST.xoml", "021BoardInput_TEST.rules", out wfName, out rlName);
                  
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);
             
                    Session.AddValue(Session.SessionKeys.IsComplete, false);
                    //Session.AddValue(Session.SessionKeys.ifElseBranch, false);
                    Session.AddValue(Session.SessionKeys.CustSN, sessionKey);
                    Session.AddValue("isMB", "0");
                    Session.AddValue("IsRCTOMB", "N");


                    Session.SetInstance(instance);
                    if (!SessionManager.GetInstance.AddSession(Session))
                    {
                        Session.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");

                        erpara.Add(input);
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
                //get product data for UI
               // DataModel.ProductInfo prodInfo = currentProduct.ToProductInfo();
               // retLst.Add(prodInfo);

                //get bom
                return PartCollection.GeBOM(sessionKey, TheType);
                //retLst.Add(bomItemList);
             
                //var prod = (IProduct)Session.GetValue(Session.SessionKeys.Product);
                //model = prod.Model;
                //realProdID= prod.ProId;
                //add mb info in GetCheckItemList
              //  IList<BomItemInfo> ret = GetCheckItemList(Session, prod.PCBID, prod.PCBModel);
                //return retLst;
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
                logger.Debug("(_BoardInput)InputProdId end, pdLine:" + pdLine + " input:" + input + " editor:" + editor + " stationId:" + stationId + " customerId:" + customerId);

            }
        }

        public IMES.DataModel.MatchedPartOrCheckItem InputMBSn(string prodId, string item)
        {
            logger.Debug("(_BoardInput)InputMBSn start, prodId:" + prodId + " item:" + item);

            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = prodId;
          //  string sessionKey = OriSessionkey;

            Session Session = SessionManager.GetInstance.GetSession(prodId, TheType);
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
                ArrayList returnItem = new ArrayList();

                Session.Exception = null;
                Session.AddValue(Session.SessionKeys.ValueToCheck, item);
                Session.InputParameter = item;
                Session.SwitchToWorkFlow();

                if (Session.Exception != null)
                {
                    if (Session.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        Session.ResumeWorkFlow();
                    }

                    throw Session.Exception;
                }

                //2010-02-03   207006     ITC-1122-0079
                //IList<MatchedPartOrCheckItem> MatchedList = new List<MatchedPartOrCheckItem>();
                //get matchedinfo
                PartUnit matchedPart = (PartUnit)Session.GetValue(Session.SessionKeys.MatchedParts);

                if (matchedPart != null)
                {
                    MatchedPartOrCheckItem tempMatchedPart = new MatchedPartOrCheckItem();
                    tempMatchedPart.PNOrItemName = matchedPart.Pn;
                    tempMatchedPart.CollectionData = item;
                    tempMatchedPart.ValueType = matchedPart.ValueType;
                    return tempMatchedPart;
                }

/*                IList<IBOMPart> bomPartlist = (IList<IBOMPart>)Session.GetValue(Session.SessionKeys.MatchedParts);
  
                if ((bomPartlist != null) && (bomPartlist.Count > 0))
                {
                    foreach (IBOMPart bompartitem in bomPartlist)
                    {
                        MatchedPartOrCheckItem tempMatchedPart = new MatchedPartOrCheckItem();
                        tempMatchedPart.PNOrItemName = bompartitem.PN;
                        tempMatchedPart.CollectionData = item;
                        //tempMatchedPart.CollectionData = bompartitem.MatchedSn; itc200052 , 2011.12.13
                        tempMatchedPart.ValueType = bompartitem.Type; //itc200052 , 2011.12.13
                        MatchedList.Add(tempMatchedPart);
                    }
                    return MatchedList;
                }

                else
                {
                    ICheckObject citem = (ICheckObject)Session.GetValue(Session.SessionKeys.MatchedCheckItem);
                    if (citem == null)
                    {
                        throw new FisException("MAT010", new string[] { item });
                    }
                    else
                    {
                        MatchedPartOrCheckItem tempMatchedPart = new MatchedPartOrCheckItem();
                        //tempMatchedPart.PNOrItemName = citem.ItemDisplayName; itc200052, 2011.12.13
                        //tempMatchedPart.CollectionData = citem.ValueToCollect; itc200052, 2011.12.13
                        tempMatchedPart.ValueType = "";
                        MatchedList.Add(tempMatchedPart);
                        return MatchedList;
                    }
                } */
                return null;
        
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
                logger.Debug("(_BoardInput)InputMBSn end, prodId:" + prodId + " item:" + item); 
            }

        }

        public MatchedPartOrCheckItem TryPartMatchCheck(string sessionKey, string checkValue)
        {
            // mantis 1636
            if ("Y".Equals(GetConstVal("BoardInput", "CheckExceptMB").ToUpper()))
            {
                if (ChkProduct_unnormalPCB(sessionKey) && ChkPCB_unnormalPCB(checkValue))
                {
                    FisException ex;
                    List<string> erpara = new List<string>();
                    ex = new FisException("CHK973", erpara);
                    ex.stopWF = false;
                    throw ex;
                }
            }
            return PartCollection.TryPartMatchCheck(sessionKey, TheType, checkValue);
        }

        // mantis 1636
        private bool ChkProduct_unnormalPCB(string productid)
        {
            var productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            string strSQL = "select p.ProductID from Product p inner join ModelBOM m on p.Model=m.Material where p.ProductID=@productid and m.Component in('ZM2BL0B0011401','ZM2BL0B0011301','ZM2BL0B0011501')";
            SqlParameter paraName = new SqlParameter("@productid", SqlDbType.VarChar, 10);
            paraName.Direction = ParameterDirection.Input;
            paraName.Value = productid;
            DataTable tb = SqlHelper.ExecuteDataFill(SqlHelper.ConnectionString_GetData, System.Data.CommandType.Text,
                strSQL, paraName);
            if ((tb != null) && (tb.Rows.Count > 0))
                return true;
            return false;
        }
        private bool ChkModel_unnormalPCB(string model)
        {
            var productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            string strSQL = "select Material from dbo.ModelBOM where Material=@model and Component in('ZM2BL0B0011401','ZM2BL0B0011301','ZM2BL0B0011501')";
            SqlParameter paraName = new SqlParameter("@model", SqlDbType.VarChar, 255);
            paraName.Direction = ParameterDirection.Input;
            paraName.Value = model;
            DataTable tb = SqlHelper.ExecuteDataFill(SqlHelper.ConnectionString_GetData, System.Data.CommandType.Text,
                strSQL, paraName);
            if ((tb != null) && (tb.Rows.Count > 0))
                return true;
            return false;
        }
        private bool ChkPCB_unnormalPCB(string pcb)
        {
            if (pcb.Length < 2)
                return false;
            string mbcode = pcb.Substring(0, 2);
            if (!(mbcode.Equals("F1") || mbcode.Equals("F2") || mbcode.Equals("TL")))
                return false;
            var productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            string strSQL = @"SELECT PCBNo FROM PCB WHERE PCBNo=@pcb 
and ECR in('00001', '00011', '00021', '00031','00041',
'00002', '00012', '00022', '00032','00042',
'00003', '00013', '00023', '00033','00043',
'00004', '00014', '00024', '00034','00044',
'00005', '00015', '00025', '00035','00045') ";
            SqlParameter paraName = new SqlParameter("@pcb", SqlDbType.VarChar, 11);
            paraName.Direction = ParameterDirection.Input;
            paraName.Value = pcb;
            DataTable tb = SqlHelper.ExecuteDataFill(SqlHelper.ConnectionString_GetData, System.Data.CommandType.Text,
                strSQL, paraName);
            if ((tb != null) && (tb.Rows.Count > 0))
                return true;
            return false;
        }

        public ArrayList saveForPCAShippingLabel(string prodId, string MBno, string model, string dcode, IList<PrintItem> printItems)
        {
            logger.Debug("(_BoardInput)save start, MBno:" + MBno + "model:" + model + "dcode:" + dcode);

            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = prodId;
            Session Session = SessionManager.GetInstance.GetSession(sessionKey, TheType);
            ArrayList ret = new ArrayList();
            
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
                IMBRepository mbRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();

                IMB mb = mbRepository.Find(MBno);

                Session.AddValue(Session.SessionKeys.ModelName, model);
                Session.AddValue(Session.SessionKeys.WarrantyCode, dcode);
                Session.AddValue(Session.SessionKeys.PrintItems, printItems);
                //Session.AddValue(Session.SessionKeys.ifElseBranch, true);
                Session.AddValue(Session.SessionKeys.MB, mb);
                Session.AddValue(Session.SessionKeys.PrintLogName, "MBSNO");
                Session.AddValue(Session.SessionKeys.PrintLogBegNo, MBno);
                Session.AddValue(Session.SessionKeys.PrintLogEndNo, MBno);
                Session.AddValue(Session.SessionKeys.PrintLogDescr, "");
                //modify 1389 bug
                Session.AddValue("isMB", "1");

                Session.SwitchToWorkFlow();


                if (Session.Exception != null)
                {
                    if (Session.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        Session.ResumeWorkFlow();
                    }

                    throw Session.Exception;
                }

               // var retDCode = (string)Session.GetValue(Session.SessionKeys.DCode);
                var retDCode = dcode;
                var printLst = (IList<PrintItem>)Session.GetValue(Session.SessionKeys.PrintItems);

                ret.Add(retDCode);
                ret.Add(printLst);

                return ret;
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
                logger.Debug("(_BoardInput)save end, MBno:" + MBno + "model:" + model + "dcode:" + dcode);
            }
        }

        //modify 1053 bug
        public IList<PrintItem> Save(string prodId, IList<PrintItem> printItems, bool printflag, out string custsn)
        {
            logger.Debug("(_BoardInput)Save start, prodId:" + prodId);
        //    custsn = "";
            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = prodId;
            if (prodId.Trim().Length == 10)
            { prodId = prodId.Trim().Substring(0, 9); }  
            Session Session = SessionManager.GetInstance.GetSession(sessionKey, TheType);
            if (Session == null)
            {
                erpara.Add(sessionKey);
                ex = new FisException("CHK021", erpara);
                 throw ex;
            }
            try
            {

                var mb = (IMB)Session.GetValue(Session.SessionKeys.MB);
                Session.Exception = null;
                Session.AddValue("needprintflag", printflag);

                Session.AddValue(Session.SessionKeys.IsComplete, true);
                Session.AddValue(Session.SessionKeys.PrintItems, printItems);
                Session.AddValue(Session.SessionKeys.PrintLogName, "MBSNO");
                if (mb != null)
                {
                    Session.AddValue(Session.SessionKeys.PrintLogBegNo, mb.Sn);
                    Session.AddValue(Session.SessionKeys.PrintLogEndNo, mb.Sn);
                }
                Session.AddValue(Session.SessionKeys.PrintLogDescr, "");

                Session.SwitchToWorkFlow();
                var prod = (IProduct)Session.GetValue(Session.SessionKeys.Product);

                var dCode = (string)Session.GetValue(Session.SessionKeys.DCode);
                custsn = dCode;
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
                if (e.mErrcode.ToString().Trim() == "GEN021")
                {
                    ex = new FisException("GEN047", erpara);
                    throw ex;
                }
                else
                {
                    throw e;
                }
           
            }
            catch (Exception e) 
            {
                throw new SystemException(e.Message);
            }
            finally
            { 
                logger.Debug("(_BoardInput)Save end, prodId:" + prodId);
            }
            return (IList<PrintItem>)Session.GetValue(Session.SessionKeys.PrintItems);
        }
 
        public IList<PrintItem> PrintCustsnLabel(IList<PrintItem> printItems, string prodId)
        {
            string sessionKey = prodId;
            Session Session = SessionManager.GetInstance.GetSession(sessionKey, TheType);
            Session.AddValue(Session.SessionKeys.PrintItems, printItems);
            if (Session == null)
            {

                throw new Exception("Print Error");
            }
            try
            {
                Session.AddValue(Session.SessionKeys.IsComplete, true);
                Session.SwitchToWorkFlow();
            
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
                logger.Debug("(_BoardInput)Print Custsn Label, prodId:" + prodId);
            }

            return (IList<PrintItem>)Session.GetValue(Session.SessionKeys.PrintItems);
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
        public int CountQty(string line )
        {
           


            DateTime dtNow = DateTime.Now;
            DateTime dt2030 = Convert.ToDateTime("20:30");
        //    string  cmdTxt = @"select count(distinct p.ProductID) from ProductLog p,Product Pr
          //               where p.ProductID=Pr.ProductID and  p.Cdt>@dtBegin and p.Cdt<@dtEnd and p.Station='40'  and Pr.PCBID <>'' and Pr.PCBID is not null";
            //string cmdTxt = @"select count(*)  from PCBStatus nolock where Station='32' and Line=@Line and Udt>=@dtBegin and Udt<@dtEnd";
          
            DateTime dtBegin;
            DateTime dtEnd = DateTime.Now;
           // AM8:00算日班 , PM8:30算夜班
            if (IsDayWork())// 白班
            {
                dtBegin = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 8, 0, 0);
               
            }
            else
            {
                if (DateTime.Compare(dtNow, dt2030) > 0) // it means 00:00~20:30
                {
                    dtBegin = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,20, 30, 0);
                }
                else
                {
                    dtBegin = new DateTime(DateTime.Now.AddDays(-1).Year, DateTime.Now.AddDays(-1).Month, DateTime.Now.AddDays(-1).Day, 20, 30, 0);
                }
             
            }
            SqlParameter paraName = new SqlParameter("@dtBegin", SqlDbType.DateTime);
            paraName.Direction = ParameterDirection.Input;
            paraName.Value = dtBegin;
            SqlParameter paraName2 = new SqlParameter("@dtEnd", SqlDbType.DateTime);
            paraName2.Direction = ParameterDirection.Input;
            paraName2.Value = dtEnd;
            SqlParameter paraLine = new SqlParameter("@Line", SqlDbType.VarChar);
            paraLine.Direction = ParameterDirection.Input;
            paraLine.Value = line;

            //string result = SqlHelper.ExecuteScalar(SqlHelper.ConnectionString_GetData, System.Data.CommandType.Text, cmdTxt, paraName, paraName2, paraLine).ToString();
            string result = "1";
           return int.Parse(result);
        
        
        
        }
        private bool IsDayWork()
        {
            DateTime dt1 = Convert.ToDateTime("08:00");
            DateTime dt2 = Convert.ToDateTime("20:30");
            DateTime dt3 = DateTime.Now;
          
            if (DateTime.Compare(dt3, dt1) > 0 && DateTime.Compare(dt3, dt2) < 0)
            { return true; }
            return false;
        }

        private void ChkExistWarrantySetting(Array lst, string WarrantyCode)
        {
            if ((lst == null) || (lst.Length == 0))
            {
                List<string> erpara = new List<string>();
                erpara.Add(WarrantyCode);
                erpara.Add("MBDateCode");
                FisException ex = new FisException("CHK972", erpara);
                throw ex;
            }
        }

        /// <summary>
        /// SetDataCodeValue
        /// </summary>
        /// <param name="model"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        public string SetDataCodeValue(string model, string customer)
        {
            string value = "";
            string str = "";
            string ret = "";

            try
            {
                logger.Debug("SetDataCodeValue start, model:" + model + "customer:" + customer);

                IWarrantyRepository wr = RepositoryFactory.GetInstance().GetRepository<IWarrantyRepository, Warranty>();
                IList<Warranty> warrantys = wr.GetDCodeRuleListForMB("Customer");

                IModelRepository modelRepository = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();
                Model mdl = modelRepository.Find(model);
                value = (string)mdl.GetAttribute("WRNT");
                if (value != "")
                {
                    str = value.Substring(0, 1);
                }

                if (str == "1")
                {
                    var val = (from w in warrantys
                               where (w.WarrantyCode == "4")
                               select w.Id.ToString()).ToArray();
                    ChkExistWarrantySetting(val, "4");
                    ret = val[0].ToString();
                }
                else if (str == "0")
                {
                    var val = (from w in warrantys
                               where w.WarrantyCode == "0"
                               select w.Id.ToString()).ToArray();
                    ChkExistWarrantySetting(val, "0");
                    ret = val[0].ToString();

                }
                else if (str == "3")
                {
                    var val = (from w in warrantys
                               where w.WarrantyCode == "5"
                               select w.Id.ToString()).ToArray();
                    ChkExistWarrantySetting(val, "5");
                    ret = val[0].ToString();

                }
                else if (str == "9")
                {
                    var val = (from w in warrantys
                               where w.WarrantyCode == "3"
                               select w.Id.ToString()).ToArray();
                    ChkExistWarrantySetting(val, "3");
                    ret = val[0].ToString();

                }
                //变更描述： 增加DCode与保固期之间的对应关系 modify by itc200052, 2012-5-17
                else if (str == "2")
                {
                    var val = (from w in warrantys
                               where w.WarrantyCode == "C"
                               select w.Id.ToString()).ToArray();
                    ChkExistWarrantySetting(val, "C");
                    ret = val[0].ToString();

                }

                return ret;
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
                logger.Debug("SetDataCodeValue end, model:" + model + "customer:" + customer);
            }
        }

        private string GetConstVal(string type, string name)
        {
            IPartRepository partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            ConstValueInfo info = new ConstValueInfo();
            info.type = type;
            info.name = name;
            IList<ConstValueInfo> retList = partRepository.GetConstValueInfoList(info);
            if (retList != null && retList.Count > 0)
            {
                return retList[0].value;
            }
            return "";
        }

        public string GetSpeedLimit(string station)
        {
            if (! "Y".Equals(GetConstVal("BoardInput", "EnableCounting").ToUpper()))
            {
                return "";
            }
            return GetConstVal("SpeedLimit", "station" + station);
        }

        #endregion
    }
}
