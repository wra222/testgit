/*
* INVENTEC corporation ?2011 all rights reserved. 
* Description:Activity for RCTO MB Change Page
* UI:CI-MES12-SPEC-SA-UI RCTO MB Change.docx –2012/6/15 
* UC:CI-MES12-SPEC-SA-UC RCTO MB Change.docx –2012/6/11            
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2012-8-1    Jessica Liu           Create
* Known issues:
* TODO：
* ITC-1428-0008, Jessica Liu, 2012-9-7
*/


using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Drawing;
using System.Linq;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;
using System.Collections.Generic;
using IMES.FisObject.PCA.MB;
using IMES.FisObject.PCA.MBMO ;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.Extend;
using IMES.Infrastructure.Repository.PCA;
using IMES.FisObject.PCA.MBChangeLog;
using IMES.Infrastructure.UnitOfWork;
using IMES.Infrastructure.Utility.Generates;
using IMES.FisObject.Common.NumControl;

namespace IMES.Activity
{
    /// <summary>
    /// GenerateNewMBSNo
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///         应用于
    ///         MB Label Print
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         对Session.MBNOList中每个MBNO
    ///             1.创建MB对象
    ///             2.保存MB对象
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.ModelName
    ///         Session.SMTMONO
    ///         Session.DateCode
    ///         Session.MBNOList
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         无
    /// </para> 
    ///<para> 
    /// 输出：
    ///         无
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///         insert PCB
    ///         insert PCBStatus
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         IMBRepository
    /// </para> 
    /// </remarks>
    public partial class GenerateNewMBSNo : BaseActivity
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
        public GenerateNewMBSNo()
		{
			InitializeComponent();
		}

        /// <summary>
        /// GenerateMBSn
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            MB currentMB = CurrentSession.GetValue(Session.SessionKeys.MB) as MB;
            if (currentMB == null)
            {
                throw new NullReferenceException("MB in session is null");
            }

            string newMB = currentMB.Sn;
            //CheckCode：若MBSN的第5码为’M’，则取MBSN的第6码，否则取第7码；CheckCode为数字，则为子板，为’R’，则为RCTO
            string tempSn = "";
            bool bNeedGenerateNew = false;
            string mbCode = "";
            if (newMB.Substring(4, 1) == "M")
            {
                tempSn = newMB.Substring(0, 5);
                tempSn += "R" + newMB.Substring(6, newMB.Length - 6);

                //@MBSN的CheckCode不为’1’和’C’，则根据@MBSN的MBCode+年+月+‘M’，重新生成新的流水码，以防重复
                if (newMB.Substring(5, 1).ToUpper() != "1" && newMB.Substring(5, 1).ToUpper() != "C")
                {
                    bNeedGenerateNew = true;
                }

                mbCode = newMB.Substring(0, 2);
            }
            else
            {
                tempSn = newMB.Substring(0, 6);
                tempSn += "R" + newMB.Substring(7, newMB.Length - 7);

                //@MBSN的CheckCode不为’1’和’C’，则根据@MBSN的MBCode+年+月+‘M’，重新生成新的流水码，以防重复
                if (newMB.Substring(6, 1).ToUpper() != "1" && newMB.Substring(6, 1).ToUpper() != "C")
                {
                    bNeedGenerateNew = true;
                }

                mbCode = newMB.Substring(0, 3);
            }

            newMB = tempSn;
            tempSn = "";

            DateTime dt = DateTime.Now;
            string year = string.Empty;
            string month = string.Empty;
            string seqCode = string.Empty; ;
            string tmpSeqStr = string.Empty;
            NumControl numCtrl = null;
            IList mbSnoLst = new ArrayList();
            int qty = 1;
            if (bNeedGenerateNew == true)
            {
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
                string preSeqStr = mbCode + year + month + "M" + "_";
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
                            seq = "D000"; //HP上线新旧系统并行，新系统/Docking流水初始值：流水初始值：D000
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

                        preSeqStr = preSeqStr.Replace("_", "R");
                        if (insOrUpd)
                        {
                            tmpSeqStr = preSeqStr + seq;
                            mbSnoLst.Add(tmpSeqStr);
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
                            tmpSeqStr = preSeqStr + seqCode;
                            mbSnoLst.Add(tmpSeqStr);
                        }

                        // insert and qty = 1
                        if (seqCode == string.Empty)
                        {
                            seqCode = seq;
                        }
                        string maxSeqStr = preSeqStr + seqCode;
                        newMB = maxSeqStr;

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
            }

            CurrentSession.AddValue(Session.SessionKeys.MBSN, newMB);

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
