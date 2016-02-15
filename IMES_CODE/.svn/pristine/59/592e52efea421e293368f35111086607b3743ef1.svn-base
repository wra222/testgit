/*
* INVENTEC corporation ©2011 all rights reserved. 
* Description:Content & Warranty Print
* UI:CI-MES12-SPEC-FA-UC Generate Customer SN.docx –2011/12/14 
* UC:CI-MES12-SPEC-FA-UI Generate Customer SN.docx –2011/12/14            
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2011-5-30   Jessica Liu               Create   
* Known issues:
* ITC-1414-0192, Jessica Liu, 2012-6-20
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
using System.Collections.Generic;

namespace IMES.Activity
{
    /// <summary>
    /// 產生Docking下的CustomerSN號相关逻辑
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
	public partial class GenerateCustomerSnForDocking: BaseActivity
	{
        //2012-6-20,0123456789BCDFGHJKLMNPQRSTVWXYZ,起始值：Z000,序列号第一位Z使用完之后用X、W、Y
        //转换数组（31进制）
        private static string[] numLst = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9",
                                           "B", "C", "D", "F", "G", "H", "J",
                                           "K", "L", "M", "N", "P", "Q", "R", "S", "T",
                                           "V", "W", "X", "Y", "Z"};

        /// <summary>
        /// 构造函数
        /// </summary>
        public GenerateCustomerSnForDocking()
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
                maxnum = numControl.GetMaxNumber("CPQSNO", prestr + "{0}");
                if (string.IsNullOrEmpty(maxnum))
                {
                    maxnum = "Z000";    //起始值：Z000
                    addflag = true;
                }
                else
                {
                    //maxnum="CNU248000Y";
                    string temstr = "Z000"; //起始值：Z000
                    string numstr = maxnum.Substring(maxnum.Length - 4);
                    temstr = numstr;

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
            CurrentSession.AddValue(Session.SessionKeys.PrintLogBegNo, product.ProId);
            CurrentSession.AddValue(Session.SessionKeys.PrintLogEndNo, product.ProId);
            CurrentSession.AddValue(Session.SessionKeys.PrintLogDescr, product.CUSTSN);
            
            return base.DoExecute(executionContext);
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
