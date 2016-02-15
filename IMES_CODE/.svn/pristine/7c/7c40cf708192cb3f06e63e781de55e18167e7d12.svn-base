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
using IMES.FisObject.FA.Product;
using IMES.FisObject.PAK.DN;
using IMES.FisObject.PAK.Pizza;
using IMES.FisObject.PAK.Pallet;
using IMES.FisObject.Common.Part;


namespace IMES.Station.Implementation
{
    /// <summary>
    /// </summary>
    public partial class ShipToCartonLabel : MarshalByRefObject, IShipToCartonLabel
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //private const int DEFINE_DN_LEN = 13;
        private const int DEFINE_DN_LEN = 16;
        private const int DEFINE_SN_LEN = 10;

        public IList<string> WFStart(string pdLine, string station, string editor, string customer)
        {
            logger.Debug("ShipToCartonLabel WF start, pdLine:" + pdLine + " ,editor:" + editor + " ,station:" + station + " ,customer:" + customer);
            string currentSessionKey = Guid.NewGuid().ToString();
            try
            {
                Session currentCommonSession = SessionManager.GetInstance.GetSession(currentSessionKey, Session.SessionType.Common);
                if (currentCommonSession == null)
                {
                    currentCommonSession = new Session(currentSessionKey, Session.SessionType.Common, editor, station, pdLine, customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", currentSessionKey);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", currentCommonSession);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", pdLine);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", Session.SessionType.Common);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow("ShipToCartonLabel.xoml", "ShipToCartonLabel.rules", wfArguments);

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
                IList<string> resList = new List<string>();

                resList.Add(currentSessionKey);

                return resList;
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
                logger.Debug("ShipToCartonLabel end, pdLine:" + pdLine + " ,editor:" + editor + " ,station:" + station + " ,customer:" + customer);

            }

        }

        public void WFCancel(string sessionKey)
        {
            try
            {
                logger.Debug("ShipToCartonLabel Cancel start, sessionKey:" + sessionKey);
                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, Session.SessionType.Common);
                if (currentSession != null)
                {
                    currentSession.AddValue(Session.SessionKeys.IsComplete, true);
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
                logger.Debug("ShipToCartonLabel Cancel end, sessionKey:" + sessionKey);
            }
        }

        #region IShipToCartonLabel Members

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="type"></param>
        /// <param name="sessionKey"></param>
        /// <param name="printItemLst"></param>
        /// <param name="pdLine"></param>
        /// <param name="station"></param>
        /// <param name="editor"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        public ArrayList InputProcess(string data, int type, string sessionKey, IList<PrintItem> printItemLst, string pdLine, string station, string editor, string customer)
        {
            logger.Debug("ShipToCartonLabel WF start, pdLine:" + pdLine + " ,editor:" + editor + " ,station:" + station + " ,customer:" + customer);
            string currentSessionKey = Guid.NewGuid().ToString();
            ArrayList retValue = new ArrayList();

            try
            {
                Session currentCommonSession = SessionManager.GetInstance.GetSession(currentSessionKey, Session.SessionType.Common);
                if (currentCommonSession == null)
                {
                    currentCommonSession = new Session(currentSessionKey, Session.SessionType.Common, editor, station, pdLine, customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", currentSessionKey);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", currentCommonSession);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", pdLine);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", Session.SessionType.Common);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow("ShipToCartonLabel.xoml", "ShipToCartonLabel.rules", wfArguments);
                    
                    if (data.Length == DEFINE_DN_LEN)
                    {
                        currentCommonSession.AddValue(Session.SessionKeys.MaintainAction, 0);
                        currentCommonSession.AddValue(Session.SessionKeys.DeliveryNo, data);
                    }
                    else if (data.Length == DEFINE_SN_LEN)
                    {
                        currentCommonSession.AddValue(Session.SessionKeys.MaintainAction, 1);
                        currentCommonSession.AddValue(Session.SessionKeys.CustSN, data);

                        currentCommonSession.AddValue(Session.SessionKeys.PrintItems, printItemLst);
                        currentCommonSession.AddValue(Session.SessionKeys.CN, type);
                    }
                    else
                    {
                        FisException ex;
                        List<string> erpara = new List<string>();
                        erpara.Add(data);
                        ex = new FisException("CHK837", erpara);
                        throw ex;
                    }


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

                int changeLabel = 0;
                if ((int)currentCommonSession.GetValue(Session.SessionKeys.MaintainAction) == 0)
                {
                    changeLabel = 0;
                }
                else
                {
                    changeLabel = (int)currentCommonSession.GetValue(Session.SessionKeys.labelBranch);
                }

                if (changeLabel == 1 && type == 0)
                {
                    retValue.Add(currentSessionKey);
                }
                else
                {
                    //Delivery Info
                    Delivery currentDelivery = (Delivery)currentCommonSession.GetValue(Session.SessionKeys.Delivery);
                    DNForUI item = new DNForUI();
                    item.ModelName = currentDelivery.ModelName;
                    item.PoNo = currentDelivery.PoNo;
                    item.DeliveryNo = currentDelivery.DeliveryNo;
                    item.Qty = currentDelivery.Qty;
                    item.DeliveryInfo = new Dictionary<string, string>();
                    string t = (string)currentDelivery.GetExtendedProperty("PartNo");
                    if (!String.IsNullOrEmpty(t) && t.Contains('/'))
                    {
                        string[] tmpPartNo = t.Split(new Char[] {'/'} ,2);
                        item.DeliveryInfo.Add("PartNo", tmpPartNo[1]);
                        retValue.Add(tmpPartNo[1]);
                    }
                    else
                    {
                        item.DeliveryInfo.Add("PartNo", "");
                        retValue.Add("");
                    }
                    //Delivery Info
                    //Location Info And Pallet Info
                    string message = string.Empty;
                    string pallet = string.Empty;
                    if ((int)currentCommonSession.GetValue(Session.SessionKeys.MaintainAction) == 1)
                    {
                        message = (string)currentCommonSession.GetValue("LocationMess");
                        pallet = (string)currentCommonSession.GetValue(Session.SessionKeys.Pallet);
                    }
                    else
                    {
                        message = "";
                        pallet = "";
                    }
                    //Location Info And Pallet Info
                    IList<PrintItem> printParams = new List<PrintItem>();
                    if ((int)currentCommonSession.GetValue(Session.SessionKeys.MaintainAction) == 1)
                    {
                         printParams = (IList<PrintItem>)currentCommonSession.GetValue(Session.SessionKeys.PrintItems);
                    }                    

                    int printType = 0;
                    int actionType = 0;
                    if ((int)currentCommonSession.GetValue(Session.SessionKeys.MaintainAction) == 1)
                    {
                        printType = (int)currentCommonSession.GetValue(Session.SessionKeys.InfoValue);
                        actionType = (int)currentCommonSession.GetValue(Session.SessionKeys.MaintainAction);
                    }
                    else
                    {
                        actionType = (int)currentCommonSession.GetValue(Session.SessionKeys.MaintainAction);
                        printType = 2;
                    }

                    string pdfname = "";
                    if ((int)currentCommonSession.GetValue(Session.SessionKeys.MaintainAction) == 1)
                    {
                        if ((int)currentCommonSession.GetValue(Session.SessionKeys.InfoValue) == 0)
                        {
                            pdfname = (string)currentCommonSession.GetValue("PDFFileName");
                        }
                    }


                    retValue.Add(item.ModelName);
                    retValue.Add(item.PoNo);
                    retValue.Add(item.DeliveryNo);
                    retValue.Add(item.Qty.ToString());
                    retValue.Add(printType.ToString());
                    retValue.Add(printParams);
                    retValue.Add(message);
                    retValue.Add(pallet);
                    retValue.Add(actionType.ToString());
                    retValue.Add(changeLabel.ToString());
                    retValue.Add(pdfname);
                }

                return retValue;

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
                logger.Debug("ShipToCartonLabel end, pdLine:" + pdLine + " ,editor:" + editor + " ,station:" + station + " ,customer:" + customer);

            }
         
        }

        public ArrayList ChangePrintLabel(string key)
        {
            logger.Debug("(ShipToCartonLabel)ChangePrintLabel start, [key]:" + key);
            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = key;
            ArrayList retValue = new ArrayList();

            try
            {
                Session currentCommonSession = SessionManager.GetInstance.GetSession(sessionKey, Session.SessionType.Common);
                if (currentCommonSession == null)
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK021", erpara);                    
                    throw ex;
                }
                else
                {
                    currentCommonSession.Exception = null;
                    currentCommonSession.SwitchToWorkFlow();

                    //check workflow exception
                    if (currentCommonSession.Exception != null)
                    {
                        if (currentCommonSession.GetValue(Session.SessionKeys.WFTerminated) != null)
                        {
                            currentCommonSession.ResumeWorkFlow();
                        }

                        throw currentCommonSession.Exception;
                    }

                    //Delivery Info
                    Delivery currentDelivery = (Delivery)currentCommonSession.GetValue(Session.SessionKeys.Delivery);
                    DNForUI item = new DNForUI();
                    item.ModelName = currentDelivery.ModelName;
                    item.PoNo = currentDelivery.PoNo;
                    item.DeliveryNo = currentDelivery.DeliveryNo;
                    item.Qty = currentDelivery.Qty;
                    item.DeliveryInfo = new Dictionary<string, string>();
                    string t = (string)currentDelivery.GetExtendedProperty("PartNo");
                    if (!String.IsNullOrEmpty(t) && t.Contains('/'))
                    {
                        string[] tmpPartNo = t.Split(new Char[] {'/'}, 2);
                        item.DeliveryInfo.Add("PartNo", tmpPartNo[1]);
                        retValue.Add(tmpPartNo[1]);
                    }
                    else
                    {
                        item.DeliveryInfo.Add("PartNo", "");
                        retValue.Add("");
                    }
                    //Delivery Info
                    //Location Info And Pallet Info
                    string message = string.Empty;
                    string pallet = string.Empty;
                    if ((int)currentCommonSession.GetValue(Session.SessionKeys.MaintainAction) == 1)
                    {
                        message = (string)currentCommonSession.GetValue("LocationMess");
                        pallet = (string)currentCommonSession.GetValue(Session.SessionKeys.Pallet);
                    }
                    else
                    {
                        message = "";
                        pallet = "";
                    }
                    //Location Info And Pallet Info
                    IList<PrintItem> printParams = (IList<PrintItem>)currentCommonSession.GetValue(Session.SessionKeys.PrintItems);

                    int printType = 0;
                    int actionType = 0;
                    if ((int)currentCommonSession.GetValue(Session.SessionKeys.MaintainAction) == 1)
                    {
                        printType = (int)currentCommonSession.GetValue(Session.SessionKeys.InfoValue);
                        actionType = (int)currentCommonSession.GetValue(Session.SessionKeys.MaintainAction);
                    }
                    else
                    {
                        actionType = (int)currentCommonSession.GetValue(Session.SessionKeys.MaintainAction);
                        printType = 0;
                    }

                    int changeLabel = (int)currentCommonSession.GetValue(Session.SessionKeys.labelBranch);

                    string pdfname = "";
                    if ((int)currentCommonSession.GetValue(Session.SessionKeys.MaintainAction) == 1)
                    {
                        if ((int)currentCommonSession.GetValue(Session.SessionKeys.InfoValue) == 0)
                        {
                            pdfname = (string)currentCommonSession.GetValue("PDFFileName");
                        }
                    }

                    retValue.Add(item.ModelName);
                    retValue.Add(item.PoNo);
                    retValue.Add(item.DeliveryNo);
                    retValue.Add(item.Qty.ToString());
                    retValue.Add(printType.ToString());
                    retValue.Add(printParams);
                    retValue.Add(message);
                    retValue.Add(pallet);
                    retValue.Add(actionType.ToString());
                    retValue.Add(changeLabel.ToString());
                    retValue.Add(pdfname);

                    return retValue;
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
                logger.Debug("(ShipToCartonLabel)ChangePrintLabel end, [key]:" + key);
            }
        }


        public IList<DNForUI> GetDNListByUI(string dn)
        {
            IList<DNForUI> dnUIList = new List<DNForUI>();
            IList<Delivery> dnList = new List<Delivery>();
            DateTime beginTime = DateTime.Now;
            string[] statuses = { "00", "87" };

            IDeliveryRepository DeliveryRepository =
                    RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();

            beginTime = beginTime.AddDays(-2);
            dnList = DeliveryRepository.GetDeliveryListByStatusesAndModel(beginTime, statuses, "PC", 12);
            //ForTest
            /*Delivery d1 = new Delivery();
            d1.ModelName = "d1Model";
            d1.PoNo = "d1PoNo";
            d1.ShipDate = beginTime;
            d1.DeliveryNo = "d1DeliveryNo";
            d1.Qty = 123;
            dnList.Add(d1);
             */
            //ForTest

            foreach (Delivery temp in dnList)
            {
                DNForUI item = new DNForUI();

                item.ModelName = temp.ModelName;

                //Customer P/N
                item.DeliveryInfo = new Dictionary<string, string>();
                string partProperty = (string)temp.GetExtendedProperty("PartNo");
                if (!String.IsNullOrEmpty(partProperty) && partProperty.Contains('/'))
                {
                    string[] tmpPartNo = partProperty.Split(new Char[] {'/'}, 2);
                    item.DeliveryInfo.Add("PartNo", tmpPartNo[1]);                    
                }
                else
                {
                    item.DeliveryInfo.Add("PartNo", "");
                }
                //Customer P/N
                               
                item.PoNo = temp.PoNo;                
                item.ShipDate = temp.ShipDate;
                item.DeliveryNo = temp.DeliveryNo;
                item.Qty = temp.Qty;

                dnUIList.Add(item);
            }
            return dnUIList;
        }

        public IList<DeliveryPalletInfo> GetPalletInfoByDN(string dn)
        {
            IList<DeliveryPalletInfo> palletList = new List<DeliveryPalletInfo>();
            IDeliveryRepository DeliveryRepository =
                    RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();


            palletList = DeliveryRepository.GetDeliveryPalletListOrderByUnitQty(dn);

            //ForTest
            /*
            DeliveryPalletInfo info = new DeliveryPalletInfo();
            DeliveryPalletInfo info1 = new DeliveryPalletInfo();
            info.deliveryNo = "d1DeliveryNo";
            info.deliveryQty = 1;
            info.id = 1;
            info.palletNo = "palletNo1";
            palletList.Add(info);
            info1.deliveryNo = "d1DeliveryNo";
            info1.deliveryQty = 2;
            info1.id = 2;
            info1.palletNo = "palletNo2";
            palletList.Add(info1);
            */
            //ForTest
            return palletList;
        }
        public IList<ProductModel> GetProductByPallet(string plt)
        {
            IList<ProductModel> proList = new List<ProductModel>();
            IProductRepository productRepository =
                    RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();




            proList = productRepository.GetProductListByPalletNo(plt);
            //ITC-1360-1350:用Model字段暂存CartonSN
            IProduct currentProduct = null;
            foreach (ProductModel temp in proList)
            {
                currentProduct = productRepository.Find(temp.ProductID);
                if (currentProduct == null)
                {
                    temp.Model = "";
                }
                else
                {
                    temp.Model = currentProduct.CartonSN;
                }
            }
            //ForTest
            /*
            if (plt == "palletNo1")
            {
                ProductModel p1 = new ProductModel();
                ProductModel p2 = new ProductModel();

                p1.CustSN = plt + "CustSN1";
                p1.ProductID = plt + "ProductID1";
                p1.Model = plt + "Model1";
                p2.CustSN = plt + "CustSN2";
                p2.ProductID = plt + "ProductID2";
                p2.Model = plt + "Model2";
                proList.Add(p1);
                proList.Add(p2);
            }*/
            //FotTest

            return proList;
        }
        public int GetQtyByPallet(string plt)
        {
            IDeliveryRepository DeliveryRepository =
                    RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
            int qty = 0;

            qty = DeliveryRepository.GetSumofDeliveryQtyFromDeliveryPallet(plt);
            //qty = 10;

            return qty;
        }
        public int GetScanQtyByPallet(string plt)
        {
            int scanQty = 0;
            IPalletRepository palletRepository = 
                    RepositoryFactory.GetInstance().GetRepository<IPalletRepository, Pallet>();
            scanQty = palletRepository.GetCountOfBoundProduct(plt);
            //scanQty = 100;
            return scanQty;
        }


        public string GetMessageByPalletAndLocation(string deliveryNo, string pallet)
        {
            IDeliveryRepository DeliveryRepository =
                    RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
            IPalletRepository palletRepository = 
                    RepositoryFactory.GetInstance().GetRepository<IPalletRepository, Pallet>();

            Delivery DeliveryObject = new Delivery();

            DeliveryObject = DeliveryRepository.Find(deliveryNo);
            string strCarrier = string.Empty;
            string strLocation = string.Empty;
            string strPalletQty = string.Empty;

            strCarrier = (string)DeliveryObject.GetExtendedProperty("Carrier");
            //strLocation = palletRepository.GetSnoIdFromPakLocMasByPno(pallet);
            strLocation = "MyTestForLocation";
            if (strLocation == "" && pallet.Substring(0, 2) != "BA" && pallet.Substring(0, 2) != "NA")
            {
                strLocation = "Other";
            }
            else if (pallet.Substring(0, 2) == "NA")
            {
                strLocation = "Others";
            }
            else if (pallet.Substring(0, 2) == "BA")
            {
                strLocation = strCarrier.TrimEnd();
            }

            
            strPalletQty = (string)DeliveryObject.GetExtendedProperty("PalletQty");
            if (strPalletQty == "")
            {
                strPalletQty = "60";
            }
            else
            {
            }

            int iPalletTotal = 10;
            iPalletTotal = DeliveryRepository.GetSumDeliveryQtyOfACertainPallet(pallet);
            int iPalletTier = 6;
            iPalletTier = palletRepository.GetTierQtyFromPalletQtyInfo(iPalletTotal.ToString());

            string strShipWay = string.Empty;
            string strRegId = string.Empty;
            string strRegIdM = string.Empty;

            strShipWay = (string)DeliveryObject.GetExtendedProperty("ShipWay");
            strRegId = (string)DeliveryObject.GetExtendedProperty("RegId");
            strRegIdM = (string)DeliveryObject.GetExtendedProperty("RegIdM");

            strShipWay = "T001";
            strRegId = "SAF";

            string strMessage = string.Empty;
            if (strLocation == "")
            {
                strMessage = "此栈板上机器数量小於等於6台,未分配库位";
            }
            else
            {
                if (iPalletTier == 0)
                {
                    strMessage = "请搬入" + strLocation +  "IE未maintain此种栈板的一层数量";
                }
                else if (strShipWay == "T002" && strRegId != "SNE" && strRegIdM != "SCE" && iPalletTotal >= iPalletTier)
                {
                    strMessage = "请搬入" + strLocation + "海運，滿一層請使用大的木頭棧板";
                }
                else if (strShipWay == "T002" && (strRegId == "SNE" || strRegIdM == "SCE"))
                {
                    strMessage = "请搬入" + strLocation + "EMEA海運，請使用大的木頭棧板";
                }
                else if (strShipWay == "T001" && strRegId == "SNL")
                {
                    strMessage = "请搬入" + strLocation + "滿一層請使用綠色塑料棧板";
                }
                else if (strShipWay == "T001" && (strRegId == "SNE" || strRegIdM == "SCE") && iPalletTotal >= iPalletTier)
                {
                    strMessage = "请搬入" + strLocation + "滿一層請使用chep棧板";
                }
                else if (strShipWay == "T001" && (strRegId == "SNU" || strRegIdM == "SCU") && iPalletTotal >= iPalletTier)
                {
                    strMessage = "请搬入" + strLocation + "滿一層請使用藍色塑料棧板";
                }
                else if (strShipWay == "T001" && strRegId == "SAF" && iPalletTotal >= iPalletTier)
                {
                    strMessage = "请搬入" + strLocation + "滿一層請使用新的木头棧板";
                }
                else if (strShipWay == "T001" && strRegId == "SCN" && iPalletTotal >= iPalletTier)
                {
                    strMessage = "请搬入" + strLocation + "滿一層請使用大的木头棧板";
                }
                else
                {
                    strMessage = "请搬入" + strLocation;
                }
            }


            return strMessage;
        }



        public ArrayList Reprint(string prodid, int type, string editor, string station, string customer, string reason, string pCode, IList<PrintItem> printItems)
        {
            ArrayList retrunValue = new ArrayList();
            FisException ex;
            List<string> erpara = new List<string>();           
            try
            {
                logger.Debug("(ShipToCartonLabel)ReprintLabel start, ProdId:" + prodid + " editor:" + editor + " customerId:" + customer + " reason:" + reason);
                         
                string sessionKey = prodid;
                Session Session = SessionManager.GetInstance.GetSession(sessionKey, Session.SessionType.Product);
                if (Session == null)
                {
                    Session = new Session(sessionKey, Session.SessionType.Product, editor, station, "", customer);
                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();

                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", Session);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", "");
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", Session.SessionType.Product);
                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(station, "ReprintShipToCartonLabel.xoml", "ReprintShipToCartonLabel.rules", out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    Session.AddValue(Session.SessionKeys.CustSN, prodid);
                    Session.AddValue(Session.SessionKeys.PalletNo, prodid);
                    Session.AddValue(Session.SessionKeys.Reason, reason);
                    Session.AddValue(Session.SessionKeys.PrintItems, printItems);
                    Session.AddValue(Session.SessionKeys.PCode, pCode); 
                   
                    Session.AddValue(Session.SessionKeys.PrintLogName, "Shipto Label");
                    Session.AddValue(Session.SessionKeys.PrintLogBegNo, prodid);
                    Session.AddValue(Session.SessionKeys.PrintLogEndNo, prodid);                               
                    Session.AddValue(Session.SessionKeys.PrintLogDescr, prodid);                               
                    Session.AddValue(Session.SessionKeys.Reason, reason);

                    Session.AddValue(Session.SessionKeys.CN, type);

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
                //ChangeLabel
                int changeLabel = (int)Session.GetValue(Session.SessionKeys.labelBranch);

                if (changeLabel == 1 && type == 0)
                {
                    retrunValue.Add("1");
                    retrunValue.Add(sessionKey);                    
                }
                else
                {
                    IList<PrintItem> returnList = (IList<PrintItem>)Session.GetValue(Session.SessionKeys.PrintItems);                    

                    int printType = (int)Session.GetValue(Session.SessionKeys.InfoValue);
                    string pdfname = "";
                    string templatename = "";
                    if (printType == 0)
                    {
                        pdfname = (string)Session.GetValue("PDFFileName");
                        templatename = (string)Session.GetValue("ShiptoTemplate");
                    }

                    //Mantis 972
                    string returnCustSN = (string)Session.GetValue(Session.SessionKeys.CustSN);
                    Delivery currentDelivery = (Delivery)Session.GetValue(Session.SessionKeys.Delivery);
                    string returnDN = currentDelivery.DeliveryNo;
                    int BTFlag = (int)Session.GetValue(Session.SessionKeys.IsBT);


                    retrunValue.Add("0");
                    retrunValue.Add(returnList);
                    retrunValue.Add(changeLabel.ToString());
                    retrunValue.Add(printType.ToString());
                    retrunValue.Add(pdfname);

                    //Mantis 972
                    retrunValue.Add(returnCustSN);
                    retrunValue.Add(returnDN);
                    retrunValue.Add(templatename);
                    retrunValue.Add(BTFlag.ToString());
                }

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
                logger.Debug("(ShipToCartonLabel)ReprintLabel end,:" + "ProdId:" + prodid + " editor:" + editor + " station:" + station + " customerId:" + customer + " reason:" + reason);

            }
        }


        public ArrayList ChangeRePrintLabel(string key)
        {
            logger.Debug("(ShipToCartonLabel Reprint)ChangeRePrintLabel start, [key]:" + key);
            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = key;
            ArrayList retValue = new ArrayList();

            try
            {
                Session currentProductSession = SessionManager.GetInstance.GetSession(sessionKey, Session.SessionType.Product);
                if (currentProductSession == null)
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK021", erpara);
                    throw ex;
                }
                else
                {
                    currentProductSession.Exception = null;
                    currentProductSession.SwitchToWorkFlow();

                    //check workflow exception
                    if (currentProductSession.Exception != null)
                    {
                        if (currentProductSession.GetValue(Session.SessionKeys.WFTerminated) != null)
                        {
                            currentProductSession.ResumeWorkFlow();
                        }

                        throw currentProductSession.Exception;
                    }
                    int changeLabel = (int)currentProductSession.GetValue(Session.SessionKeys.labelBranch);
                    IList<PrintItem> returnList = (IList<PrintItem>)currentProductSession.GetValue(Session.SessionKeys.PrintItems);

                    int printType = (int)currentProductSession.GetValue(Session.SessionKeys.InfoValue);
                    string pdfname = "";
                    string templatename = "";
                    if (printType == 0)
                    {
                        pdfname = (string)currentProductSession.GetValue("PDFFileName");
                        templatename = (string)currentProductSession.GetValue("ShiptoTemplate");
                    }
                    //Mantis 972
                    string returnCustSN = (string)currentProductSession.GetValue(Session.SessionKeys.CustSN);
                    Delivery currentDelivery = (Delivery)currentProductSession.GetValue(Session.SessionKeys.Delivery);
                    string returnDN = currentDelivery.DeliveryNo;
                    int BTFlag = (int)currentProductSession.GetValue(Session.SessionKeys.IsBT);

                    retValue.Add("0");
                    retValue.Add(returnList);
                    retValue.Add(changeLabel.ToString());
                    retValue.Add(printType.ToString());
                    retValue.Add(pdfname);

                    //Mantis 972
                    retValue.Add(returnCustSN);
                    retValue.Add(returnDN);
                    retValue.Add(templatename);
                    retValue.Add(BTFlag.ToString());

                    return retValue;
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
                logger.Debug("(ShipToCartonLabel Reprint)ChangeRePrintLabel end, [key]:" + key);
            }
        }

        public IList<string> GetEditAddr(string name)
        {
            IList<string> retList = new List<string>();

            IPartRepository partRep = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            retList = partRep.GetValueFromSysSettingByName(name);

            return retList;
        }


        #endregion
    }
}
