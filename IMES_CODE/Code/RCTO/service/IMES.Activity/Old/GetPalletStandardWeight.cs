// INVENTEC corporation (c)2010 all rights reserved. 
// Description: Pallet称重时，获取标准重量和误差
// 将获取的StandardWeight，Tolerance放到session中                       
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2010-04-08   Yuan XiaoWei                 create
// 2010-05-30   Yuan XiaoWei                 ITC-1155-0133
// 2010-06-04   Yuan XiaoWei                 ITC-1155-0164
// Known issues:
using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Drawing;
using System.Linq;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;
using IMES.FisObject.PAK.Pallet;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.Generic;
using IMES.FisObject.PAK.DN;

namespace IMES.Activity
{
    /// <summary>
    /// 获取Pallet的标准重量
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      应用于PalletWeight
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.根据Pallet的Family获取其中Model的Region
    ///         2.根据Pallet获取Pallet上绑定的Product数量;
    ///         3.根据Family，Region，Pallet上绑定的Product数量获取维护的PalletWeight;
    ///         4.如果没有获取到，表示不需要进行Pallet的重量检查，将Session的NeedCheckPalletWeight设置为False
    ///         5.如果有将Session的NeedCheckPalletWeight设置为True
    ///         6.将包材重量+Pallet上所有Carton的重量 作为标准重量 
    ///         7.将获取的StandardWeight，Tolerance放到session中 
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.Product
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         无
    /// </para> 
    ///<para> 
    /// 输出：
    ///         无
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///         无
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         IModelWeightRepository
    ///         ModelWeight
    ///         Product
    /// </para> 
    /// </remarks>
    public partial class GetPalletStandardWeight : BaseActivity
    {
        /// <summary>
        /// 
        /// </summary>
        public GetPalletStandardWeight()
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
            Pallet CurrentPallet = (Pallet)CurrentSession.GetValue(Session.SessionKeys.Pallet);
            IPalletWeightRepository pltWeightRepository = RepositoryFactory.GetInstance().GetRepository<IPalletWeightRepository, PalletWeight>();

            Product CurrentProduct = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);
            PalletWeight CurrentPalletWeight = null;

            short palletQty = 0;
            IPalletRepository pltRepository = RepositoryFactory.GetInstance().GetRepository<IPalletRepository, Pallet>();

            IList<DeliveryPallet> dpList = pltRepository.GetDeliveryPallet(CurrentPallet.PalletNo);
            if (dpList != null)
            {
                foreach (DeliveryPallet temp in dpList)
                {
                    palletQty = (short)(palletQty + temp.DeliveryQty);
                }
            }

            if ( CurrentProduct.ModelObj == null)
            {

                FisException ex;
                List<string> erpara = new List<string>();
                //errpara.Add(currentProduct.CUSTSN);
                //ex = new FisException("CHK189", erpara);
                ex = new FisException("CHK170", erpara);
                throw ex;
            }

            IList<PalletWeight> palletWeightList = pltWeightRepository.GetPltWeight(CurrentProduct.ModelObj.FamilyName, CurrentProduct.ModelObj.Region, palletQty);
            if (palletWeightList != null && palletWeightList.Count > 0)
            {
                CurrentPalletWeight = palletWeightList[0];
            }

            if (CurrentPalletWeight == null)
            {
                CurrentSession.AddValue(Session.SessionKeys.NeedCheckPalletWeight, false);

            }
            else
            {
                CurrentSession.AddValue(Session.SessionKeys.NeedCheckPalletWeight, true);
                IProductRepository currentProductRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();

                decimal AllCartonWeight = currentProductRepository.GetAllCartonWeightByPallet(CurrentPallet.PalletNo);


                CurrentSession.AddValue(Session.SessionKeys.Tolerance, CurrentPalletWeight.Tolerance);
                CurrentSession.AddValue(Session.SessionKeys.StandardWeight, CurrentPalletWeight.Weight + AllCartonWeight);


            }


            return base.DoExecute(executionContext);
        }
    }
}
