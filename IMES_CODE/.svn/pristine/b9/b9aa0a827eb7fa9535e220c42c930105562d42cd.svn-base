/*
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:Activity/CheckImageDownLoadLog
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
* Known issues:
* TODO：
*/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Workflow.ComponentModel;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using System.ComponentModel;
using IMES.FisObject.Common.PrintLog;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.DataModel;
using IMES.Common;

namespace IMES.Activity
{
    /// <summary>
    /// TODO
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
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
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
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    /// </para> 
    /// </remarks>
    public partial class CheckImageDownLoadLog : BaseActivity
    {
        /// <summary> 
        /// </summary>
        public CheckImageDownLoadLog()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 输入DefaultImageDownLoadStation
        /// </summary>
        public static DependencyProperty DefaultImageDownLoadStationProperty = DependencyProperty.Register("DefaultImageDownLoadStation", typeof(string), typeof(CheckImageDownLoadLog), new PropertyMetadata("6P"));

        /// <summary>
        /// 输入DefaultImageDownLoadStation
        /// </summary>
        [DescriptionAttribute("DefaultImageDownLoadStation")]
        [CategoryAttribute("InArugment")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string DefaultImageDownLoadStation
        {
            get
            {
                return ((string)(base.GetValue(CheckImageDownLoadLog.DefaultImageDownLoadStationProperty)));
            }
            set
            {
                base.SetValue(CheckImageDownLoadLog.DefaultImageDownLoadStationProperty, value);
            }
        }

        /// <summary> 
        /// </summary>        
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            List<string> errpara = new List<string>();

			Product CurrentProduct = CurrentSession.GetValue(Session.SessionKeys.Product) as Product;
			if (CurrentProduct == null)
			{
                errpara.Add("");
				throw new FisException("SFC002", errpara);
			}

            string myDefaultImageDownLoadStation = DefaultImageDownLoadStation;

			IList<ConstValueInfo> lstCV = ActivityCommonImpl.Instance.GetConstValueListByType("SpecialDLModelStation", "");
            if (lstCV != null && lstCV.Count > 0)
            {
                bool bFound = false;
                foreach (ConstValueInfo cv in lstCV)
                {
                    if (ActivityCommonImpl.Instance.CheckModelByRegex(CurrentProduct.Model, cv.name))
                    {
                        myDefaultImageDownLoadStation = cv.value;
                        bFound = true;
                        break;
                    }
                }
                if (!bFound)
                {
                    foreach (ConstValueInfo cv in lstCV)
                    {
                        if (ActivityCommonImpl.Instance.CheckModelByRegex(CurrentProduct.Family, cv.name))
                        {
                            myDefaultImageDownLoadStation = cv.value;
                            break;
                        }
                    }
                }
            }
			
            var prodRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            ProductLog prodLog = prodRep.GetLatestLogByWc(CurrentProduct.ProId, myDefaultImageDownLoadStation);

            if (prodLog == null || CurrentProduct.Status.Udt > prodLog.Cdt)
            {
                throw new FisException("CQCHK0001", new string[] { myDefaultImageDownLoadStation });
            }
            if (prodLog.Status != IMES.FisObject.Common.Station.StationStatus.Pass)
            {
                throw new FisException("CQCHK0001", new string[] { myDefaultImageDownLoadStation });
            }

            return base.DoExecute(executionContext);
        }
    }
}