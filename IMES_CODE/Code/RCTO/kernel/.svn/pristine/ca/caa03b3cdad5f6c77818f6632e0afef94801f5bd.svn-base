using System.Linq;
using System.Collections.Generic;
using IMES.FisObject.Common.Part;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Text.RegularExpressions;
using IMES.Infrastructure.Utility.Cache;


namespace IMES.FisObject.Common.FisBOM
{
    public class BOMNode : IBOMNode
    {
        #region Link To Other
        private static IMES.FisObject.Common.Part.IPartRepository _prtRepository = null;
        private static IMES.FisObject.Common.Part.IPartRepository PrtRepository
        {
            get
            {
                if (_prtRepository == null)
                    _prtRepository = RepositoryFactory.GetInstance().GetRepository<IMES.FisObject.Common.Part.IPartRepository, IMES.FisObject.Common.Part.IPart>();
                return _prtRepository;
            }
        }
        #endregion

        private readonly IPart _part;
        private readonly int _qty;
        private readonly string _alternativeItemGroup;
        
        private IBOMNode _parent;
        private readonly IList<IBOMNode> _children = new List<IBOMNode>();

        public BOMNode(IPart part, int qty)
        {
            _part = part;
            _qty = qty;
        }

        public BOMNode(IPart part, int qty, string alternativeItemGroup)
            : this(part, qty)
        {
            _alternativeItemGroup = alternativeItemGroup;
        }

        public IPart Part
        {
            get 
            {
                if (_part == null)
                {
                    return _part;
                }
                else if (IsCached)  //假如是Cached 需重新抓Cache 資料
                {
                    return PrtRepository.Find(_part.Key);
                }
                else  //DB case
                {
                    return _part;
                }                
            }
        }

        public int Qty
        {
            get { return _qty; }
        }

        public string AlternativeItemGroup
        {
            get { return _alternativeItemGroup; }
        }

        public IBOMNode Parent
        {
            get { return _parent; }
            set { _parent = value; }
        }

        public IList<IBOMNode> Children
        {
            get { return _children; }
        }

        public IList<IBOMNode> Sibling
        {
            get
            {
                return IsRoot ? null : Parent.Children;
            }
        }

        public int Level
        {
            get 
            {
                int level = 0;
                IBOMNode tmpParent = this;
                while (!tmpParent.IsRoot)
                {
                    level++;
                    tmpParent = tmpParent.Parent;
                }
                return level;
            }
        }

        public bool IsRoot
        {
            get { return _parent == null; }
        }

        public void AddChild(IBOMNode child)
        {
            if (!_children.Contains(child))
            {
                _children.Add(child);
                child.Parent = this;
            }
        }

        public void AddChildren(IList<IBOMNode> children)
        {
            foreach (var child in children)
            {
                AddChild(child);
            }
        }

        public void RemoveChild(IBOMNode child)
        {
            if (Children.Contains(child))
            {
                child.Parent = null;
                Children.Remove(child);
            }
        }

        private bool IsCached
        {
            get
            {
                if (DataChangeMediator.CheckCacheSwitchOpen(DataChangeMediator.CacheSwitchType.BOM) ||
                    DataChangeMediator.CheckCacheSwitchOpen(DataChangeMediator.CacheSwitchType.Part))
                {
                    return true;
                }
                return false;
            }
        }
       
        /// <summary>
        /// 根據BomNode/PartType/PartDescr 過濾下一階Node 
        /// </summary>
        /// <param name="bomNodeType">empty is bypass condition</param>
        /// <param name="partType">empty is bypass condition,Regex match rule</param>
        /// <param name="partDescr">empty is bypass condition,Regex match rule</param>
        /// <param name="partNo">empty is bypass condition,Regex match rule</param>
        /// <returns></returns>
        public IList<IBOMNode> FilterChildNode(string bomNodeType, string partType, string partDescr, string partNo)
        {
            IList<IBOMNode> ret = new List<IBOMNode>();
            if (_children.Count > 0)
            {
                return _children.Where(x => checkPartRule(x.Part, bomNodeType, partType, partDescr, partNo)).ToList();
            }

            return ret;
        }
        /// <summary>
        /// 檢查Part 規則,空白或Null 不檢查
        /// </summary>
        /// <param name="part"></param>
        /// <param name="bomNodeType"></param>
        /// <param name="partType"></param>
        /// <param name="partDescr"></param>
        /// <param name="partNo"></param>
        /// <returns></returns>
        private bool checkPartRule(IPart part, string bomNodeType, string partType, string partDescr, string partNo)
        {
            if (!string.IsNullOrEmpty(bomNodeType) &&
               bomNodeType != part.BOMNodeType)
            {
                return false;
            }

            if (!string.IsNullOrEmpty(partType) &&
                !Regex.IsMatch(part.Type, partType))
            {
                return false;
            }

            if (!string.IsNullOrEmpty(partDescr) &&
                 !Regex.IsMatch(part.Descr, partDescr))
            {
                return false;
            }

            if (!string.IsNullOrEmpty(partNo) &&
               !Regex.IsMatch(part.PN, partNo))
            {
                return false;
            }

            return true;
        }
        /// <summary>
        /// 檢查Part 規則,空白或Null 不檢查
        /// </summary>
        /// <param name="bomNodeType">Equals 規則</param>
        /// <param name="partType">Regex 規則</param>
        /// <param name="partDescr">Regex 規則</param>
        /// <param name="partNo">Regex 規則</param>
        /// <returns></returns>
        public bool CheckNodePartRule(string bomNodeType, string partType, string partDescr, string partNo)
        {
            return checkPartRule(this.Part, bomNodeType, partType, partDescr, partNo);
        }

        /// <summary>
        /// Part.BomNode
        /// </summary>
        public string BomNodeType { 
            get
            {
                if (Part == null) return null;
                return Part.BOMNodeType;
            } 
        }
        /// <summary>
        /// Part.Type
        /// </summary>
        public string PartType {
            get
            {
                if (Part == null) return null;
                return Part.Type;
            } 
        }
        /// <summary>
        /// Part.PartNo
        /// </summary>
        public string PartNo {
            get
            {
                if (Part == null) return null;
                return Part.PN;
            }
        }
        /// <summary>
        /// Part.PartDescr
        /// </summary>
        public string PartDescr {
            get
            {
                if (Part == null) return null;
                return Part.Descr;
            }
        }
    }
}