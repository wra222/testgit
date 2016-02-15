// INVENTEC corporation (c)2011 all rights reserved. 
// Description: Update FwdPlt Set Status = 'Out', Operator = @Editor, Udt = GETDATE()
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-11-02   Kerwin                       create
// Known issues:
using System;
using System.Workflow.ComponentModel;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.Generic;
using IMES.FisObject.PAK.Pallet;
using IMES.DataModel;
using System.ComponentModel;
namespace IMES.Activity
{
    /// <summary>
    /// Check Zero Pallet weight
    /// </summary>
    public partial class CheckPltZeroWeight : BaseActivity
    {
        ///<summary>
        /// constructor
        ///</summary>
        public CheckPltZeroWeight()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 禁用時要停止workflow
        /// </summary>
        public static DependencyProperty IsStopWFProperty = DependencyProperty.Register("IsStopWF", typeof(bool), typeof(CheckPltZeroWeight), new PropertyMetadata(false));
        /// <summary>
        /// 禁用時要停止workflow 
        /// </summary>
        [DescriptionAttribute("IsStopWF")]
        [CategoryAttribute("IsStopWF")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public bool IsStopWF
        {
            get
            {
                return ((bool)(base.GetValue(IsStopWFProperty)));
            }
            set
            {
                base.SetValue(IsStopWFProperty, value);
            }
        }

     /// <summary>
        /// Check Zero Pallet weight
     /// </summary>
     /// <param name="executionContext"></param>
     /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
           Session session =CurrentSession;
           Pallet pallet = (Pallet)session.GetValue(Session.SessionKeys.Pallet);
           string pltNo = (string)session.GetValue(Session.SessionKeys.PalletNo)??"";
           var palletRep = RepositoryFactory.GetInstance().GetRepository<IPalletRepository, Pallet>();
           if (pallet == null)
           {
               
               if (!string.IsNullOrEmpty(pltNo))
               {
                 pallet = palletRep.Find(pltNo);                 
               }
           }

           if (pallet == null)
           {
               throw new FisException("CHK106", this.IsStopWF, new string[] { pltNo }); 
           }

           if (pallet.PalletModel == "Docking")
           {
               pallet = palletRep.FindWithDocking(pltNo);

               if (pallet == null)
               {
                   throw new FisException("CHK106", this.IsStopWF, new string[] { pltNo });
               }
           }

           if (pallet != null && pallet.Weight <= 0 || pallet.Weight_L <= 0)
           {
               throw new FisException("CQCHK1095", this.IsStopWF, new string[] { pltNo });
           }
            return base.DoExecute(executionContext);
        }
    }
}
