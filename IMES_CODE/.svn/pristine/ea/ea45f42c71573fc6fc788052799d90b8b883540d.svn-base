// INVENTEC corporation (c)2011 all rights reserved. 
// Description: CI-MES12-SPEC-FA-UC IEC Label Print.docx
//              Get IEC Label    
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-12-08   zhu lei                      create
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
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.Generic;

namespace IMES.Activity
{
    /// <summary>
    /// Get IEC Label
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
    ///        MB
    /// </para> 
    /// </remarks>
    public partial class GetIECLabel : BaseActivity
	{
        /// <summary>
        /// GetIECLabel
        /// </summary>
        public GetIECLabel()
		{
			InitializeComponent();
		}

        //ITC-1103-0050
        /// <summary>
        /// Get IEC Label
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            //string type = "MB";
            string valueP1 = string.Empty;
            string partNoP1 = string.Empty;
            string valueAS = string.Empty;
            IList printItems = new ArrayList();
            IList vendorCTLst = new ArrayList();

            string partNoKP = (string)CurrentSession.GetValue(Session.SessionKeys.PartNo);
            string dCode = (string)CurrentSession.GetValue(Session.SessionKeys.DCode);
            string rev = (string)CurrentSession.GetValue(Session.SessionKeys.IECVersion);
            vendorCTLst = (IList)CurrentSession.GetValue(Session.SessionKeys.VCodeInfoLst);
            int count = vendorCTLst.Count - 1;
            //string mbSNo = this.Key;

            IPartRepository partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            IBOMRepository bomRepository = RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();
            IList<string> partNoLst = bomRepository.GetMaterialByComponent(partNoKP);

            foreach (var partNo in partNoLst)
            {
                IPart partP1s = partRepository.Find(partNo);
                if (partP1s.BOMNodeType == "P1")
                {
                    valueP1 = (string)partP1s.GetAttribute("DESC");
                    if (valueP1 != "")
                    {
                        partNoP1 = partNo;
                        break;
                    }
                }
            }

            IPart parts = partRepository.Find(partNoKP);
            valueAS = (string)parts.GetAttribute("AS");

            printItems.Add(string.Format(DateTime.Now.ToString(), "YYYY/MM/DD"));
            printItems.Add(partNoP1);
            printItems.Add(valueP1);
            printItems.Add(valueAS);
            printItems.Add(dCode);
            printItems.Add(dCode.Substring(0, 1));
            printItems.Add(dCode.Substring(1,4));
            printItems.Add(rev);
            printItems.Add(vendorCTLst[count]);
            printItems.Add(vendorCTLst[0].ToString().Substring(0, 1));
            printItems.Add(vendorCTLst[0]);
            CurrentSession.AddValue(Session.SessionKeys.PrintItems, printItems);

            string desc = dCode + "," + rev;
            CurrentSession.AddValue(Session.SessionKeys.PrintLogName, "KP");
            CurrentSession.AddValue(Session.SessionKeys.PrintLogBegNo, vendorCTLst[0]);
            CurrentSession.AddValue(Session.SessionKeys.PrintLogEndNo, vendorCTLst[count]);
            CurrentSession.AddValue(Session.SessionKeys.PrintLogDescr, desc);

            return base.DoExecute(executionContext);
        }
	
	}
}
