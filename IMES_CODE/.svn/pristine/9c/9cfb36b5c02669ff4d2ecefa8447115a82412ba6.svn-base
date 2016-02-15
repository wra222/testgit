// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-12-19   210003                       Create
// 2012-02-28   210003                       ITC-1360-0460
// 2012-03-06   210003                       ITC-1360-1109
// 2012-03-06   210003                       ITC-1360-0455
// 2012-03-13   210003                       UC Checnged
// Known issues:
//
using System.Linq;
using System.ComponentModel.Composition;
using System.Management.Instrumentation;
using IMES.CheckItemModule.Interface;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Process;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PCA.MB;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.Model;
using IMES.DataModel;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using IMES.FisObject.Common.Part;
namespace IMES.CheckItemModule.TabletPL.Filter
{
    [Export(typeof(ICheckModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.TabletPL.Filter.dll")]
    class CheckModule : ICheckModule
    {
      
        private const string part_check_type = "TabletPL";
   
        public void Check(object partUnit, object bomItem, string station)
        {
            if (partUnit != null)
            {
                CheckRegxMatch(partUnit);
                if (HaveVendorCode(bomItem) && NeedUniqueCheck(partUnit))
                {
                   CheckHaveProductPart((PartUnit)partUnit);
                }

            }
            else
            {
                throw new FisException("CHK174", new string[] { "IMES.CheckItemModule.TabletPL.Filter.CheckModule.Check" });

            }
        }
        private bool NeedUniqueCheck(object partUnit)
        {
            Session currentSession = (Session)((PartUnit)partUnit).CurrentSession;
            string saveRule=((string)currentSession.GetValue(part_check_type + "SaveRule" + ((PartUnit)partUnit).ValueType)).Trim();
            return !string.Equals(saveRule, "0");
        
        }
        private void CheckRegxMatch(object partUnit)
        {

            //if (currentSession == null)
            //{
            //    currentSession = (Session)((PartUnit)partUnit).CurrentSession;
            //}
            Session currentSession = (Session)((PartUnit)partUnit).CurrentSession;
            if (currentSession == null)
            {
                throw new InstanceNotFoundException("Can not get Session instance from SessionManager!");
            }
           
            string partSn = ((PartUnit)partUnit).Sn.Trim();
            string regx = (string)currentSession.GetValue(part_check_type + "Regx" + ((PartUnit)partUnit).ValueType);
         
            if (string.IsNullOrEmpty(regx))
            { return; }
            string[] regxArr = regx.Split(',');
            foreach (string pattern in regxArr)
            {
                Regex Regex1 = new Regex(pattern);
                if (Regex1.IsMatch(partSn))
                {
                    return;
                }
            }
            List<string> errpara = new List<string>();
            errpara.Add(partSn);
            FisException e = new FisException("MAT010", errpara);
            throw e;
        }
        private void CheckHaveProductPart(PartUnit pu)
        {
            IProductRepository productRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IMES.FisObject.FA.Product.IProduct>();
            IList<IMES.FisObject.Common.Part.IProductPart> lstPart =
                productRep.GetProductPartsByValue(pu.Sn).Where(x => x.PartType == pu.ValueType).ToList();
            if (lstPart.Count > 0)
            {
                List<string> erpara = new List<string>();
                erpara.Add("Part");
                erpara.Add(pu.Sn);
                erpara.Add(lstPart[0].ProductID);
                var ex = new FisException("CHK009", erpara);
                throw ex;

            }

        }

        private bool HaveVendorCode(object bom_item)
        {

            IList<IPart> flat_bom_items = ((IFlatBOMItem)bom_item).AlterParts;
            if (flat_bom_items != null)
            {
                foreach (IPart flat_bom_item in flat_bom_items)
                {
                    IList<PartInfo> part_infos = flat_bom_item.Attributes;
                    foreach (PartInfo part_info in part_infos)
                    {
                        if (part_info.InfoType.Equals("VendorCode"))
                        {
                            return true;
                        }
                    }

                }

            }
            return false;


        }

       
    }
}
