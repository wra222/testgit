// 2010-01-28 Liu Dong(eB1-4)         Modify ITC-1122-0005
// 2010-04-28 Liu Dong(eB1-4)         Modify ITC-1122-0291
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
using IMES.FisObject.Common.Model;
using IMES.FisObject.Common.NumControl;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.Utility.Generates.impl;
using System.Transactions;
using IMES.Infrastructure.UnitOfWork;
using IMES.FisObject.FA.Product;

namespace IMES.Activity
{
    /// <summary>
    /// 产生Customer Sn
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseAcquireActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///         应用于022GenerateCustomerSN.xoml
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.获取CustomerSn最大值;
    ///         2.产生指定机型的CustomerSn;
    ///         3.更新CustomerSn最大值;
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         SessionKeys.ModelObj
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         无
    /// </para> 
    ///<para> 
    /// 输出：
    ///         Session.CustomSnList
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
    public partial class AcquireCustomerSnSection : BaseAcquireActivity
	{
        private static object _syncRoot_GetSeq = new object();

		public AcquireCustomerSnSection()
		{
			InitializeComponent();
		}

        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            return base.Execute(executionContext);
        }

        public ActivityExecutionStatus DoExecuteO()
        {
            return DoExecute(null);
        }

        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {

            SNTemplateManager snTemplatesManager = SNTemplateManager.GetInstance();

            //this.AddTemplatesOfAType("CustomerSn", new IConverter[] { new MonthConverterOneBitCodeXYZ(Session.SessionKeys.presentTime),
            //                                                          new YearConverterNormal(1,'0',Session.SessionKeys.presentTime),
            //                                                          new SequenceConverterNormal("0123456789",6,"999999","012345",'0'),
            //                                                          new ProductionSiteConverter(1,'0',Session.SessionKeys.ProductionSite),
            //                                                          new SFGCustomizingSiteConverter(1,'0',Session.SessionKeys.SFGCustomizingSite)
            //                                                            });

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

            CurrentSession.AddValue(Session.SessionKeys.SFGCustomizingSite, this.getSFGCustomizingSite((Model)CurrentSession.GetValue(Session.SessionKeys.ModelObj)));
            CurrentSession.AddValue(Session.SessionKeys.presentTime, DateTime.Now);
            CurrentSession.AddValue(Session.SessionKeys.ProductionSite, "Q");//TSB目前写死为Q????

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

            int qty = (int)CurrentSession.GetValue(Session.SessionKeys.Qty);

            SequencialNumberRanges snrs = getSequence(CurrentSession, preSeqStr, qty, false, seqCvt);

            CurrentSession.AddValue(Session.SessionKeys.SerialNumbersGenerated, snrs);

            CurrentSession.AddValue(Session.SessionKeys.CustomSnList, this.GetAllNumbersInRange(snrs, seqCvt, qty, preSeqStr));

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
            return GeneratesConstants.CustomerSn;
        }

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
                        SqlConnectionManager.Begin();

                        bool insOrUpd = true;
                        string maxMo = numCtrlRepository.GetMaxNumber(GeneratesConstants.MappingToStandard(type), this.ExtendWildcardToEnd(preSeqStr));//, this.Customer);//2011-09-27 不考虑Customer
                        string seq = string.Empty;
                        SequencialNumberSegment cnbs = new SequencialNumberSegment();
                        if (string.IsNullOrEmpty(maxMo))
                        {
                            seq = seqCvt.NumberRule.MinNumber;
                            insOrUpd = true;
                        }
                        else
                        {
                            seq = this.CutOutSeqOnlyPre(preSeqStr, maxMo, seqCvt.NumberRule.iBits);
                            insOrUpd = false;
                        }

                        cnbs.NumberBegin = seqCvt.Convert(string.Format(preSeqStr, seqCvt.NumberRule.IncreaseToNumber(seq, 1)));
                        cnbs.NumberEnd = seqCvt.Convert(string.Format(preSeqStr, seqCvt.NumberRule.IncreaseToNumber(seq, quantity)));

                        numCtrlRepository.SaveMaxNumberWithOutByCustomer(GetANumControl(GeneratesConstants.MappingToStandard(type), string.Empty/*subruleName*/, cnbs.NumberEnd, this.Customer), insOrUpd, this.ExtendWildcardToEnd(preSeqStr));//2011-09-27 不考虑Customer
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

        protected string getSFGCustomizingSite(Model mdl)
        {
            #region . Old . 
            ////Blank for FG shipment(ModelInfo.Name=’Type’,Value=’FG’)

            ////若非FG，
            //// G : Model.Region = TRO 
            //// H : Model.Region = TIH 
            //// J : Model.Region = OME
            //// U : Model.Region = TAIS

            ////Blank for FG shipment(ModelInfo.Name='Type',Value='FG')
            //string val = mdl.GetAttribute("Type");
            //if (val == "FG")
            //    return string.Empty;

            //if (mdl.Region == null)
            //    return string.Empty;

            ////若非FG,
            ////G : Model.Region = TRO 
            ////H : Model.Region = TIH 
            ////J : Model.Region = OME
            ////U : Model.Region = TAIS
            //switch (mdl.Region)
            //{
            //    case "TRO": return "G";
            //    case "TIH": return "H";
            //    case "OME": return "J";
            //    case "TAIS": return "U";
            //}
            //return string.Empty;
            #endregion

            //对于非FG 机型，则需要按照如下方法确定SFG Customizing Site Code:
            //参考方法：
            //DECLARE @SFGCustomizingSiteCode varchar(1)
            //SET @SFGCustomizingSiteCode = NULL
            //SELECT @SFGCustomizingSiteCode = ISNULL(InfoValue, '')
            //    FROM SFGSite
            //    WHERE InfoType = 'PN' AND InfoValue = SUBSTRING(@CustPN, 6, 1)

            //SET @SFGCustomizingSiteCode = ISNULL(@SFGCustomizingSiteCode, '')

            //如何判断FG/SFG 机型
            //如果Model 的CustPN 的第七位为’-‘，则为FG机型，否则为SFG 机型
            //参考方法：
            //SELECT @CustPN = CustPN FROM Model WHERE Model = @Model
            //IF SUBSTRING(@CustPN, 7, 1) = '-'
            //    PRINT 'FG'
            //ELSE
            //    PRINT 'SFG'

            if (mdl.CustPN != null && mdl.CustPN.Length > 6 && mdl.CustPN.Substring(6, 1).Equals("-"))
            {
                return string.Empty;
            }
            else if (mdl.CustPN != null && mdl.CustPN.Length > 5)// 2010-04-28 Liu Dong(eB1-4)         Modify ITC-1122-0291
            {
                IProductRepository prodRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
                return prodRepository.GetSFGCustomizingSiteCode("PN", mdl.CustPN.Substring(5, 1));
            }
            else
            {
                return string.Empty;
            }
        }
	}
}
