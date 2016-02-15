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
using IMES.Common;
namespace IMES.Activity
{
    /// <summary>
    /// ChangePo Product
    /// </summary>
    public partial class ChangePo : BaseActivity
    {
        ///<summary>
        ///</summary>
        public ChangePo()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Change PO
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            Session session = CurrentSession;
            ActivityCommonImpl utl = ActivityCommonImpl.Instance;
            IProduct srcProd = utl.IsNull<IProduct>(session, Session.SessionKeys.SrcProduct);
            IProduct destProd = utl.IsNull<IProduct>(session, Session.SessionKeys.DestProduct);

            IProductRepository prodRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            
            prodRep.BackUpProductDefered(session.UnitOfWork, srcProd.ProId, this.Editor);
            prodRep.BackUpProductDefered(session.UnitOfWork, destProd.ProId, this.Editor);

            srcProd.AddLog(new ProductLog
            {
                ProductID = srcProd.ProId,
                Model = srcProd.Model,
                Line = string.IsNullOrEmpty(this.Line) ? srcProd.Status.Line : this.Line,
                Station = this.Station,
                Status = IMES.FisObject.Common.Station.StationStatus.Pass,
                Editor = this.Editor
            });

            destProd.AddLog(new ProductLog
            {
                ProductID = destProd.ProId,
                Model = destProd.Model,
                Line = string.IsNullOrEmpty(this.Line) ? destProd.Status.Line : this.Line,
                Station = this.Station,
                Status = IMES.FisObject.Common.Station.StationStatus.Pass,
                Editor = this.Editor
            });

            string srcMoId = srcProd.MO;
            string destMoId = destProd.MO;

            srcProd.MO = destMoId;
            destProd.MO = srcMoId;

            prodRep.Update(srcProd, session.UnitOfWork);
            prodRep.Update(destProd, session.UnitOfWork);

            return base.DoExecute(executionContext);
        }

    }
}
