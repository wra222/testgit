// INVENTEC corporation (c)2010 all rights reserved. 
// Description: PO Data模块的删除DN功能中的删除Delivery、Pallet部分子功能
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-12-09   itc202017                     Create
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
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.Station;
using System.Collections.Generic;
using IMES.FisObject.PAK.DN;
using IMES.FisObject.PAK.Pallet;
using IMES.Infrastructure.Repository._Schema;
using System.Data.SqlClient;
using IMES.SAP.Interface;
using IMES.Infrastructure.Extend.Utility;
using IMES.DataModel;


//using IMES.Infrastructure.Utility.Generates.cus.Utility.Generates.impl;
namespace IMES.Activity
{
    /// <summary>
    /// 呼叫SAP Service,傳入DN,删除綁定的Custimer SN
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
    ///         按照DN删除SAP中删除綁定的Custimer SN
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.SessionKeys.DeliveryNo
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
    ///          
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///             
    ///             
    /// </para> 
    /// </remarks>
    public partial class DeleteSnOnSAP : BaseActivity
	{
        /// <summary>
        /// 构造函数
        /// </summary>
        public DeleteSnOnSAP()
		{
			InitializeComponent();
      
		}

        /// <summary>
        /// 機型分類
        /// </summary>
        public static DependencyProperty ModelCategoryProperty = DependencyProperty.Register("ModelCategory", typeof(ModelCategoryEnum), typeof(DeleteSnOnSAP), new PropertyMetadata(ModelCategoryEnum.Unknow));
        /// <summary>
        ///  機型分類
        /// </summary>
        [DescriptionAttribute("ModelCategory")]
        [CategoryAttribute("ModelCategory")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public ModelCategoryEnum ModelCategory
        {
            get
            {
                return ((ModelCategoryEnum)(base.GetValue(DeleteSnOnSAP.ModelCategoryProperty)));
            }
            set
            {
                base.SetValue(DeleteSnOnSAP.ModelCategoryProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        /// 
      
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            IDeliveryRepository DeliveryRepository = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
            //string dnNo="";
            IList<Delivery> dnList = new List<Delivery>();
            if (CurrentSession.GetValue(Session.SessionKeys.DeliveryList) != null)
            {
                dnList = (IList<Delivery>)CurrentSession.GetValue(Session.SessionKeys.DeliveryList);
            }
            else if(CurrentSession.GetValue(Session.SessionKeys.Delivery) != null)
            {
                dnList.Add((Delivery)CurrentSession.GetValue(Session.SessionKeys.Delivery) );
            }
            else if (CurrentSession.GetValue(Session.SessionKeys.DeliveryNo) != null)
            {
                string dnNo = (string)CurrentSession.GetValue(Session.SessionKeys.DeliveryNo);
                
                if (!string.IsNullOrEmpty(dnNo))
                {
                    Delivery dn= DeliveryRepository.Find(dnNo);
                    dnList.Add(dn);
                    CurrentSession.AddValue(Session.SessionKeys.Delivery, dn);
                }                
            }
            else if(CurrentSession.GetValue(Session.SessionKeys.Product) !=null)
            {
                var product = CurrentSession.GetValue(Session.SessionKeys.Product) as IProduct;
                string dnNo = product.DeliveryNo;
                
                if (!string.IsNullOrEmpty(dnNo))
                {
                    Delivery dn= DeliveryRepository.Find(dnNo);
                    dnList.Add(dn);
                    CurrentSession.AddValue(Session.SessionKeys.Delivery, dn);
                }
            }


            //if (CurrentSession.GetValue("DNStatus") == null)
            //{                
            //    if (!string.IsNullOrEmpty(dn))
            //    {
            //        Delivery CurrentDelivery = DeliveryRepository.Find(dn);
            //        if (CurrentDelivery != null)
            //        {
            //            CurrentSession.AddValue("DNStatus", CurrentDelivery.Status);
            //        }
            //    }             
            //}        
           // string dn = (string)CurrentSession.GetValue(Session.SessionKeys.DeliveryNo);
           
                foreach (Delivery item in dnList)
                {
                    if (
                        //  CurrentSession.GetValue("IsSuper")!=null && 
                        //(string)CurrentSession.GetValue("IsSuper") == "Y" && 
                        //!string.IsNullOrEmpty(dn) &&
                         CurrentSession.GetValue("ExcuteDeleteSNonSAP") != null &&
                         (string)CurrentSession.GetValue("ExcuteDeleteSNonSAP") == "Y" &&
                        //CurrentSession.GetValue("DNStatus") != null &&
                        //(string)CurrentSession.GetValue("DNStatus") == "98"
                        item.Status == "98"
                       )
                    {
                        string plant = (string)CurrentSession.GetValue("PlantCode");

                        IMES.SAP.Interface.ISAPService sap = ServiceAgent.getInstance().GetObjectByName<ISAPService>(ServiceConstant.SAPService);
                        string sapErr="";
                        string sapResult="";

                        //sapResult = sap.CancelBindDN(plant, dn.Substring(0, 10), out sapErr);
                        if (ModelCategory == ModelCategoryEnum.Unknow)
                        {
                            sapResult = sap.CancelBindDN(plant, item.DeliveryNo.Substring(0, 10), out sapErr);
                        }
                        else
                        {
                            sapResult = sap.CancelBindDNbyItem(plant, 
                                                                        item.DeliveryNo.Substring(0, 10), 
                                                                        item.DeliveryNo.Substring(10),
                                                                        ModelCategory== ModelCategoryEnum.SKU?"C":"O",
                                                                        out sapErr);
                        }
                        if (!IsAllowUnpack(sapResult))
                        {
                            throw new FisException("PAK170", new string[] { sapErr });
                        }
            
                    }
                }
            
           

             return base.DoExecute(executionContext);
        }

        private bool IsAllowUnpack(string errCode)
        {
            string allowUnpackCode = (string)CurrentSession.GetValue("AllowUnpackCode") ?? "";
            string[] allowUnpackCodeLst = allowUnpackCode.Split(',');
            return allowUnpackCodeLst.Contains(errCode);
        }

        /// <summary>
        /// Model Category
        /// </summary>
        public enum ModelCategoryEnum
        {
            /// <summary>
            /// 
            /// </summary>
            Unknow=0,
            /// <summary>
            /// 
            /// </summary>
            SKU,
            /// <summary>
            /// 
            /// </summary>
            Docking
        }

	}
}
