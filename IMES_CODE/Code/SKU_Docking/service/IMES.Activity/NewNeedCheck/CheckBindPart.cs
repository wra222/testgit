using System;
using System.Collections.Generic;
using System.Workflow.ComponentModel;
using IMES.FisObject.Common.Part;
using IMES.FisObject.Common.Repair;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PCA.MB;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.Extend;

namespace IMES.Activity
{
    public partial class CheckBindPart : BaseActivity
	{
        /// <summary>
        /// 
        /// </summary>
		public CheckBindPart()
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
            try
            {                
                IPartRepository partRep = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
                IProduct product = (IProduct)CurrentSession.GetValue(Session.SessionKeys.Product);
                //Input defect data
                RepairDefect defect = (RepairDefect)CurrentSession.GetValue(Session.SessionKeys.CurrentRepairdefect);

                IList<IProductPart> productParts = product.ProductParts;               
                CurrentSession.AddValue(ExtendSession.SessionKeys.ProductPart, productParts);
                CurrentSession.AddValue(ExtendSession.SessionKeys.ReleaseMB, false);
                CurrentSession.AddValue(ExtendSession.SessionKeys.ReleasePart, false);
                CurrentSession.AddValue(ExtendSession.SessionKeys.RepairPartType,"");
                //no change parts case then don't check
                if (defect.OldPartSno.Trim() == "") 
                    return base.DoExecute(executionContext);                
                
                // check rule: 1. check MB 2. product parts
                if (defect.PartType == "MB")
                {
                    if (defect.OldPartSno.Trim() != "" &&
                        product.PCBID.Trim() == defect.OldPartSno.Trim())
                    {
                        CurrentSession.AddValue(ExtendSession.SessionKeys.ReleaseMB, true);
                        CurrentSession.AddValue(ExtendSession.SessionKeys.RepairPartType, "MB");
                        CurrentSession.AddValue("MBorVGA", "MB");
                    }
                    else
                    {
                        bool bFind = false;
                        foreach(IProductPart temp in productParts)
                        {
                            if(temp.PartSn == defect.OldPartSno.Trim() && 
                                temp.CheckItemType == "VGA")
                            {
                                bFind = true;
                                CurrentSession.AddValue(ExtendSession.SessionKeys.ReleaseMB, true);
                                CurrentSession.AddValue(ExtendSession.SessionKeys.RepairPartType, "MB");
                                CurrentSession.AddValue("MBorVGA", "VGA");
                                break;
                            }
                        }
                        if(bFind == false)
                        {
                            List<string> errpara = new List<string>();
                            FisException e = new FisException("CHK828", errpara);
                            e.stopWF = false;
                            throw e;
                        }
                    }

                    //else if (defect.OldPartSno.Trim() != "" &&
                    //    product.PCBID.Trim() != defect.OldPartSno.Trim())
                    //{
                    //    List<string> errpara = new List<string>();
                    //    FisException e = new FisException("CHK828", errpara);
                    //    e.stopWF = false;
                    //    throw e;
                    //}
                }
                else  // product part checking
                {
                    bool bChecked = false;
                    foreach (ProductPart part in productParts)
                    {
                        if (defect.OldPartSno.Trim() != "" && !String.IsNullOrEmpty(part.PartSn) && part.PartSn.Trim() == defect.OldPartSno.Trim())
                        {
                            CurrentSession.AddValue(ExtendSession.SessionKeys.RepairPartType, defect.PartType);
                            CurrentSession.AddValue(ExtendSession.SessionKeys.ReleasePart, true);
                            bChecked = true;
                            break;
                        }                        
                    }
                    if (bChecked == false)
                    {
                        List<string> errpara = new List<string>();
                        FisException e = new FisException("CHK829", errpara);
                        e.stopWF = false;
                        throw e;
                    }
                }
                return base.DoExecute(executionContext);
            }
            catch (FisException ex)
            {
                throw ex;
            }
        }
	}
}
