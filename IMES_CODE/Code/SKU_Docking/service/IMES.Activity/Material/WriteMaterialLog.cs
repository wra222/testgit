// INVENTEC corporation (c)2011 all rights reserved. 
// Description: 根据输入的MaterialCT,获取Material对象，并放到Session中
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2014-03-01  Vincent
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
    public partial class WriteMaterialLog : BaseActivity
    {
        ///<summary>
        ///</summary>
        public WriteMaterialLog()
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
           

            var materialRep = RepositoryFactory.GetInstance().GetRepository<IMaterialRepository>();
            if (IsSingleCT)
            {
                 string materialCT =(string)CurrentSession.GetValue(Session.SessionKeys.MaterialCT);
                if (materialCT==null)
                {
                    throw new FisException("CQCHK0006", new string[] { Session.SessionKeys.MaterialCT });
                }
                Material material = materialRep.Find(materialCT);
                if (material == null)
                {
                    throw new FisException("CQCHK0011", new string[] { material.MaterialCT });
                }

                material.AddMaterialLog(new MaterialLog()
                { Action =this.LogActionName,
                     Line  = this.Line,
                     PreStatus = material.Status,
                     Status =this.Station,
                     Comment="",
                     Editor =this.Editor,
                     Cdt=DateTime.Now,
                     MaterialCT=material.MaterialCT,
                      Stage=material.Stage
                });
                materialRep.Update(material, CurrentSession.UnitOfWork);
            }
            else
            {
                IList<string> materialCTList = (IList<string>)CurrentSession.GetValue(Session.SessionKeys.MaterialCTList);
                if (materialCTList == null || materialCTList.Count == 0)
                {
                    throw new FisException("CQCHK0006", new string[] { Session.SessionKeys.MaterialCTList });
                }

                Material material = materialRep.Find(materialCTList[0]);
                string materialStage = (string)CurrentSession.GetValue("MaterialStage") ?? "";
                       materialRep.AddMultiMaterialLogDefered(CurrentSession.UnitOfWork,
                                                                                materialCTList,
                                                                                 this.LogActionName,
                                                                                 material == null ? materialStage : material.Stage,
                                                                                 this.Line,
                                                                                 material==null? "":material.Status??"",
                                                                                 this.Station,
                                                                                 "",
                                                                                 this.Editor);
            }
          

           
            return base.DoExecute(executionContext);
        }

       

      
        /// <summary>
        /// 输入的类型:ProductID,CustSN,ProductIDOrCustSN
        /// </summary>
        public static DependencyProperty IsSingleCTProperty = DependencyProperty.Register("IsSingleCT", typeof(bool), typeof(WriteMaterialLog), new PropertyMetadata(true));

        /// <summary>
        /// 输入的类型:ProductID,CustSN,ProductIDOrCustSN
        /// </summary>
        [DescriptionAttribute("IsSingleCT")]
        [CategoryAttribute("InArugment Of GetMaterial")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public bool IsSingleCT
        {
            get
            {
                return ((bool)(base.GetValue(WriteMaterialLog.IsSingleCTProperty)));
            }
            set
            {
                base.SetValue(WriteMaterialLog.IsSingleCTProperty, value);
            }
        }

        /// <summary>
        /// LogActionName
        /// </summary>
        public static DependencyProperty LogActionNameProperty = DependencyProperty.Register("LogActionName", typeof(string), typeof(WriteMaterialLog), new PropertyMetadata(""));

        /// <summary>
        /// LogActionName
        /// </summary>
        [DescriptionAttribute("LogActionName")]
        [CategoryAttribute("InArugment Of WriteMaterialLog")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string LogActionName
        {
            get
            {
                return ((string)(base.GetValue(WriteMaterialLog.LogActionNameProperty)));
            }
            set
            {
                base.SetValue(WriteMaterialLog.LogActionNameProperty, value);
            }
        }

       
    }
}
