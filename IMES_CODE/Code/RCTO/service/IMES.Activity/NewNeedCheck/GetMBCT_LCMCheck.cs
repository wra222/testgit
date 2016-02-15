// INVENTEC corporation (c)2011 all rights reserved. 
// Description: 判断当前Product的DeliveryNo的InfoType= CustPo / IECSo对应的InfoValue属性是否全
//                   
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-10-26   Kerwin                       create
// Known issues:
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Linq;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Design;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.Runtime;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.Repository.PAK;


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
    ///         
    ///
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    ///                
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
    public partial class GetMBCT_LCMCheck : BaseActivity
    {
        /// <summary>
        /// get part list action.
        /// </summary>
        public GetMBCT_LCMCheck()
        {
            InitializeComponent();
        }

        /// <summary>
        /// get partlist action.
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
			// now come to get part list action.
            IProduct currenProduct = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);
            if (currenProduct == null)
            {
                List<string> errpara = new List<string>();
                errpara.Add(this.Key);
                throw new FisException("SFC002", errpara);
            }

            IList<IMES.FisObject.Common.Part.IProductPart>productPartList = currenProduct.ProductParts;

            IList<IMES.FisObject.Common.Part.IProductPart> lcmList = (from q in productPartList
                                                                  where q.CheckItemType == "LCM"
                                                                  select q).ToList<IMES.FisObject.Common.Part.IProductPart>();
            if (lcmList == null || lcmList.Count == 0)
            {
                //error
                throw new FisException("CHK1069", new string[] { this.Key, "LCM" });
            }
            string lcm = lcmList[0].PartSn;

            
            IList<ProductInfo> productInfoList = currenProduct.ProductInfoes;
            IList<ProductInfo> mbctList = (from q in productInfoList
                                           where q.InfoType == "ModelCT"
                                           select q).ToList<ProductInfo>();
            if (mbctList == null || mbctList.Count == 0)
            {
                //error
                mbctList = (from q in productInfoList
                            where q.InfoType == "MBCT"
                            select q).ToList<ProductInfo>();
                if (mbctList == null || mbctList.Count == 0)
                {
                    throw new FisException("CHK1069", new string[] { this.Key, "MBCT" });
                }
            }
            string mbct = mbctList[0].InfoValue;

            string CUSTSN = currenProduct.CUSTSN;

            
            CurrentSession.AddValue("LCM", lcm);
            CurrentSession.AddValue(Session.SessionKeys.MBCT, mbct);
            CurrentSession.AddValue("CUSTSN", CUSTSN);

            return base.DoExecute(executionContext);
        }

    }
}

