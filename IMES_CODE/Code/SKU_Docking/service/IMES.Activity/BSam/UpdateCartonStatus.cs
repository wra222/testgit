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
using IMES.FisObject.PAK.Carton;
using IMES.FisObject.FA.Product;
using System.Collections.Generic;
using IMES.FisObject.Common.Station;

namespace IMES.Activity
{



    /// <summary>
    ///Update Carton Status
    /// </summary>
  
    public partial class UpdateCartonStatus : BaseActivity
	{
        /// <summary>
        /// 
        /// </summary>
        public UpdateCartonStatus()
		{
			InitializeComponent();
		}

        /// <summary>
        /// UnPackDNByXXX        
        /// </summary>
        public static DependencyProperty UnPackByProperty = DependencyProperty.Register("UnPackBy", typeof(UnPackByEnum), typeof(UpdateCartonStatus), new PropertyMetadata(UnPackByEnum.Carton));

        /// <summary>
        /// UnPackDNByXXX
        /// </summary>
        [DescriptionAttribute("UnPackBy")]
        [CategoryAttribute("InArguments Of UnPackBackupProductByCarton")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public UnPackByEnum UnPackBy
        {
            get
            {
                return ((UnPackByEnum)(base.GetValue(UpdateCartonStatus.UnPackByProperty)));
            }
            set
            {
                base.SetValue(UpdateCartonStatus.UnPackByProperty, value);
            }
        }

        /// <summary>
        /// Activtiy所在Status
        /// </summary>
        public static DependencyProperty StatusProperty = DependencyProperty.Register("Status", typeof(StationStatus), typeof(UpdateCartonStatus), new PropertyMetadata(StationStatus.Pass));

        /// <summary>
        /// Status of Product
        /// </summary>
        [DescriptionAttribute("Status")]
        [CategoryAttribute("Status Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public StationStatus Status
        {
            get
            {
                return ((StationStatus)(base.GetValue(UpdateCartonStatus.StatusProperty)));
            }
            set
            {
                base.SetValue(UpdateCartonStatus.StatusProperty, value);
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


                carton.PreStation = carton.CurrentStation.Station;
                carton.PreStationStatus = carton.CurrentStation.Status;

                carton.CurrentStation.Station = this.Station;
                carton.CurrentStation.Line = CurrentSession.Line;
                carton.CurrentStation.Status = (int)Status;
                carton.CurrentStation.Editor = CurrentSession.Editor;

                
                cartonRep.Update(carton, CurrentSession.UnitOfWork);
            }
            else
            {
                 string dnNo = (string)CurrentSession.GetValue(Session.SessionKeys.DeliveryNo);
                 cartonRep.UpdateCartonPreStationByDnDefered( CurrentSession.UnitOfWork,dnNo, CurrentSession.Editor);
                 cartonRep.UpdateCartonStatusByDnDefered(CurrentSession.UnitOfWork, dnNo, this.Station, (int)Status, this.Line, CurrentSession.Editor); 

            }
            return base.DoExecute(executionContext);
          
        }
	}
}
