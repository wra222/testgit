// INVENTEC corporation (c)2010all rights reserved. 
// Description:CI-MES12-SPEC-FA-UC IEC Label Print.docx
//             获取DCode            
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2010-12-01   zhu lei                      create
// Known issues:
using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;
using IMES.FisObject.PAK;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.Warranty;
using IMES.FisObject.Common.Part;
using IMES.DataModel;

namespace IMES.Activity
{
    /// <summary>
    /// 获取DCode
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      应用于IECLabelPrint
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         获取DCode
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///        Session.SessionKeys.WarrantyCode 
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         无
    /// </para> 
    ///<para> 
    /// 输出：
    ///         DCode
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///         无
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///              IWarrantyRepository
    ///              Warranty
    /// </para> 
    /// </remarks>
    public partial class GetDCodeForShipping : BaseActivity
    {
        /// <summary>
        /// constructor
        /// </summary>
        public GetDCodeForShipping()
        {
            InitializeComponent();
        }
        /// <summary>
        /// MBShipMode Category
        /// </summary>
        public enum MBShipModeCategory
        {
            /// <summary>
            /// 
            /// </summary>
            SKU=1,
            /// <summary>
            /// 
            /// </summary>
            RCTO,
            /// <summary>
            /// 
            /// </summary>
            FRU,
            /// <summary>
            /// 
            /// </summary>
            RMA
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty CQSideCodeMBShipModeProperty = DependencyProperty.Register("CQSideCodeMBShipMode", typeof(MBShipModeCategory), typeof(GetDCodeForShipping), new PropertyMetadata(MBShipModeCategory.FRU));

        /// <summary>
        /// CQ SideCode MBShipMode Category
        /// </summary>
        [DescriptionAttribute("CQ SideCode MBShipMode Category")]
        [CategoryAttribute("InArguments Of MBShipMode")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public MBShipModeCategory CQSideCodeMBShipMode
        {
            get
            {
                return ((MBShipModeCategory)(base.GetValue(GetDCodeForShipping.CQSideCodeMBShipModeProperty)));
            }
            set
            {
                base.SetValue(GetDCodeForShipping.CQSideCodeMBShipModeProperty, value);
            }
        }
        /// <summary>
        /// 获取DCode
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            DateTime dt = DateTime.Now;
            string year = string.Empty;
            string month = string.Empty;
            string siteCode = string.Empty;
            string dataCode = (string)CurrentSession.GetValue(Session.SessionKeys.WarrantyCode);

            IPartRepository partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            IList<string> valueList = partRepository.GetValueFromSysSettingByName("Site");
            if (valueList.Count == 0)
            { 
                throw new FisException("PAK095", new string[] { "Site" });
            }
            if (valueList[0] == "ICC")//重慶
            {
                string shipMode = this.CQSideCodeMBShipMode.ToString();
                ConstValueInfo condition = new ConstValueInfo() {
                     type="SiteCodeOfDCode",
                     name =shipMode
                };

                IList<ConstValueInfo> siteCodeList = partRepository.GetConstValueInfoList(condition);
               
                if (siteCodeList == null || siteCodeList.Count == 0)
                {
                    throw new FisException("PAK095", new string[] { "SiteCodeOfDCode/" + shipMode });
                }
                siteCode = siteCodeList[0].value.Trim();
            }
            else
            {
                IList<string> partLst = partRepository.GetValueFromSysSettingByName("SITECODE");
                if (partLst.Count() > 0)
                {
                    siteCode = partLst[0].ToString();
                }
            }

            IWarrantyRepository wr =
                    RepositoryFactory.GetInstance().GetRepository<IWarrantyRepository, Warranty>();
            IList<Warranty> warrantys = wr.GetDCodeRuleListForMB("Customer");

            var wrList = (from warranty in warrantys
                         where (warranty.Id.ToString() == dataCode)
                       select new { WarrantyFormat = warranty.WarrantyFormat.ToString(), WarrantyCode = warranty.WarrantyCode.ToString() }).ToArray();
            if (wrList.Count() > 0)
            {
                if (wrList[0].WarrantyFormat.ToUpper() == "YYM")
                {
                    year = dt.Year.ToString().Substring(dt.Year.ToString().Length - 2, 2);
                    switch (dt.Month.ToString())
                    {
                        case "10":
                            month = "A";
                            break;
                        case "11":
                            month = "B";
                            break;
                        case "12":
                            month = "C";
                            break;
                        default:
                            month = dt.Month.ToString().Substring(dt.Month.ToString().Length - 1, 1);
                            break;
                    }
                }
                else if (wrList[0].WarrantyFormat.ToUpper() == "YMM")
                {
                    year = dt.Year.ToString().Substring(dt.Year.ToString().Length - 1, 1);
                    //month = dt.Month.ToString().Substring(dt.Month.ToString().Length - 2, 2);
                    if (dt.Month.ToString().Length == 2)
                    {
                        month = dt.Month.ToString();
                    }
                    else
                    {
                        month = "0" + dt.Month.ToString();
                    }
                }
            }

            var dCode = siteCode + year + month + wrList[0].WarrantyCode;
            CurrentSession.AddValue(Session.SessionKeys.DCode, dCode);

            return base.DoExecute(executionContext);
        }
    }
}
