// INVENTEC corporation (c)2011 all rights reserved. 
// Description:To every one in a COANoList, delete bound to Product and COA No, 
//             set status to current product. set status to COA, and insert COA log.
//                   
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-10-26   liuqingbiao                  create
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
using IMES.FisObject.PAK.Pizza;
using IMES.DataModel;
using IMES.FisObject.PAK.COA;
using IMES.Infrastructure;
using IMES.Infrastructure.Repository.PAK;
//using IMES.Infrastructure.Repository.PAK.COARepository;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.Activity
{

    /// <summary>
    /// To every one in a COANoList, delete bound to Product and COA No, set status to current product.
	/// set status to COA, and insert COA log.
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      CI-MES12-SPEC-PAK-COA Removal
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         针对List中的每一COA No
    ///         5．获取与此COA No绑定的Product的ID、CUSTSN,Model对应的HP Pno
    ///         6．解除Product与COA No的绑定：
    ///         delete Product_Part where PartSn=@coano and ProductID=@productID
    ///         设置对应Product的当前状态：
    ///         Update ProductStatus set Station=’69’, Editor=@user, Udt=GetDate where ProducntID=@productID
    ///         设置COA状态：
    ///         Update COAStatus set Line=’REM’，Status=’A2’，Editor=@user, Udt=GetDate where COASN=@coano
    ///         记录COA Log：
    ///         insert COALog values (@coano, 'A2',custsn+'/'+hppno, rtrim(@user), GetDate, ‘COA’)
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
    ///         Session.SessionKeys.COASNList
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
    ///         IProductRepository ICOAStatusRepository
    ///         
    /// </para> 
    /// </remarks>
    public partial class SaveCOANoList : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SaveCOANoList()
        {
            InitializeComponent();
        }

        /// <summary>
        /// To every one in a COANoList, delete bound to Product and COA No, set status to current product.
		/// set status to COA, and insert COA log.
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            string editor = this.Editor;
            string pdLine = this.Line;
            string scrap = (string)CurrentSession.GetValue("scrap");
            string cause = (string)CurrentSession.GetValue("cause");

            List<string> list = (List<string>)CurrentSession.GetValue(Session.SessionKeys.COASNList);
            List<string> Actionlist = (List<string>)CurrentSession.GetValue("ActionList");
            List<string> Causelist = (List<string>)CurrentSession.GetValue("CauseList");
            int listcount = list.Count;
            int i = 0;
            IProductRepository productRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            ICOAStatusRepository coaStatusRep = RepositoryFactory.GetInstance().GetRepository<ICOAStatusRepository, COAStatus>();

            IPizzaRepository repPizza = RepositoryFactory.GetInstance().GetRepository<IPizzaRepository, Pizza>();
          
            try
            {
                foreach (string coaNo in list)
                {
                    bool found = false;
                    string productID = "";
                    string CUSTSN = "";
                    string CustPartNo = "";

                    string pizzaID = "";

                    bool first_in = true;
                    string partSn = coaNo;
                    IMES.FisObject.FA.Product.ProductStatus newStatus = new IMES.FisObject.FA.Product.ProductStatus();
                    IList<string> product_list = new List<string>();
                    IList<ProductAndCustInfo> product_and_cust_info_list = productRep.GetProductAndCustInfoListByPartSn(partSn);
                    foreach (ProductAndCustInfo node in product_and_cust_info_list)
                    {
                        if (first_in)
                        {
                            productID = node.productId;
                            IProduct _product = productRep.FindOneProductWithProductIDOrCustSN(productID);
                            pizzaID = _product.PizzaID;
                            newStatus = _product.Status;
                            CUSTSN = node.custSn;
                            CustPartNo = node.custPno; //_product.ModelObj.CustPN;
                            found = true;
                            first_in = false;
                        }

                        product_list.Add(node.productId);
                        break;
                    }

                    if (found == false)
                    {
                        //FisException _ex = new FisException("CHK238"); //Get COA no bounding product info error!
                        //throw _ex;
                    }
                    /////////////////////////////////////////////////////////////////////////////
                    /* 
                     * delete Pizza_Part where PartSn=@coano and PizzaID=@PizzaId
                     */
                    ////////////////////////////////////////////////////////////////////////////////

                    IList<IProduct> pizza_list = productRep.GetProductInfoListByPizzaPartSn(partSn);
                    foreach (IProduct node in pizza_list)
                    {
                        pizzaID = node.PizzaID;

                        break;
                    }
                    if (pizzaID != "")
                    {
                        PizzaPart delpizza = new PizzaPart();

                        delpizza.PizzaID = pizzaID;
                        delpizza.PartSn = partSn;

                        repPizza.DeletePizzaPartDefered(CurrentSession.UnitOfWork, delpizza);
                    }

                    /*
                     * 
                     * 6．解除Product与COA No的绑定：
                     * delete Product_Part where PartSn=@coano and ProductID=@productID
                     * 设置对应Product的当前状态：
                     * Update ProductStatus set Station=’69’, Editor=@user, Udt=GetDate where ProducntID=@productID
                     * 设置COA状态：
                     * Update COAStatus set Line=’REM’，Status=’A2’，Editor=@user, Udt=GetDate where COASN=@coano
                     * 记录COA Log：
                     * insert COALog values (@coano, 'A2',custsn+'/'+hppno, rtrim(@user), GetDate, ‘COA’)
                     * -----------------------------------------------------------------------------------------------
                     * IProductRepository::
                     * void DeleteProductPartByPartSn(string prodId, string partSn);
                     * 或void DeleteProductPartByPartSnDefered(IUnitOfWork uow, string prodId, string partSn);
                     * 
                     * 
                     * ProductStatus的修改请使用主体对象Product的Status属性，其被修改后，调用IProductRepository的Update方法来更新修改。
                     * 
                     * 
                     * COAStatus的更新使用ICOAStatusRepository的Update方法.
                     *
                     *
                     * ICOAStatusRepository::
                     * void InsertCOALog(COALog newLog);
                     * 或void InsertCOALogDefered(IUnitOfWork uow, COALog newLog);
                     * 
                     */

                    // if found == false, product_list has no node, and it does not exist in product_part.
                    // so we need not do:
                    //    1. productRep.DeleteProductPartByPartSn(productID, partSn)
                    //    2. productRep.UpdateProductListStatus.
                    if (found == true || pizzaID != "")
                    {
                        productRep.DeleteProductPartByPartSnDefered(CurrentSession.UnitOfWork, productID, partSn);
                        
                        //IMES.FisObject.FA.Product.ProductStatus newStatus = new IMES.FisObject.FA.Product.ProductStatus();
                        //Station=’69’, Editor=@user, Udt=GetDate where ProducntID=@productID
                        newStatus.StationId = "69";
                        newStatus.Editor = editor;
                        newStatus.Udt = DateTime.UtcNow;
                        newStatus.ProId = productID;
                        productRep.UpdateProductListStatusDefered(CurrentSession.UnitOfWork, newStatus, product_list); //Defered(CurrentSession.UnitOfWork, newStatus, ProductIDList);
                    }

                    COAStatus _coaStatus = coaStatusRep.GetCoaStatus(coaNo);

                    if (Actionlist[i] == "Scrap")
                    {
                        ////////////////////////////////////////////////////////////////////////////
                        //Update COAStatus set Status=@cause，Editor=@user, Udt=GetDate where COASN=@coano
                        string strcause;
                        strcause = Causelist[i].Substring(0, Causelist[i].IndexOf(" "));

                        _coaStatus.Status = strcause;
                        _coaStatus.Editor = editor;
                        _coaStatus.Udt = DateTime.UtcNow;
                        _coaStatus.COASN = coaNo;
                        coaStatusRep.UpdateCOAStatusDefered(CurrentSession.UnitOfWork, _coaStatus);

                        ////////////////////////////////////////////////////////////////////////////
                        COAReturnInfo setCOA = new COAReturnInfo();
                        setCOA.status = strcause;
                        setCOA.editor = editor.Trim();
                        setCOA.udt = DateTime.UtcNow;

                        COAReturnInfo conditionCOA = new COAReturnInfo();
                        conditionCOA.coasn = coaNo;

                        coaStatusRep.UpdateCOAReturnInfoDefered(CurrentSession.UnitOfWork, setCOA, conditionCOA);

                    }
                    else
                    {
                        // found == true || found == false, program comes here, that is to say, COA Staus = 'A1',
                        // so we must update COA status to be 'A2'.
                        //Update COAStatus set Line=’REM’，Status=’A2’，Editor=@user, Udt=GetDate where COASN=@coano
                        _coaStatus.LineID = "REM";
                        _coaStatus.Status = "A2";
                        _coaStatus.Editor = editor;
                        _coaStatus.Udt = DateTime.UtcNow;
                        _coaStatus.COASN = coaNo;
                        coaStatusRep.UpdateCOAStatusDefered(CurrentSession.UnitOfWork, _coaStatus);
                    
                    }

                    // if found == false, we do not know the CUSTSN, and so we can not make a COALog.
                    if (found == true || pizzaID != "")
                    {
                        if (Actionlist[i] == "Scrap")
                        {
                            // insert COALog values (@coano, @cause,@coapn, rtrim(@user), GetDate, ‘COA’)
                            //Note：
                            //@cause – 该COA No录入时，选择的Cause
                            //@coapn – 该COA No 对应的COAStatus.IECPN
                            string strcause;
                            strcause = Causelist[i].Substring(0, Causelist[i].IndexOf(" "));

                            COALog newLog = new COALog();
                            newLog.COASN = coaNo;
                            newLog.StationID = strcause;
                            newLog.LineID = _coaStatus.IECPN;
                            newLog.Editor = editor.Trim();
                            newLog.Cdt = DateTime.Now;
                            newLog.Tp = "COA";

                            coaStatusRep.InsertCOALogDefered(CurrentSession.UnitOfWork, newLog);
                        }
 
                        else
                        {
                            //insert COALog values (@coano, 'A2',custsn+'/'+hppno, rtrim(@user), GetDate, ‘COA’)
                            COALog newLog = new COALog();
                            newLog.COASN = coaNo;
                            newLog.StationID = "A2";
                            newLog.LineID = CUSTSN + "/" + CustPartNo;
                            newLog.Editor = editor.Trim();
                            newLog.Cdt = DateTime.Now;
                            newLog.Tp = "COA";

                            coaStatusRep.InsertCOALogDefered(CurrentSession.UnitOfWork, newLog);
                        }
                    }
                    i++;
                }
            }
            catch(FisException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw e;
            }

            return base.DoExecute(executionContext);
        }

    }
}

