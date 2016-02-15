// INVENTEC corporation (c)2010 all rights reserved. 
// Description: 当Unit,PizzaID，Carton称重修改标准重量时，
//              根据Model获取标准重量,没有标准重量报错
//              有将当前输入的新重量保存为标准重量
//                     
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2010-01-26   Yuan XiaoWei                 create
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
using System.Collections.Generic;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PAK.StandardWeight;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
namespace IMES.Activity
{
    /// <summary>
    /// 应用于UnitWeight,PizzaWeight,CartonWeight修改标准重量
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      应用于UnitWeight,PizzaWeight,CartonWeight修改标准重量
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.根据Model获取ModelWeight，将当前输入的新标准重量作为标准重量保存到ModelWeight;
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.Model
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
    ///         更新标准重量ModelWeight
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         IModelWeightRepository
    ///         ModelWeight
    /// </para> 
    /// </remarks>
    public partial class UpdateModelWeight : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public UpdateModelWeight()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 标准重量的类型，共有两种Unit，Carton
        /// </summary>
        public static DependencyProperty WeightTypeProperty = DependencyProperty.Register("WeightType", typeof(WeightTypeEnum), typeof(UpdateModelWeight));

        /// <summary>
        /// 标准重量的类型，共有两种Unit，Carton
        /// </summary>
        [DescriptionAttribute("WeightType")]
        [CategoryAttribute("WeightType Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public WeightTypeEnum WeightType
        {
            get
            {
                return ((WeightTypeEnum)(base.GetValue(UpdateModelWeight.WeightTypeProperty)));
            }
            set
            {
                base.SetValue(UpdateModelWeight.WeightTypeProperty, value);
            }
        }

        /// <summary>
        /// 执行逻辑
        /// 标准误差如果Model没有，再按Custormer找
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            string currentModel = (string)CurrentSession.GetValue(Session.SessionKeys.ModelName);
            IModelWeightRepository currentModelWeightRepository = RepositoryFactory.GetInstance().GetRepository<IModelWeightRepository, ModelWeight>();
            ModelWeight currentModelWeight = currentModelWeightRepository.Find(currentModel);



            if (WeightType == WeightTypeEnum.Unit)
            {
                if (currentModelWeight == null || currentModelWeight.UnitWeight.Equals(0))
                {
                    List<string> errpara = new List<string>();

                    errpara.Add(currentModel);

                    throw new FisException("CHK003", errpara);
                }

                currentModelWeight.UnitWeight = (decimal)CurrentSession.GetValue(Session.SessionKeys.ActuralWeight);
                currentModelWeight.Editor = Editor;
                currentModelWeight.Udt = DateTime.Now;

                currentModelWeightRepository.Update(currentModelWeight, CurrentSession.UnitOfWork);
            }
            else
            {
                if (currentModelWeight == null || currentModelWeight.CartonWeight.Equals(0))
                {
                    List<string> errpara = new List<string>();

                    errpara.Add(currentModel);

                    throw new FisException("CHK003", errpara);
                }

                currentModelWeight.CartonWeight = (decimal)CurrentSession.GetValue(Session.SessionKeys.ActuralWeight);
                currentModelWeight.Editor = Editor;
                currentModelWeight.Udt = DateTime.Now;

                currentModelWeightRepository.Update(currentModelWeight, CurrentSession.UnitOfWork);

            }

            return base.DoExecute(executionContext);
        }

        /// <summary>
        /// 标准重量的类型，共有两种Unit，Carton
        /// </summary>
        public enum WeightTypeEnum
        {
            /// <summary>
            /// 获取Unit标准重量和误差
            /// </summary>
            Unit = 1,

            /// <summary>
            /// 获取Carton标准重量和误差
            /// </summary>
            Carton = 2
        }
    }
}
