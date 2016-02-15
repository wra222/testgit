// INVENTEC corporation (c)2011 all rights reserved. 
// Description: 根据输入的MaterialCT,放到Session中
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2014-03-01  Vic
// Known issues:
using System;
using System.Workflow.ComponentModel;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.Generic;
using System.ComponentModel;
using IMES.FisObject.Common.Material;
namespace IMES.Activity
{
    /// <summary>
    /// 
    /// </summary>
    public partial class SaveMaterialCTList : BaseActivity
    {
        ///<summary>
        ///</summary>
        public SaveMaterialCTList()
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
            string materialCT = (string)CurrentSession.GetValue(Session.SessionKeys.MaterialCT);
            if (materialCT == null)
            {
                throw new FisException("CQCHK0006", new string[] { Session.SessionKeys.MaterialCT });
            }
            IList<string> materialCTList = (IList<string>)CurrentSession.GetValue(Session.SessionKeys.MaterialCTList);
            materialCTList.Add(materialCT);
            CurrentSession.AddValue(Session.SessionKeys.MaterialCTList, materialCTList);
            return base.DoExecute(executionContext);
        }
    }
}
