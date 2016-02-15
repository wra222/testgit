// INVENTEC corporation (c)2011 all rights reserved. 
//                   
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-10-26   210003                       create
// Known issues:
using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
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
using IMES.FisObject.PAK.Paking;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.FA.Product;
namespace IMES.Activity.Special
{
    /// <summary>
    /// 取得该Delivery No 的Consolidated 属性（IMES_PAK.DeliveryInfo）
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    /// CI-MES12-SPEC-PAK-UC Pallet Verify.docx
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         无
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         无
    /// </para> 
    ///<para> 
    /// 输出：
    ///         Session.MB
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///         无
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    /// </para> 
    /// </remarks>
    public partial class CheckDNAndShipment : BaseActivity
	{
        /// <summary>
        /// 
        /// </summary>
		public CheckDNAndShipment()
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
            //var logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            ////////logger.InfoFormat("GetProductActivity: Key: {0}", this.Key);
            IPakingRepository PAK_PAKComnRepository = RepositoryFactory.GetInstance().GetRepository<IPakingRepository, PAK_PAKComn>();
            if (PAK_PAKComnRepository.DNExist(this.Dn_Shipment))
            {
                if (!PAK_PAKComnRepository.IsEDIUploadedForDN(this.Dn_Shipment))
                {
                    FisException ex;
                    List<string> erpara = new List<string>();
                    //erpara.Add(CurrentProduct.ProId);
                    ex = new FisException("PAK007", erpara);//'0','EDI850資料沒有上傳: '+@DN.错误号待定。
                    throw ex;
                }
                if (!PAK_PAKComnRepository.IsInstructionForDN(this.Dn_Shipment))
                {
                    FisException ex;
                    List<string> erpara = new List<string>();
                    //erpara.Add(CurrentProduct.ProId);
                    ex = new FisException("PAK007", erpara);//'0',@DN+' :此船务需要Instruction'.错误号待定。
                    throw ex;
                }
            }
            else if (PAK_PAKComnRepository.ShipmentExist(this.Dn_Shipment))
            {
                if (!PAK_PAKComnRepository.IsEDIUploadedForShipment(this.Dn_Shipment))
                {
                    FisException ex;
                    List<string> erpara = new List<string>();
                    //erpara.Add(CurrentProduct.ProId);
                    ex = new FisException("PAK007", erpara);//'0','EDI850資料沒有上傳: '+@DN.错误号待定。
                    throw ex;
                }
                if (!PAK_PAKComnRepository.IsInstructionForShipment(this.Dn_Shipment))
                {
                    FisException ex;
                    List<string> erpara = new List<string>();
                    //erpara.Add(CurrentProduct.ProId);
                    ex = new FisException("PAK007", erpara);//'0',@DN+' :缺少Instruction资料'.错误号待定。
                    throw ex;
                }
            }
            else
            {
                if (PAK_PAKComnRepository.WayBillNumberExist(this.Dn_Shipment))
                {
                    if (!PAK_PAKComnRepository.IsEDIUploadedForWayBillNumber(this.Dn_Shipment))
                    {
                        FisException ex;
                        List<string> erpara = new List<string>();
                        //erpara.Add(CurrentProduct.ProId);
                        ex = new FisException("PAK007", erpara);//'0','EDI850資料沒有上傳: '+@DN.错误号待定。
                        throw ex;
                    }
                    if (!PAK_PAKComnRepository.IsInstructionForWayBillNumber(this.Dn_Shipment))
                    {
                        FisException ex;
                        List<string> erpara = new List<string>();
                        //erpara.Add(CurrentProduct.ProId);
                        ex = new FisException("PAK007", erpara);//'0',@DN+' :缺少Instruction资料'.错误号待定。
                        throw ex;
                    }
                }
            }
            //ILineRepository lineRepository = RepositoryFactory.GetInstance().GetRepository<ILineRepository, Line>();
            //IList<string> lines = lineRepository.GetLineByCustAndStage(this.CustomerID, this.Stage);
            //bool dn_exist = PAK_PAKComnRepository
            //CurrentSession.AddValue(Session.SessionKeys.Lines, lines);
            return base.DoExecute(executionContext);
        }
        /// <summary>
        /// 输入的类型:string
        /// </summary>
        public static DependencyProperty Dn_ShipmentProperty = DependencyProperty.Register("Dn_Shipment", typeof(string), typeof(CheckDNAndShipment));
        /// <summary>
        /// Stage
        /// </summary>
        [DescriptionAttribute("Dn_Shipment")]
        [CategoryAttribute("InArugment")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string Dn_Shipment
        {
            get
            {
                return ((string)(base.GetValue(CheckDNAndShipment.Dn_ShipmentProperty)));
            }
            set
            {
                base.SetValue(CheckDNAndShipment.Dn_ShipmentProperty, value);
            }
        }
	}
}
