using System.Workflow.ComponentModel;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.Generic;
using IMES.FisObject.PAK.DN;
using IMES.FisObject.Common.MO;
using IMES.DataModel;
using System.Linq;

namespace IMES.Activity
{
    /// <summary>
    /// Check Product Print Qty And Return
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      CreateProductandCombineLCM
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
    ///         Family
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
    ///         none
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///              
    ///              
    /// </para> 
    /// </remarks>
    public partial class CheckProductPrintQty : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CheckProductPrintQty()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Get QCStatus of Product
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        /// 

        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            var family = (string)CurrentSession.GetValue("Family");
            var mo = (string)CurrentSession.GetValue(Session.SessionKeys.MONO);
            var moObject = (MO)CurrentSession.GetValue(Session.SessionKeys.ProdMO);
            var model = (string)CurrentSession.GetValue(Session.SessionKeys.ModelName);
            var moPrefix = (string)CurrentSession.GetValue("MOPrefix");
            //IMORepository iMORepository = RepositoryFactory.GetInstance().GetRepository<IMORepository>();
            
            //IList<MOInfo> tempList = iMORepository..GetMOListByFamily(family);
            //IList<MOInfo> moList = new List<MOInfo>();
            //moList = (from q in tempList
            //          where q.friendlyName.StartsWith(moPrefix) && q.id == mo && q.model == model
            //          select q).ToList<MOInfo>();
            //if (moList != null && moList.Count > 0)
            //{
            //    var qty = moList[0].qty - moList[0].pqty;
            //    if (qty < 0)
            //    {
            //        new FisException("CQCHK1009", new string[] { });
            //    }

            //    if (qty == 0)
            //    {
            //        new FisException("CQCHK1008", new string[] { });
            //    }
            //}
            //else
            //{
            //    new FisException("CQCHK1008", new string[] { });
            //}
            if (moObject != null)
            {
                var qty = moObject.Qty - moObject.PrtQty;
                if (qty < 0)
                {
                    new FisException("CQCHK1009", new string[] { });
                }

                if (qty == 0)
                {
                    new FisException("CQCHK1008", new string[] { });
                }
            }
            else
            {
                new FisException("CQCHK1008", new string[] { });
            }
            return base.DoExecute(executionContext);
        }
    }
}
