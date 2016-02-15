using System.Workflow.ComponentModel;
using System.Linq;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.Model;
using IMES.FisObject.Common.FisBOM;
using System.Collections.Generic;
using IMES.FisObject.Common.Part;

namespace IMES.Activity
{
    /// <summary>
    /// 若是ClearRoom 機型才可以開CRXXX 開頭的工單
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      ClearRoom
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         若是ClearRoom 機型才可以開CRXXX 開頭的工單
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
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
    ///             
    /// </para> 
    /// </remarks>
    public partial class CheckClearRoomModel : BaseActivity
	{
        /// <summary>
        /// constructor
        /// </summary>
        public CheckClearRoomModel()
		{
			InitializeComponent();
		}

        /// <summary>
        /// Check ClearRoom
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            //Product currentProduct = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);
            string model = (string)CurrentSession.GetValue(Session.SessionKeys.ModelName);

            //for mantis449: 无尘室touchsmart机型判定逻辑变更。
            //IModelRepository modelRepository = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();
            //Model mdl = modelRepository.Find(model);
            //string mn = (string)mdl.GetAttribute("MN1");
            //if (string.IsNullOrEmpty(mn) || mn.ToUpper().IndexOf("TOUCHSMART") < 0)
            //{
            //    throw new FisException("CQCHK1005", new string[] { model });
            //}
            if (string.IsNullOrEmpty(model))
            {
                throw new FisException("CQCHK0006", new string[] { Session.SessionKeys.ModelName });
            }

            IBOMRepository bomRep = RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();
            IHierarchicalBOM curBOM = bomRep.GetHierarchicalBOMByModel(model);
            IList<IBOMNode> bomNodeLst = curBOM.FirstLevelNodes;

            if (bomNodeLst != null && bomNodeLst.Count > 0)
            {
                var PartPLList = (from p in bomNodeLst
                                  where p.Part.BOMNodeType.ToUpper() == "PL" && 
                                            p.Part.Attributes != null && 
                                            p.Part.Attributes.Count>0
                                  select p.Part).ToList();
                if (PartPLList != null && PartPLList.Count > 0)
                {
                    bool bExistTouch = false;
                    foreach (IPart part in PartPLList)
                    {
                        var attrList = (from p in part.Attributes
                                        where !string.IsNullOrEmpty(p.InfoType) &&
                                                  p.InfoType == "TYPE" &&
                                                  !string.IsNullOrEmpty(p.InfoValue) && 
                                                  p.InfoValue.ToUpper().StartsWith("TOUCH")
                                        select p).ToList();
                        if (attrList != null && attrList.Count > 0)
                        {
                            bExistTouch = true;
                            break;
                        }
                    }
                    if (!bExistTouch)
                    {
                        throw new FisException("CQCHK1005", new string[] { model });
                    }
                }
                else
                {
                    throw new FisException("CQCHK1005", new string[] { model });
                }
            }

	        return base.DoExecute(executionContext);
        }
	}
}
