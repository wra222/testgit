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

namespace IMES.CheckItemModule.DockingBaseSN.Filter
{
    [Export(typeof(ICheckModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.DockingBaseSN.Filter.dll")]
    class CheckModule : ICheckModule
    {

        //a)检查Base 的状态为69 ,Status为1(检查Docking 数据库的ProductStatus)        
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
                    DataTable tb = productRepository.ExecSpForQuery(SqlHelper.ConnectionString_PAK, 
                                                                                                "PC_CombineBaseforCheckProductStatus", 
                                                                                                new SqlParameter("DockingSn", dockingSn));

                    if (tb == null || tb.Rows.Count == 0)
                    {
                        List<string> errpara = new List<string>();
                        FisException ex =new FisException("CHK320", errpara);
                        ex.stopWF=false;
                        throw ex;//status is error.
                    }

                    //DataTable tb1 = productRepository.ExecSpForQuery(SqlHelper.ConnectionString_PAK, "PC_CombinePizzaforCheckQCStatus", new SqlParameter("DockingSn", dockingSn));

                    //if (tb1 == null || tb1.Rows.Count == 0)
                    //{
                    //    List<string> errpara = new List<string>();
                    //    FisException ex = new FisException("CHK321", errpara);
                    //    ex.stopWF = false;
                    //    throw ex;//QCstatus is error.
                    //}
                  
                }
            }
        }
    }
}
