// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 判断CSN2的是否在指定的BegNo和EndNo之内 
//                   
// Known issues:
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
using IMES.FisObject.PAK.COA;
using IMES.DataModel;

namespace IMES.Activity
{
    /// <summary>
    /// 
    /// </summary>
    public partial class CheckCSN2InBegNoAndEndNo : BaseActivity
    {
        /// <summary>
        /// 
        /// </summary>
        public CheckCSN2InBegNoAndEndNo()
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
            var currentRepository = RepositoryFactory.GetInstance().GetRepository<ICOAStatusRepository, COAStatus>();
            //从Session里取得begNo和endNo
            string begNo = (string)CurrentSession.GetValue("BegNo");
            string endNo = (string)CurrentSession.GetValue("EndNo");


            IList<CSNMasInfo> list = currentRepository.GetCSNMasRange(begNo, endNo);
            //TODO: CSN2 是否在begNo和endNo之中,TestCode
            if(list != null && list.Count != 0)
            {
                List<string> errpara = new List<string>();                
                throw new FisException("CHK815", errpara);
            }

            return base.DoExecute(executionContext);
        }
    }
}

