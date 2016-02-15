﻿/*
 * INVENTEC corporation (c)2011all rights reserved. 
 * Description: PAK UnitWeight interface implement
 * UI:CI-MES12-SPEC-PAK-UI Unit Weight.docx
 * UC:CI-MES12-SPEC-PAK-UC Unit Weight.docx                 
 * Update: 
 * Date       Name                  Reason 
 * ========== ===================== =====================================
 * 2011-11-29 Chen Xu itc208014     Create 
 * Known issues:
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Workflow.Runtime;
using IMES.DataModel;
using IMES.FisObject.Common.Model;
using IMES.FisObject.Common.Part;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PAK.DN;
using IMES.FisObject.PAK.StandardWeight;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.Route;
using IMES.Station.Interface.StationIntf;
using log4net;
using IProduct = IMES.FisObject.FA.Product.IProduct;
using ModelInfo = IMES.FisObject.Common.Model.ModelInfo;
using IMES.FisObject.Common.FisBOM;
using IMES.Infrastructure.Repository.Common;
using IMES.FisObject.PAK.Pallet;
using IMES.Infrastructure.UnitOfWork;
using IMES.FisObject.PAK.Pizza;
using System.Data;
using System.Linq;

namespace IMES.Station.Implementation
{
    /// <summary>
    ///  UC 具体业务：  本站站号：85
    ///                1. Unit 称重；
    ///                2. Print Config Label; Print POD Label
    ///                3. 上传数据至SAP
    /// </summary>
    public class PakUnitWeight : MarshalByRefObject, IPakUnitWeight
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region GetCOMSettingInfo

        /// <summary>
        /// 获取COMSettingInfo
        /// </summary>
        /// <param name="hostname">hostname</param>
        /// <returns></returns>
        public IList<COMSettingDef> GetWeightSettingInfo(string hostname)
        {
            IPalletRepository iPalletRepository = RepositoryFactory.GetInstance().GetRepository<IPalletRepository, Pallet>();
            IList<COMSettingInfo> UnitWeighSettingInfo = new List<COMSettingInfo>();
            UnitWeighSettingInfo = iPalletRepository.FindCOMSettingByName(hostname);


            IList<COMSettingDef> UnitWeighInfoList = new List<COMSettingDef>();
            foreach (COMSettingInfo wsInfo in UnitWeighSettingInfo)
            {
                COMSettingDef wsd = new COMSettingDef();
                wsd.id = wsInfo.id;
                wsd.name = wsInfo.name;
                wsd.commport = wsInfo.commPort;
                wsd.baudRate = wsInfo.baudRate;
                wsd.rthreshold = wsInfo.rthreshold.ToString();
                wsd.sthreshold = wsInfo.sthreshold.ToString();
                wsd.handshaking = wsInfo.handshaking.ToString();
                wsd.editor = wsInfo.editor;
                wsd.cdt = wsInfo.cdt.ToString("yyyy-MM-dd hh:mm:ss");
                wsd.udt = wsInfo.udt.ToString("yyyy-MM-dd hh:mm:ss");

                UnitWeighInfoList.Add(wsd);

            }
            if (UnitWeighInfoList == null || UnitWeighInfoList.Count <= 0)
            {
                return null;
            }
            return UnitWeighInfoList;
        }

        #endregion

        #region PakUnitWeight

        /// <summary>
        /// 此站输入的是SN，需要在BLL中先根据SN获取Product调用CommonImpl.GetProductByInput()
        /// 如果获取不到，报CHK079！
        /// 用ProductID启动工作流
        /// 将获得的Product放到Session.Product中
        /// 获取Model和标准重量和ProductID
        /// </summary>
        /// <param name="custSN"></param>
        /// <param name="actualWeight"></param>
        /// <param name="line"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <param name="configParams"></param>
        /// <returns>ArrayList对象</returns>
        public ArrayList InputUUT(string custSN, decimal actualWeight, string line, string editor, string station, string customer, out List<string> configParams)
        {
            logger.Debug("(UnitWeight)InputUUT Start,"
                + " [custSN]:" + custSN
                + " [line]:" + line
                + " [editor]:" + editor
                + " [station]:" + station
                 + " [actualWeight]:" + actualWeight.ToString()
                + " [customer]:" + customer);
            FisException ex;
            List<string> erpara = new List<string>();
            try
            {
                ArrayList retLst = new ArrayList();
                var currentProduct = CommonImpl.GetProductByInput(custSN, CommonImpl.InputTypeEnum.CustSN);
                if (currentProduct == null)
                {
                    FisException fe = new FisException("CHK079", new string[] { custSN });
                    throw fe;
                }
                string productID = currentProduct.ProId;
                string sessionKey = productID;
                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, Session.SessionType.Product);

                //Mantis 961
                //获取Pallet No : IMES_FA..Product.PalletNo
                if (!currentProduct.IsBT && string.IsNullOrEmpty(currentProduct.PalletNo) && currentProduct.Model.Substring(0, 3) != "146")
                {
                    erpara.Add(custSN);
                    ex = new FisException("PAK021", erpara);     //该Customer S/N还未与Pallet绑定!
                    throw ex;
                }


                if (currentSession == null)
                {
                    currentSession = new Session(sessionKey, Session.SessionType.Product, editor, station, line, customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", currentSession);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", line);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", Session.SessionType.Product);

                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(station, "PakUnitWeight.xoml", "PakUnitWeight.rules", out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);
                    currentSession.AddValue(Session.SessionKeys.Product, currentProduct);
                    currentSession.AddValue(Session.SessionKeys.ActuralWeight, actualWeight);
                    currentSession.AddValue(Session.SessionKeys.PalletNo, currentProduct.PalletNo);
                    currentSession.AddValue(Session.SessionKeys.IsComplete, false);
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

                string modelTolerance = currentSession.GetValue(Session.SessionKeys.Tolerance).ToString();
                decimal standardWeight = -1;
                if (currentSession.GetValue(Session.SessionKeys.StandardWeight) == null)
                {
                    standardWeight = -1;
                }
                else
                {
                    standardWeight = (decimal)currentSession.GetValue(Session.SessionKeys.StandardWeight);
                }

                //PAQC 抽检: 查询IMES_FA..QCStatus 取Udt 最新的记录，如果该记录的Status = ‘8’，则在UI 上显示'PAQC 抽检'，否则该位置显示' '
                string labelType1 = string.Empty;

                IDeliveryRepository iDeliveryRepository = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
                ProductQCStatus qcStatus = iDeliveryRepository.GetQCStatus(productID, "PAQC");
                //Vincent for ICC special PAQC/PAQC
                if (qcStatus != null && (qcStatus.Status == "8" || qcStatus.Status == "B" || qcStatus.Status == "C"))
                {
                    labelType1 = "P" + qcStatus.Status;
                }

                //1.判断是否为BT Product, 2.于非BT Product
                string delievery = string.Empty;    // currentDeliveryNo
                string plt = string.Empty;      // currentPalletNo
                string pno = string.Empty;      // currentModel
                string country = string.Empty;
                string jcid = string.Empty;     //configID
                string flg = string.Empty;    // Flag
                string printlabeltype1 = string.Empty; // Config Label
                string printlabeltype2 = string.Empty; // POD Label

                if (!currentProduct.IsBT)
                {
                    //获取与Customer S/N 绑定的Delivery No :IMES_FA..Product.DeliveryNo
                    if (string.IsNullOrEmpty(currentProduct.DeliveryNo))
                    {
                        erpara.Add(custSN);
                        ex = new FisException("PAK020", erpara);    //该Customer S/N还未与DN绑定!
                        throw ex;
                    }
                    delievery = currentProduct.DeliveryNo;

                    //获取Pallet No : IMES_FA..Product.PalletNo
                    //if (string.IsNullOrEmpty(currentProduct.PalletNo))
                    //{
                    //    erpara.Add(custSN);
                    //    ex = new FisException("PAK021", erpara);     //该Customer S/N还未与Pallet绑定!
                    //    throw ex;
                    //}
                    plt = currentProduct.PalletNo;

                    //获取Model : IMES_FA..Product.Model
                    //if (string.IsNullOrEmpty(currentProduct.Model))
                    //{
                    //    erpara.Add(custSN);
                    //    ex = new FisException("PAK028", erpara);     //该Customer S/N还未与Model绑定!
                    //    throw ex;
                    //}
                    // pno = currentProduct.Model;

                    if (string.IsNullOrEmpty(currentProduct.Model))
                    {
                        pno = "";
                    }
                    else
                    {
                        pno = currentProduct.Model.Trim();
                    }

                    //获取Delivery 的Country / configID / Flag 
                    country = iDeliveryRepository.GetDeliveryInfoValue(delievery, "Country");
                    if (string.IsNullOrEmpty(country))
                    {
                        //List<string> errpara = new List<string>();
                        //errpara.Add(delievery);
                        //errpara.Add("Country");
                        //throw new FisException("PAK029", errpara);

                        country = "";
                    }
                    jcid = iDeliveryRepository.GetDeliveryInfoValue(delievery, "ConfigID");
                    if (string.IsNullOrEmpty(jcid))   //ITC-1360-1222 取消报错！
                    {
                        //List<string> errpara = new List<string>();
                        //errpara.Add(delievery);
                        //errpara.Add("configID");
                        //throw new FisException("PAK029", errpara);

                        jcid = "";
                    }
                    flg = iDeliveryRepository.GetDeliveryInfoValue(delievery, "Flag");
                    if (string.IsNullOrEmpty(flg))
                    {
                        //List<string> errpara = new List<string>();
                        //errpara.Add(delievery);
                        //errpara.Add("Flag");
                        //throw new FisException("PAK029", errpara);

                        flg = "";
                    }
                }
                else
                {
                    //获取Model : IMES_FA..Product.Model
                    if (string.IsNullOrEmpty(currentProduct.Model))
                    {
                        erpara.Add(custSN);
                        ex = new FisException("PAK028", erpara);     //该Customer S/N还未与Model绑定!
                        throw ex;
                    }
                    pno = currentProduct.Model;

                }
               
                IProductRepository iproductRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();

                // 3. 是否有与当前Product 绑定的BoxID或UCC:（IMES_FA..ProductInfo.InfoValue，Condition: InfoType = 'BoxId'）或UCC （IMES_FA..ProductInfo.InfoValue，Condition: InfoType = 'UCC'）
                ////---- UC Update : SVN Revison: 11531 取消BoxId/UCC唯一性检查----
                //int bindQty1 = 0;
                //int bindQty2 = 0;
                ////bindQty1 = iproductRepository.GetProductInfoCountByInfoType("BoxId");
                ////bindQty2 = iproductRepository.GetProductInfoCountByInfoType("UCC");
                //string PrdInfo_BoxID_Value = iproductRepository.GetProductInfoValue(productID, "BoxId");
                //string PrdInfo_UCC_Value = iproductRepository.GetProductInfoValue(productID, "UCC");

                //if (!String.IsNullOrEmpty(PrdInfo_BoxID_Value))
                //{
                //    IProduct product = null;
                //    product = (IProduct)currentSession.GetValue(Session.SessionKeys.Product);
                //    IList<IMES.FisObject.FA.Product.ProductInfo> infos = new List<IMES.FisObject.FA.Product.ProductInfo>();
                //    infos = product.ProductInfoes;

                //    foreach (IMES.FisObject.FA.Product.ProductInfo iInfo in infos)
                //    {
                //        if (iInfo.InfoValue == PrdInfo_BoxID_Value)
                //        {
                //            bindQty1++;
                //        }
                //    }
                //    if (bindQty1 > 1)
                //    {
                //        erpara.Add("BoxId");
                //        ex = new FisException("PAK030", erpara);     //此机器BoxId与其他机器重复，请unpack后重流亮灯第二站！
                //        throw ex;
                //    }
                //}

                //if (!String.IsNullOrEmpty(PrdInfo_UCC_Value))
                //{
                //    IProduct product = null;
                //    product = (IProduct)currentSession.GetValue(Session.SessionKeys.Product);
                //    IList<IMES.FisObject.FA.Product.ProductInfo> infos = new List<IMES.FisObject.FA.Product.ProductInfo>();
                //    infos = product.ProductInfoes;

                //    foreach (IMES.FisObject.FA.Product.ProductInfo iInfo in infos)
                //    {
                //        if (iInfo.InfoValue == PrdInfo_UCC_Value)
                //        {
                //            bindQty2++;
                //        }
                //    }

                //    if (bindQty2 > 1)
                //    {
                //        erpara.Add("UCC");
                //        ex = new FisException("PAK030", erpara);     //此机器UCC与其他机器重复，请unpack后重流亮灯第二站！
                //        throw ex;
                //    }
                //}
                ////---- UC Update : SVN Revison: 11531 取消BoxId/UCC唯一性检查----


                // 4. Adaptor Label / India Label 判定
                string labelType2 = string.Empty;
                if (pno.Length >= 11 && (pno.Substring(9, 2) == "16" || pno.Substring(9, 2) == "DM" || pno.Substring(9, 2) == "D0"))
                {
                    labelType2 = "A";
                }
                else if (country == "INDIA")
                {
                    labelType2 = "I";
                }

                // 6. Get Asset Tag Item / Asset Tag Item Value 
                //  a) wc ="85"；@ast=''（Asset Tag Item Value）；@item='' (Asset Tag Item)
                // string wc = "85";
                string ast = string.Empty;  // Asset Tag Item Value
                string item = string.Empty; // Asset Tag Item
                string asttp = string.Empty;
                string astrm = string.Empty;
                string cust = string.Empty;

                //  b) 取ModelBOM 中Model 直接下阶中有BomNodeType 为'AT' 的Part 
                IPartRepository ipartRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
                IMES.FisObject.Common.FisBOM.IBOMRepository ibomRepository = RepositoryFactory.GetInstance().GetRepository<IMES.FisObject.Common.FisBOM.IBOMRepository>();
                IHierarchicalBOM sessionBOM = null;
                sessionBOM = ibomRepository.GetHierarchicalBOMByModel(currentProduct.Model);
                IList<IBOMNode> bomNodeLst = new List<IBOMNode>();
                
                bomNodeLst = sessionBOM.FirstLevelNodes;
                //For CQ
                CommonImpl2 cm2 = new CommonImpl2();
                string site = cm2.GetValueFromSysSetting("Site");
                if (site == "ICC")
                {
                    currentSession.AddValue("Site", "ICC");
                    currentSession.AddValue("BlackLabel",true);
                    IList<string> lstValue
                         =cm2.GetConstValueTypeByType("POD_White_Lable_PN").Where(x=>x.value!="").Select(x=>x.value).ToList();
                    if (lstValue != null && lstValue.Count > 0 )
                    {
                        foreach (IBOMNode ibomnode in bomNodeLst)
                        {
                            if (lstValue.Contains(ibomnode.Part.PN))
                            { currentSession.AddValue("BlackLabel", false); break; }

                        }
                    }
                }


                //For CQ



                if (bomNodeLst == null || bomNodeLst.Count <= 0)
                {
                    erpara.Add(currentProduct.Model);
                    ex = new FisException("PAK039", erpara);
                    throw ex;
                }
                IList<IPart> AT_PartList = new List<IPart>();
                foreach (IBOMNode ibomnode in bomNodeLst)
                {
                    //Revision: 9707:6. Get Asset Tag Item / Asset Tag Item Value 中b） “取AT Part 时需要按照Descr 逆序排取第一条”
                    IPart currentPart = ibomnode.Part;
                    if (currentPart.BOMNodeType == "AT")
                    {
                        //asttp = currentPart.Descr;  //IMES_GetData..Part.Descr
                        //astrm = currentPart.Remark; //IMES_GetData..Part.Remark
                        //break;
                        AT_PartList.Add(currentPart);
                    }
                }
                IPart BindCurrentPart;
               
            
                if (AT_PartList.Count > 0)
                {
                    asttp = AT_PartList[AT_PartList.Count - 1].Descr;  //逆序排取第一条 ==> 正序取最后一条
             
                    astrm = AT_PartList[AT_PartList.Count - 1].Remark; //逆序排取第一条 ==> 正序取最后一条
                    BindCurrentPart = AT_PartList[AT_PartList.Count - 1];
                }
                // ********************Begin New AST Check Rule ******************** Add by Benson
                string av="";
                IList<IPart> AST_Check = new List<IPart>();
                foreach (IPart iPart in AT_PartList)
                {
                 //    if (iPart.Descr == "ATSN1" || iPart.Descr == "ATSN4" || iPart.Descr == "ATS7" || iPart.Descr == "ATSN8")
                    if (iPart.Descr == "ATSN1" || iPart.Descr == "ATSN4")
                   
                    { AST_Check.Add(iPart); }
                }
                IList<IProductPart> productParts = new List<IProductPart>();
                productParts = currentProduct.ProductParts;
                if (AST_Check.Count > 0)
                {
                    if (productParts == null || productParts.Count <= 0)
                    {
                        List<string> errpara = new List<string>();
                        errpara.Add(productID);
                        throw new FisException("PAK038", errpara);  //该Product尚未绑定Part！
                    }
                    foreach (IPart iCheckPart in AST_Check)
                    {
                        asttp=iCheckPart.Descr;
                        if (asttp == "ATSN1")
                        { 
                           if(CheckHaveATS1(productParts,ipartRepository))
                           {
                               FisException fe = new FisException("PAK031", new string[] { });  //此机器未结合Asset Tag！
                               throw fe;
                           }
                        }
                        else if (asttp == "ATSN4")
                        { 
                              // Add by Benson for Mantis 0001723: weight站页面修改 at 2013/04/12
                              IList<ProductLog> logList=  iproductRepository.GetProductLogs(currentProduct.ProId, "PKAT");
                              if (logList == null || logList.Count == 0)
                              {
                                  throw new FisException("PAK180", new string[] { });
                              }
                              // Add by Benson for Mantis 0001723: weight站页面修改 at 2013/04/12
                             foreach (PartInfo ele in iCheckPart.Attributes)
                                {
                                    if (ele.InfoType == "AV")
                                    {
                                      av=ele.InfoValue;break;
                                    }
                                }
                              if(av=="")
                                {
                                    FisException fe = new FisException("PAK171", new string[] { iCheckPart.PN });  //请联系IE维护%1的AV值
                                    throw fe;
                                }
                          IList<AstRuleInfo> astrule = iproductRepository.GetAstRuleByCodeAndStationAndCust(asttp, station, av);

                          if (astrule != null && astrule.Count > 0 && !string.IsNullOrEmpty(astrule[0].checkItem))
                          {
                              item = astrule[0].checkItem;
                              switch (item)
                              {
                                  case "PoNo":
                                      Delivery CurrentDelivery = iDeliveryRepository.Find(delievery);
                                      if (CurrentDelivery != null && !string.IsNullOrEmpty(CurrentDelivery.PoNo))
                                      {
                                          ast = CurrentDelivery.PoNo;
                                      }
                                      break;
                                  case "CustPo":
                                      ast = iDeliveryRepository.GetDeliveryInfoValue(delievery, "CustPo");
                                      break;
                                  case "UUID":
                                      ast = iproductRepository.GetProductInfoValue(productID, "UUID");
                                      break;
                                  case "MAC":
                                      ast = currentProduct.MAC;
                                      break;
                                  //Add by Benson for Mantis: 0001539
                                  case "WM":
                                      ast = iproductRepository.GetProductInfoValue(productID, "WM");
                                      if (!string.IsNullOrEmpty(ast))
                                      {
                                          ast = ast.Replace(":", "");     //去掉字符':' 
                                      }
                                      break;
                                  //Add by Benson for Mantis: 0001539

                                  // Add by Benson for Mantis 0001723: weight站页面修改 at 2013/04/12
                                  case "SN":
                                      ast = custSN;
                                      break;
                                      //
                                  case "CHAR":
                                      IList<ConstValueInfo> valueList =
                                          ipartRepository.GetConstValueListByType("ASTCHAR").Where(x => x.value.Trim() != "" && x.name==av).ToList();
                                     if (valueList != null && valueList.Count > 0)
                                     {
                                         ast = valueList[0].value;
                                     }
                                     break;

                                  // Add by Benson for Mantis 0001723: weight站页面修改 at 2013/04/12

                                  case "AST":
                                      //IMES_FA..Product_Part 表中与当前Product 绑定的Parts 中存在BomNodeType为'AT'的Part记录的PartSn
                                      //Boolean exitPart = true;
                                      foreach (ProductPart iprodpart in productParts)
                                      {
                                          if (iprodpart.BomNodeType == "AT")
                                          {
                                              IPart curPart = ipartRepository.GetPartByPartNo(iprodpart.PartID);

                                              //Modify by Benson at 2012/12/13
                                              //if (curPart.BOMNodeType == "AT" && curPart.Descr == "ATSN4")
                                              if (curPart.BOMNodeType == "AT" && curPart.Descr == "ATSN3")
                                              {
                                                  ast = iprodpart.PartSn;
                                                  //exitPart = false;
                                                  break;
                                              }
                                          }
                                      }
                                      // Add by Benson for Mantis 0001723: weight站页面修改 at 2013/04/12
                                      if (string.IsNullOrEmpty(ast))
                                      {
                                          foreach (ProductPart iprodpart in productParts)
                                          {
                                              if (iprodpart.BomNodeType == "AT")
                                              {
                                                  IPart curPart = ipartRepository.GetPartByPartNo(iprodpart.PartID);

                                                  //Modify by Benson at 2012/12/13
                                                  //if (curPart.BOMNodeType == "AT" && curPart.Descr == "ATSN4")
                                                  if (curPart.BOMNodeType == "AT" && curPart.Descr == "ATSN1")
                                                  {
                                                      ast = iprodpart.PartSn;
                                                      //exitPart = false;
                                                      break;
                                                  }
                                              }
                                          }
                                      }

                                      // Add by Benson for Mantis 0001723: weight站页面修改 at 2013/04/12

                                      break;
                                  default:
                                      break;
                              }
                          }
                           
                              //  astrule = iproductRepository.GetAstRuleByCodeAndStationAndCust("ATSN4", station, cust);
                        }
                        else if (asttp == "ATSN7")
                        {

                              foreach (PartInfo ele in iCheckPart.Attributes)
                                {
                                    if (ele.InfoType == "AV")
                                    {
                                      av=ele.InfoValue;break;
                                    }
                                }
                              if(av=="")
                                {
                                    FisException fe = new FisException("PAK171", new string[] { iCheckPart.PN });  //请联系IE维护%1的AV值
                                    throw fe;
                                }
                          IList<AstRuleInfo> astrule = iproductRepository.GetAstRuleByCodeAndStationAndCust(asttp, station, av);
                            if (astrule != null && astrule.Count > 0 && !string.IsNullOrEmpty(astrule[0].checkItem))
                           {
                              item=astrule[0].checkItem;
                               switch (item)
                                    {
                                        case "PoNo":
                                            Delivery CurrentDelivery = iDeliveryRepository.Find(delievery);
                                             if (CurrentDelivery != null && !string.IsNullOrEmpty(CurrentDelivery.PoNo))
                                            {
                                                ast = CurrentDelivery.PoNo;
                                            }
                                            break;
                                        case "CustPo":
                                            ast = iDeliveryRepository.GetDeliveryInfoValue(delievery, "CustPo");
                                            break;
                                        case "UUID":
                                            ast = iproductRepository.GetProductInfoValue(productID, "UUID");
                                            break;
                                        case "MAC":
                                            ast = currentProduct.MAC;
                                            break;
                                        //Add by Benson for Mantis: 0001539
                                        case "WM":
                                            ast = iproductRepository.GetProductInfoValue(productID, "WM");
                                            if (!string.IsNullOrEmpty(ast))
                                            {
                                                ast = ast.Replace(":", "");     //去掉字符':' 
                                            }
                                            break;
                                        //Add by Benson for Mantis: 0001539
                                       
                                        case "AST":
                                            //IMES_FA..Product_Part 表中与当前Product 绑定的Parts 中存在BomNodeType为'AT'的Part记录的PartSn
                                            //Boolean exitPart = true;
                                            foreach (ProductPart iprodpart in productParts)
                                            {
                                                if (iprodpart.BomNodeType == "AT")
                                                {
                                                    IPart curPart = ipartRepository.GetPartByPartNo(iprodpart.PartID);
                                                    if (curPart.BOMNodeType == "AT" && curPart.Descr == "ATSN7")
                                                    {
                                                        ast = iprodpart.PartSn;
                                                        //exitPart = false;
                                                        break;
                                                    }
                                                }
                                            }
                                    
                                            break;
                                        default:
                                            break;
                                    }

                           }



                        }
                        else if (iCheckPart.Descr == "ATSN8")
                        {
                             foreach (PartInfo ele in iCheckPart.Attributes)
                                {
                                    if (ele.InfoType == "AV")
                                    {
                                      av=ele.InfoValue;break;
                                    }
                                }
                              if(av=="")
                                {
                                    FisException fe = new FisException("PAK171", new string[] { iCheckPart.PN });  //请联系IE维护%1的AV值
                                    throw fe;
                                }
                          IList<AstRuleInfo> astrule = iproductRepository.GetAstRuleByCodeAndStationAndCust(asttp, station, av);
                           if (astrule != null && astrule.Count > 0 && !string.IsNullOrEmpty(astrule[0].checkItem))
                           {
                              item=astrule[0].checkItem;
                               switch (item)
                                    {
                                        case "PoNo":
                                            Delivery CurrentDelivery = iDeliveryRepository.Find(delievery);
                                             if (CurrentDelivery != null && !string.IsNullOrEmpty(CurrentDelivery.PoNo))
                                            {
                                                ast = CurrentDelivery.PoNo;
                                            }
                                            break;
                                        case "CustPo":
                                            ast = iDeliveryRepository.GetDeliveryInfoValue(delievery, "CustPo");
                                            break;
                                        case "UUID":
                                            ast = iproductRepository.GetProductInfoValue(productID, "UUID");
                                            break;
                                        case "MAC":
                                            ast = currentProduct.MAC;
                                            break;
                                        //Add by Benson for Mantis: 0001539
                                        case "WM":
                                            ast = iproductRepository.GetProductInfoValue(productID, "WM");
                                            if (!string.IsNullOrEmpty(ast))
                                            {
                                                ast = ast.Replace(":", "");     //去掉字符':' 
                                            }
                                            break;
                                        //Add by Benson for Mantis: 0001539
                                       
                                        case "AST":
                                          
                                            foreach (ProductPart iprodpart in productParts)
                                            {
                                                if (iprodpart.BomNodeType == "AT")
                                                {
                                                    IPart curPart = ipartRepository.GetPartByPartNo(iprodpart.PartID);
                                                    if (curPart.BOMNodeType == "AT" && curPart.Descr == "ATSN3")
                                                    {
                                                        ast = iprodpart.PartSn;
                                                        //exitPart = false;
                                                        break;
                                                    }
                                                }
                                            }
                                    
                                            break;
                                        default:
                                            break;
                                    }

                           }
                        }
                        else
                        {
                            continue;
                        }
                    }

                }


          


                IModelRepository iModelRepository = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();

                //if (!string.IsNullOrEmpty(asttp))
                //{

                //    // IMES_FA..Product_Part 表中与当前Product 绑定的Parts 
                //    /* marked by Benson at 2013/01/18
                //       IList<IProductPart> productParts = new List<IProductPart>();
                //       productParts = currentProduct.ProductParts;
                //       if (productParts == null || productParts.Count <= 0)
                //       {
                //           List<string> errpara = new List<string>();
                //           errpara.Add(productID);
                //           throw new FisException("PAK038", errpara);  //该Product尚未绑定Part！
                //       }
                //     *  marked by Benson at 2013/01/18
                //     */





                //    //   i.	当@asttp='ATSN1'


                //    if (asttp == "ATSN1")
                //    {
                //        //使用Code=@asttp AND wc="85"AND CheckTp=@astrm ,查询IMES_FA..AstRule 表存在记录时 
                //        IList<AstRuleInfo> astRuleInfoLst = new List<AstRuleInfo>();
                //        astRuleInfoLst = iproductRepository.GetAstRuleByCodeAndStationAndCheckTp(asttp, station, astrm);
                //        //if (astRuleInfoLst.Count <= 0)  // ITC-1360-1441: 确实有@ast=''的情况存在(Get Type中提及),因此只卡一处：AstRule 表存在记录时， ATSN1没有
                //        //{
                //        //    FisException fe = new FisException("PAK031", new string[] { });  //此机器未结合Asset Tag！
                //        //    throw fe;
                //        //}

                //        //IMES_FA..Product_Part 表中与当前Product 绑定的Parts 中存在BomNodeType (parttype)为'AT'，Descr属性为 "ATSN1"的Part
                //        //string[] descr = new string[1];
                //        //descr[0] = "ATSN1";
                //        //IList<PartDef> partLst = ipartRepository.GetPartByBomNodeTypeAndDescr(productID, "AT", descr);
                //        //foreach (PartDef ipart in partLst)
                //        //{
                //        //    Boolean f = false;
                //        //    if (ipart.partNo == iprodpart.PartID)
                //        //    {
                //        //        ast = iprodpart.PartSn;
                //        //        f = true;
                //        //        break;
                //        //    }
                //        //    if (f)
                //        //    { break; }
                //        //} // 改用新方法如下：

                //        if (astRuleInfoLst != null && astRuleInfoLst.Count > 0)
                //        {
                //            Boolean exitPart = true;
                //            foreach (ProductPart iprodpart in productParts)
                //            {
                //                if (iprodpart.BomNodeType == "AT")
                //                {
                //                    IPart curPart = ipartRepository.GetPartByPartNo(iprodpart.PartID);
                //                    if (curPart.BOMNodeType == "AT" && curPart.Descr == "ATSN1")
                //                    {
                //                        ast = iprodpart.PartSn;
                //                        item = "AST";
                //                        exitPart = false;
                //                        break;
                //                    }
                //                }
                //            }
                //            if (exitPart)
                //            {
                //                FisException fe = new FisException("PAK031", new string[] { });  //此机器未结合Asset Tag！
                //                throw fe;
                //            }
                //        }
                //    }
                //    else  // ii. 当@asttp<>'ATSN1' 时
                //    {
                //        IList<ModelInfo> modelInfos2 = new List<ModelInfo>();
                //        IList<ModelInfo> modelInfos = new List<ModelInfo>();
                //        //Modify 2012/07/25 get "Cust2" ->Get "Cust"
                //        //-------------------------------------------
                //        modelInfos2 = iModelRepository.GetModelInfoByModelAndName(pno, "Cust2");
                //        if (modelInfos2 == null || modelInfos2.Count <= 0)
                //        {
                //            cust = "";
                //        }
                //        else
                //        {
                //            cust = modelInfos2[0].Value;
                //        }
                //        if (cust.Trim() =="")
                //        {
                //            modelInfos = iModelRepository.GetModelInfoByModelAndName(pno, "Cust");
                //            if (modelInfos == null || modelInfos.Count <= 0)
                //            {
                //                cust = "";
                //            }
                //            else
                //            {
                //                cust = modelInfos[0].Value;
                //            }
                //        }
                //        //-------------------------------------------
                //        //modelInfos = iModelRepository.GetModelInfoByModelAndName(pno, "Cust");
                //        //if (modelInfos == null || modelInfos.Count <= 0)
                //        //{
                //        //    cust = "";
                //        //}
                //        //else
                //        //{
                //        //    cust = modelInfos[0].Value;
                //        //}
                //        //  iii.使用Code=@asttp AND Station=@wc AND CustName=@cust 查询IMES_FA..AstRule 表存在记录时
                //        IList<AstRuleInfo> astrule = iproductRepository.GetAstRuleByCodeAndStationAndCust(asttp, station, cust);
                //        if (astrule.Count > 0)
                //        {
                //            if (asttp == "ATSN4")
                //            {
                //                astrule = iproductRepository.GetAstRuleByCodeAndStationAndCust("ATSN4", station, cust);
                //                //if (astrule == null || astrule.Count <= 0 || string.IsNullOrEmpty(astrule[0].checkItem))  // ITC-1360-1441: 确实有@ast=''的情况存在(Get Type中提及),因此只卡一处：AstRule 表存在记录时， ATSN1没有
                //                //{
                //                //    FisException fe = new FisException("PAK031", new string[] { });  //此机器未结合Asset Tag！
                //                //    throw fe;
                //                //}
                //                if (astrule != null && astrule.Count > 0 && !string.IsNullOrEmpty(astrule[0].checkItem))
                //                {
                //                    item = astrule[0].checkItem;
                //                    switch (item)
                //                    {
                //                        case "PoNo":
                //                            Delivery CurrentDelivery = iDeliveryRepository.Find(delievery);
                //                            //if (CurrentDelivery == null)
                //                            //{
                //                            //    List<string> errpara = new List<string>();
                //                            //    errpara.Add(delievery);
                //                            //    throw new FisException("CHK107", errpara);
                //                            //}
                //                            if (CurrentDelivery != null && !string.IsNullOrEmpty(CurrentDelivery.PoNo))
                //                            {
                //                                ast = CurrentDelivery.PoNo;
                //                            }
                //                            break;
                //                        case "CustPo":
                //                            ast = iDeliveryRepository.GetDeliveryInfoValue(delievery, "CustPo");
                //                            break;
                //                        case "UUID":
                //                            ast = iproductRepository.GetProductInfoValue(productID, "UUID");
                //                            break;
                //                        case "MAC":
                //                            ast = currentProduct.MAC;
                //                            break;
                //                        //Add by Benson for Mantis: 0001539
                //                        case "WM":
                //                            ast = iproductRepository.GetProductInfoValue(productID, "WM");
                //                            if (!string.IsNullOrEmpty(ast))
                //                            {
                //                                ast = ast.Replace(":", "");     //去掉字符':' 
                //                            }
                //                            break;
                //                        //Add by Benson for Mantis: 0001539
                                       
                //                        case "AST":
                //                            //IMES_FA..Product_Part 表中与当前Product 绑定的Parts 中存在BomNodeType为'AT'的Part记录的PartSn
                //                            //Boolean exitPart = true;
                //                            foreach (ProductPart iprodpart in productParts)
                //                            {
                //                                if (iprodpart.BomNodeType == "AT")
                //                                {
                //                                    IPart curPart = ipartRepository.GetPartByPartNo(iprodpart.PartID);
                                                   
                //                                    //Modify by Benson at 2012/12/13
                //                                    //if (curPart.BOMNodeType == "AT" && curPart.Descr == "ATSN4")
                //                                    if (curPart.BOMNodeType == "AT" && curPart.Descr == "ATSN3")
                //                                    {
                //                                        ast = iprodpart.PartSn;
                //                                        //exitPart = false;
                //                                        break;
                //                                    }
                //                                }
                //                            }
                //                            //if (exitPart) // ITC-1360-1441: 确实有@ast=''的情况存在(Get Type中提及),因此只卡一处：AstRule 表存在记录时， ATSN1没有
                //                            //{
                //                            //    FisException fe = new FisException("PAK031", new string[] { });  //此机器未结合Asset Tag！
                //                            //    throw fe;
                //                            //}
                //                            break;
                //                        default:
                //                            break;
                //                    }

                //                    //if (string.IsNullOrEmpty(ast)) // ITC-1360-1441: 确实有@ast=''的情况存在(Get Type中提及),因此只卡一处：AstRule 表存在记录时， ATSN1没有
                //                    //{
                //                    //    FisException fe = new FisException("PAK031", new string[] { });  //此机器未结合Asset Tag！
                //                    //    throw fe;
                //                    //}
                //                }
                //            }

                //            else if (asttp == "ATSN7")
                //            {
                //                astrule = iproductRepository.GetAstRuleByCodeAndStationAndCust("ATSN7", station, cust);
                //                //if (astrule == null || astrule.Count <= 0 || string.IsNullOrEmpty(astrule[0].checkItem)) // ITC-1360-1441: 确实有@ast=''的情况存在(Get Type中提及),因此只卡一处：AstRule 表存在记录时， ATSN1没有
                //                //{
                //                //    FisException fe = new FisException("PAK031", new string[] { });  //此机器未结合Asset Tag！
                //                //    throw fe;
                //                //}
                //                if (astrule != null && astrule.Count > 0 && !string.IsNullOrEmpty(astrule[0].checkItem))
                //                {
                //                    item = astrule[0].checkItem;
                //                    switch (item)
                //                    {
                //                        case "PoNo":
                //                            Delivery CurrentDelivery = iDeliveryRepository.Find(delievery);
                //                            //if (CurrentDelivery == null || string.IsNullOrEmpty(CurrentDelivery.PoNo))
                //                            //{
                //                            //    List<string> errpara = new List<string>();
                //                            //    errpara.Add(delievery);
                //                            //    throw new FisException("CHK107", errpara);
                //                            //}
                //                            if (CurrentDelivery != null && !string.IsNullOrEmpty(CurrentDelivery.PoNo))
                //                            {
                //                                ast = CurrentDelivery.PoNo;
                //                            }
                //                            break;
                //                        case "CustPo":
                //                            ast = iDeliveryRepository.GetDeliveryInfoValue(delievery, "CustPo");
                //                            break;
                //                        case "UUID":
                //                            //ast = currentProduct.UUID; // QA 发现item=UUID时，ATSN4与ATSN7的处理不一样，经UC确认，按ATSN4方式处理
                //                            ast = iproductRepository.GetProductInfoValue(productID, "UUID");
                //                            break;
                //                        case "WM":
                //                            ast = iproductRepository.GetProductInfoValue(productID, "WM");
                //                            if (!string.IsNullOrEmpty(ast))
                //                            {
                //                                ast = ast.Replace(":", "");     //去掉字符':' 
                //                            }
                //                            break;
                //                        case "MAC":
                //                            ast = currentProduct.MAC;
                //                            break;
                //                        case "AST":
                //                            //Boolean exitPart = true;
                //                            foreach (ProductPart iprodpart in productParts)
                //                            {
                //                                if (iprodpart.BomNodeType == "AT")
                //                                {
                //                                    IPart curPart = ipartRepository.GetPartByPartNo(iprodpart.PartID);
                //                                    if (curPart.BOMNodeType == "AT" && curPart.Descr == "ATSN7")
                //                                    {
                //                                        ast = iprodpart.PartSn;
                //                                        //exitPart = false;
                //                                        break;
                //                                    }
                //                                }
                //                            }
                //                            //if (exitPart) // ITC-1360-1441: 确实有@ast=''的情况存在(Get Type中提及),因此只卡一处：AstRule 表存在记录时， ATSN1没有
                //                            //{
                //                            //    FisException fe = new FisException("PAK031", new string[] { });  //此机器未结合Asset Tag！
                //                            //    throw fe;
                //                            //}
                //                            break;
                //                        default:
                //                            break;
                //                    }
                //                    //if (string.IsNullOrEmpty(ast))  // ITC-1360-1441: 确实有@ast=''的情况存在(Get Type中提及),因此只卡一处：AstRule 表存在记录时， ATSN1没有
                //                    //{
                //                    //    FisException fe = new FisException("PAK031", new string[] { });  //此机器未结合Asset Tag！
                //                    //    throw fe;
                //                    //}
                //                }
                //            }

                //            else if (asttp != "ATSN7" && asttp != "ATSN4" && asttp != "ATSN1")
                //            {
                //                //Boolean exitPart = true;
                //                foreach (ProductPart iprodpart in productParts)
                //                {
                //                    if (iprodpart.BomNodeType == "AT")
                //                    {
                //                        IPart curPart = ipartRepository.GetPartByPartNo(iprodpart.PartID);
                //                        if (curPart.BOMNodeType == "AT" && curPart.Descr == asttp)
                //                        {
                //                            ast = iprodpart.PartSn;
                //                            //exitPart = false;
                //                            break;
                //                        }
                //                    }
                //                }
                //                //if (exitPart) // ITC-1360-1441: 确实有@ast=''的情况存在(Get Type中提及),因此只卡一处：AstRule 表存在记录时， ATSN1没有
                //                //{
                //                //    FisException fe = new FisException("PAK031", new string[] { });  //此机器未结合Asset Tag！
                //                //    throw fe;
                //                //}

                //                item = "AST";
                //            }
                //        }

                //        //else    //astrule.Count<=0 // ITC-1360-1441: 确实有@ast=''的情况存在(Get Type中提及),因此只卡一处：AstRule 表存在记录时， ATSN1没有
                //        //{
                //        //    FisException fe = new FisException("PAK031", new string[] { });  //此机器未结合Asset Tag！
                //        //    throw fe;
                //        //}
                //    }
                //}

                // 7. Get Type
                string type = string.Empty;
                Model modelObject = iModelRepository.Find(currentProduct.Model);
                //if (modelObject == null || string.IsNullOrEmpty(modelObject.FamilyName))
                //{
                //    throw new FisException("CHK038", new string[] { currentProduct.Model});
                //}
                string family = string.Empty;
                if (modelObject != null && !string.IsNullOrEmpty(modelObject.FamilyName))
                {
                    family = modelObject.FamilyName;
                }
                //ITC-1360-1272 : BT产品没有plt，做个保护
                //if (plt!=""&& plt.Length>=2 && !string.IsNullOrEmpty(family))
                //if (plt!=""&& plt.Length>=2)
                //{
                if (plt != "" && plt.Length >= 2 && plt.Substring(0, 2) == "NA" && ast != "" && (family == "TAOS 2.0" || family == "DAVOS 3.0" || family == "TIANSHAN 1.0"))
                {
                    type = "YA";
                }
                else
                {
                    if (plt != "" && plt.Length >= 2 && plt.Substring(0, 2) == "NA" && ast == "" && (family == "TAOS 2.0" || family == "DAVOS 3.0" || family == "TIANSHAN 1.0"))
                    {
                        type = "N";
                    }
                    else
                    {
                        if (flg == "N" && jcid != "" && ast != "")
                        {
                            type = "YABJ";
                        }
                        else
                        {
                            if (flg == "N" && string.IsNullOrEmpty(jcid) && ast != "")
                            {
                                type = "YAB";
                            }
                            else
                            {
                                if (flg != "N" && jcid != "" && jcid != null && ast != "")
                                {
                                    type = "YAJ";
                                }
                                else
                                {
                                    // ITC-1360-1440 : @flg<>'N' AND ( @jcid='' OR @jcid is null ) AND @ast<>'' 
                                    if (flg != "N" && string.IsNullOrEmpty(jcid) && ast != "")
                                    {
                                        type = "YA";
                                    }
                                    else
                                    {
                                        if (flg == "N" && jcid != "" && ast == "")
                                        {
                                            type = "YBJ";
                                        }
                                        else
                                        {
                                            if (flg == "N" && string.IsNullOrEmpty(jcid) && ast == "")
                                            {
                                                type = "YB";
                                            }
                                            else
                                            {
                                                if (flg != "N" && jcid != "" && jcid != null && ast == "")
                                                {
                                                    type = "YJ";
                                                }
                                                else type = "N";
                                            }
                                        }
                                    }
                                }
                            }

                        }
                    }
                }
                //}
                // 8. Get RMN Flag
                //如果ModelBOM 中Model 直接下阶中有Part No 为'6060B0232501' 或者'6060B0487001'的Part 时，@rmnflag = '0'；否则@rmnflag = '1'
                //如果ModelBOM 中Model 直接下阶存在Descr in ('China label','TAIWAN Label')的Part (两者不会同时存在)，@rmnflag = '0'；否则@rmnflag = '1'   （UC Update: SVN Revision: 11535）
                string rmnflag = "1";
                foreach (IBOMNode ibomnode in bomNodeLst)
                {
                    IPart currentPart = ibomnode.Part;
                    //if (currentPart.PN == "6060B0232501" || currentPart.PN == "6060B0487001")
                    if (currentPart.Descr == "China label" || currentPart.Descr == "TAIWAN Label")
                    {
                        rmnflag = "0";
                        break;
                    }
                }


                // Print =====如果为BT 产品，当满足如下条件时，需要列印Config Label=====

                currentSession.AddValue(Session.SessionKeys.labelBranch, "");
                delievery = currentProduct.DeliveryNo;
                configParams = new List<string>();
               
                //Revision: 9810:	修改列印Config Label 的条件为非BT
                if (!currentProduct.IsBT)
                {
                    string BTRegId = iDeliveryRepository.GetDeliveryInfoValue(delievery, "RegId");
                    string BTShipTp = iDeliveryRepository.GetDeliveryInfoValue(delievery, "ShipTp");
                    string BTCountry = iDeliveryRepository.GetDeliveryInfoValue(delievery, "Country");

                    if (!string.IsNullOrEmpty(BTRegId) && !string.IsNullOrEmpty(BTShipTp) && !string.IsNullOrEmpty(BTCountry))
                    {
                        //ITC-1360-1527:RegId 为'SNE'时，未能打印CONFIG LABEL
                        IPartRepository partRep = RepositoryFactory.GetInstance().GetRepository<IPartRepository>();
                   
                        
                        //bool bRegId = new[] { "SCN", "SAF", "SNE", "SCE", "QCN", "QAF", "QNE", "QCE" }.Any(BTRegId.Trim().Equals);
                        bool bRegId = false;
                        IList<ConstValueTypeInfo> constValueTypeList = partRep.GetConstValueTypeList("ConfigLabelRegionId", BTRegId.Trim());
                        if (constValueTypeList != null && constValueTypeList.Count > 0)
                        {
                            bRegId = true;
                        }
                       // if ((BTRegId == "SCN" || BTRegId == "SAF" || BTRegId == "SNE" || BTRegId == "SCE") && (BTShipTp == "CTO") && (BTCountry != "JAPAN"))
                        if (bRegId && (BTShipTp == "CTO") && (BTCountry != "JAPAN"))
                         {
                            printlabeltype1 = "ConfigLabel";
                            currentSession.AddValue(Session.SessionKeys.labelBranch, printlabeltype1);

                            //IList<ModelInfo> modelInfo = new List<ModelInfo>();
                            //modelInfo = iModelRepository.GetModelInfoByModelAndName(pno, "PN");
                            //string AV1= modelInfo[0].Value;
                            //configParams.Add(AV1); 

                            //string bu = string.Empty;
                            //foreach (IBOMNode ibomnode in bomNodeLst)
                            //{
                            //    IPart currentPart = ibomnode.Part;
                            //    if (currentPart.BOMNodeType == "AV" && currentPart.GetProperty("Descr").Substring(0,2)=="BU")
                            //    {
                            //      bu = currentPart.GetProperty("AV");
                            //      IList<string> CTOBomDescrLst = new List<string>();
                            //      CTOBomDescrLst = ibomRepository.GetCTOBomDescr(pno, bu);
                            //      if (CTOBomDescrLst == null || CTOBomDescrLst.Count <= 0)
                            //      {
                            //          List<string> errpara = new List<string>();
                            //          errpara.Add("Config");
                            //          errpara.Add("DESCR1");
                            //          throw new FisException("PAK040", errpara);
                            //      }
                            //      configParams.Add(CTOBomDescrLst[0]);  
                            //      break;
                            //    }
                            //}

                            //configParams.Add(currentProduct.CUSTSN);   
                            //configParams.Add(currentProduct.ModelObj.FamilyName);  
                            //configParams.Add(currentProduct.Model.Substring(0, 6));

                            ////AV / QTY / DESCR这三个变量对应的数据是查询IMES_GetData..CTOBom 的得到的满足条件的记录集的SPno / Qty / Descr 字段.对应记录数量，n 从1开始依次编号
                            //int AVn = 0;
                            //int QTYn =0;
                            //int DESCRn =0;
                            //IList<CtoBomInfo> CTOBomLst = new List<CtoBomInfo>();
                            //CTOBomLst = ibomRepository.GetCTOBomList(pno,bu);
                            //if (CTOBomLst == null || CTOBomLst.Count <= 0)
                            //{
                            //    List<string> errpara = new List<string>();
                            //    errpara.Add("Config");
                            //    errpara.Add("AV/QTY/DESCR");
                            //    throw new FisException("PAK040", errpara);
                            //}
                            //foreach (CtoBomInfo ctobom in CTOBomLst)
                            //{
                            //    if (string.IsNullOrEmpty(ctobom.spno))
                            //    {
                            //        AVn++;
                            //    }
                            //    if (ctobom.qty >0)
                            //    {
                            //        QTYn++;
                            //    }
                            //    if (string.IsNullOrEmpty(ctobom.descr))
                            //    {
                            //        DESCRn++;
                            //    }

                            //}
                            //configParams.Add(AVn.ToString());
                            //configParams.Add(QTYn.ToString());
                            //configParams.Add(DESCRn.ToString());

                        }
                    }
                }

                //  Print =====如果IMES_PAK..PODLabelPart中有维护PartNo等于Model的前几位字符的记录 and (Model的第7位不是数字)，则需要列印POD Label====

                IList<PODLabelPartDef> podLabelPartLst = new List<PODLabelPartDef>();
                podLabelPartLst = ipartRepository.GetPODLabelPartListByPartNo(pno);
                if (podLabelPartLst.Count > 0)
                {
                    string number = "0123456789";
                    string modelbit = currentProduct.Model.Substring(6, 1);
                    if (!number.Contains(modelbit)) //Model的第7位不是数字
                    {
                        printlabeltype2 = "PODLabel";
                        currentSession.AddValue(Session.SessionKeys.labelBranch, printlabeltype2);  //如果列印PodLabel，则需要再记录一次ProductLog (Station = 'PD'，Line = 'POD Label Print')
                    }
                }

                // （\\hp-iis\OUT\）需要查询SysSetting 表获取, 参考方法：select Value from SysSetting nolock where Name = 'EditsFISAddr'
                IList<string> EditsFISAddrLst = new List<string>();
                string EditsFISAddr = string.Empty;
                EditsFISAddrLst = ipartRepository.GetValueFromSysSettingByName("EditsFISAddr");
                if (EditsFISAddrLst != null && EditsFISAddrLst.Count > 0)
                {
                    EditsFISAddr = EditsFISAddrLst[0];
                }

                retLst.Add(productID);              //  [0] IMES_FA..ProductID
                retLst.Add(pno);                    //  [1] IMES_FA..Product.Model
                retLst.Add(standardWeight);         //  [2] (decimal) IMES_GetData..ModelWeight.UnitWeight 
                retLst.Add(labelType1);             //  [3] PAQC 抽检
                retLst.Add(labelType2);             //  [4] Adaptor Label / India Label 判定
                retLst.Add(item);                   //  [5] AssetTagItem

                retLst.Add(rmnflag);                //  [6] RMN Flag
                retLst.Add(jcid);                   //  [7] ConfigID
                retLst.Add(ast);                    //  [8] AssetTagItemValue
                retLst.Add(type);                   //  [9] Type
                retLst.Add(modelTolerance);         //  [10] (string) modelTolerance

                retLst.Add(printlabeltype1);            //  [11] print Config Label
                retLst.Add(printlabeltype2);            //  [12] print POD Label

                retLst.Add(EditsFISAddr);           //  [13]  （\\hp-iis\OUT\）需要查询SysSetting 表获取, 参考方法：select Value from SysSetting nolock where Name = 'EditsFISAddr'

                string printexepath = string.Empty;
                IList<string> printexepathLst = ipartRepository.GetValueFromSysSettingByName("PDFPrintPath");
                if (printexepathLst != null && printexepathLst.Count > 0)
                {
                    printexepath = printexepathLst[0];
                }
                else printexepath = "C:\\FIS\\";

                retLst.Add(printexepath);
                IList<string> lstWin8PartNo = CheckIsWin8(bomNodeLst);
                retLst.Add(lstWin8PartNo);

                string faiCheck = CheckFAI(currentProduct.Model.Trim());
                retLst.Add(faiCheck);
                return retLst;
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg, e);
                if (e.mErrcode == "CHK020") //序號已被刷入
                {
                    throw e;
                }
                throw new Exception(e.mErrmsg);
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw new SystemException(e.Message);
            }
            finally
            {
                logger.Debug("(UnitWeight)InputUUT End,"
                    + " [custSN]:" + custSN
                    + " [line]:" + line
                    + " [editor]:" + editor
                    + " [station]:" + station
                    + " [customer]:" + customer);
            }

        }

        /// <summary>
        /// 刷入RMN后，检查RMN。
        /// RMN 是China Label or Taiwan Label 上的安规号码
        /// </summary>
        /// <param name="productID"></param>
        /// <param name="RMN"></param>
        /// <returns></returns>
        public void CheckRMN(string productID, string RMN)
        {
            logger.Debug("(UnitWeight)CheckRMN Start,"
                + " [productID]:" + productID + "[RMN]" + RMN);
            FisException ex;
            List<string> erpara = new List<string>();
            try
            {
                Session currentSession = SessionManager.GetInstance.GetSession(productID, Session.SessionType.Product);

                if (currentSession == null)
                {
                    erpara.Add(productID);
                    ex = new FisException("CHK021", erpara);
                    logger.Error(ex.Message, ex);
                    throw ex;
                }
                else
                {
                    currentSession.AddValue(Session.SessionKeys.ifElseBranch, "RMN");
                    currentSession.AddValue(Session.SessionKeys.RMN, RMN);
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
                logger.Debug("(UnitWeight)CheckRMN End,"
                    + " [productID]:" + productID + "[RMN]" + RMN);
            }
        }


        /// <summary>
        /// 刷入RMN后，检查RMN。
        /// [UCC]（20位） / [Box Id]（10位，但是有的Label 列印的Box Id Barcode 中会多出2位前缀，请注意） 只能刷其中一个
        /// </summary>
        /// <param name="productID"></param>
        /// <param name="BoxID"></param>
        /// <returns></returns>
        public void CheckBoxIDorUCC(string productID, string BoxID)
        {
            logger.Debug("(UnitWeight)CheckBoxIDorUCC Start,"
                + " [productID]:" + productID + "[BoxID]" + BoxID);
            FisException ex;
            List<string> erpara = new List<string>();
            try
            {
                Session currentSession = SessionManager.GetInstance.GetSession(productID, Session.SessionType.Product);

                if (currentSession == null)
                {
                    erpara.Add(productID);
                    ex = new FisException("CHK021", erpara);
                    logger.Error(ex.Message, ex);
                    throw ex;
                }
                else
                {
                    currentSession.AddValue(Session.SessionKeys.ifElseBranch, "BoxID");
                    currentSession.AddValue(Session.SessionKeys.boxId, BoxID);
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

            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg);
                throw e;

                //logger.Error(e.mErrmsg, e);
                //if (e.mErrcode == "PAK035" || e.mErrcode == "PAK034") //Box ID / UCC 与机器不匹配！
                //{
                //    e.stopWF = false;

                //    throw e;
                //}
                //else
                //{
                //    throw new Exception(e.mErrmsg);
                //}

            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw new SystemException(e.Message);
            }
            finally
            {
                logger.Debug("(UnitWeight)CheckBoxIDorUCC End,"
                    + " [productID]:" + productID + "[BoxID]" + BoxID);
            }
        }

        private List<string> CheckIsWin8(IList<IBOMNode> bomNodeLst)
        {
            string[] strArray = new string[] { "WIN8 BOX LABEL", "WIND8 Box Label"};
            IList<IPart> lst = (IList<IPart>)(from x in bomNodeLst
                                              where strArray.Contains(x.Part.Descr)
                                              select x.Part).ToList();
            var tn = (from y in lst
                      where
                       ((from z in y.Attributes
                         where z.InfoType == "Descr"
                         select z.InfoValue).ToList().Count) > 0
                      select y);

            List<string> result = (List<string>)(from x in tn
                                             select x.Attributes.Where(h => h.InfoType == "Descr").ToList()[0].InfoValue).ToList();
            return result;  
            
            /* OK
            string[] strArray = new string[] { "WIN8 BOX LABEL", "WIND8 Box Label" };
            IList<IPart> lst = (IList<IPart>)(from x in bomNodeLst
                                              where strArray.Contains(x.Part.Descr)
                                              select x.Part).ToList();

            List<string> rst = new List<string>();
            foreach (IPart p in lst)
            {
                var x = (from y in p.Attributes
                         where y.InfoType == "Descr"
                         select y.InfoValue).ToList();

                if (x.Count > 0)
                {
                    rst.Add(x[0].Trim());
                }
            } 
            return rst;OK*/
                 
        }

        /// <summary>
        /// 保存机器重量，更新机器状态，返回打印重量标签的PrintItem，结束工作流。
        /// 将printItems放到Session.PrintItems中
        /// </summary>
        /// <param name="productID"></param>
        /// <param name="printItems"></param>
        /// <returns></returns>
        public IList<PrintItem> Save(string productID, IList<PrintItem> printItems)
        {
            logger.Debug("(UnitWeight)Save Start,"
                + " [productID]:" + productID + "[printItems]" + printItems);
            FisException ex;
            List<string> erpara = new List<string>();
            try
            {
                Session currentSession = SessionManager.GetInstance.GetSession(productID, Session.SessionType.Product);

                if (currentSession == null)
                {
                    erpara.Add(productID);
                    ex = new FisException("CHK021", erpara);
                    logger.Error(ex.Message, ex);
                    throw ex;
                }
                else
                {

                    //1.	如果Product Model 是'PC' 开头，并且非BT 产品，如果该Product 尚未绑定Delivery，则报告错误：“尚未结合Delivery!”
                    Product currentProduct = (Product)currentSession.GetValue(Session.SessionKeys.Product);
                    //if (currentProduct.Model.Substring(0, 2) == "PC" && !currentProduct.IsBT)
                    // mantis 2071
                    if (CommonImpl.GetInstance().CheckModelByProcReg(currentProduct.Model, "SKU,ThinClient") && !currentProduct.IsBT)
                    {
                        if (string.IsNullOrEmpty(currentProduct.DeliveryNo))
                        {
                            erpara.Add(productID);
                            ex = new FisException("PAK036", erpara);
                            throw ex;
                        }
                    }
                    currentSession.AddValue("HavePrintItem", false);
                    if (printItems != null && printItems.Count > 0)
                    {
                        currentSession.AddValue("HavePrintItem", true);
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

                return (IList<PrintItem>)currentSession.GetValue(Session.SessionKeys.PrintItems);

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
                logger.Debug("(UnitWeight)Save End,"
                    + " [productID]:" + productID + "[printItems]" + printItems);
            }
        }



        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="custSN"></param>
        public void Cancel(string custSN)
        {
            logger.Debug("(UnitWeight)Cancel Start,"
               + " [custSN]:" + custSN);

            try
            {
                var currentProduct = CommonImpl.GetProductByInput(custSN, CommonImpl.InputTypeEnum.CustSN);
                string sessionKey = currentProduct.ProId;

                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, Session.SessionType.Product);

                if (currentSession != null)
                {
                    SessionManager.GetInstance.RemoveSession(currentSession);
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
                logger.Debug("(UnitWeight)Cancel End,"
                    + " [productID]:" + custSN);
            }

        }
        #endregion




        #region Reprint

        /// <summary>
        /// 重印Unit Weight Label
        /// </summary>
        /// <param name="custSN"></param>
        /// <param name="reason"></param>
        /// <param name="line"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <param name="printItemList"></param>
        /// <param name="printlabeltype"></param>
        /// <param name="model"></param>
        /// <param name="printexepath"></param>
        /// <returns></returns>
        public IList<PrintItem> ReprintLabel(string custSN, string reason, string line, string editor, string station, string customer, IList<PrintItem> printItemList, out string printlabeltype, out string model, out string printexepath)
        {
            logger.Debug("(UnitWeight)ReprintLabel Start,"
                            + " [custSN]:" + custSN
                            + " [reason]:" + reason
                            + " [line]:" + line
                            + " [editor]:" + editor
                            + " [station]:" + station
                            + " [customer]:" + customer);
            FisException ex;
            List<string> erpara = new List<string>();

            try
            {
                var currentProduct = CommonImpl.GetProductByInput(custSN, CommonImpl.InputTypeEnum.CustSN);
                //if (currentProduct == null)
                //{
                //    FisException fe = new FisException("CHK079", new string[] { custSN });
                //    throw fe;
                //}
                if (currentProduct == null)
                {
                    erpara.Add(custSN);
                    ex = new FisException("PAK042", erpara);
                    throw ex;
                }

                //获取与Customer S/N 绑定的Delivery No :IMES_FA..Product.DeliveryNo
                if (!currentProduct.IsBT && string.IsNullOrEmpty(currentProduct.DeliveryNo))
                {
                    erpara.Add(custSN);
                    ex = new FisException("PAK020", erpara);    //该Customer S/N还未与DN绑定!
                    throw ex;
                }
                string delievery = currentProduct.DeliveryNo;

                //获取Pallet No : IMES_FA..Product.PalletNo
                if (!currentProduct.IsBT && string.IsNullOrEmpty(currentProduct.PalletNo) && currentProduct.Model.Substring(0, 3) != "146")
                {
                    erpara.Add(custSN);
                    ex = new FisException("PAK021", erpara);     //该Customer S/N还未与Pallet绑定!
                    throw ex;
                }
                string palletno = currentProduct.PalletNo;

                string sessionKey = currentProduct.ProId;
                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, Session.SessionType.Product);

                if (currentSession == null)
                {
                    currentSession = new Session(sessionKey, Session.SessionType.Product, editor, station, line, customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", currentSession);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", line);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", Session.SessionType.Product);

                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(station, "PakUnitWeightReprint.xoml", "PakUnitWeightReprint.rules", out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    currentSession.AddValue(Session.SessionKeys.CustSN, custSN);
                    currentSession.AddValue(Session.SessionKeys.Product, currentProduct);
                    currentSession.AddValue(Session.SessionKeys.PrintLogBegNo, custSN);
                    currentSession.AddValue(Session.SessionKeys.PrintLogEndNo, custSN);
                    currentSession.AddValue(Session.SessionKeys.PrintLogDescr, "UnitWeightReprint");
                    currentSession.AddValue(Session.SessionKeys.Reason, reason);
                    currentSession.AddValue(Session.SessionKeys.PrintItems, printItemList);
                    currentSession.AddValue(Session.SessionKeys.DeliveryNo, delievery);
                    currentSession.AddValue(Session.SessionKeys.PalletNo, palletno);
                    currentSession.AddValue("HavePrintItem", false);
                    if (printItemList != null && printItemList.Count > 0)
                    {
                        currentSession.AddValue("HavePrintItem", true);
                    }

                    currentSession.SetInstance(instance);

                    if (!SessionManager.GetInstance.AddSession(currentSession))
                    {
                        currentSession.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");
                        erpara.Add(custSN);
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

                printlabeltype = string.Empty;
                printlabeltype = (string)currentSession.GetValue(Session.SessionKeys.labelBranch);

                model = string.Empty;
                model = currentProduct.Model;

                printexepath = string.Empty;
                IPartRepository ipartRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
                IList<string> printexepathLst = ipartRepository.GetValueFromSysSettingByName("PDFPrintPath");
                if (printexepathLst != null && printexepathLst.Count > 0)
                {
                    printexepath = printexepathLst[0];
                }
                else printexepath = "C:\\FIS\\";

                return (IList<PrintItem>)currentSession.GetValue(Session.SessionKeys.PrintItems);
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
                logger.Debug("(UnitWeight)ReprintLabel End,"
            + " [custSN]:" + custSN
            + " [reason]:" + reason
            + " [line]:" + line
            + " [editor]:" + editor
            + " [station]:" + station
            + " [customer]:" + customer);
            }
        }
        #endregion

        /// <summary>
        /// 获取ModelWeight
        /// </summary>
        /// <param name="inputData">inputData</param>
        /// <returns></returns>
        public ModelWeightDef GetModelWeightByModelorCustSN(string inputData)
        {
            //检查合法model
            //看取得的数据是否有数据
            String result = "";
            try
            {
                string model = string.Empty;
                IModelRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IModelRepository>();


                //3)	如果刷入为10位，如果是CN打头的初步认定为Customer SN，如在Product．CustSN中不存在，则提示“非法的Customer SN”
                //if (inputData.Length == 10 && inputData.Substring(0, 2) == "CN")
                if (CommonImpl.CheckCustSNPreFix2(inputData))
                {
                    var currentProduct = CommonImpl.GetProductByInput(inputData, CommonImpl.InputTypeEnum.CustSN);
                    if (currentProduct == null)
                    {
                        FisException fe = new FisException("PAK042", new string[] { inputData });  //此Customer S/N %1 不存在！
                        throw fe;
                    }
                    else if (string.IsNullOrEmpty(currentProduct.Model))
                    {
                        FisException fe = new FisException("PAK028", new string[] { inputData });  //该Customer SN %1 还未与Model绑定！
                        throw fe;
                    }
                    else model = currentProduct.Model;
                }
                else
                {
                    Model modelItem = itemRepository.Find(inputData);
                    if (modelItem == null)
                    {
                        var currentProduct = CommonImpl.GetProductByInput(inputData, CommonImpl.InputTypeEnum.CustSN);
                        if (currentProduct == null || string.IsNullOrEmpty(currentProduct.Model))
                        {
                            FisException fe = new FisException("CHK079", new string[] { inputData });   //找不到与此序号 %1 匹配的Product! 
                            throw fe;
                        }
                        else model = currentProduct.Model;

                    }
                    else model = inputData;

                }

                IModelWeightRepository itemRepositoryModelWeight = RepositoryFactory.GetInstance().GetRepository<IModelWeightRepository>();

                DataTable modelWeight = itemRepositoryModelWeight.GetModelWeightItem(model);
                if (modelWeight == null || modelWeight.Rows.Count == 0)
                {
                    //This Model, there is no standard weight, please go to the weighing.
                    //该Model尚无标准重量，请先去称重。
                    List<string> erpara = new List<string>();
                    erpara.Add(model);
                    FisException ex;
                    ex = new FisException("PAK123", erpara);
                    throw ex;
                }

                result = modelWeight.Rows[0][1].ToString();

                if (result == "")
                {
                    //该Model尚无标准重量，请先去称重。
                    List<string> erpara = new List<string>();
                    erpara.Add(model);
                    FisException ex;
                    ex = new FisException("PAK123", erpara);
                    throw ex;
                }

                ModelWeightDef item = new ModelWeightDef();
                item.Model = model;
                item.UnitWeight = result;
                return item;
            }
            catch (FisException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new SystemException(e.Message);
            }

        }

        /// <summary>
        /// 保存修改的ModelWeight
        /// </summary>
        /// <param name="item">item</param>
        /// <returns></returns>
        public void SaveModelWeightItem(ModelWeightDef item)
        {

            try
            {

                IModelWeightRepository itemRepositoryModelWeight = RepositoryFactory.GetInstance().GetRepository<IModelWeightRepository>();
                //看取得的数据是否有, 防止update的是空记录，但[PAK_SkuMasterWeight_FIS]中加入了记录
                DataTable modelWeight = itemRepositoryModelWeight.GetModelWeightItem(item.Model);
                if (modelWeight == null || modelWeight.Rows.Count == 0)
                {
                    //该Model尚无标准重量，请先去称重。
                    List<string> erpara = new List<string>();
                    erpara.Add(item.Model);
                    FisException ex;
                    ex = new FisException("PAK123", erpara);
                    throw ex;
                }

                IPizzaRepository itemRepositoryPizza = RepositoryFactory.GetInstance().GetRepository<IPizzaRepository>();

                ModelWeightInfo setValue = new ModelWeightInfo();
                setValue.unitWeight = Decimal.Parse(item.UnitWeight);
                setValue.editor = item.Editor;
                setValue.udt = DateTime.Now;

                ModelWeightInfo condition = new ModelWeightInfo();
                condition.model = item.Model;

                PakSkuMasterWeightFisInfo pakSkuMasterWeight = new PakSkuMasterWeightFisInfo();
                pakSkuMasterWeight.model = item.Model;
                pakSkuMasterWeight.weight = setValue.unitWeight;
                pakSkuMasterWeight.cdt = setValue.udt;

                UnitOfWork uow = new UnitOfWork();
                itemRepositoryModelWeight.UpdateModelWeightDefered(uow, setValue, condition);
                itemRepositoryPizza.DeletetPakSkuMasterWeightFisByModelDefered(uow, item.Model);
                itemRepositoryPizza.InsertPakSkuMasterWeightFisDefered(uow, pakSkuMasterWeight);
                uow.Commit();

            }
            catch (FisException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new SystemException(e.Message);
            }
        }

        private static String Null2String(Object _input)
        {
            if (_input == null)
            {
                return "";
            }
            return _input.ToString().Trim();
        }

        private bool CheckHaveATS1(IList<IProductPart> productParts, IPartRepository ipartRepository )
        {
            bool isNoAT=true;
            foreach (ProductPart iprodpart in productParts)
            {
                if (iprodpart.BomNodeType == "AT")
                {
                    IPart curPart = ipartRepository.GetPartByPartNo(iprodpart.PartID);
                    if (curPart.BOMNodeType == "AT" && curPart.Descr == "ATSN1")
                    {
                        isNoAT = false;
                        break;
                    }
                }
            }
            return isNoAT;
        }
        public ArrayList GetPODLabelPathAndSite()
        {
           ArrayList arr = new ArrayList();
           string site = "";
           string path = "";
           CommonImpl2 cm2 = new CommonImpl2();
           site = cm2.GetSite();
           if (site == "ICC")
           { path = cm2.GetValueFromSysSetting("PODLabelPath"); }
           arr.Add(site);
           arr.Add(path);
           return arr;
        }

        public string GetCqPodLabelColor(string model)
        {
           
            IMES.FisObject.Common.FisBOM.IBOMRepository ibomRepository = RepositoryFactory.GetInstance().GetRepository<IMES.FisObject.Common.FisBOM.IBOMRepository>();
            IHierarchicalBOM sessionBOM = null;
            sessionBOM = ibomRepository.GetHierarchicalBOMByModel(model);
            IList<IBOMNode> bomNodeLst = new List<IBOMNode>();
            bomNodeLst = sessionBOM.FirstLevelNodes;
            CommonImpl2 cm2 = new CommonImpl2();
            string site = cm2.GetValueFromSysSetting("Site");
            if (site != "ICC") { return ""; }
            string color = "Black";
            IList<string> lstValue
                     = cm2.GetConstValueTypeByType("POD_White_Lable_PN").Where(x => x.value != "").Select(x => x.value).ToList();
                if (lstValue != null && lstValue.Count > 0)
                {
                    foreach (IBOMNode ibomnode in bomNodeLst)
                    {
                        if (lstValue.Contains(ibomnode.Part.PN))
                        { color = "White"; ; break; }

                    }
                }
                return color;
        }

        public string CheckFAI(string model)
        {
            IModelRepository iModelRepository = RepositoryFactory.GetInstance().GetRepository<IModelRepository>();
            FAIModelInfo faimodel = iModelRepository.GetFAIModelByModelWithTrans(model);
            string r = "";
            if (faimodel != null && faimodel.PAKState=="Hold")
            {
                FisException err = new FisException("CQCHK1096", new List<string>());
                r = err.mErrmsg;
            }
            return r;
        }
    }
}
