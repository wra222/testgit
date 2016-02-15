/*
 * INVENTEC corporation ©2012 all rights reserved. 
 * Description:Add defect for SMT Defect Input Page
 *             (Also can be used in KP defect input page.)
 *             
 * UI:CI-MES12-SPEC-SA-UI SMT Defect Input.docx –2012/05/21
 * UC:CI-MES12-SPEC-SA-UC SMT Defect Input.docx –2012/05/21            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2012-05-21  itc202017             (Reference Ebook SourceCode) Create
 * Known issues:
*/
using System.Collections;
using System.Collections.Generic;
using IMES.DataModel;
using System.Workflow.ComponentModel;
using IMES.FisObject.Common.Defect;
using IMES.Infrastructure;
using IMES.FisObject.PCA.MB;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.PCA.MBModel;
using IMES.Infrastructure.Extend;
using IMES.FisObject.FA.Product;

namespace IMES.Activity
{

    /// <summary>
    /// 新增Defect信息
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      应用于SMT Defect Input/KP Defect Input
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.MB或者Session.Product
    ///         Session.ReapirDefectInfo
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
    ///         
    /// </para> 
    ///<para> 
    /// 
    /// </para> 
    /// </remarks>
    public partial class AddSMTDefect : BaseActivity
    {

        /// <summary>
        /// 新增Defect信息
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            IList<DefectCodeDescr> defectInfoList = CurrentSession.GetValue("DefectInfoList") as IList<DefectCodeDescr>;
            IList<string> defectList = CurrentSession.GetValue(Session.SessionKeys.DefectList) as IList<string>;
            string defect = CurrentSession.GetValue(Session.SessionKeys.DCode) as string;

            IDefectRepository defectRepository = RepositoryFactory.GetInstance().GetRepository<IDefectRepository, Defect>();
            IList<IMES.DataModel.DefectInfo> defList = defectRepository.GetDefectList("PRD");

            bool bFound = false;

            foreach (IMES.DataModel.DefectInfo ele in defList)
            {
                if (defect == ele.id)
                {
                    DefectCodeDescr item = new DefectCodeDescr();
                    item.DefectCode = ele.id;
                    item.Description = ele.description;
                    defectInfoList.Add(item);
                    defectList.Add(defect);
                    bFound = true;
                    break;
                }
            }

            if (bFound)
            {
                CurrentSession.AddValue(Session.SessionKeys.DefectList, defectList);
                CurrentSession.AddValue("DefectInfoList", defectInfoList);
            }
            else
            {
                List<string> errpara = new List<string>();
                errpara.Add(defect);
                FisException e = new FisException("CHK305", errpara);
                e.stopWF = false;
                throw e;
            }

            return base.DoExecute(executionContext);
        }

    }
}
