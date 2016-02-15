/*
 * INVENTEC corporation (c)2012 all rights reserved. 
 * Description: Acquire EEP
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2012-01-18   Kerwin            Create 
 * 2012-02-14   Yuan XiaoWei      ITC-1360-0421
 * 
 * Known issues:Any restrictions about this file 
 */
using System;
using System.Linq;
using System.Workflow.ComponentModel;
using IMES.Infrastructure;
using IMES.Infrastructure.Utility.Generates.intf;
using IMES.FisObject.Common.NumControl;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.Utility.Generates.impl;
using IMES.FisObject.Common.Part;
using IMES.FisObject.PCA.MB;
using IMES.Infrastructure.UnitOfWork;

namespace IMES.Activity
{
    /// <summary>
    /// 产生指定机型的EEP
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
    ///         1.获取EEP最大值;
    ///         2.产生指定机型的EEP;
    ///         3.更新EEP最大值;
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         SessionKeys.MB
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         无
    /// </para> 
    ///<para> 
    /// 输出：
    ///         Session.EEPList
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
    public partial class AcquireEEP : BaseActivity
    {
        /// <summary>
        /// 
        /// </summary>
        public AcquireEEP()
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
            MB currenMB = CurrentSession.GetValue(Session.SessionKeys.MB) as MB;

            IPartRepository CurrentPartRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            string cust = CurrentPartRepository.GetPartInfoValue(currenMB.PCBModelID, "Cust");
            string[] CustCondition = { "ZELDA", "HALFPIPE", "MOSELEY" };

            if (CustCondition.Contains(cust))
            {
                IMBRepository currentMBRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();
                string eepValue = currentMBRepository.GetPCBInfoValue(currenMB.Sn, "EEPROM");
                if (string.IsNullOrEmpty(eepValue))
                {
                    try
                    {
                        SqlTransactionManager.Begin();
                        lock (_syncRoot_GetSeq)
                        {
                            INumControlRepository numCtrlRepository = RepositoryFactory.GetInstance().GetRepository<INumControlRepository, NumControl>();
                            string mbFamily = CurrentSession.GetValue(Session.SessionKeys.FamilyName) as string;
                            MACRange currentRange = numCtrlRepository.GetMACRange(mbFamily, new string[] { "R", "A" });
                            if (currentRange == null)
                            {
                                throw new FisException("ICT015", new string[] { });
                            }
                            else
                            {

                                NumControl currentMaxNum = numCtrlRepository.GetMaxValue("EEPROM", mbFamily);
                                if (currentMaxNum == null)
                                {
                                    currentMaxNum = new NumControl();
                                    currentMaxNum.NOName = mbFamily;
                                    currentMaxNum.NOType = "EEPROM";
                                    currentMaxNum.Value = currentRange.BegNo.Substring(0, 11) + "1";
                                    currentMaxNum.Customer = "";

                                    IUnitOfWork uof = new UnitOfWork();
                                    numCtrlRepository.InsertNumControlDefered(uof, currentMaxNum);
                                    if (currentMaxNum.Value.Substring(0, 11) == currentRange.EndNo.Substring(0, 11))
                                    {
                                        numCtrlRepository.SetMACRangeStatusDefered(uof, currentRange.ID, MACRange.MACRangeStatus.Closed);
                                    }
                                    else
                                    {
                                        numCtrlRepository.SetMACRangeStatusDefered(uof, currentRange.ID, MACRange.MACRangeStatus.Active);
                                    }
                                    uof.Commit();
                                    SqlTransactionManager.Commit();
                                    CurrentSession.AddValue(Session.SessionKeys.EEP, currentMaxNum.Value);
                                }
                                else
                                {
                                    if (currentMaxNum.Value.Substring(0, 11) == currentRange.EndNo.Substring(0, 11))
                                    {
                                        numCtrlRepository.SetMACRangeStatus(currentRange.ID, MACRange.MACRangeStatus.Closed);
                                        currentRange = numCtrlRepository.GetMACRange(mbFamily, new string[] { "R" });
                                        if (currentMaxNum.Value.Substring(0, 11) == currentRange.BegNo.Substring(0, 11) || currentMaxNum.Value.Substring(0, 11) == currentRange.EndNo.Substring(0, 11))
                                        {
                                            throw new FisException("ICT017", new string[] { currentMaxNum.Value.Substring(0, 11) });
                                        }
                                    }

                                    if (string.Compare(currentMaxNum.Value.Substring(0, 11), currentRange.BegNo.Substring(0, 11)) < 0 || string.Compare(currentMaxNum.Value.Substring(0, 11), currentRange.EndNo.Substring(0, 11)) > 0)
                                    {
                                        currentMaxNum.Value = currentRange.BegNo.Substring(0, 11) + "1";
                                    }
                                    else
                                    {
                                        ISequenceConverter seqCvt = new SequenceConverterNormal("0123456789", 7, "9999999", "0000000", '0');
                                        string sequenceNumber = currentMaxNum.Value.Substring(4, 7);
                                        sequenceNumber = seqCvt.NumberRule.IncreaseToNumber(sequenceNumber, 1);
                                        currentMaxNum.Value = currentMaxNum.Value.Substring(0, 4) + sequenceNumber + "1";
                                    }


                                    IUnitOfWork uof = new UnitOfWork();
                                    numCtrlRepository.Update(currentMaxNum, uof);
                                    if (currentMaxNum.Value.Substring(0, 11) == currentRange.EndNo.Substring(0, 11))
                                    {
                                        numCtrlRepository.SetMACRangeStatusDefered(uof, currentRange.ID, MACRange.MACRangeStatus.Closed);
                                    }
                                    else
                                    {
                                        numCtrlRepository.SetMACRangeStatusDefered(uof, currentRange.ID, MACRange.MACRangeStatus.Active);
                                    }
                                    uof.Commit();
                                    SqlTransactionManager.Commit();
                                    CurrentSession.AddValue(Session.SessionKeys.EEP, currentMaxNum.Value);
                                }
                            }
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
            }

            return base.DoExecute(executionContext);
        }


        private static object _syncRoot_GetSeq = new object();

    }
}
