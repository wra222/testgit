﻿using System.Collections.Generic;

namespace IMES.FisObject.Common.FisBOM
{

    #region HierarchicalBOM
    public interface IHierarchicalBOM
    {
        IBOMNode Root { get; }
        IList<IBOMNode> FirstLevelNodes { get; }
        IList<IBOMNode> Nodes { get; set; }
        string Model { get; set; }
        IList<IBOMNode> GetNodesByNodeType(string bomNodeType);
        IList<IBOMNode> GetFirstLevelNodesByNodeType(string bomNodeType);
        IList<IBOMNode> FilterFirstLevelBomNodes(string bomNodeType, string partType, string partDescr, string partNo);
        //IList<IBOMNode> QueryBomNode(string bomNodeType, string descrPattern, string parttypePatter);
        //IList<IBOMNode> QueryBomNode(string bomNodeType, string descrPattern, string parttypePatter, int level);
    }

    #endregion
}
