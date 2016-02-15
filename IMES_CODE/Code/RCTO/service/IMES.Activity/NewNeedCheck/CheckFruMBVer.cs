// INVENTEC corporation (c)2009 all rights reserved. 
// Description: CI-MES12-SPEC-FA-UC PCA Repair Test&Output.docx
//              Check MB Pass    
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2012-01-10   zhu lei                      create
// Known issues:
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
using IMES.FisObject.PCA.MB;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PCA.EcrVersion;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.Generic;
using IMES.FisObject.Common.Part;
using IMES.DataModel;
using IMES.FisObject.Common.Process;

namespace IMES.Activity
{
    /// <summary>
    /// CheckFruMBVer
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///         应用于需要站
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
    ///         Session.SessionKeys.MB 
    ///         
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
    ///        Product
    /// </para> 
    /// </remarks>
    public partial class CheckFruMBVer : BaseActivity
	{
        /// <summary>
        /// CheckRCTOMBECR
        /// </summary>
        public CheckFruMBVer()
		{
			InitializeComponent();
		}

        /// <summary>
        /// CheckRCTOMBECR
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {


            string checkRCTOMBFRUMB = (string)CurrentSession.GetValue(Session.SessionKeys.CN);
            string FRUNO = "FRUNO";
            MB currentMB = (MB)CurrentSession.GetValue(Session.SessionKeys.MB);
            string mbSNo = this.Key;
            IPartRepository iPartRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository>();
            IEcrVersionRepository iEcrVersionRepository = RepositoryFactory.GetInstance().GetRepository<IEcrVersionRepository>();
            IMBRepository currentMBRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository>();

            #region Check this Station
           

            if (checkRCTOMBFRUMB == "FRU")
            {
                IList<MBLog> mbLogListtemp = currentMBRepository.GetMBLog(mbSNo, CheckStation, 1);
                IList<MBLog> mbLogList = (from q in mbLogListtemp
                                          orderby q.Cdt descending
                                          select q).ToList<MBLog>();

                if (mbLogList == null || mbLogList.Count == 0)
                {
                    throw new FisException("CHK1068", new string[] { CheckStation });//沒有 1A Pass PCBLog記錄，不可FRU 出貨
                }
                if (mbLogList[0].StationID != CheckStation && mbLogList[0].Status != 1)
                {
                    throw new FisException("CHK1068", new string[] { CheckStation });//沒有 1A Pass PCBLog記錄，不可FRU 出貨
                }
            }

            //if (checkRCTOMBFRUMB == "FRU" && currentMB.MBStatus.Station.ToString() != CheckStation)
            //{
            //    //FRU 出貨站點必須為%1,當前站為 %2
            //    throw new FisException("CHK1064", new string[] { CheckStation, currentMB.MBStatus.Station.ToString() });
            //}

            
            #endregion
            #region Check ECR VERsion
            IList<EcrVersion> ecrList = iEcrVersionRepository.GetECRVersionByFamilyMBCodeAndECR(currentMB.Family, currentMB.MBCode, currentMB.ECR);
            if (ecrList == null || ecrList.Count == 0)
            {
                throw new FisException("CHK090", new string[] { });
                //error EcrVersion is not found CHK090    
            }
            #endregion
            #region FruMBVer
            //1.
            if (currentMB.MBStatus.Station != CheckStation)
            {
                return base.DoExecute(executionContext);
            }
            string fruNo = iPartRepository.GetPartInfoValue(currentMB.PCBModelID, FRUNO);
            if (string.IsNullOrEmpty(fruNo))
            {
                throw new FisException("CHK1058", new string[] { currentMB.PCBModelID });
                //此Model:[%1],查無FRUNo
            }
            FruMBVerInfo condition = new FruMBVerInfo();
            condition.partNo = fruNo;
            condition.mbCode = currentMB.MBCode;
            IList<FruMBVerInfo> fruInfoList = iEcrVersionRepository.GetFruMBVer(condition);
            if (fruInfoList == null || fruInfoList.Count == 0)
            {
                throw new FisException("CHK1059", new string[] { currentMB.MBCode });
                //此MB:[%1],查無 FRU MB Ver
            }
            
            string verFirstCode = ecrList[0].IECVer.Substring(0, 1);
            string verEndCode = ecrList[0].IECVer.Substring(ecrList[0].IECVer.Length - 2, 2);
            IList<FruMBVerInfo> checkFRUMBVerList = (from q in fruInfoList
                                                     where q.ver.StartsWith(verFirstCode)
                                                     orderby q.ver descending
                                                     select q).ToList<FruMBVerInfo>();
            if (checkFRUMBVerList == null || checkFRUMBVerList.Count == 0)
            {
                throw new FisException("CHK1065", new string[] { verFirstCode });
                //查無FRU MB Ver,輸入VER版本為 %1 
            }

            if (string.Compare(verEndCode, checkFRUMBVerList[0].ver.Substring(checkFRUMBVerList[0].ver.Length - 2, 2)) < 0)
            {
                throw new FisException("CHK1060", new string[] { verEndCode, checkFRUMBVerList[0].ver.Substring(checkFRUMBVerList[0].ver.Length - 2, 2) });
                //Expired Version 此MB版本低於可控版本,輸入版本為 %1 ,設定版本為 %2
            }
            #endregion
            return base.DoExecute(executionContext);
        }

        /// <summary>
        ///  CheckStation
        /// </summary>
        public static DependencyProperty CheckStationProperty = DependencyProperty.Register("CheckStation", typeof(string), typeof(CheckFruMBVer));

        /// <summary>
        /// AttributeName
        /// </summary>
        [DescriptionAttribute("CheckStationProperty")]
        [CategoryAttribute("InArguments of CheckFruMBVer")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        [DefaultValue("")]
        public string CheckStation
        {
            get
            {
                return ((string)(base.GetValue(CheckStationProperty)));
            }
            set
            {
                base.SetValue(CheckStationProperty, value);
            }
        }
	
	}
}
