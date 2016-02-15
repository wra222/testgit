// INVENTEC corporation (c)2011 all rights reserved. 
// Description: 
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
//2014-06-06    Vincent           release
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
using IMES.FisObject.FA.Product;
using IMES.FisObject.PAK.BSam;
using IMES.Infrastructure.Extend;
using System.Collections.Generic;
using IMES.FisObject.PCA.MB;
using IMES.FisObject.Common.MO;
using IMES.DataModel;


namespace IMES.Activity
{

    /// <summary>
    /// Check PilotMo State
    /// </summary>  
    public partial class CheckPilotMoState : BaseActivity
	{
        /// <summary>
        /// 
        /// </summary>
        public CheckPilotMoState()
		{
			InitializeComponent();
		}

        /// <summary>
        /// 禁用時要停止workflow
        /// </summary>
        public static DependencyProperty IsStopWFProperty = DependencyProperty.Register("IsStopWF", typeof(bool), typeof(CheckPilotMoState), new PropertyMetadata(true));
        /// <summary>
        ///  禁用時要停止workflow
        /// </summary>
        [DescriptionAttribute("IsStopWF")]
        [CategoryAttribute("IsStopWF")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public bool IsStopWF
        {
            get
            {
                return ((bool)(base.GetValue(CheckPilotMoState.IsStopWFProperty)));
            }
            set
            {
                base.SetValue(CheckPilotMoState.IsStopWFProperty, value);
            }
        }

        /// <summary>
        ///  Stage
        /// </summary>
        public static DependencyProperty StageProperty = DependencyProperty.Register("Stage", typeof(StageEnum), typeof(CheckPilotMoState), new PropertyMetadata(StageEnum.FA));
        /// <summary>
        /// Stage
        /// </summary>
        [DescriptionAttribute("Stage")]
        [CategoryAttribute("Stage")]
        [BrowsableAttribute(true)]        
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public StageEnum Stage
        {
            get
            {
                return ((StageEnum)(base.GetValue(CheckPilotMoState.StageProperty)));
            }
            set
            {
                base.SetValue(CheckPilotMoState.StageProperty, value);
            }
        }

        /// <summary>
        ///  Stage
        /// </summary>
        public static DependencyProperty AllowPilotMoStateProperty = DependencyProperty.Register("AllowPilotMoState", typeof(PilotMoStateEnum), typeof(CheckPilotMoState), new PropertyMetadata(PilotMoStateEnum.Release));
        /// <summary>
        /// Stage
        /// </summary>
        [DescriptionAttribute("AllowPilotMoState")]
        [CategoryAttribute("AllowPilotMoState")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public PilotMoStateEnum AllowPilotMoState
        {
            get
            {
                return ((PilotMoStateEnum)(base.GetValue(CheckPilotMoState.AllowPilotMoStateProperty)));
            }
            set
            {
                base.SetValue(CheckPilotMoState.AllowPilotMoStateProperty, value);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            string pilotMo = "";
            if (Stage == StageEnum.MB)
            {
                IMB mb = (IMB)CurrentSession.GetValue(Session.SessionKeys.MB);
                if (mb == null)
                {
                    FisException e = new FisException("CHK975", new string[] { Session.SessionKeys.MB });
                    e.stopWF = this.IsStopWF;
                    throw e;
                }
                pilotMo = (string)mb.GetExtendedProperty("PilotMo"); 
            }
            else
            {
                Product prod = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);                
                if (prod == null)
                {
                    FisException e = new FisException("CHK975", new string[] { Session.SessionKeys.Product });
                    e.stopWF = this.IsStopWF;
                    throw e;
                }
                pilotMo = (string)prod.GetExtendedProperty("PilotMo");
            }

            if (!string.IsNullOrEmpty(pilotMo))
            {
                IMORepository moRep = RepositoryFactory.GetInstance().GetRepository<IMORepository>();
                PilotMoInfo moInfo = moRep.GetPilotMo(pilotMo);
                if (moInfo == null)
                {
                    FisException ex = new FisException("CHK1095", new string[] { pilotMo });
                    ex.stopWF = this.IsStopWF;
                    throw ex;
                }
                else if (moInfo.state != this.AllowPilotMoState.ToString())
                {
                    FisException ex = new FisException("CHK1096", new string[] { pilotMo, moInfo.state });
                    ex.stopWF = this.IsStopWF;
                    throw ex;
                }
            }
            
            return base.DoExecute(executionContext);          
        }
	}
    /// <summary>
    /// Stage list 
    /// </summary>
    public enum StageEnum
    {
        /// <summary>
        /// PCB Object
        /// </summary>
        MB=1,
        /// <summary>
        /// Product object
        /// </summary>
        FA
    }
}
