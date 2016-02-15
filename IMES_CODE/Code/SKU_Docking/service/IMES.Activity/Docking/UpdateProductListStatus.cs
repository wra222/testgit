
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
using System.Collections.Generic;
using IMES.FisObject.FA.Product;
using IMES.FisObject.Common.MO;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.Station;
using IMES.Infrastructure.Extend;
using System.Data.SqlClient;


namespace IMES.Activity
{

    /// <summary>
    /// 
    /// </summary>
    public partial class UpdateProductListStatus : BaseActivity
    {
        /// <summary> 
        /// </summary>
        public UpdateProductListStatus()
        {
            InitializeComponent();
        }

        /// <summary> 
        /// </summary>        
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            var currentStation = default(string);

            currentStation = Station;
            

            IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            IList<IProduct> prodList = new List<IProduct>();
            prodList = (IList<IProduct>)CurrentSession.GetValue(Session.SessionKeys.ProdList);

            #region  record multi-productId product status
             
            //System.Data.DataTable productTb = CreateDataTable.CreateStringListTb();
            //foreach (IProduct currentProduct in prodList)
            //{
            //    productTb.Rows.Add(currentProduct.ProId);
            //}
            //SqlParameter para1 = new SqlParameter("ProductIDList", System.Data.SqlDbType.Structured);
            //para1.Direction = System.Data.ParameterDirection.Input;
            //para1.Value = productTb;

            //IProductRepository productRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
               
            //productRep.ExecSpForNonQueryDefered(CurrentSession.UnitOfWork,
            //                                                                                IMES.Infrastructure.Repository._Schema.SqlHelper.ConnectionString_FA,
            //                                                                                "IMES_MultiUpdateProductStatus",
            //                                                                               para1,
            //                                                                               new SqlParameter("station", Station),
            //                                                                               new SqlParameter("status", 1),
            //                                                                               new SqlParameter("line", Line),
            //                                                                               new SqlParameter("editor", Editor),
            //                                                                               new SqlParameter("udt", DateTime.Now)
            //                                                                               );

            var productIDList = (from item in prodList
                                             select item.ProId).ToList();

            IList<IMES.DataModel.TbProductStatus> stationList = productRepository.GetProductStatus(productIDList);
            productRepository.UpdateProductPreStationDefered(CurrentSession.UnitOfWork, stationList);

  
            #endregion

            foreach (IProduct currentProduct in prodList)
            {
                var newStatus = new ProductStatus();

                string line = default(string);
                if (string.IsNullOrEmpty(this.Line))
                {
                    line = currentProduct.Status.Line;
                }
                else
                {
                    line = this.Line;
                }

                newStatus.Status = StationStatus.Pass;
                newStatus.StationId = currentStation;
                newStatus.Editor = Editor;
                newStatus.Line = line;

                newStatus.TestFailCount = 0;
                
                
               
                if (!string.IsNullOrEmpty(currentProduct.Status.ReworkCode) && productRepository.IsLastReworkStation(currentProduct.Status.ReworkCode, Station, (int)StationStatus.Pass))
                {
                    newStatus.ReworkCode = "";
                    IMES.DataModel.Rework r = new IMES.DataModel.Rework();
                    r.ReworkCode = currentProduct.Status.ReworkCode;
                    r.Editor = Editor;
                    r.Status = "3";
                    r.Udt = DateTime.Now;
                    productRepository.UpdateReworkConsideredProductStatusDefered(CurrentSession.UnitOfWork, r, currentProduct.ProId);
                }
                else
                {
                    newStatus.ReworkCode = currentProduct.Status.ReworkCode;
                }
                newStatus.ProId = currentProduct.ProId;


                currentProduct.UpdateStatus(newStatus);

                productRepository.Update(currentProduct, CurrentSession.UnitOfWork);
            }

            return base.DoExecute(executionContext);
        }
    }
}