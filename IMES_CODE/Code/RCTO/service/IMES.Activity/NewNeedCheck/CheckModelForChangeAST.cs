
using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PAK.DN;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.FisBOM;
using IMES.DataModel;

namespace IMES.Activity
{
    /// <summary>
    ///
    /// </summary>
    public partial class CheckModelForChangeAST : BaseActivity
    {
        /// <summary>
        /// 
        /// </summary>
        public CheckModelForChangeAST()
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

            string model1 = (string)CurrentSession.GetValue(Session.SessionKeys.Model1);
            string model2 = (string)CurrentSession.GetValue(Session.SessionKeys.Model2);

            ASTInfo info = (ASTInfo)CurrentSession.GetValue(Session.SessionKeys.ASTInfoList);

            if (model1 != model2)
            {
                IBOMRepository ibomRepository = RepositoryFactory.GetInstance().GetRepository<IMES.FisObject.Common.FisBOM.IBOMRepository>();

                MoBOMInfo cond = new MoBOMInfo();
                cond.material = model2;
                cond.component = info.PartNo;
                cond.flag = 1;
                IList<MoBOMInfo> list = new List<MoBOMInfo>();
                list = ibomRepository.GetModelBomList(cond);
                if (list == null || list.Count == 0)
                {
                    FisException ex;
                    List<string> erpara = new List<string>();
                    ex = new FisException("CHK895", erpara);
                    throw ex;
                }
            }
            
            return base.DoExecute(executionContext);
        }
    }
}

