// INVENTEC corporation (c)2011 all rights reserved. 
// Description: 根据DeliveryNo号码，UnPack属于DeliveryNo的所有Product
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-11-21                   create
// Known issues:
using System.Workflow.ComponentModel;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.PAK.Pizza;
using System.Collections.Generic;
using IMES.FisObject.Common.FisBOM;
using carton = IMES.FisObject.PAK.CartonSSCC;
using IMES.DataModel;
using IMES.FisObject.PAK.DN;

namespace IMES.Activity
{
    /// <summary>
    /// 用于UnPack属于DeliveryNo的所有Product
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      CI-MES12-SPEC-PAK-UC Unpack
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.从Session中获取DeliveryNo，调用ProductRepository的Update方法
    ///           update Product set DeliveryNo='',PalletNo='',CartonSN='',CartonWeight=0.0
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.DeliveryNo
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
    ///         更新Product
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///              IProductRepository
    ///              
    /// </para> 
    /// </remarks>
    public partial class UnPackCarton : BaseActivity
	{
        /// <summary>
        /// 构造函数
        /// </summary>
        public UnPackCarton()
		{
			InitializeComponent();
		}


        /// <summary>
        /// 执行根据DeliveryNo修改所有属于该DeliveryNo的Product状态的操作
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            string CartonNo = (string)CurrentSession.GetValue(Session.SessionKeys.Carton);
            carton.ICartonSSCCRepository cartRep = RepositoryFactory.GetInstance().GetRepository<carton.ICartonSSCCRepository, IMES.FisObject.PAK.CartonSSCC.CartonSSCC>();
            IDeliveryRepository DeliveryRepository = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
            IProductRepository repProduct = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
    
            IList<IProduct> lstProduct = repProduct.GetProductListByCartonNo(CartonNo);

            CartonInfoInfo condition = new CartonInfoInfo();
            condition.cartonNo = CartonNo;
            cartRep.DeleteCartonInfo(condition);
            DeliveryRepository.UpdateDeliveryForStatusChange(Editor,  CartonNo);
            //
            CartonStatusInfo setValue = new CartonStatusInfo();
            CartonStatusInfo status_condition = new CartonStatusInfo();
            setValue.cartonNo = CartonNo;
            setValue.status = 1;
            setValue.editor = Editor;
            setValue.station = Station;

            status_condition.cartonNo = CartonNo; 
            cartRep.UpdateCartonStatus( setValue,  status_condition);

           
            ///////////////
            CartonLogInfo item = new CartonLogInfo();
            item.cartonNo = CartonNo;
            item.editor = Editor;
            item.line = Line;
            item.station = Station;
            item.status = 1;
            cartRep.AddCartonLogInfo(item);
            
           //////////////////

            CurrentSession.AddValue(Session.SessionKeys.ProdList, lstProduct);
            CurrentSession.AddValue(Session.SessionKeys.DnIndex, 0);
            CurrentSession.AddValue(Session.SessionKeys.DnCount, lstProduct.Count);

            return base.DoExecute(executionContext);
        }
	}
}
