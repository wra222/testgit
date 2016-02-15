/* INVENTEC corporation (c)2011 all rights reserved. 
 * Description: 向QCStatus寫入記錄
 *                         
 * Update: 
 * Date         Name                         Reason 
 * ==========   =======================      ==========================
 * 2011-12-01   Kerwin                        Create
 * Known issues:
 */
using System;
using System.ComponentModel;
using System.Workflow.ComponentModel;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.Model;
using System.Collections.Generic;
using IMES.FisObject.Common.Part;
using IMES.FisObject.Common.Station;
using IMES.Infrastructure.Extend;
using IMES.DataModel;
using System.Data.SqlClient;
using IMES.Infrastructure.Repository._Schema;
using System.Data;
namespace IMES.Activity
{
    /// <summary>
    /// 增加抽检/向QCStatus寫入記錄/向ProductLog/寫入記錄
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      OQC Input,PAQC Input,PAQC Output
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         Insert QCStatus
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.SessionKeys.Product
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
    ///         无
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         IProduct
    ///         IProductRepository
    ///         ProductQCStatus
    /// </para> 
    /// </remarks>
    public partial class PAQC : BaseActivity
    {
        private static List<string> num = new List<string> { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
        /// <summary>
        /// 构造函数
        /// </summary>
        public PAQC()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 向QCStatus寫入記錄
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            IPartRepository ipartRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            IFamilyRepository famliyRep = RepositoryFactory.GetInstance().GetRepository<IFamilyRepository, Family>();
           
            IList<string> setList =new List<string>();
            string paqcStation = "PAQCStation";
            IProduct product = (IProduct)CurrentSession.GetValue(Session.SessionKeys.Product);

            #region Check PAQC Station Rule  by FamilyInfo.Category or BSam Model
            if ((string)CurrentSession.GetValue(ExtendSession.SessionKeys.IsBSamModel) == "Y")
            {
                paqcStation = "BSam" + paqcStation;
            }
            else
            {
                string family = product.Family;
                FamilyInfoDef fcond = new FamilyInfoDef();
                fcond.family = family;
                fcond.name = "Category";
                IList<FamilyInfoDef> famValList = famliyRep.GetExistFamilyInfo(fcond);
                if (famValList.Count > 0)
                {
                    paqcStation = famValList[0].value.Trim() + paqcStation;
                }
            }
            #endregion

            setList = ipartRepository.GetValueFromSysSettingByName(paqcStation);
      //     setList= ipartRepository.GetValueFromSysSettingByName("PAQCStation");;

            IList<string> siteList = ipartRepository.GetValueFromSysSettingByName("Site");
            string site = (siteList != null && siteList.Count > 0 && !string.IsNullOrEmpty(siteList[0])) ? siteList[0].Trim() : "IPC";
            
            try
            {
              
                if (setList.Count > 0 && setList[0].Trim() == CurrentSession.Station.Trim())
                {
                    //IProduct product = (IProduct)CurrentSession.GetValue(Session.SessionKeys.Product);
                    string writeStatus;
                    string remark = "";
                    int qcRatio = 0;
                    string lastQCStatus = "";                    
                    //string specialRuleQCStatus = "";
                    bool isQC = false;
                    CurrentSession.AddValue(Session.SessionKeys.QCStatus, false);
                    if (site != "ICC")
                    {
                        isQC = CalQCStatus(site, product, out qcRatio, out lastQCStatus);
                        if (isQC)
                        {
                            if (string.IsNullOrEmpty(lastQCStatus))
                            {
                                writeStatus = this.QCStatus;
                            }
                            else
                            {
                                writeStatus = lastQCStatus;
                            }

                            remark = this.Remark.Trim();
                        }
                        ////else if( site=="ICC" && qcRatio>0  && CalQCStatusForICC(product)){
                        //else if (site == "ICC" && qcRatio > 0 && CalQCStatusForICCSpecialRule(product, out specialRuleQCStatus))
                        //{
                        //    //writeStatus = string.IsNullOrEmpty(this.QCSpecialStatus)?this.QCStatus: this.QCSpecialStatus ;
                        //    writeStatus = specialRuleQCStatus;
                        //    remark = this.Remark.Trim();
                        //    isQC = true;
                        //}
                        else
                        {
                            writeStatus = this.NotQCStatus;
                        }
                    }
                    else  //for ICC Site
                    {
                        bool bByModel=false;
                        bool bByFamily =false;
                       isQC= CheckReworkQCStatusAndRatio(product, out qcRatio, out lastQCStatus, out bByModel, out bByFamily);
                       if (isQC)
                       {
                           if (string.IsNullOrEmpty(lastQCStatus))
                           {
                               writeStatus = this.QCStatus;
                           }
                           else
                           {
                               writeStatus = lastQCStatus;
                           }
                           remark = this.Remark.Trim();
                       }
                       else if (qcRatio == 0) //禁止抽檢
                       {
                           isQC = false;
                           writeStatus = this.NotQCStatus;
                       }
                       else
                       {
                           isQC = CalQCStatusForICCSP(product, bByModel, bByFamily, qcRatio, out writeStatus);
                       }


                    }

                    CurrentSession.AddValue(Session.SessionKeys.QCStatus, isQC);

                  
                    ProductQCStatus status = new ProductQCStatus(-1,
                                                                                                product.ProId,
                                                                                                Type,
                                                                                                string.IsNullOrEmpty(this.Line) ? product.Status.Line : this.Line,
                                                                                                product.Model,
                                                                                                writeStatus,
                                                                                                Editor,
                                                                                                DateTime.Now,
                                                                                                DateTime.Now);
                    status.Remark = remark;
                    product.AddQCStatus(status);


                    if (!string.IsNullOrEmpty(ProductAttrName))
                    {
                        product.SetAttributeValue(ProductAttrName, writeStatus, Editor, this.Station, remark);
                    }
                    else if (!string.IsNullOrEmpty(Type))
                    {
                        product.SetAttributeValue(Type + "_QCStatus", writeStatus, Editor, this.Station, remark);
                    }
                   

                    if (IsWriteProductLog && isQC)
                    {
                        WriteProductLog(product);
                    }

                    IProductRepository prodRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
                    prodRep.Update(product, CurrentSession.UnitOfWork);
                
                }
            }
             catch (FisException fe)
            {
                fe.stopWF = IsStopWF;
                throw fe;
              
            }
            return base.DoExecute(executionContext);
        }

        private bool CheckReworkQCStatusAndRatio(IProduct product, out int ratio, out string lastQCStatus, out  bool bByModel, out bool bByFamily)
        {
            lastQCStatus = "";
            bool result = false;
            string tmpline = this.Line.Trim();
            //DateTime qcStartTime = new DateTime(1900, 1, 1);
            bByModel = false;
            bByFamily = false;
            IFamilyRepository CurrentRepository = RepositoryFactory.GetInstance().GetRepository<IFamilyRepository, Family>();
            IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            //By Model -> Family ->Line
            #region 抓取QCRatio By Model ->Family -> Line
            QCRatio currentQCRatio = CurrentRepository.GetQCRatio(product.Model);
            if (currentQCRatio == null || currentQCRatio.PAQCRatio == 0)
            {
                currentQCRatio = CurrentRepository.GetQCRatio(product.Family);
                if (currentQCRatio == null || currentQCRatio.PAQCRatio == 0)
                {
                    currentQCRatio = CurrentRepository.GetQCRatio(tmpline.Substring(0, 1));
                    if (currentQCRatio == null)
                    {
                        List<string> errpara = new List<string>();
                        throw new FisException("CHK040", errpara);//change Error msg
                    }
                }
                else
                {
                    bByFamily = true;
                }
            }
            else
            {
                bByModel = true;
            }
            //---------------------------------------------------------------------

            int iQCRadio = currentQCRatio.PAQCRatio;
            ratio = iQCRadio;
            //if (iQCRadio == 0)
            //{
            //    result = false;
            //    //CurrentSession.AddValue(Session.SessionKeys.QCStatus, result);
            //    return result;
            //}
            #endregion


            #region 檢查重流PAQC
            string[] tps = new string[1];
            tps[0] = this.Type;

            IList<ProductQCStatus> QCStatusList = new List<ProductQCStatus>();
            QCStatusList = productRepository.GetQCStatusOrderByCdtDesc(product.ProId, tps);
            bool isLastQCStatus = true;
            foreach (ProductQCStatus tmp in QCStatusList)
            {
                if (tmp.Remark == this.Remark)
                {
                    result = true;
                    //last QCStatus need return lastQCStatus
                    if (isLastQCStatus)
                    {
                        lastQCStatus = tmp.Status;
                    }
                    break;
                }
                isLastQCStatus = false;
            }

            //if (result)
            //{
            //    //CurrentSession.AddValue(Session.SessionKeys.QCStatus, result);
            //    return result;
            //}

            #endregion
            return result;
        }

        private bool CalQCStatus(string site, IProduct product, out int ratio, out string lastQCStatus)
        {
            lastQCStatus = "";
            bool result=false ;
            string tmpline = this.Line.Trim();
            //DateTime qcStartTime = new DateTime(1900, 1, 1);
            bool bByModel=false;
            bool bByFamily = false;
            IFamilyRepository CurrentRepository = RepositoryFactory.GetInstance().GetRepository<IFamilyRepository, Family>();
            IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            //By Model -> Family ->Line
            #region 抓取QCRatio By Model ->Family -> Line
            QCRatio currentQCRatio = CurrentRepository.GetQCRatio(product.Model);
            if (currentQCRatio == null || currentQCRatio.PAQCRatio==0)
            {
                currentQCRatio = CurrentRepository.GetQCRatio(product.Family);
                if (currentQCRatio == null || currentQCRatio.PAQCRatio==0)
                {
                    currentQCRatio = CurrentRepository.GetQCRatio(tmpline.Substring(0, 1));
                    if (currentQCRatio == null)
                    {
                        List<string> errpara = new List<string>();
                        throw new FisException("CHK040", errpara);//change Error msg
                    }
                }
                else
                {
                    bByFamily = true;
                }
            }
            else
            {
                bByModel = true;
            }
            //---------------------------------------------------------------------

            int iQCRadio = currentQCRatio.PAQCRatio;
            ratio = iQCRadio;
            if (iQCRadio == 0)
            {
                result = false;
                //CurrentSession.AddValue(Session.SessionKeys.QCStatus, result);
                return result;
            }
           #endregion

            #region 檢查重流PAQC 
            string[] tps = new string[1];
            tps[0] = this.Type;

            IList<ProductQCStatus> QCStatusList = new List<ProductQCStatus>();
            QCStatusList = productRepository.GetQCStatusOrderByCdtDesc(product.ProId, tps);
            bool isLastQCStatus = true;
            foreach (ProductQCStatus tmp in QCStatusList)
            {
                if (tmp.Remark == this.Remark)
                {
                    result = true;
                    //last QCStatus need return lastQCStatus
                    if (isLastQCStatus)
                    {
                        lastQCStatus = tmp.Status;
                    }
                    break;
                }
                isLastQCStatus = false;
            }

            if (result)
            {
                //CurrentSession.AddValue(Session.SessionKeys.QCStatus, result);
                return result;
            }

            #endregion

            //获取Product数量@Count：
            //   IProduct product = (IProduct)CurrentSession.GetValue(Session.SessionKeys.Product);
            #region 計算PAQC站流過機器總數 by Line & Model ,  by Line & Family, by Line
            int iQcCount=0;
            if (site == "ICC")
            {
                DateTime now = DateTime.Now;

                DateTime befoeOneDayNow = now.AddDays(-1);
               
                //每天以07:50開始計算
                DateTime startTime = new DateTime(now.Year, now.Month, now.Day, 7, 50, 0);
                DateTime endTime = now;
                DateTime beforeTime = new DateTime(befoeOneDayNow.Year, befoeOneDayNow.Month, befoeOneDayNow.Day, 7, 50, 0);
           
                if (!(now > startTime))  // 換日時case
               {
                    startTime = beforeTime;
                }
                if (bByModel)
                {
                    iQcCount = productRepository.GetSampleCountByPdLineModel(this.Type, tmpline, product.Model, startTime, endTime);
                }
                else if (bByFamily)
                {
                    iQcCount = productRepository.GetSampleCountByPdLineFamily(this.Type, tmpline, product.Family, startTime, endTime);
                }
                else
                {
                    iQcCount = productRepository.GetSampleCountByPdLine(this.Type, tmpline, startTime, endTime);
                }

            }
            else
            {
                if (bByModel)
                {
                    iQcCount = productRepository.GetSampleCountByModel(this.Type, tmpline, product.Model);
                }
                else if (bByFamily)
                {
                    iQcCount = productRepository.GetSampleCountByFamily(this.Type, tmpline, product.Family);
                }
                else
                {
                    iQcCount = productRepository.GetSampleCount(this.Type, tmpline);
                }
            }
            if ((iQcCount == 0))
            {
                result = true;
                
            }
            else if (iQCRadio != 0)
            {
                if (iQcCount % iQCRadio == 0)
                {
                    result = true;

                }
                else
                {
                    result = false;

                }
            }
            else
            {
                result = false;
            }
            #endregion
            //    IProductRepository repProduct = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();

            

            //if (currentQCRatio.PAQCRatio == 0)
            //{
            //    result = false;
            //}
            //CurrentSession.AddValue(Session.SessionKeys.QCStatus, result);
            return result;
        
        }

        private bool CalQCStatusForICC(IProduct product)
        {

            bool result = false;
            //For ICC add check CTO or BTO

            IProductRepository prodRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            IPartRepository PartRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            //IList<string> valueList = PartRepository.GetValueFromSysSettingByName("Site");
            //if (valueList !=null && valueList.Count > 0 && valueList.Contains("ICC"))
            //{
            string model = product.Model;
            //List<string> num = new List<string> { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
            // [IMES2012_GetData].[dbo].[Model].[Model] 机型12码第七码是字母的是BTO，第七码是数字的是CTO,Y为特殊类型
            if (!num.Contains(model.Substring(6, 1)))  //BTO 
            {
                if (!prodRep.ExistQCStatusByPdLineModelTp(this.Line, model, this.Type))
                {
                    result = true;
                }
            }
            else
            {
                if (!prodRep.ExistQCStatusByPdLineRegionTp(this.Line, model.Substring(9, 3), this.Type))
                {
                    result = true;
                }
            }
            // }
            //CurrentSession.AddValue(Session.SessionKeys.QCStatus, result);
            return result;
        }

        private bool CalQCStatusForICCSpecialRule(IProduct product, out string status)
        {
            status = "";
            bool result = false;
            //For ICC add check CTO or BTO

            IProductRepository prodRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            IPartRepository PartRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            IList<string> checkDays = PartRepository.GetValueFromSysSettingByName("QCStatusCheckDay");
            int intervalDays = 15;
            if (checkDays != null || checkDays.Count > 0)
            {
                intervalDays = int.Parse(checkDays[0]);
            }

            string model = product.Model;
            //List<string> num = new List<string> { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
            // [IMES2012_GetData].[dbo].[Model].[Model] 机型12码第七码是字母的是BTO，第七码是数字的是CTO,Y为特殊类型
            if (!num.Contains(model.Substring(6, 1)))  //BTO 
            {
                bool existIECSample = prodRep.ExistQCStatusByModelTpStatus(model,
                                                                                                                this.Type,
                                                                                                                new List<string>() { this.QCStatus, this.QCSpecialStatus },
                                                                                                                intervalDays);

                if (existIECSample)
                {
                    bool existHPSample = prodRep.ExistQCStatusByModelTpStatus(model,
                                                                                                                     this.Type,
                                                                                                                     new List<string>() { this.QCHpStatus },
                                                                                                                     intervalDays);
                    if (!existHPSample)
                    {
                        status = this.QCHpStatus;
                        result = true;
                    }

                }
                else
                {
                    status = this.QCSpecialStatus;
                    result = true;
                }
            }
            else
            {
                IList<string> ctoSampleCount = PartRepository.GetValueFromSysSettingByName("QCCTOSampleCount");
                int NeedCheckSampleCount = 10;
                if (ctoSampleCount != null || ctoSampleCount.Count > 0)
                {
                    NeedCheckSampleCount = int.Parse(ctoSampleCount[0]);
                }
                int sampleCount = prodRep.GetSampleCountByModelDays(this.Type, model, intervalDays);
                if (sampleCount > NeedCheckSampleCount)
                {
                    bool existIECSample = prodRep.ExistQCStatusByModelTpStatus(model,
                                                                                                                    this.Type,
                                                                                                                    new List<string>() { this.QCStatus, this.QCSpecialStatus },
                                                                                                                    intervalDays);

                    if (existIECSample)
                    {
                        bool existHPSample = prodRep.ExistQCStatusByModelTpStatus(model,
                                                                                                                         this.Type,
                                                                                                                         new List<string>() { this.QCHpStatus },
                                                                                                                         intervalDays);
                        if (!existHPSample)
                        {
                            status = this.QCHpStatus;
                            result = true;
                        }

                    }
                    else
                    {
                        status = this.QCSpecialStatus;
                        result = true;
                    }
                }

            }
            //CurrentSession.AddValue(Session.SessionKeys.QCStatus, result);
            return result;
        }


        private bool CalQCStatusForICCSP(IProduct product, bool bByModel, bool bByFamily, int qcRatio, out string status)
        {
            status = this.NotQCStatus;
            DateTime now = DateTime.Now;

            DateTime befoeOneDayNow = now.AddDays(-1);

            //每天以07:50開始計算
            DateTime startTime = new DateTime(now.Year, now.Month, now.Day, 7, 50, 0);
            DateTime endTime = now;
            DateTime beforeTime = new DateTime(befoeOneDayNow.Year, befoeOneDayNow.Month, befoeOneDayNow.Day, 7, 50, 0);

            if (!(now > startTime))  // 換日時case
            {
                startTime = beforeTime;
            }

            string pdLine = this.Line.Trim();

            string whichTotalType = bByModel? "Model": (bByFamily? "Family":  "Line");             
            string orderType = "CTO";           
            // [IMES2012_GetData].[dbo].[Model].[Model] 机型12码第七码是字母的是BTO，第七码是数字的是CTO,Y为特殊类型
            if (!num.Contains(product.Model.Substring(6, 1)))
            {
                orderType = "BTO";
            }

            //for ICC call PROCEDURE [dbo].[IMES_PAQC]
                //@QCType			varchar(8),  --PAQC
                //@WhichTotalType varchar(8), --Model,Family.Line
                //@QCRatio        int, 
                //@PdLine         varchar(8),
                //@ProductId      varchar(16),
                //@Model          varchar(16),
                //@Family         varchar(16),
                //@OrderType      varchar(8),  --BTO/CTO
                //@StartTime		datetime,
                //@EndTime		datetime

            List<SqlParameter> paramsArray = new List<SqlParameter>();
            paramsArray.Add(new SqlParameter("QCType",this.Type));
            paramsArray.Add(new SqlParameter("WhichTotalType", whichTotalType));
            paramsArray.Add(new SqlParameter("QCRatio", qcRatio));
            paramsArray.Add(new SqlParameter("PdLine", pdLine));
            paramsArray.Add(new SqlParameter("ProductId", product.ProId));

            paramsArray.Add(new SqlParameter("Model", product.Model));
            paramsArray.Add(new SqlParameter("Family", product.Family));
            paramsArray.Add(new SqlParameter("OrderType", orderType));
            paramsArray.Add(new SqlParameter("StartTime", startTime));
            paramsArray.Add(new SqlParameter("EndTime", endTime));


           
            IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            DataTable dt = new DataTable();
            dt = productRepository.ExecSpForQuery(SqlHelper.ConnectionString_PAK, "IMES_PAQC", paramsArray.ToArray());

            if (dt.Rows.Count != 1)
            {
                throw new Exception("SP return value is invalid![row cnt:" + dt.Rows.Count + "]");
            }
            DataRow row = dt.Rows[0];
            string qcStatus = row[0].ToString().Trim().ToUpper();
            if (qcStatus == "SPAQC")
            {
                status = this.QCStatus;
                return true;
            }
            else if (qcStatus == "PAQC")
            {
                status = this.QCSpecialStatus;
                return true;
            }
            else if (qcStatus == "HPAQC")
            {
                status = this.QCHpStatus;
                return true;
            }
            else
            {
                status = this.NotQCStatus;
                return false;
            }
        }

        private void WriteProductLog(IProduct product)
        {
         //   var product = CurrentSession.GetValue(Session.SessionKeys.Product) as IProduct;
          // var productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
      
           string line = string.IsNullOrEmpty(this.Line) ? product.Status.Line : this.Line;
           string station = string.IsNullOrEmpty(this.Station) ? CurrentSession.Station:this.Station;
            var productLog = new ProductLog
            {
                Model = product.Model,
                Status = StationStatus.Pass,
                Editor = this.Editor,
                Line = line,
                Station = station,
                Cdt = DateTime.Now
            };

            product.AddLog(productLog);
            //productRepository.Update(product, CurrentSession.UnitOfWork);
        
        }

       

        /// <summary>
        /// 状态值
        /// </summary>
        public static DependencyProperty QCStatusProperty = DependencyProperty.Register("QCStatus", typeof(string), typeof(PAQC));

        /// <summary>
        /// 状态值
        /// </summary>
        [DescriptionAttribute("QCStatus")]
        [CategoryAttribute("QCStatus Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string QCStatus
        {
            get
            {
                return ((string)(base.GetValue(PAQC.QCStatusProperty)));
            }
            set
            {
                base.SetValue(PAQC.QCStatusProperty, value);
            }
        }


        /// <summary>
        /// 状态值
        /// </summary>
        public static DependencyProperty QCSpecialStatusProperty = DependencyProperty.Register("QCSpecialStatus", typeof(string), typeof(PAQC), new PropertyMetadata("B"));

        /// <summary>
        /// 状态值
        /// </summary>
        [DescriptionAttribute("QCSpecialStatus")]
        [CategoryAttribute("QCStatus Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string QCSpecialStatus
        {
            get
            {
                return ((string)(base.GetValue(PAQC.QCSpecialStatusProperty)));
            }
            set
            {
                base.SetValue(PAQC.QCSpecialStatusProperty, value);
            }
        }

        /// <summary>
        /// 状态值
        /// </summary>
        public static DependencyProperty NotQCStatusProperty = DependencyProperty.Register("NotQCStatus", typeof(string), typeof(PAQC));

        /// <summary>
        /// 状态值
        /// </summary>
        [DescriptionAttribute("NotQCStatus")]
        [CategoryAttribute("NotQCStatus Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string NotQCStatus
        {
            get
            {
                return ((string)(base.GetValue(PAQC.NotQCStatusProperty)));
            }
            set
            {
                base.SetValue(PAQC.NotQCStatusProperty, value);
            }
        }

        /// <summary>
        /// 状态值
        /// </summary>
        public static DependencyProperty QCHpStatusProperty = DependencyProperty.Register("QCHpStatus", typeof(string), typeof(PAQC), new PropertyMetadata("C"));

        /// <summary>
        /// 状态值
        /// </summary>
        [DescriptionAttribute("QCHpStatus")]
        [CategoryAttribute("QCStatus Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string QCHpStatus
        {
            get
            {
                return ((string)(base.GetValue(PAQC.QCHpStatusProperty)));
            }
            set
            {
                base.SetValue(PAQC.QCHpStatusProperty, value);
            }
        }

        /// <summary>
        /// 类型值
        /// </summary>
        public static DependencyProperty TypeProperty = DependencyProperty.Register("Type", typeof(string), typeof(PAQC), new PropertyMetadata(""));

        /// <summary>
        /// 类型值
        /// </summary>
        [DescriptionAttribute("Type")]
        [CategoryAttribute("Type Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string Type
        {
            get
            {
                return ((string)(base.GetValue(PAQC.TypeProperty)));
            }
            set
            {
                base.SetValue(PAQC.TypeProperty, value);
            }
        }

        /// <summary>
        /// 类型值
        /// </summary>
        public static DependencyProperty RemarkProperty = DependencyProperty.Register("Remark", typeof(string), typeof(PAQC), new PropertyMetadata(""));

        /// <summary>
        /// 类型值
        /// </summary>
        [DescriptionAttribute("Remark")]
        [CategoryAttribute("Remark Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string Remark
        {
            get
            {
                return ((string)(base.GetValue(PAQC.RemarkProperty)));
            }
            set
            {
                base.SetValue(PAQC.RemarkProperty, value);
            }
        }
		 //Add by Benson
        /// <summary>
        /// 类型值 Product Attr Name
        /// </summary>
        public static DependencyProperty ProductAttrNameProperty = DependencyProperty.Register("ProductAttrName", typeof(string), typeof(PAQC), new PropertyMetadata(""));


        /// <summary>
        /// 类型值
        /// </summary>
        [DescriptionAttribute("ProductAttrName")]
        [CategoryAttribute("ProductAttrName Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string ProductAttrName
        {
            get
            {
                return ((string)(base.GetValue(PAQC.ProductAttrNameProperty)));
            }
            set
            {
                base.SetValue(PAQC.ProductAttrNameProperty, value);
            }
        }
        /// <summary>
        /// Exception時要停止workflow
        /// </summary>
        public static DependencyProperty IsStopWFProperty = DependencyProperty.Register("IsStopWF", typeof(bool), typeof(PAQC), new PropertyMetadata(true));
        /// <summary>
        ///  Exception時要停止workflow
        /// </summary>
        [DescriptionAttribute("IsStopWF")]
        [CategoryAttribute("IsStopWF")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public bool IsStopWF
        {
            get
            {
                return ((bool)(base.GetValue(PAQC.IsStopWFProperty)));
            }
            set
            {
                base.SetValue(PAQC.IsStopWFProperty, value);
            }
        }
        /// <summary>
        /// Flag for Write ProductLog
        /// </summary>
        public static DependencyProperty IsWriteProductLogProperty = DependencyProperty.Register("IsWriteProductLog", typeof(bool), typeof(PAQC), new PropertyMetadata(true));
        /// <summary>
        ///  Flag for Write ProductLog
        /// </summary>
        [DescriptionAttribute("IsWriteProductLog")]
        [CategoryAttribute("IsWriteProductLog")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public bool IsWriteProductLog
        {
            get
            {
                return ((bool)(base.GetValue(PAQC.IsWriteProductLogProperty)));
            }
            set
            {
                base.SetValue(PAQC.IsWriteProductLogProperty, value);
            }
        }
    }
}
