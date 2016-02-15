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

using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure;
using IMES.FisObject.FA.Product;
using IMES.FisObject.Common.QTime;
using IMES.FisObject.Common.Line;
using System.Collections.Generic;
using IMES.Infrastructure.UnitOfWork;
using IMES.FisObject.Common.TestLog;
using IMES.Infrastructure.Extend;
using System.Data.SqlClient;
using IMES.FisObject.Common.Station;
using IMES.Infrastructure.Repository._Schema;
using IMES.DataModel;
using IMES.Common;

namespace IMES.Activity
{
    /// <summary>
    /// 
    /// </summary>
    public partial class OfflineControl : BaseActivity
	{
        /// <summary>
        /// 
        /// </summary>
		public OfflineControl()
		{
			InitializeComponent();
		}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            IProductRepository prodRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            IQTimeRepository qTimeRep = RepositoryFactory.GetInstance().GetRepository<IQTimeRepository, QTime>();
            ILineRepository lineRep = RepositoryFactory.GetInstance().GetRepository<ILineRepository, Line>();
            DateTime now = DateTime.MinValue;
          
            var prod = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);
            string pdline = prod.Status.Line;
            IUnitOfWork uow = new UnitOfWork();
            string defectStation = this.Station;
            string preStation = prod.Status.StationId.Trim();
            string defctcode=  (string)CurrentSession.GetValue(Session.SessionKeys.DefectList);
            TestLog.TestLogStatus defectStatus = TestLog.TestLogStatus.Fail;
            prod.UpdateStatus(new IMES.FisObject.FA.Product.ProductStatus()
                    {
                        Line = pdline,
                        ProId = prod.ProId,
                        TestFailCount = 0,
                        ReworkCode = "",
                        StationId = this.Station,
                        Status = StationStatus.Fail ,
                        Editor = this.Editor,
                        Udt = now
                    });
           IList<IMES.DataModel.TbProductStatus> stationList = prodRep.GetProductStatus(new List<string> { prod.ProId });
           prodRep.UpdateProductPreStationDefered(uow, stationList);

                  

           #region write Productlog

            ProductLog productLog = new ProductLog
                    {
                        Model = prod.Model,
                        Status =StationStatus.Fail ,
                        Editor = this.Editor,
                        Line = pdline,
                        Station = defectStation,
                        Cdt = now
                    };
             prod.AddLog(productLog);

           #endregion
              
           #region add test log
                    string actionName =  "OfflineQTime";
                    string errorCode = "";
                    string descr = "PreStation:" + preStation + "~CurStation:" + this.Station + "~TimeOut:" ;

                    //TestLog testLog = new TestLog(0, prod.ProId, this.Line, "", defectStation, defectStatus, "", this.Editor, "PRD", DateTime.Now);
                    TestLog testLog = new TestLog(0, prod.ProId, pdline, "", defectStation, defectStatus, "",
                                                                        actionName, errorCode, descr, this.Editor, "PRD", now);

                    prod.AddTestLog(testLog);
                    //add defect
                    TestLogDefect defectItem = new TestLogDefect(0, 0, defctcode, this.Editor, now);
                    testLog.AddTestLogDefect(defectItem);
              #endregion   
           prodRep.Update(prod, uow);
           uow.Commit();

            return base.DoExecute(executionContext);
        }
	}
}
