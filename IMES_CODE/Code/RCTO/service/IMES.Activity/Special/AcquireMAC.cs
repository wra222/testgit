// INVENTEC corporation (c)2012 all rights reserved. 
// Description: 根据Session中保存的MB产生指定机型的MAC
//                    
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2012-01-12   Yuan XiaoWei                 create
// 2012-02-14   Yuan XiaoWei                 ITC-1360-0418
// 2012-02-14   Yuan XiaoWei                 ITC-1360-0419
// 2012-02-14   Yuan XiaoWei                 ITC-1360-0420
// Known issues:
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
using System;

namespace IMES.Activity
{
    /// <summary>
    /// 产生指定机型的MAC
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
    ///         1.获取MAC最大值;
    ///         2.产生指定机型的MAC;
    ///         3.更新MAC最大值;
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
    ///         Session.MAC
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
    public partial class AcquireMAC : BaseActivity
    {
        /// <summary>
        /// 
        /// </summary>
        public AcquireMAC()
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
            string macValue = CurrentPartRepository.GetPartInfoValue(currenMB.PCBModelID, "MAC");
            string[] CustCondition = { "1", "3", "5", "7", "9", "B", "D", "F" };
            if (macValue == "T")
            {
                string cust = CurrentPartRepository.GetPartInfoValue(currenMB.PCBModelID, "Cust");
                if (string.IsNullOrEmpty(cust))
                {
                    throw new FisException("ICT016", new string[] { currenMB.PCBModelID });
                }
                string maxMAC = GetMAC(cust);
                if (string.IsNullOrEmpty(maxMAC))
                {
                    throw new FisException("ICT008", new string[] { });
                }
                if (cust == "VB 1.0" && CustCondition.Contains(maxMAC.Substring(maxMAC.Length - 1, 1)))
                {
                    int maxLoop = 5;
                    bool getVBMac = false;
                    while (maxLoop > 0)
                    {

                        maxMAC = GetMAC(cust);
                        if (!string.IsNullOrEmpty(maxMAC) && !CustCondition.Contains(maxMAC.Substring(maxMAC.Length - 1, 1)))
                        {
                            maxLoop = 0;
                            getVBMac = true;
                        }
                        else
                        {
                            maxLoop = maxLoop - 1;
                        }
                    }

                    if (!getVBMac)
                    {
                        throw new FisException("ICT008", new string[] { });
                    }
                }
                CheckBoundWithMB(maxMAC);
                CurrentSession.AddValue(Session.SessionKeys.MAC, maxMAC);

            }


            return base.DoExecute(executionContext);
        }

        private void CheckBoundWithMB(string mac)
        {
            IMBRepository mbRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();
            IMB res = mbRepository.GetMBByMAC(mac);
            if (res != null)
            {
                throw new FisException("ICT007", new string[] { mac, res.Sn });
            }
        }

        private string GetMAC(string cust)
        {
            try
            {
                SqlTransactionManager.Begin();
                lock (_syncRoot_GetSeq)
                {
                    INumControlRepository numCtrlRepository = RepositoryFactory.GetInstance().GetRepository<INumControlRepository, NumControl>();

                    MACRange currentRange = numCtrlRepository.GetMACRange(cust, new string[] { "R", "A" });
                    if (currentRange == null)
                    {
                        throw new FisException("ICT014", new string[] { });
                    }
                    else
                    {
                        NumControl currentMaxNum = numCtrlRepository.GetMaxValue("MAC", cust);
                        if (currentMaxNum == null)
                        {
                            currentMaxNum = new NumControl();
                            currentMaxNum.NOName = cust;
                            currentMaxNum.NOType = "MAC";
                            currentMaxNum.Value = currentRange.BegNo;
                            currentMaxNum.Customer = "";

                            IUnitOfWork uof = new UnitOfWork();
                            numCtrlRepository.InsertNumControlDefered(uof, currentMaxNum);
                            if (currentMaxNum.Value == currentRange.EndNo)
                            {
                                numCtrlRepository.SetMACRangeStatusDefered(uof, currentRange.ID, MACRange.MACRangeStatus.Closed);
                            }
                            else
                            {
                                numCtrlRepository.SetMACRangeStatusDefered(uof, currentRange.ID, MACRange.MACRangeStatus.Active);
                            }
                            uof.Commit();
                            SqlTransactionManager.Commit();
                            return currentMaxNum.Value;

                        }
                        else
                        {
                            if (currentMaxNum.Value == currentRange.EndNo)
                            {
                                numCtrlRepository.SetMACRangeStatus(currentRange.ID, MACRange.MACRangeStatus.Closed);
                                currentRange = numCtrlRepository.GetMACRange(cust, new string[] { "R" });
                                if (currentMaxNum.Value == currentRange.BegNo || currentMaxNum.Value == currentRange.EndNo)
                                {
                                    throw new FisException("ICT018", new string[] { currentMaxNum.Value });
                                }
                            }

                            string change34MaxNum = Change34(currentMaxNum.Value);
                            string change34BeginNo = Change34(currentRange.BegNo);
                            string change34EndNo = Change34(currentRange.EndNo);
                            if (string.Compare(change34MaxNum, change34BeginNo) < 0 || string.Compare(change34MaxNum, change34EndNo) > 0)
                            {
                                currentMaxNum.Value = currentRange.BegNo;
                            }
                            else
                            {
                                ISequenceConverter seqCvt = new SequenceConverterNormal("0123456789ABCDEF", 4, "FFFF", "0000", '0');
                                string sequenceNumber = currentMaxNum.Value.Substring(currentMaxNum.Value.Length - 4, 4);
                                string change34Sequence = sequenceNumber.Substring(1, 3).Insert(1, sequenceNumber.Substring(0, 1));
                                change34Sequence = seqCvt.NumberRule.IncreaseToNumber(change34Sequence, 1);
                                sequenceNumber = change34Sequence.Substring(1, 3).Insert(1, change34Sequence.Substring(0, 1));

                                currentMaxNum.Value = currentMaxNum.Value.Substring(0, currentMaxNum.Value.Length - 4) + sequenceNumber;
                            }


                            IUnitOfWork uof = new UnitOfWork();
                            numCtrlRepository.Update(currentMaxNum, uof);
                            if (currentMaxNum.Value == currentRange.EndNo)
                            {
                                numCtrlRepository.SetMACRangeStatusDefered(uof, currentRange.ID, MACRange.MACRangeStatus.Closed);
                            }
                            else
                            {
                                numCtrlRepository.SetMACRangeStatusDefered(uof, currentRange.ID, MACRange.MACRangeStatus.Active);
                            }
                            uof.Commit();
                            SqlTransactionManager.Commit();
                            return currentMaxNum.Value;
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

        private static object _syncRoot_GetSeq = new object();

        private string Change34(string input)
        {
            string sequenceNumber = input.Substring(input.Length - 4, 4);
            string change34Sequence = sequenceNumber.Substring(1, 3).Insert(1, sequenceNumber.Substring(0, 1));
            return input.Substring(0, input.Length - 4) + change34Sequence;
        }

    }
}
