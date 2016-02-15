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

using IMES.Infrastructure;
using IMES.FisObject.FA.Product;
using IMES.FisObject.Common.Line;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.DataModel;
using IMES.FisObject.Common.Part;
using IMES.Infrastructure.Extend;
using IMES.FisObject.Common.Station;
using IMES.FisObject.Common.TestLog;


namespace IMES.Activity
{
    /// <summary>
    /// 檢查Image download Station log
    /// </summary>
    public partial class CheckImageDownLoadStation : BaseActivity
	{
        /// <summary>
        /// 构造函数
        /// </summary>
		public CheckImageDownLoadStation()
		{
			InitializeComponent();
		}

        private string ConstType = "DLCheckStation";

        /// <summary>
        /// DefaultCheckDLStation
        /// </summary>
        public static DependencyProperty CheckDefaultDLStationProperty = DependencyProperty.Register("CheckDefaultDLStation", typeof(string), typeof(CheckImageDownLoadStation), new PropertyMetadata("6A~67"));

        /// <summary>
        ///  Check Default DL Station
        /// </summary>
        [DescriptionAttribute("DefaultCheckDLStation")]
        [CategoryAttribute("Setup ImageDown Station Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string CheckDefaultDLStation
        {
            get
            {
                return ((string)(base.GetValue(CheckImageDownLoadStation.CheckDefaultDLStationProperty)));
            }
            set
            {
                base.SetValue(CheckImageDownLoadStation.CheckDefaultDLStationProperty, value);
            }
        }


        /// <summary>
        /// ImageDownLoadStation
        /// </summary>
        public static DependencyProperty ImageDownLoadStationProperty = DependencyProperty.Register("ImageDownLoadStation", typeof(string), typeof(CheckImageDownLoadStation), new PropertyMetadata("66"));

        /// <summary>
        /// ImageDownLoadStation
        /// </summary>
        [DescriptionAttribute("ImageDownLoadStation")]
        [CategoryAttribute("Setup ImageDown Station Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string ImageDownLoadStation
        {
            get
            {
                return ((string)(base.GetValue(CheckImageDownLoadStation.ImageDownLoadStationProperty)));
            }
            set
            {
                base.SetValue(CheckImageDownLoadStation.ImageDownLoadStationProperty, value);
            }
        }

        /// <summary>
        /// OnlyCheckLatestImageLog
        /// </summary>
        public static DependencyProperty CheckLatestImageLogProperty = DependencyProperty.Register("CheckLatestImageLog", typeof(bool), typeof(CheckImageDownLoadStation), new PropertyMetadata(true));

        /// <summary>
        /// CheckLatestImageLog
        /// </summary>
        [DescriptionAttribute("ImageDownLoadStation")]
        [CategoryAttribute("Setup ImageDown Station Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public bool CheckLatestImageLog
        {
            get
            {
                return ((bool)(base.GetValue(CheckImageDownLoadStation.CheckLatestImageLogProperty)));
            }
            set
            {
                base.SetValue(CheckImageDownLoadStation.CheckLatestImageLogProperty, value);
            }
        }

        /// <summary>
        /// EAPIAOutStation
        /// </summary>
        public static DependencyProperty EAPIAOutStationProperty = DependencyProperty.Register("EAPIAOutStation", typeof(string), typeof(CheckImageDownLoadStation), new PropertyMetadata("6A"));

        /// <summary>
        /// EAPIAOutStation
        /// </summary>
        [DescriptionAttribute("EPIAOutStation")]
        [CategoryAttribute("Setup Station Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string EAPIAOutStation
        {
            get
            {
                return ((string)(base.GetValue(CheckImageDownLoadStation.EAPIAOutStationProperty)));
            }
            set
            {
                base.SetValue(CheckImageDownLoadStation.EAPIAOutStationProperty, value);
            }
        }

        ///// <summary>
        ///// EPIAInputStation
        ///// </summary>
        //public static DependencyProperty EPIAInputStationProperty = DependencyProperty.Register("EPIAInputStation", typeof(string), typeof(CheckImageDownLoadStation), new PropertyMetadata("73"));

        ///// <summary>
        ///// EPIAOutStation
        ///// </summary>
        //[DescriptionAttribute("EAPIAInputStation")]
        //[CategoryAttribute("Setup Station Category")]
        //[BrowsableAttribute(true)]
        //[DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        //public string EPIAInputStation
        //{
        //    get
        //    {
        //        return ((string)(base.GetValue(CheckImageDownLoadStation.EPIAInputStationProperty)));
        //    }
        //    set
        //    {
        //        base.SetValue(CheckImageDownLoadStation.EPIAInputStationProperty, value);
        //    }
        //}


        /// <summary>
        /// 沒有需要OS Image, 在ITCND站報錯不須走ITCND Check Station
        /// </summary>
        public static DependencyProperty NoNeedOSImageThrowErrorProperty = DependencyProperty.Register("NoNeedOSImageThrowError", typeof(bool), typeof(CheckImageDownLoadStation), new PropertyMetadata(false));
        /// <summary>
        /// NoNeedITCNDThrowError
        /// </summary>
        [DescriptionAttribute("NoNeedITCNDThrowError")]
        [CategoryAttribute("Setup Image OS Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public bool NoNeedOSImageThrowError
        {
            get
            {
                return ((bool)(base.GetValue(CheckImageDownLoadStation.NoNeedOSImageThrowErrorProperty)));
            }
            set
            {
                base.SetValue(CheckImageDownLoadStation.NoNeedOSImageThrowErrorProperty, value);
            }
        }


        /// <summary>
        /// 抓QCStatus
        /// </summary>
        public static DependencyProperty IsGetQCPIAStatusProperty = DependencyProperty.Register("IsGetQCPIAStatus", typeof(bool), typeof(CheckImageDownLoadStation), new PropertyMetadata(false));
        /// <summary>
        /// 抓QCStatus
        /// </summary>
        [DescriptionAttribute("IsGetQCPIAStatus")]
        [CategoryAttribute("Setup Image OS Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public bool IsGetQCPIAStatus
        {
            get
            {
                return ((bool)(base.GetValue(CheckImageDownLoadStation.IsGetQCPIAStatusProperty)));
            }
            set
            {
                base.SetValue(CheckImageDownLoadStation.IsGetQCPIAStatusProperty, value);
            }
        }

        /// <summary>
        /// 檢查Image DL Station log
        /// </summary>        
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            IProductRepository prodRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            IPartRepository partRep = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            ILineRepository lineRep = RepositoryFactory.GetInstance().GetRepository<ILineRepository, Line>();
            DateTime now = DateTime.MinValue;
            string checkStation = "";

            #region 檢查設置資料，若無，則不報錯
            string pdline = this.Line;
            var prod = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);
            CurrentSession.AddValue(ExtendSession.SessionKeys.DLCheckStation, "");

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

            Line line = lineRep.Find(pdline);
            if (line == null ||
                line.LineEx == null ||
                string.IsNullOrEmpty(line.LineEx.AliasLine))
            {
                //未設置 LineEx 
                //throw new FisException("IDL001", new string[] { });
                checkStation = "";
            }
            else
            {
                string aliasLine = line.LineEx.AliasLine;

                IList<ConstValueInfo> constValueInfoList = partRep.GetConstValueListByType(ConstType);

                if (constValueInfoList == null || constValueInfoList.Count == 0)
                {
                    //throw new FisException("IDL002", new string[] { ConstType });
                    checkStation = "";
                }
                else
                {
                    var lineCheckStationList = (from item in constValueInfoList
                                                where item.name == aliasLine
                                                select item).ToList();

                    if (lineCheckStationList == null ||
                        lineCheckStationList.Count == 0 ||
                        string.IsNullOrEmpty(lineCheckStationList[0].value))
                    {
                        //throw new FisException("IDL003", new string[] { ConstType, aliasLine });
                        checkStation = "";
                    }
                    else
                    {
                        checkStation = lineCheckStationList[0].value;
                       
                    }
                }
            }
            #endregion

            if (string.IsNullOrEmpty(checkStation))
            {
                checkStation = this.CheckDefaultDLStation;
            }

            CurrentSession.AddValue(ExtendSession.SessionKeys.DLCheckStation, checkStation);

            string[] CheckStation = checkStation.Split(new char[] { ',', ';', '~' });

            #region Check Image DL Station main logical
            //Check DL station log
            if (CheckStation.Contains(this.Station))
            {
                //Mantis 0000559: 【FA】針對特殊商业型机种down load 方式，在ITCND Check 66log不做檢查
                bool checkImgLog = (string)prod.GetExtendedProperty("NotCheckImgLog")=="Y"?false : true;
                string imgStation = (string)prod.GetExtendedProperty("ImgStation");

                imgStation = string.IsNullOrEmpty(imgStation) ||checkImgLog ? 
                                                this.ImageDownLoadStation.Trim() :
                                                imgStation;
               
                IList<TsModelInfo> tsInfo = null;

                //exception case No need download image for RCTO (No Image OS DownLoad) 
                TsModelInfo cond = new TsModelInfo();
                cond.model = prod.Model;
                cond.mark = "0";
                tsInfo = prodRep.GetTsModelList(cond);
                if (tsInfo != null && tsInfo.Count != 0)
                {
                    if (this.NoNeedOSImageThrowError)
                    {

                        throw new FisException("CHK805", new string[] { });
                    }
                    else
                    {
                        return base.DoExecute(executionContext);
                    }
                }


                cond.model = prod.Model;
                cond.mark = "1";
                tsInfo = prodRep.GetTsModelList(cond);
                if (tsInfo == null || tsInfo.Count == 0)  // if no redownload Image flag,Need Check EPIA download
                {
                    ProductLog epiaLog =  prodRep.GetLatestLogByWcAndStatus(prod.ProId, this.EAPIAOutStation.Trim(), 1);
                    if (epiaLog != null)
                    {
                        //ProductLog imageLog = prodRep.GetLatestLogByWcAndStatus(prod.ProId, this.ImageDownLoadStation.Trim(), 1);
                        ProductLog lastProdLog = null;
                        ProductLog lastStation = null;
                        if (!checkPreHoldStation(prod, out lastProdLog, out lastStation))
                        {
                            lastProdLog = prodRep.GetLatestLog(prod.ProId);
                        }
                        else
                        {
                            epiaLog = lastStation;
                        }


                        //ProductLog imageLog = prodRep.GetLatestLog(prod.ProId);
                        if (lastProdLog == null || epiaLog.Cdt >= lastProdLog.Cdt)
                        {
                            List<string> errpara = new List<string>();
                            throw new FisException("CHK807", new string[] { });
                        }
                       
                        //if (lastProdLog.Station != this.ImageDownLoadStation.Trim() ||
                        if (lastProdLog.Station != imgStation ||
                            lastProdLog.Status != IMES.FisObject.Common.Station.StationStatus.Pass)
                        {
                            throw new FisException("CQCHK1007", new string[] { });
                        }
                        
                    }
                }

                if (checkImgLog)   //正常機器 
                {
                    //Vincent add for CQ case
                    if (this.CheckLatestImageLog)
                    {

                        //ProductLog prodLog = prodRep.GetLatestLogByWc(prod.ProId, this.ImageDownLoadStation.Trim());
                        ProductLog lastProdLog = null;
                        ProductLog lastStation = null;
                        DateTime lastCheckOutTime = prod.Status.Udt;
                        if (checkPreHoldStation(prod, out lastProdLog, out lastStation))
                        {
                            lastCheckOutTime = lastStation == null ? lastCheckOutTime : lastStation.Cdt;
                        }
                        else
                        {
                            lastProdLog = prodRep.GetLatestLog(prod.ProId);
                        }

                        if (lastProdLog == null || lastCheckOutTime > lastProdLog.Cdt)
                        {
                            throw new FisException("CHK845", new string[] { });
                        }

                        if (lastProdLog.Station != this.ImageDownLoadStation.Trim() ||
                            lastProdLog.Status != IMES.FisObject.Common.Station.StationStatus.Pass)
                        {
                            throw new FisException("CHK806", new string[] { });
                        }
                    }
                    else
                    {
                        ProductLog failLog = prodRep.GetLatestLogByWcAndStatus(prod.ProId, this.ImageDownLoadStation.Trim(), 0);
                        if (failLog != null && prod.Status.Udt < failLog.Cdt)
                        {
                            throw new FisException("CHK806", new string[] { });
                        }

                        ProductLog passLog = prodRep.GetLatestLogByWcAndStatus(prod.ProId, this.ImageDownLoadStation.Trim(), 1);

                        if (passLog == null || prod.Status.Udt > passLog.Cdt)
                        {
                            throw new FisException("CHK845", new string[] { });
                        }
                    }
                }
                else if (this.EAPIAOutStation == this.Station)  //EPIA Out Station
                {
                    checkEPIAOutImageLog(prod, imgStation);
                }
            }
            #endregion
            if (!IsGetQCPIAStatus)
                return base.DoExecute(executionContext);

            #region Get QCStatus
            string[] tps = new string[2];
            tps[0] = "PIA";
            tps[1] = "PIA1";
            string status = "NA";
            string remark = "";
            IList<ProductQCStatus> QCStatusList = new List<ProductQCStatus>();
            QCStatusList = prodRep.GetQCStatusOrderByUdtDesc(prod.ProId, tps);
            if (QCStatusList != null && QCStatusList.Count > 0)
            {
                ProductQCStatus qcStatus = QCStatusList[0];
                status = qcStatus.Status.Trim();
                remark = qcStatus.Remark.Trim();
                if (status == "2")
                {
                    status = "PIA";
                    if (remark == "1")
                    {
                        status = "EPIA"; //epia
                    }
                }
                else if (status == "1")
                {
                    status = "NONE";
                }
                else if (remark == "1")
                {
                    status = "EPIAOut"; //epia
                }
                else if (status == "6" || status == "7")
                {
                    status = "PIAOut"; //pia
                }              
               
            }
            
            CurrentSession.AddValue(ExtendSession.SessionKeys.FAQCStatus, status);
            #endregion

            return base.DoExecute(executionContext);
        }

        private bool checkPreHoldStation(IProduct prod, out ProductLog lastLog, out ProductLog actualStationLog )
        {
            lastLog = null;
            actualStationLog = null;
            IProductRepository prodRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            ProductStatusExInfo preStation = (ProductStatusExInfo)CurrentSession.GetValue("PreStation");
            string isPreHoldStation = (string)CurrentSession.GetValue("IsPreHoldStation");
            string preStationId=null;
            string curStation = prod.Status.StationId;
            DateTime preStationUdt=DateTime.Now;

            if (preStation == null)
            {
                IList<ProductStatusExInfo> preStatusList = prodRep.GetProductPreStation(new List<string> { prod.ProId });
                if (preStatusList == null || 
                    preStatusList.Count == 0 || 
                    string.IsNullOrEmpty(preStatusList[0].PreStation))
                {
                    return false;
                }
               
                preStation = preStatusList[0];
                CurrentSession.AddValue("PreStation",preStation);
                preStationId = preStation.PreStation;
                preStationUdt = preStation.PreUdt;

                if (string.IsNullOrEmpty(isPreHoldStation))
                {
                    IStationRepository stationRep = RepositoryFactory.GetInstance().GetRepository<IStationRepository>();
                    isPreHoldStation = stationRep.GetStationAttrValue(preStationId, "IsHold");
                    CurrentSession.AddValue("IsPreHoldStation",isPreHoldStation);
                }
            }
            else
            {
                preStationId = preStation.PreStation;
                preStationUdt = preStation.PreUdt;
            }

            if (isPreHoldStation == "Y")
            {
                IList<string> excludeStation = new List<string>() {preStationId,
                                                                                          curStation,
                                                                                          "Release"};
                                                                                                   
                IList<ProductLog> prodLogList = prod.ProductLogs;
                
                var logList = prodLogList.Where(x => !excludeStation.Contains(x.Station));
                if (logList != null && logList.Count() > 0)
                {
                    lastLog = logList.OrderByDescending(x => x.Cdt).First();                    
                }

                var stationLogList = prodLogList.Where(x => x.Cdt < preStationUdt && x.Station == curStation);
                if (stationLogList != null && stationLogList.Count() > 0)
                {
                    actualStationLog = stationLogList.OrderByDescending(x => x.Cdt).First();                    
                }

               
                return true;                
            }

            return false;
        }

        /// <summary>
        /// 1)EAPIA Out站(6A):
        ///流程:50,6R, 6P, 6Q,65
        ///檢查:50至56站出站時間的最後一筆log是否為ProductInfo.InfoType='ImgStation' Station
        /// </summary>
        /// <param name="prod"></param>
        /// <param name="imgStation"></param>

        private void checkEPIAOutImageLog(IProduct prod, string imgStation)
        {
            IProductRepository prodRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            ProductLog lastProdLog = null;
            ProductLog lastStation = null;
            DateTime lastTrackOutTime =prod.Status.Udt;
            DateTime preTrackOutTime = DateTime.Now;
             IList<ProductLog> prodLogList = prod.ProductLogs;

            if (checkPreHoldStation(prod, out lastProdLog, out lastStation))
            {
                lastTrackOutTime = lastStation.Cdt;

                IList<TestLog> testLogList = prod.TestLog;
                var releaseTestLog = testLogList.Where(x => x.Station == "Release").OrderByDescending(x => x.Cdt).FirstOrDefault();
                if (releaseTestLog != null &&
                    !string.IsNullOrEmpty(releaseTestLog.Descr) )
                {                   
                    string preStation = releaseTestLog.Descr.Split(new char[] { '~' }).Where(x => x.Contains("PreStation:")).FirstOrDefault();
                    if (!string.IsNullOrEmpty(preStation))
                    {
                         preStation= (preStation.Split( new char[] { ':' }))[1].TrimEnd();
                         var preStationLog=prodLogList.Where(x => x.Station == preStation && x.Cdt < lastTrackOutTime).OrderByDescending(x => x.Cdt).FirstOrDefault();
                         if (preStationLog != null)
                         {
                             preTrackOutTime = preStationLog.Cdt;
                         }
                    }                   
                }                                               
            }
            else
            {
                ProductStatusExInfo preStation = (ProductStatusExInfo)CurrentSession.GetValue("PreStation");
                 preTrackOutTime = preStation.PreUdt;
            }  

            var imgLog = (from p in prodLogList
                                   orderby  p.Cdt descending  
                                where p.Cdt >preTrackOutTime && 
                                         p.Cdt <lastTrackOutTime &&
                                         p.Station == imgStation
                               select p).FirstOrDefault();
            if (imgLog != null)
            {                
                if (imgLog.Status != StationStatus.Pass)
                {
                    throw new FisException("CHK806", new string[] { });
                }
            }
            else
            {
                throw new FisException("CHK845", new string[] { });
            }
           
        }

	}
}
