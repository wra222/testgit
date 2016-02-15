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
using System.Text.RegularExpressions;
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
using IMES.FisObject.Common.Part;
using System.Collections.Generic;
using IMES.DataModel;
using IMES.FisObject.PCA.MB;
using IMES.Common;

namespace IMES.Activity
{
    /// <summary>
    /// 產生 KPCT號相关逻辑
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
    ///         
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
    ///         
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         IProduct
    ///         IProductRepository
    /// </para> 
    /// </remarks>
	public partial class GenSkodaSN: BaseActivity
	{
        //转换数组（10进制）
        private static string[] numLst = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };

        private static object _syncRoot_GetSeq = new object();

        /// <summary>
        /// 构造函数
        /// </summary>
        public GenSkodaSN()
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
            Session session = CurrentSession;
            IProduct product = (IProduct)session.GetValue(Session.SessionKeys.Product);
            IMB mb = (IMB)session.GetValue(Session.SessionKeys.MB);
            IProductRepository productRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            IMBRepository mbRep = RepositoryFactory.GetInstance().GetRepository<IMBRepository>();
            IPartRepository partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            IModelRepository modelRepository = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();
            ActivityCommonImpl utl = ActivityCommonImpl.Instance;

            string customSN = null;
            string customer = null;
           string family =null;
           if (product==null)
           {
               if (mb == null)
               {
                   family = utl.IsNull<string>(session, Session.SessionKeys.FamilyName);
               }
               else
               {
                   family = mb.MBCode;
                   customer = mb.Customer;
               }
            }
            else
            {
                family =  product.Family;
                customer = product.Customer;
            }

           if (string.IsNullOrEmpty(customer))
           {
               customer = this.Customer;
           }

            ConstValueInfo info = new ConstValueInfo();
            info.type = "CustSNExceptRule";
            info.name = family;
            IList<ConstValueInfo> retList = partRepository.GetConstValueInfoList(info);
            if (retList == null || retList.Count == 0)
            {
                // 請维护 ConstValue的 CustSNExceptRule
                //throw new FisException("CHK1076", new string[] { "ConstValue", "CustSNExceptRule" });
                string seqFormat = null;
              
                IList<ConstValueInfo> valueList = utl.ConstValue("CustSNRule", family, out seqFormat);
                string nextNum = utl.GenSN.GetNextSequence(product, customer, "CUSTSN",
                                                                           seqFormat);
                customSN = nextNum;
                if (product!=null)
                {
                    productRep.ExistsCustomSnThrowErrorDefered(session.UnitOfWork, product.ProId, customSN);
                    product.CUSTSN = nextNum;
                    productRep.Update(product, session.UnitOfWork);
                }
                else if (mb != null)
                {
                    mbRep.ExistsCustomSnThrowErrorDefered(session.UnitOfWork,mb.Sn,customSN);
                    mb.CustSn = nextNum;
                    mbRep.Update(mb, session.UnitOfWork);
                }

            }
            else
            {
                #region for Skoda model
                if (product == null)
                {
                    throw new FisException("CHK975", new List<string> { Session.SessionKeys.Product });
                }

                string skodaFamily4 = retList[0].value;
                if (string.IsNullOrEmpty(skodaFamily4) || skodaFamily4.Length != 4)
                {
                    throw new FisException("CHK1076", new string[] { "ConstValue", "CustSNExceptRule" });
                }

                IList<IMES.FisObject.Common.Model.ModelInfo> infoModel = modelRepository.GetModelInfoByModelAndName(product.Model, "Country");
                if (infoModel == null || infoModel.Count == 0)
                {
                    throw new FisException("CHK1076", new string[] { "ModelInfo", "Country" });
                }

                info = new ConstValueInfo();
                info.type = "SKODAConutryCode";
                info.name = infoModel[0].Value;
                retList = partRepository.GetConstValueInfoList(info);
                if (retList == null || retList.Count == 0)
                {
                    // 請维护 ConstValue的 SKODAConutryCode
                    throw new FisException("CHK1076", new string[] { "ConstValue", "SKODAConutryCode" });
                }

                string skodaCountry = retList[0].value;
                if (string.IsNullOrEmpty(skodaCountry) || skodaCountry.Length != 5)
                {
                    throw new FisException("CHK1076", new string[] { "ConstValue", "SKODAConutryCode" });
                }

                DateTime curDate = DateTime.Now;
                string year = curDate.Year.ToString();

                string prestr = ""; ;
                // 1~4位:是Family4内容
                prestr = skodaFamily4;

                //  5~9位:根据国别判断,如果不足5位末尾补0
                prestr += skodaCountry;

                // 第10位：年份最后一码
                prestr += year.Substring(year.Length - 1, 1);

                // 第11位：1-9月是数字，10月为A，11月B，12月C
                if (curDate.Month <= 9)
                    prestr += curDate.Month.ToString();
                else if (curDate.Month == 10)
                    prestr += "0";
                else if (curDate.Month == 11)
                    prestr += "A";
                else if (curDate.Month == 12)
                    prestr += "B";

                bool addflag = false;
                string custSn = "";

                try
                {
                    SqlTransactionManager.Begin();
                    lock (_syncRoot_GetSeq)
                    {
                        //从NumControl中获取流水号
                        //GetMaxNumber
                        INumControlRepository numControl = RepositoryFactory.GetInstance().GetRepository<INumControlRepository, NumControl>();
                        //maxnum = numControl.GetMaxNumber("CPQSNO", prestr + "{0}");
                        //var maxObj = numControl.GetMaxNumberObj("CPQSNO", prestr + "{0}");

                        string maxnum = "";
                        var maxObj = numControl.GetMaxValue("SkodaSN", prestr);
                        //檢查有沒有lock index, 沒有lock index, 改變查詢條件
                        if (maxObj == null)
                        {
                            //lock NoType='CPQSNO' and NoName='Lock'
                            var data = numControl.GetMaxValue("SkodaSN", "Lock");
                            maxObj = numControl.GetMaxValue("SkodaSN", prestr);
                        }
                        if (maxObj != null)
                            maxnum = maxObj.Value;



                        if (string.IsNullOrEmpty(maxnum))
                        {
                            maxnum = "000000";    //起始值：000000
                            addflag = true;
                        }
                        else
                        {
                            //maxnum="CNU248000Y";
                            string temstr = "000000"; //起始值：000000

                            string numstr = maxnum.Substring(maxnum.Length - 6);
                            temstr = numstr;

                            if (numstr.ToUpper() == "999999")
                            {
                                FisException fe = new FisException("CHK867", new string[] { });   //流水号已满!
                                throw fe;
                            }

                            maxnum = GetSN(temstr);
                        }

                        custSn = prestr + maxnum.ToUpper();


                        // 自己管理事务开始
                        IUnitOfWork uof = new UnitOfWork();//使用自己的UnitOfWork

                        NumControl item = null;// new NumControl();

                        if (addflag)
                        {
                            item = new NumControl();
                            item.NOType = "SkodaSN";
                            item.Value = custSn;
                            item.NOName = prestr;
                            item.Customer = this.Customer;
                        }
                        else
                        {
                            item = maxObj;
                            item.Value = custSn;
                        }

                        //numControl.SaveMaxNumber(item, addflag, prestr + "{0}");
                        numControl.SaveMaxNumber(item, addflag);
                        customSN = custSn;
                        if (product != null)
                        {
                            product.CUSTSN = custSn;
                            productRep.Update(product, uof);
                        }
                        else if (mb != null)
                        {
                            mb.CustSn = custSn;
                            mbRep.Update(mb, uof);
                        }
                        uof.Commit();  //立即提交UnitOfWork更新NumControl里面的最大值

                        // [Customer SN Print]保存结果：增加更新[CustomerSN_Qty]. CustomerSN_Qty栏位
                        //UPDATE [HPIMES].[dbo].[MO]
                        //SET [CustomerSN_Qty] =[ CustomerSN_Qty]+1
                        //From Product a,MO b  WHERE a.MO=b.MO and a.ProductID=ProductID#
                        if (product != null)
                        {
                            IMORepository moRepository = RepositoryFactory.GetInstance().GetRepository<IMORepository, MO>();
                            moRepository.UpdateMoForIncreaseCustomerSnQty(product.ProId, 1);
                        }
                    }
                    SqlTransactionManager.Commit();//提交事物，释放行级更新锁

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
                #endregion
            }

            session.AddValue(Session.SessionKeys.CustSN, customSN);
            if (product != null)
            {
                session.AddValue(Session.SessionKeys.PrintLogName, product.Customer + "SNO");
                session.AddValue(Session.SessionKeys.PrintLogBegNo, product.CUSTSN);
                session.AddValue(Session.SessionKeys.PrintLogEndNo, product.CUSTSN);
                session.AddValue(Session.SessionKeys.PrintLogDescr, product.ProId);
            }
            else
            {
                session.AddValue(Session.SessionKeys.PrintLogName, this.Customer + "SNO");
                session.AddValue(Session.SessionKeys.PrintLogBegNo, customSN);
                session.AddValue(Session.SessionKeys.PrintLogEndNo, customSN);
                session.AddValue(Session.SessionKeys.PrintLogDescr, family);
            }

            return base.DoExecute(executionContext);
        }

        private string GetSN(string temstr)
        {
            string[] seqLst = new string[6];
            seqLst[0] = temstr.Substring(0, 1);
            seqLst[1] = temstr.Substring(1, 1);
            seqLst[2] = temstr.Substring(2, 1);
            seqLst[3] = temstr.Substring(3, 1);
            seqLst[4] = temstr.Substring(4, 1);
            seqLst[5] = temstr.Substring(5, 1);

            int[] idexLst = getSeqNum(seqLst);

            if (idexLst[5] == 9)
            {
                if (idexLst[4] == 9)
                {
                    if (idexLst[3] == 9)
                    {
                        if (idexLst[2] == 9)
                        {
                            if (idexLst[1] == 9)
                            {
                                if (idexLst[0] == 9)
                                {
                                    List<string> errpara = new List<string>();
                                    throw new FisException("CHK867", errpara);  //流水号已满!
                                }
                                else
                                {
                                    idexLst[0] += 1;
                                    idexLst[1] = 0;
                                    idexLst[2] = 0;
                                    idexLst[3] = 0;
                                    idexLst[4] = 0;
                                    idexLst[5] = 0;
                                }
                            }
                            else
                            {
                                idexLst[1] += 1;
                                idexLst[2] = 0;
                                idexLst[3] = 0;
                                idexLst[4] = 0;
                                idexLst[5] = 0;
                            }
                        }
                        else
                        {
                            idexLst[2] += 1;
                            idexLst[3] = 0;
                            idexLst[4] = 0;
                            idexLst[5] = 0;
                        }
                    }
                    else
                    {
                        idexLst[3] += 1;
                        idexLst[4] = 0;
                        idexLst[5] = 0;
                    }
                }
                else
                {
                    idexLst[4] += 1;
                    idexLst[5] = 0;
                }
            }
            else
            {
                idexLst[5] += 1;
            }

            temstr = numLst[idexLst[0]] + numLst[idexLst[1]] + numLst[idexLst[2]] + numLst[idexLst[3]] + numLst[idexLst[4]] + numLst[idexLst[5]];
            return temstr;
        }

        private int[] getSeqNum(string[] str)
        {
            int[] list = new int[6];
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

                if (str[4] == numLst[l].ToString())
                {
                    list[4] = l;
                    flag += 1;
                }

                if (str[5] == numLst[l].ToString())
                {
                    list[5] = l;
                    flag += 1;
                }

                if (flag == 6)
                {
                    break;
                }
            }
            return list;
        }
    }
}
