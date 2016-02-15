using System.Collections.Generic;
using IMES.DataModel;
using IMES.FisObject.Common.Part;
using IMES.Infrastructure.FisObjectBase;

namespace IMES.FisObject.Common.FisBOM
{
    public interface IFlatBOM 
    {
        IFlatBOMItem CurrentMatchedBomItem { get; }
        IList<IFlatBOMItem> BomItems { get; }
        PartUnit Match(string subject, string station);
        PartUnit Match(string subject, string station, bool allowReplaceMatch);
        void Check(PartUnit pu, IPartOwner owner, string station);
        void AddCheckedPart(PartUnit pu);
        void AddCheckedPart(PartUnit pu, bool allowReplaceMatch);
        IList<BomItemInfo> ToBOMItemInfoList();
        IList<PartUnit> GetCheckedPart();
        void Merge(IFlatBOM bom);
        void ClearCheckedPart();
        void AddBomItem(IFlatBOMItem item);
    }
}