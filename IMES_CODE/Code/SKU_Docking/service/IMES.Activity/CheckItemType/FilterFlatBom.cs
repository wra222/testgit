using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Drawing;
using System.Linq;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;
using IMES.DataModel;
using IMES.Infrastructure;
using System.Collections.Generic;
using IMES.FisObject.PCA.MB;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PAK.Pallet;
using IMES.FisObject.PAK.Carton;
using IMES.FisObject.PAK.DN;
using IMES.FisObject.Common.PrintLog;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.PAK.Pizza;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;
using IMES.Infrastructure.Extend;

namespace IMES.Activity
{
    /// <summary>
    /// 檢查GetBom 回傳的FlatBomItem 清單是有在Product_Part/Pizza_Part 收集過
    /// </summary>
    public partial class FilterFlatBom : BaseActivity
    {
        /// <summary>
        /// 檢查GetBom 回傳的FlatBomItem 清單是有在Product_Part/Pizza_Part 收集過
        /// </summary>
        public FilterFlatBom()
        {
            InitializeComponent();
        }
        /// <summary>
        ///  檢查GetBom 回傳的FlatBomItem 清單是有在Product_Part/Pizza_Part 收集過
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            Session session = CurrentSession;
			session.AddValue("NoBomItem", "");
            session.AddValue(ExtendSession.SessionKeys.NeedMoveOut, "");
            IProduct prod = (IProduct)CurrentSession.GetValue(Session.SessionKeys.Product);
            IList<IProductPart> productPartList = new List<IProductPart>();
            IList<IProductPart> pizzaPartList = new List<IProductPart>();
            if (prod.ProductParts != null)
            { 
                productPartList = prod.ProductParts; 
            }
            if (prod.PizzaObj != null)
            { 
                pizzaPartList = prod.PizzaObj.PizzaParts; 
            }
         
            var bom = (IFlatBOM)CurrentSession.GetValue(Session.SessionKeys.SessionBom);
            IList<IFlatBOMItem> removeBomLst = new List<IFlatBOMItem>();
            if (bom!=null && bom.BomItems!=null && bom.BomItems.Count > 0)
            {
                foreach (IFlatBOMItem flatBom in bom.BomItems)
                {
                    foreach (IPart part in flatBom.AlterParts)
                    {
                        if (productPartList.Count > 0 && productPartList.Any(x => x.PartID == part.PN &&
                                                                                                               x.BomNodeType == part.BOMNodeType  && 
                                                                                                               x.CheckItemType == flatBom.CheckItemType))
                        {
                            if (this.CheckBindedCT)
                            {
                                flatBom.HasBinded = true;
                                flatBom.NeedSave = false;
                                flatBom.NeedCommonSave = false;
                            }
                            else
                            {
                                removeBomLst.Add(flatBom);
                            }
                            break;
                        }
                    }
                    foreach (IPart part in flatBom.AlterParts)
                    {
                        if (pizzaPartList.Count > 0 && pizzaPartList.Any(x => x.PartID == part.PN &&
                                                                                                 x.BomNodeType == part.BOMNodeType &&    
                                                                                                x.CheckItemType == flatBom.CheckItemType))
                        {
                            if (this.CheckBindedCT)
                            {
                                flatBom.HasBinded = true;
                                flatBom.NeedSave = false;
                                flatBom.NeedCommonSave = false;
                            }
                            else
                            {
                                removeBomLst.Add(flatBom);
                            }
                            break;
                        }
                    }
                }

                foreach (IFlatBOMItem f in removeBomLst)
                {
                    bom.BomItems.Remove(f);
                }
            }

            if (bom == null || bom.BomItems==null || bom.BomItems.Count == 0)
           {
			   session.AddValue("NoBomItem", "Y");
               if (NoFlatBomAction == NoFlatBomActionEnum.ThrowError)
               {
                   throw new FisException("MAT013", new string[] { prod.ProId, this.Station });
               }
               else if (NoFlatBomAction == NoFlatBomActionEnum.MoveOut)
               {
                   session.AddValue(ExtendSession.SessionKeys.NeedMoveOut, "Y"); 
               }
               else //NoFlatBomActionEnum NoMoveOut
               {
                   session.AddValue(ExtendSession.SessionKeys.NeedMoveOut, "N"); 
               }
           }

            return base.DoExecute(executionContext);
        }


        /// <summary>
        /// NoFlatBomActionEnum
        /// </summary>     
        public enum NoFlatBomActionEnum
        {
            /// <summary>
            /// ThrowError
            /// </summary>
            ThrowError = 1,
            /// <summary>
            /// NoMoveOut
            /// </summary>
            NoMoveOut = 2,
            /// <summary>
            /// MoveOut
            /// </summary>
            MoveOut = 3,

        }


        /// <summary>
        /// NoFlatBomActionProperty
        ///  </summary>
        public static DependencyProperty NoFlatBomActionProperty = DependencyProperty.Register("NoFlatBomAction", typeof(NoFlatBomActionEnum), typeof(FilterFlatBom), new PropertyMetadata(NoFlatBomActionEnum.ThrowError));

        /// <summary>
        /// No FlatBom Action
        /// </summary>
        [DescriptionAttribute("NoFlatBomAction")]
        [CategoryAttribute("InArugment")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public NoFlatBomActionEnum NoFlatBomAction
        {
            get
            {
                return ((NoFlatBomActionEnum)(base.GetValue(FilterFlatBom.NoFlatBomActionProperty)));
            }
            set
            {
                base.SetValue(FilterFlatBom.NoFlatBomActionProperty, value);
            }
        }


        /// <summary>
        /// 檢查收集過CT
        ///  </summary>
        public static DependencyProperty CheckBindedCTProperty = DependencyProperty.Register("CheckBindedCT", typeof(bool), typeof(FilterFlatBom), new PropertyMetadata(true));

        /// <summary>
        /// 檢查收集過CT
        /// </summary>
        [DescriptionAttribute("CheckBindedCT")]
        [CategoryAttribute("CheckBindedCT")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public bool CheckBindedCT
        {
            get
            {
                return ((bool)(base.GetValue(CheckBindedCTProperty)));
            }
            set
            {
                base.SetValue(CheckBindedCTProperty, value);
            }
        }
    }
}
