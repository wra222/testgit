using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using IMES.FisObject.PAK.Carton;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.PAK.DN;

namespace IMES.Activity
{
    /// <summary>
    /// Handle Session TimeOut Class
    /// </summary>
    public sealed class HandleRollBack
    {
        private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly ICartonRepository cartonRep = RepositoryFactory.GetInstance().GetRepository<ICartonRepository, Carton>();
        private static readonly IDeliveryRepository deliveryRep = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="workflowName"></param>
        /// <param name="session"></param>
        /// <param name="key"></param>
        /// <param name="line"></param>
        /// <param name="editor"></param>
        /// <param name="customer"></param>
        public static void Execute(string workflowName, Session session, string key, string line, string editor, string customer)
        {
            string methodName = "HandleRollBackExecute";
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                if (session == null)
                {
                    return;
                }
                //if (workflowName== "CombineCartonInDN" ||
                //    workflowName == "CombineCartonInDN_BIRCH"||
                //    workflowName == "CombineCartonInDN_JR"||
                //    workflowName == "CombineCartonInDN_Skoda") 
                if (workflowName.StartsWith("CombineCartonInDN")) //Tablet Combine Carton In Delivery
                {
                    Carton carton = (Carton)session.GetValue(Session.SessionKeys.Carton);
                    if (carton != null)
                    {
                        //ICartonRepository cartonRep = RepositoryFactory.GetInstance().GetRepository<ICartonRepository, Carton>();
                        //IDeliveryRepository deliveryRep = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
                        cartonRep.RollBackAssignCarton(carton.CartonSN, editor);
                        deliveryRep.RollBackAssignBoxId(carton.CartonSN);
                    }
                }
                //else if (workflowName == "PalletVerifyAndAssignDN")  //handle Delivery_Pallet to H->'0' for IPC ASUS case
                //{

                //    IList<DeliveryPallet> sapDPList = (IList<DeliveryPallet>)session.GetValue(Session.SessionKeys.SAPDeliveryPalletList);
                //    if (sapDPList != null && sapDPList.Count > 0)
                //    {
                //        IDeliveryRepository deliveryRep = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
                //        foreach (DeliveryPallet item in sapDPList)
                //        {
                //            deliveryRep.UpdateDeliveryPalletInfo(new DeliveryPalletInfo { status = "0" },
                //            new DeliveryPalletInfo { id = item.ID });
                //        }
                //    }
                //}
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
            }
            finally
            {
                logger.DebugFormat("END: {0}()", methodName);
            }
        }
    }
}
