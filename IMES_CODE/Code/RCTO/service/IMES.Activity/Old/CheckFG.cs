// INVENTEC corporation (c)2009 all rights reserved. 
// Description:检查Model是否是FG 
//                   
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2010-03-10   Lucy Liu                 create
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
using IMES.FisObject.Common.Model;

namespace IMES.Activity
{

    /// <summary>
    /// 检查Model是否是FG   
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      FG Shipping Label(TRO)
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.;根据Product对象的Model得到Model对象,判断Name=TYPE的Value是否为FG，或SFG，如果不是报错
    ///         2.;
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    ///                     PAK003
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
    public partial class CheckFG : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CheckFG()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 根据Session.Product获取到Product对象
        /// 根据Product对象的Model得到Model对象,判断Name=TYPE的Value是否为FG，或SFG，如果不是报错
        /// (This is not FG Model)
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            string ret = null;
            //从Session里取得Product对象
            Product currentProduct = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);
           
            //根据Product对象的model获取到Model对象
            IModelRepository modelRep = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();
            Model model = modelRep.Find(currentProduct.Model);
           
            if (model == null)
            {
                //model对象获取为空,抛异常
                List<string> erpara = new List<string>();
                erpara.Add(currentProduct.Model);
                FisException ex = new FisException("CHK038", erpara);
                throw ex;
            }
            
            //取得Name="TYPE"的Value字段值
            ret = model.GetAttribute("TYPE");
            if (ret == null || ret.Trim() == string.Empty)
            {
                //This is not FG Model !
           
                throw new FisException("PAK003", new string[] { currentProduct.ProId  });
            }
            if ((ret.ToUpper().CompareTo("FG") != 0) && (ret.ToUpper().CompareTo("SFG") != 0))
            {
                //This is not FG Model !
                throw new FisException("PAK003", new string[] { currentProduct.ProId });
            }



            return base.DoExecute(executionContext);
        }

       


    }
}


