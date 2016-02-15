// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 根据CSN1 BegNo和EndNo生产新的CSN2，并生成CSNMas记录 
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
    public partial class CreateCSNMas : BaseActivity
    {
        /// <summary>
        /// 
        /// </summary>
        public CreateCSNMas()
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

            //从Session里取得begNo和endNo
            string begNo = (string)CurrentSession.GetValue("BegNo");
            string endNo = (string)CurrentSession.GetValue("EndNo");
            string partNo = (string)CurrentSession.GetValue(Session.SessionKeys.PartNo);

            //TODO: 创建CSN2并生成CSNMas表条目            
            
            var currentRepository = RepositoryFactory.GetInstance().GetRepository<ICOAStatusRepository, COAStatus>();
            CSNMasInfo cond = new CSNMasInfo();
            //ITC-1360-139  no condition
            //cond.pno = partNo;
            string max = currentRepository.GetMaxCSN1FromCSNMas(cond);
            if (!string.IsNullOrEmpty(max) && max.Length == 19)
            {
                string max11 = max.Substring(0, 11);
                string max8 = max.Substring(11, 8);

                int beg = Convert.ToInt32(begNo.Substring(4, 10));
                int end = Convert.ToInt32(endNo.Substring(4, 10));

                string csn1 = max11 + (Convert.ToInt32(max8) + 1).ToString().PadLeft(8).Replace(' ', '0');
                for (long i = beg; i <= end; i++)
                {
                    string csn2 = begNo.Substring(0, 4) + i.ToString().PadLeft(10).Replace(' ', '0');
                    CSNMasInfo item = new CSNMasInfo();
                    item.pno = partNo;
                    item.csn1 = csn1;
                    item.csn2 = csn2;
                    item.status = "A0";
                    item.pdLine = "PAK";
                    item.editor = this.Editor;
                    //item.cdt = DateTime.Now;
                    //item.udt = DateTime.Now;
                    currentRepository.InsertCSNMas(item);
                }
            }
            else
            {
                FisException ex;
                List<string> erpara = new List<string>();
                ex = new FisException("CHK822", erpara);
                throw ex;
            }
        
            return base.DoExecute(executionContext);
        }
    }
}

