/*
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:IMES service implement for PoData Page
 *             
 * UI:CI-MES12-SPEC-PAK-UI PO Data.docx –2011/11/08 
 * UC:CI-MES12-SPEC-PAK-UC PO Data.docx –2011/11/10            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2011-12-09  itc202017             (Reference Ebook SourceCode) Create
 * Known issues:
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Workflow.Runtime;
using System.Globalization; 
using IMES.DataModel;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PAK.DN;
using IMES.FisObject.PAK.Pallet;
using IMES.FisObject.PAK.Pizza;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.Station.Interface.StationIntf;
using log4net;
namespace IMES.Station.Implementation
{
    /// <summary>
    /// IMES service for PoData.
    /// </summary>
    public class _PoData : MarshalByRefObject, IPoData
    {
        private struct S_PLDNLine
        {
            public string DeliveryNo;
            public DateTime ShipDate;
            public string PoNo;
            public string Model;
            public string ShipmentNo;
            public string strQty;
            public int Qty;
            public Hashtable HDeliveryInfo;
        };
        
        private struct S_PLPNLine
        {
            public string DeliveryNo;
            public string PalletNo;
            public short DeliveryQty;
            public string PalletType;
            public string UCC;
            public int DeviceQty;
        };

        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
        private IDeliveryRepository deliveryRepository = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
        private IPalletRepository palletRepository = RepositoryFactory.GetInstance().GetRepository<IPalletRepository, Pallet>();
        private IPizzaRepository pizzaRepository = RepositoryFactory.GetInstance().GetRepository<IPizzaRepository, Pizza>();

        #region IPoData Members
        private int fixQty(string str)
        {
            if (str == null)
            {
                return 0;
            }

            string input = str.Trim();
            if (input.Length <= 0)
            {
                return 0;
            }

            bool bHasDot = false;
            for (int j = 0; j < input.Length; j++)
            {
                if ((input[j] < '0' || input[j] > '9') && input[j] != '.')
                {
                    return 0;
                }

                if (input[j] == '.')
                {
                    if (bHasDot) return 0;
                    bHasDot = true;
                }
            }

            if (bHasDot)
            {
                return int.Parse(input.Substring(0, input.IndexOf('.')));
            }
            else
            {
                return int.Parse(input);
            }
        }
        //173船务、普通船务、内销船务三种DN文件格式区别仅在于：
        //第41字段（即fields[40]）：173船务为ShipmentNo，普通船务和内销船务该字段为Consolidated
        //普通船务和内销船务最后多一个字段，普通船务中叫InvoiceNum，内销船务中叫Invoice_NO
        //为避免后续文件格式变化引起维护不便，三种格式分别写解析函数。
        private S_PLDNLine ParseDNInfo173(string line)
        {
            S_PLDNLine ret = new S_PLDNLine();
            ret.HDeliveryInfo = new Hashtable();
            //<UC:k>将DN属性值”&”替换成”^&”
            string[] fields = line.Replace("&", "^&").Split('~');
            if (fields.Length < 74)
            {
                throw new FisException("CHK502", new string[] { line.Substring(0, 30) + "..." });
            }
            ret.DeliveryNo = fields[0].Trim();

            //<UC:g>Delivery.ShipDate: 原文件格式是YYYYMMDD，需要转为YYYY/MM/DD
            string shipdate = fields[1].Trim();
            ret.ShipDate = DateTime.ParseExact(shipdate, "yyyyMMdd", DateTimeFormatInfo.InvariantInfo); 
            ret.PoNo = fields[2].Trim();
            ret.HDeliveryInfo.Add("Customer", fields[3].Trim());
            ret.HDeliveryInfo.Add("Name1", fields[4].Trim());
            ret.HDeliveryInfo.Add("Name2", fields[5].Trim());
            ret.HDeliveryInfo.Add("ShiptoId", fields[6].Trim());
            ret.HDeliveryInfo.Add("Adr1", fields[7].Trim());
            ret.HDeliveryInfo.Add("Adr2", fields[8].Trim());
            ret.HDeliveryInfo.Add("Adr3", fields[9].Trim());
            ret.HDeliveryInfo.Add("Adr4", fields[10].Trim());
            ret.HDeliveryInfo.Add("City", fields[11].Trim());
            ret.HDeliveryInfo.Add("State", fields[12].Trim());
            ret.HDeliveryInfo.Add("Postal", fields[13].Trim());
            ret.HDeliveryInfo.Add("Country", fields[14].Trim());
            ret.HDeliveryInfo.Add("Carrier", fields[15].Trim());
            ret.HDeliveryInfo.Add("SO", fields[16].Trim());
            ret.HDeliveryInfo.Add("CustPo", fields[17].Trim());
            ret.Model = fields[18].Trim();
            ret.HDeliveryInfo.Add("BOL", fields[19].Trim());
            ret.HDeliveryInfo.Add("Invoice", fields[20].Trim());
            ret.HDeliveryInfo.Add("RetCode", fields[21].Trim());
            ret.HDeliveryInfo.Add("ProdNo", fields[22].Trim());
            ret.HDeliveryInfo.Add("IECSo", fields[23].Trim());
            ret.HDeliveryInfo.Add("IECSoItem", fields[24].Trim());
            ret.HDeliveryInfo.Add("PoItem", fields[25].Trim());
            ret.HDeliveryInfo.Add("CustSo", fields[26].Trim());
            ret.HDeliveryInfo.Add("ShipFrom", fields[27].Trim());
            ret.HDeliveryInfo.Add("ShipingMark", fields[28].Trim());
            ret.HDeliveryInfo.Add("Flag", fields[29].Trim());
            ret.HDeliveryInfo.Add("RegId", fields[30].Trim());
            ret.HDeliveryInfo.Add("BoxSort", fields[31].Trim());
            ret.HDeliveryInfo.Add("Duty", fields[32].Trim());
            ret.HDeliveryInfo.Add("RegCarrier", fields[33].Trim());
            ret.HDeliveryInfo.Add("Destination", fields[34].Trim());
            ret.HDeliveryInfo.Add("Department", fields[35].Trim());
            ret.HDeliveryInfo.Add("OrdRefrence", fields[36].Trim());
            ret.HDeliveryInfo.Add("DeliverTo", fields[37].Trim());
            ret.HDeliveryInfo.Add("Tel", fields[38].Trim());
            ret.HDeliveryInfo.Add("WH", fields[39].Trim());
            ret.ShipmentNo = fields[40].Trim();
            ret.HDeliveryInfo.Add("Consolidated", ret.ShipmentNo);
            if (ret.ShipmentNo == "")
            {
                //throw new FisException("CHK524", new string[] { ret.DeliveryNo });
                ret.ShipmentNo = ret.DeliveryNo;
            }
            else if (ret.ShipmentNo.Length >= 10)
            {
                ret.ShipmentNo = ret.ShipmentNo.Substring(0, 10);
            }
            ret.HDeliveryInfo.Add("SKU", fields[41].Trim());
            ret.HDeliveryInfo.Add("Deport", fields[42].Trim());
            ret.strQty = fields[43].Trim();
            ret.HDeliveryInfo.Add("CQty", fields[44].Trim());
            ret.HDeliveryInfo.Add("EmeaCarrier", fields[45].Trim());
            ret.HDeliveryInfo.Add("Plant", fields[46].Trim());
            ret.HDeliveryInfo.Add("ShipTp", fields[47].Trim());
            ret.HDeliveryInfo.Add("ConfigID", fields[48].Trim());
            ret.HDeliveryInfo.Add("HYML", fields[49].Trim());
            ret.HDeliveryInfo.Add("CustName", fields[50].Trim());
            ret.HDeliveryInfo.Add("ShipHold", fields[51].Trim());
            ret.HDeliveryInfo.Add("CTO-DS", fields[52].Trim());
            ret.HDeliveryInfo.Add("PackType", fields[53].Trim());
            ret.HDeliveryInfo.Add("PltType", fields[54].Trim());
            ret.HDeliveryInfo.Add("ShipWay", fields[55].Trim());
            ret.HDeliveryInfo.Add("Dept", fields[56].Trim());
            ret.HDeliveryInfo.Add("MISC", fields[57].Trim());
            ret.HDeliveryInfo.Add("ShipFromNme", fields[58].Trim());
            ret.HDeliveryInfo.Add("ShipFromAr1", fields[59].Trim());
            ret.HDeliveryInfo.Add("ShipFromCty", fields[60].Trim());
            ret.HDeliveryInfo.Add("ShipFromTl", fields[61].Trim());
            ret.HDeliveryInfo.Add("DnItem", fields[62].Trim());
            ret.HDeliveryInfo.Add("Price", fields[63].Trim());
            ret.HDeliveryInfo.Add("BoxReg", fields[64].Trim());
            ret.HDeliveryInfo.Add("850DAY", fields[65].Trim());
            ret.HDeliveryInfo.Add("PackingType", fields[66].Trim());
            ret.HDeliveryInfo.Add("PackingLV", fields[67].Trim());
            ret.HDeliveryInfo.Add("CnQty", fields[68].Trim());
            ret.HDeliveryInfo.Add("PalletQty", fields[69].Trim());
            ret.HDeliveryInfo.Add("UCC", fields[70].Trim());
            ret.HDeliveryInfo.Add("LabelFlg", fields[71].Trim());
            ret.HDeliveryInfo.Add("PAKType", fields[72].Trim());
            return ret;
        }

        private S_PLDNLine ParsePLDNInfoNormal(string line)
        {
            S_PLDNLine ret = new S_PLDNLine();
            ret.HDeliveryInfo = new Hashtable();
            //<UC:h>将DN属性值”&”替换成”^&”
            string[] fields = line.Replace("&", "^&").Split('~');
            if (fields.Length < 75)
            {
                throw new FisException("CHK502", new string[] { line.Substring(0, 30) + "..." });
            }
            ret.DeliveryNo = fields[0].Trim();

            //<UC:l>Delivery.ShipDate: 原文件格式是YYYYMMDD，需要转为YYYY/MM/DD
            string shipdate = fields[1].Trim();
            ret.ShipDate = DateTime.ParseExact(shipdate, "yyyyMMdd", DateTimeFormatInfo.InvariantInfo);
            ret.PoNo = fields[2].Trim();
            ret.HDeliveryInfo.Add("Customer", fields[3].Trim());
            ret.HDeliveryInfo.Add("Name1", fields[4].Trim());
            ret.HDeliveryInfo.Add("Name2", fields[5].Trim());
            ret.HDeliveryInfo.Add("ShiptoId", fields[6].Trim());
            ret.HDeliveryInfo.Add("Adr1", fields[7].Trim());
            ret.HDeliveryInfo.Add("Adr2", fields[8].Trim());
            ret.HDeliveryInfo.Add("Adr3", fields[9].Trim());
            ret.HDeliveryInfo.Add("Adr4", fields[10].Trim());
            ret.HDeliveryInfo.Add("City", fields[11].Trim());
            ret.HDeliveryInfo.Add("State", fields[12].Trim());
            ret.HDeliveryInfo.Add("Postal", fields[13].Trim());
            ret.HDeliveryInfo.Add("Country", fields[14].Trim());
            ret.HDeliveryInfo.Add("Carrier", fields[15].Trim());
            ret.HDeliveryInfo.Add("SO", fields[16].Trim());
            ret.HDeliveryInfo.Add("CustPo", fields[17].Trim());
            ret.Model = fields[18].Trim();
            ret.HDeliveryInfo.Add("BOL", fields[19].Trim());
            ret.HDeliveryInfo.Add("Invoice", fields[20].Trim());
            ret.HDeliveryInfo.Add("RetCode", fields[21].Trim());
            ret.HDeliveryInfo.Add("ProdNo", fields[22].Trim());
            ret.HDeliveryInfo.Add("IECSo", fields[23].Trim());
            ret.HDeliveryInfo.Add("IECSoItem", fields[24].Trim());
            ret.HDeliveryInfo.Add("PoItem", fields[25].Trim());
            ret.HDeliveryInfo.Add("CustSo", fields[26].Trim());
            ret.HDeliveryInfo.Add("ShipFrom", fields[27].Trim());
            ret.HDeliveryInfo.Add("ShipingMark", fields[28].Trim());
            ret.HDeliveryInfo.Add("Flag", fields[29].Trim());
            ret.HDeliveryInfo.Add("RegId", fields[30].Trim());
            ret.HDeliveryInfo.Add("BoxSort", fields[31].Trim());
            ret.HDeliveryInfo.Add("Duty", fields[32].Trim());
            ret.HDeliveryInfo.Add("RegCarrier", fields[33].Trim());
            ret.HDeliveryInfo.Add("Destination", fields[34].Trim());
            ret.HDeliveryInfo.Add("Department", fields[35].Trim());
            ret.HDeliveryInfo.Add("OrdRefrence", fields[36].Trim());
            ret.HDeliveryInfo.Add("DeliverTo", fields[37].Trim());
            ret.HDeliveryInfo.Add("Tel", fields[38].Trim());
            ret.HDeliveryInfo.Add("WH", fields[39].Trim());
            ret.HDeliveryInfo.Add("Consolidated", fields[40].Trim());
            ret.HDeliveryInfo.Add("SKU", fields[41].Trim());
            ret.HDeliveryInfo.Add("Deport", fields[42].Trim());
            ret.strQty = fields[43].Trim();
            /*
             * Answer to: ITC-1360-0867
             * Description: Set Qty.
             */
            ret.Qty = fixQty(ret.strQty);
            ret.HDeliveryInfo.Add("CQty", fields[44].Trim());
            ret.HDeliveryInfo.Add("EmeaCarrier", fields[45].Trim());
            ret.HDeliveryInfo.Add("Plant", fields[46].Trim());
            ret.HDeliveryInfo.Add("ShipTp", fields[47].Trim());
            ret.HDeliveryInfo.Add("ConfigID", fields[48].Trim());
            ret.HDeliveryInfo.Add("HYML", fields[49].Trim());
            ret.HDeliveryInfo.Add("CustName", fields[50].Trim());
            ret.HDeliveryInfo.Add("ShipHold", fields[51].Trim());
            ret.HDeliveryInfo.Add("CTO-DS", fields[52].Trim());
            ret.HDeliveryInfo.Add("PackType", fields[53].Trim());
            ret.HDeliveryInfo.Add("PltType", fields[54].Trim());
            ret.HDeliveryInfo.Add("ShipWay", fields[55].Trim());
            ret.HDeliveryInfo.Add("Dept", fields[56].Trim());
            ret.HDeliveryInfo.Add("MISC", fields[57].Trim());
            ret.HDeliveryInfo.Add("ShipFromNme", fields[58].Trim());
            ret.HDeliveryInfo.Add("ShipFromAr1", fields[59].Trim());
            ret.HDeliveryInfo.Add("ShipFromCty", fields[60].Trim());
            ret.HDeliveryInfo.Add("ShipFromTl", fields[61].Trim());
            ret.HDeliveryInfo.Add("DnItem", fields[62].Trim());
            ret.HDeliveryInfo.Add("Price", fields[63].Trim());
            ret.HDeliveryInfo.Add("BoxReg", fields[64].Trim());
            ret.HDeliveryInfo.Add("850DAY", fields[65].Trim());
            ret.HDeliveryInfo.Add("PackingType", fields[66].Trim());
            ret.HDeliveryInfo.Add("PackingLV", fields[67].Trim());
            ret.HDeliveryInfo.Add("CnQty", fields[68].Trim());
            ret.HDeliveryInfo.Add("PalletQty", fields[69].Trim());
            ret.HDeliveryInfo.Add("UCC", fields[70].Trim());
            ret.HDeliveryInfo.Add("LabelFlg", fields[71].Trim());
            ret.HDeliveryInfo.Add("PAKType", fields[72].Trim());
            ret.HDeliveryInfo.Add("InvoiceNum", fields[73].Trim());
            return ret;
        }

        private S_PLDNLine ParsePLDNInfoDomestic(string line)
        {
            S_PLDNLine ret = new S_PLDNLine();
            ret.HDeliveryInfo = new Hashtable();
            //<UC:k>将DN属性值”&”替换成”^&”
            string[] fields = line.Replace("&", "^&").Split('~');
            if (fields.Length < 75)
            {
                throw new FisException("CHK502", new string[] { line.Substring(0, 30) + "..." });
            }
            ret.DeliveryNo = fields[0].Trim();

            //<UC:b>Delivery.ShipDate: 原文件格式是YYYYMMDD，需要转为YYYY/MM/DD
            string shipdate = fields[1].Trim();
            ret.ShipDate = DateTime.ParseExact(shipdate, "yyyyMMdd", DateTimeFormatInfo.InvariantInfo);
            ret.PoNo = fields[2].Trim();
            ret.HDeliveryInfo.Add("Customer", fields[3].Trim());
            ret.HDeliveryInfo.Add("Name1", fields[4].Trim());
            ret.HDeliveryInfo.Add("Name2", fields[5].Trim());
            ret.HDeliveryInfo.Add("ShiptoId", fields[6].Trim());
            ret.HDeliveryInfo.Add("Adr1", fields[7].Trim());
            ret.HDeliveryInfo.Add("Adr2", fields[8].Trim());
            ret.HDeliveryInfo.Add("Adr3", fields[9].Trim());
            ret.HDeliveryInfo.Add("Adr4", fields[10].Trim());
            ret.HDeliveryInfo.Add("City", fields[11].Trim());
            ret.HDeliveryInfo.Add("State", fields[12].Trim());
            ret.HDeliveryInfo.Add("Postal", fields[13].Trim());
            ret.HDeliveryInfo.Add("Country", fields[14].Trim());
            ret.HDeliveryInfo.Add("Carrier", fields[15].Trim());
            ret.HDeliveryInfo.Add("SO", fields[16].Trim());
            ret.HDeliveryInfo.Add("CustPo", fields[17].Trim());
            ret.Model = fields[18].Trim();
            ret.HDeliveryInfo.Add("BOL", fields[19].Trim());
            ret.HDeliveryInfo.Add("Invoice", fields[20].Trim());
            ret.HDeliveryInfo.Add("RetCode", fields[21].Trim());
            ret.HDeliveryInfo.Add("ProdNo", fields[22].Trim());
            ret.HDeliveryInfo.Add("IECSo", fields[23].Trim());
            ret.HDeliveryInfo.Add("IECSoItem", fields[24].Trim());
            ret.HDeliveryInfo.Add("PoItem", fields[25].Trim());
            ret.HDeliveryInfo.Add("CustSo", fields[26].Trim());
            ret.HDeliveryInfo.Add("ShipFrom", fields[27].Trim());
            ret.HDeliveryInfo.Add("ShipingMark", fields[28].Trim());
            ret.HDeliveryInfo.Add("Flag", fields[29].Trim());
            ret.HDeliveryInfo.Add("RegId", fields[30].Trim());
            ret.HDeliveryInfo.Add("BoxSort", fields[31].Trim());
            ret.HDeliveryInfo.Add("Duty", fields[32].Trim());
            ret.HDeliveryInfo.Add("RegCarrier", fields[33].Trim());
            ret.HDeliveryInfo.Add("Destination", fields[34].Trim());
            ret.HDeliveryInfo.Add("Department", fields[35].Trim());
            ret.HDeliveryInfo.Add("OrdRefrence", fields[36].Trim());
            ret.HDeliveryInfo.Add("DeliverTo", fields[37].Trim());
            ret.HDeliveryInfo.Add("Tel", fields[38].Trim());
            ret.HDeliveryInfo.Add("WH", fields[39].Trim());
            ret.HDeliveryInfo.Add("Consolidated", fields[40].Trim());
            ret.HDeliveryInfo.Add("SKU", fields[41].Trim());
            ret.HDeliveryInfo.Add("Deport", fields[42].Trim());
            ret.strQty = fields[43].Trim();
            ret.Qty = fixQty(ret.strQty);
            ret.HDeliveryInfo.Add("CQty", fields[44].Trim());
            ret.HDeliveryInfo.Add("EmeaCarrier", fields[45].Trim());
            ret.HDeliveryInfo.Add("Plant", fields[46].Trim());
            ret.HDeliveryInfo.Add("ShipTp", fields[47].Trim());
            ret.HDeliveryInfo.Add("ConfigID", fields[48].Trim());
            ret.HDeliveryInfo.Add("HYML", fields[49].Trim());
            ret.HDeliveryInfo.Add("CustName", fields[50].Trim());
            ret.HDeliveryInfo.Add("ShipHold", fields[51].Trim());
            ret.HDeliveryInfo.Add("CTO-DS", fields[52].Trim());
            ret.HDeliveryInfo.Add("PackType", fields[53].Trim());
            ret.HDeliveryInfo.Add("PltType", fields[54].Trim());
            ret.HDeliveryInfo.Add("ShipWay", fields[55].Trim());
            ret.HDeliveryInfo.Add("Dept", fields[56].Trim());
            ret.HDeliveryInfo.Add("MISC", fields[57].Trim());
            ret.HDeliveryInfo.Add("ShipFromNme", fields[58].Trim());
            ret.HDeliveryInfo.Add("ShipFromAr1", fields[59].Trim());
            ret.HDeliveryInfo.Add("ShipFromCty", fields[60].Trim());
            ret.HDeliveryInfo.Add("ShipFromTl", fields[61].Trim());
            ret.HDeliveryInfo.Add("DnItem", fields[62].Trim());
            ret.HDeliveryInfo.Add("Price", fields[63].Trim());
            ret.HDeliveryInfo.Add("BoxReg", fields[64].Trim());
            ret.HDeliveryInfo.Add("850DAY", fields[65].Trim());
            ret.HDeliveryInfo.Add("PackingType", fields[66].Trim());
            ret.HDeliveryInfo.Add("PackingLV", fields[67].Trim());
            ret.HDeliveryInfo.Add("CnQty", fields[68].Trim());
            ret.HDeliveryInfo.Add("PalletQty", fields[69].Trim());
            ret.HDeliveryInfo.Add("UCC", fields[70].Trim());
            ret.HDeliveryInfo.Add("LabelFlg", fields[71].Trim());
            ret.HDeliveryInfo.Add("PAKType", fields[72].Trim());

            /*
             * Answer to: ITC-1360-1240
             * Description: Set Invoice_NO to 0 if the field is "".
             */
            if (fields[73].Trim() == "")
            {
                ret.HDeliveryInfo.Add("Invoice_NO", "0");
            }
            else
            {
                ret.HDeliveryInfo.Add("Invoice_NO", fields[73].Trim());
            }
            return ret;
        }

        private S_PLPNLine ParsePLPNInfo(string line)
        {
            S_PLPNLine ret = new S_PLPNLine();
            string[] fields = line.Split('~');
            if (fields.Length < 6)
            {
                throw new FisException("CHK503", new string[] { line.Substring(0, 30) + "..." });
            }
            ret.DeliveryNo = fields[0].Trim();
            ret.PalletNo = fields[1].Trim();
            ret.DeliveryQty = (short)fixQty(fields[2].Trim());
            ret.PalletType = fields[3].Trim();
            ret.UCC = fields[4].Trim();
            ret.DeviceQty = 0;
            if (fields.Length > 6)
            {
                ret.DeviceQty = fixQty(fields[6].Trim());
            }
            return ret;
        }

        /*
         * Answer to: ITC-1360-1221
         * Description: Trim spaces in each field.
         */
        private PakDotPakComnInfo ParseOBDNInfo(string line)
        {
            PakDotPakComnInfo ret = new PakDotPakComnInfo();
            string[] fields = line.Split('~');
            if (fields.Length < 154)
            {
                throw new FisException("CHK502", new string[] { line.Substring(0, 30) + "..." });
            }

            ret.internalID = fields[0].Trim();
            ret.actual_shipdate = fields[1].Trim();
            ret.po_num = fields[2].Trim();
            ret.ship_to_name = fields[3].Trim();
            ret.ship_to_name_2 = fields[4].Trim();
            ret.c6 = fields[5].Trim();
            ret.ship_to_id = fields[6].Trim();
            ret.ship_to_street = fields[7].Trim();
            ret.ship_to_city = fields[8].Trim();
            ret.ship_to_name_3 = fields[9].Trim();
            ret.ship_to_street_2 = fields[10].Trim();
            ret.c12 = fields[11].Trim();
            ret.ship_to_state = fields[12].Trim();
            ret.ship_to_zip = fields[13].Trim();
            ret.ship_to_country_name = fields[14].Trim();
            ret.intl_carrier = fields[15].Trim();
            ret.hp_so = fields[16].Trim();
            ret.cust_po = fields[17].Trim();
            ret.hp_pn = fields[18].Trim();
            ret.waybill_number = fields[19].Trim();
            ret.pack_id = fields[20].Trim();
            ret.c22 = fields[21].Trim();
            ret.c23 = fields[22].Trim();
            ret.c24 = fields[23].Trim();
            ret.c25 = fields[24].Trim();
            ret.c26 = fields[25].Trim();
            ret.cust_so_num = fields[26].Trim();
            ret.c28 = fields[27].Trim();
            ret.c29 = fields[28].Trim();
            ret.c30 = fields[29].Trim();
            ret.region = fields[30].Trim();
            ret.sub_region = fields[31].Trim();
            ret.duty_code = fields[32].Trim();
            ret.reg_carrier = fields[33].Trim();
            ret.dest_code = fields[34].Trim();
            ret.c36 = fields[35].Trim();
            ret.cust_ord_ref = fields[36].Trim();
            ret.ship_to_contact = fields[37].Trim();
            ret.ship_to_telephone = fields[38].Trim();
            ret.c40 = fields[39].Trim();
            ret.pack_id_cons = fields[40].Trim();
            ret.hp_cust_pn = fields[41].Trim();
            ret.c43 = fields[42].Trim();
            ret.pack_id_line_item_unit_qty = (decimal)fixQty(fields[43].Trim());
            ret.pack_id_line_item_box_max_unit_qty = fields[44].Trim();
            ret.c46 = fields[45].Trim();
            ret.c47 = fields[46].Trim();
            ret.c48 = fields[47].Trim();
            ret.c49 = fields[48].Trim();
            ret.c50 = fields[49].Trim();
            ret.c51 = fields[50].Trim();
            ret.c52 = fields[51].Trim();
            ret.c53 = fields[52].Trim();
            ret.c54 = fields[53].Trim();
            ret.pack_id_unit_uom = fields[54].Trim();
            ret.c56 = fields[55].Trim();
            ret.c57 = fields[56].Trim();
            ret.return_to_name = fields[57].Trim();
            ret.return_to_street = fields[58].Trim();
            ret.c60 = fields[59].Trim();
            ret.c61 = fields[60].Trim();
            ret.c62 = fields[61].Trim();
            ret.c63 = fields[62].Trim();
            ret.c64 = fields[63].Trim();
            ret.ship_ref = fields[64].Trim();
            ret.ship_via_id = fields[65].Trim();
            ret.ship_via_name = fields[66].Trim();
            ret.ship_via_name_2 = fields[67].Trim();
            ret.c69 = fields[68].Trim();
            ret.ship_via_street = fields[69].Trim();
            ret.ship_via_city = fields[70].Trim();
            ret.ship_via_name_3 = fields[71].Trim();
            ret.ship_via_street_2 = fields[72].Trim();
            ret.c74 = fields[73].Trim();
            ret.ship_via_state = fields[74].Trim();
            ret.ship_via_zip = fields[75].Trim();
            ret.ship_via_country_name = fields[76].Trim();
            ret.ship_via_contact = fields[77].Trim();
            ret.ship_via_telephone = fields[78].Trim();
            ret.return_to_id = fields[79].Trim();
            ret.return_to_name_2 = fields[80].Trim();
            ret.return_to_name_3 = fields[81].Trim();
            ret.return_to_city = fields[82].Trim();
            ret.return_to_state = fields[83].Trim();
            ret.return_to_zip = fields[84].Trim();
            ret.return_to_country_code = fields[85].Trim();
            ret.return_to_country_name = fields[86].Trim();
            ret.return_to_contact = fields[87].Trim();
            ret.return_to_telephone = fields[88].Trim();
            ret.ship_via_country_code = fields[89].Trim();
            ret.ship_from_name = fields[90].Trim();
            ret.ship_from_name_2 = fields[91].Trim();
            ret.c93 = fields[92].Trim();
            ret.ship_from_id = fields[93].Trim();
            ret.ship_from_street = fields[94].Trim();
            ret.ship_from_city = fields[95].Trim();
            ret.ship_from_name_3 = fields[96].Trim();
            ret.ship_from_street_2 = fields[97].Trim();
            ret.c99 = fields[98].Trim();
            ret.ship_from_state = fields[99].Trim();
            ret.ship_from_zip = fields[100].Trim();
            ret.ship_from_country_name = fields[101].Trim();
            ret.ship_from_country_code = fields[102].Trim();
            ret.ship_from_contact = fields[103].Trim();
            ret.ship_from_telephone = fields[104].Trim();
            ret.reseller_name = fields[105].Trim();
            ret.edi_phc = fields[106].Trim();
            ret.edi_mfg_code = fields[107].Trim();
            ret.edi_pl_code = fields[108].Trim();
            ret.edi_intl_carrier = fields[109].Trim();
            ret.ship_to_country_code = fields[110].Trim();
            ret.sold_to_contact = fields[111].Trim();
            ret.return_to_street_2 = fields[112].Trim();
            ret.edi_distrib_channel = fields[113].Trim();
            ret.order_date_hp_po = DateTime.ParseExact(fields[114].Trim(), "yyyyMMdd", DateTimeFormatInfo.InvariantInfo);
            ret.cond_priority = (fields[115].Trim() == "" ? 0 : decimal.Parse(fields[115].Trim()));
            ret.order_type = fields[116].Trim();
            ret.sales_chan = fields[117].Trim();
            ret.ship_mode = fields[118].Trim();
            ret.ship_cat_type = fields[119].Trim();
            ret.c121 = fields[120].Trim();
            ret.mand_carrier_id = fields[121].Trim();
            ret.trans_serv_level = fields[122].Trim();
            ret.c124 = fields[123].Trim();
            ret.pref_gateway = fields[124].Trim();
            ret.area_group_id = fields[125].Trim();
            ret.multi_line_id = fields[126].Trim();
            ret.hp_except = fields[127].Trim();
            ret.phys_consol = fields[128].Trim();
            ret.edi_trans_serv_level = fields[129].Trim();
            ret.shipment = fields[130].Trim();
            ret.doc_set_number = fields[131].Trim();
            ret.language = fields[132].Trim();
            ret.instr_flag = fields[133].Trim();
            ret.c135 = fields[134].Trim();
            ret.packing_type = fields[135].Trim();
            ret.packing_lv = fields[136].Trim();
            ret.carton_qty = (decimal)fixQty(fields[137].Trim());
            ret.pallet_qty = (decimal)fixQty(fields[138].Trim());
            ret.ucc_code = fields[139].Trim();
            ret.india_generic_desc = fields[140].Trim();
            ret.india_price = fields[141].Trim();
            ret.india_price_id = fields[142].Trim();
            ret.master_waybill_number = fields[143].Trim();
            ret.pak_type = fields[144].Trim();
            ret.sold_to_name2 = fields[145].Trim();
            ret.sold_to_name3 = fields[146].Trim();
            ret.sold_to_street = fields[147].Trim();
            ret.sold_to_street2 = fields[148].Trim();
            ret.sold_to_city = fields[149].Trim();
            ret.sold_to_state = fields[150].Trim();
            ret.sold_to_zip = fields[151].Trim();
            ret.sold_to_country_name = fields[152].Trim();
            if (fields[153].Trim().Length >= 20)
            {
                ret.c146 = fields[153].Trim().Substring(0, 20);  //(只获取字串的前20位)
            }
            else
            {
                ret.c146 = fields[153].Trim();
            }
            
            ret.customer_id = ret.hp_so;
            ret.consol_invoice = ret.pack_id_cons;
            ret.box_unit_qty = "1";

            /*
             * Answer to: ITC-1360-1217
             * Description: Delete from '.' if pack_id_line_item_unit_qty already contain '.' before set value to pack_id_unit_qty.
             */
            string tmp = ret.pack_id_line_item_unit_qty.ToString();
            if (tmp.Contains('.'))
            {
                ret.pack_id_unit_qty = tmp.Substring(0, tmp.IndexOf('.')) + ".000";
            }
            else
            {
                ret.pack_id_unit_qty = tmp + ".000";
            }

            if (ret.hp_pn.Length >= 12)
            {
                ret.model = ret.hp_pn.Substring(0, 12);
            }
            else
            {
                ret.model = ret.hp_pn;
            }

            return ret;
        }

        private PakDashPakComnInfo CopyPakComnInfo(PakDotPakComnInfo input)
        {
            PakDashPakComnInfo ret = new PakDashPakComnInfo();
            ret.internalID = input.internalID;
            ret.actual_shipdate = input.actual_shipdate;
            ret.po_num = input.po_num;
            ret.ship_to_name = input.ship_to_name;
            ret.ship_to_name_2 = input.ship_to_name_2;
            ret.c6 = input.c6;
            ret.ship_to_id = input.ship_to_id;
            ret.ship_to_street = input.ship_to_street;
            ret.ship_to_city = input.ship_to_city;
            ret.ship_to_name_3 = input.ship_to_name_3;
            ret.ship_to_street_2 = input.ship_to_street_2;
            ret.c12 = input.c12;
            ret.ship_to_state = input.ship_to_state;
            ret.ship_to_zip = input.ship_to_zip;
            ret.ship_to_country_name = input.ship_to_country_name;
            ret.intl_carrier = input.intl_carrier;
            ret.hp_so = input.hp_so;
            ret.cust_po = input.cust_po;
            ret.hp_pn = input.hp_pn;
            ret.waybill_number = input.waybill_number;
            ret.pack_id = input.pack_id;
            ret.c22 = input.c22;
            ret.c23 = input.c23;
            ret.c24 = input.c24;
            ret.c25 = input.c25;
            ret.c26 = input.c26;
            ret.cust_so_num = input.cust_so_num;
            ret.c28 = input.c28;
            ret.c29 = input.c29;
            ret.c30 = input.c30;
            ret.region = input.region;
            ret.sub_region = input.sub_region;
            ret.duty_code = input.duty_code;
            ret.reg_carrier = input.reg_carrier;
            ret.dest_code = input.dest_code;
            ret.c36 = input.c36;
            ret.cust_ord_ref = input.cust_ord_ref;
            ret.ship_to_contact = input.ship_to_contact;
            ret.ship_to_telephone = input.ship_to_telephone;
            ret.c40 = input.c40;
            ret.pack_id_cons = input.pack_id_cons;
            ret.hp_cust_pn = input.hp_cust_pn;
            ret.c43 = input.c43;
            ret.pack_id_line_item_unit_qty = input.pack_id_line_item_unit_qty;
            ret.pack_id_line_item_box_max_unit_qty = input.pack_id_line_item_box_max_unit_qty;
            ret.c46 = input.c46;
            ret.c47 = input.c47;
            ret.c48 = input.c48;
            ret.c49 = input.c49;
            ret.c50 = input.c50;
            ret.c51 = input.c51;
            ret.c52 = input.c52;
            ret.c53 = input.c53;
            ret.c54 = input.c54;
            ret.pack_id_unit_uom = input.pack_id_unit_uom;
            ret.c56 = input.c56;
            ret.c57 = input.c57;
            ret.return_to_name = input.return_to_name;
            ret.return_to_street = input.return_to_street;
            ret.c60 = input.c60;
            ret.c61 = input.c61;
            ret.c62 = input.c62;
            ret.c63 = input.c63;
            ret.c64 = input.c64;
            ret.ship_ref = input.ship_ref;
            ret.ship_via_id = input.ship_via_id;
            ret.ship_via_name = input.ship_via_name;
            ret.ship_via_name_2 = input.ship_via_name_2;
            ret.c69 = input.c69;
            ret.ship_via_street = input.ship_via_street;
            ret.ship_via_city = input.ship_via_city;
            ret.ship_via_name_3 = input.ship_via_name_3;
            ret.ship_via_street_2 = input.ship_via_street_2;
            ret.c74 = input.c74;
            ret.ship_via_state = input.ship_via_state;
            ret.ship_via_zip = input.ship_via_zip;
            ret.ship_via_country_name = input.ship_via_country_name;
            ret.ship_via_contact = input.ship_via_contact;
            ret.ship_via_telephone = input.ship_via_telephone;
            ret.return_to_id = input.return_to_id;
            ret.return_to_name_2 = input.return_to_name_2;
            ret.return_to_name_3 = input.return_to_name_3;
            ret.return_to_city = input.return_to_city;
            ret.return_to_state = input.return_to_state;
            ret.return_to_zip = input.return_to_zip;
            ret.return_to_country_code = input.return_to_country_code;
            ret.return_to_country_name = input.return_to_country_name;
            ret.return_to_contact = input.return_to_contact;
            ret.return_to_telephone = input.return_to_telephone;
            ret.ship_via_country_code = input.ship_via_country_code;
            ret.ship_from_name = input.ship_from_name;
            ret.ship_from_name_2 = input.ship_from_name_2;
            ret.c93 = input.c93;
            ret.ship_from_id = input.ship_from_id;
            ret.ship_from_street = input.ship_from_street;
            ret.ship_from_city = input.ship_from_city;
            ret.ship_from_name_3 = input.ship_from_name_3;
            ret.ship_from_street_2 = input.ship_from_street_2;
            ret.c99 = input.c99;
            ret.ship_from_state = input.ship_from_state;
            ret.ship_from_zip = input.ship_from_zip;
            ret.ship_from_country_name = input.ship_from_country_name;
            ret.ship_from_country_code = input.ship_from_country_code;
            ret.ship_from_contact = input.ship_from_contact;
            ret.ship_from_telephone = input.ship_from_telephone;
            ret.reseller_name = input.reseller_name;
            ret.edi_phc = input.edi_phc;
            ret.edi_mfg_code = input.edi_mfg_code;
            ret.edi_pl_code = input.edi_pl_code;
            ret.edi_intl_carrier = input.edi_intl_carrier;
            ret.ship_to_country_code = input.ship_to_country_code;
            ret.sold_to_contact = input.sold_to_contact;
            ret.return_to_street_2 = input.return_to_street_2;
            ret.edi_distrib_channel = input.edi_distrib_channel;
            ret.order_date_hp_po = input.order_date_hp_po;
            ret.cond_priority = input.cond_priority;
            ret.order_type = input.order_type;
            ret.sales_chan = input.sales_chan;
            ret.ship_mode = input.ship_mode;
            ret.ship_cat_type = input.ship_cat_type;
            ret.c121 = input.c121;
            ret.mand_carrier_id = input.mand_carrier_id;
            ret.trans_serv_level = input.trans_serv_level;
            ret.c124 = input.c124;
            ret.pref_gateway = input.pref_gateway;
            ret.area_group_id = input.area_group_id;
            ret.multi_line_id = input.multi_line_id;
            ret.hp_except = input.hp_except;
            ret.phys_consol = input.phys_consol;
            ret.edi_trans_serv_level = input.edi_trans_serv_level;
            ret.shipment = input.shipment;
            ret.doc_set_number = input.doc_set_number;
            ret.language = input.language;
            ret.instr_flag = input.instr_flag;
            ret.c135 = input.c135;
            ret.packing_type = input.packing_type;
            ret.packing_lv = input.packing_lv;
            ret.carton_qty = input.carton_qty;
            ret.pallet_qty = input.pallet_qty;
            ret.ucc_code = input.ucc_code;
            ret.india_generic_desc = input.india_generic_desc;
            ret.india_price = input.india_price;
            ret.india_price_id = input.india_price_id;
            ret.master_waybill_number = input.master_waybill_number;
            ret.pak_type = input.pak_type;
            ret.sold_to_name2 = input.sold_to_name2;
            ret.sold_to_name3 = input.sold_to_name3;
            ret.sold_to_street = input.sold_to_street;
            ret.sold_to_street2 = input.sold_to_street2;
            ret.sold_to_city = input.sold_to_city;
            ret.sold_to_state = input.sold_to_state;
            ret.sold_to_zip = input.sold_to_zip;
            ret.sold_to_country_name = input.sold_to_country_name;
            ret.c146 = input.c146;
            ret.customer_id = input.customer_id;
            ret.consol_invoice = input.consol_invoice;
            ret.box_unit_qty = input.box_unit_qty;
            ret.pack_id_unit_qty = input.pack_id_unit_qty;
            ret.model = input.model;
 
            return ret;
        }
        private PakDotPakPaltnoInfo ParseOBPNInfo(string line)
        {
            PakDotPakPaltnoInfo ret = new PakDotPakPaltnoInfo();
            string[] fields = line.Split('~');
            if (fields.Length != 5 && fields.Length != 6)
            {
                throw new FisException("CHK503", new string[] { line.Substring(0, 30) + "..." });
            }

            ret.internalID = fields[0].Trim();
            ret.pallet_id = fields[1].Trim();
            ret.pallet_box_qty = (decimal)fixQty(fields[2].Trim());
            ret.p4 = fields[3].Trim();
            /*
             * Answer to: ITC-1360-1224
             * Description: Add substring if needed while getting P6.
             */
            if (fields.Length == 5)
            {
                ret.p5 = "";
                ret.p6 = fixQty(fields[4].Trim().Length >= 4 ? fields[4].Trim().Substring(0, 4) : fields[4].Trim()).ToString();
            }
            else
            {
                ret.p5 = fields[4].Trim();
                ret.p6 = fixQty(fields[5].Trim().Length >= 4 ? fields[5].Trim().Substring(0, 4) : fields[5].Trim()).ToString();
            }

            /*
             * Answer to: ITC-1360-1220
             * Description: Add setting: PALLET_UNIT_QTY= PALLET_BOX_QTY.
             */
            ret.pallet_unit_qty = ret.pallet_box_qty.ToString();

            return ret;
        }

        private string GetPrimaryModel(string str, IList<string> strList)
        {
            string ret = (str.Split('/'))[0].Trim();
            while (true)
            {
                bool bChanged = false;
                foreach (string tmp in strList)
                {
                    if (tmp.Contains('/') && (tmp.Split('/'))[1].Trim().Equals(ret))
                    {
                        ret = (tmp.Split('/'))[0].Trim();
                        bChanged = true;
                        break;
                    }
                }
                if (!bChanged)
                {
                    break;
                }
            }
            return ret;
        }

        private string GetUCCorBoxIdFor173(S_PLDNLine dn)
        {
            string prefix = "";
            string regId = (string)dn.HDeliveryInfo["RegId"];
            string deport = (string)dn.HDeliveryInfo["Deport"];
            if (regId == "SNA") prefix = "H410-";
            else if (regId == "SNA") prefix = "H410-";
            else if (regId == "SCA") prefix = "H410-";
            else if (regId == "SNL") prefix = "LA" + deport;
            else if (regId == "SNU") prefix = "D7" + deport;
            else if (regId == "SCU") prefix = "D7" + deport;
            else if (regId == "SNE") prefix = "63D7-";
            else if (regId == "SCE") prefix = "63D7-";
            else if (regId == "SAF") prefix = "H4FN-0";
            else if (regId == "SCN") prefix = "H4FN-0";
            else return "";
            //从SnoCtrl_BoxId里获取满足条件的第一条记录：Cust= BoxId前缀 and valid=’1’，得到BoxId
            SnoCtrlBoxIdInfo info = deliveryRepository.GetSnoCtrlBoxIdInfoByLikeCustAndValid(prefix, "1");
            if (info == null)
            {
                return "";
            }

            //从SnoCtrl_BoxId删除这条记录
            deliveryRepository.DeleteSnoCtrlBoxIdInfo(info.id);

            return info.boxId;
        }

        /// <summary>
        /// 上传PO Data(173船务)
        /// </summary>
        private IList<DNForUI> UploadData173(IList<string> dnLines, IList<string> pnLines, string editor)
        {
            logger.Debug("(_PoData)UploadData173 starts");
            try
            {
                Hashtable hDnList = new Hashtable();
                IList<S_PLPNLine> pnList = new List<S_PLPNLine>();
                IList<string> toAddBAList = new List<string>();
                IList<string> modelList = new List<string>();

                foreach (string tmp in dnLines)
                {
                    S_PLDNLine ele = ParseDNInfo173(tmp);

                    //若DN文件中包含多个相同DN记录，报文件错误
                    if (hDnList.Contains(ele.DeliveryNo))
                    {
                        throw new FisException("CHK504", new string[] { ele.DeliveryNo });
                    }

                    //<UC:m>若DN中包含属性RegId=”SCN”，不导入
                    if ((string)ele.HDeliveryInfo["RegId"] == "SCN")
                    {
                        continue;
                    }

                    //<UC:c>若DN属性RegId=”SUC”，则设置为RegId=”SNU”
                    if ((string)ele.HDeliveryInfo["RegId"] == "SUC")
                    {
                        ele.HDeliveryInfo["RegId"] = "SNU";
                    }

                    //<UC:h>将Delivery.ShipmentNo作为属性Consolidated保存在DeliveryInfo表中，若属性Flag=O 或 C时，属性Consolidated改为RedShipment；Delivery.ShipmentNo只取数据的前10位保存
                    //2012-9-25:删除：若属性Flag=O 或 C时，属性Consolidated改为RedShipment
                    //if ((string)ele.HDeliveryInfo["Flag"] == "O" || (string)ele.HDeliveryInfo["Flag"] == "C")
                    //{
                        //ele.HDeliveryInfo.Add("RedShipment", ele.ShipmentNo);
                    //}
                    //else
                    //{
                    //    ele.HDeliveryInfo.Add("Consolidated", ele.ShipmentNo);
                    //}

                    //<UC:f>(1)将Delivery.Model对应的原文件的值作为属性PartNo保存在DeliveryInfo里
                    ele.HDeliveryInfo.Add("PartNo", ele.Model);
                    modelList.Add(ele.Model);

                    hDnList.Add(ele.DeliveryNo, ele);

                    if ((tmp.Contains("~BNAF~") || tmp.Contains("~DZNA~")) && (tmp.Contains("~SCA~") || tmp.Contains("~SNA~") || tmp.Contains("~SAF~")))
                    {
                        toAddBAList.Add(ele.DeliveryNo);
                    }
                }

                Hashtable hDnQty = new Hashtable();
                foreach (string tmp in pnLines)
                {
                    S_PLPNLine ele = ParsePLPNInfo(tmp);

                    if (!hDnList.Contains(ele.DeliveryNo))
                    {
                        continue;
                    }

                    //<UC:a>对于pallet当PalletType前两码不是ZP(散装出货)时：如果其对应的DN对应的文本文件的属性串中包含”~BNAF~”或”~DZNA~”或”~SCA~”或”~SNA~”或”~SAF~”字串时，PalletNo前加上”BA”；否则PalletNo前加上”NA”
                    if (!ele.PalletType.StartsWith("ZP"))
                    {
                        if (toAddBAList.Contains(ele.DeliveryNo))
                        {
                            ele.PalletNo = "BA" + ele.PalletNo;
                        }
                        else
                        {
                            ele.PalletNo = "NA" + ele.PalletNo;
                        }
                    }

                    if (hDnQty.Contains(ele.DeliveryNo))
                    {
                        hDnQty[ele.DeliveryNo] = fixQty(hDnQty[ele.DeliveryNo].ToString()) + (int)ele.DeliveryQty;
                    }
                    else
                    {
                        hDnQty.Add(ele.DeliveryNo, (int)ele.DeliveryQty);
                    }

                    //<UC:b>先按照DN,Pallet,UCC分组，合并Qty后再作后续处理
                    foreach (S_PLPNLine ele1 in pnList)
                    {
                        if (ele.DeliveryNo == ele1.DeliveryNo && ele.PalletNo == ele1.PalletNo && ele.UCC == ele1.UCC)
                        {
                            ele.DeliveryQty += ele1.DeliveryQty;
                            pnList.Remove(ele1);
                            break;
                        }
                    }

                    //<UC:d>若pallet对应的DN不存在于IMES_PAK.. Delivery，且(DN的RegId属性值等于SCU,SNU之一，and UCC=”” and pallet前2位不等于NA)时，需要重新获取Pallte的UCC，若没有得到，则连同其对应的DN都不导入
                    Delivery tmpDlv = deliveryRepository.Find(ele.DeliveryNo);
                    if (tmpDlv == null && ele.UCC == "" && !ele.PalletNo.StartsWith("NA"))
                    {
                        S_PLDNLine tmpDN = (S_PLDNLine)hDnList[ele.DeliveryNo];
                        string tmpRegId = (string)tmpDN.HDeliveryInfo["RegId"];
                        if (tmpRegId == "SCU" || tmpRegId == "SNU")
                        {
                            string ucc = GetUCCorBoxIdFor173(tmpDN);
                            if (ucc == "")
                            {
                                /*
                                 * Answer to: ITC-1414-0215
                                 * Description: [新需求]在获取Box Id 失败的时候，需要报错并终止流程， 整机和docking都需要修改
                                 */
                                throw new FisException("PAK098", new string[] { tmpDN.DeliveryNo });
                                //hDnList.Remove(ele.DeliveryNo);
                                //continue;
                            }
                            /*
                             * Answer to: ITC-1360-1122
                             * Description: Update UCC of pallet.
                             */
                            ele.UCC = ucc;
                        }
                    }

                    pnList.Add(ele);
                }

                Hashtable tmpHT = hDnList.Clone() as Hashtable;
                hDnList.Clear();

                foreach (DictionaryEntry de in tmpHT)
                {
                    S_PLDNLine ele = (S_PLDNLine)de.Value;
                    //<UC:f>(2)Delivery.Model做转换，详见UC
                    ele.Model = GetPrimaryModel(ele.Model, modelList);

                    //<UC:e>若Delivery.Qty=””时，数量等于sum(Delivery_Pallet.DeliveryQty) group by DeliveryNo；否则参考以下语句作转换：select convert(int,convert(float (18),convert(decimal(15,3), Delivery.Qty)))[对第四位小数四舍五入，然后向下取整]

                    /*
                     * Answer to: ITC-1360-1417
                     * Description: Delivery.Qty data type changed from smallint to int.
                     */
                    if (ele.strQty == "")
                    {
                        /*
                         * Answer to: ITC-1360-1076
                         * Description: Fix bad casting.
                         */
                        ele.Qty = fixQty(hDnQty[ele.DeliveryNo].ToString());
                    }
                    else
                    {
                        ele.Qty = fixQty(ele.strQty);
                    }

                    //<UC:l>若DN对应的(PartNo(Model)前两位=PC或PO)且PartNo not like rtrim(Model)+”/%”时，不导入此记录
                    if ((ele.Model.StartsWith("PC") || ele.Model.StartsWith("PO")) && !(((string)ele.HDeliveryInfo["PartNo"]).StartsWith(ele.Model + "/")))
                    {
                        continue;
                    }
                    hDnList.Add(de.Key, ele);
                }

                //上传DN
                IList<Delivery> deliveryList = new List<Delivery>();
                IList<string> updateList = new List<string>();

                tmpHT.Clear();
                tmpHT = hDnList.Clone() as Hashtable;
                hDnList.Clear();

                foreach (DictionaryEntry de in tmpHT)
                {
                    S_PLDNLine ele = (S_PLDNLine)de.Value;

                    //<UC:i>若DN已存在则不再导入，但DB中的Udt还需要update
                    if (null != deliveryRepository.Find(ele.DeliveryNo))
                    {
                        updateList.Add(ele.DeliveryNo);
                        continue;
                    }
                    Delivery toSave = new Delivery();
                    toSave.DeliveryNo = ele.DeliveryNo;
                    toSave.ShipDate = ele.ShipDate;
                    toSave.PoNo = ele.PoNo;
                    toSave.ModelName = ele.Model;
                    toSave.ShipmentNo = ele.ShipmentNo;
                    toSave.Qty = ele.Qty;
                    //<UC:j>(1)Delivery.Status=00
                    toSave.Status = "00";
                    toSave.Editor = editor;
                    foreach (DictionaryEntry eleInfo in ele.HDeliveryInfo)
                    {
                        //<UC:n>若DN属性值等于空，则不需要在DeliveryInfo里加入记录
                        /*
                         * Answer to: ITC-1360-1842
                         * Description: (2012-6-4)UC updated:RegId 如果解析上传文件得到空值，需要将该属性按照空串记录到DeliveryInfo 表中(PL shipment三种情况均修改)
                         */
                        if (((string)eleInfo.Value) == "" && ((string)eleInfo.Key) != "RegId") continue;
                        toSave.SetExtendedProperty((string)eleInfo.Key, eleInfo.Value, editor);
                    }
                    deliveryList.Add(toSave);
                    hDnList.Add(de.Key, ele);
                }

                //上传PN
                IList<Pallet> palletList = new List<Pallet>();
                IList<DeliveryPalletInfo> dpList = new List<DeliveryPalletInfo>();
                IList<string> savePnList = new List<string>();
                foreach (S_PLPNLine ele in pnList)
                {
                    if (!hDnList.Contains(ele.DeliveryNo))
                    {
                        continue;
                    }
                    
                    /*
                    * Answer to: ITC-1360-1790
                    * Description: Pallet已存在于数据库时，Pallet不再导入，
                    * 但Delivery_Pallet表中仍需导入相关数据，PL-Normal和PL-Domestic中存在相同处理
                    */
                    //<UC:p>若导入的pallet存在于IMES2012_PAK中，则这个pallet不再导入
                    if (!savePnList.Contains(ele.PalletNo) && null == palletRepository.Find(ele.PalletNo))
                    {
                        Pallet toSave = new Pallet();
                        toSave.PalletNo = ele.PalletNo;
                        toSave.UCC = ele.UCC;
                        //<UC:j>(2)Pallet.Station=00
                        toSave.Station = "00";
                        toSave.Editor = editor;
                        palletList.Add(toSave);
                        savePnList.Add(ele.PalletNo);
                    }

                    DeliveryPalletInfo toSave1 = new DeliveryPalletInfo();
                    toSave1.deliveryNo = ele.DeliveryNo;
                    toSave1.palletNo = ele.PalletNo;
                    toSave1.shipmentNo = ((S_PLDNLine)hDnList[ele.DeliveryNo]).ShipmentNo;
                    toSave1.deliveryQty = ele.DeliveryQty;
                    //<UC:j>(3)Delivery_Pallet.Status=0
                    toSave1.status = "0";

                    toSave1.deviceQty = ele.DeviceQty;
                    toSave1.palletType = ele.PalletType.Substring(0, 2);

                    toSave1.editor = editor;
                    dpList.Add(toSave1);
                }
                
                FisException ex;
                List<string> erpara = new List<string>();
                Session.SessionType TheType = Session.SessionType.Common;
                string sessionKey = "UPLOAD_PO_DATA_173";

                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, TheType);

                if (currentSession == null)
                {
                    currentSession = new Session(sessionKey, TheType, editor, "", "", "");

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", "");
                    wfArguments.Add("CurrentFlowSession", currentSession);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", "");
                    wfArguments.Add("Customer", "");
                    wfArguments.Add("SessionType", TheType);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow("UploadPoData.xoml", "UploadPoData.rules", wfArguments);

                    currentSession.SetInstance(instance);
                    currentSession.AddValue("DeliveryList", deliveryList);
                    currentSession.AddValue("PalletList", palletList);
                    currentSession.AddValue("DeliveryPalletList", dpList);
                    currentSession.AddValue("UpdateUDTDNList", updateList);
                    currentSession.AddValue("shipType", "PL-173");

                    if (!SessionManager.GetInstance.AddSession(currentSession))
                    {
                        currentSession.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");
                        erpara.Add(sessionKey);
                        ex = new FisException("CHK192", erpara);
                        throw ex;
                    }

                    currentSession.WorkflowInstance.Start();
                    currentSession.SetHostWaitOne();
                }
                else
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK193", erpara);
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

                //获取界面需要显示的本次上传DN列表
                IList<string> dnList = new List<string>(hDnList.Keys.Cast<string>());
                foreach (string tmp in updateList)
                {
                    dnList.Add(tmp);
                }
                return deliveryRepository.GetDNListByDNList(dnList);
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
                logger.Debug("(_PoData)UploadData173 end");
            }
        }

        /// <summary>
        /// 上传PO Data(PL Normal船务)
        /// </summary>
        private IList<DNForUI> UploadDataPLNormal(IList<string> dnLines, IList<string> pnLines, string editor)
        {
            logger.Debug("(_PoData)UploadDataPLNormal starts");
            try
            {
                bool bExistInDB = false;
                Hashtable hDnList = new Hashtable();
                Hashtable hInvoice = new Hashtable();
                IList<S_PLPNLine> pnList = new List<S_PLPNLine>();
                IList<string> modelList = new List<string>();
                IList<string> consolidatedList = new List<string>();

                foreach (string tmp in dnLines)
                {
                    S_PLDNLine ele = ParsePLDNInfoNormal(tmp);

                    //若DN文件中包含多个相同DN记录，报文件错误
                    if (hDnList.Contains(ele.DeliveryNo))
                    {
                        throw new FisException("CHK504", new string[] { ele.DeliveryNo });
                    }

                    if ((string)ele.HDeliveryInfo["PackingType"] == "")
                    {
                        throw new FisException("CHK505", new string[] { ele.DeliveryNo });
                    }

                    if ((string)ele.HDeliveryInfo["PackingType"] == "S" && (string)ele.HDeliveryInfo["Consolidated"] == "")
                    {
                        throw new FisException("CHK506", new string[] { ele.DeliveryNo });
                    }

                    if ((string)ele.HDeliveryInfo["PAKType"] != "A")
                    {
                        throw new FisException("CHK507", new string[] { ele.DeliveryNo });
                    }

                    //若DN的Consolidated属性值不等于””时，其第11位字符不是”/”，或者‘/’号后的部分不是一个大于0的整数，则报错
                    string thisConsolidated = (string)ele.HDeliveryInfo["Consolidated"];
                    if (thisConsolidated != "" && ((thisConsolidated.Length < 12) || (thisConsolidated[10] != '/') || (fixQty(thisConsolidated.Substring(11)) <= 0)))
                    {
                        throw new FisException("CHK508", new string[] { ele.DeliveryNo });
                    }

                    /*
                     * Answer to: ITC-1360-1335
                     * Description: Pre-caculate InvoiceNum.
                     */
                    //InvoiceNum的比较应使用初始文件中的状态
                    string invoice = (string)ele.HDeliveryInfo["Invoice"];
                    if (invoice != "")
                    {
                        if (hInvoice.Contains(invoice))
                        {
                            hInvoice[invoice] = int.Parse((hInvoice[invoice].ToString())) + 1;
                        }
                        else
                        {
                            hInvoice.Add(invoice, 1);
                        }
                    }
                    
                    //<UC:a>(1)只抓取Delivery.Model前2位=PC的记录
                    if (!ele.Model.StartsWith("PC"))
                    {
                        continue;
                    }
                    //<UC:a>(2)删除已存在于DB中DN记录
                    if (null != deliveryRepository.Find(ele.DeliveryNo))
                    {
                        bExistInDB = true;
                        if (thisConsolidated != "" && !consolidatedList.Contains(thisConsolidated.Substring(0, 10)))
                        {
                            consolidatedList.Add(thisConsolidated.Substring(0, 10));
                        }
                        continue;
                    }

                    //<UC:j>(1)从DN中删除RegId=”SCN”的记录
                    if ((string)ele.HDeliveryInfo["RegId"] == "SCN")
                    {
                        continue;
                    }

                    //<UC:b>若DN属性RegId=”SUC”，则设置为RegId=”SNU”
                    if ((string)ele.HDeliveryInfo["RegId"] == "SUC")
                    {
                        ele.HDeliveryInfo["RegId"] = "SNU";
                    }

                    //<UC:c>当PackingType<>'S'时， Consolidated =””
                    if ((string)ele.HDeliveryInfo["PackingType"] != "S")
                    {
                        ele.HDeliveryInfo["Consolidated"] = "";
                    }

                    //<UC:d>Delivery.ShipmentNo 等于DN
                    ele.ShipmentNo = ele.DeliveryNo;

                    //<UC:e>(1)将Delivery.Model对应的原文件的值作为属性PartNo保存在DeliveryInfo里
                    ele.HDeliveryInfo.Add("PartNo", ele.Model);
                    modelList.Add(ele.Model);

                    //<UC:i>若属性Flag=O 或 C时，属性Consolidated改为RedShipment
                    //2012-9-25:删除：若属性Flag=O 或 C时，属性Consolidated改为RedShipment
                    /*
                    if ((string)ele.HDeliveryInfo["Flag"] == "O" || (string)ele.HDeliveryInfo["Flag"] == "C")
                    {
                        ele.HDeliveryInfo.Add("RedShipment", ele.HDeliveryInfo["Consolidated"]);
                        ele.HDeliveryInfo.Remove("Consolidated");
                    }
                    */                      
                    hDnList.Add(ele.DeliveryNo, ele);
                }
                
                Hashtable tmpHT = hDnList.Clone() as Hashtable;
                hDnList.Clear();

                foreach (DictionaryEntry de in tmpHT)
                {
                    S_PLDNLine ele = (S_PLDNLine)de.Value;
                    string thisConsolidated = (string)ele.HDeliveryInfo["Consolidated"];
                    if (thisConsolidated == null)
                    {
                        thisConsolidated = (string)ele.HDeliveryInfo["RedShipment"];
                    }
                    /*
                     * Answer to: ITC-1360-1075
                     * Description: Wrong processing during excluding DNs with same 10-bit Consolidated value.
                     */
                    if (thisConsolidated != null && thisConsolidated.Length >= 10 && consolidatedList.Contains(thisConsolidated.Substring(0, 10))) continue;
                    hDnList.Add(de.Key, ele);
                }

                if (hDnList.Count == 0)
                {
                    if (bExistInDB)
                    {
                        throw new FisException("CHK509", new string[] { });
                    }
                    else
                    {
                        throw new FisException("CHK516", new string[] { });
                    }
                }

                foreach (DictionaryEntry de in hDnList)
                {
                    S_PLDNLine ele = (S_PLDNLine)de.Value;
                    string invoice = ele.HDeliveryInfo["Invoice"].ToString();
                    if (invoice == "") continue;
                    if (int.Parse(ele.HDeliveryInfo["InvoiceNum"].ToString()) != int.Parse(hInvoice[invoice].ToString()))
                    {
                        /*
                         * Answer to: ITC-1360-1081
                         * Description: Lack of argument to log.
                         */
                        throw new FisException("CHK510", new string[] { invoice });
                    }
                }

                IList<string> pltDNList = new List<string>();
                foreach (string tmp in pnLines)
                {
                    S_PLPNLine ele = ParsePLPNInfo(tmp);

                    if (!hDnList.Contains(ele.DeliveryNo))
                    {
                        continue;
                    }

                    //<UC:f>对于pallet当PalletType前两码是ZC，且PalletNo的前2位不是”BA”或”NA”时：如果其对应的DN的Carrier对应的属性值等于(”BNAF”，”DZNA”，”NPN9”，”EXDO”)之一，DN的RegId对应的属性值等于(“SNA”，”SAF”，”SCA”)之一时，PalletNo前加上”BA”；否则PalletNo前加上”NA”
                    if (ele.PalletType.StartsWith("ZC") && !ele.PalletNo.StartsWith("BA") && !ele.PalletNo.StartsWith("NA"))
                    {
                        string carrier = ((S_PLDNLine)hDnList[ele.DeliveryNo]).HDeliveryInfo["Carrier"].ToString();
                        string regid = ((S_PLDNLine)hDnList[ele.DeliveryNo]).HDeliveryInfo["RegId"].ToString();
                        if ((carrier == "BNAF" || carrier == "DZNA" || carrier == "NPN9" || carrier == "EXDO")
                            && (regid == "SNA" || regid == "SAF" || regid == "SCA"))
                        {
                            ele.PalletNo = "BA" + ele.PalletNo;
                        }
                        else
                        {
                            ele.PalletNo = "NA" + ele.PalletNo;
                        }
                    }

                    /*
                     * Answer to: ITC-1360-1561
                     * Description: Merge pn records(New requirement by UC).
                     */
                    //<UC:ef>对于pallet数据，注意相同的DN，Pallet，UCC的数据作合并，DeliveryQty相加
                    foreach (S_PLPNLine ele1 in pnList)
                    {
                        if (ele.DeliveryNo == ele1.DeliveryNo && ele.PalletNo == ele1.PalletNo && ele.UCC == ele1.UCC)
                        {
                            ele.DeliveryQty += ele1.DeliveryQty;
                            pnList.Remove(ele1);
                            break;
                        }
                    }

                    pnList.Add(ele);
                    pltDNList.Add(ele.DeliveryNo);
                }

                foreach (string tmp in hDnList.Keys)
                {
                    if (!pltDNList.Contains(tmp))
                    {
                        throw new FisException("CHK511", new string[] { tmp });
                    }
                }

                //上传DN
                IList<Delivery> deliveryList = new List<Delivery>();
                foreach (DictionaryEntry de in hDnList)
                {
                    S_PLDNLine ele = (S_PLDNLine)de.Value;

                    //<UC:e>(2)Delivery.Model做转换，详见UC
                    ele.Model = GetPrimaryModel(ele.Model, modelList);

                    Delivery toSave = new Delivery();
                    toSave.DeliveryNo = ele.DeliveryNo;
                    toSave.ShipDate = ele.ShipDate;
                    toSave.PoNo = ele.PoNo;
                    toSave.ModelName = ele.Model;
                    toSave.ShipmentNo = ele.ShipmentNo;
                    toSave.Qty = ele.Qty;
                    //<UC:g>(1)Delivery.Status=00
                    toSave.Status = "00";
                    toSave.Editor = editor;
                    foreach (DictionaryEntry eleInfo in ele.HDeliveryInfo)
                    {
                        //<UC:k>若DN属性值等于空，则不需要在DeliveryInfo里加入记录
                        if (((string)eleInfo.Value) == "" && ((string)eleInfo.Key) != "RegId") continue;
                        toSave.SetExtendedProperty((string)eleInfo.Key, eleInfo.Value, editor);
                    }
                    deliveryList.Add(toSave);
                }

                //上传PN
                IList<Pallet> palletList = new List<Pallet>();
                IList<Pallet> notSavePalletList = new List<Pallet>();
                IList<DeliveryPalletInfo> dpList = new List<DeliveryPalletInfo>();
                IList<string> savePnList = new List<string>();
                foreach (S_PLPNLine ele in pnList)
                {
                    if (!hDnList.Contains(ele.DeliveryNo))
                    {
                        continue;
                    }
                    
                    //(4/27)若导入的pallet存在于Pallet表中，则这个pallet不再导入
                    if (!savePnList.Contains(ele.PalletNo))
                    {
                        savePnList.Add(ele.PalletNo);
                        Pallet toSave = new Pallet();
                        toSave.PalletNo = ele.PalletNo;
                        toSave.UCC = ele.UCC;
                        //<UC:g>(2)Pallet.Station=00
                        toSave.Station = "00";
                        toSave.Editor = editor;
                        if (null == palletRepository.Find(ele.PalletNo))
                        {
                            palletList.Add(toSave);
                        }
                        else
                        {
                            notSavePalletList.Add(toSave);
                        }
                    }

                    DeliveryPalletInfo toSave1 = new DeliveryPalletInfo();
                    toSave1.deliveryNo = ele.DeliveryNo;
                    toSave1.palletNo = ele.PalletNo;
                    toSave1.shipmentNo = ((S_PLDNLine)hDnList[ele.DeliveryNo]).ShipmentNo;
                    toSave1.deliveryQty = ele.DeliveryQty;
                    //<UC:g>(3)Delivery_Pallet.Status=0
                    toSave1.status = "0";
                    toSave1.editor = editor;

                    toSave1.deviceQty = ele.DeviceQty;
                    toSave1.palletType = ele.PalletType.Substring(0, 2);
                    
                    dpList.Add(toSave1);
                }

                FisException ex;
                List<string> erpara = new List<string>();
                Session.SessionType TheType = Session.SessionType.Common;
                string sessionKey = "UPLOAD_PO_DATA_PLNormal";

                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, TheType);

                if (currentSession == null)
                {
                    currentSession = new Session(sessionKey, TheType, editor, "", "", "");

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", "");
                    wfArguments.Add("CurrentFlowSession", currentSession);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", "");
                    wfArguments.Add("Customer", "");
                    wfArguments.Add("SessionType", TheType);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow("UploadPoData.xoml", "UploadPoData.rules", wfArguments);

                    currentSession.SetInstance(instance);
                    currentSession.AddValue("DeliveryList", deliveryList);
                    currentSession.AddValue("PalletList", palletList);
                    currentSession.AddValue("NotSavePalletList", notSavePalletList);
                    currentSession.AddValue("DeliveryPalletList", dpList);
                    currentSession.AddValue("shipType", "PL-Normal");

                    if (!SessionManager.GetInstance.AddSession(currentSession))
                    {
                        currentSession.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");
                        erpara.Add(sessionKey);
                        ex = new FisException("CHK192", erpara);
                        throw ex;
                    }

                    currentSession.WorkflowInstance.Start();
                    currentSession.SetHostWaitOne();
                }
                else
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK193", erpara);
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

                //获取界面需要显示的本次上传DN列表
                return deliveryRepository.GetDNListByDNList(new List<string>(hDnList.Keys.Cast<string>()));
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
                logger.Debug("(_PoData)UploadDataPLNormal end");
            }
        }

        /// <summary>
        /// 上传PO Data(PL Domestic船务)
        /// </summary>
        private IList<DNForUI> UploadDataPLDomestic(IList<string> dnLines, IList<string> pnLines, string editor)
        {
            logger.Debug("(_PoData)UploadDataPLDomestic starts");
            try
            {
                Hashtable hDnList = new Hashtable();
                Hashtable hInvoice = new Hashtable();
                Hashtable hCQty = new Hashtable();
                IList<S_PLPNLine> pnList = new List<S_PLPNLine>();
                IList<string> toAddBAList = new List<string>();
                IList<string> modelList = new List<string>();

                foreach (string tmp in dnLines)
                {
                    S_PLDNLine ele = ParsePLDNInfoDomestic(tmp);

                    //若DN文件中包含多个相同DN记录，报文件错误
                    if (hDnList.Contains(ele.DeliveryNo))
                    {
                        throw new FisException("CHK504", new string[] { ele.DeliveryNo });
                    }

                    if ((string)ele.HDeliveryInfo["PackingType"] == "S" && (string)ele.HDeliveryInfo["Consolidated"] == "")
                    {
                        throw new FisException("CHK506", new string[] { ele.DeliveryNo });
                    }

                    if ((string)ele.HDeliveryInfo["PAKType"] != "A" && ((string)ele.HDeliveryInfo["PackingType"] == "" || (string)ele.HDeliveryInfo["PackingLV"] == "" || (string)ele.HDeliveryInfo["CnQty"] == "" || (string)ele.HDeliveryInfo["PalletQty"] == ""))
                    {
                        throw new FisException("CHK512", new string[] { ele.DeliveryNo });
                    }

                    if ((string)ele.HDeliveryInfo["PAKType"] == "")
                    {
                        throw new FisException("CHK513", new string[] { ele.DeliveryNo });
                    }

                    //Invoice_NO的比较应使用初始文件中的状态
                    string invoice = ele.HDeliveryInfo["Invoice"].ToString();
                    if (invoice != "")
                    {
                        if (hInvoice.Contains(invoice))
                        {
                            hInvoice[invoice] = int.Parse(hInvoice[invoice].ToString()) + 1;
                        }
                        else
                        {
                            hInvoice.Add(invoice, 1);
                        }
                    }

                    //<UC:a>(3)删除掉PartNo属性的前两位等于PC，但不包含”/”的DN及其对应的pallet
                    if (ele.Model.StartsWith("PC") && !ele.Model.Contains("/"))
                    {
                        continue;
                    }

                    //<UC:a>(1)将Delivery.Model对应的原文件的值作为属性PartNo保存在DeliveryInfo里
                    ele.HDeliveryInfo.Add("PartNo", ele.Model);
                    modelList.Add(ele.Model);

                    //<UC:c>Delivery.ShipmentNo: 当PAKType<>A时，ShipmentNo=DN的前十位；当PAKType=A时，ShipmentNo=DN
                    if ((string)ele.HDeliveryInfo["PAKType"] != "A" && ele.DeliveryNo.Length >= 10)
                    {
                        ele.ShipmentNo = ele.DeliveryNo.Substring(0, 10);
                    }
                    else
                    {
                        ele.ShipmentNo = ele.DeliveryNo;
                    }

                    //<UC:f>(1)删除RegId<>”SCN”的DN及其对应的pallet
                    if ((string)ele.HDeliveryInfo["RegId"] != "SCN")
                    {
                        continue;
                    }

                    if ((string)ele.HDeliveryInfo["PAKType"] != "A")
                    {
                        string str1 = ele.HDeliveryInfo["Consolidated"].ToString();
                        if (str1 != "" && str1.Length >= 10)
                        {
                            string con = str1.Substring(0, 10);
                            string dn = ele.DeliveryNo.Substring(0, 10);
                            if (hCQty.Contains(con))
                            {
                                IList<string> conDN = hCQty[con] as List<string>;
                                if (!conDN.Contains(dn))
                                {
                                    conDN.Add(dn);
                                }
                                hCQty[con] = conDN;
                            }
                            else
                            {
                                IList<string> conDN = new List<string>();
                                conDN.Add(dn);
                                hCQty.Add(con, conDN);
                            }
                        }
                    }

                    //<UC:i>若属性Flag=O 或 C时，属性Consolidated改为RedShipment
                    //2012-9-25:删除：若属性Flag=O 或 C时，属性Consolidated改为RedShipment
                    /*
                    if ((string)ele.HDeliveryInfo["Flag"] == "O" || (string)ele.HDeliveryInfo["Flag"] == "C")
                    {
                        /*
                         * Answer to: ITC-1360-0869
                         * Description: Set attr value of RedShipment to the old value of Consolidated.
                         * /
                        ele.HDeliveryInfo.Add("RedShipment", ele.HDeliveryInfo["Consolidated"]);
                        ele.HDeliveryInfo.Remove("Consolidated");
                    }
                    */

                    hDnList.Add(ele.DeliveryNo, ele);

                    if ((tmp.Contains("~EXDO~") || tmp.Contains("~BNAF~") || tmp.Contains("~DZNA~")) && (tmp.Contains("~SCA~") || tmp.Contains("~SNA~") || tmp.Contains("~SAF~")))
                    {
                        toAddBAList.Add(ele.DeliveryNo);
                    }
                }

                foreach (DictionaryEntry de in hDnList)
                {
                    S_PLDNLine ele = (S_PLDNLine)de.Value;
                    string invoice = ele.HDeliveryInfo["Invoice"].ToString();
                    if (invoice == "") continue;
                    if (int.Parse(ele.HDeliveryInfo["Invoice_NO"].ToString()) != int.Parse(hInvoice[invoice].ToString()))
                    {
                        throw new FisException("CHK510", new string[] { invoice });
                    }
                }

                IList<string> pltDNList = new List<string>();
                foreach (string tmp in pnLines)
                {
                    S_PLPNLine ele = ParsePLPNInfo(tmp);
                 
                    if (!hDnList.Contains(ele.DeliveryNo))
                    {
                        continue;
                    }

                    //<UC:d>当PalletType的前2位不等于ZP时：如果其对应的DN对应的文本文件的属性串中包含”~EXDO~”或”~BNAF~”或”~DZNA~”或”~SCA~”或”~SNA~”或”~SAF~”字串时，PalletNo前加上”BA”；否则PalletNo前加上”NA”
                    if (!ele.PalletType.StartsWith("ZP"))
                    {
                        if (toAddBAList.Contains(ele.DeliveryNo))
                        {
                            ele.PalletNo = "BA" + ele.PalletNo;
                        }
                        else
                        {
                            ele.PalletNo = "NA" + ele.PalletNo;
                        }
                    }

                    //<UC:g>对于pallet数据，注意相同的DN，Pallet，UCC的数据作合并，DeliveryQty相加
                    foreach (S_PLPNLine ele1 in pnList)
                    {
                        if (ele.DeliveryNo == ele1.DeliveryNo && ele.PalletNo == ele1.PalletNo && ele.UCC == ele1.UCC)
                        {
                            ele.DeliveryQty += ele1.DeliveryQty;
                            pnList.Remove(ele1);
                            break;
                        }
                    }

                    pnList.Add(ele);
                    pltDNList.Add(ele.DeliveryNo);
                }

                foreach (string tmp in hDnList.Keys)
                {
                    if (!pltDNList.Contains(tmp) && ((S_PLDNLine)hDnList[tmp]).HDeliveryInfo["PAKType"].ToString() == "A")
                    {
                        throw new FisException("CHK511", new string[] { tmp });
                    }
                }

                IList<string> dns = new List<string>(hDnList.Keys.Cast<string>());
                IList<string> conList = new List<string>();

                string badStr = "";
                foreach (DictionaryEntry de in hDnList)
                {
                    S_PLDNLine ele = (S_PLDNLine)de.Value;
                    if ((string)ele.HDeliveryInfo["PAKType"] == "A") continue;
                    /*
                    * Answer to: ITC-1360-1336
                    * Description: If Consolidated has changed to RedShipment, then get RedShipment instead of Consolidated.
                    */
                    string str1 = (string)ele.HDeliveryInfo["Consolidated"];
                    if (str1 == null)
                    {
                        str1 = (string)ele.HDeliveryInfo["RedShipment"];
                    }
                    if (str1 == null || str1 == "" || conList.Contains(str1)) continue;
                    conList.Add(str1);
                    string preStr = "";
                    int cqty = 0;
                    /*
                    * Answer to: ITC-1360-1283
                    * Description: To deal with bad string of "Consolidated".
                    */
                    if (str1.Length >= 10)
                    {
                        preStr = str1.Substring(0, 10);
                        if (str1.Length >= 17)
                        {
                            cqty = fixQty(str1.Substring(11, 6));
                        }
                        else
                        {
                            cqty = fixQty(str1.Substring(11));
                        }
                    }
                    
                    int cnt = (hCQty[preStr] as List<string>).Count;
                    if (cnt != cqty && !badStr.Contains(preStr))
                    {
                        badStr += preStr + ",";
                    }
                }
                if (badStr.EndsWith(","))
                {
                    throw new FisException("CHK514", new string[] { badStr.Remove(badStr.Length - 1) });
                }
                

                //上传DN
                IList<Delivery> deliveryList = new List<Delivery>();
                IList<string> updateList = new List<string>();

                Hashtable tmpHT = hDnList.Clone() as Hashtable;
                hDnList.Clear();

                foreach (DictionaryEntry de in tmpHT)
                {
                    if (null != deliveryRepository.Find(de.Key))
                    {
                        updateList.Add(de.Key as string);
                        continue;
                    }

                    S_PLDNLine ele = (S_PLDNLine)de.Value;

                    //<UC:a>(2)Delivery.Model做转换，详见UC
                    ele.Model = GetPrimaryModel(ele.Model, modelList);

                    //<UC:e>删除Model的前两位不等于PC的DN及其对应的pallet
                    if (!ele.Model.StartsWith("PC"))
                    {
                        continue;
                    }

                    Delivery toSave = new Delivery();
                    toSave.DeliveryNo = ele.DeliveryNo;
                    toSave.ShipDate = ele.ShipDate;
                    toSave.PoNo = ele.PoNo;
                    toSave.ModelName = ele.Model;
                    toSave.ShipmentNo = ele.ShipmentNo;
                    toSave.Qty = ele.Qty;
                    //<UC:h>(1)Delivery.Status=00
                    toSave.Status = "00";
                    toSave.Editor = editor;
                    foreach (DictionaryEntry eleInfo in ele.HDeliveryInfo)
                    {
                        //若DN属性值等于空，则不需要在DeliveryInfo里加入记录
                        if (((string)eleInfo.Value) == "" && ((string)eleInfo.Key) != "RegId") continue;
                        toSave.SetExtendedProperty((string)eleInfo.Key, eleInfo.Value, editor);
                    }
                    deliveryList.Add(toSave);
                    hDnList.Add(de.Key, ele);
                }

                //上传PN
                IList<Pallet> palletList = new List<Pallet>();
                IList<Pallet> notSavePalletList = new List<Pallet>();
                IList<DeliveryPalletInfo> dpList = new List<DeliveryPalletInfo>();
                IList<string> savePnList = new List<string>();
                foreach (S_PLPNLine ele in pnList)
                {
                    if (!hDnList.Contains(ele.DeliveryNo))
                    {
                        continue;
                    }

                    //(4/27)若导入的pallet存在于Pallet表中，则这个pallet不再导入
                    if (!savePnList.Contains(ele.PalletNo))
                    {
                        savePnList.Add(ele.PalletNo);
                        Pallet toSave = new Pallet();
                        toSave.PalletNo = ele.PalletNo;
                        toSave.UCC = ele.UCC;
                        //<UC:h>(2)Pallet.Station=00
                        toSave.Station = "00";
                        toSave.Editor = editor;
                        if (null == palletRepository.Find(ele.PalletNo))
                        {
                            palletList.Add(toSave);
                        }
                        else
                        {
                            notSavePalletList.Add(toSave);
                        }
                    }

                    DeliveryPalletInfo toSave1 = new DeliveryPalletInfo();
                    toSave1.deliveryNo = ele.DeliveryNo;
                    toSave1.palletNo = ele.PalletNo;
                    toSave1.shipmentNo = ((S_PLDNLine)hDnList[ele.DeliveryNo]).ShipmentNo;
                    toSave1.deliveryQty = ele.DeliveryQty;
                    //<UC:h>(3)Delivery_Pallet.Status=0
                    toSave1.status = "0";
                    toSave1.editor = editor;

                    toSave1.deviceQty = ele.DeviceQty;
                    toSave1.palletType = ele.PalletType.Substring(0, 2);
                    
                    dpList.Add(toSave1);
                }

                FisException ex;
                List<string> erpara = new List<string>();
                Session.SessionType TheType = Session.SessionType.Common;
                string sessionKey = "UPLOAD_PO_DATA_PLDomestic";

                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, TheType);

                if (currentSession == null)
                {
                    currentSession = new Session(sessionKey, TheType, editor, "", "", "");

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", "");
                    wfArguments.Add("CurrentFlowSession", currentSession);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", "");
                    wfArguments.Add("Customer", "");
                    wfArguments.Add("SessionType", TheType);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow("UploadPoData.xoml", "UploadPoData.rules", wfArguments);

                    currentSession.SetInstance(instance);
                    currentSession.AddValue("DeliveryList", deliveryList);
                    currentSession.AddValue("PalletList", palletList);
                    currentSession.AddValue("NotSavePalletList", notSavePalletList);
                    currentSession.AddValue("DeliveryPalletList", dpList);
                    currentSession.AddValue("UpdateUDTDNList", updateList);
                    currentSession.AddValue("shipType", "PL-Domestic");

                    if (!SessionManager.GetInstance.AddSession(currentSession))
                    {
                        currentSession.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");
                        erpara.Add(sessionKey);
                        ex = new FisException("CHK192", erpara);
                        throw ex;
                    }

                    currentSession.WorkflowInstance.Start();
                    currentSession.SetHostWaitOne();
                }
                else
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK193", erpara);
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
                
                //获取界面需要显示的本次上传DN列表
                IList<string> dnList = new List<string>(hDnList.Keys.Cast<string>());
                foreach (string tmp in updateList)
                {
                    dnList.Add(tmp);
                }
                return deliveryRepository.GetDNListByDNList(dnList);
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
                logger.Debug("(_PoData)UploadDataPLDomestic end");
            }
        }

        /// <summary>
        /// 上传PO Data(OB船务)
        /// </summary>
        private IList<DNForUI> UploadDataOB(string type, IList<string> dnLines, IList<string> pnLines, string editor)
        {
            logger.Debug("(_PoData)UploadDataOB starts");
            try
            {
                IList<PakDotPakComnInfo> dotDNList = new List<PakDotPakComnInfo>();
                IList<PakDashPakComnInfo> dashDNList = new List<PakDashPakComnInfo>();
                IList<PakDotPakPaltnoInfo> palletList = new List<PakDotPakPaltnoInfo>();
                IList<string> dnList = new List<string>();

                foreach (string tmp in dnLines)
                {
                    PakDotPakComnInfo ele = ParseOBDNInfo(tmp);
                    if (type == "OB-Normal")
                    {
                        if (ele.ship_mode == "FDE") ele.ship_mode = "Air";
                    }
                    else
                    {
                        if (ele.region == "SCN") ele.ship_mode = "Truck";
                    }
                    dotDNList.Add(ele);

                    PakDashPakComnInfo ele1 = CopyPakComnInfo(ele);
                    dashDNList.Add(ele1);

                    dnList.Add(ele.internalID);
                }

                foreach (string tmp in pnLines)
                {
                    PakDotPakPaltnoInfo ele = ParseOBPNInfo(tmp);

                    if (!dnList.Contains(ele.internalID))
                    {
                        continue;
                    }

                    palletList.Add(ele);
                }

                FisException ex;
                List<string> erpara = new List<string>();
                Session.SessionType TheType = Session.SessionType.Common;
                string sessionKey = "";
                if (type == "OB-Normal")
                {
                    sessionKey = "UPLOAD_PO_DATA_OBNormal";
                }
                else
                {
                    sessionKey = "UPLOAD_PO_DATA_OBDomestic";
                }

                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, TheType);

                if (currentSession == null)
                {
                    currentSession = new Session(sessionKey, TheType, editor, "", "", "");

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", "");
                    wfArguments.Add("CurrentFlowSession", currentSession);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", "");
                    wfArguments.Add("Customer", "");
                    wfArguments.Add("SessionType", TheType);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow("UploadPoData.xoml", "UploadPoData.rules", wfArguments);

                    currentSession.SetInstance(instance);
                    currentSession.AddValue("dotDNList", dotDNList);
                    currentSession.AddValue("dashDNList", dashDNList);
                    currentSession.AddValue("PalletList", palletList);
                    currentSession.AddValue("shipType", type);

                    if (!SessionManager.GetInstance.AddSession(currentSession))
                    {
                        currentSession.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");
                        erpara.Add(sessionKey);
                        ex = new FisException("CHK192", erpara);
                        throw ex;
                    }

                    currentSession.WorkflowInstance.Start();
                    currentSession.SetHostWaitOne();
                }
                else
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK193", erpara);
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

                //获取界面需要显示的本次上传DN列表
                if (type == "OB-Normal")
                {
                    return deliveryRepository.GetDNListByDNListFromPoDataEDI(dnList);
                }
                else
                {
                    return deliveryRepository.GetDNListByDNListFromPakDotPakComn(dnList);
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
                logger.Debug("(_PoData)UploadDataOB end");
            }
        }

        /// <summary>
        /// 上传PO Data
        /// </summary>
        public IList<DNForUI> UploadData(string type, IList<string> dnLines, IList<string> pnLines, string editor)
        {
            logger.Debug("(_PoData)UploadData starts");
            try
            {
                if (dnLines.Count == 0 && pnLines.Count == 0)
                {
                    throw new FisException("CHK257", new string[] { });
                }

                if (type == "PL-173")
                {
                    return UploadData173(dnLines, pnLines, editor);
                }

                if (type == "PL-Normal")
                {
                    return UploadDataPLNormal(dnLines, pnLines, editor);
                }

                if (type == "PL-Domestic")
                {
                    return UploadDataPLDomestic(dnLines, pnLines, editor);
                }

                if (type == "OB-Normal" || type == "OB-Domestic")
                {
                    return UploadDataOB(type, dnLines, pnLines, editor);
                }

                throw new Exception("(_PoData)UploadData - bad argument type[" + type + "]");
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
                logger.Debug("(_PoData)UploadData end");
            }
        }
        
        /// <summary>
        /// 获取符合输入条件的DN列表
        /// </summary>
        public IList<DNForUI> QueryData(string type, DNQueryCondition cond, out int realCount)
        {
            logger.Debug("(_PoData)QueryData start");
            try
            {
                if (type.StartsWith("PL"))
                {
                    IList<DNForUI> ret = deliveryRepository.GetDNListByCondition(cond, out realCount);
                    return ret;
                }
                else if (type == "OB-Normal")
                {
                    IList<DNForUI> ret = deliveryRepository.GetDNListByConditionFromPoDataEdi(cond, out realCount);
                    return ret;
                }
                else if (type == "OB-Domestic")
                {
                    IList<DNForUI> ret = deliveryRepository.GetDNListByConditionFromPakDotPakComn(cond, out realCount);
                    return ret;
                }
                else
                {
                    throw new Exception("(_PoData)QueryData - bad argument type[" + type + "]");
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
                logger.Debug("(_PoData)QueryData end");
            }
        }

        /// <summary>
        /// 获取符合输入条件的DN列表(delete for OB user page)
        /// </summary>
        public IList<VPakComnInfo> QueryOBData(string input, out int realCount)
        {
            logger.Debug("(_PoData)QueryOBData start");
            try
            {
                /* for debug
                IList<VPakComnInfo> ret = new List<VPakComnInfo>();
                VPakComnInfo ele = new VPakComnInfo();
                ele.consol_envoice = "ABCDEFG";
                ele.internalID = "HIJKLMN";
                ele.intl_carrier = "OPQ RST";
                ele.waybill_number = "UVW XYZ";
                ret.Add(ele);
                realCount = 1;
                return ret;
                 */
                return pizzaRepository.GetVPakComnListByInternalIdOrConsolInvoiceOrWaybillNumber(input, out realCount);
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
                logger.Debug("(_PoData)QueryOBData end");
            }
        }

        public class IcpDNInfoForUI : IComparer<DNInfoForUI>
        {
            public int Compare(DNInfoForUI x, DNInfoForUI y)
            {
                return x.InfoType.CompareTo(y.InfoType);
            }
        }
        /*
        public class IcpDNForUI : IComparer<DNForUI>
        {
            public int Compare(DNForUI x, DNForUI y)
            {
                return x.DeliveryNo.CompareTo(y.DeliveryNo);
            }
        }
        */
        /// <summary>
        /// 获取DN属性列表
        /// </summary>
        public IList<DNInfoForUI> GetDNInfoList(string type, string dn)
        {
            logger.Debug("(_PoData)GetDNInfoList start, DeliveryNo:" + dn);
            try
            {
                if (type.StartsWith("PL"))
                {
                    return deliveryRepository.GetDNInfoList(dn);
                }
                else if (type == "OB-Normal")
                {
                    //在DN Info中显示PoData_EDI.Descr表里的相关信息；按属性名排序
                    PoDataEdiInfo ediInfo = deliveryRepository.GetPoDataEdiInfo(dn);
                    List<DNInfoForUI> ret = new List<DNInfoForUI>();

                    string[] fields = ediInfo.descr.Split('~');
                    foreach (string tmp in fields)
                    {
                        if (tmp == "" || !tmp.Contains("=")) continue;
                        string[] strs = tmp.Split('=');
                        if (strs.Length != 2 || strs[1] == "") continue;
                        DNInfoForUI info = new DNInfoForUI();
                        info.InfoType = strs[0];
                        info.InfoValue = strs[1];
                        ret.Add(info);
                    }

                    ret.Sort(new IcpDNInfoForUI());
                    
                    return ret;
                }
                else if (type == "OB-Domestic")
                {
                    return deliveryRepository.GetPakDotPakComnInKeyValue(dn);
                }
                else
                {
                    throw new Exception("(_PoData)GetDNInfoList - bad argument type[" + type + "]");
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
                logger.Debug("(_PoData)GetDNInfoList end, DeliveryNo:" + dn);
            }
        }

        /// <summary>
        /// 获取DN对应的Pallet列表
        /// </summary>
        public IList<DNPalletQty> GetDNPalletList(string type, string dn)
        {
            logger.Debug("(_PoData)GetDNPalletList start, DeliveryNo:" + dn);
            try
            {
                if (type.StartsWith("PL"))
                {
                    return deliveryRepository.GetPalletList(dn);
                }
                else if (type == "OB-Normal")
                {
                    return deliveryRepository.GetPalletListFromPoPltEdi(dn);
                }
                else if (type == "OB-Domestic")
                {
                    return deliveryRepository.GetPakDotPakPaltnoInfoList(dn);
                }
                else
                {
                    throw new Exception("(_PoData)GetDNInfoList - bad argument type[" + type + "]");
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
                logger.Debug("(_PoData)GetDNPalletList end, DeliveryNo:" + dn);
            }
        }

        /// <summary>
        /// 获取Shipment对应的PalletCapacity列表
        /// </summary>
        public IList<PalletCapacityInfo> GetDNPalletCapacityList(string type, string ship)
        {
            logger.Debug("(_PoData)GetDNPalletCapacityList start, ShipmentNo.:" + ship);
            try
            {
                if (type.StartsWith("PL"))
                {
                    return palletRepository.GetPalletCapacityInfoList(ship);
                }
                else
                {
                    throw new Exception("(_PoData)GetDNPalletCapacityList - bad argument type[" + type + "]");
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
                logger.Debug("(_PoData)GetDNPalletCapacityList end, ShipmentNo.:" + ship);
            }
        }

        /// <summary>
        /// 删除DN(for PL user)
        /// </summary>
        public int DeletePLDN(string dn, string editor)
        {
            logger.Debug("(_PoData)DeletePLDN start, DeliveryNo:" + dn);

            try
            {
                //DN已与unit绑定
                if (productRepository.GetCombinedQtyByDN(dn) != 0)
                {
                    return -1;
                }

                //DN为SAP分配栈板
                if (deliveryRepository.Find(dn).ShipmentNo.Length == 16 && deliveryRepository.GetDeliveryInfoValue(dn, "PAKType") == "A")
                {
                    return -2;
                }

                FisException ex;
                List<string> erpara = new List<string>();
                Session.SessionType TheType = Session.SessionType.Common;
                string sessionKey = dn;

                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, TheType);

                if (currentSession == null)
                {
                    currentSession = new Session(sessionKey, TheType, editor, "", "", "");

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", "");
                    wfArguments.Add("CurrentFlowSession", currentSession);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", "");
                    wfArguments.Add("Customer", "");
                    wfArguments.Add("SessionType", TheType);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow("PoDataDeletePLDN.xoml", "", wfArguments);

                    currentSession.SetInstance(instance);
                    currentSession.AddValue(Session.SessionKeys.DeliveryNo, dn);

                    if (!SessionManager.GetInstance.AddSession(currentSession))
                    {
                        currentSession.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");
                        erpara.Add(sessionKey);
                        ex = new FisException("CHK192", erpara);
                        throw ex;
                    }

                    currentSession.WorkflowInstance.Start();
                    currentSession.SetHostWaitOne();
                }
                else
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK193", erpara);
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

                return 0;
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
                logger.Debug("(_PoData)DeletePLDN end, DeliveryNo:" + dn);
            }
        }

        /// <summary>
        /// 删除DN(for OB user)
        /// </summary>
        public void DeleteOBDN(string dn, string editor)
        {
            logger.Debug("(_PoData)DeleteOBDN start, DeliveryNo:" + dn);
            try
            {
                FisException ex;
                List<string> erpara = new List<string>();
                Session.SessionType TheType = Session.SessionType.Common;
                string sessionKey = dn;

                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, TheType);

                if (currentSession == null)
                {
                    currentSession = new Session(sessionKey, TheType, editor, "", "", "");

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", "");
                    wfArguments.Add("CurrentFlowSession", currentSession);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", "");
                    wfArguments.Add("Customer", "");
                    wfArguments.Add("SessionType", TheType);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow("PoDataDeleteOBDN.xoml", "", wfArguments);

                    currentSession.SetInstance(instance);
                    currentSession.AddValue(Session.SessionKeys.DeliveryNo, dn);

                    if (!SessionManager.GetInstance.AddSession(currentSession))
                    {
                        currentSession.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");
                        erpara.Add(sessionKey);
                        ex = new FisException("CHK192", erpara);
                        throw ex;
                    }

                    currentSession.WorkflowInstance.Start();
                    currentSession.SetHostWaitOne();
                }
                else
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK193", erpara);
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

                return;
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
                logger.Debug("(_PoData)DeleteOBDN end, DeliveryNo:" + dn);
            }
        }

        /// <summary>
        /// 删除DN By 10-bit DN List(for OB user)
        /// </summary>
        public void BatchDeleteOBDN(IList<string> dnList, string editor, out IList<string> fList)
        {
            logger.Debug("(_PoData)BatchDeleteOBDN start");
            fList = new List<string>();
            IList<string> delDNList = new List<string>();
            
            foreach (string pre in dnList)
            {
                if (pre.Trim().Length != 10)
                {
                    fList.Add(pre.Trim());
                }
                else
                {
                    delDNList.Add(pre.Trim());
                }
            }

            try
            {
                if (delDNList.Count == 0) return;

                FisException ex;
                List<string> erpara = new List<string>();
                Session.SessionType TheType = Session.SessionType.Common;
                string sessionKey = "BATCH_DELETE_OBDN_" + Guid.NewGuid().ToString().Replace("-", "");

                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, TheType);

                if (currentSession == null)
                {
                    currentSession = new Session(sessionKey, TheType, editor, "", "", "");

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", "");
                    wfArguments.Add("CurrentFlowSession", currentSession);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", "");
                    wfArguments.Add("Customer", "");
                    wfArguments.Add("SessionType", TheType);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow("PoDataDeleteOBDN.xoml", "", wfArguments);

                    currentSession.SetInstance(instance);
                    currentSession.AddValue(Session.SessionKeys.DeliveryList, delDNList);
                    currentSession.AddValue("IsBatch", true);

                    if (!SessionManager.GetInstance.AddSession(currentSession))
                    {
                        currentSession.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");
                        erpara.Add(sessionKey);
                        ex = new FisException("CHK192", erpara);
                        throw ex;
                    }

                    currentSession.WorkflowInstance.Start();
                    currentSession.SetHostWaitOne();
                }
                else
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK193", erpara);
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

                return;
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
                logger.Debug("(_PoData)BatchDeleteOBDN end");
            }
        }

        /// <summary>
        /// 删除Shipment
        /// </summary>
        public int DeleteShipment(string ship, string editor)
        {
            logger.Debug("(_PoData)DeleteShipment start, ShipmentNo:" + ship);

            try
            {
                //DN已与unit绑定
                if (productRepository.GetCombinedQtyByShipmentNo(ship) != 0)
                {
                    return -1;
                }

                FisException ex;
                List<string> erpara = new List<string>();
                Session.SessionType TheType = Session.SessionType.Common;
                string sessionKey = ship;

                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, TheType);

                if (currentSession == null)
                {
                    currentSession = new Session(sessionKey, TheType, editor, "", "", "");

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", "");
                    wfArguments.Add("CurrentFlowSession", currentSession);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", "");
                    wfArguments.Add("Customer", "");
                    wfArguments.Add("SessionType", TheType);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow("PoDataDeletePLDN.xoml", "", wfArguments);

                    currentSession.SetInstance(instance);
                    currentSession.AddValue("ShipmentNo", ship);

                    if (!SessionManager.GetInstance.AddSession(currentSession))
                    {
                        currentSession.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");
                        erpara.Add(sessionKey);
                        ex = new FisException("CHK192", erpara);
                        throw ex;
                    }

                    currentSession.WorkflowInstance.Start();
                    currentSession.SetHostWaitOne();
                }
                else
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK193", erpara);
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

                return 0;
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
                logger.Debug("(_PoData)DeleteShipment end, ShipmentNo:" + ship);
            }
        }

        #endregion
    }
}
