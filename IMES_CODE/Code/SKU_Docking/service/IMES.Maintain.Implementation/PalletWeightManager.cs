using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;
using IMES.Maintain.Interface.MaintainIntf;
using IMES.FisObject.PAK.Pallet;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using log4net;

namespace IMES.Maintain.Implementation
{
    public class PalletWeightManager : MarshalByRefObject, IPalletWeight
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public IPalletRepository palletWeightRepository = RepositoryFactory.GetInstance().GetRepository<IPalletRepository, Pallet>();

        public IList<PalletWeightMaintain> GetPalletWeightToleranceList(string family)
        {
            IList<PalletWeightMaintain> palettList = new List<PalletWeightMaintain>();
            IList<PalletWeight> fisObjectList = palletWeightRepository.GetAllPalletWeightByFamily(family);
            if (fisObjectList != null)
            {
                foreach (PalletWeight temp in fisObjectList)
                {
                    PalletWeightMaintain palletWeight = new PalletWeightMaintain();
                    palletWeight.Id = temp.Id;
                    palletWeight.Family = temp.FamilyID;
                    palletWeight.Region = temp.Region;
                    palletWeight.Qty = temp.Qty;
                    palletWeight.Weight = temp.Weight;
                    palletWeight.Tolerance = temp.Tolerance;
                    palletWeight.Editor = temp.Editor;
                    palletWeight.Cdt = temp.Cdt;
                    palletWeight.Udt = temp.Udt;

                    palettList.Add(palletWeight);
                }
            }
            return palettList;
        }

        public void AddPalletWeight(PalletWeightMaintain palletWeight)
        {
            PalletWeight fisObject = new PalletWeight();
            fisObject.Id = palletWeight.Id;
            fisObject.FamilyID = palletWeight.Family;
            fisObject.Region = palletWeight.Region;
            fisObject.Qty = palletWeight.Qty;
            fisObject.Weight = palletWeight.Weight;
            fisObject.Tolerance = palletWeight.Tolerance;
            fisObject.Editor = palletWeight.Editor;
            fisObject.Cdt = palletWeight.Cdt;
            fisObject.Udt = palletWeight.Udt;
            palletWeightRepository.AddPalletWeight(fisObject);
        }

        public void UpdatePalletWeight(PalletWeightMaintain palletWeight)
        {
            PalletWeight fisObject = new PalletWeight();
            fisObject.Id = palletWeight.Id;
            fisObject.FamilyID = palletWeight.Family;
            fisObject.Region = palletWeight.Region;
            fisObject.Qty = palletWeight.Qty;
            fisObject.Weight = palletWeight.Weight;
            fisObject.Tolerance = palletWeight.Tolerance;
            fisObject.Editor = palletWeight.Editor;
            fisObject.Cdt = palletWeight.Cdt;
            fisObject.Udt = palletWeight.Udt;
            palletWeightRepository.UpdatePalletWeight(fisObject);
        }

        public void DeletePalletWeight(int id)
        {
            palletWeightRepository.DeletePalletWeightByID(id);
        }

        public bool IFPalletWeightIsExists(string region, string family)
        {
            return palletWeightRepository.IFPalletWeightIsExists(family, region);
        }
    }
}
