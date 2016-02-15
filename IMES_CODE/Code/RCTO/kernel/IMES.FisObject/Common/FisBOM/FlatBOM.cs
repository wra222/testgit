using System.Collections.Generic;
using System.Text;
using IMES.DataModel;
using IMES.FisObject.Common.Part;
using IMES.Infrastructure;

namespace IMES.FisObject.Common.FisBOM
{
    public class FlatBOM : IFlatBOM
    {
        private readonly IList<IFlatBOMItem> _bomItems;
        private IFlatBOMItem _currentMatchedBomItem;

        public FlatBOM()
        {
            _bomItems = new List<IFlatBOMItem>();
        }

        public FlatBOM(IList<IFlatBOMItem> bomItems)
        {
            foreach (IFlatBOMItem item in bomItems)
            {
                IList<IMES.DataModel.CheckItemTypeRuleDef> checkItemTypeRuleList = item.CheckItemTypeRuleList;
                if (checkItemTypeRuleList != null && checkItemTypeRuleList.Count > 0)
                {
                    item.NeedCheckUnique = checkItemTypeRuleList[0].NeedUniqueCheck == "Y";
                    item.NeedCommonSave = checkItemTypeRuleList[0].NeedCommonSave == "Y";
                    item.NeedSave = checkItemTypeRuleList[0].NeedSave== "Y";
                }
            }
            _bomItems = bomItems;
        }

        public IList<IFlatBOMItem> BomItems
        {
            get { return _bomItems; }
        }

        public IFlatBOMItem CurrentMatchedBomItem
        {
            get { return _currentMatchedBomItem; }
        }

        public void AddBomItem(IFlatBOMItem item)
        {
            IList<IMES.DataModel.CheckItemTypeRuleDef> checkItemTypeRuleList = item.CheckItemTypeRuleList;
            if (checkItemTypeRuleList != null && checkItemTypeRuleList.Count > 0)
            {
                item.NeedCheckUnique = checkItemTypeRuleList[0].NeedUniqueCheck == "Y";
                item.NeedCommonSave = checkItemTypeRuleList[0].NeedCommonSave == "Y";
                item.NeedSave = checkItemTypeRuleList[0].NeedSave == "Y";
            }
            _bomItems.Add(item);
        }

        public PartUnit Match(string subject, string station)
        {
            foreach (var item in _bomItems)
            {
                if (item.IsFull())
                {
                    continue;
                }
                var matchedPart = item.Match(subject, station);
                if (matchedPart != null)
                {
                    _currentMatchedBomItem = item;
                    return matchedPart;
                }
            }
            return null;
        }

        public PartUnit Match(string subject, string station, bool allowReplaceMatch)
        {
            if (allowReplaceMatch)
            {
                foreach (var item in _bomItems)
                {
                    var matchedPart = item.Match(subject, station);
                    if (matchedPart != null)
                    {
                        _currentMatchedBomItem = item;
                        return matchedPart;
                    }
                }
                return null;
            }
            else
            {
                return Match(subject, station);
            }
        }

        public void Check(PartUnit pu, IPartOwner owner, string station)
        {
            if (CurrentMatchedBomItem.RelationBomItem != null)
            {
                int relationQty = CurrentMatchedBomItem.RelationBomItem.CheckedPart.Count;
                int checkedQty = CurrentMatchedBomItem.CheckedPart.Count;
                if (relationQty == 0 ||
                   checkedQty >= relationQty)
                {
                    throw new FisException("BOM002", new string[] { CurrentMatchedBomItem.RelationBomItem.Descr ?? string.Empty });
                }
            }

            CurrentMatchedBomItem.Check(pu, owner, station, this);
        }

        public void AddCheckedPart(PartUnit pu)
        {
            if (pu.IsPrecheckedPart)
            {
                CurrentMatchedBomItem.AddPrecheckedPart(pu);
            }
            else
            {
                CurrentMatchedBomItem.AddCheckedPart(pu);
            }
        }

        public void AddCheckedPart(PartUnit pu, bool allowReplaceMatch)
        {
            if (allowReplaceMatch)
            {
                CurrentMatchedBomItem.ClearCheckedPart();
            }
            if (pu.IsPrecheckedPart)
            {
                CurrentMatchedBomItem.AddPrecheckedPart(pu);
            }
            else
            {
                CurrentMatchedBomItem.AddCheckedPart(pu);
            }
        }

        public IList<PartUnit> GetCheckedPart()
        {
            var ret = new List<PartUnit>();
            foreach (var item in _bomItems)
            {
                ret.AddRange(item.CheckedPart);
            }
            return ret;

        }

        public void Merge(IFlatBOM bom)
        {
            foreach (var flatBOMItem in bom.BomItems)
            {
                _bomItems.Add(flatBOMItem);
            }
        }

        public void ClearCheckedPart()
        {
            foreach (var item in _bomItems)
            {
                item.ClearCheckedPart();
            }
        }

        public IList<BomItemInfo> ToBOMItemInfoList()
        {
            IList<BomItemInfo> retLst = new List<BomItemInfo>();
            foreach (IFlatBOMItem item in _bomItems)
            {
                BomItemInfo ItemInfo = new BomItemInfo();
                ItemInfo.qty = item.Qty;
                ItemInfo.description = item.Descr ?? string.Empty;
                ItemInfo.PartNoItem = item.PartNoItem ?? string.Empty;
                ItemInfo.type = item.CheckItemType ?? string.Empty;
                ItemInfo.tp = item.Tp ?? string.Empty;
                ItemInfo.GUID = item.GUID;
                if (item.StationPreCheckedPart != null)
                {
                    ItemInfo.scannedQty = item.StationPreCheckedPart.Count;
                    ItemInfo.collectionData = new List<pUnit>();
                    foreach (PartUnit preItem in item.StationPreCheckedPart)
                    {
                        pUnit temp = new pUnit();
                        temp.sn = preItem.Sn;
                        temp.pn = preItem.Pn;
                        temp.valueType = item.CheckItemType;
                        ItemInfo.collectionData.Add(temp);
                    }
                }
                else
                {
                    ItemInfo.scannedQty = 0;
                    ItemInfo.collectionData = new List<pUnit>();
                }

                List<PartNoInfo> allPart = new List<PartNoInfo>();

                ItemInfo.parts = new List<PartNoInfo>();
                if (item.AlterParts != null)
                {
                    foreach (IPart part in item.AlterParts)
                    {
                        PartNoInfo aPart = new PartNoInfo();
                        aPart.description = part.Descr ?? string.Empty;
                        aPart.id = part.PN ?? string.Empty;
                        aPart.friendlyName = aPart.id ?? string.Empty;
                        aPart.partTypeId = part.Type ?? string.Empty;
                        aPart.iecPartNo = part.PN;
                        aPart.valueType = item.CheckItemType ?? string.Empty;
                        IList<NameValueInfo> properties = new List<NameValueInfo>();
                        foreach (PartInfo property in part.Attributes)
                        {
                            var tempProperty = new NameValueInfo();
                            tempProperty.Name = property.InfoType;
                            tempProperty.Value = property.InfoValue;
                            properties.Add(tempProperty);
                        }
                        aPart.properties = properties;
                        allPart.Add(aPart);
                    }
                    allPart.Sort(delegate(PartNoInfo p1, PartNoInfo p2) { return p1.id.CompareTo(p2.id); });

                    ItemInfo.parts = allPart;
                }
                retLst.Add(ItemInfo);
            }
            return retLst;
        }

        public override string ToString()
        {
            if (_bomItems != null)
            {
                StringBuilder sb = new StringBuilder();
                foreach (IFlatBOMItem item in _bomItems)
                {
                    sb.AppendFormat("BOMItem:{0};\r\n", item);
                }
                sb.AppendFormat("CurrentMatchedBomItem:{0};\r\n", _currentMatchedBomItem);
                return sb.ToString();
            }
            return string.Empty;
        }
    }
}