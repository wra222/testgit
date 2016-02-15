using System;
using System.Workflow.ComponentModel;
using IMES.FisObject.Common.MO;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.PCA.MBMO;

namespace IMES.Activity
{
    /// <summary>
    /// Update MO_Excel by TravelCardPrint Excel
    /// </summary>
    public partial class UpdateMO_Excel : BaseActivity
    {
        private static Object _syncObj = new Object();

        /// <summary>
        /// </summary>
        public UpdateMO_Excel()
        {
            InitializeComponent();
        }

        /// <summary>
        /// </summary>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            //var mo = (MO)CurrentSession.GetValue(Session.SessionKeys.ProdMO);
            object Qty = CurrentSession.GetValue(Session.SessionKeys.Qty);
            string modelName = (string)CurrentSession.GetValue(Session.SessionKeys.ModelName);

            short IncreasePrintedQty = (short)0;
            if (Qty != null)
            {
                short.TryParse(Qty.ToString(), out IncreasePrintedQty);
            }
            else
            {
                IncreasePrintedQty = (short)0;
            }

            lock (_syncObj)
            {
                if (IncreasePrintedQty != (short)0)
                {
                    IMBMORepository myRepository = RepositoryFactory.GetInstance().GetRepository<IMBMORepository, IMBMO>();
                    myRepository.IncreasePrintQtyForMoExcelDefered(CurrentSession.UnitOfWork, IncreasePrintedQty, this.Line.Substring(0, 1), modelName);
                    //mo.PrtQty = (short)(mo.PrtQty + IncreasePrintedQty);
                    //IMORepository moRepository = RepositoryFactory.GetInstance().GetRepository<IMORepository, MO>();
                    //moRepository.Update(mo, CurrentSession.UnitOfWork);
                }
            }

            return base.DoExecute(executionContext);
        }
    }
}
