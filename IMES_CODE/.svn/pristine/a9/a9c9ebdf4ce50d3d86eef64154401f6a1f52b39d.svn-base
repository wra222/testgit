// 2010-03-19 Liu Dong(eB1-4)         Modify 客户确认: IMES_PCA..MACRange.Code 栏位的数据不是111 Part 的Code 属性，而是111 Part 的Cust 属性
// 2010-04-28 Liu Dong(eB1-4)         Modify 与需求不一致: MAC地址生成后,不是无条件地向NumControl里插入,而在已存在的情况下,更新当前值(不一定是最大值,因为不一定总是一个Range)就可以了.

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
using IMES.Infrastructure;
using IMES.Infrastructure.Utility.Generates;
using IMES.FisObject.Common.NumControl;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.Utility;
using IMES.FisObject.PCA.MB;
using System.Transactions;
using IMES.FisObject.Common.Part;
using IMES.Infrastructure.UnitOfWork;

namespace IMES.Activity
{
    /// <summary>
    /// 产生指定MB的指定数量的MAC
    /// (多Customer(多BU)的时候,需要考虑修改此处的取地址,MACRange里可能需要加Customer字段)
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseAcquireActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      应用于ICT Input
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.取得已经使用的最大的MAC;
    ///         2.在该MAC 的基础上加1得到新的MAC;
    ///         3.检查新的MAC 是否已经被其他MB 绑定;
    ///         4.检查新的MAC 是否在合法MAC Range;
    ///         3.更新MACRange 状态, [MACRange] 新的MAC 所在的记录，如果新的MAC = [MACRange].EndNo，则更新该记录的Status = ‘C’；否则更新该记录的Status = ‘A’;
    ///         3.更新MAC最大值;
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
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         无
    /// </para> 
    ///<para> 
    /// 输出：
    ///         Session.MAC
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///         update NumControl
    ///         update MACRange.Status 
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         
    /// </para> 
    /// </remarks>
    public partial class AcquireMacSection : BaseActivity
    {
        private static MathSequenceWithCarryNumberRule _mr = new MathSequenceWithCarryNumberRule(12, "0123456789ABCDEF");

        private static object _syncRoot_GetSeq = new object();

        public AcquireMacSection()
        {
            InitializeComponent();
        }

        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            return base.Execute(executionContext);
        }

        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            //try
            //{
                string newNum = getSequence(CurrentSession);
                CurrentSession.AddValue(Session.SessionKeys.MAC, newNum);
            //}
            //catch (FisException ex)
            //{
            //    throw ex;
            //}
            //catch (Exception ex)
            //{
                //Temp for untested!
                //string orig = "7F01FF02FF03";

                //MathSequenceWithCarryNumberRule mr = new MathSequenceWithCarryNumberRule(12, "0123456789ABCDEF");

                //int i = ((IMB)CurrentSession.GetValue(Session.SessionKeys.MB)).Sn.GetHashCode();
                //i = Math.Abs(i);

                //CurrentSession.AddValue(Session.SessionKeys.MAC, mr.IncreaseToNumber(orig, i));
                //Temp for untested!
            //}
                return base.DoExecute(executionContext);//default(ActivityExecutionStatus);
        }



        protected string getSequence(Session sess)
        {
            #region . Remarks  .
            ///1.	取得已经使用的最大的MAC
            ///GetData..[NumControl] – NoType = ‘MAC’and NoName = [MACRange].Code = 111 Level Part 的Code 属性 (PartInfo.InfoType = ‘Code’ 记录的InfoValue)

            ///2.	在该MAC 的基础上加1得到新的MAC

            ///3.	检查新的MAC 是否已经被其他MB 绑定，如果绑定则报告错误:”MAC Address<%MAC%> is used on another MB<%MB Sno%>, Please call PE!!”
            ///[PCB]

            ///4.	检查新的MAC 是否在合法MAC Range
            ///[MACRange] – Code = 111 Level Part 的Code 属性，Status IN (‘R’, ‘A’)

            ///5.	如果新的MAC 不在合法的MAC Range 中存在，则在[MACRange] 中选取一个新的Range 来使用
            ///SELECT TOP 1 BegNo as [New MAC] FROM MACRange WHERE Status = 'R' AND Code = @Code
            ///    ORDER BY Cdt

            ///6.	更新MACRange 状态
            ///[MACRange]
            ///新的MAC 所在的记录，如果新的MAC = [MACRange].EndNo，则更新该记录的Status = ‘C’；否则更新该记录的Status = ‘A’

            ///7.	更新GetData..[NumControl]
            #endregion

            int quantity = 1; //目前只支持一次取一個

            string model = ((MB)sess.GetValue(Session.SessionKeys.MB)).Model;
            // 2010-03-19 Liu Dong(eB1-4)         Modify 客户确认: IMES_PCA..MACRange.Code 栏位的数据不是111 Part 的Code 属性，而是111 Part 的Cust 属性
            //string _111PartCode = this.Get111PartCode(model);
            string _111PartCust = this.Get111PartCust(model);

            sess.AddValue(Session.SessionKeys.PN111Code, _111PartCust);

            INumControlRepository numCtrlRepository = RepositoryFactory.GetInstance().GetRepository<INumControlRepository, NumControl>();
            string newNum = string.Empty;
            string maxNum = string.Empty;
            MACRange macr = null;
            lock (_syncRoot_GetSeq)
            {
                try
                {
                    SqlConnectionManager.Begin();

                    macr = numCtrlRepository.GetMaxMACRange(_111PartCust, MACRange.MACRangeStatus.Active, out maxNum);//Attempt #1
                    if (macr != null)
                    {
                        newNum = _mr.IncreaseToNumber(maxNum, quantity);
                    }
                    else
                    {
                        macr = numCtrlRepository.GetAvailableRange(_111PartCust, MACRange.MACRangeStatus.Virgin);//Attempt #2
                        if (macr != null)
                        {
                            newNum = macr.BegNo;
                        }
                        else
                        {
                            macr = numCtrlRepository.GetAvailableRange(_111PartCust, MACRange.MACRangeStatus.Active);//Attempt #3: 此Attempt爲了避免數據的不完整性: 如果NumControl裏的記錄丟失,則可以通過此來補上; 如果MACRange裏的記錄丟失,則必然會報下面的異常.
                            if (macr != null)
                            {
                                newNum = macr.BegNo;
                            }
                            else
                            {
                                throw new FisException("MDL002", new string[] { _111PartCust });
                            }
                        }
                    }

                    this.CheckBoundWithMB(newNum);

                    //TransactionOptions tsos = new TransactionOptions();
                    //tsos.IsolationLevel = IsolationLevel.ReadCommitted;
                    //using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew, tsos))
                    //{
                    UnitOfWork uow = new UnitOfWork();

                    if (macr.IsEnd(newNum))
                    {
                        numCtrlRepository.SetMACRangeStatusDefered(uow, macr.ID, MACRange.MACRangeStatus.Closed);
                    }
                    else
                    {
                        numCtrlRepository.SetMACRangeStatusDefered(uow, macr.ID, MACRange.MACRangeStatus.Active);
                    }
                    NumControl newMax = new NumControl(0, GeneratesConstants.MappingToStandard(GeneratesConstants.Mac), _111PartCust, newNum, this.Customer);
                    // 2010-04-28 Liu Dong(eB1-4)         Modify 与需求不一致: MAC地址生成后,不是无条件地向NumControl里插入,而在已存在的情况下,更新当前值(不一定是最大值,因为不一定总是一个Range)就可以了.                    
                    //numCtrlRepository.SaveMaxNumberDefered(uow, newMax);
                    numCtrlRepository.SaveMaxMACDefered(uow, newMax);
                    // 2010-04-28 Liu Dong(eB1-4)         Modify 与需求不一致: MAC地址生成后,不是无条件地向NumControl里插入,而在已存在的情况下,更新当前值(不一定是最大值,因为不一定总是一个Range)就可以了.
                    uow.Commit();

                    SqlConnectionManager.Commit();

                    //    scope.Complete();
                    //}
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
            return newNum;
        }

        //protected string Get111PartCode(string _111PN)
        protected string Get111PartCust(string _111PN)
        {
            string ret = null;
            IPartRepository partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            IPart part = partRepository.Find(_111PN);
            if (part != null && part.Attributes != null && part.Attributes.Count > 0)
            {
                // 2010-03-19 Liu Dong(eB1-4)         Modify 客户确认: IMES_PCA..MACRange.Code 栏位的数据不是111 Part 的Code 属性，而是111 Part 的Cust 属性
                //ret = part.GetAttribute("Code");
                ret = part.GetAttribute("Cust");
                // 2010-03-19 Liu Dong(eB1-4)         Modify 客户确认: IMES_PCA..MACRange.Code 栏位的数据不是111 Part 的Code 属性，而是111 Part 的Cust 属性
            }
            if (ret == null || ret.Trim() == string.Empty)
            {
                throw new FisException("MDL003", new string[] { _111PN }); //MDL003
            }
            return ret.Trim();
        }

        protected void CheckBoundWithMB(string mac)
        {
            IMBRepository mbRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();
            IMB res = mbRepository.GetMBByMAC(mac);
            if (res != null)
            {
                throw new FisException("MDL004", new string[] { mac, res.Sn });
            }
        }
    }
}