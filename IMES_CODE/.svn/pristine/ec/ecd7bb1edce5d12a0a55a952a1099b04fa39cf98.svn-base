// INVENTEC corporation (c)2011 all rights reserved. 
// Description: 判断当前Product的DeliveryNo的InfoType= CustPo / IECSo对应的InfoValue属性是否全
//                   
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-10-26   Kerwin                       create
// Known issues:
using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PAK.DN;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.FisBOM;

namespace IMES.Activity
{

    /// <summary>
    /// 判断当前Product的DeliveryNo的InfoType= CustPo / IECSo对应的InfoValue属性是否全
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      CI-MES12-SPEC-PAK-UC Asset Tag Label Print
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.判断当前Product的Product.DeliveryNo 在DeliveryInfo分别查找InfoType= CustPo / IECSo对应的InfoValue，
    ///           其中某一个得不到时，都需要报错:DN資料不全CustPo/IECSo,請檢查一下船務
    ///
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    ///                     PAK007
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
    ///         Product
    ///         
    /// </para> 
    /// </remarks>
    public partial class CheckDNForAssetTagPrint : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CheckDNForAssetTagPrint()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 判断当前Product的Product.DeliveryNo 在DeliveryInfo分别查找InfoType= CustPo / IECSo对应的InfoValue，
        /// 其中某一个得不到时，都需要报错:DN資料不全CustPo/IECSo,請檢查一下船務
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {

            //从Session里取得Product对象
            Product CurrentProduct = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);

            /* 2012-9-3, Jessica Liu, for UC Change
            IDeliveryRepository currentDNRepository = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
            string CustPo = currentDNRepository.GetDeliveryInfoValue(CurrentProduct.DeliveryNo, "CustPo");
            if (string.IsNullOrEmpty(CustPo))
            {
                FisException ex;
                List<string> erpara = new List<string>();
                erpara.Add(CurrentProduct.ProId);
                ex = new FisException("PAK007", erpara);
                throw ex;
            }

            string IECSo = currentDNRepository.GetDeliveryInfoValue(CurrentProduct.DeliveryNo, "IECSo");
            if (string.IsNullOrEmpty(IECSo))
            {
                FisException ex;
                List<string> erpara = new List<string>();
                erpara.Add(CurrentProduct.ProId);
                ex = new FisException("PAK007", erpara);
                throw ex;
            }
            */

            //Update by Kaisheng,Zhang -2012/04/21 
            //UC Update:
            //------------------------------------------------
            //E.	如果该Product 不需要列印Asset Tag Label，则报告错误：“该产品不需要列印Asset Tag Label!”
            //條件：如果ModelBOM 的直接下阶不存在BomNodeType = 'AT'，并且Descr in ('ATSN4','ATSN7','ATSN8') 的Part 时，则该Product 不需要列印Asset Tag Label
            IBOMRepository bomRepository = RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();
            IHierarchicalBOM curBOM = null;
            bool printFlag = false;
            curBOM = bomRepository.GetHierarchicalBOMByModel(CurrentProduct.Model);
            IList<IBOMNode> bomNodeLst = new List<IBOMNode>();
            bomNodeLst = curBOM.FirstLevelNodes;
            if (bomNodeLst != null && bomNodeLst.Count > 0)
            {
                foreach (IBOMNode ibomnode in bomNodeLst)
                {
                    if ((ibomnode.Part.BOMNodeType.ToUpper() == "AT") && ((ibomnode.Part.Descr.ToUpper() == "ATSN4")||(ibomnode.Part.Descr.ToUpper() == "ATSN7")||(ibomnode.Part.Descr.ToUpper() == "ATSN8")))
                    {
                        printFlag = true;
                        break;
                    }
                }
            }
            if (printFlag == false)
            {
                FisException ex;
                List<string> erpara = new List<string>();
                erpara.Add(this.Key);
                ex = new FisException("PAK097", erpara);// No need to print Asset Tag Label!--modify code
                throw ex;
            }
            //------------------------------------------------

            //2012-9-6, Jessica Liu
            if (string.IsNullOrEmpty(CurrentProduct.DeliveryNo))   
            {
                List<string> errpara = new List<string>();

                errpara.Add(CurrentProduct.CUSTSN);

                throw new FisException("CHK955", errpara);  //该Product尚未结合Delivery,不能列印Asset Label!
            }

            return base.DoExecute(executionContext);
        }

    }
}

