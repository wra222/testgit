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
using IMES.FisObject.PAK.Pallet;
namespace IMES.Activity
{
   /// <summary>
   /// 
   /// </summary>
    public partial class GetRUNo : BaseActivity
    {
        private static object _syncRoot_GetSeq = new object();

        /// <summary>
        /// constructor
        /// </summary>
        public GetRUNo()
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
            IPalletRepository palletRep = RepositoryFactory.GetInstance().GetRepository<IPalletRepository>();
            
            int ruNoQty = 0;
            if (CurrentSession.GetValue("RUNoQty") == null)
            {
                throw new FisException("CQCHK0006", new string[] {"RUNoQty" });
            }
            else
            {
                ruNoQty = int.Parse((string)CurrentSession.GetValue("RUNoQty"));
                if (ruNoQty <= 0)
                {
                    throw new FisException("CQCHK0006", new string[] { "RUNoQty" });
                }
            }

            string palletNo=(string)CurrentSession.GetValue(Session.SessionKeys.PalletNo);
            if (string.IsNullOrEmpty(palletNo))
            {
                throw new FisException("CQCHK0006", new string[] { Session.SessionKeys.PalletNo });
            }

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
            int startNum = 0;
            int endNum = 0;
            string numFormat = "00000";
            NumControl numCtrl = null;
            INumControlRepository numCtrlRepository = RepositoryFactory.GetInstance().GetRepository<INumControlRepository, NumControl>();

            string type = "RUNO";
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
                        seq = "00001";
                        insOrUpd = true;                        
                    }
                    else
                    {
                        string maxMo = numCtrl.Value;
                        seq = maxMo.Substring(3, 5);
                        insOrUpd = false;                     
                    }

                    if (insOrUpd)
                    {
                        startNum = 1;
                        endNum = ruNoQty;
                        maxNumStr = ruNoQty.ToString(numFormat);
                    }
                    else
                    {
                        int num = Convert.ToInt32(seq);
                        int maxNum = num + ruNoQty;

                        if (maxNum >= 99999)
                        {
                            startNum = 1;
                            endNum = ruNoQty;
                            maxNumStr = ruNoQty.ToString(numFormat);
                        }
                        else
                        {                                               
                            num += 1;
                            startNum = num;
                            endNum = maxNum;
                            maxNumStr = maxNum.ToString(numFormat);                           
                        }
                    }

                    maxSeqStr = weekCode + "C"+maxNumStr;
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
                    CurrentSession.AddValue(Session.SessionKeys.PrintLogBegNo, weekCode + "C" + startNum.ToString(numFormat));
                    CurrentSession.AddValue(Session.SessionKeys.PrintLogEndNo, maxSeqStr);
                    CurrentSession.AddValue(Session.SessionKeys.PrintLogDescr, palletNo);
                   

                    IList<string> ruNoList= new List<string>();
                    for(int i= startNum;i<= endNum;i++)
                    {
                        ruNoList.Add(weekCode + "C" + i.ToString(numFormat));
                    }

                    CurrentSession.AddValue("RUNoList", ruNoList);
                    palletRep.UpdateAttr(palletNo,"RUNoList", string.Join("~",ruNoList.ToArray()),"",this.Editor);

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
