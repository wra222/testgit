// INVENTEC corporation (c)2011 all rights reserved. 
// Description:  
// UI:CI-MES12-SPEC-PAK-UI PD PA Label 1.docx
// UC:CI-MES12-SPEC-PAK-UC PD PA Label 1.docx                           
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-12-02   Du Xuan (itc98066)          create
// ITC-1360-1461 修改Product Model子传取错问题
// ITC-1360-1462 同1461
// Known issues:
using System;
using System.Data;
using System.Workflow.ComponentModel;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Model;
using IMES.FisObject.Common.Part;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PAK.DN;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.PAK.Pallet;
using IMES.DataModel;
using System.Collections.Generic;

namespace IMES.Activity
{
    /// <summary>
    /// 2 China label, GOST label, ICASA Label, ICASA Label2, KC Label, Taiwan Label 的列印条件
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///     
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
    ///         
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///          
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///              IProductRepository
    ///              productId
    /// </para> 
    /// </remarks>
    public partial class CheckLANOMLabelPrint : BaseActivity
	{
        /// <summary>
        /// constructor
        /// </summary>
        public CheckLANOMLabelPrint()
		{
			InitializeComponent();
		}

        /// <summary>
        /// Check RMN
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            Product curProduct = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);
            
            IModelRepository modelRep = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();
            IBOMRepository bomRep = RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();
            IPalletRepository palletRep = RepositoryFactory.GetInstance().GetRepository<IPalletRepository, Pallet>();
            IPartRepository partRep = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            IDeliveryRepository DeliveryRepository = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
            
            Delivery curDelivery = DeliveryRepository.Find(curProduct.DeliveryNo);
            Model curModel = modelRep.Find(curProduct.Model);
            string site = (string)CurrentSession.GetValue("Site");
            //满足下列条件之一，才需要列印LA NOM Label
            //Product Model 的第10，11码为’16’ or ‘DM’ or ‘D0’
            Boolean message = false;
            string  tmpstr = curProduct.Model.Substring(9,2);
            if(site=="ICC")
            {
                if (tmpstr == "16" || tmpstr == "DM" || tmpstr == "D0" || tmpstr == "D3")
                {
                    message = true;
                }
            }
            else
            {
                if (tmpstr == "16" || tmpstr == "DM" || tmpstr == "D0")
                {
                    message = true;
                }
            }
           
          
            //Product Model 的第10，11码为’D1’，并且在ModelBOM 中的Model 直接下阶中存在Part No = ‘6060B0284501’ 的Part
            //在ModelBOM 中的Model 直接下阶中存在Part No = ‘60LANOM00001’ 的Part
            if (site != "ICC")
            {
                IHierarchicalBOM curBom = bomRep.GetHierarchicalBOMByModel(curProduct.Model);
                IList<IBOMNode> bomList = curBom.FirstLevelNodes;
                if (tmpstr == "D1")
                {
                    foreach (IBOMNode node in bomList)
                    {
                        if (tmpstr == "D1" && node.Part.PN == "6060B0284501")
                        {
                            message = true;
                            break;
                        }

                        if (node.Part.PN == "60LANOM00001")
                        {
                            message = true;
                            break;
                        }
                    }
                }


            }// if (site != "ICC")
           
          
            if (message)
            {
                CurrentSession.AddValue("LANOMLabel", "LA NOM Label");
            }
            else
            {
                CurrentSession.AddValue("LANOMLabel", "");
            }

            return base.DoExecute(executionContext);
        }
	}
}
