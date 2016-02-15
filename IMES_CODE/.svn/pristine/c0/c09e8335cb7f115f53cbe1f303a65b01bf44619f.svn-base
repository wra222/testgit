/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: PCARepairImpl
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2009-11-03   207006            Create 
 * 
 * 
 * Known issues:Any restrictions about this file 
 */


//在PrintLog中查找
//Start ProdId和End ProdId在BegNo和EndNo中范围内，且MO满足1的条件

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
using IMES.FisObject.FA.Product;
using IMES.FisObject.Common.MO;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.Station;
using System.Collections.Generic;
using IMES.Infrastructure;

namespace IMES.Activity
{
    //no need
    public partial class CheckTravelCardReprint : BaseActivity
    {
        public CheckTravelCardReprint()
        {
            InitializeComponent();
        }

        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            //放入web检查
            //IProductRepository prodRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();

            //if (!prodRepository.CheckTravelCardReprint(Session.GetValue(Session.SessionKeys.startProdId).ToString(), Session.GetValue(Session.SessionKeys.endProdId).ToString()))
            //{
               
            //    List<string> erpara = new List<string>();
            //    FisException ex = new FisException("PRD001", erpara);
            //    ex.stopWF = true;
            //    throw ex;
            //}
            return base.DoExecute(executionContext);
        }

    }
}
