// INVENTEC corporation (c)2011 all rights reserved. 
// Description: 根据productId,station获取UnitWeightProductLog对象
//                    
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-12-16   Chen Xu  itc208014           create
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
using IMES.FisObject.Common.PartSn;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure;
using IMES.FisObject.Common.MO;
using IMES.FisObject.FA.Product;
using System.Collections.Generic;
using IMES.FisObject.Common.Station;

namespace IMES.Activity
{
    /// <summary>
    ///   根据productId,station获取UnitWeightProductLog对象
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///         应用于检查是否有UnitWeight过站log
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.参考UC;
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.业务异常：
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
    ///         无
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         IProductRepository
    ///         IProduct
    /// </para> 
    /// </remarks>
    public partial class GetUnitWeightProductLog : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public GetUnitWeightProductLog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// LabelName
        /// </summary>
        public static DependencyProperty LabelNameProperty = DependencyProperty.Register("LabelName", typeof(LabelNameFromEnum), typeof(GetUnitWeightProductLog));

        /// <summary>
        /// Status
        /// </summary>
        [DescriptionAttribute("labelName")]
        [CategoryAttribute("labelName Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public LabelNameFromEnum LabelName
        {
            get
            {
                return ((LabelNameFromEnum)(base.GetValue(GetUnitWeightProductLog.LabelNameProperty)));
            }
            set
            {
                base.SetValue(GetUnitWeightProductLog.LabelNameProperty, value);
            }
        }


        /// <summary>
        /// Model的来源，共有两种ModelName，ModelObj
        /// </summary>
        public enum LabelNameFromEnum
        {
            /// <summary>
            /// Config Label
            /// </summary>
            ConfigLabel = 1,

            /// <summary>
            /// POD Label
            /// </summary>
            PODLabel = 2
        }
        
        /// <summary>
        /// 根据productId,station获取UnitWeightProductLog对象
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            FisException ex;
            List<string> erpara = new List<string>();

            IProductRepository iproductRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            Product currentProduct = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);

            // //ITC-1360-1517:  CurrentSession.AddValue(Session.SessionKeys.isPassStationLog, false);
            CurrentSession.AddValue(Session.SessionKeys.isPassStationLog, false);

            IList<ProductLog> prodConfigLogLst = new List<ProductLog>();
            prodConfigLogLst = iproductRepository.GetProductLogs(currentProduct.ProId, this.Station);

            IList<ProductLog> prodPODLogLst = new List<ProductLog>();
            prodPODLogLst = iproductRepository.GetProductLogs(currentProduct.ProId, "PD");

            if ((prodConfigLogLst == null || prodConfigLogLst.Count <= 0) && (prodPODLogLst == null || prodPODLogLst.Count <= 0))
            {
                //该Customer S/N %1 没有Pass过Unit Weight 站，不能重印！
                erpara.Add(currentProduct.CUSTSN);
                ex = new FisException("SFC016", erpara);
                throw ex;
            }
            
            //Boolean flag = false;
            
            string labelBranch = CurrentSession.GetValue(Session.SessionKeys.labelBranch).ToString();

            if ((LabelName == LabelNameFromEnum.ConfigLabel) && (labelBranch =="C" || labelBranch =="CP"))
            {
                //CurrentSession.AddValue(Session.SessionKeys.isPassStationLog, false);
                
                foreach (ProductLog iprodlog in prodConfigLogLst)
                {
                    if (iprodlog.Status == StationStatus.Pass)
                    {
                        CurrentSession.AddValue(Session.SessionKeys.PrintLogName, "Config Label");
                        CurrentSession.AddValue(Session.SessionKeys.PrintLogList, prodConfigLogLst);
                        CurrentSession.AddValue(Session.SessionKeys.isPassStationLog, true);
                        
                        break;
                    }
                }

            }
            else if ((LabelName == LabelNameFromEnum.PODLabel) && (labelBranch=="P" || labelBranch =="CP"))
            {
                //CurrentSession.AddValue(Session.SessionKeys.isPassStationLog, false);

                foreach (ProductLog iprodlog in prodPODLogLst)
                {
                    if (iprodlog.Status == StationStatus.Pass)
                    {
                        CurrentSession.AddValue(Session.SessionKeys.PrintLogName, "POD Label");
                        CurrentSession.AddValue(Session.SessionKeys.PrintLogList, prodPODLogLst);
                        CurrentSession.AddValue(Session.SessionKeys.isPassStationLog, true);
                       
                        break;
                    }
                }
               
            }

            //if (!flag )
            //if (!(Boolean)CurrentSession.GetValue(Session.SessionKeys.isPassStationLog))
            //{
            //     erpara.Add(currentProduct.CUSTSN);
            //     ex = new FisException("SFC017", erpara);    //该CustomerSN不需要列印Config Label 和POD Label!
            //     throw ex;
            //}

            

            if ((LabelName == LabelNameFromEnum.PODLabel)&&(string.IsNullOrEmpty((string)CurrentSession.GetValue(Session.SessionKeys.PrintLogName))))
            {
                erpara.Add(currentProduct.CUSTSN);
                ex = new FisException("SFC017", erpara);    //该CustomerSN不需要列印Config Label 和POD Label!
                throw ex;
            }

            return base.DoExecute(executionContext);
        }

        
    }
}
