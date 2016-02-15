using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.Repository._Metas;
using mtns = IMES.Infrastructure.Repository._Metas;

namespace IMES.DataModel
{
    [Serializable]
    [ORMapping(typeof(Pak_Pakcomn))]
    public class PakDashPakComnInfo
    {
        [ORMapping(Pak_Pakcomn.fn_actual_shipdate)]
        public String actual_shipdate = null;
        [ORMapping(Pak_Pakcomn.fn_area_group_id)]
        public String area_group_id = null;
        [ORMapping(Pak_Pakcomn.fn_box_unit_qty)]
        public String box_unit_qty = null;
        [ORMapping(Pak_Pakcomn.fn_c12)]
        public String c12 = null;
        [ORMapping(Pak_Pakcomn.fn_c121)]
        public String c121 = null;
        [ORMapping(Pak_Pakcomn.fn_c124)]
        public String c124 = null;
        [ORMapping(Pak_Pakcomn.fn_c135)]
        public String c135 = null;
        [ORMapping(Pak_Pakcomn.fn_c146)]
        public String c146 = null;
        [ORMapping(Pak_Pakcomn.fn_c22)]
        public String c22 = null;
        [ORMapping(Pak_Pakcomn.fn_c23)]
        public String c23 = null;
        [ORMapping(Pak_Pakcomn.fn_c24)]
        public String c24 = null;
        [ORMapping(Pak_Pakcomn.fn_c25)]
        public String c25 = null;
        [ORMapping(Pak_Pakcomn.fn_c26)]
        public String c26 = null;
        [ORMapping(Pak_Pakcomn.fn_c28)]
        public String c28 = null;
        [ORMapping(Pak_Pakcomn.fn_c29)]
        public String c29 = null;
        [ORMapping(Pak_Pakcomn.fn_c30)]
        public String c30 = null;
        [ORMapping(Pak_Pakcomn.fn_c36)]
        public String c36 = null;
        [ORMapping(Pak_Pakcomn.fn_c40)]
        public String c40 = null;
        [ORMapping(Pak_Pakcomn.fn_c43)]
        public String c43 = null;
        [ORMapping(Pak_Pakcomn.fn_c46)]
        public String c46 = null;
        [ORMapping(Pak_Pakcomn.fn_c47)]
        public String c47 = null;
        [ORMapping(Pak_Pakcomn.fn_c48)]
        public String c48 = null;
        [ORMapping(Pak_Pakcomn.fn_c49)]
        public String c49 = null;
        [ORMapping(Pak_Pakcomn.fn_c50)]
        public String c50 = null;
        [ORMapping(Pak_Pakcomn.fn_c51)]
        public String c51 = null;
        [ORMapping(Pak_Pakcomn.fn_c52)]
        public String c52 = null;
        [ORMapping(Pak_Pakcomn.fn_c53)]
        public String c53 = null;
        [ORMapping(Pak_Pakcomn.fn_c54)]
        public String c54 = null;
        [ORMapping(Pak_Pakcomn.fn_c56)]
        public String c56 = null;
        [ORMapping(Pak_Pakcomn.fn_c57)]
        public String c57 = null;
        [ORMapping(Pak_Pakcomn.fn_c6)]
        public String c6 = null;
        [ORMapping(Pak_Pakcomn.fn_c60)]
        public String c60 = null;
        [ORMapping(Pak_Pakcomn.fn_c61)]
        public String c61 = null;
        [ORMapping(Pak_Pakcomn.fn_c62)]
        public String c62 = null;
        [ORMapping(Pak_Pakcomn.fn_c63)]
        public String c63 = null;
        [ORMapping(Pak_Pakcomn.fn_c64)]
        public String c64 = null;
        [ORMapping(Pak_Pakcomn.fn_c69)]
        public String c69 = null;
        [ORMapping(Pak_Pakcomn.fn_c74)]
        public String c74 = null;
        [ORMapping(Pak_Pakcomn.fn_c93)]
        public String c93 = null;
        [ORMapping(Pak_Pakcomn.fn_c99)]
        public String c99 = null;
        [ORMapping(Pak_Pakcomn.fn_carton_qty)]
        public decimal carton_qty = default(decimal);
        [ORMapping(Pak_Pakcomn.fn_cond_priority)]
        public decimal cond_priority = default(decimal);
        [ORMapping(Pak_Pakcomn.fn_consol_invoice)]
        public String consol_invoice = null;
        [ORMapping(Pak_Pakcomn.fn_cust_ord_ref)]
        public String cust_ord_ref = null;
        [ORMapping(Pak_Pakcomn.fn_cust_po)]
        public String cust_po = null;
        [ORMapping(Pak_Pakcomn.fn_cust_so_num)]
        public String cust_so_num = null;
        [ORMapping(Pak_Pakcomn.fn_customer_id)]
        public String customer_id = null;
        [ORMapping(Pak_Pakcomn.fn_dest_code)]
        public String dest_code = null;
        [ORMapping(Pak_Pakcomn.fn_doc_set_number)]
        public String doc_set_number = null;
        [ORMapping(Pak_Pakcomn.fn_duty_code)]
        public String duty_code = null;
        [ORMapping(Pak_Pakcomn.fn_edi_distrib_channel)]
        public String edi_distrib_channel = null;
        [ORMapping(Pak_Pakcomn.fn_edi_intl_carrier)]
        public String edi_intl_carrier = null;
        [ORMapping(Pak_Pakcomn.fn_edi_mfg_code)]
        public String edi_mfg_code = null;
        [ORMapping(Pak_Pakcomn.fn_edi_phc)]
        public String edi_phc = null;
        [ORMapping(Pak_Pakcomn.fn_edi_pl_code)]
        public String edi_pl_code = null;
        [ORMapping(Pak_Pakcomn.fn_edi_trans_serv_level)]
        public String edi_trans_serv_level = null;
        [ORMapping(Pak_Pakcomn.fn_hp_cust_pn)]
        public String hp_cust_pn = null;
        [ORMapping(Pak_Pakcomn.fn_hp_except)]
        public String hp_except = null;
        [ORMapping(Pak_Pakcomn.fn_hp_pn)]
        public String hp_pn = null;
        [ORMapping(Pak_Pakcomn.fn_hp_so)]
        public String hp_so = null;
        [ORMapping(Pak_Pakcomn.fn_id)]
        public Int32 id = int.MinValue;
        [ORMapping(Pak_Pakcomn.fn_india_generic_desc)]
        public String india_generic_desc = null;
        [ORMapping(Pak_Pakcomn.fn_india_price)]
        public String india_price = null;
        [ORMapping(Pak_Pakcomn.fn_india_price_id)]
        public String india_price_id = null;
        [ORMapping(Pak_Pakcomn.fn_instr_flag)]
        public String instr_flag = null;
        [ORMapping(Pak_Pakcomn.fn_internalID)]
        public String internalID = null;
        [ORMapping(Pak_Pakcomn.fn_intl_carrier)]
        public String intl_carrier = null;
        [ORMapping(Pak_Pakcomn.fn_language)]
        public String language = null;
        [ORMapping(Pak_Pakcomn.fn_mand_carrier_id)]
        public String mand_carrier_id = null;
        [ORMapping(Pak_Pakcomn.fn_master_waybill_number)]
        public String master_waybill_number = null;
        [ORMapping(Pak_Pakcomn.fn_model)]
        public String model = null;
        [ORMapping(Pak_Pakcomn.fn_multi_line_id)]
        public String multi_line_id = null;
        [ORMapping(Pak_Pakcomn.fn_order_date_hp_po)]
        public DateTime order_date_hp_po = DateTime.MinValue;
        [ORMapping(Pak_Pakcomn.fn_order_type)]
        public String order_type = null;
        [ORMapping(Pak_Pakcomn.fn_pack_id)]
        public String pack_id = null;
        [ORMapping(Pak_Pakcomn.fn_pack_id_cons)]
        public String pack_id_cons = null;
        [ORMapping(Pak_Pakcomn.fn_pack_id_line_item_box_max_unit_qty)]
        public String pack_id_line_item_box_max_unit_qty = null;
        [ORMapping(Pak_Pakcomn.fn_pack_id_line_item_unit_qty)]
        public decimal pack_id_line_item_unit_qty = default(decimal);
        [ORMapping(Pak_Pakcomn.fn_pack_id_unit_qty)]
        public String pack_id_unit_qty = null;
        [ORMapping(Pak_Pakcomn.fn_pack_id_unit_uom)]
        public String pack_id_unit_uom = null;
        [ORMapping(Pak_Pakcomn.fn_packing_lv)]
        public String packing_lv = null;
        [ORMapping(Pak_Pakcomn.fn_packing_type)]
        public String packing_type = null;
        [ORMapping(Pak_Pakcomn.fn_pak_type)]
        public String pak_type = null;
        [ORMapping(Pak_Pakcomn.fn_pallet_qty)]
        public decimal pallet_qty = default(decimal);
        [ORMapping(Pak_Pakcomn.fn_phys_consol)]
        public String phys_consol = null;
        [ORMapping(Pak_Pakcomn.fn_po_num)]
        public String po_num = null;
        [ORMapping(Pak_Pakcomn.fn_pref_gateway)]
        public String pref_gateway = null;
        [ORMapping(Pak_Pakcomn.fn_reg_carrier)]
        public String reg_carrier = null;
        [ORMapping(Pak_Pakcomn.fn_region)]
        public String region = null;
        [ORMapping(Pak_Pakcomn.fn_reseller_name)]
        public String reseller_name = null;
        [ORMapping(Pak_Pakcomn.fn_return_to_city)]
        public String return_to_city = null;
        [ORMapping(Pak_Pakcomn.fn_return_to_contact)]
        public String return_to_contact = null;
        [ORMapping(Pak_Pakcomn.fn_return_to_country_code)]
        public String return_to_country_code = null;
        [ORMapping(Pak_Pakcomn.fn_return_to_country_name)]
        public String return_to_country_name = null;
        [ORMapping(Pak_Pakcomn.fn_return_to_id)]
        public String return_to_id = null;
        [ORMapping(Pak_Pakcomn.fn_return_to_name)]
        public String return_to_name = null;
        [ORMapping(Pak_Pakcomn.fn_return_to_name_2)]
        public String return_to_name_2 = null;
        [ORMapping(Pak_Pakcomn.fn_return_to_name_3)]
        public String return_to_name_3 = null;
        [ORMapping(Pak_Pakcomn.fn_return_to_state)]
        public String return_to_state = null;
        [ORMapping(Pak_Pakcomn.fn_return_to_street)]
        public String return_to_street = null;
        [ORMapping(Pak_Pakcomn.fn_return_to_street_2)]
        public String return_to_street_2 = null;
        [ORMapping(Pak_Pakcomn.fn_return_to_telephone)]
        public String return_to_telephone = null;
        [ORMapping(Pak_Pakcomn.fn_return_to_zip)]
        public String return_to_zip = null;
        [ORMapping(Pak_Pakcomn.fn_sales_chan)]
        public String sales_chan = null;
        [ORMapping(Pak_Pakcomn.fn_ship_cat_type)]
        public String ship_cat_type = null;
        [ORMapping(Pak_Pakcomn.fn_ship_from_city)]
        public String ship_from_city = null;
        [ORMapping(Pak_Pakcomn.fn_ship_from_contact)]
        public String ship_from_contact = null;
        [ORMapping(Pak_Pakcomn.fn_ship_from_country_code)]
        public String ship_from_country_code = null;
        [ORMapping(Pak_Pakcomn.fn_ship_from_country_name)]
        public String ship_from_country_name = null;
        [ORMapping(Pak_Pakcomn.fn_ship_from_id)]
        public String ship_from_id = null;
        [ORMapping(Pak_Pakcomn.fn_ship_from_name)]
        public String ship_from_name = null;
        [ORMapping(Pak_Pakcomn.fn_ship_from_name_2)]
        public String ship_from_name_2 = null;
        [ORMapping(Pak_Pakcomn.fn_ship_from_name_3)]
        public String ship_from_name_3 = null;
        [ORMapping(Pak_Pakcomn.fn_ship_from_state)]
        public String ship_from_state = null;
        [ORMapping(Pak_Pakcomn.fn_ship_from_street)]
        public String ship_from_street = null;
        [ORMapping(Pak_Pakcomn.fn_ship_from_street_2)]
        public String ship_from_street_2 = null;
        [ORMapping(Pak_Pakcomn.fn_ship_from_telephone)]
        public String ship_from_telephone = null;
        [ORMapping(Pak_Pakcomn.fn_ship_from_zip)]
        public String ship_from_zip = null;
        [ORMapping(Pak_Pakcomn.fn_ship_mode)]
        public String ship_mode = null;
        [ORMapping(Pak_Pakcomn.fn_ship_ref)]
        public String ship_ref = null;
        [ORMapping(Pak_Pakcomn.fn_ship_to_city)]
        public String ship_to_city = null;
        [ORMapping(Pak_Pakcomn.fn_ship_to_contact)]
        public String ship_to_contact = null;
        [ORMapping(Pak_Pakcomn.fn_ship_to_country_code)]
        public String ship_to_country_code = null;
        [ORMapping(Pak_Pakcomn.fn_ship_to_country_name)]
        public String ship_to_country_name = null;
        [ORMapping(Pak_Pakcomn.fn_ship_to_id)]
        public String ship_to_id = null;
        [ORMapping(Pak_Pakcomn.fn_ship_to_name)]
        public String ship_to_name = null;
        [ORMapping(Pak_Pakcomn.fn_ship_to_name_2)]
        public String ship_to_name_2 = null;
        [ORMapping(Pak_Pakcomn.fn_ship_to_name_3)]
        public String ship_to_name_3 = null;
        [ORMapping(Pak_Pakcomn.fn_ship_to_state)]
        public String ship_to_state = null;
        [ORMapping(Pak_Pakcomn.fn_ship_to_street)]
        public String ship_to_street = null;
        [ORMapping(Pak_Pakcomn.fn_ship_to_street_2)]
        public String ship_to_street_2 = null;
        [ORMapping(Pak_Pakcomn.fn_ship_to_telephone)]
        public String ship_to_telephone = null;
        [ORMapping(Pak_Pakcomn.fn_ship_to_zip)]
        public String ship_to_zip = null;
        [ORMapping(Pak_Pakcomn.fn_ship_via_city)]
        public String ship_via_city = null;
        [ORMapping(Pak_Pakcomn.fn_ship_via_contact)]
        public String ship_via_contact = null;
        [ORMapping(Pak_Pakcomn.fn_ship_via_country_code)]
        public String ship_via_country_code = null;
        [ORMapping(Pak_Pakcomn.fn_ship_via_country_name)]
        public String ship_via_country_name = null;
        [ORMapping(Pak_Pakcomn.fn_ship_via_id)]
        public String ship_via_id = null;
        [ORMapping(Pak_Pakcomn.fn_ship_via_name)]
        public String ship_via_name = null;
        [ORMapping(Pak_Pakcomn.fn_ship_via_name_2)]
        public String ship_via_name_2 = null;
        [ORMapping(Pak_Pakcomn.fn_ship_via_name_3)]
        public String ship_via_name_3 = null;
        [ORMapping(Pak_Pakcomn.fn_ship_via_state)]
        public String ship_via_state = null;
        [ORMapping(Pak_Pakcomn.fn_ship_via_street)]
        public String ship_via_street = null;
        [ORMapping(Pak_Pakcomn.fn_ship_via_street_2)]
        public String ship_via_street_2 = null;
        [ORMapping(Pak_Pakcomn.fn_ship_via_telephone)]
        public String ship_via_telephone = null;
        [ORMapping(Pak_Pakcomn.fn_ship_via_zip)]
        public String ship_via_zip = null;
        [ORMapping(Pak_Pakcomn.fn_shipment)]
        public String shipment = null;
        [ORMapping(Pak_Pakcomn.fn_sold_to_contact)]
        public String sold_to_contact = null;
        [ORMapping(Pak_Pakcomn.fn_sub_region)]
        public String sub_region = null;
        [ORMapping(Pak_Pakcomn.fn_trans_serv_level)]
        public String trans_serv_level = null;
        [ORMapping(Pak_Pakcomn.fn_ucc_code)]
        public String ucc_code = null;
        [ORMapping(Pak_Pakcomn.fn_waybill_number)]
        public String waybill_number = null;
        [ORMapping(Pak_Pakcomn.fn_sold_to_city)]
        public String sold_to_city = null;
        [ORMapping(PakDotpakcomn.fn_sold_to_country_name)]
        public String sold_to_country_name = null;
        [ORMapping(Pak_Pakcomn.fn_sold_to_name2)]
        public String sold_to_name2 = null;
        [ORMapping(Pak_Pakcomn.fn_sold_to_name3)]
        public String sold_to_name3 = null;
        [ORMapping(Pak_Pakcomn.fn_sold_to_state)]
        public String sold_to_state = null;
        [ORMapping(Pak_Pakcomn.fn_sold_to_street)]
        public String sold_to_street = null;
        [ORMapping(Pak_Pakcomn.fn_sold_to_street2)]
        public String sold_to_street2 = null;
        [ORMapping(Pak_Pakcomn.fn_sold_to_zip)]
        public String sold_to_zip = null;

        [ORMapping(Pak_Pakcomn.fn_container_id)]
        public String container_id = null;
        [ORMapping(Pak_Pakcomn.fn_sold_to_name)]
        public String sold_to_name = null;

        [ORMapping(Pak_Pakcomn.fn_incoterm)]
        public String incoterm = null;
    }
}
