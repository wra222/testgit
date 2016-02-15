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
		public CheckBindPart()
		{
			InitializeComponent();
		}
        
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
                if (defect.OldPart.Trim() == "" && defect.OldPartSno.Trim() == "") return base.DoExecute(executionContext);

                if (defect.OldPartSno.Trim() == "")
                {
                    List<string> errpara = new List<string>();

                    errpara.Add(defect.OldPartSno);
                    // not allow serial number is null
                    FisException e = new FisException("MAT010", errpara);
                    e.stopWF = false;
                    throw e;
                }

                // UI protect defect.OldPartSn
                // check rule: 1. check MB 2. product parts
                if (defect.OldPart.Trim() != "" &&
                    product.PCBModel.Trim() == defect.OldPart.Trim() &&
                    product.PCBID.Trim() == defect.OldPartSno.Trim())
                {
                    
                    CurrentSession.AddValue(ExtendSession.SessionKeys.ReleaseMB, true);
                    CurrentSession.AddValue(ExtendSession.SessionKeys.RepairPartType, "MB");
                    return base.DoExecute(executionContext);
                }                
                else if (defect.OldPart.Trim() != "" &&
                    product.PCBModel.Trim() != defect.OldPart.Trim() &&
                    product.PCBID.Trim() == defect.OldPartSno.Trim())
                {
                    List<string> errpara = new List<string>();

                    errpara.Add(defect.OldPartSno);
                    // not match
                    FisException e = new FisException("MAT010", errpara);
                    e.stopWF = false;
                    throw e;
                }
                else if (defect.OldPart.Trim() != "" &&
                    product.PCBModel.Trim() == defect.OldPart.Trim() &&
                    product.PCBID.Trim() != defect.OldPartSno.Trim())
                {
                    List<string> errpara = new List<string>();

                    errpara.Add(defect.OldPartSno);
                    // not match
                    FisException e = new FisException("MAT010", errpara);
                    e.stopWF = false;
                    throw e;
                }
                else if (defect.OldPart.Trim() == "" && 
                         product.PCBID.Trim() == defect.OldPartSno.Trim())
                {
                    CurrentSession.AddValue(ExtendSession.SessionKeys.ReleaseMB, true);
                    CurrentSession.AddValue(ExtendSession.SessionKeys.RepairPartType, "MB");
                    return base.DoExecute(executionContext);
                }
                else  // product part checking
                {
                    foreach (ProductPart part in productParts)
                    {
                        if (defect.OldPart.Trim() != "" &&
                                part.PartID.Trim() == defect.OldPart.Trim() &&
                                part.Value.Trim() == defect.OldPartSno.Trim())
                        {
                            CurrentSession.AddValue(ExtendSession.SessionKeys.RepairPartType, part.PartType);
                            CurrentSession.AddValue(ExtendSession.SessionKeys.ReleasePart, true);
                            return base.DoExecute(executionContext);
                        }
                        else if (defect.OldPart.Trim() != "" &&
                                part.PartID.Trim() != defect.OldPart.Trim() &&
                                part.Value.Trim() == defect.OldPartSno.Trim())
                        {
                            List<string> errpara = new List<string>();

                            errpara.Add(defect.OldPartSno);
                            // not match
                            FisException e = new FisException("MAT010", errpara);
                            e.stopWF = false;
                            throw e;
                        }
                        else if (defect.OldPart.Trim() != "" &&
                                part.PartID.Trim() == defect.OldPart.Trim() &&
                                part.Value.Trim() != defect.OldPartSno.Trim())
                        {
                            List<string> errpara = new List<string>();

                            errpara.Add(defect.OldPartSno);
                            // not match
                            FisException e = new FisException("MAT010", errpara);
                            e.stopWF = false;
                            throw e;
                        }
                        else if (defect.OldPart.Trim() == "" &&
                                part.Value.Trim() == defect.OldPartSno.Trim())
                        {
                            CurrentSession.AddValue(ExtendSession.SessionKeys.RepairPartType, part.PartType);
                            CurrentSession.AddValue(ExtendSession.SessionKeys.ReleasePart, true);
                            return base.DoExecute(executionContext);
                        }
                    }
                }

                
                List<string> errpara1 = new List<string>();
                errpara1.Add(defect.OldPart);
                FisException e1 = new FisException("CHK027", errpara1);
                e1.stopWF = false;
                throw e1;

                //throw new FisException("CHK027", errpara);
                //return base.DoExecute(executionContext);
            }
            catch (FisException ex)
            {
                throw;
            }
        }
	}
}
