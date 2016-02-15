
// INVENTEC corporation (c)2011 all rights reserved. 
// Description:  UPallet称重时，获取标准重量和误差
// UI:CI-MES12-SPEC-PAK-UI Pallet Weight.docx
// UC:CI-MES12-SPEC-PAK-UC Pallet Weight.docx                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-12-24   Du Xuan (itc98066)          create
// ITC-1360-0664 从SysSetting获取相应数据
// ITC-1360-1802 根据uc修改standweight算法
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
using IMES.FisObject.PAK.DN;
using IMES.FisObject.Common.Part;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.Generic;
using IMES.DataModel;

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
    ///        
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

            IPalletRepository palletRep = RepositoryFactory.GetInstance().GetRepository<IPalletRepository, Pallet>();
            //decimal standWeight = 0;
            /*WhPltWeightInfo whInfo = new WhPltWeightInfo();
            whInfo.plt = CurrentPallet.PalletNo;            
            IList<WhPltWeightInfo> infoList = palletRep.GetWhPltWeightList(whInfo);
            
             foreach (WhPltWeightInfo node in infoList)
            {
                standWeight = standWeight + node.forecasetPltWeight;
            }
            */

            //a.	Get Pallet Standard Weight
            //SELECT @ProductsWeight = AVG(UnitWeight) FROM Product nolock WHERE PalletNo = @PalletNo AND ISNULL(UnitWeight, 0.000) <> 0.000
            int count = 0;
            decimal productWeight = 0;
            IProductRepository productRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            IList<IProduct> productList = productRep.GetProductByPalletNo(CurrentPallet.PalletNo);
            foreach (Product node in productList)
            {
                if (node.UnitWeight != 0)
                {
                    count++;
                    productWeight = productWeight + node.UnitWeight;
                }
            }

            if (count > 0)
            {
                productWeight = productWeight / count;
            }

            //SELECT @PalletQty = SUM(DeliveryQty) FROM Delivery_Pallet nolock WHERE PalletNo = @PalletNo
            IDeliveryRepository deliveryRep = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
            int PalletQty = deliveryRep.GetSumDeliveryQtyOfACertainPallet(CurrentPallet.PalletNo);

            //SELECT @ProductsWeight = @ProductsWeight * @PalletQty
            productWeight = productWeight * PalletQty;

            CurrentSession.AddValue("ProductWeight", productWeight);

            //SELECT @PalletWeight = PalletWeight FROM PalletWeight NOLOCK WHERE PalletType = @PalletType
            //分析顺序问题，放入后端处理
            /*decimal palletWeight = 0;
            IList<PalletWeightInfo> weightList = null;
            PalletWeightInfo conf = new PalletWeightInfo();
            string pclor = (string)CurrentSession.GetValue("PalletColor");
            conf.palletType = pclor.Trim();
            weightList = pltWeightRepository.GetPltWeightByCondition(conf);
            if ((weightList != null) &&(weightList.Count>0))
            {
                palletWeight = weightList[0].palletWeight;
            }

            //SELECT @PalletStandardWeight = @ProductsWeight + @PalletWeight
            standWeight = productWeight + palletWeight;
            CurrentSession.AddValue(Session.SessionKeys.StandardWeight, standWeight);*/


            decimal tolerance = 0; 
            IPartRepository partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            IList<string> valueList = new List<string>();
            valueList = partRepository.GetValueFromSysSettingByName("PltWeightTolerance");
            if (valueList.Count == 0)
            {
                tolerance = 2;
            }
            else
            {
                tolerance =Convert.ToDecimal(valueList[0]);
            }

            CurrentSession.AddValue(Session.SessionKeys.Tolerance, tolerance);

            return base.DoExecute(executionContext);
        }
    }
}
