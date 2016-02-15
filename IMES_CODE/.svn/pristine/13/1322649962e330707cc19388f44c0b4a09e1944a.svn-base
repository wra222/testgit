// INVENTEC corporation (c)2012all rights reserved. 
// Description:Combine COA and DN.docx
//             获取Pizza ID           
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2012-02-03   207003                     create
// Known issues:
using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
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
using IMES.Infrastructure;
using IMES.Infrastructure.UnitOfWork;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.NumControl;
using IMES.FisObject.Common.Part;
using IMES.FisObject.Common.Model;
using IMES.FisObject.Common.PrintLog;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PAK;
using IMES.FisObject.PAK.DN;
using IMES.FisObject.PAK.COA;
using IMES.FisObject.PAK.Pizza;
using IMES.DataModel;
using System.Data.SqlClient;
using System.Data;
using IMES.Infrastructure.Repository._Schema;
using IMES.FisObject.Common.MO;


namespace IMES.Activity
{
    /// <summary>
    /// 获取DCode
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      应用于Combine coa and dn
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         获取Pizza ID
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///        Session.SessionKeys.Lines 
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         无
    /// </para> 
    ///<para> 
    /// 输出：
    ///         PizzaID
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///         无
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///              INumControlRepository
    /// </para> 
    /// </remarks>
    public partial class UpdateCOAandDN : BaseActivity
    {
        private static object _syncRoot_GetSeq = new object();

        /// <summary>
        /// constructor
        /// </summary>
        public UpdateCOAandDN()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 获取DCode
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            IProductRepository productRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            IDeliveryRepository deliveryRep = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
            IModelRepository modelRep = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();
            IPizzaRepository pizzaRep = RepositoryFactory.GetInstance().GetRepository<IPizzaRepository, Pizza>();
            ICOAStatusRepository coaRep = RepositoryFactory.GetInstance().GetRepository<ICOAStatusRepository, COAStatus>();
            IPrintLogRepository printLogRep = RepositoryFactory.GetInstance().GetRepository<IPrintLogRepository, PrintLog>();

            Product curProduct = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);
            Delivery curDn = (Delivery)CurrentSession.GetValue(Session.SessionKeys.Delivery);
            string pizzaID = (string)CurrentSession.GetValue(Session.SessionKeys.PizzaID);
            string coaSN = (string)CurrentSession.GetValue(Session.SessionKeys.COASN);
            string curEditor = "";
            string curStation = "";

            if (!string.IsNullOrEmpty(this.Editor))
            {
                curEditor = this.Editor;
            }
            if (!string.IsNullOrEmpty(this.Station))
            {
                curStation = this.Station;
            }

            if (!curProduct.IsBT)
            {
                //Update Product – Combine DN
                //Product.DeliveryNo – Delivery No (from UI)
                bool bindFlag = false;
                curProduct.DeliveryNo = curDn.DeliveryNo;
                Delivery newDn = null;
                IList<string> proList = new List<string>();
                proList.Add(curProduct.ProId);
                bindFlag = productRep.BindDN(curDn.DeliveryNo, proList, curDn.Qty);
                while (!bindFlag)
                {
                    newDn = GetNextDelivery(curProduct);
                    if (newDn == null)
                    {
                        FisException fe = new FisException("PAK103", new string[] { });   //没找到可分配的delivery
                        throw fe;
                    }
                    else
                    {
                        bindFlag = productRep.BindDN(newDn.DeliveryNo, proList, newDn.Qty);
                    }
                }
                if (newDn != null)
                {
                    curDn = newDn;
                    curProduct.DeliveryNo = curDn.DeliveryNo;
                    CurrentSession.AddValue(Session.SessionKeys.Delivery, curDn);
                }

                int dvQty = productRep.GetCombinedQtyByDN(curDn.DeliveryNo);
                if (dvQty == curDn.Qty)
                {
                    curDn.Status = "87";
                    deliveryRep.Update(curDn, CurrentSession.UnitOfWork);
                }
            }

            //3.	如果有绑定COA，则需要完成如下操作
            //a)	Insert Product_Part - Combine COA 
            //b)	Update COAStatus - Update COA Status
            //      COAStatus.Status = 'A1'
            //c)	Insert COALog – Insert COA Log
            //      COASN – COA
            //      Line – 当前绑定的Customer S/N
            //      Station – 'A1'
            if (!string.IsNullOrEmpty(coaSN))
            {
                ProductPart coaPart = (ProductPart)CurrentSession.GetValue("COAPart");
                IPart bomPart = (IPart)CurrentSession.GetValue("COABOMPart");
                COAStatus reCOA = coaRep.Find(coaSN);

                IProductPart bindPart = new ProductPart();
                bindPart.ProductID = curProduct.ProId;
                bindPart.PartID = bomPart.PN;
                bindPart.PartSn = coaSN;
                bindPart.Cdt = DateTime.Now;
                bindPart.BomNodeType = "P1";
                bindPart.PartType = bomPart.Type;
                bindPart.CustomerPn = bomPart.CustPn;
                bindPart.Editor = curEditor;
                bindPart.Station = curStation;
                bindPart.CheckItemType = "";
                bindPart.Iecpn = "";
                curProduct.AddPart(bindPart);

                reCOA.Status = "A1";
                reCOA.Editor = curEditor;
                coaRep.UpdateCOAStatusDefered(CurrentSession.UnitOfWork, reCOA);

                COALog newItem = new COALog();
                newItem.COASN = coaSN;
                newItem.LineID = curProduct.CUSTSN;
                newItem.Editor = Editor;
                newItem.StationID = "A1";
                newItem.Tp = "";
                coaRep.InsertCOALogDefered(CurrentSession.UnitOfWork, newItem);
            }

            //7.Insert IMES_PAK..Pizza / IMES_PAK..PizzaStatus / 
            //IMES_PAK..Pizza.MMIID = ''
            //IMES_PAK..PizzaStatus.Station = '00'

            Pizza CurrentPizza = new Pizza();
            PizzaStatus currentPizzaStatus = new PizzaStatus();

            currentPizzaStatus.Editor = this.Editor;
            if (null == this.Line)
            {
                currentPizzaStatus.LineID = "";
            }
            else
            {
                currentPizzaStatus.LineID = this.Line;
            }
            currentPizzaStatus.PizzaID = pizzaID;
            currentPizzaStatus.StationID = "00";

            CurrentPizza.PizzaID = pizzaID;
            CurrentPizza.MMIID = "";
            CurrentPizza.Status = currentPizzaStatus;

            pizzaRep.Add(CurrentPizza, CurrentSession.UnitOfWork);

            //8.Update Product – Combine Pizza Id
            //Product.PizzaID – Pizza ID         
            curProduct.PizzaID = pizzaID;
            productRep.Update(curProduct, CurrentSession.UnitOfWork);

            //Model 的第10，11码是”29” 或者”39” 的产品是出货日本的产品；否则，是非出货日本的产品
            string jpmodel = curProduct.Model.Substring(9, 2);
            bool jpflag = false;

          //  if (jpmodel == "29" || jpmodel == "39")
            if((jpmodel == "29" || jpmodel == "39") && CheckJapanByPart(curProduct.Model))
            {
                jpflag = true;
            }

            //IMES_GetData..PrintLog
            var item = new PrintLog
            {
                Name = "PIZZA Label-1",
                BeginNo = curProduct.CUSTSN,
                EndNo = curProduct.CUSTSN,
                Descr = "PIZZA Label-1",
                Editor = this.Editor
            };

            printLogRep.Add(item, CurrentSession.UnitOfWork);

            //出货日本在列印列印Pizza Label 后，还需要列印Japan Pizza Label 
            if (jpflag)
            {
                var jitem = new PrintLog
                {
                    Name = "PIZZA Label-2",
                    BeginNo = curProduct.CUSTSN,
                    EndNo = curProduct.CUSTSN,
                    Descr = "PIZZA Label-2",
                    Editor = this.Editor
                };
                printLogRep.Add(jitem, CurrentSession.UnitOfWork);
            }
            if (curProduct.IsBT)
            {
                var btitem = new PrintLog
                {
                    Name = "BT COO Label",
                    BeginNo = curProduct.CUSTSN,
                    EndNo = curProduct.CUSTSN,
                    Descr = "BT COO Label",
                    Editor = this.Editor
                };
                printLogRep.Add(btitem, CurrentSession.UnitOfWork);
            }
            CurrentSession.AddValue("JPFlag", jpflag);

            return base.DoExecute(executionContext);
        }

        private Delivery GetNextDelivery(Product curProduct)
        {
            IProductRepository productRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            IDeliveryRepository deliveryRep = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
            IModelRepository modelRep = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();
            IMORepository moRep = RepositoryFactory.GetInstance().GetRepository<IMORepository>();

            Delivery assignDelivery = new Delivery();
            CurrentSession.AddValue("HasDN", "N");
            //CDSI 机器Assign Delivery的特殊要求
            //IList<IMES.FisObject.Common.Model.ModelInfo> GetModelInfoByModelAndName(string model, string name);
            IList<IMES.FisObject.Common.Model.ModelInfo> infoList = modelRep.GetModelInfoByModelAndName(curProduct.Model, "PO");
            Model model = modelRep.Find(curProduct.Model);
            string cdsi = "";
            cdsi = model.GetAttribute("PO");
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
                    return assignDelivery;
                }

                string factoryPo = "";
                //获取Product 结合的Factory Po，对于CDSI 机器如果获取不到结合的Factory Po，
                //需要报告错误：“CDSI AST MISSING DATA!”
                //SELECT @FactoryPo = Sno FROM CDSIAST NOLOCK WHERE SnoId = @ProductId AND Tp = 'FactoryPO'

                CdsiastInfo conf = new CdsiastInfo();
                conf.snoId = curProduct.ProId;
                conf.tp = "FactoryPO";
                IList<CdsiastInfo> cdsiList = productRep.GetCdsiastInfoList(conf);
                /*if (cdsiList.Count == 0)
                {
                    errpara.Add(this.Key);
                    throw new FisException("PAK140", errpara);//“CDSI AST MISSING DATA!”
                }*/
                factoryPo = cdsiList[0].sno;
                IList<Delivery> dnList = deliveryRep.GetDeliveryListByModelPrefix(factoryPo, "PC", 12, "00");

                //IF @Delivery = ''	SELECT 'CDSI 机器，无此PoNo: ' + @FactoryPo + ' 的Delivery!'
                if (dnList.Count == 0)
                {
                    return null;
                }
                assignDelivery = dnList[0];

            }
            else if (curProduct.IsBindedPo)
            {
                if (!string.IsNullOrEmpty(curProduct.DeliveryNo))
                {
                    assignDelivery = deliveryRep.Find(curProduct.DeliveryNo);
                    CurrentSession.AddValue(Session.SessionKeys.Delivery, assignDelivery);
                    return assignDelivery;
                }

                string factoryPo = curProduct.BindPoNo;
                IList<Delivery> dnList = deliveryRep.GetDeliveryListByModelPrefix(factoryPo, "PC", 12, "00");               
                if (dnList.Count == 0)
                {
                    return null;
                }
                assignDelivery = dnList[0];
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
                //Vincent 2015-02-27 過濾綁訂PoDN
                if (dnList != null && dnList.Count > 0)
                {
                    IList<string> bindPoNoList = moRep.GetBindPoNoByModel(curProduct.Model);
                    if (bindPoNoList != null && bindPoNoList.Count > 0)
                    {
                        dnList = dnList.Where(x => !bindPoNoList.Contains(x.PoNo)).ToList();
                    }
                }
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

                    int qty = 0;
                    int packedQty = 0;
                    qty = tmp.Qty;
                    IList<IProduct> productList = new List<IProduct>();
                    productList = productRep.GetProductListByDeliveryNo(tmp.DeliveryNo);
                    packedQty = productList.Count;
                    if (packedQty + 1 > qty)
                    {
                        continue;
                    }

                    assignDelivery = deliveryRep.Find(tmp.DeliveryNo);
                    break;
                }
                if (assignDelivery == null)
                {
                    return null;
                }
            }

            return assignDelivery;
        }

        private bool CheckJapanByPart(string model)
        {
            string strSQL = @"select count(*) from ModelBOM a,
                                                 PartInfo b
                                                  where a.Component=b.PartNo
                                                     and a.Material=@model
                                                     and b.InfoType='JPWRT'
                                                     and b.InfoValue='Y' ";
            SqlParameter paraName = new SqlParameter("@model", SqlDbType.VarChar, 32);
            paraName.Direction = ParameterDirection.Input;
            paraName.Value = model;
            int r = (int)SqlHelper.ExecuteScalar(SqlHelper.ConnectionString_GetData,
                                    System.Data.CommandType.Text,
                                  strSQL, paraName);
            bool b = r == 0 ? false : true;
            return b;
        }

    }
}
