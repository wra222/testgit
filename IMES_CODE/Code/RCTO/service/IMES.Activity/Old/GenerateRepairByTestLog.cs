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
 * Known issues:
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
using IMES.FisObject.Common.Repair;
using IMES.FisObject.PCA.MB;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectBase;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.Generic;
using IMES.FisObject.Common.TestLog;
using IMES.FisObject.Common.Station;
using IMES.Infrastructure.Extend;
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
                return ((string)(base.GetValue(GenerateRepairByTestLog.RepairStationTypeProperty)));
            }
            set
            {
                base.SetValue(GenerateRepairByTestLog.RepairStationTypeProperty, value);
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
            try
            {
                IRepairTarget repairTarget = GetRepairTarget();
                TestLog testLog = null;
                //IList<TestLog> testLogs = repairTarget.GetTestLog(); //Dean 20110315 performance Issue
                IList<TestLogDefect> testLogDefects = null;
                IStationRepository stationRepository = RepositoryFactory.GetInstance().GetRepository<IStationRepository, IStation>();
                IStation station = stationRepository.Find(Station);

                string OQCStation = "";
                IProduct product = null;
                if (string.Compare(RepairStationType, "FA", true) == 0)
                {
                    //Update Dean 20110315
                    product = (IProduct)CurrentSession.GetValue(Session.SessionKeys.Product);



                    IList<ProductInfo> productinfos = product.ProductInfoes;
                    foreach (ProductInfo info in productinfos)
                    {
                        if (info.InfoType == ExtendSession.SessionKeys.OQCRepairStation && info.InfoValue != "")
                        {
                            OQCStation = info.InfoValue;
                            break;
                        }
                    }
                }
                //Update Dean 20110315
                if (OQCStation == "") //Update Dean 20110315
                {

                    //if (testLogs != null && testLogs.Count != 0)
                    //{
                    Repair repair = null;
                    RepairDefect repairDefect = null;
                    IList<RepairDefect> repairDefects = new List<RepairDefect>();

                    testLog = repairTarget.LatestTestLog;//抓取latestTestLog by PreStation 

                    if (testLog == null)
                    {
                        List<string> errpara = new List<string>();
                        errpara.Add(Key);
                        throw new FisException("CHK025", errpara);
                    }

                    if (testLog.Status == TestLog.TestLogStatus.Pass)
                    {
                        List<string> errpara = new List<string>();
                        throw new FisException("CHK024", errpara);
                    }

                    testLogDefects = testLog.Defects;

                    if (testLogDefects != null && testLogDefects.Count != 0)
                    {
                        //if (station.StationType == StationType.SARepair)
                        if (string.Compare(RepairStationType, "SA", true) == 0)
                        {
                            foreach (TestLogDefect defectItem in testLogDefects)
                            {
                                //ITC-1103-0134 Tong.Zhi-Yong 2010-01-26
                                repairDefect = new RepairDefect(0, 0, testLog.Type, defectItem.DefectCode, null, null, null, null, null,
                                    null, null, null, null, null, null, null, null, null, false, null,
                                    null, null, string.Empty, null, "0", null, null, null, null, null, null, null,
                                    null, null, null, testLog.Editor, DateTime.Now, DateTime.Now);

                                repairDefects.Add(repairDefect);
                            }
                        }
                        //else if (station.StationType == StationType.FARepair)
                        else if (string.Compare(RepairStationType, "FA", true) == 0)
                        {
                            foreach (TestLogDefect defectItem in testLogDefects)
                            {
                                repairDefect = new RepairDefect(0, 0, testLog.Type, defectItem.DefectCode, null, null, null, null, null,
                                    null, null, null, null, null, null, null, null, null, false, null,
                                    null, null, "0", null, "0", null, null, null, null, null, null, null,
                                    null, null, null, testLog.Editor, DateTime.Now, DateTime.Now);

                                repairDefects.Add(repairDefect);
                            }
                        }
                        else if (string.Compare(RepairStationType, "PAQC", true) == 0)
                        {
                            foreach (TestLogDefect defectItem in testLogDefects)
                            {
                                //ITC-1155-0109 Tong.Zhi-Yong 2010-05-17
                                repairDefect = new RepairDefect(0, 0, testLog.Type, defectItem.DefectCode, null, null, null, null, null,
                                    null, null, null, null, null, null, null, null, null, false, null,
                                    null, null, "0", null, "0", null, null, null, null, null, null, null,
                                    null, null, null, testLog.Editor, DateTime.Now, DateTime.Now);

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
                                    null, null, null, testLog.Editor, DateTime.Now, DateTime.Now);

                                repairDefects.Add(repairDefect);
                            }
                        }

                    }

                    repair = new Repair(0, testLog.Sn, repairTarget.RepairTargetModel, testLog.Type, testLog.Line, testLog.Station, IMES.FisObject.Common.Repair.Repair.RepairStatus.NotFinished, repairDefects, testLog.Editor, testLog.ID.ToString(), DateTime.Now, DateTime.Now);
                    repairTarget.AddRepair(repair);
                    UpdateRepairTarget(repairTarget);
                    CurrentSession.AddValue(ExtendSession.SessionKeys.IsOQCRepair, false);



                    //Update Dean 20110315
                    //CurrentSession.AddValue(ExtendSession.SessionKeys.TestLog, testLog);                    
                    //CurrentSession.AddValue(ExtendSession.SessionKeys.RepairDefect, repairDefects);                    
                    //}
                    /*else
                    {
                        List<string> errpara = new List<string>();
                        errpara.Add(Key);
                        throw new FisException("CHK025", errpara);
                    }*/
                }
                else  // get and copy repair info from OQC repair station   Update Dean  20110316
                {
                    IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository>();
                    product.SetExtendedProperty(ExtendSession.SessionKeys.OQCRepairStation, "", Editor);
                    productRepository.Update(product, CurrentSession.UnitOfWork);

                }
                return base.DoExecute(executionContext);
            }
            catch (FisException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
            }
        }
	}
}
