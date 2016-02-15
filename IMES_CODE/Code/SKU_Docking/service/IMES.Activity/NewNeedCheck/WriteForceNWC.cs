/*
 * INVENTEC corporation (c)2011 all rights reserved. 
 * Description:Update/Insert ForceNWC.
 *                 
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2011-12-20  itc202017             Create
 * Known issues:
*/
using System.Collections.Generic;
using System.Workflow.ComponentModel;
using IMES.FisObject.Common.Part;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure;
using IMES.DataModel;
using System.ComponentModel;
namespace IMES.Activity
{
    /// <summary>
    /// Update/Insert ForceNWC.
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      CI-MES12-SPEC-FA-UC Change Key Parts.docx
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///    详见UC
    /// </para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.SessionKeys.Product
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         无
    /// </para> 
    ///<para> 
    /// 输出：
    ///         Session.SessionKeys.RandomInspectionStation
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///         无
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         IProduct
    ///         IFamilyRepository
    ///         IModelRepository
    /// </para> 
    /// </remarks>
    public partial class WriteForceNWC : BaseActivity
    {

        /// <summary>
        ///  PreStation
        /// </summary>
        public static DependencyProperty PreStationProperty = DependencyProperty.Register("PreStation", typeof(string), typeof(WriteForceNWC), new PropertyMetadata("RK"));

        /// <summary>
        /// PreStation
        /// </summary>
        [DescriptionAttribute("PreStationProperty")]
        [CategoryAttribute("InArguments of WriteForceNWC")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
      
        public string PreStation
        {
            get
            {
                return ((string)(base.GetValue(PreStationProperty)));
            }
            set
            {
                base.SetValue(PreStationProperty, value);
            }
        }


        /// <summary>
        ///  PreStation
        /// </summary>
        public static DependencyProperty IsClearForceNWCProperty = DependencyProperty.Register("IsClearForceNWC", typeof(bool), typeof(WriteForceNWC), new PropertyMetadata(false));

        /// <summary>
        /// PreStation
        /// </summary>
        [DescriptionAttribute("IsClearForceNWCProperty")]
        [CategoryAttribute("InArguments of WriteForceNWC")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]

        public bool IsClearForceNWC
        {
            get
            {
                return ((bool)(base.GetValue(IsClearForceNWCProperty)));
            }
            set
            {
                base.SetValue(IsClearForceNWCProperty, value);
            }
        }


        ///<summary>
        ///</summary>
        public WriteForceNWC()
        {
            InitializeComponent();
        }
        

       

	


        /// <summary>
        /// Update/Insert ForceNWC.
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            IPartRepository partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            IProduct currentProduct = ((Product)CurrentSession.GetValue(Session.SessionKeys.Product));
            string retWC = (string)CurrentSession.GetValue(Session.SessionKeys.ReturnStation);
            
            ForceNWCInfo item = new ForceNWCInfo();
            item.editor = Editor;
            item.forceNWC = retWC;
            //item.preStation = "RK";
            item.preStation = this.PreStation.Trim();

            /*
             * Answer to: ITC-1360-1106
             * Description: Use CheckExistForceNWC to check if record exists.
             */
            ForceNWCInfo cond = new ForceNWCInfo();
            cond.productID = currentProduct.ProId;
            if (IsClearForceNWC)
            {
                item.forceNWC = "";
                item.preStation = "";
                cond.forceNWC = this.Station;
                cond.preStation = (string)CurrentSession.GetValue("ForceNWCPreStation");
                partRepository.UpdateForceNWCDefered(CurrentSession.UnitOfWork, item, cond);
            }
            else
            {
              
                if (partRepository.CheckExistForceNWC(cond))
                {
                    partRepository.UpdateForceNWCDefered(CurrentSession.UnitOfWork, item, cond);
                }
                else
                {
                    item.productID = currentProduct.ProId;
                    partRepository.InsertForceNWCDefered(CurrentSession.UnitOfWork, item);
                }
            
            }
            return base.DoExecute(executionContext);
        }
    }
}
