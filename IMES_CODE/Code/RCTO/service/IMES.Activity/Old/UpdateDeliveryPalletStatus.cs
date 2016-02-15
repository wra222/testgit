// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2009-11-05   Yuan XiaoWei                 create
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
using IMES.FisObject.PAK.DN;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.Generic;

namespace IMES.Activity
{

    /// <summary>
    /// DN在一个Pallet上刷满时，更新Delivery_Pallet的Status
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      应用于PAK Pallet Data Collection
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.从Session中获取Pallet对象
    ///         2.从Session中获取Delivery对象
    ///         3.调用Delivery对象的UpdateDeliveryPalletStatus方法
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.Pallet
    ///         Session.Delivery
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
    ///         更新DeliveryPallet
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///              IDeliveryRepository
    ///              Pallet
    ///              Delivery
    ///              DeliveryPallet
    ///              
    /// </para> 
    /// </remarks>
    public partial class UpdateDeliveryPalletStatus : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public UpdateDeliveryPalletStatus()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 更新Delivery_Pallet的Status
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            Pallet CurrentPallet = (Pallet)CurrentSession.GetValue(Session.SessionKeys.Pallet);
           
            DeliveryPallet MyDeliveryPalet = new DeliveryPallet();
            //ship to pallet/pallet print/unpack pallet
            if (Station == "87" || (Station == "87U"))
            {
                IPalletRepository pltRepository = RepositoryFactory.GetInstance().GetRepository<IPalletRepository, Pallet>();
                IList<DeliveryPallet> dnpltlist= pltRepository.GetDeliveryPallet(CurrentPallet.PalletNo);
                IList<Delivery> deliveryList = new List<Delivery>(); 
                foreach (DeliveryPallet temp in dnpltlist)
                {
                    IDeliveryRepository DeliveryRepository = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
                    Delivery thisDelivery = DeliveryRepository.Find(temp.DeliveryID);                
                    MyDeliveryPalet = temp;
                    MyDeliveryPalet.Status = Status;
                    MyDeliveryPalet.Editor = this.Editor;
                    thisDelivery.UpdateDeliveryPalletStatus(MyDeliveryPalet);
                    deliveryList.Add(thisDelivery);//for session                    
                    DeliveryRepository.Update(thisDelivery, CurrentSession.UnitOfWork);


                }
                CurrentSession.AddValue(Session.SessionKeys.DeliveryList, deliveryList);

            }
            else
            {
                Delivery CurrentDelivery = (Delivery)CurrentSession.GetValue(Session.SessionKeys.Delivery);
                foreach (DeliveryPallet temp in CurrentDelivery.DnPalletList)
                {
                    if (temp.PalletID.Equals(CurrentPallet.PalletNo))
                    {
                        MyDeliveryPalet = temp;
                        break;
                    }
                }

                MyDeliveryPalet.Status = Status;
                MyDeliveryPalet.Editor = this.Editor;

                CurrentDelivery.UpdateDeliveryPalletStatus(MyDeliveryPalet);
                IDeliveryRepository DeliveryRepository = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
                DeliveryRepository.Update(CurrentDelivery, CurrentSession.UnitOfWork);
            }
            return base.DoExecute(executionContext);
           
        }

        public static DependencyProperty StatusProperty = DependencyProperty.Register("Status", typeof(string), typeof(UpdateDeliveryPalletStatus));

        [DescriptionAttribute("Status")]
        [CategoryAttribute("Status Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string Status
        {
            get
            {
                return ((string)(base.GetValue(UpdateDeliveryPalletStatus.StatusProperty)));
            }
            set
            {
                base.SetValue(UpdateDeliveryPalletStatus.StatusProperty, value);
            }
        }
    }
}
