// INVENTEC corporation (c)2011 all rights reserved. 
// Description: light off
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-12-06   itc207013               create
// Known issues:

using System;
using System.Collections.Generic;
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
using IMES.DataModel;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;
using IMES.FisObject.PAK.Pallet;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.Activity
{
    /// <summary>
    /// 
    /// </summary>
    public partial class LightOff : BaseActivity
    {
        /// <summary>
        /// 
        /// </summary>
        public LightOff()
        {
            InitializeComponent();
        }

        /// <summary>
        /// lightoff
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            IFlatBOM curBom = (IFlatBOM)CurrentSession.GetValue(Session.SessionKeys.SessionBom);
            string stationDescr = CurrentSession.GetValue(Session.SessionKeys.StationDescr) as string;

            IFlatBOMItem curMatchBomItem = curBom.CurrentMatchedBomItem;
            if (curMatchBomItem != null && curMatchBomItem.CheckedPart != null && curMatchBomItem.CheckedPart.Count > 0 && curMatchBomItem.Qty == curMatchBomItem.CheckedPart.Count)
            {
                IList<WipBuffer> wipbuffers = (IList<WipBuffer>)CurrentSession.GetValue(Session.SessionKeys.PizzaKitWipBuffer);

                foreach (WipBuffer wipbufobj in wipbuffers)
                {
                    if (PartMatchWipBuffer(wipbufobj.PartNo, curMatchBomItem.AlterParts))
                    {

                        IPalletRepository pltRepository = RepositoryFactory.GetInstance().GetRepository<IPalletRepository, Pallet>();
                        IList<KittingLocPLMappingStInfo> kitlocstinfos = pltRepository.GetKitLocPLMapST(Line.Substring(0, 1), stationDescr, short.Parse(wipbufobj.LightNo));
                        if (kitlocstinfos != null && kitlocstinfos.Count > 0)
                        {
                            //only one
                            pltRepository.UpdateKitLocationFVOffDefered(CurrentSession.UnitOfWork, kitlocstinfos[0].tagID, false, true);
                        }

                        break;
                    }
                }

            }
            return base.DoExecute(executionContext);
        }

        private bool PartMatchWipBuffer(string pn, IList<IPart> partlist)
        {
            //foreach (IPart part in partlist)
            //{
                //if (pn.Substring(0, 3).Equals("DIB"))
                //{
                //    pn = pn.Substring(3);
                //}
                if (pn == partlist[0].PN)
                {
                    return true;
                }
            //}
            return false;
        }
    }
}
