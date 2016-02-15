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
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.NumControl;
using System.Collections.Generic;
using IMES.FisObject.Common.Model;
using IMES.Infrastructure.UnitOfWork;
using IMES.FisObject.Common.MO;
using IMES.Infrastructure.Utility;
using IMES.FisObject.Common.Part;
using IMES.Common;

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
    public partial class GenerateSnForMasterLabel : BaseActivity
    {
        
        /// <summary>
        /// 构造函数
        /// </summary>
        public GenerateSnForMasterLabel()
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
            IPartRepository PartRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            string stag =(string) CurrentSession.GetValue(Session.SessionKeys.CN);
            //need modify
            if (stag == "T") {
                return base.DoExecute(executionContext);
            }
            try
            {
                //CN(中国代码)+U（Site Code）+年尾码+周别码+流水码
                string custSn = "";
                //string weekCode="";
                DateTime curDate = DateTime.Now;
                //string year = curDate.Year.ToString();
                string maxnum = "";
                string prestr = "";
                bool addflag = false;

                DateTime dateTime = new DateTime(2016, 1, 1);
                var dd = DateTime.Now;
                var ret = IMES.Infrastructure.Utility.Generates.WeekRuleEngine.Calculate("103", DateTime.Now);

                string weekCode = ret.Week.ToString().PadLeft(2, '0');
                string year = ret.Year.ToString();
                //prestr = custSn; //xxx 20130826 error

                SqlTransactionManager.Begin();
                IUnitOfWork uof = new UnitOfWork();//使用自己的UnitOfWork
                IList<string> valueList = PartRepository.GetValueFromSysSettingByName("Site");
                if (valueList.Count == 0)
                {
                    throw new Exception("Error:尚未設定Site...");
                }
                else
                {
                    if (valueList[0] == "ICC")//重慶
                    {
                        custSn = "5CG"; //+ custSn + year.Substring(year.Length - 1, 1) + weekCode;
                    }
                    else //IPC 上海
                    {
                        custSn = "CNU";// +custSn + year.Substring(year.Length - 1, 1) + weekCode;//weeks.ToString("d2");
                    }
                }
                //custSn = "CN" + "U" + custSn + year.Substring(year.Length - 1, 1) + weekCode;
                ActivityCommonImpl commonImpl = ActivityCommonImpl.Instance;
                custSn = commonImpl.GetCustSNPreFix3(custSn,product.Family) + year.Substring(year.Length - 1, 1) + weekCode;
				prestr = custSn;

                //从NumControl中获取流水号
                //GetMaxNumber
                INumControlRepository numControl = RepositoryFactory.GetInstance().GetRepository<INumControlRepository, NumControl>();

                //maxnum = numControl.GetMaxNumber("CPQSNO", prestr + "{0}");

                var maxObj = numControl.GetMaxNumberObj("CPQSNO", prestr + "{0}");
                if (maxObj != null)
                    maxnum = maxObj.Value;

                if (string.IsNullOrEmpty(maxnum))
                {
                    maxnum = "0000";
                    addflag = true;
                }
                else
                {
                    //maxnum="CNU248000Y";
                    MathSequenceWithCarryNumberRule marc = new MathSequenceWithCarryNumberRule(4, "0123456789BCDFGHJKLMNPQRSTVWXYZ");
                    string numstr = maxnum.Substring(maxnum.Length - 4);

                    //string temstr = "0000";

                    if (numstr.ToUpper() == "YZZZ")
                    {
                        FisException fe = new FisException("CHK867", new string[] { });   //流水号已满!
                        throw fe;
                    }
                    numstr = marc.IncreaseToNumber(numstr, 1);
                    maxnum = numstr;
                }
                    
                custSn = custSn + maxnum.ToUpper();

                if (productRep.CheckExistCustomSn(custSn))
                {
                    List<string> errpara = new List<string>();
                    errpara.Add(custSn);
                    throw new FisException("CHK1021", errpara);
                }

                product.CUSTSN = custSn;
                //productRep.Update(product, CurrentSession.UnitOfWork);

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

                /*CurrentSession.AddValue(Session.SessionKeys.PrintLogName, product.Customer + " SNO");
            CurrentSession.AddValue(Session.SessionKeys.PrintLogBegNo, product.CUSTSN);
            CurrentSession.AddValue(Session.SessionKeys.PrintLogEndNo, product.CUSTSN);
            CurrentSession.AddValue(Session.SessionKeys.PrintLogDescr, product.ProId);
            */
            return base.DoExecute(executionContext);
        }

        private string Convert10_36(int num)
        {
            string s = "";
            while (num >= 32)
            {
                int j = num % 32;
                s += ((j <= 9) ? Convert.ToChar(j + '0') : Convert.ToChar(j - 10 + 'b'));
                num = num / 32;
            }
            s += ((num <= 9) ? Convert.ToChar(num + '0') : Convert.ToChar(num - 10 + 'b'));

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
                fb *= 32;
            }
            return value;
        }
        
    }
}
