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
    public partial class GenerateSnForRegenerateCustSN : BaseActivity
    {
        //转换数组（31进制）
        private static string[] numLst = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9",
                                           "B", "C", "D", "F", "G", "H", "J",
                                           "K", "L", "M", "N", "P", "Q", "R", "S", "T",
                                           "V", "W", "X", "Y", "Z"};

        /// <summary>
        /// 构造函数
        /// </summary>
        public GenerateSnForRegenerateCustSN()
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
            IPartRepository ipartRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();

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
                maxnum = numControl.GetMaxNumber("CPQSNO", prestr + "{0}");
                if (string.IsNullOrEmpty(maxnum))
                {
                    /* 2012-6-27，为产线测试用，正式使用时要修改回来
                    maxnum = "Z000";    //起始值：Z000
                    */
                    maxnum = "Z000";    //非正式起始值：V000

                    addflag = true;
                }
                else
                {
                    //maxnum="CNU248000Y";
                    /* 2012-6-27，为产线测试用，正式使用时要修改回来
                    string temstr = "Z000"; //起始值：Z000
                    */
                    string temstr = "Z000";

                    string numstr = maxnum.Substring(maxnum.Length - 4);
                    temstr = numstr;

                    //2012-6-27，为产线测试用，正式使用时要删除
                 /*   if (numstr.ToUpper() == "VZZZ")
                    {
                        FisException fe = new FisException("CHK867", new string[] { });   //流水号已满!
                        throw fe;
                    }
                    */

                    if (numstr.ToUpper() == "YZZZ")
                    {
                        FisException fe = new FisException("CHK867", new string[] { });   //流水号已满!
                        throw fe;
                    }

                    string[] seqLst = new string[4];
                    seqLst[0] = temstr.Substring(0, 1);
                    seqLst[1] = temstr.Substring(1, 1);
                    seqLst[2] = temstr.Substring(2, 1);
                    seqLst[3] = temstr.Substring(3, 1);

                    int[] idexLst = getSeqNum(seqLst);

                    if (idexLst[3] == 30)
                    {
                        if (idexLst[2] == 30)
                        {
                            if (idexLst[1] == 30)
                            {
                                if (idexLst[0] == 29)
                                {
                                    List<string> errpara = new List<string>();
                                    throw new FisException("CHK867", errpara);  //流水号已满!
                                }
                                else
                                {
                                    if (idexLst[0] == 30)
                                    {
                                        idexLst[0] = 28;
                                    }
                                    else if (idexLst[0] == 28)
                                    {
                                        idexLst[0] = 27;
                                    }
                                    else if (idexLst[0] == 27)
                                    {
                                        idexLst[0] = 29;
                                    }
                                    else
                                    {
                                        List<string> errpara = new List<string>();
                                        throw new FisException("CHK867", errpara);  //非合法的Z、X、W、Y,视为满
                                    }

                                    idexLst[1] = 0;
                                    idexLst[2] = 0;
                                    idexLst[3] = 0;
                                }
                            }
                            else
                            {
                                idexLst[1] += 1;
                                idexLst[2] = 0;
                                idexLst[3] = 0;
                            }
                        }
                        else
                        {
                            idexLst[2] += 1;
                            idexLst[3] = 0;
                        }
                    }
                    else
                    {
                        idexLst[3] += 1;
                    }

                    temstr = numLst[idexLst[0]] + numLst[idexLst[1]] + numLst[idexLst[2]] + numLst[idexLst[3]];
                    maxnum = temstr;
                }

                custSn = custSn + maxnum.ToUpper();

                product.CUSTSN = custSn;
                productRep.Update(product, uof);

                NumControl item = new NumControl();
                item.NOType = "CPQSNO";
                item.Value = custSn;
                item.NOName = "";
                item.Customer = "HP";

                numControl.SaveMaxNumber(item, addflag, prestr + "{0}");

                uof.Commit();  //立即提交UnitOfWork更新NumControl里面的最大值

                SqlTransactionManager.Commit();//提交事物，释放行级更新锁

                //IMORepository moRepository = RepositoryFactory.GetInstance().GetRepository<IMORepository, MO>();
               // moRepository.UpdateMoForIncreaseCustomerSnQty(product.ProId, 1);

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

            IList<string> values = ipartRepository.GetValueFromSysSettingByName("Customer");
            string value="";
            if (values.Count > 0)
                value = values[0];

            CurrentSession.AddValue(Session.SessionKeys.PrintLogName, value + " SNO");
            CurrentSession.AddValue(Session.SessionKeys.PrintLogBegNo, product.ProId);
            CurrentSession.AddValue(Session.SessionKeys.PrintLogEndNo, product.ProId);
            CurrentSession.AddValue(Session.SessionKeys.PrintLogDescr, product.CUSTSN);
            
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

        private int[] getSeqNum(string[] str)
        {
            int[] list = new int[4];
            int flag = 0;
            for (int l = 0; l < numLst.Count(); l++)
            {
                if (str[0] == numLst[l].ToString())
                {
                    list[0] = l;
                    flag += 1;
                }

                if (str[1] == numLst[l].ToString())
                {
                    list[1] = l;
                    flag += 1;
                }

                if (str[2] == numLst[l].ToString())
                {
                    list[2] = l;
                    flag += 1;
                }

                if (str[3] == numLst[l].ToString())
                {
                    list[3] = l;
                    flag += 1;
                }

                if (flag == 4)
                {
                    break;
                }
            }
            return list;
        }
    }
}
