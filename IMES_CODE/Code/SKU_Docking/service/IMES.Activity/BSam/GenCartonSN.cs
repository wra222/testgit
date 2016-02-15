// INVENTEC corporation (c)2011 all rights reserved. 
// Description: 
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2013-02-07   Benson          create
//2013-03-13    Vincent           release
// Known issues:
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
using IMES.FisObject.Common;
using IMES.FisObject.PAK.Pallet;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure.UnitOfWork;
using IMES.FisObject.Common.NumControl;
using IMES.Infrastructure.Extend;
using IMES.FisObject.PAK.Carton;

namespace IMES.Activity
{


    /// <summary>
    /// Generate Cartion SN
    /// </summary>
  
    public partial class GenCartonSN : BaseActivity
	{
        /// <summary>
        /// 
        /// </summary>
        public GenCartonSN()
		{
			InitializeComponent();
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            IProduct product = (IProduct)CurrentSession.GetValue(Session.SessionKeys.Product);
            IProductRepository productRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();

            //Format of Carton No
            //TYMDDSSSS
            //Remark:
            //T – 前缀，線別第一碼
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

                CartonNO = this.Line.Substring(0,1)+ year.Substring(year.Length - 1, 1) + month + dd;
                prestr = CartonNO;
                // 自己管理事务开始
                SqlTransactionManager.Begin();
                IUnitOfWork uof = new UnitOfWork();//使用自己的UnitOfWork

                //从NumControl中获取流水号
                //GetMaxNumber
                INumControlRepository numControl = RepositoryFactory.GetInstance().GetRepository<INumControlRepository, NumControl>();

                bool addflag = false;
                //var maxObj = numControl.GetMaxNumberObj("CARTONNO", prestr + "{0}");
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
              
                //maxnum = numControl.GetMaxNumber("CARTONNO", prestr + "{0}");
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
                    int result = Convert.ToInt32(numstr) + 1;
                    maxnum = result.ToString("d4");
                }

                CartonNO = CartonNO + maxnum.ToUpper();
                NumControl item = null;// new NumControl();
                if (addflag)
                {
                    item = new NumControl();
                    item.NOType = "CARTONNO";
                    item.Value = CartonNO;
                    item.NOName = prestr; //"";
                    item.Customer = "HP";
                }
                else
                {
                    item = maxObj;
                    item.Value = CartonNO;
                }

                //NumControl item = new NumControl();
                //item.NOType = "CARTONNO";
                //item.Value = CartonNO;
                //item.NOName = "";
                //item.Customer = "HP";

                //numControl.SaveMaxNumber(item, addflag, prestr + "{0}");

                numControl.SaveMaxNumber(item, addflag);
                
                //Create Carton object
                Carton carton = new Carton(CartonNO, CurrentSession.Station, 1, CurrentSession.Line, CurrentSession.Editor);
                ICartonRepository cartonRep = RepositoryFactory.GetInstance().GetRepository<ICartonRepository, Carton>();
                cartonRep.Add(carton, uof);

                uof.Commit();  //立即提交UnitOfWork更新NumControl里面的最大值

                SqlTransactionManager.Commit();//提交事物，释放行级更新锁

                CurrentSession.AddValue(ExtendSession.SessionKeys.CartonSN, CartonNO);

                
                CurrentSession.AddValue(Session.SessionKeys.Carton, carton);              

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
