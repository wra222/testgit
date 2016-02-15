using System;
using System.Workflow.ComponentModel;
using IMES.FisObject.Common.Model;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.DataModel;
using System.Collections.Generic;

namespace IMES.Activity
{
    /// <summary>
    /// 
    /// </summary>
    public partial class GetQCForPAQCCosmetic : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public GetQCForPAQCCosmetic()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 检查是否满足complete条件, 更新Repair状态
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            try
            {
                //PAQC抽检率取自QCRatio.PAQCRatio 栏位
                //按照Family = 页面选择PdLine 的第一码 获取PAQCRatio，
                //如果没有，则报告错误：“Please maintain QCRatio!”
                IFamilyRepository CurrentRepository = RepositoryFactory.GetInstance().GetRepository<IFamilyRepository, Family>();
                
                QCRatioInfo cond = new QCRatioInfo();
                cond.family = this.Line.Substring(0, 1);
                IList<QCRatioInfo> list = new List<QCRatioInfo>();
                list = CurrentRepository.GetQCRatioInfoList(cond);
                if(list == null || list.Count == 0)
                {
                    List<string> errpara = new List<string>();
                    throw new FisException("CHK040", errpara);//change Error msg
                }
                int iQCRadio = list[0].rpaqcratio;

                /*QCRatio currentQCRatio = CurrentRepository.GetQCRatio(this.Line.Substring(0, 1));
                //若@QCRatio为空或者为Null，则报错：“请维护当前Line的QC抽检率”
                //if ((currentQCRatio == null) || (currentQCRatio.PAQCRatio == 0))
                if ((currentQCRatio == null))
                {
                    List<string> errpara = new List<string>();
                    throw new FisException("CHK040", errpara);//change Error msg
                }
                int iQCRadio = currentQCRatio.PAQCRatio;
                */
                if (iQCRadio != 0)
                {
                    //SELECT @cnt=count(DISTINCT ProductID) FROM QCStatus WHERE Tp='PAQC'
                    //AND LEFT(Line, 1) = LEFT(@pdline, 1)
                    //AND Cdt >= @StartDate AND Cdt < @EndDate

                    IModelRepository modelRepository = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();
                    DateTime currentTime = DateTime.Now;
                    int iProductCount = 0;
                    if (currentTime.Hour >= 12)
                    {
                        DateTime startTime = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day, 12, 0, 0);
                        DateTime endTime = new DateTime(currentTime.AddDays(1).Year, currentTime.AddDays(1).Month, currentTime.AddDays(1).Day, 12, 0, 0);

                        iProductCount = modelRepository.GetCountOfQcStatus("PAQC", this.Line.Substring(0, 1), startTime, endTime);
                    }
                    else
                    {
                        DateTime startTime = new DateTime(currentTime.AddDays(-1).Year, currentTime.AddDays(-1).Month, currentTime.AddDays(-1).Day, 12, 0, 0);
                        DateTime endTime = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day, 12, 0, 0);

                        iProductCount = modelRepository.GetCountOfQcStatus("PAQC", this.Line.Substring(0, 1), startTime, endTime);
                    }


                    CurrentSession.AddValue(Session.SessionKeys.C, iQCRadio);
                    CurrentSession.AddValue(Session.SessionKeys.Prod1, iProductCount);


                    int res = iProductCount % iQCRadio;
                    if (res == 0)
                    {
                        CurrentSession.AddValue("GoodSample", "1");
                    }
                    else
                    {
                        CurrentSession.AddValue("GoodSample", "0");
                    }
                }
                else
                {
                    CurrentSession.AddValue(Session.SessionKeys.C, 0);
                    CurrentSession.AddValue(Session.SessionKeys.Prod1, 0);
                    CurrentSession.AddValue("GoodSample", "0");
                }

                return base.DoExecute(executionContext);
            }
            catch (FisException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
