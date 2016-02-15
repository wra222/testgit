/*
* INVENTEC corporation ©2011 all rights reserved. 
* Description:Content & Warranty Print
* UI:CI-MES12-SPEC-FA-UC Generate Customer SN.docx –2011/12/14 
* UC:CI-MES12-SPEC-FA-UI Generate Customer SN.docx –2011/12/14            
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2011-12-14   Du.Xuan               Create   
* Known issues:
* ITC-1360-0330 week应该占两位
* ITC-1360-0334 36进制计算流水号
* ITC-1360-0335 按照建议修改
* ITC-1360-0523 36进制计算流水号
* ITC-1360-0577 增加事务管理防止并发
* ITC-1360-1149 numctrl只有一条数据
* ITC-1360-1355 统一使用底层year和week
* ITC-1360-1421 增加更新qty
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
using IMES.Infrastructure.Utility;
using IMES.Infrastructure.UnitOfWork;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.NumControl;
using IMES.FisObject.Common.Model;
using IMES.FisObject.Common.MO;
using System.Collections.Generic;

namespace IMES.Activity
{
    /// <summary>
    /// 產生CustomerSN號相关逻辑
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      Generate customer SN
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
	public partial class GenerateCustomerSn: BaseActivity
	{
        /// <summary>
        /// 构造函数
        /// </summary>
		public GenerateCustomerSn()
		{
			InitializeComponent();
		}

        /// <summary>
        /// 產生CustomerSN號相关逻辑
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override System.Workflow.ComponentModel.ActivityExecutionStatus DoExecute(System.Workflow.ComponentModel.ActivityExecutionContext executionContext)
        { 
            IProduct product = (IProduct)CurrentSession.GetValue(Session.SessionKeys.Product);
            IProductRepository productRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();

            //lock (_syncRoot_GetSeq)//用于防止同一段代码被同时执行，以前只有一个Service时有效，现在多个Service没有去掉，聊胜于无

            //var logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            //logger.InfoFormat("GenerateCustomerSnActivity: CurrentSession Hash: {0}; CurrentSession Key: {1}; CurrentSession Type: {2}", CurrentSession.GetHashCode().ToString(), CurrentSession.Key, CurrentSession.Type.ToString());
            //logger.InfoFormat("GenerateCustomerSnActivity: IProduct Hash: {0}; IProduct Key: {1}", product.GetHashCode().ToString(), product.Key);

            //need modify
            try
            {
                //CN(中try国代码)+U（Site Code）+年尾码+周别码+流水码
                string custSn = "";
                DateTime curDate = DateTime.Now;
               // string year = curDate.Year.ToString();
                string maxnum = "";
                string prestr = "";
                //求当前日期是一年的中第几周
                //int weeks = 0;
                DateTime dateTime = DateTime.Now;//new DateTime(2016,1,1);
                var ret = IMES.Infrastructure.Utility.Generates.WeekRuleEngine.Calculate("103", dateTime);
                string weekCode = ret.Week.ToString().PadLeft(2, '0');
                string year = ret.Year.ToString();//dateTime.Year.ToString();

                custSn = "CN" + "U" + custSn + year.Substring(year.Length - 1, 1) + weekCode;//weeks.ToString("d2");
                prestr = custSn;
                // 自己管理事务开始
                SqlTransactionManager.Begin();
                IUnitOfWork uof = new UnitOfWork();//使用自己的UnitOfWork

                //从NumControl中获取流水号
                //GetMaxNumber
                INumControlRepository numControl = RepositoryFactory.GetInstance().GetRepository<INumControlRepository, NumControl>();

                bool addflag = false;

                //maxnum = numControl.GetMaxNumber("CPQSNO", prestr + "{0}");

                var maxObj = numControl.GetMaxNumberObj("CPQSNO", prestr + "{0}");
                if (maxObj != null)
                    maxnum = maxObj.Value;

                if (string.IsNullOrEmpty(maxnum))
                {
                    maxnum = "9000";
                    addflag = true;
                }
                else
                {
                    //36进制改为31进制
                    MathSequenceWithCarryNumberRule marc = new MathSequenceWithCarryNumberRule(4, "0123456789BCDFGHJKLMNPQRSTVWXYZ");
                    string numstr = maxnum.Substring(maxnum.Length - 4);
                    if (numstr.ToUpper() == "YZZZ")
                    {
                        FisException fe = new FisException("CHK867", new string[] { });   //流水号已满!
                        throw fe;
                    }
                    numstr = marc.IncreaseToNumber(numstr, 1);
                    maxnum = numstr;

                }
                custSn = custSn + maxnum.ToUpper();

                product.CUSTSN = custSn;
                productRep.Update(product, uof);

                NumControl item = null;// new NumControl();

                if (addflag)
                {
                    item = new NumControl();
                    item.NOType = "CPQSNO";
                    item.Value = custSn;
                    item.NOName = "";
                    item.Customer = "HP";
                }
                else
                {
                    item = maxObj;
                    item.Value = custSn;
                }

                //numControl.SaveMaxNumber(item, addflag, prestr + "{0}");
                numControl.SaveMaxNumber(item, addflag);
                 
                uof.Commit();  //立即提交UnitOfWork更新NumControl里面的最大值

                SqlTransactionManager.Commit();//提交事物，释放行级更新锁
                
                // [Customer SN Print]保存结果：增加更新[CustomerSN_Qty]. CustomerSN_Qty栏位
                //UPDATE [HPIMES].[dbo].[MO]
                //SET [CustomerSN_Qty] =[ CustomerSN_Qty]+1
                //From Product a,MO b  WHERE a.MO=b.MO and a.ProductID=ProductID#

                IMORepository moRepository = RepositoryFactory.GetInstance().GetRepository<IMORepository, MO>();
                moRepository.UpdateMoForIncreaseCustomerSnQty(product.ProId, 1);

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

            CurrentSession.AddValue(Session.SessionKeys.PrintLogName, product.Customer + "SNO");
            CurrentSession.AddValue(Session.SessionKeys.PrintLogBegNo, product.CUSTSN);
            CurrentSession.AddValue(Session.SessionKeys.PrintLogEndNo, product.CUSTSN);
            CurrentSession.AddValue(Session.SessionKeys.PrintLogDescr, product.ProId);
            
            return base.DoExecute(executionContext);
        }

        private string Convert10_36(int num)
        {
            string s = "";
            while (num >= 36)
            {
                int j = num % 36;
                s += ((j <= 9) ? Convert.ToChar(j + '0') : Convert.ToChar(j - 10 + 'a'));
                num = num / 36;
            }
            s += ((num <= 9) ? Convert.ToChar(num + '0') : Convert.ToChar(num - 10 + 'a'));

            Char[] c = s.ToCharArray();
            Array.Reverse(c);
            return new string(c);
      
        }
        private int ConvertToInt32(char c)
        {
            if (c >= 48 && c <= 57) { return c - 48; }
            if (c >= 97 && c <= 122) { return c - 87; }
            if (c >= 65 && c <= 90) { return c - 55; }
            return 0;
        }

        private int Convert36_10(string numstr)
        {
            char[] chars = numstr.ToCharArray();
            Array.Reverse(chars);
            int fb = 1;
            int value = 0;
            foreach (char aa in chars)
            {
                value += ConvertToInt32(aa) * fb;
                fb *= 36;
            }
            return value;
        }

            /*int firstdayofweek = Convert.ToInt32(Convert.ToDateTime(year + "- " + "1-1 ").DayOfWeek);
              int days = curDate.DayOfYear;
              int daysOutOneWeek = days - (7 - firstdayofweek);
            if (daysOutOneWeek <= 0)
            {
                weeks = 1;
            }
            else
            {
                weeks = daysOutOneWeek / 7;
                if (daysOutOneWeek % 7 != 0)
                    weeks++;
                weeks++;
            }*/

            /*string weekCode = "";
            IModelRepository CurrentModelRepository = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();
            IList<string> weekCodeList = CurrentModelRepository.GetCodeFromHPWeekCodeInRangeOfDescr();
            if (weekCodeList != null && weekCodeList.Count > 0)
            {
                weekCode = weekCodeList[0];
            }
            else
            {
                throw new FisException("ICT009", new string[] { });
            }
            */
    }
}
