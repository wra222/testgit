// INVENTEC corporation (c)2012 all rights reserved. 
// Description: 根据Session中保存的MB生成子板序号
//                    
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2012-06-12   Yuan XiaoWei                 create
// 2012-06-12   Yuan XiaoWei                 ITC-1414-0033
// Known issues:
using System.Collections.Generic;
using System.Workflow.ComponentModel;
using IMES.FisObject.PCA.MB;
using IMES.Infrastructure;
using IMES.Common;

namespace IMES.Activity
{
    /// <summary>
    /// 根据Session中保存的MB生成子板序号
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      应用于ICT INPUT,PrintComMb
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.获取母版MB序号
    ///         2.获取产生子板数量;
    ///         3.产生子板序号(母版序号和子板序号仅倒数第5位不同。母版倒数第5位为M，子板第六位为1-9);
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.MB
    ///         Session.Qty
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         无
    /// </para> 
    ///<para> 
    /// 输出：
    ///         Session.MBNOList
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///         无
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         IMBRepository
    /// </para> 
    /// </remarks>
    public partial class AcquireChildMBSn : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public AcquireChildMBSn()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 根据母版号码和数量生成子板号码
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            Session session = CurrentSession;
            ActivityCommonImpl utl = ActivityCommonImpl.Instance;

            int qty = (int)session.GetValue(Session.SessionKeys.MultiQty);
            MB currentMB = (MB)session.GetValue(Session.SessionKeys.MB);
            string PreMBSn = currentMB.Sn.Substring(0, 5);
            string endMBSn = currentMB.Sn.Substring(6, 4);

            if(currentMB.Sn.Length ==11){
                PreMBSn = currentMB.Sn.Substring(0, 6);
                endMBSn = currentMB.Sn.Substring(7, 4);
            }
            List<string> childMBSnList = new List<string>();
            for (int i = 1; i <= qty; i++)
            {
                string childMBSn = PreMBSn + utl.ChildMBChar[i] + endMBSn;
                childMBSnList.Add(childMBSn);
            }
            session.AddValue(Session.SessionKeys.MBNOList, childMBSnList);
            session.AddValue(Session.SessionKeys.PrintLogBegNo, childMBSnList[0]);
            session.AddValue(Session.SessionKeys.PrintLogEndNo, childMBSnList[qty - 1]);
            return base.DoExecute(executionContext);
        }


    }
}
