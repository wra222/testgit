// INVENTEC corporation (c)2010 all rights reserved. 
// Description: 检查输入CT SN的正确性
//                    
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2010-01-06   Yuan XiaoWei                 create
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
using IMES.FisObject.Common.PartSn;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure;
using IMES.FisObject.Common.Part;
using IMES.FisObject.Common.BOM;
using System.Collections.Generic;

namespace IMES.Activity
{
    /// <summary>
    /// 检查输入CT SN的正确性
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///         应用于Combine KPCT
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.参考UC;
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.业务异常：CHK028
    ///                     CHK029
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.PartSN
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
    ///         无
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         无
    /// </para> 
    /// </remarks>
    public partial class CheckPartSN : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CheckPartSN()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 检查输入CT SN的正确性,KPType是否相同，VendorSN是否已结合
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {

            PartSn currentPartSN = (PartSn)CurrentSession.GetValue(Session.SessionKeys.PartSN);

            string currentKPType = (string)CurrentSession.GetValue(Session.SessionKeys.KPType);

            //1.在PartSN表里根据 CT找到对应的记录，得到Part Type，然后比对PartType=KP Type
            if (string.Compare(currentPartSN.Type, currentKPType, false) != 0)
            {
                var ex = new FisException("CHK029", new string[] { Key });
                throw ex;
            }

            //2.判斷此是否CT已与其它vendor SN结合，不能再结合.
            if (!string.IsNullOrEmpty(currentPartSN.VendorSn))
            {
                var ex = new FisException("CHK028", new string[] { Key });
                throw ex;
            }


            /*
                以下是TSB特有业务需求：
                若(KP Type=LCM，且Vendor=LG)或family前7码=POTOMAC，则提示user以下信息：” 需贴rubber.”
            */

            if (Customer == "TSB" && currentKPType == "LCM")
            {
                var currentPartRepository = IMES.Infrastructure.FisObjectRepositoryFramework.RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
                var currentPart = currentPartRepository.Find(currentPartSN.IecPn);
                if (currentPart == null)
                {
                    List<string> errpara = new List<string>();
                    errpara.Add(currentPartSN.IecPn);
                    throw new FisException("CHK027", errpara);
                }
                else if (currentPart.GetAttribute("Vendor", currentPartSN.IecSn.Substring(0, 6)) == "LG")
                {
                    var currentBomRepository = IMES.Infrastructure.FisObjectRepositoryFramework.RepositoryFactory.GetInstance().GetRepository<IBOMRepository, BOM>();
                    var currentFamily = currentBomRepository.GetFirstFamilyViaMoBOM(currentPartSN.IecPn);

                    if (currentFamily != null && currentFamily.FamilyName.StartsWith("POTOMAC"))
                    {
                        var ex = new FisException("CHK036", new string[] { Key });
                        ex.stopWF = false;
                        throw ex;
                    }
                }

            }

            return base.DoExecute(executionContext);
        }
    }
}
