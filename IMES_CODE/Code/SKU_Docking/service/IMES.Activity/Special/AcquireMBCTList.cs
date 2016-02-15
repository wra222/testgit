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
    public partial class AcquireMBCTList : BaseActivity
    {
        /// <summary>
        /// 
        /// </summary>
        public AcquireMBCTList()
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
            string mbctValue = CurrentPartRepository.GetPartInfoValue(currenMB.PCBModelID, "MBCT");

            if (mbctValue == "T")
            {
                int multiQty = (int)CurrentSession.GetValue(Session.SessionKeys.MultiQty);
                IList<string> mbctList = new List<string>();

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

                for (int i = 0; i < multiQty; i++)
                {
                    string maxMBCT = GetMBCT(currentMBCFG.CFG, thisYear, weekCode, currentMBCFG.CFG);
                    mbctList.Add(maxMBCT);
                }
                CurrentSession.AddValue(Session.SessionKeys.MBCTList, mbctList);
            }

            return base.DoExecute(executionContext);
        }

        private string GetMBCT(string cfg, string thisYear, string weekCode,string mbcfg)
        {
            try
            {
                SqlTransactionManager.Begin();
                lock (_syncRoot_GetSeq)
                {
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

                    INumControlRepository numCtrlRepository = RepositoryFactory.GetInstance().GetRepository<INumControlRepository, NumControl>();
                    NumControl currentMaxNum = numCtrlRepository.GetMaxValue("MBCT", mbcfg);
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
                        currentMaxNum.NOName = mbcfg;
                        currentMaxNum.NOType = "MBCT";
                        currentMaxNum.Value = thisYear + weekCode + PcbSupplier + beginNO;
                        currentMaxNum.Customer = "";
                        numCtrlRepository.InsertNumControl(currentMaxNum);
                        SqlTransactionManager.Commit();
                        return cfg + PcbRevision + PcbSupplier + weekCode + beginNO;
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
                        return cfg + PcbRevision + PcbSupplier + weekCode + beginNO;
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
                            return cfg + PcbRevision + PcbSupplier + weekCode + beginNO;
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
                            return cfg + PcbRevision + PcbSupplier + weekCode + sequenceNumber;
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
        //Vincent Rset from 000
        private readonly string beginNO = "000";
    }
}
