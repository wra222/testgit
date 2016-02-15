/*
 * INVENTEC corporation (c)2012 all rights reserved. 
 * Description:CI-MES12-SPEC-SA-UC MB Label Print.docx
 *             获取MBSno 
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2013-03-12   Vincent           Create 
 * 
 * Known issues:
 */
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
using IMES.FisObject.Common.Part;
using IMES.Infrastructure.UnitOfWork;
using IMES.Infrastructure.Utility.Generates;
using IMES.FisObject.PCA.MBModel;
using IMES.FisObject.PCA.MB;
using IMES.DataModel;

namespace IMES.Activity
{
    /// <summary>
    /// 获取MBSno
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      应用于MBLabelPrint
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         获取MBSno
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
    public partial class GetChildMBSn : BaseActivity
    {
        private static object _syncRoot_GetSeq = new object();
        //转换数组（32进制）
        private static string[] numLst = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9",
                                           "A", "B", "C", "D", "E", "F", "G", "H", "J",
                                           "K", "L", "M", "N", "P", "R", "S", "T",
                                           "V", "W", "X", "Y", "Z"};
        /// <summary>
        /// constructor
        /// </summary>
        public GetChildMBSn()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 获取MBSno
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            DateTime dt = DateTime.Now;
            string year = string.Empty;
            string month = string.Empty;
            //string mfgCode = "95";
            //string weekCode = string.Empty; ;
            string seqCode = string.Empty; ;
            string tmpSeqStr = string.Empty;

            NumControl numCtrl = null;
            IList mbSnoLst = new ArrayList();
            IPartRepository partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            IMBRepository imbRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();


            int qty = (int)CurrentSession.GetValue(Session.SessionKeys.Qty);
            string mo = (string)CurrentSession.GetValue(Session.SessionKeys.MBMONO);
            string mbCode = (string)CurrentSession.GetValue(Session.SessionKeys.MBCode);
            //Get MBCode.MultiQty
            int multiQty = 0;

            MBCodeDef resultMBCode = imbRepository.GetMBCode(mbCode);
            if (resultMBCode == null)
            {
                throw new FisException("CHK965", new string[] { mbCode });
            }
            else
            {
                multiQty = resultMBCode.Qty;
            }

           

            //check factor
            string factor = (string)CurrentSession.GetValue(Session.SessionKeys.FamilyName);
            if (factor == "" || factor == null)
            {
                IList<string> partLst = partRepository.GetValueFromSysSettingByName("MB_Initial");
                if (partLst.Count() > 0)
                {
                    factor = partLst[0].ToString();
                    if (factor == "" || factor == null)
                    {
                        throw new FisException("CHK232", new string[] { "" });
                    }
                }
                else
                {
                    throw new FisException("CHK232", new string[] { "" });
                }
            }

            //获取板子类型
            string model = (string)CurrentSession.GetValue(Session.SessionKeys.ModelName);
            IPart part = partRepository.Find(model);
            IList<string> valueList = partRepository.GetValueFromSysSettingByName("Site");
            string mbType = "";
            if (valueList.Count == 0)
            {
                throw new Exception("Error:尚未設定Site...");
            }
            else
            {
                mbType = (string)part.GetAttribute("TP");
                if (valueList[0] == "ICC")
                {
                    if (mbType == "" || mbType == null)
                    {
                        mbType = "B";
                    }
                }
                else
                {
                    if (mbType == "" || mbType == null)
                    {
                        mbType = "M";
                    }

                }
            }
            //if (mbType == "" || mbType == null)
            //{
            //    mbType = "M";
            //}

            //获取年月
            bool isNextMonth = (bool)CurrentSession.GetValue(Session.SessionKeys.IsNextMonth);
            if (isNextMonth)
            {
                dt = dt.AddMonths(1);
            }
            year = dt.Year.ToString().Substring(dt.Year.ToString().Length - 1, 1);

            switch (dt.Month.ToString())
            {
                case "10":
                    month = "A";
                    break;
                case "11":
                    month = "B";
                    break;
                case "12":
                    month = "C";
                    break;
                default:
                    month = dt.Month.ToString().Substring(dt.Month.ToString().Length - 1, 1);
                    break;
            }

            INumControlRepository numCtrlRepository = RepositoryFactory.GetInstance().GetRepository<INumControlRepository, NumControl>();

            //流水码的取得
            string preSeqStr = mbCode + year + month + mbType + "_";
            string likecont = preSeqStr + "{0}";
            //seqCodeLst = getSequence(CurrentSession, preSeqStr, qty);
            string type = "MBSno";

            try
            {
                SqlTransactionManager.Begin();
                lock (_syncRoot_GetSeq)
                {
                    bool insOrUpd = true;
                    //获取最大MBSno
                    string maxMo = numCtrlRepository.GetMaxNumber(type, likecont);
                    string seq = string.Empty;
                    if (string.IsNullOrEmpty(maxMo))
                    {
                        //seq = "A000"; //HP上线新旧系统并行，新系统流水初始值：A000
                        //经和UC贯伟确认，整机和Docking都给为D000，整机和DockingMBCode会不同，不会发生重号
                        seq = "0000"; //HP上线新旧系统并行，新系统/Docking流水初始值：流水初始值：D000
                        //2012/08/13 正式生产。恢复Docking初始值 “0000”
                        //Session.AddValue("IsDocking", "Docking");
                        var isDocking = "";
                        try
                        {
                            isDocking = (string)CurrentSession.GetValue("IsDocking");
                        }
                        catch (Exception ex)
                        {
                            var strerr = ex.Message;
                            isDocking = "PC";
                        }
                        if (isDocking == "Docking")
                        {
                            seq = "0000";
                        }

                        insOrUpd = true;
                    }
                    else
                    {
                        //取后四位
                        seq = maxMo.Substring(maxMo.Length - 4, 4);
                        insOrUpd = false;
                        //当达到最大值‘ZZZZ’时报错
                        if (seq.ToUpper() == "ZZZZ")
                        {
                            List<string> errpara = new List<string>();
                            throw new FisException("CHK162", errpara);
                        }
                        IList<NumControl> numCtrlLst = numCtrlRepository.GetNumControlByNoTypeAndValue(type, maxMo);
                        int numCtrlId = numCtrlLst[0].ID;
                        numCtrl = numCtrlRepository.Find(numCtrlId);
                    }
                    string[] seqLst = new string[4];
                    seqLst[0] = seq.Substring(0, 1);
                    seqLst[1] = seq.Substring(1, 1);
                    seqLst[2] = seq.Substring(2, 1);
                    seqLst[3] = seq.Substring(3, 1);
                    //字母换成数字
                    int[] idexLst = getSeqNum(seqLst);

                    //preSeqStr = preSeqStr.Replace("_", factor);
                    if (insOrUpd)
                    {
                        for (int j = 1; j <= multiQty; j++)
                        {
                            tmpSeqStr = preSeqStr.Replace("_", j.ToString()) + seq;
                            mbSnoLst.Add(tmpSeqStr);
                        }
                        qty -= 1;
                    }
                    //进位换算（32进制）
                    for (int i = 0; i < qty; i++)
                    {
                        if (idexLst[3] == 31)
                        {
                            if (idexLst[2] == 31)
                            {
                                if (idexLst[1] == 31)
                                {
                                    if (idexLst[0] == 31)
                                    {
                                        List<string> errpara = new List<string>();
                                        throw new FisException("CHK162", errpara);
                                    }
                                    else
                                    {
                                        idexLst[0] += 1;
                                        idexLst[1] = 0;
                                        idexLst[2] = 0;
                                        idexLst[3] = 0;
                                    }
                                }
                                else
                                {
                                    idexLst[1] += 1;
                                    idexLst[2] = 0;
                                    idexLst[3] = 0;
                                }
                            }
                            else
                            {
                                idexLst[2] += 1;
                                idexLst[3] = 0;
                            }

                        }
                        else
                        {
                            idexLst[3] += 1;
                        }
                        seqCode = numLst[idexLst[0]] + numLst[idexLst[1]] + numLst[idexLst[2]] + numLst[idexLst[3]];
                        for (int j = 1; j <= multiQty; j++)
                        {
                            tmpSeqStr = preSeqStr.Replace("_", j.ToString()) + seqCode;
                            mbSnoLst.Add(tmpSeqStr);
                        }
                        //tmpSeqStr = preSeqStr + seqCode;
                        //mbSnoLst.Add(tmpSeqStr);
                    }
                    // insert and qty = 1
                    if (seqCode == string.Empty)
                    {
                        seqCode = seq;
                    }

                    string maxChildSeq = preSeqStr.Replace("_", "1") + seqCode;

                    preSeqStr = preSeqStr.Replace("_", factor);
                    string maxSeqStr = preSeqStr + seqCode;
                    CurrentSession.AddValue(Session.SessionKeys.MBNOList, mbSnoLst);
                    //CurrentSession.AddValue(Session.SessionKeys.MBSN, maxSeqStr);
                    CurrentSession.AddValue(Session.SessionKeys.MBSN, maxChildSeq);

                    //print log
                    string descr = this.Line + " " + mo + " " + model;
                    CurrentSession.AddValue(Session.SessionKeys.PrintLogDescr, descr);
                    CurrentSession.AddValue(Session.SessionKeys.PrintLogBegNo, mbSnoLst[0]);
                    //CurrentSession.AddValue(Session.SessionKeys.PrintLogEndNo, mbSnoLst[mbSnoLst.Count - 1]);
                    CurrentSession.AddValue(Session.SessionKeys.PrintLogEndNo, maxChildSeq);
                    CurrentSession.AddValue(Session.SessionKeys.MultiQty, multiQty);

                    IUnitOfWork uof = new UnitOfWork();
                    //更新数据库
                    if (insOrUpd)
                    {
                        numCtrlRepository.Add(new NumControl(0, type, string.Empty, maxSeqStr, this.Customer), uof);
                    }
                    else
                    {
                        numCtrl.Value = maxSeqStr;
                        numCtrl.Customer = this.Customer;
                        numCtrlRepository.Update(numCtrl, uof);
                    }
                    uof.Commit();
                    SqlTransactionManager.Commit();
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

            return base.DoExecute(executionContext);
        }

        //字母换算数字（根据numLst[]）
        private int[] getSeqNum(string[] str)
        {
            int[] list = new int[4];
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

                if (str[3] == numLst[l].ToString())
                {
                    list[3] = l;
                    flag += 1;
                }

                if (flag == 4)
                {
                    break;
                }
            }
            return list;
        }
    }
}

