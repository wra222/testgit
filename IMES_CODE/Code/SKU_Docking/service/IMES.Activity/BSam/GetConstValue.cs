// INVENTEC corporation (c)2011 all rights reserved. 
// Description: 
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2013-02-25   vincent          create
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
using IMES.FisObject.FA.Product;
using IMES.FisObject.PAK.Carton;
using IMES.FisObject.Common.Part;
using IMES.DataModel;
using System.Collections.Generic;

namespace IMES.Activity
{
    /// <summary>
    /// 
    /// </summary>
	public partial class GetConstValue: BaseActivity
	{
        /// <summary>
        /// 
        /// </summary>
        public GetConstValue()
		{
			InitializeComponent();
		}

        /// <summary>
        /// ConstValue Type
        /// </summary>
        public static DependencyProperty ConstValueTypeProperty = DependencyProperty.Register("ConstValueType", typeof(string), typeof(GetConstValue), new PropertyMetadata("BSamLabelLight"));

        /// <summary>
        /// ConstValue Type
        /// </summary>
        [DescriptionAttribute("ConstValueType")]
        [CategoryAttribute("InArguments Of GetConstValue")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string ConstValueType
        {
            get
            {
                return ((string)(base.GetValue(GetConstValue.ConstValueTypeProperty)));
            }
            set
            {
                base.SetValue(GetConstValue.ConstValueTypeProperty, value);
            }
        }

        /// <summary>
        /// ConstValue Name
        /// </summary>
        public static DependencyProperty ConstValueNameProperty = DependencyProperty.Register("ConstValueName", typeof(string), typeof(GetConstValue), new PropertyMetadata("LabelCode"));

        /// <summary>
        /// ConstValue Name
        /// </summary>
        [DescriptionAttribute("ConstValueName")]
        [CategoryAttribute("InArguments Of GetConstValue")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string ConstValueName
        {
            get
            {
                return ((string)(base.GetValue(GetConstValue.ConstValueNameProperty)));
            }
            set
            {
                base.SetValue(GetConstValue.ConstValueNameProperty, value);
            }
        }

        /// <summary>
        /// Set SessionKey
        /// </summary>
        public static DependencyProperty AddSessionKeyProperty = DependencyProperty.Register("AddSessionKey", typeof(string), typeof(GetConstValue), new PropertyMetadata("MBCode"));

        /// <summary>
        /// ConstValue Name
        /// </summary>
        [DescriptionAttribute("AddSessionKey")]
        [CategoryAttribute("InArguments Of GetConstValue")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string AddSessionKey
        {
            get
            {
                return ((string)(base.GetValue(GetConstValue.AddSessionKeyProperty)));
            }
            set
            {
                base.SetValue(GetConstValue.AddSessionKeyProperty, value);
            }
        }
        /// <summary>
        /// 禁用時要停止workflow
        /// </summary>
        public static DependencyProperty IsStopWFProperty = DependencyProperty.Register("IsStopWF", typeof(bool), typeof(GetConstValue), new PropertyMetadata(true));
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
                return ((bool)(base.GetValue(GetConstValue.IsStopWFProperty)));
            }
            set
            {
                base.SetValue(GetConstValue.IsStopWFProperty, value);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            
            IPartRepository partRep = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            IList<ConstValueInfo> constValueList = CurrentSession.GetValue(this.ConstValueType) as IList<ConstValueInfo>;
            if (constValueList == null || constValueList.Count == 0)
            {
                constValueList = partRep.GetConstValueListByType(this.ConstValueType);
                CurrentSession.AddValue(this.ConstValueType, constValueList);
            }

            var data = (from p in constValueList
                        where p.name.Trim() == this.ConstValueName
                        select p.value.Trim()).ToList();

            if (data.Count == 0 || string.IsNullOrEmpty(data[0]))
            {
                FisException e = new FisException("CHK990", new string[] { "ConstValue Table", this.ConstValueType, "Name:"+this.ConstValueName });
                e.stopWF = this.IsStopWF;
                throw e;
            }
            else
            {
                CurrentSession.AddValue(this.AddSessionKey, data[0]);
            }           
            return base.DoExecute(executionContext);

        }
	}
}
