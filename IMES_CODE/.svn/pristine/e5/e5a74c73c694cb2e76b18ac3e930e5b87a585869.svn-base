/*
 * INVENTEC corporation ?2011 all rights reserved. 
 * Description:Activity for REV Label Print For Docking Page
 * UI:CI-MES12-SPEC-FA-UI REV Label Print For Docking.docx –2012/5/28 
 * UC:CI-MES12-SPEC-FA-UC REV Label Print For Docking.docx –2012/5/28            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
* 2012-5-30   Jessica Liu           (Reference Ebook SourceCode) Create
* Known issues:
* ITC-1414-0212, Jessica Liu, 2012-6-26
* TODO：
*/

using System;
using System.Workflow.ComponentModel;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using System.Collections.Generic;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;
using IMES.FisObject.Common.Model;


namespace IMES.Activity
{
    /// <summary>
    /// Check Version
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      REV Label Print For Docking
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.生成序号
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.SessionKeys.FamilyName
    ///         Session.SessionKeys.DCode
    ///         Session.SessionKeys.WarrantyCode
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         无
    /// </para> 
    ///<para> 
    /// 输出：
    ///         Session.SessionKeys.PrintLogBegNo
    ///         Session.SessionKeys.PrintLogEndNo
    ///         Session.SessionKeys.PrintLogName
    ///         Session.SessionKeys.PrintLogDescr
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///          
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///              IFamilyRepository
    /// </para> 
    /// </remarks>
    public partial class CheckREVLabelPrint : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CheckREVLabelPrint()
        {
            InitializeComponent();
        }
        
        /// <summary>
        /// 产生Asset SN
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            string family = (string)CurrentSession.GetValue(Session.SessionKeys.FamilyName);
            string dcode = (string)CurrentSession.GetValue(Session.SessionKeys.DCode);
            string datacode = (string)CurrentSession.GetValue(Session.SessionKeys.WarrantyCode);
            int qty = (int)CurrentSession.GetValue(Session.SessionKeys.Qty);
            string descr = "";
            string familyInfo = "";
            IList<IMES.DataModel.FamilyInfoDef> familyInfoList = new List<IMES.DataModel.FamilyInfoDef>(); ;

            IFamilyRepository CurrentRepository = RepositoryFactory.GetInstance().GetRepository<IFamilyRepository, Family>();

            Family tempFamily = CurrentRepository.FindFamily(family);
            List<string> erpara = new List<string>();
            if (tempFamily == null)
            {
                erpara.Add(datacode);
                throw new FisException("CHK310", erpara);   
            }
            
            IMES.DataModel.FamilyInfoDef condition = new IMES.DataModel.FamilyInfoDef();
            //ITC-1414-0212, Jessica Liu, 2012-6-26
            condition.name = "VERSION";     //"Version";
            condition.family = family;
            familyInfoList = CurrentRepository.GetExistFamilyInfo(condition);

            if (familyInfoList == null || familyInfoList.Count == 0)
            {
                erpara.Add(datacode);
                throw new FisException("CHK311", erpara);   
            }
            else
            {
                foreach (IMES.DataModel.FamilyInfoDef tempFamilyInfo in familyInfoList)
                {
                    familyInfo = tempFamilyInfo.value; 
                }
            }
            
            descr = familyInfo + "  " + dcode + "  [" + qty + "]";

            CurrentSession.AddValue(Session.SessionKeys.PrintLogName, "REV Label");
            CurrentSession.AddValue(Session.SessionKeys.PrintLogBegNo, family);
            CurrentSession.AddValue(Session.SessionKeys.PrintLogEndNo, dcode);
            CurrentSession.AddValue(Session.SessionKeys.PrintLogDescr, descr);

            return base.DoExecute(executionContext);
        }
    }
}
