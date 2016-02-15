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
using IMES.FisObject.Common.Model;

namespace IMES.Maintain.Implementation
{
    class FaKittingUploadManager: MarshalByRefObject, IFaKittingUpload
    {

        public DataTable GetListForFaFromLine(String line, String family)
        {
            DataTable retLst = new DataTable();
            try
            {
                IPalletRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IPalletRepository>();
                retLst = itemRepository.ExecSpRptKittingLocUp(line,family);
            }
            catch (Exception)
            {
                throw;
            }
            return retLst;
        }

        public IList<SelectInfoDef> GetAllFamilyList()
        {
         
            List<SelectInfoDef> result = new List<SelectInfoDef>();
            try
            {
                IFamilyRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IFamilyRepository>();
                DataTable getData = itemRepository.GetAllFamily();
                for (int i = 0; i < getData.Rows.Count; i++)
                {
                    SelectInfoDef item = new SelectInfoDef();
                    string getValue = getData.Rows[i][0].ToString().Trim();
                    item.Text = getValue;
                    item.Value = getValue;
                    result.Add(item);
                }

            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

    }
}