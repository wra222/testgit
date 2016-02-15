// INVENTEC corporation (c)2011 all rights reserved. 
// Description: 根据DeliveryNo号码，UnPack属于DeliveryNo的所有Product
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-11-21                   create
// Known issues:
using System.Workflow.ComponentModel;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.FisBOM;
using System.Collections.Generic;
using IMES.FisObject.Common.Part;
using IMES.FisObject.FA.LCM;
using IMES.DataModel;

namespace IMES.Activity
{
    /// <summary>
    /// 用于UnPack属于DeliveryNo的所有Product
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      CI-MES12-SPEC-PAK-UC Unpack
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.从Session中获取DeliveryNo，调用ProductRepository的Update方法
    ///           update Product set DeliveryNo='',PalletNo='',CartonSN='',CartonWeight=0.0
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.DeliveryNo
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
    ///         更新Product
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///              IProductRepository
    ///              
    /// </para> 
    /// </remarks>
    public partial class CheckBomForPrint : BaseActivity
	{
        /// <summary>
        /// 构造函数
        /// </summary>
        public CheckBomForPrint()
		{
			InitializeComponent();
		}

        /// <summary>
        /// </summary>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            Product curProduct = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);
            IBOMRepository bomRep = RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();
            ILCMRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<ILCMRepository>();
            IHierarchicalBOM curBom = bomRep.GetHierarchicalBOMByModel(curProduct.Model);
            IList<IBOMNode> bomList = curBom.FirstLevelNodes;

            IList<IProductPart> productParts = curProduct.ProductParts;
            FisException ex;
            string isPrint = string.Empty;
            foreach (IBOMNode node in bomList)
            {
                if (node.Part.Descr == "Anatel label" || node.Part.Descr == "ICASA Label2")
                {
                    if (node.Part.BOMNodeType == "PL")
                    {
                        isPrint = "yes";
                        break;
                    }
                }
            }
            if (string.IsNullOrEmpty(isPrint)) { 
            //无需打印
                   List<string> erpara = new List<string>();
                   //erpara.Add("此机器不需要打印Inatel&ICASA Label");
                   ex = new FisException("CHK265", erpara);
                   throw ex;
               
            }
            string partSn = string.Empty;
            if (bomRep.CheckIfExistModelBomWithPart("%WWAN%", "%WIRELESS%", curProduct.Model))
            {             
                foreach (ProductPart part in productParts)
                {
                    string tmp = part.PartSn;
                    //string tmp = part.Station;
                    if (tmp.Length>=14&& tmp[0] == 'G')
                    {
                        partSn = part.PartSn;
                        break;
                    }
                }
                if (string.IsNullOrEmpty(partSn))
                {
                    //无需打印
                    List<string> erpara = new List<string>();
                    //erpara.Add("没有结合相关设备，无需打印");
                    ex = new FisException("CHK266", erpara);
                    throw ex;

                }
            }
            if (!string.IsNullOrEmpty(partSn))
            {
                string partSn5 = partSn.Substring(0, 5);
                if (!string.IsNullOrEmpty(partSn5))
                {
                    if (!itemRepository.CheckExistICASAByVC(partSn5))
                    {
                        List<string> erpara = new List<string>();
                        // erpara.Add("存在未Maintain的CT，請IE Maintain");
                        ex = new FisException("CHK267", erpara);
                        throw ex; ;
                    }

                    // mantis 1569
                    ICASADef iCASA = itemRepository.GetICASAInfoByVC(partSn5);                    if ( string.IsNullOrEmpty(iCASA.icasa) || string.IsNullOrEmpty(iCASA.anatel1) || string.IsNullOrEmpty(iCASA.anatel2) )
                    {
                        List<string> errpara = new List<string>();
                        errpara.Add(partSn5);
                        throw new FisException("CHK968", errpara);  //VC：%1 的 Antel1,Antel2,ICASA 有资料为空，請IE Maintain
                    }                }
            }
            IList<string> tmp1 = bomRep.GetPnListByModelAndBomNodeType(curProduct.Model,"PL","Anatel label");
            if (tmp1.Count > 0)
            {
                CurrentSession.AddValue(Session.SessionKeys.PrintLogName, "Anatel label");
            }
            else {
                CurrentSession.AddValue(Session.SessionKeys.PrintLogName, "ICASA label");
            }

            CurrentSession.AddValue(Session.SessionKeys.PrintLogBegNo, curProduct.ProId);
            CurrentSession.AddValue(Session.SessionKeys.PrintLogEndNo, curProduct.ProId);
            CurrentSession.AddValue(Session.SessionKeys.PrintLogDescr, curProduct.ProId);

            return base.DoExecute(executionContext);
        }
	}
}
