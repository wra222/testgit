/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: Dismantle product
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2009-12-22   207013             Create 
 * 2011-03-10   Lucy Liu           Modify:根据BN UC修改
 * 2011-04-02   Lucy Liu           Modify:ITC-1268-0006
 * 2011-06-13   Lucy Liu           Modify:ITC-1268-0362

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
namespace IMES.Activity
{
    /// <summary>
    /// 解除绑定关系
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
    /// 根据Product，解除绑定关系
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
    ///      IMBRepository
    /// </para> 
    /// </remarks>
    public partial class DismantleProductforDocking : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public DismantleProductforDocking()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 找到DISMANTLE Process，得到对应的Release Type，解除绑定关系
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            List<string> erpara = new List<string>();
            var currentProduct = (IProduct)CurrentSession.GetValue(Session.SessionKeys.Product);
            IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
          
			//若Product.PCBID不为空，则Update PCBStatus Set Station=’31’ where PCBNo = Product.PCBID 注意，不更新Line
			//Insert PCBLog 若Product.PCBID不为空，则插入PCBLog(Station=’38’ and Status=1 and Line = PCBStatus.Line)
            var mbsn = currentProduct.PCBID.Trim();
            if (!string.IsNullOrEmpty(mbsn))
            {
                IMBRepository mbRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();
                IMB mb = mbRepository.Find(mbsn);
                if (mb == null)
                {
                    var ex1 = new FisException("SFC001", new string[] { mbsn });
                    throw ex1;
                }
                string line = mb.MBStatus.Line;//string.IsNullOrEmpty(this.Line) ? mb.MBStatus.Line : this.Line;
                //var mbLog = new MBLog(0, mb.Sn, mb.Model, "30", 1, line, this.Editor, DateTime.Now);
                var mbLog = new MBLog(0, mb.Sn, mb.Model, "38", 1, line, this.Editor, DateTime.Now);
                mb.AddLog(mbLog);
                //2012/3/15	For MB ：Update PCA..PCBStatus.Station=30->15
                //mb.MBStatus.Station = "30";
                mb.MBStatus.Station = "31";
                //DEBUG:ITC-1360-0105 update status =1  ???????UC??
                mb.MBStatus.Status = MBStatusEnum.Pass;
                mb.MBStatus.Editor = this.Editor;
                mb.MBStatus.Udt = DateTime.Now;  //update????
                mbRepository.Update(mb, CurrentSession.UnitOfWork);
               
            }
		    //Update Product Update PCBID, PCBModel, MAC, UUID, MBECR, CVSN, CUSTSN, ECR 为空
            currentProduct.PCBID = "";
            currentProduct.PCBModel = "";
            currentProduct.MAC = "";
			currentProduct.UUID ="";
			currentProduct.MBECR = "";
            //new del currentProduct.UUID = "";
            currentProduct.CVSN = "";
            currentProduct.CUSTSN = "";
			currentProduct.ECR = "";
			productRepository.Update(currentProduct, CurrentSession.UnitOfWork);
            //B.删除Product_Part表
            productRepository.DeleteProductPartByProIdDefered(CurrentSession.UnitOfWork,currentProduct.ProId);

            //C.删除ProductInfo表
            productRepository.DeleteProductInfoByProIdDefered(CurrentSession.UnitOfWork, currentProduct.ProId);

            return base.DoExecute(executionContext);
        }
    }
}
