/* INVENTEC corporation (c)2011 all rights reserved. 
 * Description: 更新QCStatus記錄(按照Cdt倒排序找到ProductID对应的最新纪录)
 *                         
 * Update: 
 * Date         Name                         Reason 
 * ==========   =======================      ==========================
 * 2011-12-01   Kerwin                       Create
 * Known issues:
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Workflow.ComponentModel;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Linq;
using System.Linq.Expressions;

namespace IMES.Activity
{
    /// <summary>
    /// 更新QCStatus記錄(按照Cdt倒排序找到ProductID对应的最新纪录)
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///     CI-MES12-SPEC-PAK-UC PAQC Output
    ///     CI-MES12-SPEC-FA-UC PIA Output
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         update QCStatus –按照Cdt倒排序找到ProductID对应的最新纪录
    ///</para> 
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
    ///         无
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///         QCStatus
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         IProduct
    ///         IProductRepository
    /// </para> 
    /// </remarks>
    public partial class UpdateQCStatus : BaseActivity
    {

        /// <summary>
        /// 构造函数
        /// </summary>
        public UpdateQCStatus()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 更新QCStatus記錄(按照Cdt倒排序找到ProductID对应的最新纪录)
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override System.Workflow.ComponentModel.ActivityExecutionStatus DoExecute(System.Workflow.ComponentModel.ActivityExecutionContext executionContext)
        {
            IProduct product = (IProduct)CurrentSession.GetValue(Session.SessionKeys.Product);
            IList<ProductQCStatus> QCStatusList = product.QCStatus;

            if (QCStatusList != null && QCStatusList.Count > 0)
            {
                ProductQCStatus qcStatus = QCStatusList[0];

                if (Type == "PAQC")
                {
                    qcStatus = QCStatusList.FirstOrDefault(status => status.Type == Type && status.Status == "8");
                }
                if (qcStatus == null)
                {
                    throw new FisException("CHK041", new string[]{});
                }

                ProductQCStatus updateQCStatusObj = new ProductQCStatus(qcStatus.ID, qcStatus.ProductID, qcStatus.Type, qcStatus.Line, qcStatus.Model, Convert.ToString(Status, 16).ToUpper(), qcStatus.Editor, qcStatus.Cdt, DateTime.Now);
                if (Type == "EPIA")
                    updateQCStatusObj.Remark = "1";
                else
                    updateQCStatusObj.Remark = qcStatus.Remark;
                
                if(Type =="EPIAOutputForDocking")
                        updateQCStatusObj.Type="PIA";

                product.UpdateQCStatus(updateQCStatusObj);
                //Add by Benson
        
                if (!string.IsNullOrEmpty(ProductAttrName))
                {
                   product.SetAttributeValue(ProductAttrName, Convert.ToString(Status, 16).ToUpper(), Editor, Station, "");

                }
                else if (!string.IsNullOrEmpty(Type))
                {
                    product.SetAttributeValue(Type + "_QCStatus", Convert.ToString(Status, 16).ToUpper(), Editor, Station, "");

                }
            
                //Add by Benson

                IProductRepository ipr = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
                ipr.Update(product, CurrentSession.UnitOfWork);
            }
            else
            {
                throw new FisException("CHK041", new string[] { });
            }

            return base.DoExecute(executionContext);
        }

        /// <summary>
        /// 要更新的状态值
        /// </summary>
        public static DependencyProperty StatusProperty = DependencyProperty.Register("Status", typeof(int), typeof(UpdateQCStatus));

        /// <summary>
        /// 要更新的状态值
        /// </summary>
        [DescriptionAttribute("Status")]
        [CategoryAttribute("Status Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public int Status
        {
            get
            {
                return ((int)(base.GetValue(UpdateQCStatus.StatusProperty)));
            }
            set
            {
                base.SetValue(UpdateQCStatus.StatusProperty, value);
            }
        }

        /// <summary>
        /// 类型值 PAQC or PIA
        /// </summary>
        public static DependencyProperty TypeProperty = DependencyProperty.Register("Type", typeof(string), typeof(UpdateQCStatus));


        /// <summary>
        /// 类型值
        /// </summary>
        [DescriptionAttribute("Type")]
        [CategoryAttribute("Type Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string Type
        {
            get
            {
                return ((string)(base.GetValue(UpdateQCStatus.TypeProperty)));
            }
            set
            {
                base.SetValue(UpdateQCStatus.TypeProperty, value);
            }
        }

        //Add by Benson
        /// <summary>
        /// 类型值 Product Attr Name
        /// </summary>
        public static DependencyProperty ProductAttrNameProperty = DependencyProperty.Register("ProductAttrName", typeof(string), typeof(UpdateQCStatus));


        /// <summary>
        /// 类型值
        /// </summary>
        [DescriptionAttribute("ProductAttrName")]
        [CategoryAttribute("ProductAttrName Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string ProductAttrName
        {
            get
            {
                return ((string)(base.GetValue(UpdateQCStatus.ProductAttrNameProperty)));
            }
            set
            {
                base.SetValue(UpdateQCStatus.ProductAttrNameProperty, value);
            }
        }
        //Add by Benson

    }
}
