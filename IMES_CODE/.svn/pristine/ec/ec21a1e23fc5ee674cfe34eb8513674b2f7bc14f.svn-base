// INVENTEC corporation (c)2012 all rights reserved. 
// Description:SysSetting.Value 中包含’VGA’ Name=’ChangeModelCheckItem’
//             则获取VGA的MBCode，检查MBCode是否在[Model2]直接下阶MB阶VGA主板的MB属性(PartInfo.InfoValue Condtion InfoType=’MB’)中存在，若不存在，则报错“VGA MBCode不匹配，不能进行Change
//             SysSetting.Value 中包含’MB’ Name=’ChangeModelCheckItem’
//             则获取MB的MBCode，检查MBCode是否在[Model2]的MB阶的MB属性中存在，若不存在，则报错“MBCode不匹配，不能进行Change
// 
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2012-06-18   Kerwin                       create
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
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.Generic;

namespace IMES.Activity
{
    /// <summary>
    /// Check MB Match
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///         CI-MES12-SPEC-FA-UC Change Mode
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
    public partial class MBCodeMatch : BaseActivity
	{
        /// <summary>
        /// CheckMBMatch
        /// </summary>
        public MBCodeMatch()
		{
			InitializeComponent();
		}

        /// <summary>
        /// Check MBCode Match
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            string type = "MB";
            string noEqInfoType = "VGA";
            string noEqInfoValue = "SV";
            string check = (string)CurrentSession.GetValue(Session.SessionKeys.CN);
            string model = (string)CurrentSession.GetValue(Session.SessionKeys.ModelName);
            string mbSNo = this.Key;
            string valueInfo = "";
            bool errFlg = true;

            //Add 2012/06/21
            string strMBCode = "";
            if (mbSNo.Substring(5, 1) == "M" || mbSNo.Substring(5, 1) == "B")
                strMBCode = mbSNo.Substring(0, 3);
            else
                strMBCode = mbSNo.Substring(0, 2);
            if (check == "PC" || check == "RCTO")
            {
                IBOMRepository bomRepository = RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();
                IList<string> partValueLst = bomRepository.GetPartInfoValueListByModelAndBomNodeTypeAndInfoType(model, type, type, noEqInfoType, noEqInfoValue);
                
                if (partValueLst.Count > 0)
                {
                    for (var i = 0; i < partValueLst.Count(); i++)
                    {
                        valueInfo = partValueLst[i].ToString();
                        //if (valueInfo == mbSNo.Substring(0, 2))
                        if (valueInfo == strMBCode)
                        {
                            errFlg = false;
                            break;
                        }
                    }
                }

                if (errFlg)
                {
                    List<string> errpara = new List<string>();
                    errpara.Add(mbSNo);
                    throw new FisException("CHK161", errpara);
                }
            }

            return base.DoExecute(executionContext);
        }
	
	}
}
