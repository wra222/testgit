// INVENTEC corporation (c)2011 all rights reserved. 
// Description: PODLabelCheck
//                    
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2012-07-31   itc000052                      create
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
using IMES.FisObject.PAK.DN;
using IMES.DataModel;
using IMES.FisObject.Common.Part;

namespace IMES.Activity
{
    /// <summary>
    ///   PODLabelCheck
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///         Assign WH Location
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
    public partial class PODLabelCheck : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public PODLabelCheck()
        {
            InitializeComponent();
        }

        /// <summary>
        /// PODLabelCheck
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {

            IProductRepository iproductRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            Product currentProduct = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);

            //  Print =====如果IMES_PAK..PODLabelPart中有维护PartNo等于Model的前几位字符的记录 and (Model的第7位不是数字)，则需要列印POD Label====
            IPartRepository ipartRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            IList<PODLabelPartDef> podLabelPartLst = new List<PODLabelPartDef>();


            podLabelPartLst = ipartRepository.GetPodLabelPartListByPartNoLike(currentProduct.Model);

            if (podLabelPartLst.Count > 0)
            {
                string number = "0123456789";
                string modelbit = currentProduct.Model.Substring(6, 1);
                if (!number.Contains(modelbit)) //Model的第7位不是数字
                {

                    var productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
                    IList<ProductLog> list = new List<ProductLog>();
                    string station = "86";
                    string line = "POD Check";
                    int status = 1;
                    list = productRepository.GetProductLogs(station, status, line, currentProduct.ProId);

                    if (list == null || list.Count == 0)
                    {
                        List<string> errpara = new List<string>();
                        FisException e = new FisException("CHK293", errpara);//Product 需要在Unit Weight 列印POD Label
                        throw e;
                    }
                }
            }
            return base.DoExecute(executionContext);
        }
    }
}
