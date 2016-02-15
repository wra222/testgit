/*
* INVENTEC corporation ?2011 all rights reserved. 
* Description:Activity for Combine AST Page
* UI:CI-MES12-SPEC-FA-UI Combine AST .docx –2011/12/5 
* UC:CI-MES12-SPEC-FA-UC Combine AST .docx –2011/12/5            
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2011-12-2   Jessica Liu           (Reference Ebook SourceCode) Create
* Known issues:
* TODO：
* UC 具体业务：检查是否是 Shell（CDSI） 机器(CI-MES12-SPEC-000-UC Common Rule.docx  2.10判断一个机器是否是CDSI机器（Shell）,加密机型)-UC变更，等待数据接口（in Activity：CombineAndPrintAST，SaveAST）
* UC 具体业务：删除AST标签-等待数据接口（in Activity：DeleteAST） 
* UC 具体业务： select @ast2=Sno from CDSIAST nolock where Tp ='ASSET_TAG2' and SnoId=@prdId；select @ast1=Sno from CDSIAST nolock where Tp ='ASSET_TAG' and SnoId=@prdId-等待数据接口（in Activity：CombineAndPrintAST）
* UC 具体业务：保存product和Asset SN的绑定关系-- Insert Product_Part values(@prdid,@partpn,@astsn,’’,’AT’,@user,getdate(),getdate())注：@partpn 为PartNo in (bom中BomNodeType=’AT’  Descr=’ATSN1’ 对应的Pn)-UC变更，等待数据接口（in Activity：SaveAST）
*/

using System;
using System.Workflow.ComponentModel;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.Generic;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;
using System.Linq;  

namespace IMES.Activity
{
    /// <summary>
    /// 保存AST
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      Combine AST
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.如果不是CDSI 机器，保存product和Asset SN的绑定关系
    ///         2.如果是CDSI机器，则不用插入
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.SessionKeys.Product
    ///         AST
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
    ///              ？？？
    /// </para> 
    /// </remarks>
    public partial class SaveAST : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SaveAST()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 保存AST
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            var logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
           

            var currenProduct = (IProduct)CurrentSession.GetValue(Session.SessionKeys.Product);
            var currentAST = (string)CurrentSession.GetValue("AST");
            //2012-9-4, Jessica Liu, UC需求变更
            var currentVendor = (string)CurrentSession.GetValue("Vendor");

            bool isCDSI = currenProduct.IsCDSI;
            
            if (isCDSI == false)     
            {
                /* 2012-9-4, Jessica Liu, UC需求变更
                IBOMRepository bomRep = RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();
                IHierarchicalBOM bom = bomRep.GetHierarchicalBOMByModel(currenProduct.Model);
                for (int i = 0; i < bom.FirstLevelNodes.Count; i++)
                {
                    IPart part = ((BOMNode)bom.FirstLevelNodes.ElementAt(i)).Part;
                    if ((part.BOMNodeType == "AT") && (part.Descr == "ATSN1"))
                    {
                        IProductPart assetTag1 = new ProductPart();
                        assetTag1.BomNodeType = "AT";
                        assetTag1.Iecpn = string.Empty; //part.PN;
                        assetTag1.CustomerPn = string.Empty; //part.CustPn;
                        assetTag1.ProductID = currenProduct.ProId;
                        assetTag1.PartID = part.PN;
                        assetTag1.PartSn = currentAST;
                        assetTag1.PartType = "ATSN1"; //"AT";
                        assetTag1.Station = Station;
                        assetTag1.Editor = Editor;
                        assetTag1.Cdt = DateTime.Now;
                        assetTag1.Udt = DateTime.Now;
                        currenProduct.AddPart(assetTag1);
                    }
                }
                */
                IProductPart assetTag1 = new ProductPart();
                assetTag1.BomNodeType = "AT";
                assetTag1.Iecpn = string.Empty; //part.PN;
                assetTag1.CustomerPn = string.Empty; //part.CustPn;
                assetTag1.ProductID = currenProduct.ProId;
                assetTag1.PartID = currentVendor;
                assetTag1.PartSn = currentAST;
                assetTag1.PartType = "ATSN1"; //"AT";
                assetTag1.Station = Station;
                assetTag1.Editor = Editor;
                assetTag1.Cdt = DateTime.Now;
                assetTag1.Udt = DateTime.Now;
                currenProduct.AddPart(assetTag1);
            }
           
            return base.DoExecute(executionContext);
        }
    }
}
