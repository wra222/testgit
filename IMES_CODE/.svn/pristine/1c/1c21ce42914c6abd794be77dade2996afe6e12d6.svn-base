// INVENTEC corporation (c)2010 all rights reserved. 
// Description: PO Data模块保存上传的DeliveryNo、Pallet(for运筹User)
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
using System.Globalization; 
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
using IMES.FisObject.PAK.Pizza;
using IMES.FisObject.PAK.DN;
using IMES.DataModel;

namespace IMES.Activity
{
    /// <summary>
    /// PO Data模块保存上传的DeliveryNo、Pallet(for运筹User)
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      Upload Po Data for OB user(Normal/Domestic)
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         将[PAK.PAKComn]中本次导入的数据写入PAK..PoData_EDI中;
    ///         将[PAK.PAKPlatno]导入的数据保存到PoPlt_EDI表中
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         dotDNList
    ///         PalletList
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
    ///              IPizzaRepository
    ///              IDeliveryRepository
    /// </para> 
    /// </remarks>
    public partial class UploadPoDataEDI : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public UploadPoDataEDI()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            IDeliveryRepository deliveryRepository = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
            IPizzaRepository pizzaRepository = RepositoryFactory.GetInstance().GetRepository<IPizzaRepository, Pizza>();
            IList<PakDotPakComnInfo> origDotDNList = CurrentSession.GetValue("dotDNList") as IList<PakDotPakComnInfo>;
            IList<PakDotPakPaltnoInfo> palletList = CurrentSession.GetValue("PalletList") as IList<PakDotPakPaltnoInfo>;

            IList<PakDotPakComnInfo> dotDNList = new List<PakDotPakComnInfo>();
            foreach (PakDotPakComnInfo ele in origDotDNList)
            {
                if ((ele.hp_pn.StartsWith("PC") || ele.hp_pn.StartsWith("PO"))
                   && !ele.hp_pn.StartsWith(ele.model + "/"))
                {
                    continue;
                }
                if (ele.region.StartsWith("SCN"))
                {
                    continue;
                }
                dotDNList.Add(ele);
            }

            Hashtable hPalletQty = new Hashtable();
            foreach (PakDotPakPaltnoInfo ele in palletList)
            {
                bool bFound = false;
                foreach (PakDotPakComnInfo tmp in dotDNList)
                {
                    if (tmp.internalID == ele.internalID)
                    {
                        bFound = true;
                    }
                }
                if (!bFound)
                {
                    continue;
                }

                if (deliveryRepository.CheckExistPoDataEdi(ele.internalID))
                {
                    continue;
                }

                if (hPalletQty.Contains(ele.internalID))
                {
                    hPalletQty[ele.internalID] = int.Parse(hPalletQty[ele.internalID].ToString()) + ele.pallet_box_qty;
                }
                else
                {
                    hPalletQty.Add(ele.internalID, ele.pallet_box_qty);
                }

                PoPltEdiInfo toSave = new PoPltEdiInfo();
                toSave.deliveryNo = ele.internalID;
                toSave.plt = ele.pallet_id;
                toSave.ucc = ele.p5;
                toSave.qty = Convert.ToInt32(ele.pallet_box_qty);
                toSave.combineQty = 0;
                toSave.consolidate = "";
                toSave.conQTY = 0;
                /*
                 * Answer to: ITC-1360-1225
                 * Description: Lack of editor to save.
                 */
                toSave.editor = Editor;

                foreach (PakDotPakComnInfo tmp in dotDNList)
                {
                    if (tmp.internalID == ele.internalID)
                    {
                        if (tmp.consol_invoice.Contains("/"))
                        {
                            string str = tmp.consol_invoice;
                            str += "   ";   //To ensure there are at least 3 characters after '/'
                            toSave.consolidate = str.Substring(0, str.IndexOf('/'));
                            toSave.conQTY = int.Parse(str.Substring(str.IndexOf('/') + 1, 3));
                        }

                        if (ele.p4 != "ZP")
                        {
                            if ((tmp.intl_carrier == "BNAF" || tmp.intl_carrier == "DZNA" || tmp.intl_carrier == "NPN9")
                                && (tmp.region == "SCA" || tmp.region == "SNA" || tmp.region == "SAF"))
                            {
                                toSave.plt = "BA" + toSave.plt;
                            }
                            else
                            {
                                toSave.plt = "NA" + toSave.plt;
                            }
                        }
                        break;
                    }
                }
                deliveryRepository.AddPoPltEdiInfoDefered(CurrentSession.UnitOfWork, toSave);
            }

            foreach (PakDotPakComnInfo ele in dotDNList)
            {
                if (deliveryRepository.CheckExistPoDataEdi(ele.internalID))//
                {
                    deliveryRepository.UpdateUdtForPoDataEdiDefered(CurrentSession.UnitOfWork, ele.internalID);
                }
                else
                {
                    string descr = "PartNo=" + ele.hp_pn + "~"
                                + "BOL=" + ele.waybill_number + "~"
                                + "Customer=" + ele.ship_to_name + "~"
                                + "Name1=" + "" + "~"
                                + "Name2=" + ele.c6 + "~"
                                + "ShiptoId=" + ele.ship_to_id + "~"
                                + "Adr1=" + ele.ship_to_name_2 + "~"
                                + "Adr2=" + ele.ship_to_street + "~"
                                + "Adr3=" + ele.ship_to_city + "~"
                                + "Adr4=" + ele.ship_to_name_3 + "~"
                                + "City=" + ele.return_to_city + "~"
                                + "State=" + ele.ship_to_state + "~"
                                + "Postal=" + ele.ship_to_zip + "~"
                                + "Country=" + ele.c12 + "~"
                                + "Carrier=" + ele.intl_carrier + "~"
                                + "SO=" + ele.hp_so + "~"
                                + "CustPo=" + ele.cust_po + "~"
                                + "Invoice=" + ele.pack_id + "~"
                                + "RetCode=" + "" + "~"
                                + "ProdNo=" + "" + "~"
                                + "IECSo=" + ele.c24 + "~"
                                + "IECSoItem=" + ele.c25 + "~"
                                + "PoItem=" + ele.c26 + "~"
                                + "CustSo=" + ele.cust_so_num + "~"
                                + "ShipFrom=" + ele.c28 + "~"
                                + "ShipingMark=" + "" + "~"
                                + "Flag=" + ele.c30 + "~"
                                + "RegId=" + ele.region + "~"
                                + "BoxSort=" + ele.sub_region + "~"
                                + "Duty=" + ele.duty_code + "~"
                                + "RegCarrier=" + ele.reg_carrier + "~"
                                + "Destination=" + ele.dest_code + "~"
                                + "Department=" + "" + "~"
                                + "OrdRefrence=" + ele.cust_ord_ref + "~"
                                + "DeliverTo=" + ele.ship_to_contact + "~"
                                + "Tel=" + ele.ship_to_telephone + "~"
                                + "WH=" + "" + "~"
                                + "Consolidated=" + ele.consol_invoice + "~"
                                + "SKU=" + ele.hp_cust_pn + "~"
                                + "Deport=" + ele.c43 + "~"
                                + "POqty=" + ele.pack_id_unit_qty + "~"
                                + "CQty=" + ele.pack_id_line_item_box_max_unit_qty + "~"
                                + "EmeaCarrier=" + ele.c46 + "~"
                                + "Plant=" + ele.c47 + "~"
                                + "ShipTp=" + ele.order_type + "~"
                                + "ConfigID=" + ele.c48 + "~"
                                + "HYML=" + ele.c49 + "~"
                                + "CustName=" + "" + "~"
                                + "ShipHold=" + "" + "~"
                                + "CTO-DS=" + "" + "~"
                                + "PackType=" + "" + "~"
                                + "PltType=" + "" + "~"
                                + "ShipWay=" + ele.pack_id_unit_uom + "~"
                                + "Dept=" + "" + "~"
                                + "MISC=" + "" + "~"
                                + "ShipFromNme=" + ele.return_to_name + "~"
                                + "ShipFromAr1=" + ele.return_to_street + "~"
                                + "ShipFromCty=" + ele.return_to_city + "," + ele.return_to_state + "," + ele.return_to_zip + "~"
                                + "ShipFromTl=" + ele.return_to_telephone + "~"
                                + "DnItem=" + ele.c62 + "~"
                                + "Price=" + ele.c63 + "~"
                                + "BoxReg=" + ele.c64 + "~";
                    /*
                    if ((descr.Contains("PartNo=PC") || descr.Contains("PartNo=PO"))
                        && !descr.Contains("PartNo=" + ele.model + "/"))
                    {
                        continue;
                    }
                    */
                    /*
                    if (descr.Contains("RegId=SCN"))
                    {
                        continue;
                    }
                    */
                    /*
                    if ((ele.hp_pn.StartsWith("PC") || ele.hp_pn.StartsWith("PO"))
                        && !ele.hp_pn.StartsWith(ele.model + "/"))
                    {
                        continue;
                    }
                    if (ele.region.StartsWith("SCN"))
                    {
                        continue;
                    }
                    */

                    PoDataEdiInfo toSave = new PoDataEdiInfo();
                    toSave.deliveryNo = ele.internalID;
                    toSave.poNo = ele.po_num;
                    toSave.model = ele.model;
                    toSave.shipDate = DateTime.ParseExact(ele.actual_shipdate, "yyyyMMdd", DateTimeFormatInfo.InvariantInfo).ToString("yyyy/MM/dd");
                    if (hPalletQty.Contains(ele.internalID))
                    {
                        toSave.qty = int.Parse(hPalletQty[ele.internalID].ToString());
                    }
                    else
                    {
                        toSave.qty = 0;
                    }
                    toSave.editor = Editor;
                    toSave.status = "00";  
                    toSave.descr = descr.Replace("&", "^&");
                    deliveryRepository.AddPoDataEdiInfoDefered(CurrentSession.UnitOfWork, toSave);
                }
            }

            return base.DoExecute(executionContext);
        }
    }
}
