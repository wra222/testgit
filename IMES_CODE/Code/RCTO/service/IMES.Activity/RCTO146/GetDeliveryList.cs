// INVENTEC corporation (c)2009 all rights reserved. 
// Description:  根据输入的DeliveryNo,获取Delivery对象，并放到Session中
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2009-11-02   Yuan XiaoWei                 create
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
using IMES.FisObject.PAK.DN;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.Generic;
using IMES.FisObject.FA.Product;
using IMES.DataModel;
namespace IMES.Activity
{
    /// <summary>
    /// 根据输入的DeliveryNo,获取Delivery对象，并放到Session中
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      045
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.根据DeliveryNo，调用IDeliveryRepository的Find方法，获取Delivery对象，添加到Session中
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.DeliveryNo
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         无
    /// </para> 
    ///<para> 
    /// 输出：
    ///         Session.Delivery
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///          
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///              IDeliveryRepository
    ///              Delivery
    /// </para> 
    /// </remarks>


	public partial class GetDeliveryList: BaseActivity
	{
        /// <summary>
        /// constructor
        /// </summary>
        public GetDeliveryList()
		{
			InitializeComponent();
		}
        /// <summary>
        /// Set the end day to query delivery
        /// </summary>
        public static DependencyProperty EndDayProperty = DependencyProperty.Register("EndDay", typeof(int), typeof(GetDeliveryList), new PropertyMetadata(0));

        /// <summary>
        ///  Set the end day to query delivery
        /// </summary>
        [DescriptionAttribute("EndDay")]
        [CategoryAttribute("InArugment Of GetDeliveryList")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public int EndDay
        {
            get
            {
                return ((int)(base.GetValue(GetDeliveryList.EndDayProperty)));
            }
            set
            {
                base.SetValue(GetDeliveryList.EndDayProperty, value);
            }
        }


        /// <summary>
        /// Set the begin day to query delivery
        /// </summary>
        public static DependencyProperty BeginDayProperty = DependencyProperty.Register("BeginDay", typeof(int), typeof(GetDeliveryList), new PropertyMetadata(0));

        /// <summary>
        ///  Set the begin day to query delivery
        /// </summary>
        [DescriptionAttribute("BeginDay")]
        [CategoryAttribute("InArugment Of GetDeliveryList")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public int BeginDay
        {
            get
            {
                return ((int)(base.GetValue(GetDeliveryList.BeginDayProperty)));
            }
            set
            {
                base.SetValue(GetDeliveryList.BeginDayProperty, value);
            }
        }

        /// <summary>
        /// Set the status to query delivery
        /// </summary>
        public static DependencyProperty StatusProperty = DependencyProperty.Register("Status", typeof(string), typeof(GetDeliveryList), new PropertyMetadata(""));

        /// <summary>
        ///  Set the status day to query delivery
        /// </summary>
        [DescriptionAttribute("Status")]
        [CategoryAttribute("InArugment Of GetDeliveryList")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string Status
        {
            get
            {
                return ((string)(base.GetValue(GetDeliveryList.StatusProperty)));
            }
            set
            {
                base.SetValue(GetDeliveryList.StatusProperty, value);
            }
        }

        /// <summary>
        /// 根据DeliveryNo，调用IDeliveryRepository的Find方法，获取Delivery对象，添加到Session中
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            string model = "";
            if (CurrentSession.GetValue(Session.SessionKeys.ModelName) != null)
            { model = (string)CurrentSession.GetValue(Session.SessionKeys.ModelName); }
            else 
            {
                var product = (IProduct)CurrentSession.GetValue(Session.SessionKeys.Product);
                if (product != null)
                { model = product.Model; }
            }
            if(string.IsNullOrEmpty(model))
            { throw new FisException("The model is null in GetDelieryList"); }
          
            DateTime beginDate
               =new DateTime(DateTime.Now.AddDays(BeginDay).Year,DateTime.Now.AddDays(BeginDay).Month,
                                         DateTime.Now.AddDays(BeginDay).Day);

             DateTime endDate
               =new DateTime(DateTime.Now.AddDays(EndDay).Year,DateTime.Now.AddDays(EndDay).Month,
                                         DateTime.Now.AddDays(EndDay).Day);
         
            IDeliveryRepository DeliveryRepository = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
            if(string.IsNullOrEmpty(Status))
            {Status="00";}
            IList<DeliveryForRCTO146> lstDn=
              DeliveryRepository.GetDeliveryForRCTO146(model.Trim(), Status, beginDate, endDate);
            CurrentSession.AddValue("DeliveryForRCTO146List", lstDn);
            return base.DoExecute(executionContext);
        }
	}
}
