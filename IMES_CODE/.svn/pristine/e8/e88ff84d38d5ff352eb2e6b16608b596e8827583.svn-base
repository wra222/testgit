// INVENTEC corporation (c)2011 all rights reserved. 
// Description: 
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// Known issues:
using System;
using System.Workflow.ComponentModel;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.Generic;
using System.ComponentModel;
using IMES.FisObject.Common.Material;
using IMES.FisObject.Common.Model;
using IMES.FisObject.PAK.StandardWeight;
using IMES.FisObject.PAK.DN;
using IMES.FisObject.PCA.MB;
namespace IMES.Activity
{
    /// <summary>
    /// UpdateMbForCombineCarton146Mb
    /// </summary>
    public partial class UpdateMbForCombineCarton146Mb : BaseActivity
    {
        ///<summary>
        ///</summary>
        public UpdateMbForCombineCarton146Mb()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Get Product Object and Put it into Session.SessionKeys.Product
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            IMBRepository mbRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();

            string fruNo = (string)CurrentSession.GetValue(Session.SessionKeys.FRUNO) ?? "";

            IList<string> mbSNList = null;
            mbSNList = CurrentSession.GetValue(Session.SessionKeys.MBSNOList) as IList<string>;
            if (mbSNList != null && mbSNList.Count > 0)
            {
                if (!string.IsNullOrEmpty(fruNo))
                {
                    IList<IMES.FisObject.PCA.MB.MBInfo> lstMI = new List<IMES.FisObject.PCA.MB.MBInfo>();

                    foreach (string mbsn in mbSNList)
                    {
                        IMES.FisObject.PCA.MB.MBInfo mi = new IMES.FisObject.PCA.MB.MBInfo(0, mbsn, "FRUNO", fruNo, this.Editor, DateTime.Now, DateTime.Now);
                        lstMI.Add(mi);
                    }

                    mbRepository.AddMBInfoesDefered(CurrentSession.UnitOfWork, lstMI);
                }
            }

            return base.DoExecute(executionContext);
        }        
       
    }
}
