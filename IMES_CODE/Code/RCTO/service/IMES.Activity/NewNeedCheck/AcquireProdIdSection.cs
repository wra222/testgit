using System;
using System.Collections.Generic;
using System.Transactions;
using System.Workflow.ComponentModel;
using IMES.FisObject.Common.NumControl;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.UnitOfWork;
using IMES.Infrastructure.Utility.Generates;
using IMES.Infrastructure.Utility.Generates.intf;
using IMES.Infrastructure.Utility.Generates.impl;

namespace IMES.Activity
{
    /// <summary>
    /// 产生指定机型的Product Id
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseAcquireActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///         应用于TravelCardPrint.xoml
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.获取Product Id最大值;
    ///         2.产生指定机型的Product Id;
    ///         3.更新Product Id最大值;
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.IsNextMonth
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         无
    /// </para> 
    ///<para> 
    /// 输出：
    ///         Session.ProdNoList
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///         更新NumControl
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         IMBMORepository
    /// </para> 
    /// </remarks>
    public partial class AcquireProdIdSection : BaseAcquireActivity
	{
        private static object _syncRoot_GetSeq = new object();

        /// <summary>
        /// </summary>
		public AcquireProdIdSection()
		{
			InitializeComponent();
		}

        /// <summary>
        /// </summary>		
        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            return base.Execute(executionContext);
        }


        /// <summary>
        /// </summary>		
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            SNTemplateManager snTemplatesManager = SNTemplateManager.GetInstance();

            string cityType = (string)CurrentSession.GetValue("CityType");
            if (string.IsNullOrEmpty(cityType))
            {
                List<string> param = new List<string>();
                param.Add("Input City Code!");
                throw new FisException("GEN023", param);
            }
            else if(cityType == "sh")
            {
                this.AddTemplatesOfAType("ProdId", new IConverter[] { new LineConverter(1,'0',Session.SessionKeys.LineCode),
                                                                  new YearConverterNormal(1,'0',Session.SessionKeys.presentTime),
                                                                  new MonthConverterOneBitCode(Session.SessionKeys.presentTime),                                                                  
                                                                  new SequenceConverterNormal("0123456789",6,"499999","300000",'0')
                                                                });
            }
            else if (cityType == "cq")
            {
                this.AddTemplatesOfAType("ProdId", new IConverter[] { new LineConverter(1,'0',Session.SessionKeys.LineCode),
                                                                  new YearConverterNormal(1,'0',Session.SessionKeys.presentTime),
                                                                  new MonthConverterOneBitCode(Session.SessionKeys.presentTime),                                                                  
                                                                  new SequenceConverterNormal("0123456789",6,"999999","500000",'0')
                                                                });
            }
            else
            {
                List<string> param = new List<string>();
                param.Add("City Code Invaild!");                
                throw new FisException("GEN023", param);
            }
            
            //InitTemplates(executionContext, GetClass(), "GEN_" + GetClass(), GetClass());

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

            CurrentSession.AddValue(Session.SessionKeys.LineCode, Line);

            DateTime dt = DateTime.Now;
            bool isNextMonth = (bool)CurrentSession.GetValue(Session.SessionKeys.IsNextMonth);
            CurrentSession.AddValue(Session.SessionKeys.presentTime, isNextMonth ? dt.AddMonths(1) : dt);

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
            //preSeqStr = preSeqStr;

            int qty = (int)CurrentSession.GetValue(Session.SessionKeys.Qty);

            SequencialNumberRanges snrs = getSequence(CurrentSession, preSeqStr, qty, false, seqCvt);

            CurrentSession.AddValue(Session.SessionKeys.SerialNumbersGenerated, snrs);

            CurrentSession.AddValue(Session.SessionKeys.ProdNoList, this.GetAllNumbersInRange(snrs, seqCvt, qty, preSeqStr));

            return base.DoExecute(executionContext);
        }


        /// <summary>
        /// </summary>		
        protected internal override System.Workflow.ComponentModel.Activity GetInheritedClassInst()
        {
            return this;
        }

        /// <summary>
        /// </summary>		
        protected override string GetClass()
        {
            return GeneratesConstants.ProdId;
        }

        /// <summary>
        /// </summary>		
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
                        SqlTransactionManager.Begin();

                        bool insOrUpd = true;
                        //string maxMo = pdtRepository.GetMaxProductId(preSeqStr);
                        string maxMo = numCtrlRepository.GetMaxNumber(GeneratesConstants.MappingToStandard(type), preSeqStr, this.Customer);
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
                        numCtrlRepository.SaveMaxNumber(GetANumControl(GeneratesConstants.MappingToStandard(type), string.Empty/*subruleName*/, cnbs.NumberEnd, this.Customer), insOrUpd, preSeqStr);
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
