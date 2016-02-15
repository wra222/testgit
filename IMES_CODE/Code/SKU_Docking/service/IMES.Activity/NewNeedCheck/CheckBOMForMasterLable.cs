/*
 * INVENTEC corporation ?2011 all rights reserved. 
 * Description:Activity for Asset Tag Label Print Page
 * UI:CI-MES12-SPEC-PAK-UI Asset Tag Label Print.docx –2012/2/28 
 * UC:CI-MES12-SPEC-PAK-UC Asset Tag Label Print.docx –2012/2/28            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
* 2011-10-10   Jessica Liu           (Reference Ebook SourceCode) Create
* Known issues:
* TODO：

*/

using System;
using System.Workflow.ComponentModel;
using IMES.DataModel;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.Generic;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.NumControl;
using IMES.FisObject.FA.LCM;
using IMES.FisObject.Common.Model;
using IMES.FisObject.Common.Part;

namespace IMES.Activity
{
    /// <summary>
    /// 根据BOM查询结果，对Special_Det表进行相应修改
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///         应用于
    ///         Asset Tag Label Print
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         查询BOM
    ///         满足条件则进行Special_Det表的判断修改
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.Product
    ///         AssetSN
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
    ///         Modify Special_Det
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         IBOMRepository
    ///         IHierarchicalBOM
    ///         IBOMNode
    ///         IProductRepository
    /// </para> 
    /// </remarks>
    public partial class CheckBOMForMasterLable : BaseActivity
	{
        ///<summary>
        ///</summary>
        public CheckBOMForMasterLable()
		{
			InitializeComponent();
		}

        /// <summary>
        /// 根据BOM查询结果，对Special_Det表进行相应修改
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            var currenProduct = (IProduct)CurrentSession.GetValue(Session.SessionKeys.Product); 
            var productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            IModelRepository modelRep = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();

            List<string> BLUETOOTH = new List<string>();
            List<string> MDM = new List<string>();
            bool isBlue = false;
           // bool isMdm = false;
            Model CurrentModle = modelRep.Find(currenProduct.Model);
            string family = CurrentModle.Family.FamilyName;

            IBOMRepository bomRep = RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();
            ILCMRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<ILCMRepository>();
            IHierarchicalBOM curBom = bomRep.GetHierarchicalBOMByModel(currenProduct.Model);
            IList<IBOMNode> bomList = curBom.FirstLevelNodes;
            foreach (IBOMNode bomNode in bomList)
            {
                IList<IBOMNode> chileNodeList = bomNode.Children;
                foreach (IBOMNode bomNodeChild in chileNodeList)
                {
                    if (bomNodeChild.Part.BOMNodeType == "KP")
                    {
                        if (bomNodeChild.Part.Descr.Contains("BLUETOOTH"))
                        {
                            BLUETOOTH.Add(bomNodeChild.Part.PN);
                        }
                        if (bomNodeChild.Part.Descr.Contains("MDM"))
                        {
                            MDM.Add(bomNodeChild.Part.PN);
                        }
                    }
                }
            }
            if (BLUETOOTH.Count > 0)
            {
                IList<MasterLabelInfo> tmp = productRepository.GetMasterLabelByVCAndCode(BLUETOOTH.ToArray(), family);
                if (tmp.Count > 0 && tmp[0].code != "" && tmp[0].code != null)
                    isBlue = true;
            }
            else
            {
                isBlue = true;
            }

            if (isBlue == false)
            {
                FisException ex = new FisException("CHK896", new string[] { }); //"Machine has been in stack or out!"
                throw ex;
            }

            isBlue = false;
            if (MDM.Count > 0)
            {
                IList<MasterLabelInfo> tmp = productRepository.GetMasterLabelByVCAndCode(MDM.ToArray(), family);
                if (tmp.Count > 0 && tmp[0].code != "" && tmp[0].code != null)
                    isBlue = true;
            }
            else
            {
                isBlue = true;
            }

            if (isBlue == false)
            {
                FisException ex = new FisException("CHK897", new string[] { }); //"Machine has been in stack or out!"
                throw ex;
            }

            ProductPart WirelessPart = new ProductPart();
            WirelessPart.PartType = "WIRELESS"; //chk898
            WirelessPart.ProductID = currenProduct.ProId;
            List<string> Wireless = (List<string>)productRepository.GetPartSnPrefixListFromProductPart(WirelessPart);
            
            isBlue = false;
            if (Wireless.Count > 0)
            {
                IList<MasterLabelInfo> tmp = productRepository.GetMasterLabelByVCAndCode(Wireless.ToArray(), family);
                if (tmp.Count > 0 && tmp[0].code != "" && tmp[0].code != null) 
                      isBlue = true;
            }
            else
            {
                isBlue = true;
            }

            if (isBlue == false)
            {
                FisException ex = new FisException("CHK898", new string[] { }); //"Machine has been in stack or out!"
                throw ex;
            }


            ProductPart WWANPart = new ProductPart();
            WWANPart.PartType = "WWAN"; //chk898
            WWANPart.ProductID = currenProduct.ProId;
            List<string> WWAN = (List<string>)productRepository.GetPartSnPrefixListFromProductPart(WWANPart);
            isBlue = false;
            if (WWAN.Count > 0)
            {
                IList<MasterLabelInfo> tmp = productRepository.GetMasterLabelByVCAndCode(WWAN.ToArray(), family);
                if (tmp.Count > 0 && tmp[0].code != "" && tmp[0].code != null)
                    isBlue = true;
            }
            else
            {
                isBlue = true;
            }

            if (isBlue == false)
            {
                FisException ex = new FisException("CHK899", new string[] { }); //"Machine has been in stack or out!"
                throw ex;
            }

            return base.DoExecute(executionContext);
        }
	}
}
