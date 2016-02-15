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
            string check = (string)CurrentSession.GetValue(Session.SessionKeys.CN);
            string model = (string)CurrentSession.GetValue(Session.SessionKeys.ModelName);
            if ((check != null && check == "RCTO") || 
                ((model != null) && (model.IndexOf("173") == 0 || model.IndexOf("146") == 0)))
            {
                IMBRepository CurrentMBRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();
                IModelRepository CurrentModelRepository = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();
                var CurrentMB = (MB)CurrentSession.GetValue(Session.SessionKeys.MB);
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
                            currentMaxNum = new NumControl();
                            currentMaxNum.NOName = currentMBCFG.CFG;
                            currentMaxNum.NOType = "MBCT";
                            currentMaxNum.Value = thisYear + weekCode + first.code + beginNO;
                            currentMaxNum.Customer = "";
                            numCtrlRepository.InsertNumControl(currentMaxNum);
                            SqlTransactionManager.Commit();
                            CurrentSession.AddValue("MBCT", currentMBCFG.CFG + "00" + first.code + weekCode + beginNO);
                        }
                        else if (currentMaxNum.Value.Substring(0, 6) != (thisYear + weekCode))
                        {
                            IProductRepository currentProductRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
                            SupplierCodeInfo first = currentProductRepository.GetSupplierCodeByVendor("MB");
                            if (first == null)
                            {
                                throw new FisException("ICT013", new string[] { });
                            }

                            currentMaxNum.Value = thisYear + weekCode + first.code + beginNO;
                            IUnitOfWork uof = new UnitOfWork();
                            numCtrlRepository.Update(currentMaxNum, uof);
                            uof.Commit();
                            SqlTransactionManager.Commit();
                            CurrentSession.AddValue("MBCT", currentMBCFG.CFG + "00" + first.code + weekCode + beginNO);
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

                                currentMaxNum.Value = thisYear + weekCode + first.code + beginNO;
                                IUnitOfWork uof = new UnitOfWork();
                                numCtrlRepository.Update(currentMaxNum, uof);
                                uof.Commit();
                                SqlTransactionManager.Commit();
                                CurrentSession.AddValue("MBCT", currentMBCFG.CFG + "00" + first.code + weekCode + beginNO);
                            }
                            else
                            {
                                ISequenceConverter seqCvt = new SequenceConverterNormal("0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ", 3, "ZZZ", beginNO, '0');
                                string sequenceNumber = currentMaxNum.Value.Substring(8, 3);
                                sequenceNumber = seqCvt.NumberRule.IncreaseToNumber(sequenceNumber, 1);

                                currentMaxNum.Value = currentMaxNum.Value.Substring(0, 8) + sequenceNumber;
                                IUnitOfWork uof = new UnitOfWork();
                                numCtrlRepository.Update(currentMaxNum, uof);
                                uof.Commit();
                                SqlTransactionManager.Commit();
                                CurrentSession.AddValue("MBCT", currentMBCFG.CFG + "00" + currentMaxNum.Value.Substring(6, 2) + weekCode + sequenceNumber);
                            }
                        }
                    }
                    /*string mbct = CurrentSession.GetValue("MBCT") as string;
                    if (!string.IsNullOrEmpty(mbct))
                    {
                        IMES.FisObject.PCA.MB.MBInfo mbctInfo = new IMES.FisObject.PCA.MB.MBInfo(0, CurrentMB.Sn, "MBCT", mbct, Editor, DateTime.Now, DateTime.Now);
                        CurrentMB.AddMBInfo(mbctInfo);
                    }*/
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
            else
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
                                currentMaxNum = new NumControl();
                                currentMaxNum.NOName = currentMBCFG.CFG;
                                currentMaxNum.NOType = "MBCT";
                                currentMaxNum.Value = thisYear + weekCode + first.code + beginNO;
                                currentMaxNum.Customer = "";
                                numCtrlRepository.InsertNumControl(currentMaxNum);
                                SqlTransactionManager.Commit();
                                CurrentSession.AddValue(Session.SessionKeys.MBCT, currentMBCFG.CFG + "00" + first.code + weekCode + beginNO);
                            }
                            else if (currentMaxNum.Value.Substring(0, 6) != (thisYear + weekCode))
                            {
                                IProductRepository currentProductRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
                                SupplierCodeInfo first = currentProductRepository.GetSupplierCodeByVendor("MB");
                                if (first == null)
                                {
                                    throw new FisException("ICT013", new string[] { });
                                }

                                currentMaxNum.Value = thisYear + weekCode + first.code + beginNO;
                                IUnitOfWork uof = new UnitOfWork();
                                numCtrlRepository.Update(currentMaxNum, uof);
                                uof.Commit();
                                SqlTransactionManager.Commit();
                                CurrentSession.AddValue(Session.SessionKeys.MBCT, currentMBCFG.CFG + "00" + first.code + weekCode + beginNO);
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

                                    currentMaxNum.Value = thisYear + weekCode + first.code + beginNO;
                                    IUnitOfWork uof = new UnitOfWork();
                                    numCtrlRepository.Update(currentMaxNum, uof);
                                    uof.Commit();
                                    SqlTransactionManager.Commit();
                                    CurrentSession.AddValue(Session.SessionKeys.MBCT, currentMBCFG.CFG + "00" + first.code + weekCode + beginNO);
                                }
                                else
                                {
                                    ISequenceConverter seqCvt = new SequenceConverterNormal("0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ", 3, "ZZZ", beginNO, '0');
                                    string sequenceNumber = currentMaxNum.Value.Substring(8, 3);
                                    sequenceNumber = seqCvt.NumberRule.IncreaseToNumber(sequenceNumber, 1);

                                    currentMaxNum.Value = currentMaxNum.Value.Substring(0, 8) + sequenceNumber;
                                    IUnitOfWork uof = new UnitOfWork();
                                    numCtrlRepository.Update(currentMaxNum, uof);
                                    uof.Commit();
                                    SqlTransactionManager.Commit();
                                    CurrentSession.AddValue(Session.SessionKeys.MBCT, currentMBCFG.CFG + "00" + currentMaxNum.Value.Substring(6, 2) + weekCode + sequenceNumber);
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
