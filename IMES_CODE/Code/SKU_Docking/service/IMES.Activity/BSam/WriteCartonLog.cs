// INVENTEC corporation (c)2011 all rights reserved. 
// Description: 
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2013-02-07   Benson          create
//2013-03-13    Vincent           release
// Known issues:
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
using IMES.FisObject.Common;
using IMES.FisObject.PAK.Pallet;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.Station;
using IMES.FisObject.FA.Product;
using System.Collections.Generic;
using IMES.FisObject.PAK.Carton;

namespace IMES.Activity
{


    /// <summary>
    /// Write Carton  Log
    /// </summary>
  
    public partial class WriteCartonLog : BaseActivity
	{
        /// <summary>
        /// 
        /// </summary>
        public WriteCartonLog()
		{
			InitializeComponent();
		}

        /// <summary>
        /// UnPackDNByXXX        
        /// </summary>
        public static DependencyProperty UnPackByProperty = DependencyProperty.Register("UnPackBy", typeof(UnPackByEnum), typeof(WriteCartonLog), new PropertyMetadata(UnPackByEnum.Carton));

        /// <summary>
        /// UnPackDNByXXX
        /// </summary>
        [DescriptionAttribute("UnPackBy")]
        [CategoryAttribute("InArguments Of WriteCartonLog")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public UnPackByEnum UnPackBy
        {
            get
            {
                return ((UnPackByEnum)(base.GetValue(WriteCartonLog.UnPackByProperty)));
            }
            set
            {
                base.SetValue(WriteCartonLog.UnPackByProperty, value);
            }
        }

        /// <summary>
        /// Activtiy所在Status
        /// </summary>
        public static DependencyProperty StatusProperty = DependencyProperty.Register("Status", typeof(StationStatus), typeof(WriteCartonLog), new PropertyMetadata(StationStatus.Pass));

        /// <summary>
        /// Status
        /// </summary>
        [DescriptionAttribute("Status")]
        [CategoryAttribute("Status Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public StationStatus Status
        {
            get
            {
                return ((StationStatus)(base.GetValue(WriteCartonLog.StatusProperty)));
            }
            set
            {
                base.SetValue(WriteCartonLog.StatusProperty, value);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
           ICartonRepository cartonRep = RepositoryFactory.GetInstance().GetRepository<ICartonRepository, Carton>();

           if (UnPackBy == UnPackByEnum.Carton)
           {

               Carton carton = (Carton)CurrentSession.GetValue(Session.SessionKeys.Carton);
               if (carton == null)
               {
                   throw new FisException("No Session Key [" + Session.SessionKeys.Carton + "] value");
               }

               carton.AddCartonLog(new CartonLog
               {
                   CartonNo = carton.CartonSN,
                   Line = Line,
                   Station = Station,
                   Status = (int)this.Status,
                   Editor = CurrentSession.Editor
               });


               cartonRep.Update(carton, CurrentSession.UnitOfWork);
           }
           else
           {
               string dnNo = (string)CurrentSession.GetValue(Session.SessionKeys.DeliveryNo);
               cartonRep.InsertCartonLogByDnDefered(CurrentSession.UnitOfWork, dnNo, Station, (int)Status, Line, CurrentSession.Editor);
           }
            return base.DoExecute(executionContext);
          
        }
	}
}
