// INVENTEC corporation (c)2012all rights reserved. 
// Description:
//             FRUNo    
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2012-02-03   207003                     create
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
using IMES.FisObject.PAK;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.NumControl;
using IMES.Infrastructure.UnitOfWork;
using IMES.FisObject.PAK.Pizza;
using IMES.FisObject.FA.Product;
using IMES.FisObject.Common.Part;
using IMES.FisObject.Common.Model;
using IMES.DataModel;
namespace IMES.Activity
{
    /// <summary>
    /// 获取DCode
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      应用于Combine coa and dn
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         获取Pizza ID
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///        Session.SessionKeys.Lines 
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         无
    /// </para> 
    ///<para> 
    /// 输出：
    ///         PizzaID
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///         无
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///              INumControlRepository
    /// </para> 
    /// </remarks>
    public partial class GetFRUNo : BaseActivity
    {
        private static object _syncRoot_GetSeq = new object();

        /// <summary>
        /// constructor
        /// </summary>
        public GetFRUNo()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 获取DCode
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {

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



            string preSeqStr = thisYear + weekCode;           
            NumControl numCtrl = null;
            INumControlRepository numCtrlRepository = RepositoryFactory.GetInstance().GetRepository<INumControlRepository, NumControl>();

            string type = "FRUNO";
            try
            {
                SqlTransactionManager.Begin();
                lock (_syncRoot_GetSeq)
                {
                    bool insOrUpd = true;
                    //string maxMo = numCtrlRepository.GetMaxNumber(type, likecont);
                    string seq = string.Empty;
                    string maxSeqStr = string.Empty;
                    string maxNumStr = string.Empty;
                     //流水码的取得
                     numCtrl = numCtrlRepository.GetMaxValue(type, preSeqStr);
                    //檢查有沒有lock index, 沒有lock index, 改變查詢條件
                    if (numCtrl == null)
                    {
                        NumControl data = numCtrlRepository.GetMaxValue(type, "Lock");
                        numCtrl = numCtrlRepository.GetMaxValue(type, preSeqStr);
                    }
                  
                    if (numCtrl == null)
                    {
                        seq = "000000";
                        insOrUpd = true;                        
                    }
                    else
                    {
                        string maxMo = numCtrl.Value;
                        seq = maxMo.Substring(2, 6);
                        insOrUpd = false;                     
                    }

                    if (insOrUpd)
                    {
                        maxNumStr = seq;
                    }
                    else
                    {
                        if (seq.ToUpper() == "999999")
                        {
                            maxNumStr = "000000";
                        }
                        else
                        {
                            int num = Convert.ToInt32(seq);
                            num += 1;
                            maxNumStr = num.ToString();
                            int len = maxNumStr.Length;
                            for (var i = 0; i < 6 - len; i++)
                            {
                                maxNumStr = "0" + maxNumStr;
                            }
                        }
                    }
                    maxSeqStr = weekCode + maxNumStr;
                    CurrentSession.AddValue(Session.SessionKeys.PizzaID, maxSeqStr);

                    if (insOrUpd)
                    {
                       
                        numCtrlRepository.InsertNumControl(new NumControl(0, type, preSeqStr, maxSeqStr, this.Customer));
                    }
                    else
                    {
                        numCtrl.Value = maxSeqStr;                        
                        numCtrlRepository.SaveMaxNumber(numCtrl, false);                      
                    }

                    CurrentSession.AddValue(Session.SessionKeys.PrintLogName, Station);
                    CurrentSession.AddValue(Session.SessionKeys.PrintLogBegNo, maxSeqStr);
                    CurrentSession.AddValue(Session.SessionKeys.PrintLogEndNo, maxSeqStr);
                    CurrentSession.AddValue(Session.SessionKeys.PrintLogDescr, "");
                    CurrentSession.AddValue(Session.SessionKeys.FRUNO, maxSeqStr);
                }                                
                SqlTransactionManager.Commit();
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
        
    }
}
