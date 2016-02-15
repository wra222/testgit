using System;
using System.Collections.Generic;
using System.Transactions;
using System.Workflow.ComponentModel;
//using IMES.FisObject.Common.Station;
using IMES.FisObject.Common.NumControl;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.UnitOfWork;
using IMES.Infrastructure.Utility.Generates;
using IMES.Infrastructure.Utility.Generates.intf;

namespace IMES.Activity
{
    /// <summary>
    /// 产生Virtual MO Number
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseAcquireActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///         应用于102VirtualMo.xoml
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.获取Virtual MO Number最大值;
    ///         2.产生指定机型的Virtual MO Number;
    ///         3.更新Virtual MO Number最大值;
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
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
    ///         Session.VirtualMOList
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
    public partial class AcquireVirtualMOSection : BaseAcquireActivity
	{
        private static object _syncRoot_GetSeq = new object();

        /// <summary>
        /// constructor
        /// </summary>
        public AcquireVirtualMOSection()
		{
			InitializeComponent();
		}

        /// <summary>
        /// constructor
        /// </summary>
        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            return base.Execute(executionContext);
        }

        //public ActivityExecutionStatus DoExecuteO()
        //{
        //    return DoExecute(null);
        //}

        /// <summary>
        /// constructor
        /// </summary>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            SNTemplateManager snTemplatesManager = SNTemplateManager.GetInstance();

            //this.AddTemplatesOfAType("VTMO", new IConverter[] { new CommonConverter(1,'0', Session.SessionKeys.VirtualMOIdentifier),
            //                                                    new YearConverterNormal(2,'0',Session.SessionKeys.presentTime),
            //                                                    new MonthConverter(2,'0',Session.SessionKeys.presentTime),
            //                                                    new DayConverterNormal(2,'0',Session.SessionKeys.presentTime),
            //                                                    new SequenceConverterNormal("0123456789",5,"99999","00000",'0')
            //                                                  });

            InitTemplates(executionContext, GetClass(), "GEN_" + GetClass(), GetClass());

            IConverter[] cvts = null;

            string TypeOfSNRule = GetClass();//(string)CurrentSession.GetValue(Session.SessionKeys.TypeOfSNRule);

            SNComposer resSnC = new SNComposer();

            try
            {
                cvts = snTemplatesManager.Gets(GetKey(TypeOfSNRule));
                if (cvts != null && cvts.Length < 1)
                    throw new KeyNotFoundException();
            }
            catch (KeyNotFoundException)
            {
                List<string> param = new List<string>();
                param.Add(GetClass());
                param.Add(TypeOfSNRule);
                throw new FisException("GEN023", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            string prefix =(string) CurrentSession.GetValue(Session.SessionKeys.VirtualMOIdentifier);
            if (string.IsNullOrEmpty(prefix))
            {
                CurrentSession.AddValue(Session.SessionKeys.VirtualMOIdentifier, "V");
                prefix="V";
            }

            DateTime dt = DateTime.Now;
            CurrentSession.AddValue(Session.SessionKeys.presentTime, dt);

            ISequenceConverter seqCvt = null;

            foreach (IConverter cvt in cvts)
            {
                if (cvt is ISequenceConverter)
                {
                    resSnC.Add(new NumberElement(cvt, "{0}"));
                    seqCvt = (ISequenceConverter)cvt;
                }
                else
                    resSnC.Add(new NumberElement(cvt, CurrentSession));
            }

            string preSeqStr = resSnC.Calculate();
            preSeqStr = prefix.Trim() + preSeqStr;

            int qty = 1;//(int)CurrentSession.GetValue(Session.SessionKeys.Qty);//2010-02-04 WHJ: 一次只生成一个, 且SessionKeys.Qty会在其他地方用到,怕引起混乱.

            SequencialNumberRanges snrs = getSequence(CurrentSession, preSeqStr, qty, false, seqCvt);

            CurrentSession.AddValue(Session.SessionKeys.SerialNumbersGenerated, snrs);

            CurrentSession.AddValue(Session.SessionKeys.VirtualMOList, this.GetAllNumbersInRange(snrs, seqCvt, qty, preSeqStr));

            return base.DoExecute(executionContext);//default(ActivityExecutionStatus);
        }

        //protected internal override void PreExecute()
        //{

        //}

        //protected internal override void PostExecute()
        //{

        //}

        //protected internal override void ErrorInExecute(Exception ex)
        //{

        //}

        //protected internal override void FinallyExecute()
        //{

        //}

        /// <summary>
        /// constructor
        /// </summary>
        protected internal override System.Workflow.ComponentModel.Activity GetInheritedClassInst()
        {
            return this;
        }

        /// <summary>
        /// constructor
        /// </summary>
        protected override string GetClass()
        {
            return GeneratesConstants.VTMO;
        }

        /// <summary>
        /// constructor
        /// </summary>
        protected override SequencialNumberRanges getSequence(Session sess, string preSeqStr, int quantity, bool seqRestart, ISequenceConverter seqCvt)
        {
            SequencialNumberRanges ret = new SequencialNumberRanges();

            INumControlRepository numCtrlRepository = RepositoryFactory.GetInstance().GetRepository<INumControlRepository, NumControl>();

            string type = GetClass();
            //string subruleName = GetClass();
            
            lock (_syncRoot_GetSeq)
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
                {
                    try
                    {
                        SqlTransactionManager.Begin();

                        bool insOrUpd = true;
                        string maxMo = numCtrlRepository.GetMaxNumber(GeneratesConstants.MappingToStandard(type), preSeqStr);//, this.Customer);//2011-09-27 不考虑Customer
                        string seq = string.Empty;
                        SequencialNumberSegment cnbs = new SequencialNumberSegment();
                        if (string.IsNullOrEmpty(maxMo))
                        {
                            seq = seqCvt.NumberRule.MinNumber;
                            cnbs.NumberBegin = seqCvt.Convert(string.Format(preSeqStr, seqCvt.NumberRule.IncreaseToNumber(seq, 0)));
                            cnbs.NumberEnd = seqCvt.Convert(string.Format(preSeqStr, seqCvt.NumberRule.IncreaseToNumber(seq, quantity - 1)));
                            insOrUpd = true;
                        }
                        else
                        {
                            seq = this.CutOutSeq(preSeqStr, maxMo);
                            cnbs.NumberBegin = seqCvt.Convert(string.Format(preSeqStr, seqCvt.NumberRule.IncreaseToNumber(seq, 1)));
                            cnbs.NumberEnd = seqCvt.Convert(string.Format(preSeqStr, seqCvt.NumberRule.IncreaseToNumber(seq, quantity)));
                            insOrUpd = false;
                        }
                        numCtrlRepository.SaveMaxNumberWithOutByCustomer(GetANumControl(GeneratesConstants.MappingToStandard(type), string.Empty/*subruleName*/, cnbs.NumberEnd, this.Customer), insOrUpd, preSeqStr);//2011-09-27 不考虑Customer
                        ret.Ranges = new SequencialNumberSegment[] { (SequencialNumberSegment)cnbs };

                        SqlTransactionManager.Commit();
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
                }
            }
            return ret;
        }
	}
}
