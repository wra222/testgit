/*
* INVENTEC corporation ©2011 all rights reserved. 
* Description:Combine Po in Carton
* UI:CI-MES12-SPEC-PAK-UI Combine Po in Carton.docx –2012/05/21 
* UC:CI-MES12-SPEC-PAK-UC Combine Po in Carton.docx –2012/05/21     
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2012-05-21   Du.Xuan               Create   
* ITC-1414-0074  修正reprint入参错误
* ITC-1414-0082  补充syssetting描述
* ITC-1414-0087  修改DN显示格式
* ITC-1414-0125  DN完成后记录log
* Known issues:
* TODO：
* 
*/
using System;
using System.Collections;
using System.Collections.Generic;
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
using IBOMRepository = IMES.FisObject.Common.FisBOM.IBOMRepository;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.Infrastructure.UnitOfWork;
using IMES.Route;
using IMES.Docking.Interface.DockingIntf;
using carton = IMES.FisObject.PAK.CartonSSCC;
using log4net;
using System.Data.SqlClient;
using System.Data;
using IMES.Infrastructure.Repository._Schema;

namespace IMES.Docking.Implementation
{
    /// <summary>
    /// CombinePoInCarton接口的实现类
    /// </summary>
    public class CombinePoInCarton : MarshalByRefObject, ICombinePoInCarton
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const Session.SessionType SessionType = Session.SessionType.Product;

        private IProductRepository productRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IMES.FisObject.FA.Product.IProduct>();
        private IDeliveryRepository deliveryRep = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();

        #region members
        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputSN"></param>
        /// <param name="deliveryNo"></param>
        /// <param name="line"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        public ArrayList InputSN(string inputSN, string deliveryNo, string model,string firstProID,string line, string editor, string station, string customer)
		{
			return InputSN(inputSN, deliveryNo, model, firstProID, line, editor, station, customer, "");
		}

        public ArrayList InputSN_subFunc_isNotBT(string inputSN, string deliveryNo, string model, string firstProID, string line, string editor, string station, string customer, string isBT, bool newflag, ref IProduct curProduct)
        {
            ArrayList retList = new ArrayList();

            if ("Y".Equals(isBT))
            {
                FisException ex;
                List<string> erpara = new List<string>();
                ex = new FisException("DCK003", erpara);//非BT机器
                throw ex;
            }

            //g.	如果Product 尚未结合Delivery(Product.DeliveryNo)，则报告错误：“Not found delivery for ” + @CustomerSN
            //b)	如果Product 尚未结合Delivery(Product.DeliveryNo)，则报告错误：“Not found delivery for ” + @CustomerSN；
            //报告错误后，清空页面[Model] 内容
            if (string.IsNullOrEmpty(curProduct.DeliveryNo))
            {
                FisException ex;
                List<string> erpara = new List<string>();
                erpara.Add(curProduct.CUSTSN);
                ex = new FisException("PAK129", erpara);//Not found delivery for + @CustomerSN
                throw ex;
            }
            if (deliveryNo == "")
            {
                deliveryNo = curProduct.DeliveryNo;
            }

            if (model == "")
            {
                model = curProduct.Model;
            }

            if (firstProID == "")
            {
                firstProID = curProduct.ProId;
            }
            //g.	如果Product 结合的Delivery 与页面上[Delivery] 中的不同，则报告错误：“Delivery is not match!”
            if (curProduct.DeliveryNo != deliveryNo)
            {
                FisException ex;
                List<string> erpara = new List<string>();
                erpara.Add(curProduct.CUSTSN);
                ex = new FisException("PAK130", erpara);//Delivery is not match!
                throw ex;
            }

            //检查Product Model (Product.Model) 是否与页面上的[Model] 相同，如果不同，则报告错误：“Model is not match!”
            if (curProduct.Model != model)
            {
                FisException ex;
                List<string> erpara = new List<string>();
                erpara.Add(curProduct.Model);
                ex = new FisException("PAK131", erpara);//Model is not match!
                throw ex;
            }

            //c)如果Product 结合的Delivery 在数据库中不存在或者ShipDate 已经是5天以前了，则报告错误：“Not found Delivery for ”+ @Model
            DateTime beginCdt = DateTime.Now.AddDays(-5);
            Delivery curDev = deliveryRep.Find(curProduct.DeliveryNo);
            if (curDev == null || curDev.ShipDate < beginCdt)
            {
                FisException ex;
                List<string> erpara = new List<string>();
                erpara.Add(curProduct.Model);
                ex = new FisException("PAK129", erpara);
                throw ex;
            }

            //h.	如果用户刷入的Product Id (可能是直接刷入的，也可能是基于Customer S/N查询到的)已经刷过，则报告错误：“Duplicate data!”
            if (!string.IsNullOrEmpty(curProduct.CartonSN))
            {
                FisException ex;
                List<string> erpara = new List<string>();
                erpara.Add(curProduct.Model);
                ex = new FisException("PAK134", erpara);//Duplicate data!
                throw ex;
            }


            string sessionKey = firstProID;
            Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, SessionType);

            if (currentSession == null)
            {
                if (!newflag)
                {
                    FisException ex;
                    List<string> erpara = new List<string>();
                    ex = new FisException("PAK157", erpara);
                    retList.Add("Error");
                    retList.Add(ex);
                    return retList;
                }
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

                RouteManagementUtils.GetWorkflow(station, "CombinePoInCarton.xoml", "CombinePoInCarton.rules", out wfName, out rlName);
                WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                currentSession.AddValue(Session.SessionKeys.IsComplete, false);
                currentSession.AddValue(Session.SessionKeys.Product, curProduct);
                IList<IProduct> productList = new List<IProduct>();
                currentSession.AddValue(Session.SessionKeys.ProdList, productList);
                IList<string> productIDList = new List<string>();
                currentSession.AddValue(Session.SessionKeys.NewScanedProductIDList, productIDList);

                currentSession.SetInstance(instance);

                if (!SessionManager.GetInstance.AddSession(currentSession))
                {
                    currentSession.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");
                    FisException ex;
                    List<string> erpara = new List<string>();
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK020", erpara);
                    throw ex;
                }

                currentSession.WorkflowInstance.Start();
                currentSession.SetHostWaitOne();
            }
            else
            {
                if (newflag)
                {
                    FisException ex;
                    List<string> erpara = new List<string>();
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK020", erpara);
                    throw ex;
                }
                currentSession.AddValue(Session.SessionKeys.Product, curProduct);

                currentSession.Exception = null;
                currentSession.SwitchToWorkFlow();
                /*FisException ex;
                List<string> erpara = new List<string>();
                erpara.Add(sessionKey);
                ex = new FisException("CHK020", erpara);
                throw ex;*/
            }
            if (currentSession.Exception != null)
            {
                if (currentSession.GetValue(Session.SessionKeys.WFTerminated) != null)
                {
                    currentSession.ResumeWorkFlow();
                }
                IList<IProduct> productList = (List<IProduct>)currentSession.GetValue(Session.SessionKeys.ProdList);
                if (productList.Count == 0)
                {
                    SessionManager.GetInstance.RemoveSession(currentSession);
                }
                throw currentSession.Exception;
            }
            //========================================================                
            //SELECT Qty as [Total Qty] FROM Delivery NOLOCK WHERE DeliveryNo = @Delivery
            //SELECT InfoValue as [PCs in Carton] FROM DeliveryInfo NOLOCK WHERE DeliveryNo = @Delivery AND InfoType = ‘CQty’
            //SELECT COUNT(ProductID) as [Packed Qty] FROM Product NOLOCK WHERE DeliveryNo = @Delivery

            ProductModel curModel = new ProductModel();

            curModel.ProductID = curProduct.ProId;
            curModel.CustSN = curProduct.CUSTSN;
            curModel.Model = curProduct.Model;

            IList<IProduct> proList = (List<IProduct>)currentSession.GetValue(Session.SessionKeys.ProdList);
            IList<string> idList = (List<string>)currentSession.GetValue(Session.SessionKeys.NewScanedProductIDList);
            proList.Add(curProduct);
            idList.Add(curProduct.ProId);

            currentSession.AddValue(Session.SessionKeys.ProdList, proList);
            currentSession.AddValue(Session.SessionKeys.NewScanedProductIDList, idList);

            string cQtyStr = (string)curDev.GetExtendedProperty("CQty");
            int cQty = 0;
            if (string.IsNullOrEmpty(cQtyStr))
            {
                cQty = 5;
            }
            else
            {
                decimal tmp = Convert.ToDecimal(cQtyStr);
                cQty = Convert.ToInt32(tmp);
            }
            int packedqty = 0;
            var tmpList = productRep.GetProductListByDeliveryNo(curDev.DeliveryNo);
            foreach (var prod in tmpList)
            {
                if (!string.IsNullOrEmpty(prod.CartonSN))
                {
                    packedqty++;
                }
            }

            //RTRIM(DeliveryNo) + ‘_’ + CONVERT(varchar, ShipDate, 111) + ‘_’ + CONVERT(varchar, Qty) 
            //就是UI 中[Delivery] List 中Item 的显示格式，正确取得Product 结合的Qty 后，要在[Delivery] 中选中该Delivery
            string devStr = curDev.DeliveryNo + "_" + curDev.ShipDate.Year.ToString("d4") + "/"
                + curDev.ShipDate.Month.ToString("d2") + "/" + curDev.ShipDate.Day.ToString("d2") + "_" + curDev.Qty.ToString();

            retList.Add("SUCCESS");
            retList.Add(curModel);
            retList.Add(curProduct.DeliveryNo);
            retList.Add(curDev.Qty);//Total Qty
            retList.Add(cQty);//PCs in Carton
            retList.Add(packedqty);//Packed Qty
            retList.Add(devStr);//Delivery Item
            retList.Add("N"); // isNotBT ; mantis 1696
            //========================================================
            return retList;
        }

        public ArrayList InputSN_subFunc_isBT(string inputSN, string deliveryNo, string model, string firstProID, string line, string editor, string station, string customer, string isBT, bool newflag, ref IProduct curProduct)
        {
            ArrayList retList = new ArrayList();

            string strSQL = "";
            SqlParameter paraName = null;
            DataTable tb = null;

            if ("N".Equals(isBT))
            {
                FisException ex;
                List<string> erpara = new List<string>();
                ex = new FisException("DCK004", erpara);//BT机器
                throw ex;
            }

            if ("".Equals(isBT))
            {
                if ("".Equals(deliveryNo))
                {
                    // RTRIM(DeliveryNo) + '_' + CONVERT(varchar, ShipDate, 111) + '_' + CONVERT(varchar, Qty) as DeliveryListItem
                    strSQL = @"SELECT RTRIM(DeliveryNo) + '_' + CONVERT(varchar, ShipDate, 111) + '_' + CONVERT(varchar, Qty) as DeliveryListItem
	FROM Delivery d WHERE Status ='00' And Model = @Model And ShipDate >= CONVERT(char(10),GETDATE() - 5,111)  
    and Qty>(select COUNT(1) from Product where DeliveryNo=d.DeliveryNo) ORDER BY ShipDate";
                    // 
                    paraName = new SqlParameter("@Model", SqlDbType.VarChar, 32);
                    paraName.Direction = ParameterDirection.Input;
                    paraName.Value = curProduct.Model;
                    tb = SqlHelper.ExecuteDataFill(SqlHelper.ConnectionString_GetData, System.Data.CommandType.Text, strSQL, paraName);
                    if (tb == null || tb.Rows.Count == 0)
                    {
                        FisException ex;
                        List<string> erpara = new List<string>();
                        erpara.Add(curProduct.Model);
                        ex = new FisException("DCK002", erpara);//Not Found PoData for ' + @Model
                        throw ex;
                    }

                    ArrayList lstDNs = new ArrayList();
                    for (int i = 0; i < tb.Rows.Count; i++)
                    {
                        lstDNs.Add(tb.Rows[i]["DeliveryListItem"].ToString());
                    }

                    retList.Add("SUCCESS");
                    retList.Add(lstDNs);
                    retList.Add("");
                    retList.Add("");//Total Qty
                    retList.Add("");//PCs in Carton
                    retList.Add("");//Packed Qty
                    retList.Add("");//Delivery Item
                    retList.Add("NeedDN"); // isBT, Need choose dn ; mantis 1696
                    //========================================================
                    return retList;
                }
                isBT = "Y";
            }

            if (!string.IsNullOrEmpty(curProduct.DeliveryNo))
            {
            }
            curProduct.DeliveryNo = deliveryNo;

            if (model == "")
            {
                model = curProduct.Model;
            }

            if (firstProID == "")
            {
                firstProID = curProduct.ProId;
            }
            //g.	如果Product 结合的Delivery 与页面上[Delivery] 中的不同，则报告错误：“Delivery is not match!”
            if (curProduct.DeliveryNo != deliveryNo)
            {
                FisException ex;
                List<string> erpara = new List<string>();
                erpara.Add(curProduct.CUSTSN);
                ex = new FisException("PAK130", erpara);//Delivery is not match!
                throw ex;
            }

            //检查Product Model (Product.Model) 是否与页面上的[Model] 相同，如果不同，则报告错误：“Model is not match!”
            if (curProduct.Model != model)
            {
                FisException ex;
                List<string> erpara = new List<string>();
                erpara.Add(curProduct.Model);
                ex = new FisException("PAK131", erpara);//Model is not match!
                throw ex;
            }

            //c)如果Product 结合的Delivery 在数据库中不存在或者ShipDate 已经是5天以前了，则报告错误：“Not found Delivery for ”+ @Model
            DateTime beginCdt = DateTime.Now.AddDays(-5);
            Delivery curDev = deliveryRep.Find(curProduct.DeliveryNo);
            if (curDev == null || curDev.ShipDate < beginCdt)
            {
                FisException ex;
                List<string> erpara = new List<string>();
                erpara.Add(curProduct.Model);
                ex = new FisException("PAK129", erpara);
                throw ex;
            }

            //h.	如果用户刷入的Product Id (可能是直接刷入的，也可能是基于Customer S/N查询到的)已经刷过，则报告错误：“Duplicate data!”
            if (!string.IsNullOrEmpty(curProduct.CartonSN))
            {
                FisException ex;
                List<string> erpara = new List<string>();
                erpara.Add(curProduct.Model);
                ex = new FisException("PAK134", erpara);//Duplicate data!
                throw ex;
            }


            string sessionKey = firstProID;
            Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, SessionType);

            if (currentSession == null)
            {
                if (!newflag)
                {
                    FisException ex;
                    List<string> erpara = new List<string>();
                    ex = new FisException("PAK157", erpara);
                    retList.Add("Error");
                    retList.Add(ex);
                    return retList;
                }
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

                RouteManagementUtils.GetWorkflow(station, "CombinePoInCarton.xoml", "CombinePoInCarton.rules", out wfName, out rlName);
                WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                currentSession.AddValue(Session.SessionKeys.IsComplete, false);
                currentSession.AddValue(Session.SessionKeys.Product, curProduct);
                IList<IProduct> productList = new List<IProduct>();
                currentSession.AddValue(Session.SessionKeys.ProdList, productList);
                IList<string> productIDList = new List<string>();
                currentSession.AddValue(Session.SessionKeys.NewScanedProductIDList, productIDList);

                currentSession.SetInstance(instance);

                if (!SessionManager.GetInstance.AddSession(currentSession))
                {
                    currentSession.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");
                    FisException ex;
                    List<string> erpara = new List<string>();
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK020", erpara);
                    throw ex;
                }

                currentSession.WorkflowInstance.Start();
                currentSession.SetHostWaitOne();
            }
            else
            {
                if (newflag)
                {
                    FisException ex;
                    List<string> erpara = new List<string>();
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK020", erpara);
                    throw ex;
                }
                currentSession.AddValue(Session.SessionKeys.Product, curProduct);

                currentSession.Exception = null;
                currentSession.SwitchToWorkFlow();
                /*FisException ex;
                List<string> erpara = new List<string>();
                erpara.Add(sessionKey);
                ex = new FisException("CHK020", erpara);
                throw ex;*/
            }
            if (currentSession.Exception != null)
            {
                if (currentSession.GetValue(Session.SessionKeys.WFTerminated) != null)
                {
                    currentSession.ResumeWorkFlow();
                }
                IList<IProduct> productList = (List<IProduct>)currentSession.GetValue(Session.SessionKeys.ProdList);
                if (productList.Count == 0)
                {
                    SessionManager.GetInstance.RemoveSession(currentSession);
                }
                throw currentSession.Exception;
            }
            //========================================================                
            //SELECT Qty as [Total Qty] FROM Delivery NOLOCK WHERE DeliveryNo = @Delivery
            //SELECT InfoValue as [PCs in Carton] FROM DeliveryInfo NOLOCK WHERE DeliveryNo = @Delivery AND InfoType = ‘CQty’
            //SELECT COUNT(ProductID) as [Packed Qty] FROM Product NOLOCK WHERE DeliveryNo = @Delivery

            ProductModel curModel = new ProductModel();

            curModel.ProductID = curProduct.ProId;
            curModel.CustSN = curProduct.CUSTSN;
            curModel.Model = curProduct.Model;

            IList<IProduct> proList = (List<IProduct>)currentSession.GetValue(Session.SessionKeys.ProdList);
            IList<string> idList = (List<string>)currentSession.GetValue(Session.SessionKeys.NewScanedProductIDList);
            proList.Add(curProduct);
            idList.Add(curProduct.ProId);

            currentSession.AddValue(Session.SessionKeys.ProdList, proList);
            currentSession.AddValue(Session.SessionKeys.NewScanedProductIDList, idList);

            string keysessionDNForBT = "ChangeToDN";
            string sessionDNForBT = currentSession.GetValue(keysessionDNForBT) as string;
            if (string.IsNullOrEmpty(sessionDNForBT))
            {
                currentSession.AddValue(keysessionDNForBT, deliveryNo);
            }
            else if (!deliveryNo.Equals(sessionDNForBT))
            {
            }

            string keysessionProIdsForBT = "prodsToChangeToDN";
            ArrayList sessionProIdsForBT = currentSession.GetValue(keysessionProIdsForBT) as ArrayList;
            if (sessionProIdsForBT == null)
            {
                sessionProIdsForBT = new ArrayList();
            }
            sessionProIdsForBT.Add(curProduct);
            currentSession.AddValue(keysessionProIdsForBT, sessionProIdsForBT);

            currentSession.AddValue("isBT", isBT);

            string cQtyStr = (string)curDev.GetExtendedProperty("CQty");
            int cQty = 0;
            if (string.IsNullOrEmpty(cQtyStr))
            {
                cQty = 5;
            }
            else
            {
                decimal tmp = Convert.ToDecimal(cQtyStr);
                cQty = Convert.ToInt32(tmp);
            }
            int packedqty = 0;
            var tmpList = productRep.GetProductListByDeliveryNo(curDev.DeliveryNo);
            foreach (var prod in tmpList)
            {
                if (!string.IsNullOrEmpty(prod.CartonSN))
                {
                    packedqty++;
                }
            }

            //RTRIM(DeliveryNo) + ‘_’ + CONVERT(varchar, ShipDate, 111) + ‘_’ + CONVERT(varchar, Qty) 
            //就是UI 中[Delivery] List 中Item 的显示格式，正确取得Product 结合的Qty 后，要在[Delivery] 中选中该Delivery
            string devStr = curDev.DeliveryNo + "_" + curDev.ShipDate.Year.ToString("d4") + "/"
                + curDev.ShipDate.Month.ToString("d2") + "/" + curDev.ShipDate.Day.ToString("d2") + "_" + curDev.Qty.ToString();

            retList.Add("SUCCESS");
            retList.Add(curModel);
            retList.Add(curProduct.DeliveryNo);
            retList.Add(curDev.Qty);//Total Qty
            retList.Add(cQty);//PCs in Carton
            retList.Add(packedqty);//Packed Qty
            retList.Add(devStr);//Delivery Item
            retList.Add("Y"); // isBT ; mantis 1696
            //========================================================
            return retList;
        }

		public ArrayList InputSN(string inputSN, string deliveryNo, string model,string firstProID,string line, string editor, string station, string customer, string isBT)
        {
            logger.Debug("(CombinePoInCarton)InputSN start, inputSN:" + inputSN);

            try
            {
                ArrayList retList = new ArrayList();
                
                bool newflag = false;
                if (firstProID == "")
                {
                    newflag = true;
                }

                //var productRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IMES.FisObject.FA.Product.IProduct>();
                //IDeliveryRepository deliveryRep = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();

                IProduct curProduct;
                if (inputSN.Substring(0, 2) == "CN" || inputSN.Substring(0, 2) == "5C")
                {
                    curProduct = productRep.GetProductByCustomSn(inputSN);
                }
                else
                {
                    curProduct = productRep.Find(inputSN);
                }

				if (null == curProduct)
                {

                    if (inputSN.Substring(0, 2) == "CN" || inputSN.Substring(0, 2) == "5C")
                    {
                        List<string> errpara = new List<string>();
                        errpara.Add(inputSN);
                        throw new FisException("PAK042", errpara);
                    }
                    else
                    {
                        List<string> errpara = new List<string>();
                        errpara.Add(inputSN);
                        throw new FisException("SFC002", errpara);
                    }
                }
                //c.	如果Product.UnitWeight= 0.000，则报告错误：“No Weight”
                if (curProduct.UnitWeight == 0)
                {
                    FisException ex;
                    List<string> erpara = new List<string>();
                    erpara.Add(curProduct.CUSTSN);
                    ex = new FisException("PAK126", erpara);//No Weight
                    throw ex;
                }
                //d.	如果QCStatus 中没有该Product  的Tp = ‘PAQC’ 的记录，则报告错误：“This product is not input PAQC!”
                ProductQCStatus qcStatus = deliveryRep.GetQCStatus(curProduct.ProId, "PAQC");
                if (qcStatus == null)
                {
                    FisException ex;
                    List<string> erpara = new List<string>();
                    erpara.Add(curProduct.CUSTSN);
                    ex = new FisException("PAK127", erpara);//This product is not input PAQC!
                    throw ex;
                }
                //e.	如果QCStatus 中该Product的Tp = ‘PAQC’ 的记录中Udt 最新的记录的Status 值是’8’ 的话，则报告错误：“This product is not output PAQC!”
                if (qcStatus.Status == "8")
                {
                    FisException ex;
                    List<string> erpara = new List<string>();
                    erpara.Add(curProduct.CUSTSN);
                    ex = new FisException("PAK128", erpara);//This product is not output PAQC!
                    throw ex;
                }
                //f.	如果QCStatus 中该Product的Tp = ‘PAQC’ 的记录中Udt 最新的记录的Status 值是’A’ 的话，则报告错误：“This product does not pass PAQC!”
                if (qcStatus.Status == "A")
                {
                    FisException ex;
                    List<string> erpara = new List<string>();
                    erpara.Add(curProduct.CUSTSN);
                    ex = new FisException("PAK142", erpara);//This product does not pass PAQC!
                    throw ex;
                }
				
				bool curBT = false;
				// BT机器重流，如果變非BT，有DeliveryNo，但仍有ProductBT => 要當作非BT
				if ((null == curProduct.DeliveryNo) || ("" == curProduct.DeliveryNo)) {
					IList<ProductBTInfo> lstBT = productRep.GetProductBT(curProduct.ProId);
					if ((null != lstBT) && (lstBT.Count > 0))
						curBT = true;
				}
				
				if (curBT)
                    return InputSN_subFunc_isBT(inputSN, deliveryNo, model, firstProID, line, editor, station, customer, isBT, newflag, ref curProduct);
                else
                    return InputSN_subFunc_isNotBT(inputSN, deliveryNo, model, firstProID, line, editor, station, customer, isBT, newflag, ref curProduct);
				
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
                logger.Debug("(CombinePoInCarton)InputSN end, uutSn:" + inputSN);
            }
        }


        public ArrayList Save(string firstProID, IList<PrintItem> printItems)
        {
            logger.Debug("(CombinePoInCarton)Save start, firstProID:" + firstProID);

            try
            {
                IModelRepository modelRep = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();
                Session currentSession = SessionManager.GetInstance.GetSession(firstProID, SessionType);

                if (currentSession == null)
                {
                    FisException ex;
                    List<string> erpara = new List<string>();
                    erpara.Add(firstProID);
                    ex = new FisException("CHK021", erpara);
                    logger.Error(ex.Message, ex);
                    throw ex;
                }
                else
                {

                    //如果Product 非Frame Or TRO Or BaseModel Or SLICE 的话，需要报告错误：“Product is not Frame Or TRO Or BaseModel Or SLICE” 
                    //SELECT @PN = Value FROM ModelInfo NOLOCk WHERE Model = @Model AND Name = 'PN'
                    IProduct product = (IProduct)currentSession.GetValue(Session.SessionKeys.Product);

                    string pn = "";
                    string modelstr = product.Model;
                    Model curModel = modelRep.Find(product.Model);
                    pn = curModel.GetAttribute("PN");

                    bool labelFlag = false;
                    if (!string.IsNullOrEmpty(pn) && pn.Length >= 6)
                    {
                        if (pn.Substring(5, 1) == "U"
                            || pn.Substring(5, 1) == "E"
                            || product.Model.Substring(0, 3) == "156"
                            || product.Model.Substring(0, 3) == "146"
                            || product.Model.Substring(0, 3) == "157"
                            || product.Model.Substring(0, 3) == "158"
                            || product.Model.Substring(0, 2) == "PO"
                            || product.Model.Substring(0, 2) == "2P"
                            || product.Model.Substring(0, 3) == "172"
                            || product.Model.Substring(0, 2) == "BC")
                        {
                            labelFlag = true;
                        }
                    }
                    if (!labelFlag)
                    {
                        SessionManager.GetInstance.RemoveSession(currentSession);
                        FisException fe = new FisException("PAK133", new string[] { });  //Product is not Frame Or TRO Or BaseModel Or SLICE
                        throw fe;
                    }

                    currentSession.AddValue(Session.SessionKeys.PrintItems, printItems);
                    currentSession.AddValue(Session.SessionKeys.IsComplete, true);
                    currentSession.Exception = null;
                    currentSession.SwitchToWorkFlow();
                }

                if (currentSession.Exception != null)
                {
                    if (currentSession.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        currentSession.ResumeWorkFlow();
                    }

                    throw currentSession.Exception;
                }
                ArrayList retList = new ArrayList();
                IList<PrintItem> printList = (IList<PrintItem>)currentSession.GetValue(Session.SessionKeys.PrintItems);
                Delivery delivery = (Delivery)currentSession.GetValue(Session.SessionKeys.Delivery);
                string flagstr = (string)delivery.GetExtendedProperty("Flag");
                Product curProd = (Product)currentSession.GetValue(Session.SessionKeys.Product);

                int totalQty = delivery.Qty;
                int packedQty = 0;

                //IProductRepository productRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
                //IDeliveryRepository DeliveryRep = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();

                var tmpList = productRep.GetProductListByDeliveryNo(delivery.DeliveryNo);
                foreach (var prod in tmpList)
                {
                    if (!string.IsNullOrEmpty(prod.CartonSN))
                    {
                        packedQty++;
                    }
                }

                string strLoc = "";
                string locate = (string)currentSession.GetValue("Location");
                FisException fisex;
                List<string> erp = new List<string>();
                erp.Add(locate);
                fisex = new FisException("PAK137", erp);//“请将栈板放置在 ” + @location + “ 号库位!”
                strLoc = fisex.mErrmsg;
                
                //a)	如果Remain Qty = 0，则提示用户：’Po:’ + @Delivery + ‘ is finished!’；
                //提示用户后，执行Reset（Reset 说明见下文）
                //b)	如果PCs in Carton > Remain Qty，则令[PCs in Carton] = Remain Qty
                string strEnd="";
                int remainQty = totalQty - packedQty;
                if (remainQty == 0)
                {
                    //ITC-1414-0125
                    //7.	当页面选择的Delivery 已经完成Combine Po in Carton 时，需要将Delivery.Status 更新为’88’
                    FisException ex;
                    List<string> erpara = new List<string>();
                    erpara.Add(delivery.DeliveryNo);
                    ex = new FisException("PAK136", erpara);//’Po:’ + @Delivery + ‘ is finished!’
                    strEnd = ex.mErrmsg;

                    IUnitOfWork uof = new UnitOfWork();
                    delivery.Status = "88";
                    delivery.Udt = DateTime.Now;
                    delivery.Editor = currentSession.Editor;
                    deliveryRep.Update(delivery,uof);
                    uof.Commit(); 
                }

                // pdf
                string templatename = "";
                ICartonSSCCRepository cartRep = RepositoryFactory.GetInstance().GetRepository<ICartonSSCCRepository, IMES.FisObject.PAK.CartonSSCC.CartonSSCC>();
                if (flagstr == "N")
                {
                    //HP_EDI.dbo.op_TemplateCheck '"&DN&"','Box Ship Label'
                    templatename = cartRep.GetTemplateNameViaCallOpTemplateCheck(curProd.DeliveryNo, "Box Ship Label");
                    //Not found template of this DN: "&DN
                    if (templatename == "ERROR")
                    {
                        FisException ex;
                        List<string> erpara = new List<string>();
                        erpara.Add(curProd.DeliveryNo);
                        ex = new FisException("PAK132", erpara);//Not found template of this DN: &DN
                        throw ex;
                    }
                }

                //当Delivery 的CQty 属性=1，但Delivery.Model 的NoCarton 属性(Model.InfoType = ‘NoCarton’)不存在，或者存在但<>’Y’ 时，也需要列印Carton Label
                //Model curModel = modelRep.Find(curProduct.Model);
                Model model = modelRep.Find(curProd.Model);
                string noCarton = model.GetAttribute("NoCarton");

                string cQtyStr = (string)delivery.GetExtendedProperty("CQty");
                int cQty = 0;
                if (string.IsNullOrEmpty(cQtyStr))
                {
                    cQty = 5;
                }
                else
                {
                    decimal tmp = Convert.ToDecimal(cQtyStr);
                    cQty = Convert.ToInt32(tmp);
                }

                string printflag = "N";
                if (cQty > 1 || (cQty == 1 && noCarton != "Y"))
                {
                    printflag = "Y";
                }

                retList.Add(printList);
                retList.Add(flagstr);
                retList.Add(packedQty);
                retList.Add(curProd.CartonSN);
                retList.Add(curProd.DeliveryNo);
                retList.Add(curProd.CUSTSN);
                retList.Add(templatename);
                retList.Add(strLoc);
                retList.Add(strEnd);
                retList.Add(printflag);

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
                logger.Debug("(CombinePoInCarton)Print end, firstProID:" + firstProID);
            }
        }

        public ArrayList ReprintCartonLabel(string inputSN, string reason, string line, string editor,
                                    string station, string customer, IList<PrintItem> printItems)
        {
            logger.Debug("(CombinPoInCarton)ReprintLabel Start,"
                            + " [custSN]:" + inputSN
                            + " [line]:" + line
                            + " [editor]:" + editor
                            + " [station]:" + station
                            + " [customer]:" + customer);
            
            ArrayList retList = new ArrayList();
            try
            {
                //var productRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IMES.FisObject.FA.Product.IProduct>();
                //IDeliveryRepository deliveryRep = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();

                IProduct curProduct;
 
                curProduct = productRep.FindOneProductWithProductIDOrCustSNOrCarton(inputSN);
                if (curProduct == null)
                {
                    if (inputSN.Substring(0, 2) == "CN" || inputSN.Substring(0, 2) == "5C")
                    {
                        List<string> errpara = new List<string>();
                        errpara.Add(inputSN);
                        throw new FisException("PAK042", errpara);
                    }
                    else if (inputSN.Substring(0, 1) == "C")
                    {
                        List<string> errpara = new List<string>();
                        errpara.Add(inputSN);
                        throw new FisException("CHK801", errpara);
                    }
                    else
                    {
                        List<string> errpara = new List<string>();
                        errpara.Add(inputSN);
                        throw new FisException("SFC002", errpara);
                    }
                }

                //d.	如果Product 尚未结合的Carton，则报告错误：“该Product 尚未Combine Carton，不能Reprint Carton Label!”
                if (string.IsNullOrEmpty(curProduct.CartonSN))
                {
                    List<string> errpara = new List<string>();
                    errpara.Add(curProduct.ProId);
                    throw new FisException("PAK138", errpara);
                }

                //e.	如果输入的[Carton No] 在数据库(CartonStatus.CartonNo) 中不存在，则报告错误：“此Carton 不存在!”
                carton.ICartonSSCCRepository cartRep = RepositoryFactory.GetInstance().GetRepository<carton.ICartonSSCCRepository, IMES.FisObject.PAK.CartonSSCC.CartonSSCC>();
                CartonStatusInfo carConf = new CartonStatusInfo();
                carConf.cartonNo = curProduct.CartonSN;
                IList<CartonStatusInfo> carList = cartRep.GetCartonStatusInfo(carConf);
                if (carList.Count == 0)
                {
                    List<string> errpara = new List<string>();
                    errpara.Add(curProduct.ProId);
                    throw new FisException("PAK139", errpara);//此Carton 不存在!
                }

                //如果Product 非Frame Or TRO Or BaseModel Or SLICE 的话，需要报告错误：“Product is not Frame Or TRO Or BaseModel Or SLICE” 
                //SELECT @PN = Value FROM ModelInfo NOLOCk WHERE Model = @Model AND Name = 'PN'
                IModelRepository modelRep = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();
                string pn = "";
                string noCarton = "";
                string modelstr = curProduct.Model;
                Model curModel = modelRep.Find(curProduct.Model);
                pn = curModel.GetAttribute("PN");
                noCarton = curModel.GetAttribute("NoCarton");

                bool labelFlag = false;
                if (!string.IsNullOrEmpty(pn) && pn.Length >= 6)
                {
                    if (pn.Substring(5, 1) == "U"
                        || pn.Substring(5, 1) == "E"
                        || curProduct.Model.Substring(0, 3) == "156"
                        || curProduct.Model.Substring(0, 3) == "146"
                        || curProduct.Model.Substring(0, 3) == "157"
                        || curProduct.Model.Substring(0, 3) == "158"
                        || curProduct.Model.Substring(0, 2) == "PO"
                        || curProduct.Model.Substring(0, 2) == "2P"
                        || curProduct.Model.Substring(0, 3) == "172"
                        || curProduct.Model.Substring(0, 2) == "BC")
                    {
                        labelFlag = true;
                    }
                }
                if (!labelFlag)
                {
                    FisException fe = new FisException("PAK133", new string[] { });  //Product is not Frame Or TRO Or BaseModel Or SLICE
                    throw fe;
                }

                string sessionKey = curProduct.ProId;


                var repository = RepositoryFactory.GetInstance().GetRepository<IMES.FisObject.FA.Product.IProductRepository, IMES.FisObject.FA.Product.IProduct>();
                
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

                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow("ReprintCombinPoInCarton.xoml", "", wfArguments);
                    
                    currentSession.AddValue(Session.SessionKeys.PrintLogBegNo, curProduct.ProId);
                    currentSession.AddValue(Session.SessionKeys.PrintLogEndNo, curProduct.ProId);
                    currentSession.AddValue(Session.SessionKeys.PrintLogName, curProduct.ProId);
                    currentSession.AddValue(Session.SessionKeys.PrintLogDescr, "CombinInCarton");
                    currentSession.AddValue(Session.SessionKeys.Reason, reason);
                    currentSession.AddValue(Session.SessionKeys.PrintItems, printItems);
                    currentSession.SetInstance(instance);

                    if (!SessionManager.GetInstance.AddSession(currentSession))
                    {
                        currentSession.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");
                        FisException ex;
                        List<string> erpara = new List<string>();
                        erpara.Add(sessionKey);
                        ex = new FisException("CHK020", erpara);
                        throw ex;
                    }

                    currentSession.WorkflowInstance.Start();
                    currentSession.SetHostWaitOne();
                }
                else
                {
                    FisException ex;
                    List<string> erpara = new List<string>();
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
                
                IList<PrintItem> printList = (IList<PrintItem>)currentSession.GetValue(Session.SessionKeys.PrintItems);
                Delivery curDev = deliveryRep.Find(curProduct.DeliveryNo);
               
                string cQtyStr = (string)curDev.GetExtendedProperty("CQty");
                int cQty = 0;
                if (string.IsNullOrEmpty(cQtyStr))
                {
                    cQty = 5;
                }
                else
                {
                    decimal tmp = Convert.ToDecimal(cQtyStr);
                    cQty = Convert.ToInt32(tmp);
                }
                
                // pdf
                string flagstr = (string)curDev.GetExtendedProperty("Flag");
                string templatename = "";
                if (flagstr == "N")
                {
                    //HP_EDI.dbo.op_TemplateCheck '"&DN&"','Box Ship Label'
                    templatename = cartRep.GetTemplateNameViaCallOpTemplateCheck(curProduct.DeliveryNo, "Box Ship Label");
                    //Not found template of this DN: "&DN
                    if (templatename == "ERROR")
                    {
                        FisException ex;
                        List<string> erpara = new List<string>();
                        erpara.Add(curProduct.DeliveryNo);
                        ex = new FisException("PAK132", erpara);//Not found template of this DN: &DN
                        throw ex;
                    }
                }

                
                var log = new ReprintLog
                {
                    LabelName = currentSession.GetValue(Session.SessionKeys.PrintLogName).ToString(),
                    BegNo = currentSession.GetValue(Session.SessionKeys.PrintLogBegNo).ToString(),
                    EndNo = currentSession.GetValue(Session.SessionKeys.PrintLogEndNo).ToString(),
                    Descr = (string)currentSession.GetValue(Session.SessionKeys.PrintLogDescr),
                    Reason = (string)currentSession.GetValue(Session.SessionKeys.Reason),
                    Editor = editor
                };
                
                //当Delivery 的CQty 属性=1，但Delivery.Model 的NoCarton 属性(Model.InfoType = ‘NoCarton’)不存在，或者存在但<>’Y’ 时，也需要列印Carton Label
                string printflag = "N";
                if (cQty > 1 || (cQty ==1 && noCarton!="Y"))
                {
                    printflag = "Y";
                    IUnitOfWork uof = new UnitOfWork();
                    var rep = RepositoryFactory.GetInstance().GetRepository<IReprintLogRepository, ReprintLog>();
                    rep.Add(log, uof);
                    uof.Commit();
                }

                retList.Add(printList);
                retList.Add(flagstr);
                retList.Add(0);
                retList.Add(curProduct.CartonSN);
                retList.Add(curProduct.DeliveryNo);
                retList.Add(curProduct.CUSTSN);
                retList.Add(templatename);
                retList.Add(cQty);
                retList.Add(printflag);

                return retList;

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
                logger.Debug("(CombinPoInCarton)ReprintLabel End,"
                                + " [custSN]:" + inputSN
                                + " [line]:" + line
                                + " [editor]:" + editor
                                + " [station]:" + station
                                + " [customer]:" + customer);
            }
        }

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="prodId"></param>
        public void cancel(string deliveryNO)
        {
            logger.Debug("(CombinePoInCarton)Cancel start, [deliveryNO]:" + deliveryNO);

            string sessionKey = deliveryNO;
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
                logger.Debug("(CombinePoInCarton)Cancel end, [deliveryNO]:" + deliveryNO);
            }
        }
        #endregion

        public string GetSysSetting(string name)
        {
            try
            {
                List<string> erpara = new List<string>();
                IPartRepository partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
                IList<string> valueList = new List<string>();
                valueList = partRepository.GetValueFromSysSettingByName(name);
                if (valueList.Count == 0)
                {
                    FisException ex;
                    ex = new FisException("CHK021", erpara);
                    //ex.logErr("", "", "", "", "83");
                    //logger.Error(ex);
                    throw ex;
                }
                return valueList[0];
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
                logger.Debug("(PDPALabel02)GetSysSetting, name:" + name);
            }

        }

        public ArrayList GetSysSettingList(IList<string> nameList)
        {
            try
            {
                List<string> erpara = new List<string>();
                IPartRepository partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
                ArrayList retList = new ArrayList();

                foreach (string node in nameList)
                {
                    IList<string> valueList = new List<string>();
                    valueList = partRepository.GetValueFromSysSettingByName(node);
                    if (valueList.Count == 0)
                    {
                        retList.Add("");
                    }
                    else
                    {
                        retList.Add(valueList[0]);
                    }
                }
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
                logger.Debug("(PDPALabel02)GetSysSetting, name:");
            }

        }

        #region "methods do not interact with the running workflow"
        /// <summary>
        /// 获取setting相关值，未得到则置default值
        /// </summary>
        /// <param name="name"></param>
        /// <param name="defValue"></param>
        /// <param name="hostname"></param>
        /// <param name="editor"></param>
        /// <returns></returns>
        public string GetSysSettingSafe(string name, string defValue, string hostname, string editor)
        {
            try
            {
                List<string> erpara = new List<string>();
                IPartRepository partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
                IList<string> valueList = new List<string>();
                valueList = partRepository.GetValueFromSysSettingByName(name);
                if (valueList.Count == 0)
                {
                    SysSettingInfo info = new SysSettingInfo();
                    info.name = name;
                    info.value = defValue;
                    info.description = "PCs in Carton";

                    partRepository.AddSysSettingInfo(info);

                    valueList.Add(defValue);
                }
                return valueList[0];
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
                logger.Debug("(CombinPoInCarton)GetSysSetting, name:" + name);
            }
        }

        public void SetSysSetting(string name, string value, string hostname, string editor)
        {
            try
            {
                List<string> erpara = new List<string>();
                IPartRepository partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();

                SysSettingInfo conf = new SysSettingInfo();
                SysSettingInfo info = new SysSettingInfo();
                
                info.value = value;
                info.description = "PCs in Carton";

                conf.name = name;

                partRepository.UpdateSysSettingInfo(info,conf);               
                return ;
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
                logger.Debug("(CombinPoInCarton)GetSysSetting, name:" + name);
            }
        }
        #endregion
    }
}
