using System.Collections.Generic;
using System.Linq;

namespace IMES.FisObject.Common.FisBOM
{
    public class HierarchicalBOM : IHierarchicalBOM
    {
        private readonly IBOMNode _root;
        private string _model;


        public HierarchicalBOM(IBOMNode root)
        {
            _root = root;
        }

        public IBOMNode Root
        {
            get
            {
                return _root;
            }
        }

        public string Model
        {
            get { return _model; }
            set { _model = value; }
        }

        public IList<IBOMNode> FirstLevelNodes
        {
            get
            {
                return _root != null ? _root.Children : null;
            }
        }

        public IList<IBOMNode> Nodes { get; set; }

        public IList<IBOMNode> GetNodesByNodeType(string bomNodeType)
        {
            return Nodes.Where(bomNode => bomNode.Part.BOMNodeType.CompareTo(bomNodeType) == 0).ToList();
        }
 
        public IList<IBOMNode> GetFirstLevelNodesByNodeType(string bomNodeType)
        {
            if (FirstLevelNodes != null)
            {
                return FirstLevelNodes.Where(bomNode => bomNode.Part.BOMNodeType.CompareTo(bomNodeType) == 0).ToList();
            }
            return null;
        }

        public IList<IBOMNode> FilterFirstLevelBomNodes(string bomNodeType, string partType, string partDescr, string partNo)
        {
            IList<IBOMNode> ret = new List<IBOMNode>();
            if (_root == null || _root.Children.Count==0)
            {
                return ret;
            }
            else
            {
                IList<IBOMNode> bomNodeList = _root.Children;
                foreach (IBOMNode item in bomNodeList)
                {
                    if (item.CheckNodePartRule(bomNodeType, partType, partDescr, partNo))
                    {
                        ret.Add(item);
                    }
                }

                return ret;
            }
        }
    }
}