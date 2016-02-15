using System;
using System.Collections.Generic;
using IMES.Maintain.Interface.MaintainIntf;
using System.Data;
using IMES.DataModel;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure;
using IMES.Infrastructure.UnitOfWork;
using IMES.FisObject.Common.Station;
using IMES.FisObject.PAK.Pallet;

namespace IMES.Maintain.Implementation
{
    class PakKittingUploadManager: MarshalByRefObject, IPakKittingUpload
    {

        public DataTable GetListForPakFromLine(String line)
        {

            DataTable retLst = new DataTable();
            try
            {
                IPalletRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IPalletRepository>();
                retLst = itemRepository.ExecSpRptPAKKittingLocUp(line);
            }
            catch (Exception)
            {
                throw;
            }
            return retLst;

        }
    }
}