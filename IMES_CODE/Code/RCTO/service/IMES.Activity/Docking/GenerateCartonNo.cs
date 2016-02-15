/*
* INVENTEC corporation ©2011 all rights reserved. 
* Description:Combine Po in Carton
* UI:CI-MES12-SPEC-PAK-UI Combine Po in Carton.docx –2012/05/21 
* UC:CI-MES12-SPEC-PAK-UC Combine Po in Carton.docx –2012/05/21          
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2012-05-21   Du.Xuan               Create   
* ITC-1414-0069  重复累加
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
using IMES.Infrastructure;
using IMES.Infrastructure.UnitOfWork;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.NumControl;
using IMES.FisObject.Common.Model;
using IMES.FisObject.Common.MO;
using carton = IMES.FisObject.PAK.CartonSSCC;
using IMES.DataModel;
using System.Collections.Generic;

namespace IMES.Activity
{
    /// <summary>
    /// 產生Carton NO號相关逻辑
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
    public partial class GenerateCartonNo : BaseActivity
	{
        /// <summary>
        /// 构造函数
        /// </summary>
		public GenerateCartonNo()
		{
			InitializeComponent();
		}

        /// <summary>
        /// 產生Carton NO號相关逻辑
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override System.Workflow.ComponentModel.ActivityExecutionStatus DoExecute(System.Workflow.ComponentModel.ActivityExecutionContext executionContext)
        { 
            IProduct product = (IProduct)CurrentSession.GetValue(Session.SessionKeys.Product);
            IProductRepository productRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();

            //Format of Carton No
            //CYMDDSSSS
            //Remark:
            //T – 前缀，固定字符’C’
            //Y – Year Code，西元年的最后一位
            //M – Month Code，1～9 表示1～9 月，A~C 表示10～12 月
            //DD – Day Code，两位，00～31
            //SSSS – 流水号，10进制，起始值为0001
            try
            {
        
                string CartonNO = "";
                string maxnum = "";
                string prestr = "";
                //求当前日期是一年的中第几周
                //int weeks = 0;
                DateTime curDate = DateTime.Now;
                /*var ret = IMES.Infrastructure.Utility.Generates.WeekRuleEngine.Calculate("103", dateTime);
                string weekCode = ret.Week.ToString().PadLeft(2, '0');
                string year = ret.Year.ToString();
                string month = ret.*/
                string year = curDate.Year.ToString();
                //string month = Convert.ToString(curDate.Month,16);
                string month = curDate.Month.ToString("X");//2012/10/05
                string dd = curDate.Day.ToString("d2");

                CartonNO = "C" + year.Substring(year.Length - 1, 1) + month + dd;
                prestr = CartonNO;
                // 自己管理事务开始
                SqlTransactionManager.Begin();
                IUnitOfWork uof = new UnitOfWork();//使用自己的UnitOfWork

                //从NumControl中获取流水号
                //GetMaxNumber
                INumControlRepository numControl = RepositoryFactory.GetInstance().GetRepository<INumControlRepository, NumControl>();

                bool addflag = false;

                //maxnum = numControl.GetMaxNumber("CARTONNO", prestr + "{0}");
                var maxObj = numControl.GetMaxValue("CARTONNO", prestr);
                //檢查有沒有lock index, 沒有lock index, 改變查詢條件
                if (maxObj == null)
                {
                    //lock NoType='CARTONNO'
                    //string maxData = numControl.GetMaxNumber("CARTONNO", "{0}");
                    //maxObj = numControl.GetMaxNumberObj("CARTONNO", prestr + "{0}");
                    var data = numControl.GetMaxValue("CARTONNO", "Lock");
                    maxObj = numControl.GetMaxValue("CARTONNO", prestr);
                }

                if (maxObj != null)
                {
                    maxnum = maxObj.Value;
                } 

                if (string.IsNullOrEmpty(maxnum))
                {
                    maxnum = "0001";//"8000";
                    addflag = true;
                }
                else
                {
                    //maxnum="CNU248000Y";                   
                    string numstr = maxnum.Substring(maxnum.Length - 4);
                    if (numstr.ToUpper() == "9999")
                    {
                        FisException fe = new FisException("CHK867", new string[] { });   //流水号已满!
                        throw fe;
                    }
                    //ITC-1414-0069
                    int result = Convert.ToInt32(numstr)+1;
                    maxnum = result.ToString("d4");
                }

                CartonNO = CartonNO + maxnum.ToUpper();

               
                NumControl item = null;// new NumControl();
                if (addflag)
                {
                    item = new NumControl();
                    item.NOType = "CARTONNO";
                    item.Value = CartonNO;
                    item.NOName = prestr;
                    item.Customer = this.Customer;
                }
                else
                {
                    item = maxObj;
                    item.Value = CartonNO;
                }

                //numControl.SaveMaxNumber(item, addflag, prestr + "{0}");
                numControl.SaveMaxNumber(item, addflag);

                uof.Commit();  //立即提交UnitOfWork更新NumControl里面的最大值

                SqlTransactionManager.Commit();//提交事物，释放行级更新锁

                product.CartonSN = CartonNO;

                //a.	Insert CartonStatus
                //INSERT INTO [CartonStatus]([CartonNo],[Station],[Status],[Line],[Editor],[Cdt],[Udt])
	            //VALUES(@CartonNo, @Station, 1, @PdLine, @Editor, GETDATE(), GETDATE())
                carton.ICartonSSCCRepository cartRep = RepositoryFactory.GetInstance().GetRepository<carton.ICartonSSCCRepository, IMES.FisObject.PAK.CartonSSCC.CartonSSCC>();
                CartonStatusInfo sinfo = new CartonStatusInfo();
                sinfo.cartonNo = product.CartonSN;
                sinfo.station = Station;
                sinfo.status = 1;//pass
                sinfo.line = Line;
                sinfo.editor = Editor;
                sinfo.cdt = DateTime.Now;
                sinfo.udt = DateTime.Now;
                cartRep.AddCartonStatusInfoDefered(CurrentSession.UnitOfWork,sinfo);
                
                //b.	Insert CartonLog
                //INSERT INTO [CartonLog]([CartonNo],[Station],[Status],[Line],[Editor],[Cdt])
            	//VALUES(@CartonNo, @Station, 1, @PdLine, @Editor, GETDATE())
                CartonLogInfo linfo = new CartonLogInfo();
                linfo.cartonNo = product.CartonSN;
                linfo.station = Station;
                linfo.status = 1;//pass
                linfo.line = Line;
                linfo.editor = Editor;
                linfo.cdt = DateTime.Now;
                cartRep.AddCartonLogInfoDefered(CurrentSession.UnitOfWork,linfo);
                
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