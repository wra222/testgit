/* INVENTEC corporation (c)2009 all rights reserved. 
 * Description: 用于初次进入Repair时，根据TestLog创建Repair信息。
 * 
 * Update: 
 * Date         Name                         Reason 
 * ==========   =======================      ===========================
 * 2009-11-12   Yuan XiaoWei                 create
 * 2009-11-13   Tong.Zhi-Yong                implement DoExecute method
 * 2010-01-26   Tong.Zhi-Yong                modify ITC-1103-0134
 * 2010-05-17   Tong.Zhi-Yong                Modify ITC-1155-0109
 * 2011-03-17   Dean.Man                     Modify For B&N
 * 2012-01-10   Yang.Weihua                  Modify For IMES2012 PCA Repiar
 * 2012-01-18   zhanghe                      add return station
 * Known issues:
 */
using System;
using System.ComponentModel;
using System.Workflow.ComponentModel;
using IMES.DataModel;
using IMES.FisObject.Common.Repair;
using IMES.FisObject.PCA.MB;
using IMES.Infrastructure;
using System.Collections.Generic;
using IMES.FisObject.Common.TestLog;
using IMES.Infrastructure.Extend;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.FA.Product;

namespace IMES.Activity
{

    /// <summary>
    /// 根据TestLog生成Reair记录
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      应用于以MB或Product为主线对象的维修站
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.获取MB或者Product最新的TestLog(包含TestLogDefectInfo);
    ///         2.如果TestLog为测试失败Log, 则根据TestLog创建Repair对象(包含RepairDefectInfo);
    ///         3.如果TestLog为测试通过Log, 则创建新的Repair对象(不包含RepairDefectInfo);
    ///         4.保存Repair对象
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.MB
    ///         或者
    ///         Session.Product
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
    ///         insert PCARepair
    ///         insert PCARepair_DefectInfo
    ///         或者
    ///         insert ProductRepair
    ///         insert ProductRepair_DefectInfo
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         IMB
    ///         IMBRepository
    ///         或者
    ///         IProduct
    ///         IProductRepository
    ///         
    /// </para> 
    /// </remarks>
	public partial class GenerateRepairByTestLog: BaseActivity
	{
        ///<summary>
        /// 处理那个类型的维修(SA, FA, PAK)
        ///</summary>
        public static DependencyProperty RepairStationTypeProperty = DependencyProperty.Register("RepairStationType", typeof(string), typeof(GenerateRepairByTestLog));

        ///<summary>
        /// 处理那个类型的维修(SA, FA, PAK)
        ///</summary>
        [DescriptionAttribute("RepairStationType")]
        [CategoryAttribute("RepairStationType Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string RepairStationType
        {
            get
            {
                return ((string)(GetValue(RepairStationTypeProperty)));
            }
            set
            {
                SetValue(RepairStationTypeProperty, value);
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
		public GenerateRepairByTestLog()
		{
			InitializeComponent();
		}

        /// <summary>
        /// 用于初次进入Repair时，根据TestLog创建Repair信息。
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            IRepairTarget repairTarget = GetRepairTarget();
            IList<RepairDefect> repairDefects = new List<RepairDefect>();

            TestLog testLog;
            //2012-01-10
            //There maybe "PCA Repair Input" staion in SA process before "PCA Repair", 
            //so MBStatus.Station is not necessorily a test station. Therefor we get 
            //latestTestlog for SA in a different way than we have done for FA and PAQC.
            //For SA we get the latest test FAIL log, while for others we get the latest
            //log and check whether it's pass or fail.
            if (string.Compare(RepairStationType, "SA", true) == 0)
            {
                testLog = repairTarget.LatestFailTestLog;
                if (testLog == null)
                {
                    var errpara = new List<string> { Key };
                    throw new FisException("CHK025", errpara);
                }
            }
            else
            {
                testLog = repairTarget.LatestTestLog;
                if (testLog == null)
                {
                    var errpara = new List<string> { Key };
                    throw new FisException("CHK025", errpara);
                }
                if (testLog.Status == TestLog.TestLogStatus.Pass)
                {
                    var errpara = new List<string>();
                    throw new FisException("CHK024", errpara);
                }
            }
            
            //get Latest fail log id
            int logId = 0;
            
            if (string.Compare(RepairStationType, "SA", true) == 0)
            {
                MBLog log = ((IMB) repairTarget).GetLatestFailLog();
                if (log != null)
                {
                    logId = log.ID;
                }
            }
            else
            {
                ProductLog log = ((IProduct)repairTarget).GetLatestFailLog();
                if (log != null)
                {
                    logId = log.Id;
                }
            }

            IList<TestLogDefect> testLogDefects = testLog.Defects;
            if (testLogDefects != null && testLogDefects.Count != 0)
            {
                RepairDefect repairDefect;
                if (string.Compare(RepairStationType, "SA", true) == 0)
                {
                    foreach (TestLogDefect defectItem in testLogDefects)
                    {
                        repairDefect = new RepairDefect(0, 0, testLog.Type, defectItem.DefectCode, null, null, null, null, null,
                                                        null, null, null, null, null, null, null, null, null, false, null,
                                                        null, null, string.Empty, null, "0", null, null, null, null, null, null, null,
                                                        null, null, null, null,testLog.Editor, DateTime.Now, DateTime.Now);
                        var mta = new MtaMarkInfo {mark = "0", rep_Id = 0};
                        repairDefect.MTAMark = mta;
                        repairDefects.Add(repairDefect);
                    }
                }
                else if (string.Compare(RepairStationType, "FA", true) == 0)
                {
                    foreach (TestLogDefect defectItem in testLogDefects)
                    {
                        repairDefect = new RepairDefect(0, 0, testLog.Type, defectItem.DefectCode, null, null, null, null, null,
                                                        null, null, null, null, null, null, null, null, null, false, null,
                                                        null, null, "0", null, "0", null, null, null, null, null, null, null,
                                                        null, null, null, null, testLog.Editor, DateTime.Now, DateTime.Now);
                        //Add Return Station:
                        var stationRepository = RepositoryFactory.GetInstance().GetRepository<IMES.FisObject.Common.Station.IStationRepository, IMES.FisObject.Common.Station.IStation>();
                        IProduct productObj = (IProduct)CurrentSession.GetValue(Session.SessionKeys.Product);                        
                        
                        if(productObj == null)
                        {
                            List<string> errpara = new List<string>();
                            errpara.Add(this.Key);                            
                            throw new FisException("SFC002", errpara);
                        }
                        else
                        {
                            /*DefectCodeStationInfo cond = new DefectCodeStationInfo();
                            IList<DefectCodeStationInfo> info = null;
                            cond.defect = defectItem.DefectCode;
                            cond.pre_stn = productObj.Status.StationId;
                            cond.crt_stn = this.Station;
                            info = stationRepository.GetDefectCodeStationList(cond);
                            if (info != null && info.Count > 0)
                            {
                                repairDefect.ReturnStation = info[0].nxt_stn;
                            }
                            else
                            {
                                List<string> errpara = new List<string>();
                                FisException e = new FisException("CHK830", errpara);
                                throw e;                                
                            }*/
                        }
                        repairDefects.Add(repairDefect);
                    }
                }
                else if (string.Compare(RepairStationType, "PAQC", true) == 0)
                {
                    foreach (TestLogDefect defectItem in testLogDefects)
                    {
                        repairDefect = new RepairDefect(0, 0, testLog.Type, defectItem.DefectCode, null, null, null, null, null,
                                                        null, null, null, null, null, null, null, null, null, false, null,
                                                        null, null, "0", null, "0", null, null, null, null, null, null, null,
                                                        null, null, null, null, testLog.Editor, DateTime.Now, DateTime.Now);

                        repairDefects.Add(repairDefect);
                    }
                }
                else
                {
                    foreach (TestLogDefect defectItem in testLogDefects)
                    {
                        repairDefect = new RepairDefect(0, 0, testLog.Type, defectItem.DefectCode, null, null, null, null, null,
                                                        null, null, null, null, null, null, null, null, null, false, null,
                                                        null, null, string.Empty, null, "0", null, null, null, null, null, null, null,
                                                        null, null, null, null, testLog.Editor, DateTime.Now, DateTime.Now);

                        repairDefects.Add(repairDefect);
                    }
                }
            }

            var repair = new Repair(0, testLog.Sn, repairTarget.RepairTargetModel, testLog.Type, testLog.Line, testLog.Station, Repair.RepairStatus.NotFinished, repairDefects, testLog.Editor, testLog.ID.ToString(), logId, DateTime.Now, DateTime.Now);
            repairTarget.AddRepair(repair);
            UpdateRepairTarget(repairTarget);
            CurrentSession.AddValue(ExtendSession.SessionKeys.IsOQCRepair, false);

            return base.DoExecute(executionContext);
        }
	}
}
