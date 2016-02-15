using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;

using IMES.Infrastructure.Utility.Generates;
using IMES.Infrastructure.Utility.Generates.intf;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
//using IMES.FisObject.Common.Station;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure.Utility.Generates.impl;
using IMES.FisObject.Common.NumControl;
using System.Transactions;
using IMES.Infrastructure.UnitOfWork;

namespace IMES.Activity
{
    /// <summary>
    /// 产生指定机型的Gift Number
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseAcquireActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///         应用于059FRUGiftLabelPrint.xoml
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.获取Gift Number最大值;
    ///         2.产生指定机型的Gift Number;
    ///         3.更新Gift Number最大值;
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
    ///         Session.GiftNoList
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
    public partial class AcquireGiftSection : BaseAcquireActivity
	{
        private static object _syncRoot_GetSeq = new object();

        public AcquireGiftSection()
		{
			InitializeComponent();
		}

        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            return base.Execute(executionContext);
        }

        //public ActivityExecutionStatus DoExecuteO()
        //{
        //    return DoExecute(null);
        //}

        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            SNTemplateManager snTemplatesManager = SNTemplateManager.GetInstance();

            //this.AddTemplatesOfAType("GIFT", new IConverter[] {   new YearConverterNormal(2,'0',Session.SessionKeys.presentTime),
            //                                                      new MonthConverter(2,'0',Session.SessionKeys.presentTime),
            //                                                      new SequenceConverterNormal("0123456789",7,"9999999","0000000",'0')
            //                                                    });

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

            DateTime dt = DateTime.Now;
            //bool isNextMonth = (bool)CurrentSession.GetValue(Session.SessionKeys.IsNextMonth);
            //CurrentSession.AddValue(Session.SessionKeys.presentTime, isNextMonth ? dt.AddMonths(1) : dt);
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

            int qty = 1;// (int)CurrentSession.GetValue(Session.SessionKeys.Qty);

            SequencialNumberRanges snrs = getSequence(CurrentSession, preSeqStr, qty, false, seqCvt);

            CurrentSession.AddValue(Session.SessionKeys.SerialNumbersGenerated, snrs);

            CurrentSession.AddValue(Session.SessionKeys.GiftNoList, this.GetAllNumbersInRange(snrs, seqCvt, qty, preSeqStr));

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

        protected internal override System.Workflow.ComponentModel.Activity GetInheritedClassInst()
        {
            return this;
        }

        protected override string GetClass()
        {
            return GeneratesConstants.GIFT;
        }

        protected override SequencialNumberRanges getSequence(Session sess, string preSeqStr, int quantity, bool seqRestart, ISequenceConverter seqCvt)
        {
            SequencialNumberRanges ret = new SequencialNumberRanges();

            //IProductRepository pdtRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            INumControlRepository numCtrlRepository = RepositoryFactory.GetInstance().GetRepository<INumControlRepository, NumControl>();

            string type = GetClass();
            //string subruleName = GetClass();
            
            lock (_syncRoot_GetSeq)
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
                {
                    try
                    {
                        SqlConnectionManager.Begin();

                        bool insOrUpd = true;
                        //string maxMo = pdtRepository.GetMaxProductId(preSeqStr);
                        string maxMo = numCtrlRepository.GetMaxNumber(GeneratesConstants.MappingToStandard(type), preSeqStr);//, this.Customer);//2011-09-27 不考虑Customer
                        string seq = string.Empty;
                        SequencialNumberSegment cnbs = new SequencialNumberSegment();
                        if (string.IsNullOrEmpty(maxMo))
                        {
                            seq = seqCvt.NumberRule.MinNumber;
                            insOrUpd = true;
                        }
                        else
                        {
                            seq = this.CutOutSeq(preSeqStr, maxMo);
                            insOrUpd = false;
                        }

                        cnbs.NumberBegin = seqCvt.Convert(string.Format(preSeqStr, seqCvt.NumberRule.IncreaseToNumber(seq, 1)));
                        cnbs.NumberEnd = seqCvt.Convert(string.Format(preSeqStr, seqCvt.NumberRule.IncreaseToNumber(seq, quantity)));

                        //pdtRepository.SetMaxProductId((string)sess.GetValue(Session.SessionKeys.SMTMONO), cnbs.NumberEnd);
                        numCtrlRepository.SaveMaxNumberWithOutByCustomer(GetANumControl(GeneratesConstants.MappingToStandard(type), string.Empty/*subruleName*/, cnbs.NumberEnd, this.Customer), insOrUpd, preSeqStr);//2011-09-27 不考虑Customer
                        ret.Ranges = new SequencialNumberSegment[] { (SequencialNumberSegment)cnbs };

                        SqlConnectionManager.Commit();
                    }
                    catch (Exception)
                    {
                        SqlConnectionManager.Rollback();
                        throw;
                    }
                    finally
                    {
                        SqlConnectionManager.Dispose();
                        SqlConnectionManager.End();
                    }
                }
            }
            return ret;
        }
	}
}
