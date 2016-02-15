// INVENTEC corporation (c)2012 all rights reserved. 
// Description: DealWithECRLabel
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2012-02-27   Yuan XiaoWei                 create
// 2012-02-27   Yuan XiaoWei                 ITC-1360-0802 New Request
// Known issues:

using System;
using System.Collections;
using System.Collections.Generic;
using System.Workflow.ComponentModel;
using IMES.DataModel;
using IMES.FisObject.Common.Model;
using IMES.FisObject.Common.NumControl;
using IMES.FisObject.Common.Part;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PCA.MB;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.UnitOfWork;
using IMES.Infrastructure.Utility.Generates.impl;
using IMES.Infrastructure.Utility.Generates.intf;

namespace IMES.Activity
{
    /// <summary>
    /// 获取PrintItems，根据条件筛除不符合条件的LabelType
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///         应用于ICT Input
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.获取PrintItems，根据条件筛除不符合条件的LabelType
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         SessionKeys.PrintItems
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         无
    /// </para> 
    ///<para> 
    /// 输出：
    ///         Session.PrintItems
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///         更新Session.SessionKeys.PrintItems
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         
    /// </para> 
    /// </remarks>
    public partial class DealWithECRLabel : BaseActivity
    {
        /// <summary>
        /// 
        /// </summary>
        public DealWithECRLabel()
        {
            InitializeComponent();
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            IMB currenMB = CurrentSession.GetValue(Session.SessionKeys.MB) as MB;

            IPartRepository CurrentPartRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            string mbctValue = CurrentPartRepository.GetPartInfoValue(currenMB.PCBModelID, "MBCT");
            DealWithLabelByMBCT(mbctValue, currenMB.Family);

            return base.DoExecute(executionContext);
        }

        private void DealWithLabelByMBCT(string mbctValue, string family)
        {
            IList<PrintItem> printItemList = CurrentSession.GetValue(Session.SessionKeys.PrintItems) as IList<PrintItem>;
            IList<int> DeleteIndexList = new List<int>();
            if (printItemList != null)
            {
                if (mbctValue == "T")
                {
                    if (family != null && family.ToUpper().Contains("PIXIES"))
                    {
                        for (int i = 0; i < printItemList.Count; i++)
                        {
                            if (printItemList[i].LabelType == "ECR Label-1" || printItemList[i].LabelType == "ECR Label-2")
                            {
                                DeleteIndexList.Add(i);
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < printItemList.Count; i++)
                        {
                            if (printItemList[i].LabelType == "ECR Label-2" || printItemList[i].LabelType == "ECR Label-3")
                            {
                                DeleteIndexList.Add(i);
                            }
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < printItemList.Count; i++)
                    {
                        if (printItemList[i].LabelType == "ECR Label-1" || printItemList[i].LabelType == "ECR Label-3")
                        {
                            DeleteIndexList.Add(i);
                        }
                    }
                }

                for (int i = DeleteIndexList.Count - 1; i >= 0; i--)
                {
                    printItemList.RemoveAt(DeleteIndexList[i]);
                }

                CurrentSession.AddValue(Session.SessionKeys.PrintItems, printItemList);
                return;
            }
        }

    }
}
