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

using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure;
using IMES.FisObject.FA.Product;
using IMES.FisObject.Common.QTime;
using IMES.FisObject.Common.Line;
using System.Collections.Generic;
using IMES.Infrastructure.UnitOfWork;
using IMES.FisObject.Common.TestLog;
using IMES.Infrastructure.Extend;
using System.Data.SqlClient;
using IMES.FisObject.Common.Station;
using IMES.Infrastructure.Repository._Schema;
using IMES.DataModel;
using IMES.Common;
using IMES.FisObject.PCA.MB;



namespace IMES.Activity
{
    /// <summary>
    /// 檢查QTime及設定QTime Action
    /// </summary>
    public partial class CheckAndSetQTime : BaseActivity
	{
        /// <summary>
        /// 构造函数
        /// </summary>
		public CheckAndSetQTime()
		{
			InitializeComponent();
		}

        private const string DefectDescrFormat = "PreStation:{0}~CurStation:{1}~TimeOut:{2}";
        private const string UnPizzaDefectStation = "UnPizza";
        private readonly static IList<string> UnpackPizzaStationList = new List<string>() { "68", "8C", "PK01", "PK02", "PK03", "PK04", "PK05", "PKOK" };
        private readonly char[] Delimiter = new char[] { ',', ';', '~' };
        private const string DefectType ="PRD";
        private const string QTIMEName ="QTime";
        /// <summary>
        /// 当前流程的类型，共有两种，MB,Product
        /// </summary>
        public enum ProcessTypeEnum
        {
            /// <summary>
            /// MB
            /// </summary>
            MB = 1,
            /// <summary>
            /// Product
            /// </summary>
            Product = 2
        }

        /// <summary>
        /// 保留LineStationStopPeriodLog 資料天數
        /// </summary>
        public static DependencyProperty RemainLogDayProperty = DependencyProperty.Register("RemainLogDay", typeof(int), typeof(CheckAndSetQTime), new PropertyMetadata((int)2));

        /// <summary>
        /// Status of Product
        /// </summary>
        [DescriptionAttribute("RemainLogDay")]
        [CategoryAttribute("Remain Log Day Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public int RemainLogDay
        {
            get
            {
                return ((int)(base.GetValue(CheckAndSetQTime.RemainLogDayProperty)));
            }
            set
            {
                base.SetValue(CheckAndSetQTime.RemainLogDayProperty, value);
            }
        }

        /// <summary>
        /// 保留LineStationStopPeriodLog 資料天數
        /// </summary>
        public static DependencyProperty ProcessTypeProperty = DependencyProperty.Register("ProcessType", typeof(ProcessTypeEnum), typeof(CheckAndSetQTime), new PropertyMetadata(ProcessTypeEnum.Product));

        /// <summary>
        /// Status of Product
        /// </summary>
        [DescriptionAttribute("ProcessType")]
        [CategoryAttribute("ProcessType Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public ProcessTypeEnum ProcessType
        {
            get
            {
                return ((ProcessTypeEnum)(base.GetValue(ProcessTypeProperty)));
            }
            set
            {
                base.SetValue(ProcessTypeProperty, value);
            }
        }

        /// <summary>
        /// 檢查QTime 及設定QTime Action
        /// </summary>        
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            Session session = CurrentSession;
            ActivityCommonImpl utl = ActivityCommonImpl.Instance;
            IProductRepository prodRep = utl.prodRep; //RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            IMBRepository mbRep = utl.mbRep;            
            IQTimeRepository qTimeRep = RepositoryFactory.GetInstance().GetRepository<IQTimeRepository, QTime>();
            ILineRepository lineRep = utl.lineRep;  //RepositoryFactory.GetInstance().GetRepository<ILineRepository, Line>();
            DateTime now=DateTime.MinValue;
           
            #region 檢查 Product ,QTime及LineEx 設置資料，若無，則不做
            string pdline = this.Line;
            IProduct prod =null;
            IMB mb =null;
            string SN = null;
            string modelName = null;
            string familyName = null;
            DateTime udt = DateTime.Now;
            string preStation = null;

            if (this.ProcessType == ProcessTypeEnum.Product)
            {
                prod = utl.IsNull<IProduct>(session,Session.SessionKeys.Product);
                modelName = prod.Model;
                SN = prod.ProductID;
                familyName = prod.Family;
                udt = prod.Status.Udt;
                preStation = prod.Status.StationId.Trim();
                if (string.IsNullOrEmpty(pdline))
                {
                    pdline = prod.Status.Line;
                    if (string.IsNullOrEmpty(pdline))
                    {
                        IList<ProductStatusExInfo> preStatusList = prodRep.GetProductPreStation(new List<string> { prod.ProId });
                        if (preStatusList.Count > 0)
                        {
                            pdline = preStatusList[0].PreLine;
                        }
                    }
                }
            }
            else
            {
                mb = utl.IsNull<IMB>(session, Session.SessionKeys.MB);
                modelName = mb.Model;
                SN = mb.Sn;
                familyName = mb.Family;
                udt = mb.MBStatus.Udt;
                preStation = mb.MBStatus.Station;

                if (string.IsNullOrEmpty(pdline))
                {
                    pdline = mb.MBStatus.Line;
                    if (string.IsNullOrEmpty(pdline))
                    {
                        IList<PCBStatusExInfo> preStatusList = mbRep.GetPCBPreStation(new List<string> { mb.Sn });
                        if (preStatusList.Count > 0)
                        {
                            pdline = preStatusList[0].PreLine;
                        }
                    }
                }
            }

         

            Line line = lineRep.Find(pdline);
            if (prod==null  || 
                line == null || 
                line.LineEx == null || 
                string.IsNullOrEmpty(line.LineEx.AliasLine))
            {
                //throw new FisException();
                return base.DoExecute(executionContext);
            }

            string aliasLine = line.LineEx.AliasLine;
            //QTime qTime = qTimeRep.Find(new string[] { aliasLine, this.Station, prod.ProId });

            QTime qTime = qTimeRep.GetPriorityQTime(aliasLine, this.Station, SN,modelName, familyName);
            if (qTime == null )
            {
                return base.DoExecute(executionContext);
            }
  
            #endregion
                      

            #region 檢查QTime.StopTime,計算是否紀錄停線,不做DB事務
            if (qTime != null && qTime.StopTime > 0)
            {
                 LineStationLastProcessTime processTime= qTimeRep.GetLastProcessTime(aliasLine, this.Station);
                 if (processTime != null)
                 {
                     now = processTime.Now;
                     if (processTime.SpeedTime >= qTime.StopTime)
                     {
                         LineStationStopPeriodLog periodLog = new LineStationStopPeriodLog
                         {
                             Line = aliasLine,
                             Station = this.Station,
                             StartTime = processTime.ProcessTime,
                             EndTime = now,
                             Editor = this.Editor
                         };
                         qTimeRep.AddLineStationStopPeriodLog(periodLog);
                     }
                 }
                 // 沒有最後一次Process Time 不考慮Check TimeOut
                 else
                 {
                     now = SqlHelper.GetDateTime();
                 }

                processTime = new LineStationLastProcessTime
                 {
                     Line = aliasLine,
                     Station = this.Station,
                     ProductID = SN,
                     ProcessTime = now,
                     Editor = this.Editor
                 };
                 qTimeRep.UpdateLineStationLastProcessTime(processTime);

                //刪除過時的停線紀錄
                 qTimeRep.RemoveStationStopPeriodLog(aliasLine, this.Station, this.RemainLogDay);
            }
           #endregion

           #region 檢查TimeOut ,計算是否QTime逾時並執行QTime Action
            IUnitOfWork uow = new UnitOfWork();
            bool isDoDefect = false;
            bool isDoHoldStation = false;
            string defectStation = this.Station;
            int timeOut = 0;
            //int stopTime = 0;
            bool isCheckTimeOutStation = true;
            bool isTimeOut = false;
            //string preStation = prod.Status.StationId.Trim();
            TestLog.TestLogStatus defectStatus = TestLog.TestLogStatus.Fail;
            //檢查TimeOut例外站點
            if (!string.IsNullOrEmpty(qTime.ExceptStation))
            {
                string[] exceptStations = qTime.ExceptStation.Split(Delimiter);
                isCheckTimeOutStation = !exceptStations.Contains(preStation);
            }

            if (qTime != null && qTime.TimeOut > 0 &&
                isCheckTimeOutStation)
            {
                if (now == DateTime.MinValue)
                {
                    now = SqlHelper.GetDateTime();
                }
                    //計算TimeOut
                    //IList<int> timeOutList = qTimeRep.CalLineStopTime(aliasLine, "69", prod.Status.Udt, now);
                    //stopTime = timeOutList[0];
                    //timeOut = (timeOutList[1] - timeOutList[0]) ;

                    IList<LineStopLogInfo> logs = qTimeRep.CalLineStopMillionSecond(aliasLine, this.Station, udt, now);
                    double stopMillionSecond = 0;
                    foreach( LineStopLogInfo item in logs)
                    {
                        stopMillionSecond = stopMillionSecond + (item.EndTime - item.StartTime).TotalMilliseconds;
                    }

                    timeOut = (int)(((now - udt).TotalMilliseconds - stopMillionSecond) / 1000); 

                    // 判別有沒有TimeOut 
                    if ((qTime.Category == QTimeCategoryEnum.Max && timeOut >= qTime.TimeOut) ||
                        (qTime.Category == QTimeCategoryEnum.Min && timeOut <= qTime.TimeOut))
                    {
                        isTimeOut = true;
                        if (!string.IsNullOrEmpty(qTime.DefectCode))
                        {
                            isDoDefect = true;
                        }
                        if (!string.IsNullOrEmpty(qTime.HoldStation))
                        {
                            defectStation = qTime.HoldStation;
                            defectStatus = (qTime.HoldStatus == QTimeStationStatusEnum.Fail ?
                                                     TestLog.TestLogStatus.Fail : TestLog.TestLogStatus.Pass);
                            isDoHoldStation = true;
                        }
                    }
                    
                    //執行QTime Action
                    if (isDoDefect || isDoHoldStation)
                    {
                       
                        if (this.ProcessType == ProcessTypeEnum.Product)
                        {
                            prod.UpdateStatus(new IMES.FisObject.FA.Product.ProductStatus()
                            {
                                Line = pdline,
                                ProId = SN,
                                TestFailCount = 0,
                                ReworkCode = string.Empty,
                                StationId = defectStation,
                                Status = (defectStatus == TestLog.TestLogStatus.Fail ?
                                                               StationStatus.Fail : StationStatus.Pass),
                                Editor = this.Editor,
                                Udt = now
                            });

                            IList<IMES.DataModel.TbProductStatus> stationList = prodRep.GetProductStatus(new List<string> { prod.ProId });
                            prodRep.UpdateProductPreStationDefered(uow, stationList);

                            #region write Productlog

                            ProductLog productLog = new ProductLog
                            {
                                Model = prod.Model,
                                Status = (defectStatus == TestLog.TestLogStatus.Fail ?
                                                               StationStatus.Fail : StationStatus.Pass),
                                Editor = this.Editor,
                                Line = pdline,
                                Station = defectStation,
                                Cdt = now
                            };

                            prod.AddLog(productLog);
                        }
                        else
                        {
                            MBStatus mbStatus= new MBStatus(SN, defectStation, (defectStatus == TestLog.TestLogStatus.Fail ?
                                                               MBStatusEnum.Fail : MBStatusEnum.Pass), this.Editor, pdline, now, now);
                            mb.MBStatus = mbStatus;

                            IList<TbProductStatus> preStatusList = mbRep.GetMBStatus(new List<string>() { SN });
                            mbRep.UpdatePCBPreStationDefered(uow,
                                                                                       preStatusList);
                            #region Write MBlog
                            var mbLog = new MBLog(
                                                0,
                                                mb.Sn,
                                                mb.Model,
                                                 defectStation,
                                                 (defectStatus == TestLog.TestLogStatus.Fail ?
                                                               0 : 1),
                                                pdline,
                                                this.Editor,
                                                now);

                            mb.AddLog(mbLog);
                           
                            #endregion
                        }

                        #endregion
                    }

                    //unpack Pizza part
                    if (this.ProcessType == ProcessTypeEnum.Product && 
                        isDoHoldStation &&
                        defectStation == UnPizzaDefectStation)
                    {                        
                        utl.UnPack.unPackPizzaPart(session, uow, this.Editor);
                        utl.UnPack.unPackPAKProductPart(session, uow, this.Editor);

                        prodRep.DeleteProductPartByProductIDAndStationDefered(uow, 
                                                                                                                 new List<string>() { SN },
                                                                                                                 UnpackPizzaStationList, 
                                                                                                                 this.Editor);
                    }

                    if (isDoDefect)
                    {
                        #region add test log
                        string actionName = qTime.Category.ToString() + QTIMEName;
                        string errorCode = string.Empty;
                        string descr = string.Format(DefectDescrFormat, preStation, this.Station, timeOut.ToString()); //"PreStation:"+preStation+ "~CurStation:"+this.Station +"~TimeOut:"+ timeOut.ToString();
                         
                        //TestLog testLog = new TestLog(0, prod.ProId, this.Line, "", defectStation, defectStatus, "", this.Editor, "PRD", DateTime.Now);
                        TestLog testLog = new TestLog(0, SN, pdline, string.Empty, defectStation, defectStatus, string.Empty,
                                                                            actionName, errorCode, descr, this.Editor, DefectType, now);
                        if (this.ProcessType == ProcessTypeEnum.Product)
                        {
                            prod.AddTestLog(testLog);
                            //add defect
                            TestLogDefect defectItem = new TestLogDefect(0, 0, qTime.DefectCode, this.Editor, now);
                            testLog.AddTestLogDefect(defectItem);
                        }
                        else
                        {
                            mb.AddTestLog(testLog);
                            TestLogDefect defectItem = new TestLogDefect(0, 0, qTime.DefectCode, this.Editor, now);
                            testLog.AddTestLogDefect(defectItem);
                        }
                        #endregion
                    } 
                
                     if (isDoDefect || isDoHoldStation)
                     {
                         if (this.ProcessType == ProcessTypeEnum.Product)
                         {
                             prodRep.Update(prod, uow);
                         }
                         else
                         {
                             mbRep.Update(mb, uow);
                         }
                        uow.Commit();
                        if (qTime.Category == QTimeCategoryEnum.Min)
                        {
                            int diffTime = (timeOut - qTime.TimeOut) / 60;
                            throw new FisException("CHK093", new string[] { diffTime.ToString() });
                        }
                        else
                        {
                            throw new FisException("QTM001", new string[] { SN, qTime.Category.ToString() + QTIMEName, timeOut.ToString() });
                        }
                     }
                    
                     
                     //Min QTime Warning message nothing to do
                     if (isTimeOut && qTime.Category == QTimeCategoryEnum.Min)
                     {
                         int diffTime = (timeOut - qTime.TimeOut) / 60;
                         throw new FisException("CHK093", new string[]{diffTime.ToString()});
                     }
                }  
            #endregion

            return base.DoExecute(executionContext);
        }
	}
}
