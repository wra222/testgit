/*
 * INVENTEC corporation ?2011 all rights reserved. 
 * Description:Activity for PA Cosmetic Page
 * UI:CI-MES12-SPEC-PAK-UI Packing Pizza.docx –2011/11/07
 * UC:CI-MES12-SPEC-PAK-UC Packing Pizza.docx –2011/11/07            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
* 2011-11-07   zhu lei               Create
* Known issues:
* TODO：
*/

using System;
using System.Linq;
using System.Workflow.ComponentModel;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.Generic;
using IMES.FisObject.Common.Model;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;

namespace IMES.Activity
{
    /// <summary>
    /// 检查是否需要进行CheckItem
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///         应用于PA Cosmetic
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
    ///         Model 
    ///         
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
    /// 相关FisObject:
    ///         IModelRepository
    ///         IModel
    /// </para> 
    /// </remarks>
    public partial class CheckItem : BaseActivity
	{
        /// <summary>
        /// 构造函数
        /// </summary>
        public CheckItem()
		{
			InitializeComponent();
		}


        /// <summary>
        /// 检查是否需要进行CheckItem
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            string checkItem = string.Empty;
            string checkPCID = "PCID";
            string checkWWAN = "WWAN";
            string partNo = "60WIMAX00001";
            var prod = (IProduct)CurrentSession.GetValue(Session.SessionKeys.Product);
            string model = prod.Model;
            var bomRepository = RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();
            IModelRepository myRepository = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();
            Model modelObj = myRepository.Find(model);
            CurrentSession.AddValue(Session.SessionKeys.ModelObj, modelObj);
            CurrentSession.AddValue(Session.SessionKeys.ModelName, modelObj.ModelName);
            IHierarchicalBOM myBom = bomRepository.GetHierarchicalBOMByModel(model);
            IList<IBOMNode> firstLevelNode = myBom.FirstLevelNodes;

            var pcid = (string)modelObj.GetAttribute("PCID");
            FisException continuableException = null;

            foreach (var fnode in firstLevelNode)
            {
                if (fnode.Part.BOMNodeType.ToUpper() == "AV")
                {
                    if (fnode.Part.GetAttribute("Special_AV") == "Security_Screw")
                    {
                        continuableException = new FisException("PAK100", new string[] { "" });
                    }
                }
            }

            var prod_pcid = prod.ProductInfoes.SingleOrDefault(pi => pi.InfoType == "PCID");
            if (pcid != null && pcid != string.Empty && prod_pcid != null && prod_pcid.InfoValue != null && prod_pcid.InfoValue.Trim() != string.Empty)
            {
                checkItem = checkPCID;
            }
            else
            {
                //IList<IPart> parts = new List<IPart>();
                bool checkFlag1 = false;
                bool checkFlag2 = false;
                foreach (var f in firstLevelNode)
                {
                    if (f.Part.PN == partNo)
                    {
                        checkItem = checkWWAN;
                        break;
                    }
                    else
                    {
                        //当ModelBOM 中Model 的直接下阶存在BomNodeType = 'BM' (IMES_GetData..Part.BomNodeType)，Descr 属性(IMES_GetData..Part.Descr)为'WWAN'的Part 
                        if (f.Part.BOMNodeType.ToUpper() == "BM" && f.Part.Descr.ToUpper() == "WWAN")
                        {
                            checkFlag1 = true;
                        }
                        //并且在ModelBOM 中Model 的直接下阶中存在BomNodeType = 'PL', UPPER(Descr) LIKE 'WWAN%'的Part 时，需要进行WWAN Check
                        //2012.04.21  Liu Dong asked by Y.B.Gao
                        if (f.Part.BOMNodeType.ToUpper() == "PL" && f.Part.Descr.ToUpper().StartsWith("WWAN"))//(f.Part.Descr.ToUpper() == "WWANID LABEL 1" || f.Part.Descr.ToUpper() == "WWAN LABEL")
                        //2012.04.21  Liu Dong asked by Y.B.Gao
                        {
                            checkFlag2 = true;
                        }
                        if (checkFlag1 && checkFlag2)
                        {
                            checkItem = checkWWAN;
                            break;
                        }
                    }
                }
            }

            // append message to the end of check item to avoid change of interface
            if (continuableException != null)
                checkItem += "~" + continuableException.mErrmsg;

            CurrentSession.AddValue(Session.SessionKeys.C, checkItem);

            return base.DoExecute(executionContext);
        }
	
	}
}
