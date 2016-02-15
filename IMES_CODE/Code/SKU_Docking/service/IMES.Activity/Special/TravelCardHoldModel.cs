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

using IMES.FisObject.Common.PartSn;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure;
using IMES.FisObject.Common.MO;
using IMES.FisObject.FA.Product;
using System.Collections.Generic;
using IMES.FisObject.Common.Part;
using System.Text.RegularExpressions;
using IMES.Common;
namespace IMES.Activity
{
    /// <summary>
    /// Check CTO Bom for IPC
    /// </summary>
    public partial class TravelCardHoldModel : BaseActivity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public TravelCardHoldModel()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Check CTO Bom
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            Session session = CurrentSession;
            ActivityCommonImpl utl = ActivityCommonImpl.Instance;
            //IPartRepository ipartRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            //IProductRepository prodRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            string model = "";

            MO mo = utl.IsNull<MO>(session, Session.SessionKeys.ProdMO);
            //MO mo = (MO)CurrentSession.GetValue(Session.SessionKeys.ProdMO);
            //if (mo == null)
            //{
            //    throw new FisException("CHK1025");
            //}
            model = mo.Model;

            //IList<IMES.DataModel.ConstValueTypeInfo> holdmodellist = ipartRepository.GetConstValueTypeList("TravelCardHoldModel");
            IList<IMES.DataModel.ConstValueTypeInfo> holdmodellist = utl.GetConstValueTypeByType("TravelCardHoldModel");
                
            // if (holdmodellist == null)
            //{
            //    throw new FisException("存取ConstValueType.TravelCardHoldModel失敗，請重新嘗試...");
            //}
            
             if ( holdmodellist!=null && 
                 holdmodellist.Count > 0)
             {
                 bool travelcardhold = false;
                 string holddescr = "";
                  IList<IMES.DataModel.ConstValueTypeInfo> matchmodel= holdmodellist.Where(x => Regex.IsMatch(model, x.value)).ToList();
                  if (matchmodel != null && matchmodel.Count > 0)
                  {
                      travelcardhold = true;
                      holddescr = matchmodel[0].description;
                  
                  }
                 if (travelcardhold)
                 {
                     string[] ErrorMsgValue = { model, holddescr };
                     throw new FisException("CHK1051", ErrorMsgValue);
                 }
             }
           


            //IList<IMES.DataModel.ConstValueTypeInfo> List = ipartRepository.GetConstValueTypeList("TravelCardHoldModel", model);
            //if (List == null)
            //{
            //    throw new FisException("存取ConstValueType.TravelCardHoldModel失敗，請重新嘗試...");
            //}
            //if (List.Count != 0)
            //{

            //    string[] ErrorMsgValue = { model,List[0].description };
            //    throw new FisException("CHK1051", ErrorMsgValue);
            //}
            return base.DoExecute(executionContext);
        }
    }
}
