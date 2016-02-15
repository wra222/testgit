/*
 * INVENTEC corporation (c)2010 all rights reserved. 
 * Description: 
 *              
 * Update: 
 * Date         Name            Reason 
 * ========== ================= =====================================
 * 2010-6-23   itc210001        create
 * 
 * Known issues:Any restrictions about this file 
 *          
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Maintain.Interface.MaintainIntf;
using IMES.DataModel;
using IMES.FisObject.PAK.DN;
using IMES.Infrastructure.FisObjectRepositoryFramework;


namespace IMES.Maintain.Implementation
{
    public class KittingLocationManager : MarshalByRefObject,IKittingLocation
    {
        

        public IList<KittingLocationInfo> GetKittingLocationList()
        {
            IList<KittingLocationInfo> retLst = null;
            try
            {
                IDeliveryRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository>();
                retLst = itemRepository.GetKittingLocationList();
                return retLst;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<KittingLocationInfo> GetKittingLocationList(string tagID)
        {
            IList<KittingLocationInfo> retLst = null;
            try
            {
                IDeliveryRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository>();
                retLst = itemRepository.GetKittingLocationList(tagID);
                return retLst;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //need modify
        public void UpdateKittingLocation(KittingLocationInfo item)
        {
            try 
            {
                IDeliveryRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository>();
                itemRepository.UpdateKittingLocation(item);
            }
            catch (Exception)
            {
                throw;
            }
        }

       
    }
}
