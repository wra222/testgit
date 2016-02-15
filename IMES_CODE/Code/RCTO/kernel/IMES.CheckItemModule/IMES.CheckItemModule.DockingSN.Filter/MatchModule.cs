using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using IMES.CheckItemModule.Interface;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;
using System.Data;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.Repository._Schema;
using System.Data.SqlClient;

namespace IMES.CheckItemModule.DockingSN.Filter
{
    [Export(typeof(IMatchModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.DockingSN.Filter.dll")]
    class MatchModule : IMatchModule
    {
        public Object Match(string subject, object bomItem, string station)
        {
            if (subject == null)
            {
                throw new ArgumentNullException();
            }
            if (bomItem == null)
            {
                throw new ArgumentNullException();
            }
            PartUnit ret = null;
            IList<IPart> flat_bom_items = ((IFlatBOMItem)bomItem).AlterParts;
            IPart part = null;
            if (flat_bom_items != null)
            {
                part = flat_bom_items.ElementAt(0);    
            }
            
            //若@Data 长度为10位，并且第7位是字符'V' |'W' | 'X' | 'Y' | 'Z'，则用户刷入的为Docking S/N
            if (10 == subject.Length)
            {
                if (subject[6] == 'V' || subject[6] == 'W' || subject[6] == 'X' || subject[6] == 'Y' || subject[6] == 'Z')
                {
                    if (part != null)
                    {
                        //Copy 上海0002244: Combine Pizza页面修改(結合多個 Docking Case)

                        IProductRepository productRepository =
                       RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();

                        DataTable tb2 = productRepository.ExecSpForQuery(SqlHelper.ConnectionString_PAK, "PC_CombinePizzaforCheckDockingSn",
                                                                                                        new[] { new SqlParameter("DockingSn", subject), 
                                                                                                                   new SqlParameter("DockingPn", part.PN) });
                        if (tb2 != null && tb2.Rows.Count > 0)
                        {
                            ret = new PartUnit(part.PN, subject.Trim(), part.BOMNodeType, part.Type, string.Empty, part.CustPn, ((IFlatBOMItem)bomItem).CheckItemType);
                        }

                        //ret = new PartUnit(part.PN, subject.Trim(), part.BOMNodeType, part.Type, string.Empty, part.CustPn, ((IFlatBOMItem)bomItem).CheckItemType);
                    }
                }
            }

            return ret;
        }
    }
}
