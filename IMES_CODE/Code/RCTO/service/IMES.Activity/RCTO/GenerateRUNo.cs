/*
* INVENTEC corporation ©2011 all rights reserved. 
* Description:Combine Po in Carton
* UI:CI-MES12-SPEC-PAK-UI Combine Po in Carton.docx –2012/05/21 
* UC:CI-MES12-SPEC-PAK-UC Combine Po in Carton.docx –2012/05/21          
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2012-05-21   Du.Xuan               Create   
* ITC-1414-0065  接口参数传入错误
* ITC-1414-0119  flag不等于N时未记录status和log
* ITC-1414-0122  Flag=N and boxReg='' 不再执行UPDATE SnoCtrl_BoxId
* Known issues:
* TODO：
* 
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
using IMES.FisObject.FA.Product;
using IMES.FisObject.PAK.Pallet;
using IMES.FisObject.PAK.DN;
using IMES.Infrastructure;
using IMES.Infrastructure.Utility;
using IMES.Infrastructure.UnitOfWork;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.NumControl;
using IMES.FisObject.Common.Model;
using IMES.FisObject.Common.MO;
using System.Collections.Generic;
using carton = IMES.FisObject.PAK.CartonSSCC;
using IMES.DataModel;

namespace IMES.Activity
{
    /// <summary>
    /// 產生BOX ID號相关逻辑
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      Carton NO
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         更新Product.CUSTSN
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// </para> 
    /// <para>    
    /// 输入：
    ///         无
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
    ///         Product
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         IProduct
    ///         IProductRepository
    /// </para> 
    /// </remarks>
    public partial class GenerateRUNo : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public GenerateRUNo()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 產生BOX ID號號相关逻辑
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override System.Workflow.ComponentModel.ActivityExecutionStatus DoExecute(System.Workflow.ComponentModel.ActivityExecutionContext executionContext)
        {
            IProduct product = (IProduct)CurrentSession.GetValue(Session.SessionKeys.Product);
            Delivery delivery = (Delivery)CurrentSession.GetValue(Session.SessionKeys.Delivery);

            IProductRepository productRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            IPalletRepository palletRep = RepositoryFactory.GetInstance().GetRepository<IPalletRepository, Pallet>();
            IDeliveryRepository deliveryRep = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
            IModelRepository modelRep = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();
            carton.ICartonSSCCRepository cartRep = RepositoryFactory.GetInstance().GetRepository<carton.ICartonSSCCRepository, IMES.FisObject.PAK.CartonSSCC.CartonSSCC>();

            try
            {
                //New Format of Box Id
                //Format of RU No
                //WWSSSSSS
                //Remark:
                //WW – HP Week Code
                //SSSSSS – 流水号，36进制，基字符
                string maxnum = "";
                string prestr = "";

                DateTime dateTime = DateTime.Now;
               // var ret = IMES.Infrastructure.Utility.Generates.WeekRuleEngine.Calculate("103", dateTime);
                IList<string> codeList = modelRep.GetCodeFromHPWeekCodeInRangeOfDescr();
                string weekCode = "";
                if (codeList.Count > 0)
                {
                    weekCode = codeList[0];
                }

                string runo = "";
                runo = weekCode ;
                prestr = runo;
                // 自己管理事务开始
                SqlTransactionManager.Begin();
                IUnitOfWork uof = new UnitOfWork();//使用自己的UnitOfWork

                //从NumControl中获取流水号
                //GetMaxNumber
                INumControlRepository numControl = RepositoryFactory.GetInstance().GetRepository<INumControlRepository, NumControl>();

                bool addflag = false;

                maxnum = numControl.GetMaxNumber("RUNO", prestr + "{0}");
                if (string.IsNullOrEmpty(maxnum))
                {
                    maxnum = "000001";
                    addflag = true;
                }
                else
                {
                    MathSequenceWithCarryNumberRule marc = new MathSequenceWithCarryNumberRule(6, "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ");

                    string numstr = maxnum.Substring(maxnum.Length - 6);
                    if (numstr.ToUpper() == "ZZZZZZ")
                    {
                        FisException fe = new FisException("CHK867", new string[] { });   //流水号已满!
                        throw fe;
                    }
                    numstr = marc.IncreaseToNumber(numstr, 1);
                    maxnum = numstr;

                }

                runo = runo + maxnum.ToUpper();

                NumControl item = new NumControl();
                item.NOType = "RUNO";
                item.Value = runo;
                item.NOName = "";
                item.Customer = "HP";

                numControl.SaveMaxNumber(item, addflag, prestr + "{0}");

                uof.Commit();  //立即提交UnitOfWork更新NumControl里面的最大值

                SqlTransactionManager.Commit();//提交事物，释放行级更新锁

                //INSERT INTO [CartonInfo]([CartonNo],[InfoType],[InfoValue],[Editor],[Cdt],[Udt])
	            //VALUES (@CartonNo, 'RUNo', @RUNo, @Editor, GETDATE(), GETDATE())
                CartonInfoInfo sinfo = new CartonInfoInfo();
                sinfo.cartonNo = product.CartonSN;
                sinfo.infoType = "RUNo";
                sinfo.infoValue = runo;
                sinfo.editor = Editor;
                sinfo.cdt = DateTime.Now;
                sinfo.udt = DateTime.Now;
                cartRep.InsertCartonInfoDefered(CurrentSession.UnitOfWork, sinfo);

                //Product结合Delivery and Carton
                //将页面上[Products in Carton] 中的每一个Product和页面选定的Delivery 已经上文生成的Carton No 进行结合 – Update Product
                //Product.DeliveryNo – Delivery No
                //Product.CartonSN – Carton No

                

                IList<IProduct> productList = (List<IProduct>)CurrentSession.GetValue(Session.SessionKeys.ProdList);
                foreach (var prod in productList)
                {
                    prod.DeliveryNo = delivery.DeliveryNo;
                    prod.CartonSN = product.CartonSN;
                    productRep.Update(prod, CurrentSession.UnitOfWork);
                }

                //6.	Update ProductStatus / Insert ProductLog


            } 

            catch (Exception)
            {
                SqlTransactionManager.Rollback();
                throw;
            }
            finally
            {
                SqlTransactionManager.Dispose();
                SqlTransactionManager.End();
            }

            return base.DoExecute(executionContext);
        }

    }
}
