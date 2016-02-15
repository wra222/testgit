// INVENTEC corporation (c)2011 all rights reserved. 
// Description: Generate VendorCT List
//
// Update:
// Date         Name                         Reason 
// ==========   =======================      ============
// 2012-03-07   Li.Ming-Jun                  create
// Known issues:
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Workflow.ComponentModel;
using IMES.DataModel;
using IMES.FisObject.Common.Model;
using IMES.FisObject.Common.NumControl;
using IMES.FisObject.Common.Part;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.UnitOfWork;
using IMES.Infrastructure.Utility.Generates.intf;
using IMES.Infrastructure.Utility.Generates.impl;

namespace IMES.Activity
{
    /// <summary>
    /// 获取VendorCT
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      应用于IEC Label Print
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         Generate VendorCT List
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///        Session.SessionKeys.AssemblyCode 
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         无
    /// </para> 
    ///<para> 
    /// 输出：
    ///         VendorCT List
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///         无
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///              
    ///              
    /// </para> 
    /// </remarks>
    public partial class AcquireVendorCTList : BaseActivity
    {
        /// <summary>
        /// 
        /// </summary>
        public AcquireVendorCTList()
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
            string config = (string)CurrentSession.GetValue(Session.SessionKeys.AssemblyCode);
            string rev = (string)CurrentSession.GetValue(Session.SessionKeys.IECVersion);
            string dCode = (string)CurrentSession.GetValue(Session.SessionKeys.DCode);
            int multiQty = (int)CurrentSession.GetValue(Session.SessionKeys.Qty);
            IList<string> vendorCTList = new List<string>();

            string partNo = config + rev.Substring(0, 1) + rev.Substring(rev.Length - 2, 2);

            //Get Vendor Code and Vendor
            IPartRepository partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            IPart part = partRepository.Find(partNo);
            if (part == null)
            {
                throw new FisException("CHK027", new string[] { partNo });
            }

            string vendorCode = (string)part.GetAttribute("VendorCode");
            string vendor = (string)part.GetAttribute("VENDOR").ToUpper();
            if (vendor == null || vendor == "")
            {
                throw new FisException("IEC004", new string[] { });
            }

            //Get MfgCode
            string mfgCode = "";
            IList<string> mfgCodeList = partRepository.GetValueFromSysSettingByName("MfgCode");
            if (mfgCodeList != null && mfgCodeList.Count > 0)
            {
                mfgCode = mfgCodeList[0];
            }
            else
            {
                throw new FisException("IEC002", new string[] { });
            }

            //Get Week Code
            string weekCode = "";
            IModelRepository CurrentModelRepository = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();
            IList<string> weekCodeList = CurrentModelRepository.GetCodeFromHPWeekCodeInRangeOfDescr();
            if (weekCodeList != null && weekCodeList.Count > 0)
            {
                weekCode = weekCodeList[0];
            }
            else
            {
                throw new FisException("ICT009", new string[] { });
            }

            for (int i = 0; i < multiQty; i++)
            {
                string maxVendorCT = GetVendorCT(vendorCode, vendor, mfgCode, weekCode);
                vendorCTList.Add(maxVendorCT);
            }

            string desc = dCode + "," + rev + "," + config;
            int count = vendorCTList.Count - 1;
            CurrentSession.AddValue(Session.SessionKeys.PrintLogName, "KP");
            CurrentSession.AddValue(Session.SessionKeys.PrintLogBegNo, vendorCTList[0]);
            CurrentSession.AddValue(Session.SessionKeys.PrintLogEndNo, vendorCTList[count]);
            CurrentSession.AddValue(Session.SessionKeys.PrintLogDescr, desc);
            CurrentSession.AddValue(Session.SessionKeys.PartNo, partNo);
            CurrentSession.AddValue(Session.SessionKeys.VCodeInfoLst, vendorCTList);

            return base.DoExecute(executionContext);
        }

        private string GetVendorCT(string vendorCode, string vendor, string mfgCode, string weekCode)
        {
            try
            {
                SqlTransactionManager.Begin();
                lock (_syncRoot_GetSeq)
                {
                    INumControlRepository numCtrlRepository = RepositoryFactory.GetInstance().GetRepository<INumControlRepository, NumControl>();

                    NumControl currentMaxNum = new NumControl();
                    string preSeqStr = vendorCode + mfgCode + "__" + weekCode;
                    string likecont = preSeqStr + "{0}";
                    string type = "KP";
                    string vendorCT = "";

                    string maxNumber = numCtrlRepository.GetMaxNumber(type, likecont, this.Customer);
                    if (maxNumber != null && maxNumber != "")
                    {
                        currentMaxNum = numCtrlRepository.GetNumControlByNoTypeAndValue(type, maxNumber)[0];
                    }

                    if (maxNumber == null || maxNumber == "")
                    {
                        IProductRepository currentProductRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();

                        SupplierCodeInfo first = currentProductRepository.GetSupplierCodeByVendor(vendor);
                        if (first == null)
                        {
                            throw new FisException("ICT013", new string[] { });
                        }

                        vendorCT = vendorCode + mfgCode + first.code + weekCode + "000";
                        currentMaxNum.NOName = "";
                        currentMaxNum.NOType = type;
                        currentMaxNum.Value = vendorCT;
                        currentMaxNum.Customer = this.Customer;
                        numCtrlRepository.InsertNumControl(currentMaxNum);
                        SqlTransactionManager.Commit();
                        return vendorCT;
                    }
                    else if (maxNumber.Substring(9, 2) != weekCode)
                    {
                        IProductRepository currentProductRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
                        SupplierCodeInfo first = currentProductRepository.GetSupplierCodeByVendor(vendor);
                        if (first == null)
                        {
                            throw new FisException("ICT013", new string[] { });
                        }

                        vendorCT = vendorCode + mfgCode + first.code + weekCode + "000";
                        currentMaxNum.Value = vendorCT;
                        IUnitOfWork uof = new UnitOfWork();
                        numCtrlRepository.Update(currentMaxNum, uof);
                        uof.Commit();
                        SqlTransactionManager.Commit();
                        return vendorCT;
                    }
                    else
                    {
                        if (maxNumber.EndsWith("ZZZ"))
                        {
                            IProductRepository currentProductRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
                            IList<SupplierCodeInfo> codeList = currentProductRepository.GetSupplierCodeListByCode(maxNumber.Substring(7, 2));

                            SupplierCodeInfo first = currentProductRepository.GetSupplierCodeByVendor(vendor, codeList[0].idex);
                            if (first == null)
                            {
                                throw new FisException("IEC003", new string[] { });
                            }

                            vendorCT = vendorCode + mfgCode + first.code + weekCode + "000";
                            currentMaxNum.Value = vendorCT;
                            IUnitOfWork uof = new UnitOfWork();
                            numCtrlRepository.Update(currentMaxNum, uof);
                            uof.Commit();
                            SqlTransactionManager.Commit();
                            return vendorCT;
                        }
                        else
                        {
                            ISequenceConverter seqCvt = new SequenceConverterNormal("0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ", 3, "ZZZ", "000", '0');
                            string sequenceNumber = maxNumber.Substring(11, 3);
                            sequenceNumber = seqCvt.NumberRule.IncreaseToNumber(sequenceNumber, 1);

                            currentMaxNum.Value = maxNumber.Substring(0, 11) + sequenceNumber;
                            IUnitOfWork uof = new UnitOfWork();
                            numCtrlRepository.Update(currentMaxNum, uof);
                            uof.Commit();
                            SqlTransactionManager.Commit();
                            return vendorCode + mfgCode + maxNumber.Substring(7, 2) + weekCode + sequenceNumber;
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
    }
}
