
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
using IMES.DataModel;
using IMES.FisObject.Common.Model;

namespace IMES.Activity
{
    /// <summary>
    ///
    /// </summary>
    public partial class CheckPrintForChangeAST : BaseActivity
    {
        /// <summary>
        /// 
        /// </summary>
        public CheckPrintForChangeAST()
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

            CurrentSession.AddValue(Session.SessionKeys.InfoValue, 0);
            ASTInfo info = new ASTInfo();
            info = (ASTInfo)CurrentSession.GetValue(Session.SessionKeys.ASTInfoList);

            if (info.ASTType == "ATSN3" || info.ASTType == "ATSN5" || info.ASTType == "ATSN7")
            {
                IModelRepository modelRep = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();
                string model = (string)CurrentSession.GetValue(Session.SessionKeys.Model2);
                IList<IMES.FisObject.Common.Model.ModelInfo> infos = new List<IMES.FisObject.Common.Model.ModelInfo>();
                infos = modelRep.GetModelInfoByModelAndName(model, "ATSNAV");
                bool bFind = false;
                foreach (IMES.FisObject.Common.Model.ModelInfo temp in infos)
                {
                    if (temp.Value != "")
                    {
                        bFind = true;
                        break;
                    }
                }

                if (bFind == false)
                {
                    CurrentSession.AddValue(Session.SessionKeys.InfoValue, 1);
                }
            }

            return base.DoExecute(executionContext);
        }
    }
}

