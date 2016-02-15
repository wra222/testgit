// INVENTEC corporation (c)2011 all rights reserved. 
// Description:  
// UI:CI-MES12-SPEC-PAK-UI PD PA Label 2.docx
// UC:CI-MES12-SPEC-PAK-UC PD PA Label 2.docx                           
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-12-26   Du Xuan (itc98066)          create
// ITC-1360-0839 改为查找Delivery 出货时间
// ITC-1413-0009 新增CDSI 机器Assign Delivery的特殊要求
// Known issues:
using System;
using System.Data;
using System.Linq;
using System.Workflow.ComponentModel;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Model;
using IMES.FisObject.Common.Part;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PAK.DN;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.PAK.Pallet;
using IMES.DataModel;
using System.Collections.Generic;
using IMES.Infrastructure.Extend;
using System.ComponentModel;
namespace IMES.Activity
{
    /// <summary>
    ///
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
    ///         Delivery 分配
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
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
    ///          
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///              IProductRepository
    ///              productId
    /// </para> 
    /// </remarks>
    public partial class AssignDeliveryForTris : BaseActivity
    {
        /// <summary>
        /// constructor
        /// </summary>
        public AssignDeliveryForTris()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 禁用時要停止workflow
        /// </summary>
        public static DependencyProperty IsStopWFProperty = DependencyProperty.Register("IsStopWF", typeof(bool), typeof(AssignDeliveryForTris), new PropertyMetadata(false));
        /// <summary>
        ///  禁用時要停止workflow
        /// </summary>
        [DescriptionAttribute("IsStopWF")]
        [CategoryAttribute("IsStopWF")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public bool IsStopWF
        {
            get
            {
                return ((bool)(base.GetValue(AssignDeliveryForTris.IsStopWFProperty)));
            }
            set
            {
                base.SetValue(AssignDeliveryForTris.IsStopWFProperty, value);
            }
        }

        /// <summary>
        /// Delivery 分配原则
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            try
            {
                List<string> errpara = new List<string>();
                Product curProduct = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);
                IProductRepository productRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
                IDeliveryRepository deliveryRep = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
                if (!string.IsNullOrEmpty(curProduct.DeliveryNo))
                {
                    throw new Exception("This Product has combined DN!");
                }
                string assignDnNo = CurrentSession.GetValue("TRIS_DN").ToString();
                if (string.IsNullOrEmpty(assignDnNo))
                {
                    FisException fe = new FisException("PAK103", new string[] { });   //没找到可分配的delivery
                    fe.stopWF = IsStopWF;
                    throw fe;
                   
                }
                int combineQty = int.Parse(CurrentSession.GetValue("TRIS_CombineQty").ToString());
                Delivery assignDelivery = deliveryRep.Find(assignDnNo);
           //     productRep.BindDNDefered(CurrentSession.UnitOfWork, assignDnNo, new List<string> { curProduct.ProId });
            //productRep.BindDN(assignDnNo, new List<string> { curProduct.ProId }, assignDelivery.Qty - combineQty);
                //UPDATE Product SET DeliveryNo='4108782763000016'
                //    WHERE ((SELECT COUNT(1) FROM Product 
                //   WHERE DeliveryNo='4108782763000016')<=1) 
                int dvQty = productRep.GetCombinedQtyByDN(assignDnNo);
                curProduct.DeliveryNo = assignDnNo;
                curProduct.CartonSN = CurrentSession.GetValue(ExtendSession.SessionKeys.CartonSN).ToString();

                IList<Delivery> dnlist = (IList<Delivery>)CurrentSession.GetValue(Session.SessionKeys.DeliveryList);
                if (dnlist == null)
                {
                    dnlist = new List<Delivery>();
                    CurrentSession.AddValue(Session.SessionKeys.DeliveryList, dnlist);

                }
                if (assignDelivery.Qty == (dvQty + combineQty+1))
                {
                    assignDelivery.Status = "87";
                   // deliveryRep.Update(assignDelivery, CurrentSession.UnitOfWork);
                    dnlist.Add(assignDelivery);
                }
                IList<IProduct> prodList = CurrentSession.GetValue(Session.SessionKeys.ProdList) as IList<IProduct>;
                IList<string> prodIdList = CurrentSession.GetValue(Session.SessionKeys.NewScanedProductIDList) as IList<string>;
                if (prodList == null)
                {
                    prodList = new List<IProduct>();
                    prodIdList = new List<string>();
                    CurrentSession.AddValue(Session.SessionKeys.ProdList, prodList);
                    CurrentSession.AddValue(Session.SessionKeys.NewScanedProductIDList, prodIdList);
                }
                prodList.Add(curProduct);
                prodIdList.Add(curProduct.ProId);
            }
            catch (FisException fex)
            {
                fex.stopWF = IsStopWF;
                throw fex;
            }
            return base.DoExecute(executionContext);
        }
    }
          
}
