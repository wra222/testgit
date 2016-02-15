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
    public partial class CheckDeliveryPallet : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CheckDeliveryPallet()
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
                string palletNo = (string)CurrentSession.GetValue(Session.SessionKeys.PalletNo);
                if (!String.IsNullOrEmpty(palletNo))
                {
                    IDeliveryRepository DeliveryRepository = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
                    IList<DeliveryPalletInfo> list = DeliveryRepository.GetDeliveryPalletListByPlt(palletNo);
                    foreach(DeliveryPalletInfo temp in list)
                    {
                        if (temp.palletNo == palletNo)
                        {
                            List<string> errpara = new List<string>();
                            throw new FisException("CHK839", errpara);
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
