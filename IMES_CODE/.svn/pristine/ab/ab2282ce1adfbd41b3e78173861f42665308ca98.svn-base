/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: PCARepairImpl
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2009-11-03   207006     Create 
 * 2011-12-26   94128      modify(Change interface)
 *[RunInTimeControl] 增加字段 TestStation和 ControlType
 * * GetRunInTimeControlByTypeAndCode->GetRunInTimeControlByTypeAndTestStation(string type, string testStation);
 * Known issues:Any restrictions about this file 
 */



using System;
using System.Workflow.ComponentModel;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.Generic;
using IMES.FisObject.FA.Product;
using IMES.DataModel;
using System.ComponentModel;

namespace IMES.Activity
{
    /// <summary>
    /// 测试时间间隔检查 
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///         CI-MES12-SPEC-FA-UC FA Test Station
    ///         
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///        不符合要求则抛出异常
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.Product
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
    ///          IProductRepository
    /// </para> 
    /// </remarks>
    public partial class RunInTimeControl : BaseActivity
    {

        /// <summary>
        /// InitializeComponent-- RunInTimeControl
        /// </summary>
        public RunInTimeControl()
        {
            InitializeComponent();
        }

        /// <summary>
        ///By Pass Control Station
        /// </summary>
        public static DependencyProperty ByPassControlStationProperty = DependencyProperty.Register("ByPassControlStation", typeof(string), typeof(RunInTimeControl), new PropertyMetadata(""));
        /// <summary>
        ///  禁用時要停止workflow
        /// </summary>
        [DescriptionAttribute("ByPassControlStation")]
        [CategoryAttribute("ByPassControlStation")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string ByPassControlStation
        {
            get
            {
                return ((string)(base.GetValue(RunInTimeControl.ByPassControlStationProperty)));
            }
            set
            {
                base.SetValue(RunInTimeControl.ByPassControlStationProperty, value);
            }
        }

        /// <summary>
        /// DoExecute
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {

            IProduct currentProduct = (IProduct)this.CurrentSession.GetValue(Session.SessionKeys.Product);
            var productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            var strCustomerSN = currentProduct.CUSTSN;
            var strModel = currentProduct.Model;
            var strFamily = currentProduct.Family;
            var bSpecialRun = false;
            bool IsNeedCheckSN = this.CurrentSession.GetValue("IsPassCheckModel") == null ? true : !(bool)this.CurrentSession.GetValue("IsPassCheckModel");
            if (IsNeedCheckSN)
            {
                if (strCustomerSN.Trim().Length == 0)
                {
                    List<string> errpara1 = new List<string>();
                    errpara1.Add(this.Key);
                    errpara1.Add("CPQSNO");
                    throw new FisException("CHK074", errpara1);
                }
            }

            //New Add 2012/04/06 UC Update
            //-----------------------------------------BEGIN--------------------------------------------------------------
            //若刚修护完成的机器，则不执行下面的操作，省略该项检查。
            //select * from ProductStatus nolock where Station = '45' and ProductID = @PRDID
            ProductStatusInfo statusInfo = productRepository.GetProductStatusInfo(currentProduct.ProId);
            if (statusInfo.station != null)
            {
                if (statusInfo.station.Trim() == this.ByPassControlStation)
                {
                    CurrentSession.AddValue("HaveError", "NO!");
                    return base.DoExecute(executionContext);
                }
            }
            //-----------------------------------------END---------------------------------------------------------------- 
            //Vincent 2013-09-18 disable hardcode station logical , Check logical depend on RunInTimeControl table setup
            //if ((this.Station.Trim() != "50") && (this.Station.Trim() != "55") && (this.Station.Trim() != "60"))
            //{
            //    CurrentSession.AddValue("HaveError", "NO!");
            //    return base.DoExecute(executionContext);
            //}

            var timeList = productRepository.GetRunInTimeControlByTypeAndTestStation("CPQSNO", this.Station, strCustomerSN);
            decimal time = 0m;

            if (timeList == null || timeList.Count == 0)
            {

                if (strModel.Trim().Length == 0)
                {
                    List<string> errpara2 = new List<string>();
                    errpara2.Add(this.Key);
                    errpara2.Add("Model");
                    throw new FisException("CHK074", errpara2);
                }
                timeList = productRepository.GetRunInTimeControlByTypeAndTestStation("Model", this.Station, strModel);
                if (timeList == null || timeList.Count == 0)
                {

                    if (strFamily.Trim().Length == 0)
                    {
                        List<string> errpara3 = new List<string>();
                        errpara3.Add(this.Key);
                        errpara3.Add("Family");
                        throw new FisException("CHK074", errpara3);
                    }
                    timeList = productRepository.GetRunInTimeControlByTypeAndTestStation("Family", this.Station, strFamily);
                    if (timeList == null || timeList.Count == 0)
                    {
                        time = 0m;
                        //如果记录不存在，提示请维护Run-in时间
                        throw new FisException("CHK208", new List<string>());
                    }
                    else
                    {
                        decimal.TryParse(timeList[0].Hour, out time);
                    }
                }
                else
                {
                    decimal.TryParse(timeList[0].Hour, out time);
                }
            }
            else
            {
                decimal.TryParse(timeList[0].Hour, out time);
            }
            //test time 为0时，则不卡
            CurrentSession.AddValue("JustKeyDefectCode", false);
            if (time != 0)
            {

                DateTime now = DateTime.Now;
                DateTime logTime = productRepository.GetTheNewestTime(currentProduct.ProId);
                TimeSpan minus = now - logTime;


                if (timeList[0].ControlType) //1
                {
                    //当ControlType 为1（不足设定时间卡住）,是否超过测试时长，如果超过则Pass；否则报告错误“该Product Run In 测试时间没有超过规定时长，请联系相关负责人员!!”
                    if ((decimal)minus.TotalHours < time)
                    {
                        //throw new FisException("CHK104", new List<string>());
                        // KaishengUpdate 2012/05/02  UCUpdate:
                        // 是否超过测试时长，如果超过则Pass；
                        // 否则报告错误“该机器RunIn的时间不够！还差(RunInTimeControl.Hour*60-@DiffTime)分钟”

                        CurrentSession.AddValue("JustKeyDefectCode", true);
                        decimal diffminus = time * 60 - (decimal)minus.TotalMinutes;
                        string sdiffminus = diffminus.ToString("0");
                        CurrentSession.AddValue("RunInTimes", sdiffminus);
                        //List<string> errpara1 = new List<string>();
                        //errpara1.Add(sdiffminus);
                        //throw new FisException("CHK093", errpara1);
                    }
                }
                else  //0
                {
                    //当ControlType 为0 （超过设定时间卡住），是否超过测试时长，如果不超过则Pass；否则报告错误“Fail！，该Product 测试超过设定的测试时间”
                    if ((decimal)minus.TotalHours > time)
                    {
                        try
                        {
                            bSpecialRun = true;
                            throw new FisException("CHK105", new List<string>());

                        }
                        catch (FisException ex)
                        {
                            CurrentSession.AddValue("HaveError", ex.mErrmsg);
                        }
                    }
                    else
                    {
                        CurrentSession.AddValue("HaveError", "NO!");
                    }

                }
            }
            /*
            if ((!(time == 0m)) && (this.Station == "55"))
            {
                DateTime now = DateTime.Now;
                DateTime logTime = productRepository.GetTheNewestTime(currentProduct.ProId);

                TimeSpan minus = now - logTime;

                if ((decimal)minus.TotalHours < time)
                {
                    throw new FisException("CHK103", new List<string>());
                }
            }
             */
            if (bSpecialRun == false)
            {
                CurrentSession.AddValue("HaveError", "NO!");
            }

            return base.DoExecute(executionContext);
        }
    }
}
