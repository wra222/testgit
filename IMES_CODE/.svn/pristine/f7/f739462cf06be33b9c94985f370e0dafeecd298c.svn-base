// INVENTEC corporation (c)2012 all rights reserved. 
// Description: AcquireMBCT2
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2012-08-14   207003               create
// Known issues:

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Workflow.ComponentModel;
using System.Linq;
using IMES.DataModel;
using IMES.FisObject.Common.Model;
using IMES.FisObject.Common.NumControl;
using IMES.FisObject.Common.Part;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PCA.MB;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.UnitOfWork;
using IMES.Infrastructure.Utility.Generates.impl;
using IMES.Infrastructure.Utility.Generates.intf;
using IMES.Common;

namespace IMES.Activity
{
    /// <summary>
    /// 产生指定机型的DCode
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///         应用于
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.获取DCode最大值;
    ///         2.产生指定机型的DCode;
    ///         3.更新DCode最大值;
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         SessionKeys.SelectedWarrantyRuleID
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         无
    /// </para> 
    ///<para> 
    /// 输出：
    ///         Session.DCode
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///         更新NumControl
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         
    /// </para> 
    /// </remarks>
    public partial class AcquireMBCT2 : BaseActivity
    {
        /// <summary>
        /// 
        /// </summary>
        public AcquireMBCT2()
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
            var CurrentProduct = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);
            if (CurrentProduct == null)
            {
                throw new NullReferenceException("Product in session is null");
            }
            string PCBNO = CurrentProduct.PCBID.ToString();
            IMBRepository CurrentMBRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();
            IMB CurrentMB = CurrentMBRepository.Find(PCBNO);

            //IMBRepository CurrentMBRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();
            IModelRepository CurrentModelRepository = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();
            //var CurrentMB = (MB)CurrentSession.GetValue(Session.SessionKeys.MB);


            string check = (string)CurrentSession.GetValue(Session.SessionKeys.CN);
            string model = (string)CurrentSession.GetValue(Session.SessionKeys.ModelName);
            if ((check != null && check == "RCTO") || (model.IndexOf("173") == 0 || model.IndexOf("146") == 0))
            {
                
                IList<IMES.FisObject.Common.Model.ModelInfo> infos = new List<IMES.FisObject.Common.Model.ModelInfo>();
                infos = CurrentModelRepository.GetModelInfoByModelAndName(model, "MN");
                string series = "";
                if (infos == null || infos.Count <= 0)
                {
                    throw new FisException("ICT006", new string[] { });
                }
                else
                {
                    series = infos[0].Value;
                }
                IList<MBCFGDef> currentMBCFGList = new List<MBCFGDef>();
                MBCFGDef currentMBCFG = new MBCFGDef();
                currentMBCFGList = CurrentMBRepository.GetMBCFGByCodeSeriesAndType(CurrentMB.MBCode, series, "RCTO");
                if (currentMBCFGList == null || currentMBCFGList.Count <= 0)
                {
                    throw new FisException("ICT006", new string[] { });
                }
                else
                {
                    currentMBCFG = currentMBCFGList[0];
                }
                //string thisYear = DateTime.Today.Year.ToString("0000");
                string thisYear = "";
                string weekCode = "";
                //Vincent 2014-01-01 fixed  bug : used wrong this year cross year issue
                //IList<string> weekCodeList = CurrentModelRepository.GetCodeFromHPWeekCodeInRangeOfDescr();
                IList<HpweekcodeInfo> weekCodeList = CurrentModelRepository.GetHPWeekCodeInRangeOfDescr();
                if (weekCodeList != null && weekCodeList.Count > 0)
                {
                    weekCode = weekCodeList[0].code;
                    thisYear = weekCodeList[0].descr.Trim().Substring(0, 4);
                }
                else
                {
                    throw new FisException("ICT009", new string[] { });
                }
                try
                {
                    string MBCT4Code = "";
                    SqlTransactionManager.Begin();
                    lock (_syncRoot_GetSeq)
                    {
                        IProductRepository currentProductRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
                        SupplierCodeInfo first = currentProductRepository.GetSupplierCodeByVendor("MB");

                        INumControlRepository numCtrlRepository = RepositoryFactory.GetInstance().GetRepository<INumControlRepository, NumControl>();
                        NumControl currentMaxNum = numCtrlRepository.GetMaxValue("MBCT2", currentMBCFG.CFG);
                        
                        string CheckFamily = CurrentProduct.Family;
                        MBCT4Code = GetMBCT4Code(CheckFamily, PCBNO);

                        if (first == null)
                        {
                            throw new FisException("ICT013", new string[] { });
                        }
                        if (currentMaxNum == null)
                        {
                            currentMaxNum = new NumControl();
                            currentMaxNum.NOName = currentMBCFG.CFG;
                            currentMaxNum.NOType = "MBCT2";
                            if (MBCT4Code == "")
                            {
                                currentMaxNum.Value = thisYear + weekCode + first.code + beginNO;
                            }
                            else
                            {
                                currentMaxNum.Value = thisYear + weekCode + MBCT4Code.Substring(2, 2) + beginNO;
                            }
                            currentMaxNum.Customer = "";
                            numCtrlRepository.InsertNumControl(currentMaxNum);
                            SqlTransactionManager.Commit();
                            if (MBCT4Code == "")
                            {
                                CurrentSession.AddValue("MBCT2", currentMBCFG.CFG + "00" + first.code + weekCode + beginNO);
                            }
                            else
                            {
                                CurrentSession.AddValue("MBCT2", currentMBCFG.CFG + MBCT4Code + weekCode + beginNO);
                            }
                        }
                        else if (currentMaxNum.Value.Substring(0, 6) != (thisYear + weekCode))
                        {
                            if (MBCT4Code == "")
                            {
                                currentMaxNum.Value = thisYear + weekCode + first.code + beginNO;
                            }
                            else
                            {
                                currentMaxNum.Value = thisYear + weekCode + MBCT4Code.Substring(2, 2) + beginNO;
                            }
                            
                            IUnitOfWork uof = new UnitOfWork();
                            numCtrlRepository.Update(currentMaxNum, uof);
                            uof.Commit();
                            SqlTransactionManager.Commit();
                            if (MBCT4Code == "")
                            {
                                CurrentSession.AddValue("MBCT2", currentMBCFG.CFG + "00" + first.code + weekCode + beginNO);
                            }
                            else
                            {
                                CurrentSession.AddValue("MBCT2", currentMBCFG.CFG + MBCT4Code + weekCode + beginNO);
                            }
                            
                        }
                        else
                        {
                            if (currentMaxNum.Value.EndsWith("ZZZ"))
                            {
                                IList<SupplierCodeInfo> codeList = currentProductRepository.GetSupplierCodeListByCode(currentMaxNum.Value.Substring(6, 2));

                                SupplierCodeInfo first2 = currentProductRepository.GetSupplierCodeByVendor("MB", codeList[0].idex);
                                if (first2 == null)
                                {
                                    throw new FisException("ICT005", new string[] { });
                                }
                                if (MBCT4Code == "")
                                {
                                    currentMaxNum.Value = thisYear + weekCode + first2.code + beginNO;
                                }
                                else
                                {
                                    currentMaxNum.Value = thisYear + weekCode + MBCT4Code.Substring(2, 2) + beginNO;
                                }
                                IUnitOfWork uof = new UnitOfWork();
                                numCtrlRepository.Update(currentMaxNum, uof);
                                uof.Commit();
                                SqlTransactionManager.Commit();
                                if (MBCT4Code == "")
                                {
                                    CurrentSession.AddValue("MBCT2", currentMBCFG.CFG + "00" + first2.code + weekCode + beginNO);
                                }
                                else
                                {
                                    CurrentSession.AddValue("MBCT2", currentMBCFG.CFG + MBCT4Code + weekCode + beginNO);
                                }
                            }
                            else
                            {
                                ISequenceConverter seqCvt = new SequenceConverterNormal("0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ", 3, "ZZZ", beginNO, '0');
                                string sequenceNumber = currentMaxNum.Value.Substring(8, 3);
                                sequenceNumber = seqCvt.NumberRule.IncreaseToNumber(sequenceNumber, 1);
                                if (MBCT4Code == "")
                                {
                                    currentMaxNum.Value = currentMaxNum.Value.Substring(0, 8) + sequenceNumber;
                                }
                                else
                                {
                                    currentMaxNum.Value = currentMaxNum.Value.Substring(0, 6) + MBCT4Code.Substring(2,2) + sequenceNumber;
                                }
                                IUnitOfWork uof = new UnitOfWork();
                                numCtrlRepository.Update(currentMaxNum, uof);
                                uof.Commit();
                                SqlTransactionManager.Commit();
                                if (MBCT4Code == "")
                                {
                                    CurrentSession.AddValue("MBCT2", currentMBCFG.CFG + "00" + currentMaxNum.Value.Substring(6, 2) + weekCode + sequenceNumber);
                                }
                                else
                                {
                                    CurrentSession.AddValue("MBCT2", currentMBCFG.CFG + MBCT4Code + weekCode + sequenceNumber);
                                }
                            }
                        }
                    }
                    
                    string mbct2 = CurrentSession.GetValue("MBCT2") as string;
                    if (!string.IsNullOrEmpty(mbct2))
                    {
                        CurrentMB.SetExtendedProperty("MBCT2", mbct2, Editor);
                        CurrentMBRepository.Update(CurrentMB, CurrentSession.UnitOfWork);
                    }
                }
                catch (Exception e)
                {
                    SqlTransactionManager.Rollback();
                    throw e;
                }
                finally
                {
                    SqlTransactionManager.Dispose();
                    SqlTransactionManager.End();
                }
            }

            return base.DoExecute(executionContext);
        }

        private string GetMBCT4Code(string CheckFamily,string PCBNO)
        {
            //IMBRepository CurrentMBRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();
            //IPartRepository iPartRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository>();
            //IList<ConstValueTypeInfo> list = iPartRepository.GetConstValueTypeList("RCTOGenMBCT2RuleFamily");
            //IList<ConstValueTypeInfo> query = (from q in list
            //                                   where q.value == CheckFamily
            //                                   select q).ToList<ConstValueTypeInfo>();
            //string MBCT4Code = "";
            //if (query.Count > 0)
            //{
            //    MBCT4Code = CurrentMBRepository.GetPCBInfoValue(PCBNO, "MBCT").Substring(5, 4);
            //    if (MBCT4Code == null || MBCT4Code == "")
            //    {
            //        throw new FisException("ICT024", new string[] { });
            //    }
            //}
            //return MBCT4Code;
            string MBCT4Code = "";
            IList<ConstValueInfo> retList = ActivityCommonImpl.Instance.GetConstValueListByType("RCTOGenMBCT2RuleMBCode", "Name");
            var list = (from x in retList where x.name == PCBNO.Substring(0,2) select x.value).ToList();
            if (list != null && list.Count > 0)
            {
                MBCT4Code = list[0];
            }

            return MBCT4Code.Trim();
        }


        private static object _syncRoot_GetSeq = new object();

        private readonly string beginNO = "000";
    }
}
