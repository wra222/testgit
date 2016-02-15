// INVENTEC corporation (c)2011 all rights reserved. 
// Description: 判断当前Product的DeliveryNo的InfoType= CustPo / IECSo对应的InfoValue属性是否全
//                   
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-10-26   Kerwin                       create
// Known issues:
using System;
using System.ComponentModel;
using System.ComponentModel.Design;
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
using IMES.FisObject.FA.Product;

using IMES.FisObject.PAK.COA;
using IMES.Infrastructure;
using IMES.Infrastructure.Repository.PAK;
//using IMES.Infrastructure.Repository.PAK.COARepository;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.Activity
{

    /// <summary>
    /// 判断当前Product的DeliveryNo的InfoType= CustPo / IECSo对应的InfoValue属性是否全
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      CI-MES12-SPEC-PAK-UC Asset Tag Label Print
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.判断当前Product的Product.DeliveryNo 在DeliveryInfo分别查找InfoType= CustPo / IECSo对应的InfoValue，
    ///           其中某一个得不到时，都需要报错:DN資料不全CustPo/IECSo,請檢查一下船務
    ///
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    ///                     PAK007
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
    ///         无
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         Product
    ///         
    /// </para> 
    /// </remarks>
    public partial class CheckCOANo : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CheckCOANo()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 判断当前Product的Product.DeliveryNo 在DeliveryInfo分别查找InfoType= CustPo / IECSo对应的InfoValue，
        /// 其中某一个得不到时，都需要报错:DN資料不全CustPo/IECSo,請檢查一下船務
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
#if false
            return base.DoExecute(executionContext);
#else
            try
            {
                string CurrentCOASN = (string)CurrentSession.GetValue(Session.SessionKeys.COASN);
                IProductRepository productRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
                ICOAStatusRepository coaStatusRepository = RepositoryFactory.GetInstance().GetRepository<ICOAStatusRepository, COAStatus>();
                /* 1）所输入COA No在COAStatus数据表中存在。否则提示“无此COA记录!”
                 */
                COAStatus status = coaStatusRepository.Find(CurrentCOASN) ;
                if (status == null)
                {
                    //This COA does not exist!
                    FisException ex = new FisException("CHK235");
                    throw ex;
                }

                /* 2）COAStatus.Status必须是‘A1’。否则提示“仅A1状态的COA可以被移除！
                      该COA的当前状态是”+COAStatus.Status+“。”
                 */
                if (!status.Status.Equals("A1"))
                {
                    // Only can remove a COA whose status is 'A1', current COA's status is %1
                    FisException ex = new FisException("CHK236", new string[] { status.Status });
                    throw ex;
                }

                /* 
                 *  Attention: now I think status.IECPN 就是 product 的 SN
                 *             所以，从它来得到 product 对象
                 *             
                 * 3）此COA不可已经与机器绑定并且该机器当前在“85”或“99”站。否则提示“机器已上栈板或已经出货 !！”
                 * 
                 *   If exists (select a.ProductID from Product a (nolock), 
                 *   Product_Part b (nolock), ProductStatus c (nolock) 
                 *   where a.ProductID=b.ProductID and b.PartSn=@coano and 
                 *   a.ProductID=c.ProductID and (c.Station=’85’ or c.Station=’99’))
                 * 
                 * IProductRepository::
                 *   bool CheckExistProductByPartSnAndStations(string partSn, string[] stations);
                 * 
                 */
                string partSn = CurrentCOASN;
                string[] stations = {"85", "99"};
                if (false == productRep.CheckExistProductByPartSnAndStations(partSn, stations))
                {
                    FisException ex = new FisException("CHK237"); //"Machine has been in stack or out!"
                    throw ex;
                }
                /*
                string product_sn = status.IECPN;
                var productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
                IProduct currentProduct = productRepository.GetProductByCustomSn(product_sn);

                if (currentProduct != null)
                {
                    // now we get the product.
                    if ((currentProduct.COAID == CurrentCOASN) && (string.Equals(currentProduct.Status.StationId, "99") || string.Equals(currentProduct.Status.StationId, "99" )))
                    {
                        FisException ex = new FisException("SFC003", new string[] { "Machine has been in stack or out!" });
                        throw ex;
                    }
                }
                */
                
                IList<string> COANumberList = (IList<string>)CurrentSession.GetValue("_COANumberList");
                COANumberList.Add(CurrentCOASN);
            }
            catch (FisException ex)
            {
                //ex.stopWF = false;
                throw ex;
            }

            return base.DoExecute(executionContext);
#endif
        }

    }
}

