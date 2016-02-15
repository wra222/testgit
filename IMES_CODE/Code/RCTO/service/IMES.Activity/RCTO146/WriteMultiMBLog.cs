// INVENTEC corporation (c)2011 all rights reserved. 
// Description: 根据输入的MaterialCT,获取Material对象，并放到Session中
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2014-03-01  Vincent
// Known issues:
using System;
using System.Linq;
using System.Workflow.ComponentModel;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.Generic;
using System.ComponentModel;
using IMES.FisObject.Common.Material;
using IMES.FisObject.PCA.MB;
using IMES.DataModel;
namespace IMES.Activity
{
   /// <summary>
    /// WriteMultiMBLog
   /// </summary>
    public partial class WriteMultiMBLog : BaseActivity
    {
        ///<summary>
        ///</summary>
        public WriteMultiMBLog()
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


            IMBRepository mbRep = RepositoryFactory.GetInstance().GetRepository<IMBRepository>();

            IList<string> mbSNList = null;
            IList<RCTO146MBInfo> mbInfoList = CurrentSession.GetValue(Session.SessionKeys.MBList) as IList<RCTO146MBInfo>;
            if (mbInfoList != null && mbInfoList.Count > 0)
            {
                mbSNList = (from p in mbInfoList
                            select p.PCBNo).ToList();

                mbRep.WritePCBLogByMultiMBDefered(CurrentSession.UnitOfWork, 
                                                                            mbSNList,
                                                                            this.Station,
                                                                            this.IsPass?1:0,
                                                                            this.Line,
                                                                            this.Editor);
              
                return base.DoExecute(executionContext);
            }


            mbSNList = CurrentSession.GetValue(Session.SessionKeys.MBSNOList) as IList<string>;
            if (mbSNList != null && mbSNList.Count > 0)
            {
                mbRep.WritePCBLogByMultiMBDefered(CurrentSession.UnitOfWork,
                                                                             mbSNList,
                                                                             this.Station,
                                                                             this.IsPass ? 1 : 0,
                                                                             this.Line,
                                                                             this.Editor);
            }         

           
            return base.DoExecute(executionContext);
        }






        ///<summary>
        /// 过站结果
        ///</summary>
        public static DependencyProperty IsPassProperty = DependencyProperty.Register("IsPass", typeof(bool), typeof(WriteMultiMBLog), new PropertyMetadata(true));

        ///<summary>
        /// 过站结果
        ///</summary>
        [DescriptionAttribute("IsPass")]
        [CategoryAttribute("IsPass Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public bool IsPass
        {
            get
            {
                return ((bool)(base.GetValue(WriteMultiMBLog.IsPassProperty)));
            }
            set
            {
                base.SetValue(WriteMultiMBLog.IsPassProperty, value);
            }
        }

       
    }
}
