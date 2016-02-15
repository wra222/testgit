using System;
using System.Collections.Generic;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.PAK.Pallet;
using IMES.DataModel;
using IMES.Maintain.Interface.MaintainIntf;
using System.Data;


namespace IMES.Maintain.Implementation
{
    public class ForwarderManager : MarshalByRefObject, IForwarder
    {

        public DataTable GetForwarderList(string StartDate, string EndDate)
        {

            DataTable retLst = new DataTable();
            try
            {
                IPalletRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IPalletRepository>();
                retLst = itemRepository.GetForwarderList(StartDate, EndDate);                
            }
            catch (Exception)
            {
                throw;
            }

            return retLst;
        }

        public void ImportForwarder(List<ForwarderInfo> items)
        {
            try
            {
                IPalletRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IPalletRepository>();
                List<ForwarderInfo> itemList = items;
                itemRepository.ImportForwarders(itemList);

            }
            catch (Exception)
            {
                throw;
            }
           
        }

        public void UpdateForwarder(ForwarderInfo item)
        {
            try
            {
                IPalletRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IPalletRepository>();

                DataTable exists = itemRepository.GetExistForwarder(item);
                if (exists != null && exists.Rows.Count > 0)
                {
                    if (exists.Rows[0][0].ToString() != item.Id.ToString())
                    {
                        //!!!need change
                        //已经存在具有相同Date、Forwarder、MAWB、Driver和Truck ID的Forwarder记录
                        List<string> erpara = new List<string>();
                        FisException ex;
                        ex = new FisException("DMT055", erpara);
                        throw ex;
                    }
                }                
                itemRepository.UpdateForwarder(item);
            }
            catch (Exception)
            {
                throw;
            }

        }

        public void DeleteForwarder(ForwarderInfo item)
        {
            try
            {
                IPalletRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IPalletRepository>();
                ForwarderInfo itemForwarder = new ForwarderInfo();
                itemForwarder.Id = item.Id;
                itemRepository.DeleteForwarder(itemForwarder);
            }
            catch (Exception)
            {
                throw;
            }

        }

        public DateTime GetCurDate()
        {
            return DateTime.Now;
        }
    }
}
