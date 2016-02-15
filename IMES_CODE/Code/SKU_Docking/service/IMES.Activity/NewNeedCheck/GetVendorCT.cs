// INVENTEC corporation (c)2010all rights reserved. 
// Description:CI-MES12-SPEC-FA-UC IEC Label Print.docx
//             获取VendorCT            
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2010-12-01   zhu lei                      create
// Known issues:
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
using System.Transactions;
using IMES.FisObject.PAK;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.Model;
using IMES.FisObject.Common.NumControl;
using IMES.Infrastructure.UnitOfWork;
using IMES.Infrastructure.Utility.Generates;

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
    ///      应用于IECLabelPrint
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         获取VendorCT
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
    ///         VendorCT
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///         无
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///              IWarrantyRepository
    ///              Warranty
    /// </para> 
    /// </remarks>
    public partial class GetVendorCT : BaseActivity
    {
        private static object _syncRoot_GetSeq = new object();
        private static string[] numLst = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9",
                                           "A", "B", "C", "D", "E", "F", "G", "H", "I", "J",
                                           "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T",
                                           "U", "V", "W", "X", "Y", "Z"};
        /// <summary>
        /// constructor
        /// </summary>
        public GetVendorCT()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 获取VendorCT
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            DateTime dt = DateTime.Now;
            string year = string.Empty;
            string month = string.Empty;
            string mfgCode = "95";
            string weekCode = string.Empty; ;
            string seqCode = string.Empty; ;
            string tmpSeqStr = string.Empty;

            NumControl numCtrl = null;
            IList vendorCTLst = new ArrayList();
            string supplierCode = (string)CurrentSession.GetValue(Session.SessionKeys.AssemblyCode);
            string vendorCode = (string)CurrentSession.GetValue(Session.SessionKeys.VendorDCode);
            int qty = (int)CurrentSession.GetValue(Session.SessionKeys.Qty);

            INumControlRepository numCtrlRepository = RepositoryFactory.GetInstance().GetRepository<INumControlRepository, NumControl>();
            IModelRepository modelRepository = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();
            IList<string> list = modelRepository.GetCodeFromHPWeekCodeInRangeOfDescr();
            if (list.Count > 0)
            {
                weekCode = list[0].ToString();
            }
            //流水码的取得
            string preSeqStr = vendorCode + mfgCode + supplierCode + weekCode;
            string likecont = preSeqStr + "{0}";
            //seqCodeLst = getSequence(CurrentSession, preSeqStr, qty);
            string type = "VendorCode";
            lock (_syncRoot_GetSeq)
            {
                bool insOrUpd = true;
                string maxMo = numCtrlRepository.GetMaxNumber(type, likecont);
                string seq = string.Empty;
                if (string.IsNullOrEmpty(maxMo))
                {
                    seq = "001";
                    insOrUpd = true;
                }
                else
                {
                    seq = maxMo.Substring(maxMo.Length - 3, 3);
                    insOrUpd = false;
                    if (seq.ToUpper() == "ZZZ")
                    {
                        List<string> errpara = new List<string>();
                        throw new FisException("CHK162", errpara);
                    }
                    IList<NumControl> numCtrlLst = numCtrlRepository.GetNumControlByNoTypeAndValue(type, maxMo);
                    int numCtrlId = numCtrlLst[0].ID;
                    numCtrl = numCtrlRepository.Find(numCtrlId);
                }
                //string[] seqLst = seq.Split();
                //IList<string> seqLst = null;
                string[] seqLst = new string[3];
                seqLst[0] = seq.Substring(0, 1);
                seqLst[1] = seq.Substring(1, 1);
                seqLst[2] = seq.Substring(2, 1);
                int[] idexLst = getSeqNum(seqLst);
                bool errorflag = false;

                if (insOrUpd)
                {
                    tmpSeqStr = preSeqStr + seq;
                    vendorCTLst.Add(tmpSeqStr);
                    qty -= 1;
                }

                for (int i = 0; i < qty; i++)
                {

                    if (idexLst[2] == 35)
                    {
                        if (idexLst[1] == 35)
                        {
                            if (idexLst[0] == 35)
                            {
                                errorflag = true;
                                break;
                            }
                            else
                            {
                                idexLst[0] += 1;
                                idexLst[1] = 0;
                                idexLst[2] = 0;
                            }
                        }
                        else
                        {
                            idexLst[1] += 1;
                            idexLst[2] = 0;
                        }

                    }
                    else
                    {
                        idexLst[2] += 1;
                    }
                    seqCode = numLst[idexLst[0]] + numLst[idexLst[1]] + numLst[idexLst[2]];
                    tmpSeqStr = preSeqStr + seqCode;
                    vendorCTLst.Add(tmpSeqStr);
                }
                // insert and qty = 1
                if (seqCode == string.Empty)
                {
                    seqCode = seq;
                }
                string maxSeqStr = preSeqStr + seqCode;
                CurrentSession.AddValue(Session.SessionKeys.VCodeInfoLst, vendorCTLst);
                CurrentSession.AddValue(Session.SessionKeys.VCode, maxSeqStr);

                if (insOrUpd)
                {
                    numCtrlRepository.Add(new NumControl(0, type, string.Empty, maxSeqStr, this.Customer), CurrentSession.UnitOfWork);
                }
                else
                {
                    numCtrl.Value = maxSeqStr;
                    numCtrl.Customer = this.Customer;
                    numCtrlRepository.Update(numCtrl, CurrentSession.UnitOfWork);
                }
                if (errorflag)
                {
                    List<string> errpara = new List<string>();
                    throw new FisException("CHK162", errpara);
                }
            }
            string dCode = (string)CurrentSession.GetValue(Session.SessionKeys.DCode);
            string rev = (string)CurrentSession.GetValue(Session.SessionKeys.IECVersion);
            string desc = dCode + "," + rev;
            int count = vendorCTLst.Count - 1;
            CurrentSession.AddValue(Session.SessionKeys.PrintLogName, "KP");
            CurrentSession.AddValue(Session.SessionKeys.PrintLogBegNo, vendorCTLst[0]);
            CurrentSession.AddValue(Session.SessionKeys.PrintLogEndNo, vendorCTLst[count]);
            CurrentSession.AddValue(Session.SessionKeys.PrintLogDescr, desc);
            return base.DoExecute(executionContext);
        }

        private int[] getSeqNum(string[] str)
        {
            int[] list = new int[3];
            int flag = 0;
            for (int l = 0; l < numLst.Count(); l++)
            {
                if (str[0] == numLst[l].ToString())
                {
                    list[0] = l;
                    flag += 1;
                }

                if (str[1] == numLst[l].ToString())
                {
                    list[1] = l;
                    flag += 1;
                }

                if (str[2] == numLst[l].ToString())
                {
                    list[2] = l;
                    flag += 1;
                }

                if (flag == 3)
                {
                    break;
                }
            }
            return list;
        }
    }
}
