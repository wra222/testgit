// INVENTEC corporation (c)2011 all rights reserved. 
// Description: Change PO Product Condition
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2015-01-21 Vincent             create
// Known issues:
using System;
using System.Linq;
using System.Workflow.ComponentModel;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.Generic;
using System.ComponentModel;
using IMES.DataModel;
using IMES.FisObject.Common.Part;
using IMES.FisObject.Common.UPS;
using IMES.UPS;
using IMES.UPS.WS;
using IMES.FisObject.PAK.DN;
using System.Data;
using System.IO;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Misc;
using IMES.Common;

namespace IMES.Activity
{
    /// <summary>
    /// RemoveUPSAst Product
    /// </summary>
    public partial class RemoveUPSAst : BaseActivity
    {
        ///<summary>
        ///</summary>
        public RemoveUPSAst()
        {
            InitializeComponent();
        }

        private static string BtwFile = "BtwFile";
        private static string BtwLabelData = "BtwLabelData";

        /// <summary>
        ///  Remove IsRemoveAstPart
        /// </summary>
        public static DependencyProperty IsRemoveAstPartProperty = DependencyProperty.Register("IsRemoveAstPart", typeof(bool), typeof(RemoveUPSAst), new PropertyMetadata(false));

        /// <summary>
        /// write IsRemoveAstPart
        /// </summary>
        [DescriptionAttribute("IsRemoveAstPart")]
        [CategoryAttribute("IsRemoveAstPart Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public bool IsRemoveAstPart
        {
            get
            {
                return ((bool)(base.GetValue(IsRemoveAstPartProperty)));
            }
            set
            {
                base.SetValue(IsRemoveAstPartProperty, value);
            }
        }


        /// <summary>
        ///  Remove IsRemoveCDSISpecialDet
        /// </summary>
        public static DependencyProperty IsRemoveCDSISpecialDetProperty = DependencyProperty.Register("IsRemoveCDSISpecialDet", typeof(bool), typeof(RemoveUPSAst), new PropertyMetadata(true));

        /// <summary>
        /// write IsRemoveCDSISpecialDet
        /// </summary>
        [DescriptionAttribute("IsRemoveCDSISpecialDet")]
        [CategoryAttribute("IsRemoveCDSISpecialDet Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public bool IsRemoveCDSISpecialDet
        {
            get
            {
                return ((bool)(base.GetValue(IsRemoveCDSISpecialDetProperty)));
            }
            set
            {
                base.SetValue(IsRemoveCDSISpecialDetProperty, value);
            }
        }

        /// <summary>
        /// GetAndSetBtwPrintInfo
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            Session session = CurrentSession;
            ActivityCommonImpl utl = ActivityCommonImpl.Instance;

            var product = utl.IsNull<IProduct>(session, Session.SessionKeys.Product);
            IProductRepository prodRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            IDeliveryRepository dnRep = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository>();
            IUPSRepository upsRep = RepositoryFactory.GetInstance().GetRepository<IUPSRepository>();

            
            IBOMRepository bomRep = RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();
            IMiscRepository miscRep = RepositoryFactory.GetInstance().GetRepository<IMiscRepository>();
            IHierarchicalBOM bom = bomRep.GetHierarchicalBOMByModel(product.Model);
            IList<IBOMNode> bomNode = bom.FirstLevelNodes;

            List<string> erpara = new List<string>();

            IList<AstDefineInfo> astDefineInfoList = miscRep.GetData<IMES.Infrastructure.Repository._Metas.AstDefine, AstDefineInfo>(null);
            if (astDefineInfoList == null || astDefineInfoList.Count == 0)
            {
                throw new FisException("CQCHK1085", erpara);
            }

            #region 檢查 BOM 是否有BomNodeType 為 AT及 PP類

            var RemovePartList = bomNode.Where(x => astDefineInfoList.Any(y => y.AstType == x.Part.BOMNodeType && 
                                                                                                                     y.AstCode == x.Part.Descr &&
                                                                                                                     (y.NeedAssignAstSN=="Y"||y.NeedScanSN=="Y")))
                                                    .Select(x=>x.Part).ToList();
           
            #endregion  
          
            if (RemovePartList.Count == 0)
            {
                return base.DoExecute(executionContext); 
            }

            DateTime now = DateTime.Now;
            
            string sn = product.CUSTSN;

            UPSCombinePO combinePO = product.UPSCombinePO;

            if (combinePO != null)
            {
                UPSWS ws = UPSWS.Instance;
                ResetStruct rs=ws.ResetSN(sn);
                if (rs.retcode < 0)
                {
                    //throw error
                    throw new FisException("CQCHK1118", new string[] { "UPSResetSN", product.CUSTSN, "", "", rs.message });
                }

                combinePO.Status = EnumUPSCombinePOStatus.Release.ToString();
                combinePO.Udt = now;
                combinePO.ProductID =string.Empty;
                combinePO.CUSTSN =string.Empty;
                combinePO.IsShipPO ="N";
                combinePO.Station =this.Station;
                combinePO.Editor = this.Editor;
                combinePO.Remark = combinePO.ProductID + "~" + combinePO.CUSTSN + "~" + combinePO.Station + "~" + combinePO.IsShipPO;
                upsRep.Update(combinePO, session.UnitOfWork);

                if (product.IsCDSI)
                {
                    prodRep.RemoveCDSIASTDefered(session.UnitOfWork, product.ProId);
                    prodRep.RemoveSnoDetPoMoDefered(session.UnitOfWork, product.ProId);
                    if (this.IsRemoveCDSISpecialDet)
                    {
                        prodRep.RemoveSpecialDetDefered(session.UnitOfWork, product.ProId);
                    }

                    if (product.IsCNRS)
                    {
                        product.SetAttributeValue("CNRSState", "", this.Editor, this.Station, "");
                    }
                    else
                    {
                        product.SetAttributeValue("CDSIState", "", this.Editor, this.Station, "");
                    }

                }

                var needCleanValueList = product.ProductInfoes.Where(x => x.InfoType.StartsWith(BtwFile) || 
                                                                                                            x.InfoType.StartsWith(BtwLabelData)).ToList();
                foreach (IMES.FisObject.FA.Product.ProductInfo info in needCleanValueList)
                {
                    product.SetExtendedProperty(info.InfoType, string.Empty, this.Editor);
                }
                

                prodRep.Update(product, session.UnitOfWork);
            }
        
            //Remove Product Part
            #region dn't need release product_part
            if (this.IsRemoveAstPart)
            {
                foreach (var part in RemovePartList)
                {
                    //var productPart = product.ProductParts.Where(x => x.PartID == part.PN && x.PartType == part.Descr).FirstOrDefault();
                    //if (productPart != null)
                    //{
                    //    IList<CombinedAstNumberInfo> destBindAstInfo = prodRep.GetCombinedAstNumber(new CombinedAstNumberInfo
                    //    {
                    //        ProductID = product.ProId,
                    //        AstNo = productPart.PartSn,
                    //        State = "Used",
                    //        AstType = "AST"
                    //    });
                    //    if (destBindAstInfo != null && destBindAstInfo.Count > 0)
                    //    {
                    //        foreach (CombinedAstNumberInfo item in destBindAstInfo)
                    //        {
                    //            item.Remark = string.Format("BindStation:{0}", item.Station);
                    //            item.UnBindProductID = item.ProductID;
                    //            item.UnBindStation = this.Station;
                    //            item.ProductID = "";
                    //            item.Station = "";
                    //            item.Udt = DateTime.Now;
                    //            item.State = "Release";
                    //            item.Editor = this.Editor;
                    //            prodRep.UpdateCombinedAstNumberDefered(session.UnitOfWork, item);
                    //        }
                    //    }
                    //}

                    prodRep.BackUpProductPartByBomNodeTypeAndDescrLikeDefered(session.UnitOfWork, product.ProId,
                                                                                                                            this.Editor, part.BOMNodeType,
                                                                                                                            part.Descr);
                    prodRep.RemoveProductPartByBomNodeTypeAndDescrLikeDefered(session.UnitOfWork, product.ProId,
                                                                                                                             part.BOMNodeType,
                                                                                                                            part.Descr);

                }
            }
            #endregion

            return base.DoExecute(executionContext); 
                       
           
        }

        

    }

    
}
