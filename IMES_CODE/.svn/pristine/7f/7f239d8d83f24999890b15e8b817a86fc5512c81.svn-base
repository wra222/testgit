// INVENTEC corporation (c)2010 all rights reserved. 
// Description: PO Data模块上传DN、Pallet数据时分配BoxId
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-12-09   itc202017                     Create
// Known issues:
using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Drawing;
using System.Linq;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.UnitOfWork;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.Station;
using System.Collections.Generic;
using IMES.FisObject.PAK.DN;
using IMES.FisObject.PAK.Pallet;
using IMES.FisObject.Common.NumControl;
using IMES.Infrastructure.Repository.Common;
using IMES.DataModel;

namespace IMES.Activity
{
    /// <summary>
    /// PO Data模块上传DN、Pallet数据时分配BoxId
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      Upload Po Data for PL user(Normal/Domestic)
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         按条件获取一组BoxId并记录到ShipBoxDet中
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         DeliveryList, PalletList, DeliveryPalletList
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         无
    /// </para> 
    ///<para> 
    /// 输出：
    ///         无
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///          
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///              IDeliveryRepository
    /// </para> 
    /// </remarks>
    public partial class AllocateBoxId : BaseActivity
    {
        private static object _syncRoot_GetSeq = new object();
        /// <summary>
        /// 构造函数
        /// </summary>
        public AllocateBoxId()
        {
            InitializeComponent();
        }
                
        private IList<string> GetUCCIDList(int cnt)
        {
            try
            {
                SqlTransactionManager.Begin();
                lock (_syncRoot_GetSeq)
                {
                    IList<string> ret = new List<string>();
                    INumControlRepository numControlRepository = RepositoryFactory.GetInstance().GetRepository<INumControlRepository, NumControl>();

                    long curMax = 0;
                    bool bInsert = false;
                    string curMaxStr = numControlRepository.GetMaxUCCIDNumber("UCCID");
                    if (curMaxStr == null || curMaxStr == "")
                    {
                        bInsert = true;
                        curMax = 1;
                    }
                    else
                    {
                        curMax = long.Parse(curMaxStr);
                    }

                    if (curMax < 50000001)
                    {
                        curMax = 50000001;
                    }

                    for (int i = 1; i <= cnt; i++)
                    {
                        string s = "00000886985" + curMax.ToString("00000000");
                        int dig20 = 0;
                        int sum = 0;
                        for (int j = 0; j < 19; j++)
                        {
                            /*
                             * Answer to: ITC-1360-1155
                             * Description: Wrong processing while getting dig20.
                             */
                            if (j % 2 == 0) sum += (s[j] - '0') * 3;
                            else sum += s[j] - '0';
                        }
                        /*
                         * Answer to: ITC-1360-1153
                         * Description: Wrong processing while getting dig20.
                         */
                        dig20 = (10 - (sum % 10)) % 10;
                        s += dig20.ToString();
                        ret.Add(s);
                        curMax++;
                        if (curMax == 100000000)
                        {
                            //curMax = 1;
                            curMax = 50000001;
                        }
                    }

                    NumControl nc = new NumControl(-1, "UCCID", "", curMax.ToString(), "HP");
                    numControlRepository.SaveMaxNumber(nc, bInsert, "{0}");
                    SqlTransactionManager.Commit();

                    return ret;
                }
            }
            catch (Exception e)
            {
                SqlTransactionManager.Rollback();
                throw e;
            }
            finally
            {
                SqlTransactionManager.Dispose();
                SqlTransactionManager.End();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            IDeliveryRepository deliveryRepository = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
            IList<Delivery> deliveryList = CurrentSession.GetValue("DeliveryList") as IList<Delivery>;
            IList<Pallet> palletList = CurrentSession.GetValue("PalletList") as IList<Pallet>;
            IList<Pallet> notSavePalletList = CurrentSession.GetValue("NotSavePalletList") as IList<Pallet>;
            IList<DeliveryPalletInfo> dpList = CurrentSession.GetValue("DeliveryPalletList") as IList<DeliveryPalletInfo>;

            //Hashtable hOffset = new Hashtable();
            foreach (Delivery eleDlv in deliveryList)
            {
                IList<DeliveryInfo> infoList = eleDlv.DeliveryInfoes;
                string boxreg = "";
                string regid = "";
                string deport = "";
                string flag = "";
                string consolidated = "";
                string invoice = "";
                string ucc = "";
                foreach (DeliveryInfo info in infoList)
                {
                    if (info.InfoType == "BoxReg")
                    {
                        boxreg = info.InfoValue;
                        continue;
                    }
                    if (info.InfoType == "RegId")
                    {
                        regid = info.InfoValue;
                        continue;
                    }
                    if (info.InfoType == "Deport")
                    {
                        deport = info.InfoValue;
                        continue;
                    }
                    if (info.InfoType == "Flag")
                    {
                        flag = info.InfoValue;
                        continue;
                    }
                    if (info.InfoType == "UCC")
                    {
                        ucc = info.InfoValue;
                        continue;
                    }
                    if (info.InfoType == "Consolidated")
                    {
                        consolidated = info.InfoValue;
                        continue;
                    }
                    if (info.InfoType == "Invoice")
                    {
                        invoice = info.InfoValue;
                        continue;
                    }
                }
                    
                string prefix = "";
                if (boxreg != "")
                {
                    prefix = boxreg + "-";
                }
                else
                {
                    if (regid == "SNA" || regid == "SCA") prefix = "H410-";
                    else if (regid == "SNL") prefix = "LA" + deport;
                    else if (regid == "SNU" || regid == "SCU") prefix = "D7" + deport;
                    else if (regid == "SNE" || regid == "SCE") prefix = "63D7-";
                    else if (regid == "SAF" || regid == "SCN") prefix = "H4FN-0";
                    else if (regid == "SLA" || regid == "SCL") prefix = "LDIN";
                    /*else
                    {
                        List<string> erpara = new List<string>();
                        erpara.Add(eleDlv.DeliveryNo);
                        throw new FisException("CHK255", erpara);
                    }*/
                }

                foreach (DeliveryPalletInfo eleDP in dpList)
                {
                    if (eleDlv.DeliveryNo != eleDP.deliveryNo) continue;

                    if (deliveryRepository.CheckExistShipBoxDet(eleDP.deliveryNo, eleDP.palletNo))
                    {
                        List<string> erpara = new List<string>();
                        throw new FisException("CHK256", erpara);
                    }

                    Pallet findPlt = null;
                    foreach (Pallet elePlt in palletList)
                    {
                        if (eleDP.palletNo == elePlt.PalletNo)
                        {
                            findPlt = elePlt;
                            break;
                        }
                    }

                    if (findPlt == null)
                    {
                        foreach (Pallet elePlt in notSavePalletList)
                        {
                            if (eleDP.palletNo == elePlt.PalletNo)
                            {
                                findPlt = elePlt;
                                break;
                            }
                        }
                    }

                    if (findPlt == null) continue;

                    if (flag == "N" && ucc != "X")
                    {
                        if (prefix == "")
                        {
                            List<string> erpara = new List<string>();
                            erpara.Add(eleDlv.DeliveryNo);
                            throw new FisException("CHK255", erpara);
                        }
                        /*
                         * Answer to: ITC-1360-1135
                         * Description: Generate BoxId in turn.
                         */
                        /*
                         * Answer to: ITC-1360-1285
                         * Description: Bad casting from int to short.
                         */
                        /*
                        short offset = 0;
                        if (hOffset.Contains(prefix))
                        {
                            offset = (short)hOffset[prefix];
                            hOffset[prefix] = (short)(offset + eleDP.deliveryQty);
                        }
                        else
                        {
                            hOffset.Add(prefix, eleDP.deliveryQty);
                        }
                        IList<SnoCtrlBoxIdSQInfo> boxIdList = deliveryRepository.GetSnoCtrlBoxIdSQListByCust(prefix, (int)eleDP.deliveryQty, (int)offset);
                        */
                        try
                        {
                            SqlTransactionManager.Begin();
                            lock (_syncRoot_GetSeq)
                            {
                                IList<SnoCtrlBoxIdSQInfo> boxIdList = deliveryRepository.GetSnoCtrlBoxIdSQListByCust(prefix, (int)eleDP.deliveryQty, 0);
                                /*
                                 * Answer to: ITC-1360-1144
                                 * Description: Exception process with no efficient BoxId.
                                 */
                                if (boxIdList.Count < eleDP.deliveryQty)
                                {
                                    List<string> erpara = new List<string>();
                                    erpara.Add(prefix);
                                    throw new FisException("PAK094", erpara);
                                }

                                foreach (SnoCtrlBoxIdSQInfo item in boxIdList)
                                {
                                    ShipBoxDetInfo sbd = new ShipBoxDetInfo();
                                    if (consolidated == "")
                                    {
                                        sbd.shipment = eleDlv.DeliveryNo.Substring(0, 10);
                                    }
                                    else
                                    {
                                        sbd.shipment = consolidated.Substring(0, 10);
                                    }
                                    sbd.invioce = invoice;
                                    sbd.deliveryNo = eleDlv.DeliveryNo;
                                    sbd.plt = findPlt.PalletNo;
                                    sbd.boxId = item.boxId;
                                    sbd.snoId = "";
                                    sbd.editor = Editor;
                                    deliveryRepository.InsertShipBoxDetDefered(CurrentSession.UnitOfWork, sbd);
                                    //deliveryRepository.DeleteSnoCtrlBoxIdSQInfoDefered(CurrentSession.UnitOfWork, item);
                                    deliveryRepository.DeleteSnoCtrlBoxIdSQInfo(item);
                                }
                                SqlTransactionManager.Commit();
                            }
                        }
                        catch (Exception e)
                        {
                            SqlTransactionManager.Rollback();
                            throw e;
                        }
                        finally
                        {
                            SqlTransactionManager.Dispose();
                            SqlTransactionManager.End();
                        }
                    }
                    else if (ucc == "X")
                    {
                        if (findPlt.PalletNo.StartsWith("NA"))
                        {
                            ShipBoxDetInfo sbd = new ShipBoxDetInfo();
                            if (consolidated == "")
                            {
                                sbd.shipment = eleDlv.DeliveryNo.Substring(0, 10);
                            }
                            else
                            {
                                sbd.shipment = consolidated.Substring(0, 10);
                            }
                            sbd.invioce = invoice;
                            sbd.deliveryNo = eleDlv.DeliveryNo;
                            sbd.plt = findPlt.PalletNo;
                            sbd.boxId = findPlt.UCC;
                            sbd.snoId = "";
                            sbd.editor = Editor;
                            deliveryRepository.InsertShipBoxDetDefered(CurrentSession.UnitOfWork, sbd);
                        }
                        else
                        {
                            IList<string> uccidList = GetUCCIDList(eleDP.deliveryQty);
                            foreach (string uccid in uccidList)
                            {
                                ShipBoxDetInfo sbd = new ShipBoxDetInfo();
                                if (consolidated == "")
                                {
                                    sbd.shipment = eleDlv.DeliveryNo.Substring(0, 10);
                                }
                                else
                                {
                                    sbd.shipment = consolidated.Substring(0, 10);
                                }
                                sbd.invioce = invoice;
                                sbd.deliveryNo = eleDlv.DeliveryNo;
                                sbd.plt = findPlt.PalletNo;
                                sbd.boxId = uccid;
                                sbd.snoId = "";
                                sbd.editor = Editor;
                                deliveryRepository.InsertShipBoxDetDefered(CurrentSession.UnitOfWork, sbd);
                            }
                        }
                    }
                    else if (flag == "C")
                    {
                        for (int i = 1; i <= eleDP.deliveryQty; i++)
                        {
                            ShipBoxDetInfo sbd = new ShipBoxDetInfo();
                            if (consolidated == "")
                            {
                                sbd.shipment = eleDlv.DeliveryNo.Substring(0, 10);
                            }
                            else
                            {
                                sbd.shipment = consolidated.Substring(0, 10);
                            }
                            sbd.invioce = invoice;
                            sbd.deliveryNo = eleDlv.DeliveryNo;
                            sbd.plt = findPlt.PalletNo;
                            sbd.boxId = i.ToString();
                            sbd.snoId = "";
                            sbd.editor = Editor;
                            deliveryRepository.InsertShipBoxDetDefered(CurrentSession.UnitOfWork, sbd);
                        }
                    }
                }
            }
            return base.DoExecute(executionContext);
        }
    }
}
