// INVENTEC corporation (c)2011 all rights reserved. 
// Description: Save Data
// UI:          CI-MES12-SPEC-PAK-UI Pick Card
// UC:          CI-MES12-SPEC-PAK-UC Pick Card             
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-10-27   zhu lei                      create
// Known issues:
using System;
using System.Workflow.ComponentModel;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.Generic;
using IMES.FisObject.PAK.Pallet;
using IMES.DataModel;
using IMES.FisObject.PAK.DN;
using IMES.FisObject.Common.NumControl;
using IMES.Infrastructure.UnitOfWork;
using System.Linq;
using System.Linq.Expressions;
namespace IMES.Activity
{
    /// <summary>
    /// Save Data
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         this.Key
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         无
    /// </para> 
    ///<para> 
    /// 输出：
    ///         
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///          
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///              
    /// </para> 
    /// </remarks>
    public partial class SaveDataForPickCard : BaseActivity
    {
        private const long SS_INIT = 936;	// "Q0"
        private const string SS_TYPE = "FlowNumber";
        private const string SS_NAME = "PickCardFlowNumber";
        private const string SS_CUST = "HP";
        private const string SS_BASE = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private static object SS_LOCK = new object();

        ///<summary>
        ///</summary>
        public SaveDataForPickCard()
        {
            InitializeComponent();
        }

        private static string GetFlowNumber()
        {
            long flow_number = 0;

            lock (SS_LOCK)
            {
                try
                {
                    SqlTransactionManager.Begin();
                    INumControlRepository rep = RepositoryFactory.GetInstance()
                        .GetRepository<INumControlRepository, NumControl>();
                    NumControl next_avail = rep.GetMaxValue(SS_TYPE, SS_NAME);
                    if (next_avail == null)
                    {
                        next_avail = new NumControl();
                        next_avail.Customer = SS_CUST;
                        next_avail.NOType = SS_TYPE;
                        next_avail.NOName = SS_NAME;

                        flow_number = SS_INIT;
                        next_avail.Value = (flow_number + 1).ToString();

                        rep.InsertNumControl(next_avail);
                        SqlTransactionManager.Commit();
                    }
                    else
                    {
                        flow_number = long.Parse(next_avail.Value);
                        next_avail.Value = (flow_number + 1).ToString();

                        IUnitOfWork uof = new UnitOfWork();
                        rep.Update(next_avail, uof);
                        uof.Commit();
                        SqlTransactionManager.Commit();
                    }
                }
                catch (Exception)
                {
                    SqlTransactionManager.Rollback();
                    throw;
                }
                finally
                {
                    SqlTransactionManager.Dispose();
                    SqlTransactionManager.End();
                }
            }

            return SS_BASE[(int)((flow_number / SS_BASE.Length) % SS_BASE.Length)].ToString()
                + SS_BASE[(int)(flow_number % SS_BASE.Length)].ToString();
        }

        class DNPalletQtyEqualityComparer : IEqualityComparer<DNPalletQty>
        {
            #region IEqualityComparer<DNPalletQty> Members

            public bool Equals(DNPalletQty x, DNPalletQty y)
            {
                return x.PalletNo == y.PalletNo;
            }

            public int GetHashCode(DNPalletQty obj)
            {
                return obj.PalletNo.GetHashCode();
            }

            #endregion
        }

        class MAWBInfoEqualityComparer : IEqualityComparer<MAWBInfo>
        {
            #region IEqualityComparer<MAWBInfo> Members

            public bool Equals(MAWBInfo x, MAWBInfo y)
            {
                return x.Delivery == y.Delivery;
            }

            public int GetHashCode(MAWBInfo obj)
            {
                return obj.Delivery.GetHashCode();
            }

            #endregion
        }
        /// <summary>
        /// Save Data
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            var logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            var palletRepository = RepositoryFactory.GetInstance().GetRepository<IPalletRepository, Pallet>();
            var deliveryRepository = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
            var lstFwder = (IList<ForwarderInfo>)CurrentSession.GetValue(Session.SessionKeys.Forwarder+"List");
            var fwderInfo = (ForwarderInfo)CurrentSession.GetValue(Session.SessionKeys.Forwarder);
            string truckID = Key;
            DateTime fwdDate = Convert.ToDateTime(fwderInfo.Date);
            IList<PickIDCtrlInfo> lstPickIDCtrlInfo = palletRepository.GetPickIDByTruckIDAndDate(truckID, fwdDate);
			PickIDCtrlInfo pickIDCtrlInfo;
			
            if (lstPickIDCtrlInfo == null || lstPickIDCtrlInfo.Count == 0)
            {
                //
                // Generate Pick ID
                //
                string ff = fwderInfo.Forwarder.Substring(0, 2);
                string yy = fwdDate.Year.ToString().Substring(2, 2);
                string mm = fwdDate.Month.ToString("00");
                string dd = fwdDate.Day.ToString("00");
                string generatePickID = ff + yy + mm + dd + GetFlowNumber();

                pickIDCtrlInfo = new PickIDCtrlInfo
                {
                    Driver = fwderInfo.Driver,
                    Fwd = fwderInfo.Forwarder,
                    PickID = generatePickID,
                    TruckID = truckID,
                    InDt = string.Empty,
                    OutDt = string.Empty,
                    Cdt = DateTime.Now,
                    Dt = fwderInfo.Date
                };

                //
                // Insert Pick ID Record To PickIDCtrl Table
                //
                palletRepository.InsertPickIDCtrlDefered(CurrentSession.UnitOfWork, pickIDCtrlInfo);
			}
			else
			{
				pickIDCtrlInfo = lstPickIDCtrlInfo[0];
				// DELETE FROM FwdPlt WHERE PickID = @pickid
				palletRepository.RemoveFwdPltByPickID(pickIDCtrlInfo.PickID);
			}
			
                //
                // 根据MAWB列表(ForwarderInfo列表)取得DN列表(MAWBInfo)
                // * 确实是DN列表, 不要被表名称所迷惑
                //
                IEnumerable<MAWBInfo> lstMAWBInfo = new List<MAWBInfo>();
                foreach (ForwarderInfo fi in lstFwder)
                {
                    try
                    {
                        lstMAWBInfo = lstMAWBInfo.Union(palletRepository.GetMAWBInfoByMAWBorHAWB(fi.MAWB.Trim()),
                            new MAWBInfoEqualityComparer());
                    }
                    catch (Exception) { }
                }
                if (lstMAWBInfo.Count() == 0)
                {
                    List<string> errpara = new List<string>();
                    errpara.Add(this.Key);
                    throw new FisException("CHK148", errpara);
                }

                //
                // 根据DN列表取得Pallet列表
                //
                string[] dns = (from mi in lstMAWBInfo select mi.Delivery).Distinct().ToArray();
                IEnumerable<DNPalletQty> lstPallet = deliveryRepository.GetPalletList2FromView(dns);
                if (lstPallet.Count() == 0)
                {
                    List<string> errpara = new List<string>();
                    errpara.Add(this.Key);
                    throw new FisException("CHK148", errpara);
                }

                bool needEdits = "Y".Equals(CurrentSession.GetValue("IsAutoBOL") as string);
            if (needEdits)
            {
                foreach (DNPalletQty v in lstPallet)
                {
                    IList<IMES.DataModel.DNPalletWeight> result = palletRepository.QueryPalletWeight(v.PalletNo);
                    if (result == null || result.Count == 0)
                    {
                        // not all PLTS are Ready.
                        throw new FisException("CQCHK0035", new string[] { "(" + v.PalletNo + ")" });
                    }
                    foreach (IMES.DataModel.DNPalletWeight w in result)
                    {
                        if (w.Weight == 0)
                        {
                            throw new FisException("CQCHK0035", new string[] { "[" + v.PalletNo + "]" });
                        }
                    }
                }
            }

                //
                // 将Pallet插入FwdPlt表
                //
                foreach (var g in lstPallet.GroupBy(p => p.PalletNo))
                {
                    palletRepository.InsertFwdPltDefered(CurrentSession.UnitOfWork,
                    new FwdPltInfo
                    {
                        PickID = pickIDCtrlInfo.PickID,
                        Operator = Editor,
                        Plt = g.Key,
                        Cdt = DateTime.Now,
                        Udt = DateTime.Now,
                        Qty = g.Sum(p=>p.DeliveryQty),
                        Date = fwderInfo.Date,
                        Status = "In"
                    });
                }

                CurrentSession.AddValue(Session.SessionKeys.PickIDCtrl, pickIDCtrlInfo.PickID);
                CurrentSession.AddValue(Session.SessionKeys.PrintLogBegNo, pickIDCtrlInfo.PickID);
                CurrentSession.AddValue(Session.SessionKeys.PrintLogEndNo, pickIDCtrlInfo.PickID);
/*
            }
            else
            {
                CurrentSession.AddValue(Session.SessionKeys.PickIDCtrl, lstPickIDCtrlInfo[0].PickID);
                CurrentSession.AddValue(Session.SessionKeys.PrintLogBegNo, lstPickIDCtrlInfo[0].PickID);
                CurrentSession.AddValue(Session.SessionKeys.PrintLogEndNo, lstPickIDCtrlInfo[0].PickID);
            }
*/
            CurrentSession.AddValue(Session.SessionKeys.PrintLogName, "PickID");
            CurrentSession.AddValue(Session.SessionKeys.PrintLogDescr, string.Empty);

            return base.DoExecute(executionContext);
        }
    }
}
