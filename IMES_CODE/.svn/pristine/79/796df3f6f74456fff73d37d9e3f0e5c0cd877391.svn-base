// INVENTEC corporation (c)2011 all rights reserved. 
// Description: Jamestown RemoveKPCT
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 
// Known issues:
using System.Workflow.ComponentModel;
using IMES.DataModel;
using IMES.FisObject.Common.Part;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.Generic;
using IMES.FisObject.PAK.DN;

namespace IMES.Activity
{
    /// <summary>
    /// Jamestown RemoveKPCT
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      Jamestown RemoveKPCT
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///        
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Product
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
    ///         none
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///              IProductRepository
    ///              
    /// </para> 
    /// </remarks>
    public partial class RemoveKPCT_JS : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public RemoveKPCT_JS()
        {
            InitializeComponent();
        }
        
        private IList<ProductPart> GetNowParts(IProduct currentProduct, string keyConstType)
        {
            IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            IPartRepository partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();

            List<ProductPart> nowParts = new List<ProductPart>();
            List<string> errpara = new List<string>();
            IList<ConstValueInfo> cvInfo = new List<ConstValueInfo>();

            ConstValueInfo cvCond = new ConstValueInfo();
            cvCond.type = keyConstType;
            cvCond.name = currentProduct.Model;
            cvInfo = partRepository.GetConstValueInfoList(cvCond);

            if (cvInfo == null || cvInfo.Count == 0)
            {
                cvCond = new ConstValueInfo();
                cvCond.type = keyConstType;
                cvCond.name = currentProduct.Family;
                cvInfo = partRepository.GetConstValueInfoList(cvCond);
            }
            if (cvInfo == null || cvInfo.Count == 0)
            {
                //throw new FisException("CHK1026", errpara); // 此机器没有要解的料
				return nowParts;
            }

            ConstValueInfo tmp = cvInfo[0];
            string[] wantRemovePartTypes = tmp.value.Split(',');
            foreach (string wantRemovePartType in wantRemovePartTypes)
            {
                ProductPart cond = new ProductPart();
                cond.ProductID = currentProduct.ProId;
                cond.CheckItemType = wantRemovePartType;
                IList<ProductPart> list = productRepository.GetProductPartList(cond);
                //if (list == null || list.Count == 0)
                //{
                //    errpara.Add(wantRemovePartType);
                //    throw new FisException("CHK1027", errpara); // 该机器没有结合@KP，不能过此站
                //}
                if (list != null)
                    nowParts.AddRange(list);
            }
            return nowParts;
        }
        
        /// <summary>
        /// RemoveKPCT
        /// </summary>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            List<string> errpara = new List<string>();
            
            var currentProduct = (IProduct)CurrentSession.GetValue(Session.SessionKeys.Product);
            if (null == currentProduct)
            {
                errpara.Add("");
                throw new FisException("SFC002", errpara);
            }

            IList<ProductPart> nowParts = GetNowParts(currentProduct, "JSRemoveKP");
            
			if ((nowParts != null) && (nowParts.Count > 0)) {
				ProductPart neq = new ProductPart();
				string productID = currentProduct.ProId;
				foreach (ProductPart p in nowParts)
				{
					ProductPart eq = new ProductPart();
					eq.ProductID = productID;
					eq.PartID = p.PartID;
					productRepository.BackUpProductPartDefered(CurrentSession.UnitOfWork, this.Editor, eq, neq);

					productRepository.DeleteProductPartByPartNoDefered(CurrentSession.UnitOfWork, productID, p.PartID);
				}
			}
            
            return base.DoExecute(executionContext);
        }
    }
}
