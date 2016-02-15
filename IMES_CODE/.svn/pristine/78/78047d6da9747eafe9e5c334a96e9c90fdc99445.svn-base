// INVENTEC corporation (c)2012 all rights reserved. 
// Description: AcquireMBCT
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2012-01-16   Yuan XiaoWei                 create
// 2012-02-14   Yuan XiaoWei                 ITC-1360-0424
// 2012-02-15   Yuan XiaoWei                 ITC-1360-0425
// 2012-02-15   Yuan XiaoWei                 ITC-1360-0426
// Known issues:

using System;
using System.Collections;
using System.Collections.Generic;
using System.Workflow.ComponentModel;
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
using IMES.Infrastructure.Extend;
using System.ComponentModel;

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
    public partial class AcquireMBCT : BaseActivity
    {
        /// <summary>
        /// 
        /// </summary>
        public AcquireMBCT()
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
            string mbctValue = "T";
            if (!MustGenerate)
            {
                IPartRepository CurrentPartRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
                mbctValue = CurrentPartRepository.GetPartInfoValue(currenMB.PCBModelID, "MBCT");
            }

            if (mbctValue == "T")
            {

                IMBRepository CurrentMBRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();
                MBCFGDef currentMBCFG = CurrentMBRepository.GetMBCFG(currenMB.MBCode, "PC");
                if (currentMBCFG == null || string.IsNullOrEmpty(currentMBCFG.CFG))
                {
                    throw new FisException("ICT006", new string[] { });
                }

                //string thisYear = DateTime.Today.Year.ToString("0000");
                string thisYear = "";
                string weekCode = "";
                IModelRepository CurrentModelRepository = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();
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
                ////Vincent 2013-04-01: checkPCBVersion
                bool HasPCBVer = false;
                string PcbRevision = "00";
                string PcbSupplier = string.Empty;
                if (CurrentSession.GetValue(ExtendSession.SessionKeys.HasPCBVer) != null)
                {
                    HasPCBVer = (bool)CurrentSession.GetValue(ExtendSession.SessionKeys.HasPCBVer);
                    if (HasPCBVer)
                    {
                        PcbRevision = (string)CurrentSession.GetValue(ExtendSession.SessionKeys.Revision);
                        PcbSupplier = (string)CurrentSession.GetValue(ExtendSession.SessionKeys.Supplier);
                    }
                }                

                //////////////////////////////////////////////////////////////////
                try
                {
                    SqlTransactionManager.Begin();
                    lock (_syncRoot_GetSeq)
                    {
                        INumControlRepository numCtrlRepository = RepositoryFactory.GetInstance().GetRepository<INumControlRepository, NumControl>();
                        NumControl currentMaxNum = numCtrlRepository.GetMaxValue("MBCT", currentMBCFG.CFG);
                        if (currentMaxNum == null)
                        {
                            IProductRepository currentProductRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();

                            SupplierCodeInfo first = currentProductRepository.GetSupplierCodeByVendor("MB");
                            if (first == null)
                            {
                                throw new FisException("ICT013", new string[] { });
                            }
                            PcbSupplier = (HasPCBVer ? PcbSupplier : first.code);
                            currentMaxNum = new NumControl();
                            currentMaxNum.NOName = currentMBCFG.CFG;
                            currentMaxNum.NOType = "MBCT";
                            currentMaxNum.Value = thisYear + weekCode + PcbSupplier + beginNO;
                            currentMaxNum.Customer = "";
                            numCtrlRepository.InsertNumControl(currentMaxNum);
                            SqlTransactionManager.Commit();
                            CurrentSession.AddValue(Session.SessionKeys.MBCT, currentMBCFG.CFG + PcbRevision + PcbSupplier + weekCode + beginNO);
                        }
                        else if (currentMaxNum.Value.Substring(0, 6) != (thisYear + weekCode))
                        {
                            IProductRepository currentProductRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
                            SupplierCodeInfo first = currentProductRepository.GetSupplierCodeByVendor("MB");
                            if (first == null)
                            {
                                throw new FisException("ICT013", new string[] { });
                            }
                            PcbSupplier = (HasPCBVer ? PcbSupplier : first.code);
                            currentMaxNum.Value = thisYear + weekCode + PcbSupplier + beginNO;
                            IUnitOfWork uof = new UnitOfWork();
                            numCtrlRepository.Update(currentMaxNum, uof);
                            uof.Commit();
                            SqlTransactionManager.Commit();
                            CurrentSession.AddValue(Session.SessionKeys.MBCT, currentMBCFG.CFG + PcbRevision + PcbSupplier + weekCode + beginNO);
                        }
                        else
                        {
                            if (currentMaxNum.Value.EndsWith("ZZZ"))
                            {
                                IProductRepository currentProductRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
                                IList<SupplierCodeInfo> codeList = currentProductRepository.GetSupplierCodeListByCode(currentMaxNum.Value.Substring(6, 2));

                                SupplierCodeInfo first = currentProductRepository.GetSupplierCodeByVendor("MB", codeList[0].idex);
                                if (first == null)
                                {
                                    throw new FisException("ICT005", new string[] { });
                                }

                                PcbSupplier = (HasPCBVer ? PcbSupplier : first.code);

                                currentMaxNum.Value = thisYear + weekCode + PcbSupplier + beginNO;
                                IUnitOfWork uof = new UnitOfWork();
                                numCtrlRepository.Update(currentMaxNum, uof);
                                uof.Commit();
                                SqlTransactionManager.Commit();
                                CurrentSession.AddValue(Session.SessionKeys.MBCT, currentMBCFG.CFG + PcbRevision + PcbSupplier + weekCode + beginNO);
                            }
                            else
                            {
                                ISequenceConverter seqCvt = new SequenceConverterNormal("0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ", 3, "ZZZ", beginNO, '0');
                                string sequenceNumber = currentMaxNum.Value.Substring(8, 3);
                                sequenceNumber = seqCvt.NumberRule.IncreaseToNumber(sequenceNumber, 1);

                                if (HasPCBVer)
                                {
                                    currentMaxNum.Value = currentMaxNum.Value.Substring(0, 6) + PcbSupplier + sequenceNumber;
                                }
                                else
                                {
                                    currentMaxNum.Value = currentMaxNum.Value.Substring(0, 8) + sequenceNumber;
                                    PcbSupplier = currentMaxNum.Value.Substring(6, 2);
                                }
                                
                                IUnitOfWork uof = new UnitOfWork();
                                numCtrlRepository.Update(currentMaxNum, uof);
                                uof.Commit();
                                SqlTransactionManager.Commit();                                
                                CurrentSession.AddValue(Session.SessionKeys.MBCT, currentMBCFG.CFG + PcbRevision + PcbSupplier + weekCode + sequenceNumber);
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

            return base.DoExecute(executionContext);
        }

        /// <summary>
        ///  必须要生成号
        /// </summary>
        public static DependencyProperty MustGenerateProperty = DependencyProperty.Register("MustGenerate", typeof(bool), typeof(AcquireMBCT));

        /// <summary>
        ///  必须要生成号共有两种True,False
        /// </summary>
        [DescriptionAttribute("MustGenerate")]
        [CategoryAttribute("InArguments Of AcquireMBCT")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        [DefaultValue(false)]
        public bool MustGenerate
        {
            get
            {
                return ((bool)(base.GetValue(AcquireMBCT.MustGenerateProperty)));
            }
            set
            {
                base.SetValue(AcquireMBCT.MustGenerateProperty, value);
            }
        }

        private static object _syncRoot_GetSeq = new object();

        private readonly string beginNO = "000";
    }
}
