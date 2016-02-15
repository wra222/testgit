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
    public partial class GetConstValueType : BaseActivity
    {
        /// <summary>
        /// 
        /// </summary>
        public GetConstValueType()
        {
            InitializeComponent();
        }

        /// <summary>
        /// ConstValue Type
        /// </summary>
        public static DependencyProperty ConstValueTypeProperty = DependencyProperty.Register("ConstValueType", typeof(string), typeof(GetConstValueType), new PropertyMetadata(""));

        /// <summary>
        /// ConstValue Type
        /// </summary>
        [DescriptionAttribute("ConstValueType")]
        [CategoryAttribute("InArguments Of GetConstValueType")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string ConstValueType
        {
            get
            {
                return ((string)(base.GetValue(GetConstValueType.ConstValueTypeProperty)));
            }
            set
            {
                base.SetValue(GetConstValueType.ConstValueTypeProperty, value);
            }
        }

       

        /// <summary>
        /// Set SessionKey
        /// </summary>
        public static DependencyProperty AddSessionKeyProperty = DependencyProperty.Register("AddSessionKey", typeof(string), typeof(GetConstValueType), new PropertyMetadata(""));

        /// <summary>
        /// ConstValue Name
        /// </summary>
        [DescriptionAttribute("AddSessionKey")]
        [CategoryAttribute("InArguments Of GetConstValueType")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string AddSessionKey
        {
            get
            {
                return ((string)(base.GetValue(GetConstValueType.AddSessionKeyProperty)));
            }
            set
            {
                base.SetValue(GetConstValueType.AddSessionKeyProperty, value);
            }
        }
        /// <summary>
        /// 禁用時要停止workflow
        /// </summary>
        public static DependencyProperty IsStopWFProperty = DependencyProperty.Register("IsStopWF", typeof(bool), typeof(GetConstValueType), new PropertyMetadata(true));
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
                return ((bool)(base.GetValue(GetConstValueType.IsStopWFProperty)));
            }
            set
            {
                base.SetValue(GetConstValueType.IsStopWFProperty, value);
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
            IList<ConstValueTypeInfo> constValueTypeList = CurrentSession.GetValue(this.ConstValueType) as IList<ConstValueTypeInfo>;
            if (constValueTypeList == null || constValueTypeList.Count == 0)
            {
                constValueTypeList = partRep.GetConstValueTypeList(this.ConstValueType).Where(x => x.value.Trim() != "").ToList();
                CurrentSession.AddValue(this.ConstValueType, constValueTypeList);
            }

            var data = (from p in constValueTypeList
                              where p.type.Trim() == this.ConstValueType
                               select p.value.Trim()).ToList();

            if (data.Count == 0)
            {
                FisException e = new FisException("CHK990", new string[] {"ConstValueType Table", this.ConstValueType, "" });
                e.stopWF = this.IsStopWF;
                throw e;
            }
            else
            {
                CurrentSession.AddValue(this.AddSessionKey, data);
            }
            return base.DoExecute(executionContext);

        }
    }
}

