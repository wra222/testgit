﻿// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-12-19   210003                       Create
// 2011-12-19   210003                       ITC-1360-1193
// Known issues:

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using IMES.Infrastructure;
using IMES.CheckItemModule.Interface;
using IMES.FisObject.Common.Model;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.DataModel;

namespace IMES.CheckItemModule.W8SPS.Filter
{
    [Export(typeof(IFilterModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.W8SPS.Filter.dll")]
    public class Filter : IFilterModule
    {
        private int _qty;
        private const string part_check_type = "W8SPS";

        public object FilterBOM(object hierarchical_bom, string station, object main_object)
        {
            #region check input parameter
            //main_object 需要是IProduct
            IProduct prod = (IProduct)main_object;
            if (prod == null)
            {
                throw new FisException("FIL001", new string[] { part_check_type,"Product" });
            }
            if (hierarchical_bom == null )
            {
                throw new FisException("FIL001", new string[] { part_check_type, "BOM" });
            }
            var bom = (HierarchicalBOM)hierarchical_bom;

            if (bom.FirstLevelNodes == null || bom.FirstLevelNodes.Count==0)
            {
                throw new FisException("FIL001", new string[] { part_check_type, "BOM" });
            }

            #endregion

            IOSWINRepository osRep = RepositoryFactory.GetInstance().GetRepository<IOSWINRepository, OSWIN>();
            IPartRepository partRep = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();

           

            //根据Model展1阶，得到第一阶是MB的part [BomNodeType=MB]的MBCode[PartInfo.InfoValue(InfoType='MB')]，
            #region Check Bom structure & Filter Part
 
            OSWIN osWin = osRep.GetOSWINByModelBOM(prod.Model);
            if (osWin == null ||
                 string.IsNullOrEmpty(osWin.OS) ||
                 osWin.OS.IndexOf("W8")<0||
                 !osWin.Family.Equals(prod.Family))
            {
                return null;
            }

            IFlatBOM ret = null;
            String mb_info_value_string = "";
            var parts = new List<IPart>();
            parts.Add(partRep.Find(osWin.BomZmode));
            _qty = 1;
            
            IList<ConstValueInfo> valueInfos = partRep.GetConstValueListByType(part_check_type);
            var postFixValues = (from item in valueInfos
                                where item.name.Trim() == osWin.OS.Trim()
                                select item).ToList();
             string postFix="";
            if (postFixValues==null || postFixValues.Count()==0)
            {
                postFix = "";
            }
            else
            {
                postFix = postFixValues[0].value.Trim();
                if (!postFix.StartsWith("-"))
                {
                    postFix = "-" + postFix;
                }
            }

            var matchMBBom = (from item in bom.FirstLevelNodes
                              where ((BOMNode)item).Part.BOMNodeType.Equals("MB") && this.CheckCondition(item)
                              select ((BOMNode)item).Part).ToList();

            foreach (IPart item in matchMBBom)
            {
                IList<PartInfo> part_infos = item.Attributes;

                var partInfos = (from p in part_infos
                                 where p.InfoType == "FRUNO"
                                 select p).ToList();
                if (partInfos == null ||
                     part_infos.Count == 0 ||
                     string.IsNullOrEmpty(partInfos[0].InfoValue))
                {
                    throw new FisException("FIL002", new string[] { part_check_type, "BOM", item.PN, "FRUNO" });
                }

                string spsNo = partInfos[0].InfoValue;
                if (!string.IsNullOrEmpty(postFix))
                {
                    string[] value = partInfos[0].InfoValue.Split(new char[] { '-' });
                    string preFix = value[0].Trim();
                    if (preFix.Length != 6)
                    {
                        throw new FisException("FIL003", new string[] { part_check_type, "BOM", item.PN, "FRUNO", partInfos[0].InfoValue });
                    }
                    spsNo = preFix + postFix;

                }

                if (mb_info_value_string.Length == 0)
                {
                    mb_info_value_string = spsNo;
                }
                else
                {
                    if (!mb_info_value_string.Contains(spsNo))
                    {
                        mb_info_value_string += "," + spsNo;
                    }
                }
            }
            #endregion

            #region Generate FlatBom with filter Part for return
            if (parts.Count > 0)
            {
                if (bom.FirstLevelNodes != null && bom.FirstLevelNodes.Count > 0 && !string.IsNullOrEmpty(mb_info_value_string))
                {
                    var flat_bom_item = new FlatBOMItem(_qty, part_check_type, parts);
                    flat_bom_item.Descr = osWin.OS;
                    flat_bom_item.PartNoItem = mb_info_value_string;
                    IList<IFlatBOMItem> flat_bom_items = new List<IFlatBOMItem>();
                    flat_bom_items.Add(flat_bom_item);
                    ret = new FlatBOM(flat_bom_items);
                }
            }
           #endregion

            return ret;
        }
        public bool CheckCondition(object node)
        {
            //MBCode[PartInfo.InfoValue(InfoType='MB')]，并且第一阶的PartInfo不存在InfoTyp='VGA' InfoValue='SV'
            if (((BOMNode)node).Part == null)
            {
                return false;
            }
            bool is_mb = false;
            bool is_vga_sv = false;
            IList<PartInfo> part_infos = ((BOMNode)node).Part.Attributes;
            foreach (PartInfo part_info in part_infos)
            {
                if (part_info.InfoType.Trim().Equals("MB") && !is_mb)
                {
                    if(!is_mb) is_mb = true;
                    continue;

                }
                if (part_info.InfoType.Trim().Equals("VGA") && part_info.InfoValue.Trim().Equals("SV"))
                {
                    if (!is_vga_sv) is_vga_sv = true;
                    continue;
                }
            }
            if (is_mb && !is_vga_sv)
                return true;
            return false;
        }
    }
}
