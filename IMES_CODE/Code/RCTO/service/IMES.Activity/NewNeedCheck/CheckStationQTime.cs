/*
 * INVENTEC corporation (c)2011 all rights reserved. 
 * Description: Check Station QTime
 * Update: 
 * Date                 Name               Reason 
 * ==================================================================
 * 2011-09-08      Vincent Lee      Create 
 * 
 * 
 * Known issues:Any restrictions about this file 
 */

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
using IMES.FisObject.PCA.MB;
using IMES.FisObject.Common.MO;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.Station;

namespace IMES.Activity
{
    /// <summary>
    /// CheckStationQTime
    /// </summary>
    public partial class CheckStationQTime : BaseActivity
    {
        /// <summary>
        /// InitializeComponent
        /// </summary>
        public CheckStationQTime()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Set ModelInfo name for setting QTime
        /// </summary>
        public static DependencyProperty ModelQTimeNameProperty = DependencyProperty.Register("ModelQTimeName", typeof(string), typeof(CheckStationQTime));

        /// <summary>
        /// ModelQTimeName
        /// </summary>
        [DescriptionAttribute("ModelQTimeName")]
        [CategoryAttribute("Model Info QTimeName")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string ModelQTimeName
        {
            get
            {
                return ((string)(base.GetValue(CheckStationQTime.ModelQTimeNameProperty)));
            }
            set
            {
                base.SetValue(CheckStationQTime.ModelQTimeNameProperty, value);
            }
        }
        /// <summary>
        /// ShopCategory
        /// </summary>
        public enum ShopCategory
        {
            /// <summary>
            /// Product
            /// </summary>
            Product,
            /// <summary>
            /// MB
            /// </summary>
            MB
        }
        /// <summary>
        /// ShopProperty
        /// </summary>
        public static DependencyProperty ShopProperty = DependencyProperty.Register("Shop", typeof(ShopCategory), typeof(CheckStationQTime));
        /// <summary>
        /// Shop
        /// </summary>
        [DescriptionAttribute("Shop")]
        [CategoryAttribute("Shop name")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
      
        public ShopCategory Shop
        {
           
            get
            {
                return ((ShopCategory)(base.GetValue(CheckStationQTime.ShopProperty)));
            }
            set
            {
                base.SetValue(CheckStationQTime.ShopProperty, value);
            }
        }
        /// <summary>
        /// DoExecute
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
           
            string QTimeName="StationMinQTime";
            if (!string.IsNullOrEmpty(ModelQTimeName))
            {
                QTimeName = ModelQTimeName;            
            }

            Product product = null;
            MB mb = null;
            string strLimitTime;
            if (Shop == ShopCategory.Product)
            {
                product = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);
                strLimitTime = product.ModelObj.GetAttribute(QTimeName);
            }
            else
            {
                mb = (MB)CurrentSession.GetValue(Session.SessionKeys.MB);
                strLimitTime = mb.Model1397Obj.GetAttribute(QTimeName);
            }

            int LimitTime=0;
            if (!string.IsNullOrEmpty(strLimitTime))
            {
                LimitTime = int.Parse(strLimitTime);
            }
            DateTime startTime = product.Status.Udt;

            int StationWaitTime =(int) (DateTime.Now - startTime).TotalMinutes;

            if (StationWaitTime < LimitTime)
            {
                List<string> errpara = new List<string>();

                errpara.Add(this.Station);
                errpara.Add(LimitTime.ToString());
                errpara.Add(StationWaitTime.ToString());
                throw new FisException("CHK180", errpara);
            }

           

            return base.DoExecute(executionContext);
        }
    }
}