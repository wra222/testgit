/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: check if start mbsnno and end mbsnno has the same mo
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2009-12-22   207013     Create 
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
using IMES.FisObject.FA.Product ;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.Generic;
using IMES.FisObject.Common.Station;

namespace IMES.Activity
{
    public partial class CheckForRework : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CheckForRework()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 检查该unit是否可以做Dismantle
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {            
            IProduct currentProduct = (IProduct)CurrentSession.GetValue(Session.SessionKeys.Product);
            
            //4.检查该unit是否可以做Dismantle	Unit所在的站满足以下条件：Product.Station and Status不存在于ReworkRejectStation表
            //B.	若对应的Product不能做Dismantle时，提示”该Product当前的站别是” + wc# + ”，不能做Dismantle”
            if (!currentProduct.IsAvailableForRework)
            {
                FisException ex;
                List<string> erpara = new List<string>();
                erpara.Add(currentProduct.Status.StationId);
                IStationRepository stationRepository = RepositoryFactory.GetInstance().GetRepository<IStationRepository, IStation>();
                IStation curStation = stationRepository.Find(this.Station);
                erpara.Add(curStation.Descr);
                ex = new FisException("CHK108", erpara);
                throw ex;
            }
           
            return base.DoExecute(executionContext);
        }
    }
}
