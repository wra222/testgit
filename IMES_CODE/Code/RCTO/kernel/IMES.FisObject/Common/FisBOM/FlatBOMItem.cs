using System;
using System.Collections.Generic;
using System.Text;
using IMES.FisObject.Common.Part;
using IMES.FisObject.Common.Part.PartPolicy;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.FisObject.Common.FisBOM
{
    public class FlatBOMItem : IFlatBOMItem
    {
        private int _qty;                                        //qty of parts
        private string _partCheckType;
        private IList<IPart> _alterParts;                   //all parts
        private IList<PartUnit> _checkedPart = new List<PartUnit>();          //当前站刷入的Part
        private IList<PartUnit> _stationPrecheckedPart = new List<PartUnit>();          //当前站的当前节点之前刷入的Part， 如 PartCollection站 的final PartCollection节点需要获取 Initial PartCollection节点刷入的Part
        private IList<PartUnit> _precheckedPart = new List<PartUnit>();      //当前站之前刷入的Part
        private string _valueType;
        private string _tp;
        private string _model = null;
        private string _customerId = null;
        private string _alternativeItemGroup;
        private string _guid = Guid.NewGuid().ToString();
        private Nullable<bool> _needCheckUnique = null;
        private Nullable<bool> _needSave = null;
        private bool _hasBinded = false;
        private Nullable<bool> _needCommonSave = null;


        private static IPartPolicyRepository _policyRepository;
        private static IPartPolicyRepository PolicyRepository
        {
            get
            {
                if (_policyRepository == null)
                    _policyRepository = RepositoryFactory.GetInstance().GetRepository<IPartPolicyRepository>();
                return _policyRepository;
            }
        }

        private IPartPolicy _policy;
        private IPartPolicy PartPolicy
        {
            get
            {
                if (_policy == null)
                    _policy = PolicyRepository.GetPolicy(this._partCheckType);
                return _policy;
            }
        }

        public FlatBOMItem(int qty, string partCheckType, IList<IPart> alterParts)
        {
            _qty = qty;
            _partCheckType = partCheckType;
            _alterParts = alterParts;
        }

        /// <summary>
        ///此BOMItem对应的需要收集的part属性类型
        /// </summary>
        public string ValueType
        {
            get { return _valueType; }
            set { _valueType = value; }
        }

        /// <summary>
        /// Model
        /// </summary>
        public string Model
        {
            get { return _model; }
            set { _model = value; }
        }

        /// <summary>
        /// Customer
        /// </summary>
        public string CustomerId
        {
            get { return _customerId; }
        }

        public string CheckItemType
        {
            get { return _partCheckType; }
        }

        public Nullable<bool> NeedCheckUnique { 
            get { return _needCheckUnique;} 
            set {_needCheckUnique =value;} 
        }

        public Nullable<bool> NeedSave
        {
            get { return _needSave; }
            set { _needSave = value; }
        }

        public bool HasBinded {
            get { return _hasBinded; }
            set { _hasBinded = value; } 
        }

        public Nullable<bool> NeedCommonSave
        {
            get { return _needCommonSave; }
            set { _needCommonSave = value; }
        }

        public string Tp { get; set; }

        public string Descr { get; set; }

        public string PartNoItem { get; set; }

        //Vincent 2014-07-19  in filter module assigned session key and part forbidden
        private string _sessionKey=null;
        private IList<IMES.DataModel.PartForbidPriorityInfo> _partForbidList = null;
        public string SessionKey 
                    { 
                        get { return _sessionKey; } 
                    }
        public IList<IMES.DataModel.PartForbidPriorityInfo> PartForbidList
        {
            get { return _partForbidList; } 
        }
        public IList<IMES.DataModel.CheckItemTypeRuleDef> CheckItemTypeRuleList { get; set; }
        public object Tag { get; set; }
        public IFlatBOMItem RelationBomItem { get; set; }

        public void AddPrecheckedPart(PartUnit pu)
        {
            _precheckedPart.Add(pu);
        }
        public void ClearPrecheckedPart()
        {
            _precheckedPart.Clear();
        }
        /// <summary>
        /// 要求刷入的Part数量
        /// </summary>
        public int Qty
        {
            get { return _qty; }
        }

        public string AlternativeItemGroup
        {
            get { return _alternativeItemGroup; }
            set { _alternativeItemGroup = value; }
        }

        /// <summary>
        /// 公用料列表
        /// </summary>
        public IList<IPart> AlterParts
        {
            get { return _alterParts; }
        }

        /// <summary>
        /// 当前站之前已经绑定的Part
        /// </summary>
        public IList<PartUnit> PrecheckedPart
        {
            get { return _precheckedPart; }
        }

        /// <summary>
        /// 当前站绑定的Part
        /// </summary>
        public IList<PartUnit> CheckedPart
        {
            get { return _checkedPart; }
        }

        /// <summary>
        /// 当前站之前节点绑定的Part
        /// </summary>
        public IList<PartUnit> StationPreCheckedPart
        {
            get { return _stationPrecheckedPart; }
        }

        public void AddAlterPart(IPart part)
        {
            _alterParts.Add(part);
        }

        /// <summary>
        /// try to match a string
        /// </summary>
        /// <param name="target">需要进行match的字符串</param>
        /// <param name="station">当前Station</param>
        /// <returns>match到的part</returns>
        public PartUnit Match(string target, string station)
        {
            PartUnit ret = null;
            if (PartPolicy != null)
            {
                ret = PartPolicy.Match(target, this, station);
            }
            return ret;
        }

        public void Check(PartUnit pu, IPartOwner owner, string station, IFlatBOM bom)
        {
            if (PartPolicy != null)
            {
                PartPolicy.Check(pu, this, owner, station, bom);
            }
        }

        public void Save(PartUnit pu, IPartOwner owner, string station, string editor, string key)
        {
            if (PartPolicy != null)
            {
                //Vincent add check customize need save
                //bool saveFlag = true;
                //if (_needSave.HasValue)
                //{
                //    saveFlag = _needSave.Value;
                //}
               
                //if (saveFlag)
                //{
                PartPolicy.BindTo(pu, owner, station, editor, key, _needCommonSave, _needSave);
                //}
            }
        }

        public void Save(IPartOwner owner, string station, string editor, string key)
        {
            foreach (PartUnit partUnit in CheckedPart)
            {
                Save(partUnit, owner, station, editor, key);
            }
        }

        public void AddCheckedPart(PartUnit pu)
        {
            this._checkedPart.Add(pu);
        }

        public void ClearCheckedPart()
        {
            this._checkedPart.Clear();
        }

        public void AddStationPreCheckedPart(PartUnit pu)
        {
            this._stationPrecheckedPart.Add(pu);
        }

        /// <summary>
        /// 是否已刷满
        /// </summary>
        /// <returns>是否已刷满</returns>
        public bool IsFull()
        {
            return (StationPreCheckedPart.Count + CheckedPart.Count) == Qty;
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override string ToString()
        {
            if (_alterParts != null)
            {
                StringBuilder sb = new StringBuilder();
                foreach (IPart bompart in _alterParts)
                {
                    sb.AppendFormat("Part:{0};", bompart.ToString());
                }
                return sb.ToString();
            }
            return string.Empty;
        }

        /// <summary>
        /// CheckItem 斑醚絏
        /// </summary>
        public string GUID
        {
            get { return _guid; }
        }
    }
}