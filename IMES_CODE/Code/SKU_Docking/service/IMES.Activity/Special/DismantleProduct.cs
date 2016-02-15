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
using IMES.Common;
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
    public partial class DismantleProduct : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public DismantleProduct()
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
           
            //更新PCBStatus信息
            //bug:ITC-1268-0006
            //reason:先进行mb的处理
            //MB则通过Product.PCBID找到PCBNO
            var mbsn = currentProduct.PCBID.Trim();
            if (!string.IsNullOrEmpty(mbsn))
            {
                //MB状态修改为30 站
                IMBRepository mbRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();
                IMB mb = mbRepository.Find(mbsn);
                if (mb == null)
                {
                    var ex1 = new FisException("SFC001", new string[] { mbsn });
                    throw ex1;
                }
                string line = string.IsNullOrEmpty(this.Line) ? mb.MBStatus.Line : this.Line;
                //var mbLog = new MBLog(0, mb.Sn, mb.Model, "30", 1, line, this.Editor, DateTime.Now);
                var mbLog = new MBLog(0, mb.Sn, mb.Model, "DM", 1, line, this.Editor, DateTime.Now);
                mb.AddLog(mbLog);
                //2012/3/15	For MB ：Update PCA..PCBStatus.Station=30->15
                //mb.MBStatus.Station = "30";
                mb.MBStatus.Station = "15";
                //DEBUG:ITC-1360-0105 update status =1
                mb.MBStatus.Status = MBStatusEnum.Pass;
                mb.MBStatus.Editor = this.Editor;
                
                //mb.MBStatus.Udt = DateTime.Now;

                mbRepository.Update(mb, CurrentSession.UnitOfWork);
               
            }
            //DEBUG ITC-1360-0403
            //	如果是VGA 则通过ProductInfo ProductID=PRDID# ValueType=’VGA’找到
            IList<IMES.FisObject.FA.Product.ProductInfo> infos = new List<IMES.FisObject.FA.Product.ProductInfo>();
            infos = currentProduct.ProductInfoes;
            var mbsnforVGA = "";
            foreach (IMES.FisObject.FA.Product.ProductInfo iInfo in infos)
            {
                if (iInfo.InfoType.Trim().ToUpper() == "VGA")
                {
                    mbsnforVGA = iInfo.InfoValue.Trim();
                    break;
                }
            }
            if (!string.IsNullOrEmpty(mbsnforVGA))
            {
                //MB状态修改为30 站
                IMBRepository mbRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();
                IMB mbVGA = mbRepository.Find(mbsnforVGA);
                if (mbVGA == null)
                {
                    var ex1 = new FisException("SFC001", new string[] { mbsnforVGA });
                    throw ex1;
                }
                string line = string.IsNullOrEmpty(this.Line) ? mbVGA.MBStatus.Line : this.Line;
                //var mbLog = new MBLog(0, mb.Sn, mb.Model, "30", 1, line, this.Editor, DateTime.Now);
                var mbLog = new MBLog(0, mbVGA.Sn, mbVGA.Model, "DM", 1, line, this.Editor, DateTime.Now);
                mbVGA.AddLog(mbLog);
                //2012/3/15	For VGA ：Update PCA..PCBStatus.Station=30
                mbVGA.MBStatus.Station = "30";
                //DEBUG:ITC-1360-0105 update status =1
                mbVGA.MBStatus.Status = MBStatusEnum.Pass;
                mbVGA.MBStatus.Editor = this.Editor;

                //mb.MBStatus.Udt = DateTime.Now;

                mbRepository.Update(mbVGA, CurrentSession.UnitOfWork);

            }

            // mantis 538
            Session session = CurrentSession;
            ActivityCommonImpl.Instance.Material.UpdateMaterialCpuStatus(currentProduct.CVSN, "FA Dismantle", "Dismantle", false, false, ref session);

            //更新Product表，将初始化Product表的剩余字段(供travel card用的)字段保留，其余都清空
            //MO,Model,ProductID,ECR字段不清空 
            //	Update set is null Product.PCBID/PCBModel/MBECR /MAC /CVSN
            currentProduct.PCBID = "";
            currentProduct.PCBModel = "";
            currentProduct.MBECR = "";
            currentProduct.MAC = "";
            //new del currentProduct.UUID = "";
            currentProduct.CVSN = "";
            currentProduct.CUSTSN = "";
            //currentProduct.BIOS = ""; why?
            //currentProduct.IMGVER = "";
            //currentProduct.WMAC = "";
            //currentProduct.IMEI = "";
            //currentProduct.MEID = "";
            //currentProduct.ICCID = "";
            //currentProduct.COAID = "";
            //new del currentProduct.PizzaID = "";
            //new del currentProduct.UnitWeight = 0.0M;
            //new del currentProduct.CartonSN = "";
            //new del currentProduct.CartonWeight = 0.0M;
            //new del currentProduct.DeliveryNo = "";
            //new del currentProduct.PalletNo = "";
            //currentProduct.HDVD = "";
            //currentProduct.BLMAC = "";
            //currentProduct.TVTuner = "";
            productRepository.Update(currentProduct, CurrentSession.UnitOfWork);
            //B.删除Product_Part表
            productRepository.DeleteProductPartByProIdDefered(CurrentSession.UnitOfWork,currentProduct.ProId);

            //C.删除ProductInfo表
            productRepository.DeleteProductInfoByProIdDefered(CurrentSession.UnitOfWork, currentProduct.ProId);

            return base.DoExecute(executionContext);
        }
    }
}
