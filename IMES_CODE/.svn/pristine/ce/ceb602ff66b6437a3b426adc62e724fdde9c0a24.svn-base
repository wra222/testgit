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
    public partial class AssignDeliveryCoaDn : BaseActivity
    {
        /// <summary>
        /// constructor
        /// </summary>
        public AssignDeliveryCoaDn()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Delivery 分配原则
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {

            List<string> errpara = new List<string>();
            Product curProduct = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);

            IProductRepository productRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            IDeliveryRepository deliveryRep = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
            IModelRepository modelRep = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();

           /*IList<ProductBTInfo> btList = productRep.GetProductBT(curProduct.ProId);
            if (btList.Count > 0 && Station.Trim() == "92")
            {
                return base.DoExecute(executionContext);
            }
            */
            Delivery assignDelivery = new Delivery();
            CurrentSession.AddValue("HasDN", "N");
            //CDSI 机器Assign Delivery的特殊要求
            //IList<IMES.FisObject.Common.Model.ModelInfo> GetModelInfoByModelAndName(string model, string name);
            IList<IMES.FisObject.Common.Model.ModelInfo> infoList = modelRep.GetModelInfoByModelAndName(curProduct.Model, "PO");
            Model model = modelRep.Find(curProduct.Model);
            string cdsi = "";
            cdsi= model.GetAttribute("PO");
            if (cdsi != "Y")
            {
                cdsi = "";
                cdsi = model.GetAttribute("ATSNAV");
                if (!string.IsNullOrEmpty(cdsi))
                {
                    cdsi = "cdsi";
                }
            }
            else
            {
                cdsi = "cdsi";
            }
            CurrentSession.AddValue("CDSI", cdsi);

            if (cdsi == "cdsi")
            {
                if (!string.IsNullOrEmpty(curProduct.DeliveryNo))
                {
                    assignDelivery = deliveryRep.Find(curProduct.DeliveryNo);
                    CurrentSession.AddValue(Session.SessionKeys.Delivery, assignDelivery);
                    CurrentSession.AddValue("HasDN", "Y");
                    return base.DoExecute(executionContext);
                }

                string factoryPo = "";
                //获取Product 结合的Factory Po，对于CDSI 机器如果获取不到结合的Factory Po，
                //需要报告错误：“CDSI AST MISSING DATA!”
                //SELECT @FactoryPo = Sno FROM CDSIAST NOLOCK WHERE SnoId = @ProductId AND Tp = 'FactoryPO'

                CdsiastInfo conf = new CdsiastInfo();
                conf.snoId = curProduct.ProId;
                conf.tp = "FactoryPO";
                IList<CdsiastInfo> cdsiList = productRep.GetCdsiastInfoList(conf);
                if (cdsiList.Count == 0)
                {
                    errpara.Add(this.Key);
                    throw new FisException("PAK140", errpara);//“CDSI AST MISSING DATA!”
                }
                factoryPo = cdsiList[0].sno;
                IList<Delivery> dnList = deliveryRep.GetDeliveryListByModelPrefix(factoryPo, "PC", 12, "00");

                //IF @Delivery = ''	SELECT 'CDSI 机器，无此PoNo: ' + @FactoryPo + ' 的Delivery!'
                if (dnList.Count == 0)
                {
                    errpara.Add(factoryPo);
                    throw new FisException("PAK141", errpara);//'CDSI 机器，无此PoNo: ' + @FactoryPo + ' 的Delivery!'
                }
                assignDelivery = dnList[0];
                curProduct.DeliveryNo = assignDelivery.DeliveryNo;

            }
            else if (!curProduct.IsBT)
            {

                //a)选择的DN需要满足如下要求：
                //Delivery.ShipDate 大于3天前 (例如：当天为2011/9/13，那么获取的DN 的ShipDate 要大于2011/9/10) – 即ShipDate>=convert(char(10),getdate()-3,111)
                //Note：
                //ShipDate 需要转换为YYYY/MM/DD 格式显示
                //Sample: 2009/05/11
                //Delivery.Status = ‘00’
            	//Delivery.Model 长度为12 位
                //Delivery.Model 前两码为’PC’

                //系统自动分配另外一个DN(列表中满足条件的Delivery按照ShipDate,Qty,DeliveryNo 排序取第一个)，

                DNQueryCondition condition = new DNQueryCondition();
                DateTime temp = DateTime.Now;
                temp = temp.AddDays(-3);
                condition.ShipDateFrom = new DateTime(temp.Year, temp.Month, temp.Day, 0, 0, 0, 0);
                condition.Model = curProduct.Model;
                IList<DNForUI> dnList = deliveryRep.GetDNListByConditionWithSorting(condition);
                foreach (DNForUI tmp in dnList)
                {
                    if (tmp.Status != "00")
                    {
                        continue;
                    }
                    if (!(tmp.ModelName.Length == 12 && tmp.ModelName.Substring(0, 2) == "PC"))
                    {
                        continue;
                    }
                    //tmp.Editor = deliveryRep.GetDeliveryInfoValue(tmp.DeliveryNo, "PartNo");//CustomerPN

                    int qty = 0;
                    int packedQty = 0;
                    qty = tmp.Qty;
                    IList<IProduct> productList = new List<IProduct>();
                    productList = productRep.GetProductListByDeliveryNo(tmp.DeliveryNo);
                    packedQty = productList.Count;
                    if (packedQty+1 > qty)
                    {
                        continue;
                    }

                    assignDelivery = deliveryRep.Find(tmp.DeliveryNo);
                    break;
                }
                if (assignDelivery == null)
                {
                    FisException fe = new FisException("PAK103", new string[] { });   //没找到可分配的delivery
                    throw fe;
                }
                curProduct.DeliveryNo = assignDelivery.DeliveryNo;
            }
           
            CurrentSession.AddValue(Session.SessionKeys.Delivery, assignDelivery);

            return base.DoExecute(executionContext);
        }
    }
          
}
