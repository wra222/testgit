/*
 * INVENTEC corporation (c)2011 all rights reserved. 
 * Description: Pallet Verify
 * UI:CI-MES12-SPEC-PAK-UI Pallet Verify.docx 
 * UC:CI-MES12-SPEC-PAK-UC Pallet Verify.docx                        
 * Update: 
 * Date         Name                Reason 
 * ==========   =================   =====================================
 * 2011-11-08   Chen Xu  itc208014  create
 * 
 * UC Revision: 7366:修正POD 栏位数据的获取方法 
 * UC Revision: 7636:要求Edits 文档所在的网络路径可配置
 * Known issues:
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;
using IMES.Station.Interface.CommonIntf;
using IMES.Station.Interface.StationIntf;
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
using System.Data;
using IMES.FisObject.PAK.Pizza;



namespace IMES.Station.Implementation
{
    /// <summary>
    /// Pallet Verify
    /// station : 9A
    /// 本站实现的功能：
    ///     检查Pallet 上的所有PRODUCT；
    ///     列印Ship to Pallet Label；
    ///     内销要额外列印一张Pallet CPMO Label
    /// </summary> 

    public class PalletVerify : MarshalByRefObject, IPalletVerify
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        

        #region IPalletVerify Members

        /// <summary>
        /// 刷第一个SN时，调用该方法启动工作流，根据输入CustSN获取PalletInfo,成功后调用InputCustSn
        /// 将custSN放到Session.CustSN中(string)
        /// 在获取了Pallet No 后，要进行Delivery Download Check，以保证并板情况下，该Pallet 所对应的Delivery 资料Download 完毕。
        /// 检查方法如下：
        ///     取得Customer S/N 绑定的Delivery No (IMES_FA..Product.DeliveryNo)
        ///     取得该Delivery No 的Consolidated 属性（IMES_PAK.DeliveryInfo）
        ///     如果Consolidated 属性存在，则解析Consolidated 属性（Consolidated 属性以'/' 为分隔符，分为两部分，前面的为Consolidate No，后面为Delivery数量），得到并板的Consolidate No 和Delivery 数量
        ///     使用Consolidate No 检索IMES_PAK.DeliveryInfo 表，取得相关记录，统计这些记录共有多少个不同的LEFT(DeliveryNo，10)，该数据如果小于前面查询到的Delivery 的Consolidated 属性中定义的并板的Delivery 数量，则报告错误：“Delivery No 未完全Download!”
        ///     SELECT COUNT(DISTINCT LEFT(DeliveryNo,10)) AS Expr1
        ///     FROM IMES_PAK.dbo.DeliveryInfo
        ///     where InfoValue like @ConsolidateNo and InfoType = 'Consolidated'
        /// 返回ArrayList
        /// </summary>
        public ArrayList InputFirstCustSn(string firstSn, string line, string editor, string station, string customer, out int index, out string labeltypeBranch)
        {
            logger.Debug("(PalletVerifyImpl)InputCustSNOnCooLabel start, custsn:" + firstSn + "pdLine:" + line + "editor:" + editor + "station:" + station + "customer:" + customer);

            FisException ex;
            List<string> erpara = new List<string>();
            ArrayList retLst = new ArrayList();

            try
            {
                //根据输入的Customer S/N, 获得对应的Product
                var currentProduct = CommonImpl.GetProductByInput(firstSn, CommonImpl.InputTypeEnum.CustSN);
                if (string.IsNullOrEmpty(currentProduct.ProId))
                {
                    erpara.Add(firstSn);
                    ex = new FisException("CHK152", erpara);    //您所刷入的Customer S/N 无效 ,请重新刷入！
                    throw ex;

                }

                //取得Customer S/N 绑定的Delivery No :IMES_FA..Product.DeliveryNo
                if (string.IsNullOrEmpty(currentProduct.DeliveryNo))
                {
                    erpara.Add(firstSn);
                    ex = new FisException("PAK020", erpara);    //该Customer S/N还未与DN绑定!
                    throw ex;
                }
                string curDn = currentProduct.DeliveryNo;

                //获取Pallet No : IMES_FA..Product.PalletNo
                if (string.IsNullOrEmpty(currentProduct.PalletNo))
                {
                    erpara.Add(firstSn);
                    ex = new FisException("PAK021", erpara);     //该Customer S/N还未与Pallet绑定!
                    throw ex;
                }
                string currentPalletNo = currentProduct.PalletNo;

                if (currentPalletNo.Substring(0, 2) == "BA" || currentPalletNo.Substring(0, 2) == "NA")
                {
                    erpara.Add(currentPalletNo);
                    ex = new FisException("PAK044", erpara);     //散装Pallet，不能使用此功能！
                    throw ex;
                }

                //获取Pallet Info
                IPalletRepository iPalletRepository = RepositoryFactory.GetInstance().GetRepository<IPalletRepository, Pallet>();
                Pallet CurrentPallet = null;
                CurrentPallet = iPalletRepository.Find(currentPalletNo);
                if (CurrentPallet == null)
                {
                    erpara.Add(currentProduct.PalletNo);
                    ex = new FisException("CHK106", erpara);    // Pallet不存在！
                    throw ex;
                }
                
          
                //if (!CurrentPallet.IsPalletFull(string.Empty))
                //{
                //    FisException fe = new FisException("CHK122", new string[] { currentProduct.PalletNo });
                //    throw fe;
                //}
                

                // 获取该Pallet 上的SKU 列表
                // 该Pallet 上的SKU 列表:IMES_FA..Product.PalletNo = @PalletNo的所有记录
                // SKU 列表显示如下信息：Product ID | Customer S/N | PAQC | POD | WC | Collection Data
                // 按照Customer S/N 升序排序
                IProductRepository iproductRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IMES.FisObject.FA.Product.IProduct>();
                IList<ProductModel> prodModelLst = new List<ProductModel>();
                prodModelLst = iproductRepository.GetProductListByPalletNoOrderByCustSN(currentPalletNo);

                if (prodModelLst == null || prodModelLst.Count <= 0)
                {
                    erpara.Add(firstSn);
                    ex = new FisException("CHK079", erpara);    //找不到与此序号 %1 匹配的Product! 
                    throw ex;
                }
                IList<string> productNoList = new List<string>();
                IList<string> custSnList = new List<string>();
                int i = 0;
                index = 0;
                foreach (ProductModel prod in prodModelLst)
                {
                    productNoList.Add(prod.ProductID);
                    custSnList.Add(prod.CustSN);
                    
                    if (prod.ProductID == currentProduct.ProId && prod.CustSN == firstSn)
                    {
                        index = i+1;
                       // break;
                    }
                    else i++;
                }

                // retLst[0]-[2]
                retLst.Add(currentPalletNo);
                retLst.Add(productNoList);
                retLst.Add(custSnList);
     

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
                    RouteManagementUtils.GetWorkflow(station, "PalletVerify.xoml", "PalletVerify.rules", out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    currentSession.AddValue(Session.SessionKeys.CustSN, firstSn);
                    currentSession.AddValue(Session.SessionKeys.PalletNo, currentPalletNo);
                    currentSession.AddValue(Session.SessionKeys.Pallet, CurrentPallet);
                    currentSession.AddValue(Session.SessionKeys.Product, currentProduct);
                    currentSession.AddValue(Session.SessionKeys.ProdNoList, productNoList); 
                    currentSession.AddValue(Session.SessionKeys.NewScanedProductIDList, productNoList); // UpdateProductListStatusByProdID (Update IMES_FA..ProductStatus – 更新Pallet 上所有PRODUCT 的状态)
                    currentSession.AddValue(Session.SessionKeys.CustomSnList, custSnList);
                    currentSession.AddValue(Session.SessionKeys.DeliveryNo, curDn);
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
                
                               
                
                //Delivery Download Check，以保证并板情况下，该Pallet 所对应的Delivery 资料Download 完毕。
                //取得该Delivery No 的Consolidated 属性（IMES_PAK.DeliveryInfo）
                //如果Consolidated 属性存在，则解析Consolidated 属性（Consolidated 属性以'/' 为分隔符，分为两部分，前面的为Consolidate No，后面为Delivery数量），得到并板的Consolidate No 和Delivery 数量
                IDeliveryRepository ideliveryRepository = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
                string Consolidated = string.Empty;
                Consolidated = ideliveryRepository.GetDeliveryInfoValue(curDn, "Consolidated");
                // SVN 2569: 如果Consolidated 属性不存在或者为空，则不用进行Delivery Download Check
                if (!string.IsNullOrEmpty(Consolidated))
                {
                    string[] pattern = Consolidated.Split('/');
                    string ConsolidateNo = string.Empty;
                    int dnQty = 0;
                    if (pattern.Length.ToString() != "2" || string.IsNullOrEmpty(pattern[0]) || string.IsNullOrEmpty(pattern[1]))
                    {
                        erpara.Add(curDn);
                        ex = new FisException("PAK024", erpara);  //找不到该Delivery No 的Consolidated 属性
                        throw ex;
                    }
                    ConsolidateNo = pattern[0];
                    dnQty = Int32.Parse(pattern[1]);

                    //使用Consolidate No 检索IMES_PAK.DeliveryInfo 表，取得相关记录，统计这些记录共有多少个不同的LEFT(DeliveryNo，10)，该数据如果小于前面查询到的Delivery 的Consolidated 属性中定义的并板的Delivery 数量，则报告错误：“Delivery No 未完全Download!”
                    int DistinctDNQty = 0;
                    DistinctDNQty = ideliveryRepository.GetDistinctDeliveryNo(ConsolidateNo);
                    if (DistinctDNQty < dnQty)
                    {
                        erpara.Add(curDn);
                        ex = new FisException("PAK018", erpara);    //Delivery No 未完全Download!
                        throw ex;
                    }

                    int SumCartonQty = 0;
                    int SumDnPallletQty = 0;
                    IList<Delivery> dnList = ideliveryRepository.GetDeliveryListByInfoTypeAndValue("Consolidated", Consolidated);
                    foreach (Delivery dn in dnList)
                    {

                        SumCartonQty += dn.Qty;
                        int DnPallletQty = ideliveryRepository.GetSumDeliveryQtyOfACertainDN(dn.DeliveryNo);
                        SumDnPallletQty += DnPallletQty;
                    }
                    //临时注释掉，保出货
                    //if (SumCartonQty != SumDnPallletQty)
                    //{
                    //    //从整机库get
                    //    {
                    //        erpara.Add(currentProduct.PalletNo);
                    //        ex = new FisException("CHK903", erpara);    //PALLET  未完全Download!
                    //        throw ex;
                    //    }
                    //}
                }
                // PAQC –retLst[3]: 使用ProductID = @ProductId and Tp = 'PAQC' 查询IMES_FA..QCStatus 表取Udt 最新的记录，如果该记录的Status = '8'，则PAQC 栏位显示'PAQC'；Status = '9'，则PAQC 栏位显示'Pass'；Status = 'A'，则PAQC 栏位显示'Fail'；Status = '1'，则PAQC 栏位显示'No Check'；
                IList<string> PAQCStatusLst = new List<string>();
                foreach ( string iprod in productNoList)
                {
                    ProductQCStatus qcsStatus = ideliveryRepository.GetQCStatus(iprod, "PAQC");
                    if (qcsStatus ==null || string.IsNullOrEmpty(qcsStatus.Status))
                    {
                        erpara.Add(iprod);
                        erpara.Add("PAQC");
                        ex = new FisException("PAK051", erpara);    //QCStatus 中Product%1 的参数 %2 不存在！
                        throw ex;
                    }
                    switch (qcsStatus.Status)
                    {
                        case "8":
                            PAQCStatusLst.Add("PAQC");
                            break;
                        case "9":
                           // ITC-1360-1235 : 小写
                           // PAQCStatusLst.Add("PASS");
                            PAQCStatusLst.Add("Pass");
                            break;
                        case "A":
                            PAQCStatusLst.Add("Fail");
                            break;
                        case "1":
                            PAQCStatusLst.Add("No Check");
                            break;
                        default:
                            PAQCStatusLst.Add("");
                            break;
                    }

                }
                
                retLst.Add(PAQCStatusLst);    


                // POD –retLst[4]: 检查IMES_FA..ProductLog，如果存在Station = 'PD' 的记录，则POD 栏位显示'PD'；如果存在Station = '86' 的记录，则POD 栏位显示'86'；
                // WC –retLst[5]: IMES_ProductStatus.Station
                // UC Revision: 7366:修正POD 栏位数据的获取方法 :检查IMES_FA..ProductLog，如果存在Station = 'PD' 的记录，但不存在Station = '86' 的记录时，则POD 栏位显示'PD'；如果存在Station = '86' 的记录，则POD 栏位显示'86'；两者都不存在时，显示为空
                // UC Revision: 7422:检查IMES_FA..ProductLog，如果存在Station = ’86' 的记录，则POD 栏位显示'86'；否则如果存在Station = 'PD' 的记录，则POD 栏位显示'PD'；两者都不存在时，显示为空
                IList<string> PODLst = new List<string>();
                IList<string> WCLst = new List<string>();
                foreach (string prodid in productNoList)
                {
                    IList<ProductLog> prodLogLst = new List<ProductLog>();
                    prodLogLst = iproductRepository.GetProductLogs(prodid, "86");
                    if (prodLogLst.Count> 0)
                    {
                        PODLst.Add("86");
                    }
                    else
                    {
                        prodLogLst = iproductRepository.GetProductLogs(prodid, "PD");
                        if (prodLogLst.Count > 0)
                        {
                            PODLst.Add("PD");
                        }
                        else
                        {
                            PODLst.Add("");
                        }
                    }

                    ProductStatusInfo productStatus = iproductRepository.GetProductStatusInfo(prodid);
                    if (productStatus.station != null)
                    {
                        WCLst.Add(productStatus.station);
                    }
                    else
                    {
                        erpara.Add(prodid);
                        ex = new FisException("PAK026", erpara);    //没有Product Status 站数据！
                        throw ex;
                    }

                }
                retLst.Add(PODLst);
                retLst.Add(WCLst);
                
                // Collection Data 为空 - retLst[6]
                retLst.Add("");

                // Pallet Qty -retLst[7]: (Sum(IMES_PAK..Delivery_Pallet.DeliveryQty))
                int palletQty =0;
                IList<DeliveryPallet> dnpltlist = new List<DeliveryPallet>();
                IList<string> DeliveryPerPalletList = new List<string>();
                IList<string> modelList = new List<string>();

                dnpltlist= iPalletRepository.GetDeliveryPallet(currentPalletNo);
                if (dnpltlist != null && dnpltlist.Count > 0)
                {
                   foreach (DeliveryPallet dp in dnpltlist)
                   {
                       palletQty += dp.DeliveryQty;
                       DeliveryPerPalletList.Add(dp.DeliveryID);
                       Delivery currentDelivery = ideliveryRepository.Find(dp.DeliveryID);
                       modelList.Add(currentDelivery.ModelName);
                   }
                }
                else
                {
                    erpara.Add(currentPalletNo);
                    ex = new FisException("PAK027", erpara);    //没有获得当前Pallet的DeliveryQty！
                    throw ex;
                }

               retLst.Add(palletQty.ToString());

               // retLst[8]: SVN 2569: 9.	Scan '9999' or 'EMEA':当前Delivery 的RegId （IMES_PAK..DeliveryInfo.InfoValue, Condition: InfoType = 'RegId'）属性
               string infoValue = string.Empty;
               infoValue = ideliveryRepository.GetDeliveryInfoValue(curDn, "RegId");
               if (String.IsNullOrEmpty(infoValue))
               {
                   //erpara.Add("RegId");
                   //ex = new FisException("PAK047", erpara);    //没有InfoType为 %1 的DeliveryInfo！
                   //throw ex;
                   infoValue = string.Empty;
               }
               retLst.Add(infoValue);

               //string PLEditsURL = string.Empty;
               //string PLEditsTemplate = string.Empty;
               //string PLEditsXML = string.Empty;
               //string PLEditsPDF = string.Empty;
               //string PLEditsImage = string.Empty;
               //string FOPFullFileName = string.Empty;
               
               IList<string> PDFPLLst = new List<string>();

               //当EDI_RegId = 'SNE' or 'SCE'时， Emea = 1；否则， Emea = 0
               decimal emea =0;

               //  SVN 2569: 自动单和手动单列印 (printflag =="C"，表示手动单；否则表示自动单)
               labeltypeBranch = string.Empty;
               string printflag = ideliveryRepository.GetDeliveryInfoValue(curDn, "Flag");
               if (printflag == "SNE" || printflag == "SCE")
               {
                   emea = 1;
               }
               else emea = 0;
              
                if (printflag == "C")
               {
                   string cpmoflag = ideliveryRepository.GetDeliveryInfoValue(curDn, "Carrier");
                   if (cpmoflag == "XJ")
                   {
                       labeltypeBranch = "X";   //手动单:Pallet Label + CPMO Label
                   }
                   else labeltypeBranch = "C";   // 手动单:Pallet Label 
               }
               else
               {
                   labeltypeBranch = "A"; // 自动单：Pallet Label 

                   //UC Revision: 7636:要求Edits 文档所在的网络路径可配置:
                   // （\\hp-iis\OUT\）需要查询SysSetting 表获取, 参考方法：select Value from SysSetting nolock where Name = 'EditsFISAddr'
                   IPartRepository partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
                   IList<string> PLEditsURLLst = partRepository.GetValueFromSysSettingByName("PLEditsURL");
                   if (PLEditsURLLst != null && PLEditsURLLst.Count > 0)
                   {
                       PDFPLLst.Add(PLEditsURLLst[0]); //PDFPLLst[0]
                   }

                   IList<string> PLEditsTemplateLst = partRepository.GetValueFromSysSettingByName("PLEditsTemplate");
                   if (PLEditsTemplateLst != null && PLEditsTemplateLst.Count > 0)
                   {
                       PDFPLLst.Add(PLEditsTemplateLst[0]);//PDFPLLst[1]
                   }

                   IList<string> PLEditsXMLLst = partRepository.GetValueFromSysSettingByName("PLEditsXML");
                   if (PLEditsXMLLst != null && PLEditsXMLLst.Count > 0)
                   {
                       PDFPLLst.Add(PLEditsXMLLst[0]);//PDFPLLst[2]
                   }

                   IList<string> PLEditsPDFLst = partRepository.GetValueFromSysSettingByName("PLEditsPDF");
                   if (PLEditsPDFLst != null && PLEditsPDFLst.Count > 0)
                   {
                       PDFPLLst.Add(PLEditsPDFLst[0]); //PDFPLLst[3]
                   }
                   IList<string> PLEditsImageLst = partRepository.GetValueFromSysSettingByName("PLEditsImage");
                   if (PLEditsImageLst != null && PLEditsImageLst.Count > 0)
                   {
                       PDFPLLst.Add(PLEditsImageLst[0]); //PDFPLLst[4]
                   }

                   IList<string> FOPFullFileNameLst = partRepository.GetValueFromSysSettingByName("FOPFullFileName");
                   if (FOPFullFileNameLst != null && FOPFullFileNameLst.Count > 0)
                   {
                       PDFPLLst.Add(FOPFullFileNameLst[0]); //PDFPLLst[5]

                   }
                   IList<string> printexepathLst = partRepository.GetValueFromSysSettingByName("PDFPrintPath");
                   if (printexepathLst != null && printexepathLst.Count > 0)
                   {
                       PDFPLLst.Add(printexepathLst[0]); //PDFPLLst[6]
                   }


               }
               currentSession.AddValue(Session.SessionKeys.labelBranch,labeltypeBranch);
               // retList[9]: PLEditsPDF
               // retLst.Add(PLEditsPDF);
               retLst.Add(PDFPLLst);

                //retList[10]:DeliveryPerPalletList: 获取与当前Pallet 结合的Delivery
                retLst.Add(DeliveryPerPalletList);

                // retList[11]:'This Customer S/N's Delivery No – Product.DeliveryNo
                retLst.Add(currentProduct.DeliveryNo);

                //retList[12]: emea
                retLst.Add(emea);

                //retList[13]: modelList
                retLst.Add(modelList);

               // WriteProductLog (Insert IMES_FA..ProductLog – 记录Pallet 上所有PRODUCT 的Log)
               IList<IMES.FisObject.FA.Product.IProduct> prodLst = iproductRepository.GetProductListByIdList(productNoList);
               currentSession.AddValue(Session.SessionKeys.ProdList, prodLst);

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
                logger.Error(e.Message, e);
                throw new SystemException(e.Message);
            }
            finally
            {
                logger.Debug("(PalletVerifyImpl)InputCustSNOnCooLabel end, custsn:" + firstSn + "pdLine:" + line + "editor:" + editor + "station:" + station + "customer:" + customer);
            }
        }

        /// <summary>
        /// 每次刷SN都调用该方法,SFC
        /// 将custSN放到Session.CustSN中(string)
        /// 如果匹配到SKU List 中的某条记录，如果该记录的Collection Data为空，(页面处理)： 如果该记录的PAQC 栏位等于'PAQC'，则报告错误：“该机器尚未完成PAQC！”；
        ///                                                                                如果该记录的PAQC 栏位等于'Fail'，则报告错误：“该机器PAQC Fail！”；
        ///                                                                                如果该记录的PD 栏位等于'PD'，则报告错误：“该机器尚未完成POD Label Check！”；
        ///                                                                                如果上述错误均未发生，则该记录的Collection Data 栏位显示刷入的Customer S/N，并且Scan Qty + 1；
        ///                                  如果该记录的Collection Data 不为空，则报告错误：“You had duplicate scan.”。(页面处理)
        /// 如果没有匹配到SKU List 中的任何记录，则报告错误：“非法数据！”（service处理）
        /// </summary>
        /// <param name="firstSn">firstSn</param>
        /// <param name="custSn">custSn</param>
        /// <returns></returns>
        public int InputCustSn(string firstSn, string custSn)
        {
            logger.Debug("(PalletVerifyImpl)ScanSN start, firstSn:" + firstSn + " custSn:" + custSn);

            try
            {
                FisException ex;
                List<string> erpara = new List<string>();
                var currentProduct = CommonImpl.GetProductByInput(firstSn, CommonImpl.InputTypeEnum.CustSN);
                string sessionKey = currentProduct.ProId;
                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, Session.SessionType.Product);


                if (currentSession == null)
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK021", erpara);
                    throw ex;
                }
                else
                {
                    currentSession.AddValue(Session.SessionKeys.CustSN, custSn);

                    //SFC : 根据刷入的 Another Customer S/N （custSn）, 获得其对应的Product
                    var newProduct = CommonImpl.GetProductByInput(custSn, CommonImpl.InputTypeEnum.CustSN);
                    if (string.IsNullOrEmpty(newProduct.ProId))
                    {
                        erpara.Add(custSn);
                        ex = new FisException("CHK152", erpara);    //您所刷入的Customer S/N 无效 ,请重新刷入！
                        throw ex;

                    }
                    IList<string> productNoList = (List<string>)(currentSession.GetValue(Session.SessionKeys.ProdNoList));
                    IList<string> custSnList = (List<string>)(currentSession.GetValue(Session.SessionKeys.CustomSnList));
                    int index = 0;
                    Boolean flag = false;
                    string currentPalletNo = (string)(currentSession.GetValue(Session.SessionKeys.PalletNo));
                    foreach (string sn in custSnList)
                    {
                        if (sn == custSn)
                        {
                            flag = true;
                            break;
                        }
                        index++;
                    }
                    if (!flag)
                    {
                        erpara.Add(currentPalletNo);
                        erpara.Add(custSn);
                        ex = new FisException("PAK144", erpara);    //非法数据！
                        throw ex;
                    }

                   // string currentPalletNo = (string)(currentSession.GetValue(Session.SessionKeys.PalletNo));
                    if ((string.IsNullOrEmpty(newProduct.ProId)) || (newProduct.PalletNo != currentPalletNo) || (newProduct.ProId != productNoList[index]))
                    {
                        erpara.Add(currentPalletNo);
                        erpara.Add(custSn);
                        
                        ex = new FisException("PAK144", erpara);    //非法数据！
                        throw ex;
                    }
                    
                    currentSession.AddValue(Session.SessionKeys.Product, newProduct);
                    currentSession.Exception = null;
                    currentSession.SwitchToWorkFlow();

                    if (currentSession.Exception != null)
                    {
                        if (currentSession.GetValue(Session.SessionKeys.WFTerminated) != null)
                        {
                            currentSession.ResumeWorkFlow();
                        }
                        throw currentSession.Exception;
                    }                    

                    return index;

                }
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
                logger.Debug("(PalletVerifyImpl)ScanSN end, firstSn:" + firstSn + " custSn:" + custSn);
            }
        }

        /// <summary>
        /// Save Data，返回打印重量标签的PrintItem，结束工作流
        /// Update IMES_FA..ProductStatus – 更新Pallet 上所有SKU 的状态
        /// Station – Pallet Verify 站号
        /// Status – '1'
        /// Editor – Editor (from UI)
        /// Udt – Current Time
        /// Insert IMES_FA..ProductLog – 记录Pallet 上所有SKU 的Log
        /// 列印Ship to Pallet Label；内销要额外列印一张Pallet CPMO Label
        /// Note: 当存在并板(即一个Pallet 结合了多个Delivery，
        ///       使用某个Pallet 在IMES_PAK..Delivery_Pallet 表中可以查询到多条记录的时候，可以认为是并板)情况时，
        ///       每个结合了当前Pallet 的Delivery 都需要列印一张Pallet Label
        /// </summary>
        /// <param name="firstSn">firstSn</param>
        /// <param name="printItems">printItems</param> 
        /// <param name="printparams1">printparams1</param>
        /// <param name="printparams2">printparams2</param>
        /// <returns></returns>
        public IList<PrintItem> Save(string firstSn, IList<PrintItem> printItems, out ArrayList printparams1, out ArrayList printparams2)
        {
            logger.Debug("(PalletVerifyImpl)Save start, firstSN:" + firstSn + "printItems" + printItems);

            FisException ex;
            List<string> erpara = new List<string>();

            var currentProduct = CommonImpl.GetProductByInput(firstSn, CommonImpl.InputTypeEnum.CustSN);
            string sessionKey = currentProduct.ProId;

            Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, Session.SessionType.Product);
            if (currentSession == null)
            {
                erpara.Add(sessionKey);
                ex = new FisException("CHK021", erpara);
                throw ex;
            }
            try
            {

                currentSession.Exception = null;
                currentSession.AddValue(Session.SessionKeys.IsComplete, true);
                currentSession.AddValue(Session.SessionKeys.PrintItems, printItems);

                currentSession.SwitchToWorkFlow();

                if (currentSession.Exception != null)
                {
                    if (currentSession.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        currentSession.ResumeWorkFlow();
                    }

                    throw currentSession.Exception;
                }

                printparams1 = new ArrayList();
                printparams2 = new ArrayList();
                IList printLists = new List<string>();

                Pallet CurrentPallet = (Pallet)(currentSession.GetValue(Session.SessionKeys.Pallet));
                string currentPalletNo = (string)(currentSession.GetValue(Session.SessionKeys.PalletNo));
                string curDn = (string)currentSession.GetValue(Session.SessionKeys.DeliveryNo);
                IDeliveryRepository ideliveryRepository = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
                IProductRepository iproductRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IMES.FisObject.FA.Product.IProduct>();

                //// bat打印：
                //string labeltypeBranch = (string)currentSession.GetValue(Session.SessionKeys.labelBranch);
                //if (labeltypeBranch == "C")    // 手动单:Pallet Label
                //{
                //    IList<DeliveryPalletInfo> DeliveryPalletInfoList = ideliveryRepository.GetDeliveryPalletListByPlt(currentPalletNo);
                //    if (DeliveryPalletInfoList == null || DeliveryPalletInfoList.Count <= 0)
                //    {
                //        erpara.Add(currentPalletNo);    //没有获得当前Pallet的DeliveryPallet！
                //        ex = new FisException("PAK045", erpara);
                //        throw ex;
                //    }
                //    foreach (DeliveryPalletInfo iDPI in DeliveryPalletInfoList) //存在并板
                //    {
                //        printLists = new List<string>();
                //        Delivery CurrentDelivery = ideliveryRepository.Find(iDPI.deliveryNo);
                //        if (CurrentDelivery == null)
                //        {
                //            erpara.Add(curDn);
                //            throw new FisException("CHK107", erpara);
                //        }
                //        printLists.Add(CurrentDelivery.PoNo);
                //        printLists.Add(iDPI.deliveryNo);
                //        printLists.Add(CurrentDelivery.ShipDate.Day.ToString());
                //        printLists.Add(currentPalletNo);
                //        printLists.Add(CurrentPallet.UCC);
                //        printLists.Add(CurrentDelivery.ModelName);

                //        printparams1.Add(printLists);
                //     }
                //}
                //else if (labeltypeBranch == "X")   //手动单:Pallet Label + CPMO Label
                //{

                //    IList<DeliveryPalletInfo> DeliveryPalletInfoList = ideliveryRepository.GetDeliveryPalletListByPlt(currentPalletNo);
                //    if (DeliveryPalletInfoList == null || DeliveryPalletInfoList.Count <= 0)
                //    {
                //        erpara.Add(currentPalletNo);    //没有获得当前Pallet的DeliveryPallet！
                //        ex = new FisException("PAK045", erpara);
                //        throw ex;
                //    }
                //    foreach (DeliveryPalletInfo iDPI in DeliveryPalletInfoList) //存在并板
                //    {
                //        printLists = new List<string>();
                //        Delivery CurrentDelivery = ideliveryRepository.Find(iDPI.deliveryNo);
                //        if (CurrentDelivery == null)
                //        {
                //            erpara.Add(curDn);
                //            throw new FisException("CHK107", erpara);
                //        }
                //        printLists.Add(CurrentDelivery.PoNo);
                //        printLists.Add(iDPI.deliveryNo);
                //        printLists.Add(CurrentDelivery.ShipDate.Day.ToString());
                //        printLists.Add(CurrentPallet.PalletNo);
                //        printLists.Add(CurrentPallet.UCC);
                //        printLists.Add(CurrentDelivery.ModelName);

                //        string delPartNo = ideliveryRepository.GetDeliveryInfoValue(iDPI.deliveryNo, "PartNo");
                //        string PN = string.Empty;
                //        if (!string.IsNullOrEmpty(delPartNo))
                //        {
                //            string[] pattern = delPartNo.Split('/');
                //            if (pattern.Length.ToString() == "2")
                //            {
                //                PN = pattern[1];
                //            }
                //        }
                //        printLists.Add(PN);
                //        printparams1.Add(printLists);
                //        IList<IMES.FisObject.FA.Product.IProduct> prodcutLst = iproductRepository.GetProductListByDeliveryNoAndPalletNo(curDn, currentPalletNo);
                //        if (prodcutLst == null || prodcutLst.Count <= 0)
                //        {
                //            erpara.Add(currentPalletNo);
                //            erpara.Add(curDn);
                //            throw new FisException("PAK046", erpara);
                //        }
                //        foreach (Product iprod in prodcutLst)
                //        {
                //            printparams2.Add(iprod.CUSTSN);
                //        }
                //    }                   
                //}
                //else if (labeltypeBranch == "A") // 自动单：Pallet Label 
                //{
                //    //Pallet Label 
                //    printparams1.Add(curDn); 
                //}


                ////模板打印：
                IList<DeliveryPalletInfo> DeliveryPalletInfoList = ideliveryRepository.GetDeliveryPalletListByPlt(currentPalletNo);
                if (DeliveryPalletInfoList == null || DeliveryPalletInfoList.Count <= 0)
                {
                    erpara.Add(currentPalletNo);    //没有获得当前Pallet的DeliveryPallet！
                    ex = new FisException("PAK045", erpara);
                    throw ex;
                }
                foreach (DeliveryPalletInfo iDPI in DeliveryPalletInfoList)
                {
                    printparams1.Add(iDPI.deliveryNo);
                    
                }

                return (IList<PrintItem>)currentSession.GetValue(Session.SessionKeys.PrintItems);
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
                logger.Debug("(PalletVerifyImpl)Save end, firstSn:" + firstSn + "printItems" + printItems);
            }
        }

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="uutSn">uutSn</param>
        public void Cancel(string uutSn)
        {
            logger.Debug("(PalletVerifyImpl)Cancel Start," + "uutSn:" + uutSn);
            try
            {
                var currentProduct = CommonImpl.GetProductByInput(uutSn, CommonImpl.InputTypeEnum.CustSN);
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
                logger.Debug("(PalletVerifyImpl)Cancel End," + "uutSn:" + uutSn);
            }

        }

        /// <summary>
        /// 调存储过程： HP_EDI.dbo.op_TemplateCheck_LANEW 
        /// </summary>
        /// <param name="dn">dn</param>
        /// <param name="docType">docType</param>
        /// <returns>DataTable</returns>
        public DataTable call_Op_TemplateCheckLaNew(string dn, string docType)
        {
            try
            {
                IPizzaRepository repPizza = RepositoryFactory.GetInstance().GetRepository<IPizzaRepository, Pizza>();
                DataTable TempDB = new DataTable();
                TempDB = repPizza.CallTemplateCheckLaNew(dn, docType);
               foreach (DataRow row in TempDB.Rows)
                {
                    if (row[0].ToString() == "ERROR")
                    {
                        FisException fe = new FisException("PAK105", new string[] { });  //存储过程HP_EDI.dbo.op_TemplateCheck_LANEW 报错，请检查！
                        throw fe;
                    }
                }
               // */
                return TempDB;
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
        /// 与Pallet绑定的所有Delivery
        /// </summary>
        /// <param name="palletNo">palletNo</param>
        /// <returns>list</returns>
        public IList<string> getDeliveryPalletList(string palletNo)
        {
            FisException ex;
            List<string> erpara = new List<string>();

            IDeliveryRepository ideliveryRepository = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
            IList<DeliveryPalletInfo> DeliveryPalletInfoList = ideliveryRepository.GetDeliveryPalletListByPlt(palletNo);
            if (DeliveryPalletInfoList == null || DeliveryPalletInfoList.Count <= 0)
            {
                erpara.Add(palletNo);    //没有获得当前Pallet的DeliveryPallet！
                ex = new FisException("PAK045", erpara);
                throw ex;
            }
            IList<string> DeliveryList = new List<string>();
            foreach (DeliveryPalletInfo iDPI in DeliveryPalletInfoList)
            {
                DeliveryList.Add(iDPI.deliveryNo);
            }
            return DeliveryList;

        }

        #endregion

        #region rePrint


        /// <summary>
        /// 重印
        /// </summary>
        /// <param name="palletNo">palletNo</param>
        /// <param name="reason">reason</param>
        /// <param name="line">line</param>
        /// <param name="editor">editor</param>
        /// <param name="station">station</param>
        /// <param name="customer">customer</param>
        /// <param name="printItems">printItems</param>
        /// <param name="printparams1">printparam1</param>
        /// <param name="labeltypeBranch">labeltypeBranch</param>
        /// <param name="modelList">modelList</param>
        /// <param name="PDFPLLst">PDFPLLst</param>
        public IList<PrintItem> rePrint(string palletNo, string reason, string line, string editor, string station, string customer, IList<PrintItem> printItems, out ArrayList printparams1, out string labeltypeBranch, out IList<string> PDFPLLst, out IList<string> modelList)
        {
            logger.Debug("(PalletVerifyOnlyImpl)rePrint Start," + "palletNo:" + palletNo + "pdLine:" + line + "editor:" + editor + "station:" + station + "customer:" + customer + "printItems" + printItems);
            FisException ex;
            List<string> erpara = new List<string>();
            try
            {
                IPalletRepository iPalletRepository = RepositoryFactory.GetInstance().GetRepository<IPalletRepository, Pallet>();

                var currentProduct = CommonImpl.GetProductByInput(palletNo, CommonImpl.InputTypeEnum.ProductIDOrCustSNOrPallet);

                if (currentProduct == null)
                {
                    erpara.Add(palletNo);
                    ex = new FisException("CHK079", erpara);    //找不到与此序号匹配的Product! 
                    throw ex;
                }

                //与Pallet 结合的Customer S/N 可以通过PalletNo = @PalletNo 查询IMES_FA..Product 得到
                IProductRepository iproductRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IMES.FisObject.FA.Product.IProduct>();
                IList<ProductModel> prodModelLst = new List<ProductModel>();

                prodModelLst = iproductRepository.GetProductListByPalletNoOrderByCustSN(palletNo);
                if (prodModelLst == null || prodModelLst.Count <= 0)
                {
                    erpara.Add(palletNo);
                    ex = new FisException("CHK079", erpara);    //找不到与此序号 %1 匹配的Product! 
                    throw ex;
                }

                //Customer S/N 是否Pass 过Pallet Verify ，可以通过查看IMES_FA..ProductLog 是否有Pass Pallet Verify站的记录来判断
                Boolean exitFlag = false;
                IList<ProductLog> prodLogLst = new List<ProductLog>();
                foreach (ProductModel prod in prodModelLst)
                {
                    //  prodLogLst = iproductRepository.GetProductLogs(prod.ProductID, "9A");
                    prodLogLst = iproductRepository.GetProductLogs(prod.ProductID, station);
                    //if (prodLogLst.Count > 0)
                    //{
                    //    exitFlag = true;
                    //    break;
                    //}

                    if (prodLogLst.Count <= 0)
                    {
                        //ITC-1360-1642:存在尚未Pass Pallet Verify 的Product，请先刷Pallet Verify！
                        FisException fe = new FisException("PAK120", new string[] { prod.ProductID });
                        throw fe;

                        //exitFlag = true;
                        //break;
                    }
                }
                //if (!exitFlag)
                //{
                //    //沒有任何Pass Pallet Verify 的Product，请先刷Pallet Verify！
                //    FisException fe = new FisException("PAK076", new string[] { });
                //    throw fe;
                //}

                //if (exitFlag)
                //{
                //    //ITC-1360-1642:存在尚未Pass Pallet Verify 的Product，请先刷Pallet Verify！
                //    FisException fe = new FisException("PAK120", new string[] { });
                //    throw fe;
                //}

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

                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow("PalletVerifyReprint.xoml", "", wfArguments);
                    currentSession.AddValue(Session.SessionKeys.PalletNo, palletNo);
                    currentSession.AddValue(Session.SessionKeys.PrintLogName, "PalletVerify");
                    currentSession.AddValue(Session.SessionKeys.PrintLogBegNo, palletNo);
                    currentSession.AddValue(Session.SessionKeys.PrintLogEndNo, palletNo);
                    currentSession.AddValue(Session.SessionKeys.PrintLogDescr, "PalletVerifyReprint");
                    currentSession.AddValue(Session.SessionKeys.Reason, reason);
                    currentSession.AddValue(Session.SessionKeys.PrintItems, printItems);
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

                ////模板打印：
                IDeliveryRepository ideliveryRepository = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();

                labeltypeBranch = string.Empty;
                string printflag = ideliveryRepository.GetDeliveryInfoValue(currentProduct.DeliveryNo, "Flag");
                if (printflag == "C")
                {
                    string cpmoflag = ideliveryRepository.GetDeliveryInfoValue(currentProduct.DeliveryNo, "Carrier");
                    if (cpmoflag == "XJ")
                    {
                        labeltypeBranch = "X";   //手动单:Pallet Label + CPMO Label
                    }
                    else labeltypeBranch = "C";   // 手动单:Pallet Label 
                }
                else labeltypeBranch = "A"; // 自动单：Pallet Label 

                printparams1 = new ArrayList();
                IList<DeliveryPalletInfo> DeliveryPalletInfoList = ideliveryRepository.GetDeliveryPalletListByPlt(palletNo);
                modelList = new List<string>();
                if (DeliveryPalletInfoList == null || DeliveryPalletInfoList.Count <= 0)
                {
                    erpara.Add(palletNo);    //没有获得当前Pallet的DeliveryPallet！
                    ex = new FisException("PAK045", erpara);
                    throw ex;
                }
                foreach (DeliveryPalletInfo iDPI in DeliveryPalletInfoList)
                {
                    printparams1.Add(iDPI.deliveryNo);
                    Delivery currentDelivery = ideliveryRepository.Find(iDPI.deliveryNo);
                    modelList.Add(currentDelivery.ModelName);
                }

                
                PDFPLLst = new List<string>();

                IPartRepository partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
                IList<string> PLEditsURLLst = partRepository.GetValueFromSysSettingByName("PLEditsURL");
                if (PLEditsURLLst != null && PLEditsURLLst.Count > 0)
                {
                    PDFPLLst.Add(PLEditsURLLst[0]); //PDFPLLst[0]
                }

                IList<string> PLEditsTemplateLst = partRepository.GetValueFromSysSettingByName("PLEditsTemplate");
                if (PLEditsTemplateLst != null && PLEditsTemplateLst.Count > 0)
                {
                    PDFPLLst.Add(PLEditsTemplateLst[0]);//PDFPLLst[1]
                }

                IList<string> PLEditsXMLLst = partRepository.GetValueFromSysSettingByName("PLEditsXML");
                if (PLEditsXMLLst != null && PLEditsXMLLst.Count > 0)
                {
                    PDFPLLst.Add(PLEditsXMLLst[0]);//PDFPLLst[2]
                }

                IList<string> PLEditsPDFLst = partRepository.GetValueFromSysSettingByName("PLEditsPDF");
                if (PLEditsPDFLst != null && PLEditsPDFLst.Count > 0)
                {
                    PDFPLLst.Add(PLEditsPDFLst[0]); //PDFPLLst[3]
                }
                IList<string> PLEditsImageLst = partRepository.GetValueFromSysSettingByName("PLEditsImage");
                if (PLEditsImageLst != null && PLEditsImageLst.Count > 0)
                {
                    PDFPLLst.Add(PLEditsImageLst[0]); //PDFPLLst[4]
                }

                IList<string> FOPFullFileNameLst = partRepository.GetValueFromSysSettingByName("FOPFullFileName");
                if (FOPFullFileNameLst != null && FOPFullFileNameLst.Count > 0)
                {
                    PDFPLLst.Add(FOPFullFileNameLst[0]); //PDFPLLst[5]

                }
                IList<string> printexepathLst = partRepository.GetValueFromSysSettingByName("PDFPrintPath");
                if (printexepathLst != null && printexepathLst.Count > 0)
                {
                    PDFPLLst.Add(printexepathLst[0]); //PDFPLLst[6]
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
                logger.Debug("(PalletVerifyOnlyImpl)rePrint End," + "palletNo:" + palletNo + "pdLine:" + line + "editor:" + editor + "station:" + station + "customer:" + customer + "printItems" + printItems);
            }

        }


        #endregion
    }


}
