// INVENTEC corporation (c)2011 all rights reserved. 
// Description: write Transaction Data log in table
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2016-02-05  Vincent
// Known issues:
using System;
using System.Workflow.ComponentModel;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.Generic;
using System.ComponentModel;
using IMES.FisObject.Common.Misc;
using IMES.DataModel;
using IMES.Infrastructure.Extend;
using IMES.Infrastructure.Repository._Metas;

namespace IMES.Activity
{
    /// <summary>
    /// WriteTxnDataLog
    /// </summary>
    public partial class WriteTxnDataLog : BaseActivity
    {
        ///<summary>
        ///WriteTxnDataLog
        ///</summary>
        public WriteTxnDataLog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// WriteTxnDataLog
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
           Session sesion = CurrentSession;
           DateTime now = DateTime.Now;
            var miscRep = RepositoryFactory.GetInstance().GetRepository<IMiscRepository>();
            string txnkey1 = sesion.GetValue<string>(ExtendSession.SessionKeys.TxnKeyValue1);
            string txnkey2 = sesion.GetValue<string>(ExtendSession.SessionKeys.TxnKeyValue2);
            string txnId = sesion.GetValue<string>(ExtendSession.SessionKeys.TxnId);
            string txnRawData = sesion.GetValue<string>(ExtendSession.SessionKeys.TxnRawData);
            string txnErrorCode = sesion.GetValue<string>(ExtendSession.SessionKeys.TxnErrorCode);
            string txnErrorDescr = sesion.GetValue<string>(ExtendSession.SessionKeys.TxnErrorDescr);

            TxnDataLogInfo log = new TxnDataLogInfo
            {
                Category = this.Category.ToString(),
                Action = this.Action,
                Cdt = now,
                Comment = txnRawData ?? string.Empty,
                ErrorCode = txnErrorCode ?? string.Empty,
                ErrorDescr = txnErrorDescr ?? string.Empty,
                KeyValue1 = txnkey1 ?? this.Key,
                KeyValue2 = txnkey2 ?? string.Empty,
                State = this.State.ToString(),
                TxnId = txnId ?? string.Empty
            };

            miscRep.InsertDataWithID<TxnDataLog, TxnDataLogInfo>(log);

            return base.DoExecute(executionContext);
        }

        public enum TxnCategoryEnum
        {
            Send=1,
            Receive,
            Response
        }

        public enum TxnStateEnum
        {
            Success = 1,
            Fail,
            Received
        }
        /// <summary>
        /// Category
        /// </summary>
        public static DependencyProperty CategoryProperty = DependencyProperty.Register("Category", typeof(TxnCategoryEnum), typeof(WriteTxnDataLog), new PropertyMetadata(TxnCategoryEnum.Receive));

        /// <summary>
        /// Category
        /// </summary>
        [DescriptionAttribute("Category")]
        [CategoryAttribute("InArugment Of Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public TxnCategoryEnum Category
        {
            get
            {
                return ((TxnCategoryEnum)(base.GetValue(CategoryProperty)));
            }
            set
            {
                base.SetValue(CategoryProperty, value);
            }
        }


        /// <summary>
        /// Action
        /// </summary>
        public static DependencyProperty ActionProperty = DependencyProperty.Register("Action", typeof(string), typeof(WriteTxnDataLog), new PropertyMetadata(string.Empty));

        /// <summary>
        /// Action
        /// </summary>
        [DescriptionAttribute("Action")]
        [CategoryAttribute("InArugment Of Action")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string Action
        {
            get
            {
                return ((string)(base.GetValue(ActionProperty)));
            }
            set
            {
                base.SetValue(ActionProperty, value);
            }
        }


        /// <summary>
        /// State
        /// </summary>
        public static DependencyProperty StateProperty = DependencyProperty.Register("State", typeof(TxnStateEnum), typeof(WriteTxnDataLog), new PropertyMetadata(TxnStateEnum.Success));

        /// <summary>
        /// Action
        /// </summary>
        [DescriptionAttribute("State")]
        [CategoryAttribute("InArugment Of State")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public TxnStateEnum State
        {
            get
            {
                return ((TxnStateEnum)(base.GetValue(StateProperty)));
            }
            set
            {
                base.SetValue(StateProperty, value);
            }
        }




       
    }
}
