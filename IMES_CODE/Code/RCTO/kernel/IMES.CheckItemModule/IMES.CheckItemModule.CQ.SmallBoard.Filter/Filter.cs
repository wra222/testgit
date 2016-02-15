using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using IMES.CheckItemModule.Interface;
using IMES.CheckItemModule.Utility;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;
using IMES.Infrastructure;
using IMES.DataModel;
using IMES.FisObject.FA.Product;
using IMES.FisObject.Common.Model;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Text.RegularExpressions;
using IMES.FisObject.PAK.DN;
using IMES.FisObject.PCA.MB;
using IMES.Infrastructure.Util;
using IMES.Resolve.Common;

namespace IMES.CheckItemModule.CQ.SmallBoard.Filter
{
    [Export(typeof(IFilterModuleEx))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.CQ.SmallBoard.Filter.dll")]
    public class Filter : IFilterModuleEx
    {
        private const string differentQtyError = "Qty is not same in all parts {0}";       
        private const string ConstDescr = "Descr";       

        public object FilterBOMEx(object hierarchical_bom, string station, string checkItemType, object main_object)
        {
            IFlatBOM ret = null;
            IList<IFlatBOMItem> flat_bom_items = new List<IFlatBOMItem>();
            //var kp_parts = new List<IPart>();
            if (hierarchical_bom == null)
            {
                throw new ArgumentNullException();
            }
            var bom = (HierarchicalBOM)hierarchical_bom;

            UtilityCommonImpl utl = UtilityCommonImpl.GetInstance();
            IList<CheckItemTypeRuleDef> filteredChkItemRules = new List<CheckItemTypeRuleDef>();
            //IList<CheckItemTypeRuleDef> lstChkItemRule = utl.GetCheckItemTypeRule(main_object, checkItemType, station);
            IList<CheckItemTypeRuleDef> lstChkItemRule = utl.GetCheckItemTypeRuleByRegex(main_object, checkItemType, station);
            if (null == lstChkItemRule || lstChkItemRule.Count == 0)
            {
                // 未維護CheckItemTypeRule，請先維護!
                throw new FisException("CHK1151", new string[] { checkItemType });
                //return ret;
            }

            var noDataItemList = lstChkItemRule.Where(x => string.IsNullOrEmpty(x.BomNodeType) ||
                                                           string.IsNullOrEmpty(x.PartDescr) ||
                                                           string.IsNullOrEmpty(x.MatchRule)).ToList();
            if (noDataItemList.Count > 0)
            {
                throw new FisException("ICT027", new string[] { string.Join(GlobalConstName.CommaStr, noDataItemList.Select(x=>x.ID.ToString()).ToArray()), 
                                                                                        "BomNodeType,PartDescr, MatchRule"});
            }

            IList<IBOMNode> mbBomList = ResolveValue.GetBom(bom, GlobalConstName.BomNodeType.MB, null, null, null);
            IList<string> mbCodeList = new List<string>();
            if (mbBomList.Count > 0)
            {
                foreach(IBOMNode node in mbBomList)
                {
                    string value = node.Part.Attributes.Where(x => x.InfoType == GlobalConstName.PartInfo.MBCode && 
                                                                            !string.IsNullOrEmpty(x.InfoValue)).Select(x=>x.InfoValue).FirstOrDefault();
                    if (!string.IsNullOrEmpty(value))
                    {
                        mbCodeList.Add(value);
                    }
                }
            }
            
            foreach(CheckItemTypeRuleDef checkItem in lstChkItemRule)
            {
                IList<IBOMNode> bomNodeList= ResolveValue.GetBom(bom, checkItem.BomNodeType, checkItem.PartType, null, null);
                if (bomNodeList.Count > 0)
                {
                    var matchNodeList = bomNodeList.Where(x => x.Part.Attributes.Any(y => y.InfoType == ConstDescr && Regex.IsMatch(y.InfoValue, checkItem.PartDescr))).ToList();
                    if (matchNodeList.Count > 0)
                    {
                        IList<int> matchQtyList= matchNodeList.Select(x => x.Qty).Distinct().ToList();
                        if (matchQtyList.Count > 1)
                        {
                            throw new Exception(string.Format(differentQtyError, string.Join(GlobalConstName.CommaStr, matchNodeList.Select(x => x.PartNo).ToArray())));
                        }
                        IList<IPart> alterPartList = matchNodeList.Select(x => x.Part).ToList();
                        IFlatBOMItem kp_flat_bom_item = new FlatBOMItem(matchNodeList[0].Qty, checkItemType, alterPartList);
                        kp_flat_bom_item.PartNoItem = string.Join(GlobalConstName.CommaStr, alterPartList.Select(x=>x.PartNo).ToArray()) ;
                        kp_flat_bom_item.Descr = checkItem.PartDescr;
                        kp_flat_bom_item.CheckItemTypeRuleList = new List<CheckItemTypeRuleDef> { checkItem };
                        kp_flat_bom_item.Tag = mbCodeList;
                        flat_bom_items.Add(kp_flat_bom_item);
                    }
                }
           }
            
            if (flat_bom_items.Count > 0)
            {
                ret = new FlatBOM(flat_bom_items);
            }          

            return ret;
        }        
    }   
}
