
using System;
using System.Workflow.ComponentModel;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using System.Collections.Generic;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;
using System.Linq;
using System.ComponentModel;
using metas=IMES.Infrastructure.Repository._Metas;
using IMES.FisObject.Common.PrintLog;
using IMES.DataModel;
using IMES.Common; 

namespace IMES.Activity
{
    /// <summary>
    ///  非產生Asset SN寫PrintLog 或 ProductLog
    /// </summary>
    public partial class WriteASTPrint : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public WriteASTPrint()
        {
            InitializeComponent();
        }

        /// <summary>
        ///  write Print Log
        /// </summary>
        public static DependencyProperty IsWritePrintLogProperty = DependencyProperty.Register("IsWritePrintLog", typeof(bool), typeof(WriteASTPrint), new PropertyMetadata(true));

        /// <summary>
        /// write Print Log
        /// </summary>
        [DescriptionAttribute("IsWritePrintLog")]
        [CategoryAttribute("IsWritePrintLog Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public bool IsWritePrintLog
        {
            get
            {
                return ((bool)(base.GetValue(IsWritePrintLogProperty)));
            }
            set
            {
                base.SetValue(IsWritePrintLogProperty, value);
            }
        }

        /// <summary>
        ///  write Product Log
        /// </summary>
        public static DependencyProperty IsWriteProductLogProperty = DependencyProperty.Register("IsWriteProductLog", typeof(bool), typeof(WriteASTPrint), new PropertyMetadata(true));

        /// <summary>
        /// write Product Log
        /// </summary>
        [DescriptionAttribute("IsWriteProductLog")]
        [CategoryAttribute("IsWriteProductLog Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public bool IsWriteProductLog
        {
            get
            {
                return ((bool)(base.GetValue(IsWriteProductLogProperty)));
            }
            set
            {
                base.SetValue(IsWriteProductLogProperty, value);
            }
        }

        /// <summary>
        /// 非產生Asset SN
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            Session session = CurrentSession;
            ActivityCommonImpl utl = ActivityCommonImpl.Instance;
            
            IProduct currenProduct = utl.IsNull<IProduct>(session, Session.SessionKeys.Product);
            var prodRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            IList<AstDefineInfo> needCombineAstDefineList = utl.IsNull<IList<AstDefineInfo>>(session, Session.SessionKeys.NeedCombineAstDefineList);
            IList<IPart> needCombineAstPartList = utl.IsNull<IList<IPart>>(session, Session.SessionKeys.NeedCombineAstPartList);
            var needCombineAstList = needCombineAstDefineList.Where(x => x.NeedAssignAstSN != "Y" || x.AssignAstSNStation!=this.Station).ToList();
            if (needCombineAstList.Count>0)
            {
                foreach(AstDefineInfo item in needCombineAstList)
                {
                   if (IsWriteProductLog)
                  {
                      writeProductLog(session, currenProduct, IMES.FisObject.Common.Station.StationStatus.Pass,item.AstCode);
                  }

                   if (IsWritePrintLog)
                   {
                       if (item.NeedAssignAstSN == "Y")
                       {
                           var partNosList = needCombineAstPartList.Where(x => x.Descr == item.AstCode).Select(y => y.PN).Distinct().ToArray();
                           IList<IProductPart> productPartList = currenProduct.ProductParts.Where(x => partNosList.Contains(x.PartID)).ToList();  //prodRep.GetProductPartsByPartNosAndProdId(partNosList, currenProduct.ProId);
                           foreach (IProductPart part in productPartList)
                           {
                               if (item.NeedPrint == "Y")
                               {
                                   writePrintLog(session, currenProduct.ProId, currenProduct.ProId, part.PartSn, item.AstCode);
                               }
                               session.AddValue("AssetSN", part.PartSn);
                           }
                       }
                       else if (item.NeedPrint == "Y")
                       {
                           writePrintLog(session, currenProduct.ProId, currenProduct.ProId, "", item.AstCode);
                       }
                   }                
                }
            }        

            return base.DoExecute(executionContext);
        }

        private void writePrintLog(Session session, string beginNo, string endNo, string descr,  string templateName)
        {
              var printRep = RepositoryFactory.GetInstance().GetRepository<IPrintLogRepository, PrintLog>();  
                var item = new PrintLog
                {
                    Name = "AT",
                    BeginNo = beginNo,
                    EndNo = endNo,
                    Descr = descr,
                    Station = this.Station,
                    LabelTemplate = templateName,
                    Cdt = DateTime.Now,
                    Editor = this.Editor
                };
               
                printRep.Add(item, session.UnitOfWork);           
        }

        private void writeProductLog(Session session,IProduct product, IMES.FisObject.Common.Station.StationStatus status,  string line)
        {
            var prodRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            var productLog = new ProductLog
            {
                Model = product.Model,
                Status = status,
                Editor = this.Editor,
                Line = line,
                Station = this.Station,
                Cdt = DateTime.Now
            };

            product.AddLog(productLog);
            prodRep.Update(product, session.UnitOfWork);
        }
    }
}
