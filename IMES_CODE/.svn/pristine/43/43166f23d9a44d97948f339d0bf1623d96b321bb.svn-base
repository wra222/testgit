/*
 * INVENTEC corporation ©2012 all rights reserved. 
 * Description:Service for UploadShipmentData(for Docking) Page
 *             
 * UI:CI-MES12-SPEC-PAK-UI Upload Data to SAP for Docking.docx
 * UC:CI-MES12-SPEC-PAK-UC Upload Data to SAP for Docking.docx
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2012-05-30  itc202017             (Reference Ebook SourceCode) Create
 * Known issues:
 * TODO:
*/

using System;
using System.Collections;
using System.Collections.Generic;
using IMES.DataModel;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PAK.DN;
using IMES.FisObject.PAK.CartonSSCC;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Docking.Interface.DockingIntf;
using log4net;
using IMES.FisObject.Common.Part;
using System.Text.RegularExpressions;
namespace IMES.Docking.Implementation
{
    /// <summary>
    /// IMES service for UploadShipmentData.
    /// </summary>
    public class _UploadShipmentData : MarshalByRefObject, IUploadShipmentData
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
        private IDeliveryRepository deliveryRepository = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
        private ICartonSSCCRepository cartonRepository = RepositoryFactory.GetInstance().GetRepository<ICartonSSCCRepository>();
        private IPartRepository partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
        #region IUploadShipmentData Members
        /// <summary>
        /// 获取表格数据信息
        /// </summary>
        public IList<S_RowData_UploadShipmentData> GetTableData(DateTime fromDate, DateTime toDate, bool bAllData)
        {
            logger.Debug("(_UploadShipmentData)GetTableData start, fromDate:" + fromDate.ToString("yyyy-MM-dd") + " toDate:" + toDate.ToString("yyyy-MM-dd") + " bAllData:" + bAllData);
            try
            {
                IList<S_RowData_UploadShipmentData> ret = new List<S_RowData_UploadShipmentData>();
                IList<Delivery> dlvList = deliveryRepository.GetDeliveriesByShipDateRange(fromDate, toDate);
                foreach (Delivery tmp in dlvList)
                {
                    if (!bAllData && tmp.Status.StartsWith("9"))
                    {
                        continue;
                    }

                    //UC updated(2012-07-31):Docking UpLoad Shipment Data to SAP页面除掉整机的船务数据
                    if (tmp.ModelName.StartsWith("PC"))
                    {
                        continue;
                    }

                    S_RowData_UploadShipmentData ele;
                    ele.m_date = tmp.ShipDate;
                    ele.m_dn = tmp.DeliveryNo;
                    ele.m_pn = tmp.PoNo;
                    ele.m_model = tmp.ModelName;
                    ele.m_qty = tmp.Qty;
                    ele.m_status = tmp.Status;
                    ele.m_pack = productRepository.GetCombinedQtyByDN_WithCartonSNNotNull(tmp.DeliveryNo);
                    ele.m_paqc = "";
                    ele.m_udt = tmp.Udt;
                    IList<string> toCheckList = productRepository.GetProductIDListNeedToCheck(tmp.DeliveryNo);
                    foreach (string str in toCheckList)
                    {
                        ProductQCStatus qcs = productRepository.GetNewestProductQCStatus(str);
                        if (qcs != null && (qcs.Status.Equals("8") || qcs.Status.Equals("A")))
                        {
                            ele.m_paqc = "PQ";
                            break;
                        }
                    }

                    ret.Add(ele);
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
                logger.Debug("(_UploadShipmentData)GetTableData end, fromDate:" + fromDate.ToString("yyyy-MM-dd") + " toDate:" + toDate.ToString("yyyy-MM-dd") + " bAllData:" + bAllData);
            }            
        }

        /// <summary>
        /// 获取记入c:\serial.txt的信息
        /// </summary>
        /// <param name="dns">DN list</param>
        public IList<string> GetFileData(IList<S_DnUdt> dns)
        {
            logger.Debug("(_UploadShipmentData)GetFileData start");
            try
            {
                IList<string> ret = new List<string>();
                //foreach (string tmpDn in dns)
                string tmpDn;
                foreach (S_DnUdt sDnUdt in dns)
                {
                    tmpDn = sDnUdt.dn;
                    IList<string> subRet = new List<string>();
                    int caseNoByDN = 2; //0 - case A; 1 - case B; 2 - case C

                    Delivery dlv = deliveryRepository.Find(tmpDn);
                    //Check Udt..
                    if (sDnUdt.udt != dlv.Udt)
                    {
                        throw new FisException("PAK173", new string[] { tmpDn });

                    }
                    //Check Udt..

                    //IList<string> subRet = new List<string>();
                    
                    //Delivery dlv = deliveryRepository.Find(tmpDn);
                    
                    IList<DNInfoForUI> dlvInfoList = deliveryRepository.GetDNInfoList(tmpDn);
                    IList<IProduct> proList = productRepository.GetProductListByDeliveryNo(tmpDn);
                    foreach (IProduct p in proList)
                    {
                        string s = "";

                        string palletNo = "";
                        //UC Exception 1
                        if (p.PalletNo.Equals(""))
                        {
                            throw new FisException("PAK111", new string[] { tmpDn });
                            //subRet.Clear();
                            //subRet.Add(tmpDn + " Pallet Qty is error!");
                            //break;
                        }
                        else
                        {
                            //PalletNo：若Product.PalletNo的前两位等于NA or BA时，PalletNo=’~’+Product.PalletNo从第3位开始取10位；否则PalletNo=Product.PalletNo+’~’
                            if (p.PalletNo.StartsWith("NA") || p.PalletNo.StartsWith("BA"))
                            {
                                palletNo = "~" + p.PalletNo.Substring(2, 10);
                            }
                            else
                            {
                                palletNo = p.PalletNo + "~";
                            }
                        }
                        
                        //Boxid：Product 结合的Carton在CartonInfo里InfoType=BoxId或UCC对应记录的InfoValue
                        string boxId = "";

                        CartonInfoInfo cond = new CartonInfoInfo();
                        cond.cartonNo = p.CartonSN;
                        cond.infoType = "BoxId";

                        IList<CartonInfoInfo> pInfoList = cartonRepository.GetCartonInfoInfo(cond);
                        if (pInfoList.Count <= 0)
                        {
                            cond.infoType = "UCC";
                            pInfoList = cartonRepository.GetCartonInfoInfo(cond);
                        }

                        if (pInfoList.Count > 0)
                        {
                            boxId = pInfoList[0].infoValue;
                        }
                        
                        s += dlv.DeliveryNo.Substring(0, 10);
                        s += "~";
                        s += dlv.DeliveryNo.Substring(dlv.DeliveryNo.Length - 6);
                        s += "~";
                        s += dlv.PoNo;
                        s += "~";
                        s += palletNo;
                        s += "~";
                        s += p.CUSTSN;
                        s += "~";
                        s += boxId;
                        s += "~1~";
                        s += p.Model.Substring(1,1)=="C" ? "C" : "O";
                        s += "~~~~~";
                        subRet.Add(s);
                    }
                    foreach (string ele in subRet)
                    {
                        ret.Add(ele);
                    }
                }
                //   Check Udt again
                foreach (S_DnUdt dnUdt in dns)
                {
                    Delivery dnObj = deliveryRepository.Find(dnUdt.dn);
                    if (dnUdt.udt != dnObj.Udt)
                    {
                        throw new FisException("PAK173", new string[] { dnUdt.dn });

                    }

                }
                //Check Udt again
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
                logger.Debug("(_UploadShipmentData)GetFileData end");
            }
        }
        
        /// <summary>
        /// 成功生成文件后，每个Delivery 需要更新状态
        /// </summary>
        public void ChangeDNStatus(IList<string> dns)
        {
            logger.Debug("(_UploadShipmentData)ChangeDNStatus start");
            try
            {
                string[] input = new string[dns.Count];
                for (int i = 0; i < dns.Count; i++)
                {
                    input[i] = dns[i];
                }
                //deliveryRepository.UpdateDeliveryForStatusChange(input, "9");
                deliveryRepository.UpdateMultiDeliveryForStatusChange(input, "98");
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
                logger.Debug("(_UploadShipmentData)ChangeDNStatus end");
            }
        }

        public IList<S_RowData_UploadShipmentData> GetTableDataByDnList(string dnList, bool bAllData)
        {
            logger.Debug("(_UploadShipmentData)GetTableDataByDnList start, dnList:" + dnList + " bAllData:" + bAllData);
            try
            {
                IList<S_RowData_UploadShipmentData> ret = new List<S_RowData_UploadShipmentData>();
                string[] dnArr = dnList.Split(',');
                List<Delivery> dlvList = new List<Delivery>();
                foreach (String dn in dnArr)
                {
                    if (!string.IsNullOrEmpty(dn))
                    {
                        Delivery tmpD = deliveryRepository.GetDelivery(dn);
                        if (tmpD != null)
                        {
                            dlvList.Add(tmpD);
                        }
                    }


                }
                foreach (Delivery tmp in dlvList)
                {
                    if (!bAllData && tmp.Status.StartsWith("9"))
                    {
                        continue;
                    }

                    //UC updated(2012-07-31):Docking UpLoad Shipment Data to SAP页面除掉整机的船务数据
                    if (tmp.ModelName.StartsWith("PC"))
                    {
                        continue;
                    }

                    S_RowData_UploadShipmentData ele;
                    ele.m_date = tmp.ShipDate;
                    ele.m_dn = tmp.DeliveryNo;
                    ele.m_pn = tmp.PoNo;
                    ele.m_model = tmp.ModelName;
                    ele.m_qty = tmp.Qty;
                    ele.m_status = tmp.Status;
                    ele.m_pack = productRepository.GetCombinedQtyByDN_WithCartonSNNotNull(tmp.DeliveryNo);
                    ele.m_paqc = "";
                    ele.m_udt = tmp.Udt;
                    IList<string> toCheckList = productRepository.GetProductIDListNeedToCheck(tmp.DeliveryNo);
                    foreach (string str in toCheckList)
                    {
                        ProductQCStatus qcs = productRepository.GetNewestProductQCStatus(str);
                        if (qcs != null && (qcs.Status.Equals("8") || qcs.Status.Equals("A")))
                        {
                            ele.m_paqc = "PQ";
                            break;
                        }
                    }

                    ret.Add(ele);
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
                logger.Debug("(_UploadShipmentData)GetTableDataByDnList end, dnList:" + dnList + " bAllData:" + bAllData);
            }            

        }
        #endregion
    }
}
