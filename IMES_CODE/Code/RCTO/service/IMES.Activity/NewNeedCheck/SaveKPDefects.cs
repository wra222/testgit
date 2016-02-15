/*
 * INVENTEC corporation ©2012 all rights reserved. 
 * Description:Save defects for KP Defect Input Page
 *             
 * UI:CI-MES12-SPEC-SA-UI KeyParts Defect Input.docx
 * UC:CI-MES12-SPEC-SA-UC KeyParts Defect Input.docx
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2012-06-20  itc202017             (Reference Ebook SourceCode) Create
 * Known issues:
*/

using System.Collections.Generic;
using IMES.DataModel;
using System.Workflow.ComponentModel;
using IMES.FisObject.Common.Defect;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.Activity
{

    /// <summary>
    /// 保存Defect信息
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      应用于KP Defect Input
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
    public partial class SaveKPDefects : BaseActivity
    {

        /// <summary>
        /// 保存Defect信息
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            IList<string> defectList = CurrentSession.GetValue(Session.SessionKeys.DefectList) as IList<string>;
            IDefectRepository defectRepository = RepositoryFactory.GetInstance().GetRepository<IDefectRepository, Defect>();
                     
            foreach (string ele in defectList)
            {
                IqcCause1Info info = new IqcCause1Info();
                info.ctLabel = this.Key;
                info.mpDefect = ele;
                if (defectRepository.CheckIfIqcCauseExist(info))
                {
                    defectRepository.UpdateUDTofIqcCauseDefered(CurrentSession.UnitOfWork, new IqcCause1Info(), info);
                }
                else
                {
                    defectRepository.AddIqcCauseDefered(CurrentSession.UnitOfWork, info);
                }
            }

            return base.DoExecute(executionContext);
        }

    }
}
