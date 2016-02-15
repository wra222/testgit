using System;
using System.Collections.Generic;
using System.Collections;
using System.Workflow.ComponentModel;
using IMES.FisObject.Common.Part;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.PAK.DN;
using IMES.DataModel;

namespace IMES.Activity
{
    /// <summary>
    /// 
    /// </summary>
    public partial class CheckBOXAndUCC : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CheckBOXAndUCC()
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
                IProduct currentProduct = (IProduct)CurrentSession.GetValue(Session.SessionKeys.Product);
                if (currentProduct != null)
                {
                    if (currentProduct.PalletNo == "")
                    {
                        List<string> errpara = new List<string>();
                        throw new FisException("CHK841", errpara);
                    }
                    IDeliveryRepository DeliveryRepository = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
                    if (!String.IsNullOrEmpty(currentProduct.DeliveryNo))
                    {
                        Delivery CurrentDelivery = DeliveryRepository.Find(currentProduct.DeliveryNo);
                        if (CurrentDelivery == null)
                        {
                            List<string> errpara = new List<string>();
                            errpara.Add(currentProduct.DeliveryNo);
                            throw new FisException("CHK107", errpara);
                        }
                        CurrentSession.AddValue(Session.SessionKeys.Delivery, CurrentDelivery);
                        string dnFlag = (string)CurrentDelivery.GetExtendedProperty("Flag");
                        if (!String.IsNullOrEmpty(dnFlag) && dnFlag == "N")
                        {
                            string boxid = (string)currentProduct.GetExtendedProperty("BoxId");
                            string ucc = (string)currentProduct.GetExtendedProperty("UCC");
                            //if (String.IsNullOrEmpty(boxid) && String.IsNullOrEmpty(ucc))
                            if (boxid == null && ucc == null)
                            {
                                List<string> errpara = new List<string>();                                
                                throw new FisException("CHK841", errpara);
                            }
                        }
                    }
                }

                return base.DoExecute(executionContext);
            }
            catch (FisException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
