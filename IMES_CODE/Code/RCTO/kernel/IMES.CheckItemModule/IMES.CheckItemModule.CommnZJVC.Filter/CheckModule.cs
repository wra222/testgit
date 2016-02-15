using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using IMES.CheckItemModule.Interface;
using IMES.DataModel;
using IMES.FisObject.PAK.COA;
using IMES.FisObject.PAK.Pizza;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.FisBOM;
using IMES.Infrastructure;
using IMES.FisObject.Common.Part;
using IMES.FisObject.Common.Material;
using IMES.CheckItemModule.Utility;

namespace IMES.CheckItemModule.CommnZJVC.Filter
{
    [Export(typeof(ICheckModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.CommnZJVC.Filter.dll")]
    public class CheckModule : ICheckModule
    {
        public void Check(object partUnit, object bomItem, string station)
        {
             if (partUnit != null)
                {
                    string sn = ((PartUnit)partUnit).Sn;
                    if (!string.IsNullOrEmpty(sn))
                    {
                        IList<CheckItemTypeRuleDef> lstChkItemRule = ((FlatBOMItem)bomItem).CheckItemTypeRuleList;
                        CheckItemTypeRuleDef chkItemRule = lstChkItemRule[0];
                        int OverTestKPCountLimit=UtilityCommonImpl.GetInstance().GetTestKPCountLimit(((PartUnit)partUnit).ValueType,station,3); 
                        if (chkItemRule.CheckTestKPCount == "Y")
                        {
                            var materialRep = RepositoryFactory.GetInstance().GetRepository<IMaterialRepository>();
                            Material material = materialRep.Find(sn);
                            if (material == null)// not used
                            {
                                return;
                            }
                            else
                            {
                                if (Convert.ToInt32(material.QCStatus) >= OverTestKPCountLimit)//material used qty >maintain qty
                                {
                                    throw new FisException("CQCHK1111", new string[] { sn,material.QCStatus, OverTestKPCountLimit.ToString() });
                                }
                            }
                        }
                        else
                        {
                            return;
                        }
                    }
                }
            
        }
    }
}

