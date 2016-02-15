// INVENTEC corporation (c)2011 all rights reserved. 
// Description:  删除旧的绑定关系
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-12-29   itc202017                 create
// Known issues:
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Workflow.ComponentModel;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PAK.Pizza;
using IMES.FisObject.PCA.MB;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.DataModel;

namespace IMES.Activity
{
    /// <summary>
    /// 删除旧的绑定关系
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      应用于ChangeKeyParts
    /// </para>
    /// <para>
    /// 删除旧的绑定关系
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.Product,Session.MB,Session.Pizza
    ///         Session.SessionBOM
    ///         
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
    /// 相关FisObject:
    ///              IProductRepository
    ///              IProduct
    ///              ProductPart
    /// </para> 
    /// </remarks>
    public partial class UnBindParts : BaseActivity
    {
        /// <summary>
        /// 
        /// </summary>
        public UnBindParts()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 删除旧的绑定关系
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            /*
             * Answer to: ITC-1360-0561
             * Description: Call ProductRepository.Update() to delete records from product_part.
            */
            IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            IMBRepository mbRep = RepositoryFactory.GetInstance().GetRepository<IMBRepository>();

            IProduct currentProduct = (IProduct)CurrentSession.GetValue(Session.SessionKeys.Product);
            string kpType= CurrentSession.GetValue(Session.SessionKeys.KPType) as string;
            currentProduct.RemovePartsByType(kpType);

            productRepository.BackUpProductPartByCheckItemType(currentProduct.ProId, this.Editor,
                                                                                                   kpType);

            productRepository.Update(currentProduct, CurrentSession.UnitOfWork);

            //Need Change PCBStatus to 15A station (FA Dismantle MB Station)
            if (kpType == "MB" && !string.IsNullOrEmpty(currentProduct.PCBID) )
            {
                IList<string> unpackMBSnList = new List<string>() { currentProduct.PCBID };

                IList<TbProductStatus> mbStatusList = mbRep.GetMBStatus(unpackMBSnList);
                mbRep.UpdatePCBPreStationDefered(CurrentSession.UnitOfWork, mbStatusList);
                mbRep.UpdatePCBStatusByMultiMBDefered(CurrentSession.UnitOfWork,
                                                                                    unpackMBSnList,
                                                                                    "15A",
                                                                                    1,
                                                                                    mbStatusList[0].Line,
                                                                                    this.Editor);
                mbRep.WritePCBLogByMultiMBDefered(CurrentSession.UnitOfWork,
                                                                                    unpackMBSnList,
                                                                                    "15A",
                                                                                    1,
                                                                                    mbStatusList[0].Line,
                                                                                    this.Editor);
            }

            return base.DoExecute(executionContext);
        }
    }
}
