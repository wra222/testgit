
// INVENTEC corporation (c)2011 all rights reserved. 
// Description:  UPallet称重时，获取标准重量和误差
// UI:CI-MES12-SPEC-PAK-UI Pallet Weight.docx
// UC:CI-MES12-SPEC-PAK-UC Pallet Weight.docx                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2012-05-29   Du Xuan (itc98066)          create

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
    public partial class GetPalletStandardWeightForDocking : BaseActivity
    {
        /// <summary>
        /// 
        /// </summary>
        public GetPalletStandardWeightForDocking()
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

            IPalletRepository palletRep = RepositoryFactory.GetInstance().GetRepository<IPalletRepository, Pallet>();
            IDeliveryRepository deliveryRep = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
            IPalletWeightRepository pltWeightRep = RepositoryFactory.GetInstance().GetRepository<IPalletWeightRepository, PalletWeight>();

            Pallet CurrentPallet = (Pallet)CurrentSession.GetValue(Session.SessionKeys.Pallet);

            //为了支持FRU 出货，当Pallet 结合的Delivery 的Model 是PF 开头时，
            //不需要录入Customer S/N 和Check Customer S/N
            bool fruFLag = false;
            IList<DeliveryPallet> dnList = palletRep.GetDeliveryPallet(CurrentPallet.PalletNo);
            if (dnList.Count > 0)
            {
                Delivery dn = deliveryRep.Find(dnList[0].DeliveryID);
                string modelstr = dn.ModelName;
                if (!string.IsNullOrEmpty(modelstr) && modelstr.Length >= 2)
                {
                    if (modelstr.Substring(0, 2) == "PF")
                    {
                        fruFLag = true;
                    }
                }

            }
            CurrentSession.AddValue("FRUFlag", fruFLag);

            //基于UI 选择的Pallet Type 查询ConstValue 表得到Value 值
            string palletType = (string)CurrentSession.GetValue("PalletType");
            IPartRepository partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            ConstValueInfo info = new ConstValueInfo();
            info.type = "PT";
            info.name = palletType;
            IList<ConstValueInfo> retList = partRepository.GetConstValueInfoList(info);
            decimal palletWeight = 0;
            palletWeight = Convert.ToDecimal(retList[0].value);

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

            //SELECT @PalletQty = SUM(a.DeliveryQty * CONVERT(INT, CONVERT(Float, ISNULL(b.InfoValue, ''))))
	        //FROM Delivery_Pallet a (NOLOCK) LEFT JOIN DeliveryInfo b (NOLOCK)
		    //ON a.DeliveryNo = b.DeliveryNo AND b.InfoType = 'CQty'
	        //WHERE a.PalletNo = @PalletNo
	        //GROUP BY a.PalletNo
            int PalletQty =0;
            IList<DeliveryPallet> dpList = palletRep.GetDeliveryPallet(CurrentPallet.PalletNo);
            foreach (var node in dpList)
            {
                var dev = deliveryRep.Find(node.DeliveryID);
                string cQtyStr = (string)dev.GetExtendedProperty("CQty");
                int cqty;
                if (string.IsNullOrEmpty(cQtyStr))
                {
                    cqty = 0;
                }
                else
                {
                    decimal tmp = Convert.ToDecimal(cQtyStr);
                    cqty = Convert.ToInt32(tmp);
                }

                PalletQty = PalletQty + node.DeliveryQty * cqty;
            }

            //SELECT @ProductsWeight = @ProductsWeight * @PalletQty
            productWeight = productWeight * PalletQty;

            CurrentSession.AddValue("ProductWeight", productWeight);

            decimal tolerance = 0; 
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
            
            decimal standWeight = productWeight + palletWeight;
            if (fruFLag)
            {
                standWeight = 0;
            }

            CurrentSession.AddValue(Session.SessionKeys.StandardWeight, standWeight);
            CurrentSession.AddValue("PalletWeight", palletWeight);

            return base.DoExecute(executionContext);
        }
    }
}
