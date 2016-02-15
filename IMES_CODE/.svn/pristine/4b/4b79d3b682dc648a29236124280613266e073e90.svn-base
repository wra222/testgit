// INVENTEC corporation (c)2011 all rights reserved. 
// Description: Insert SnoDetBTLoc
//                   
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-11-01   Kerwin                       create
// Known issues:
using System.Workflow.ComponentModel;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PAK.Pallet;
using System.ComponentModel;
using IMES.DataModel;

namespace IMES.Activity
{
    /// <summary>
    /// Insert SnoDetBTLoc
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      CI-MES12-SPEC-PAK-UC Assign WH Location for BT
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.Insert SnoDet_BTLoc
    ///          SnoId – Product ID
    ///          CPQSNO – Customer S/N
    ///          Tp – 'BTLoc'
    ///          Sno – Location（From UI）
    ///          Status – 'In'
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
    ///         Pak_Btlocmas
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         Product
    /// </para> 
    /// </remarks>
    public partial class InsertSnoDetBTLoc : BaseActivity
    {

        /// <summary>
        /// constructor
        /// </summary>
        public InsertSnoDetBTLoc()
        {
            InitializeComponent();
        }

        /// <summary>
        /// InsertSnoDetBTLoc
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            //从Session里取得Product对象
            Product CurrentProduct = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);
            string this_line = (string)CurrentSession.GetValue("_product_line");

            //从Session里取得PakBtLocMasInfo对象
            PakBtLocMasInfo CurrentLocation = (PakBtLocMasInfo)CurrentSession.GetValue(Session.SessionKeys.WHLocationObj);

            var currentRepository = RepositoryFactory.GetInstance().GetRepository<IPalletRepository, Pallet>();
            SnoDetBtLocInfo newSnoDetBT = new SnoDetBtLocInfo();
            newSnoDetBT.cpqsno = CurrentProduct.CUSTSN;
            newSnoDetBT.editor = Editor;
            newSnoDetBT.pdLine = this_line; //Line;
            newSnoDetBT.sno = CurrentLocation.snoId;
            newSnoDetBT.snoId = CurrentProduct.ProId;
            newSnoDetBT.status = Status;
            newSnoDetBT.tp = Tp;
            newSnoDetBT.cdt= System.DateTime.Now;
            newSnoDetBT.udt = newSnoDetBT.cdt;
            currentRepository.InsertSnoDetBtLocInfoDefered(CurrentSession.UnitOfWork, newSnoDetBT);

            return base.DoExecute(executionContext);
        }

        /// <summary>
        ///  Tp
        /// </summary>
        public static DependencyProperty TpProperty = DependencyProperty.Register("Tp", typeof(string), typeof(InsertSnoDetBTLoc));

        /// <summary>
        /// Tp
        /// </summary>
        [DescriptionAttribute("TpProperty")]
        [CategoryAttribute("InArguments of InsertSnoDetBTLoc")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        [DefaultValue("BTLoc")]
        public string Tp
        {
            get
            {
                return ((string)(base.GetValue(TpProperty)));
            }
            set
            {
                base.SetValue(TpProperty, value);
            }
        }

        /// <summary>
        /// Status
        /// </summary>
        public static DependencyProperty StatusProperty = DependencyProperty.Register("Status", typeof(string), typeof(InsertSnoDetBTLoc));

        /// <summary>
        /// Status
        /// </summary>
        [DescriptionAttribute("StatusProperty")]
        [CategoryAttribute("InArguments of InsertSnoDetBTLoc")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        [DefaultValue("In")]
        public string Status
        {
            get
            {
                return ((string)(base.GetValue(StatusProperty)));
            }
            set
            {
                base.SetValue(StatusProperty, value);
            }
        }
    }
}
