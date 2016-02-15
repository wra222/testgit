using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using IMES.CheckItemModule.Interface;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.ComponentModel.Composition;
using IMES.Infrastructure.Repository._Schema;

namespace IMES.CheckItemModule.DockingSN.Filter
{
    [Export(typeof(ICheckModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.DockingSN.Filter.dll")]
    class CheckModule : ICheckModule
    {
        
        //a. 检查Docking 的状态为85 或者81 或者PO (检查Docking 数据库的ProductStatus)
        //b. 检查Docking 的PAQC 抽检状态是否为PAQC 抽检免检或者PAQC Check Pass （检查Docking 数据库的QCStatus）
        //b. 检查Docking 的PN 是否与ModelBOM 中的Docking Part No 一致（检查Docking 数据库的ModelInfo）
        public void Check(object partUnit, object bomItem, string station)
        {
            if (partUnit != null)
            {
                string dockingSn = ((PartUnit) partUnit).Sn;
                string dockingPn = ((PartUnit)partUnit).Pn;
                if (!string.IsNullOrEmpty(dockingSn))
                {
                    IProductRepository productRepository =
                        RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
                    DataTable tb = productRepository.ExecSpForQuery(SqlHelper.ConnectionString_PAK, "PC_CombinePizzaforCheckProductStatus", new SqlParameter("DockingSn", dockingSn));

                    if (tb == null || tb.Rows.Count == 0)
                    {
                        List<string> errpara = new List<string>();
                        FisException ex = new FisException("CHK320", errpara);
                        ex.stopWF = false;
                        throw ex;//status is error.
                    }

                    DataTable tb1 = productRepository.ExecSpForQuery(SqlHelper.ConnectionString_PAK, "PC_CombinePizzaforCheckQCStatus", new SqlParameter("DockingSn", dockingSn));
                   
                    if (tb1 == null || tb1.Rows.Count == 0)
                    {
                        List<string> errpara = new List<string>();
                        FisException ex = new FisException("CHK321", errpara);
                        ex.stopWF = false;
                        throw ex;//QCstatus is error.
                    }

                    //Copy 上海0002244: Combine Pizza页面修改(結合多個 Docking Case) disable
                    //DataTable tb2 = productRepository.ExecSpForQuery(SqlHelper.ConnectionString_PAK, "PC_CombinePizzaforCheckDockingSn", new[] { new SqlParameter("DockingSn", dockingSn), new SqlParameter("DockingPn", dockingPn) });

                    //if (tb2 == null || tb2.Rows.Count == 0)
                    //{
                    //    List<string> errpara = new List<string>();
                    //    throw new FisException("CHK322", errpara);//QCstatus is error.
                    //}

                }
            }
        }
    }
}
