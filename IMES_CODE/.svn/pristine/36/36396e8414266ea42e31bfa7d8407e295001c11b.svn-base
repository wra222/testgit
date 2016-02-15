/*
 * INVENTEC corporation (c)2011 all rights reserved. 
 * Description:GetASTList activity for AFTMVS Page
 *                 
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2011-12-20  itc202017             Create
 * Known issues:
*/
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Workflow.ComponentModel;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;
using IMES.FisObject.FA.Product;
using IMES.FisObject.Common.PrintLog;
using IMES.Infrastructure;
using IMES.DataModel;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.Model;
using IMES.FisObject.Common.Misc;
using Metas = IMES.Infrastructure.Repository._Metas;
using System.ComponentModel;
using IMES.Common;


namespace IMES.Activity
{
    /// <summary>
    /// 获取要卡的资产列表
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      CI-MES12-SPEC-FA-UC AFT MVS.docx
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///     详见UC
    /// </para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.SessionKeys.Product
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         无
    /// </para> 
    ///<para> 
    /// 输出：
    ///         Session.SessionKeys.RandomInspectionStation
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///         无
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         IProduct
    ///         IFamilyRepository
    ///         IModelRepository
    /// </para> 
    /// </remarks>
    public partial class GetASTList : BaseActivity
    {
        ///<summary>
        ///</summary>
        public GetASTList()
        {
            InitializeComponent();
        }
        /// <summary>
        ///  NoAstDefineThrowError
        /// </summary>
        public static DependencyProperty NoAstDefineThrowErrorProperty = DependencyProperty.Register("NoAstDefineThrowError", typeof(bool), typeof(GetASTList), new PropertyMetadata(true));

        /// <summary>
        /// NoAstDefineThrowError
        /// </summary>
        [DescriptionAttribute("NoAstDefineThrowError")]
        [CategoryAttribute("GenerateATSN7 Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public bool NoAstDefineThrowError
        {
            get
            {
                return ((bool)(base.GetValue(NoAstDefineThrowErrorProperty)));
            }
            set
            {
                base.SetValue(NoAstDefineThrowErrorProperty, value);
            }
        }

        /// <summary>
        /// 获取要卡的资产列表
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            Session session = CurrentSession;
            ActivityCommonImpl utl = ActivityCommonImpl.Instance;
            //IProduct CurrentProduct = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);
            IProduct CurrentProduct = utl.IsNull<IProduct>(session, Session.SessionKeys.Product);

            IBOMRepository bomRep = RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();
            IModelRepository iModelRepository = RepositoryFactory.GetInstance().GetRepository<IModelRepository>();
            IMiscRepository miscRep = RepositoryFactory.GetInstance().GetRepository<IMiscRepository>();

            IHierarchicalBOM bom = bomRep.GetHierarchicalBOMByModel(CurrentProduct.Model);
            IList<IBOMNode> bomNodeList = bom.FirstLevelNodes;

            List<string> errpara = new List<string>();
            errpara.Add(CurrentProduct.ProId);

            //IList<string> pnsOfATSN1AndATSN3 = new List<string>();
            IList<string> pnsOfATSN1 = new List<string>();
            IList<string> pnsOfATSN3 = new List<string>();
            IList<string> pnsOfAT_any_5 = new List<string>();
            IList<string> pnsOfATSN7 = new List<string>();
            //IList<string> pnsOfATSN = new List<string>();
            
            //List<BomItemInfo> sessionBOM = new List<BomItemInfo>();
            //bool bExistAT_any_7 = false;
            IList<AstDefineInfo> astDefineInfoList = miscRep.GetData<Metas.AstDefine, AstDefineInfo>(null);
            if (astDefineInfoList == null || astDefineInfoList.Count == 0)
            {
                throw new FisException("CQCHK1085", (List<string>) null);                
            }

            bool bNeedPrintASTN7Label = false;
            foreach (IBOMNode bomNode in bomNodeList)
            {
                if (bomNode.Part.BOMNodeType == "AT" && bomNode.Part.Descr == "ATSN7")
                {
                    bNeedPrintASTN7Label = true;
                    pnsOfATSN7.Add(bomNode.Part.PN);
                }
            }

            bool bIsCDSIModel = false;
            string atsnav = CurrentProduct.GetModelProperty("ATSNAV") as string;
            if (atsnav != null && atsnav != "") bIsCDSIModel = true;

            IList<IProductPart> productParts = CurrentProduct.ProductParts;
            //bool bExistPart = false;
            bool bExistATSN7Part = false;
            bool bExistCDSIParts = false;
            foreach (IProductPart partNode in productParts)
            {
                //if (partNode.BomNodeType == "AT" && (partNode.PartType == "ATSN3" || partNode.PartType == "ATSN5")) bExistCDSIParts = true;
                if (astDefineInfoList.Any(x=> x.AstCode       !=  "ATSN7" &&
                                                            x.HasCDSIAst =="Y"   &&
                                                            partNode.BomNodeType== x.AstType && 
                                                            partNode.PartType == x.AstCode ))
                {
                    bExistCDSIParts = true;
                }

                if (partNode.BomNodeType == "AT" && partNode.PartType == "ATSN7") bExistATSN7Part = true;
            }

            IPrintLogRepository printLogRep = RepositoryFactory.GetInstance().GetRepository<IPrintLogRepository>();
            
            //(4/19)若ProductID为CDSI机器，检查Product_Part结合的AT料(Product_Part.BomNodeType=’AT’)，且不存在ATSN3或ATSN5或ATSN7(Product_Part.PartType like ‘ATSN[3,5,7]’)，则报错：“Product：XXX为CDSI机器，必须结合资产标签。请与IE 联系，检查BOM资料的完整性”
            IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            SnoDetPoMoInfo cond1 = new SnoDetPoMoInfo();
            cond1.snoId = CurrentProduct.ProId;
            cond1.remark = "N";
            bool bCDSICond3 = productRepository.CheckExistSnoDetPoMoInfo(cond1);
            if (bIsCDSIModel && !bCDSICond3 && !bExistCDSIParts)
            {
                throw new FisException("CHK522", errpara);
            }           
           

            bool astDefineATSN7 =astDefineInfoList.Any(x => x.NeedAssignAstSN == "Y" && x.AssignAstSNStation == this.Station);
            if (!astDefineATSN7 &&
               this.NoAstDefineThrowError )
            { 
                throw new FisException("'CQCHK1090'", new List<string> { CurrentProduct.ProId, this.Station, "ATSN7" });
            }
            
            //sessionBOM.Sort(new IcpBomItemInfo());

            //CurrentSession.AddValue(Session.SessionKeys.SessionBom, sessionBOM);
           // CurrentSession.AddValue("bMatchSN", false);
          //  CurrentSession.AddValue("bMatchID", false);
            //CurrentSession.AddValue("CRCOfProductID", getCRC(CurrentProduct.ProId));
            //CurrentSession.AddValue(Session.SessionKeys.MatchedParts, ",");
            CurrentSession.AddValue("bNeedATSN7", (bNeedPrintASTN7Label && !bExistATSN7Part && astDefineATSN7));
            CurrentSession.AddValue("PnListOfATSN7", pnsOfATSN7);
            CurrentSession.AddValue("bCDSI", bIsCDSIModel);

            return base.DoExecute(executionContext);
        }

        private string getCRC(string orig)
        {
            string sequence = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            int sum = 0;
            foreach (char c in orig.ToUpper())
            {
                int pos = sequence.IndexOf(c);
                sum += pos >= 0 ? pos : 36;
            }
            sum %= 16;
            return sequence[sum].ToString();
        }

        private class IcpBomItemInfo : IComparer<BomItemInfo>
        {
            public int Compare(BomItemInfo x, BomItemInfo y)
            {
                return x.type.CompareTo(y.type);
            }
        }
    }
}
