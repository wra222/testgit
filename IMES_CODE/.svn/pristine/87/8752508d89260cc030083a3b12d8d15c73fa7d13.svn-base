/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: Dismantle KP
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 
 * Known issues:Any restrictions about this file 
 */


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
using IMES.FisObject.Common .Process;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.Generic;
using IMES.FisObject.Common.Station;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PCA.MB;
using IMES.FisObject.Common;
using IMES.FisObject.Common.CheckItem;
using IMES.FisObject.Common.Part;
using IMES.DataModel;
using IMES.Common;

namespace IMES.Activity
{
    /// <summary>
    /// Dismantle KP处理
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
    /// DismantleKP处理
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
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
    /// 根据类型更新对应表
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///      IProductRepository 
    /// </para> 
    /// </remarks>
    public partial class DismantleKP: BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public DismantleKP()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 根据Product，删除 ProductInfo表中的IMEI数据
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            List<string> erpara = new List<string>();
            string sKeyParts = (string)CurrentSession.GetValue(Session.SessionKeys.KPType);
            string sReturnStation = (string)CurrentSession.GetValue(Session.SessionKeys.ReturnStation);

            var currentProduct = (IProduct)CurrentSession.GetValue(Session.SessionKeys.Product);
            IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
                       
            IPartRepository partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();

            //Update [ForceNWC]  [ForceNWC]=ReturnStation# [PreStation]= 'DM' Where [ProductID]= ProductID# 
            //DEBUG ITC-1360-0405
            //? DELETE  productRepository.DeleteProductInfoByProIdDefered(CurrentSession.UnitOfWork, currentProduct.ProId);
            //ADD:如果ForceNWC不存在此ProductID的记录，需要执行Insert ForceNWC
            //Modify UC: [PreStation]= 'DM'->38 -2012/03/09
            ForceNWCInfo cond = new ForceNWCInfo();
            cond.productID = currentProduct.ProId;
            bool bExist = false;
            bExist = partRepository.CheckExistForceNWC(cond);
            if (bExist == true)
            {
                partRepository.UpdateForceNWCByProductIDDefered(CurrentSession.UnitOfWork, sReturnStation, "38", currentProduct.ProId);
            }
            else
            {
                ForceNWCInfo item = new ForceNWCInfo();
                item.editor = this.Editor;
                item.forceNWC = sReturnStation;
                item.preStation = "38";
                item.productID = currentProduct.ProId;
                partRepository.InsertForceNWCDefered(CurrentSession.UnitOfWork, item);
            }

            string Dismantletype = CurrentSession.GetValue("Dismantletype") as string;
            if (Dismantletype == "KP" && sKeyParts.ToUpper() == "CPU")
            {
                // mantis 538
				Session session = CurrentSession;
                //if (ActivityCommonImpl.Instance.NeedCheckMaterialCpuStatus(currentProduct, currentProduct.CVSN, ref session))
                //{
                    ActivityCommonImpl.Instance.Material.UpdateMaterialCpuStatus(currentProduct.CVSN, "FA Dismantle", "Dismantle", true, false, ref session);
                //}
            }
            
            //delete 相关信息Delete from Product_Part where ProductID=productID# and PartType=KP#
            currentProduct.RemovePartsByType(sKeyParts);

            //ITC-1360-0124 add update method-->Delete from Product_Part where ProductID=productID# and PartType=KP#
            productRepository.Update(currentProduct, CurrentSession.UnitOfWork);

            return base.DoExecute(executionContext);
        }
    }
}
