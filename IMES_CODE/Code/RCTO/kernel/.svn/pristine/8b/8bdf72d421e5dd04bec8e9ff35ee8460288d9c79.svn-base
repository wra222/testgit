using System.Collections.Generic;
using IMES.FisObject.Common.Part;

namespace IMES.FisObject.Common.FisBOM
{
    public interface ITreeNode
    {
    }
    public interface IBOMNode
    {
        IPart Part { get; }
        int Qty { get; }
        IBOMNode Parent { get; set; }
        IList<IBOMNode> Children { get; }
        IList<IBOMNode> Sibling { get; }
        int Level { get; }
        bool IsRoot { get; }
        string AlternativeItemGroup { get; }

        void AddChild(IBOMNode child);
        void AddChildren(IList<IBOMNode> children);
        void RemoveChild(IBOMNode child);
        /// <summary>
        /// 根據BomNode/PartType/PartDescr 過濾下一階Node 
        /// </summary>
        /// <param name="bomNodeType">empty is bypass condition</param>
        /// <param name="partType">empty is bypass condition</param>
        /// <param name="partDescr">empty is bypass condition</param>
        /// <param name="partNo">empty is bypass condition</param>
        /// <returns></returns>
        IList<IBOMNode> FilterChildNode(string bomNodeType, string partType, string partDescr, string partNo);
        /// <summary>
        /// 檢查Part 規則
        /// </summary>       
        /// <param name="bomNodeType"></param>
        /// <param name="partType">Regex規則</param>
        /// <param name="partDescr">Regex規則</param>
        /// <param name="partNo">Regex規則</param>
        /// <returns></returns>
        bool CheckNodePartRule(string bomNodeType, string partType, string partDescr, string partNo);
        /// <summary>
        /// Part.BomNode
        /// </summary>
        string BomNodeType { get; }
        /// <summary>
        /// Part.Type
        /// </summary>
        string PartType { get; }
        /// <summary>
        /// Part.PartNo
        /// </summary>
        string PartNo { get; }
        /// <summary>
        /// Part.PartDescr
        /// </summary>
        string PartDescr { get; }
    }
}