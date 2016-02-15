using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMES.DataModel
{
    [Serializable]
    public class ModelBOMInfoDef
    {
        public string Component;
        public string Material_group;
        public string Quantity;
        public string Alternative_item_group;
        public string Priority;
        public string Editor;
        public string Cdt;
        public string Udt;
        public string ID;
        public string Flag;
    }

    [Serializable]
    public class ApproveInfoDef
    {
        public string Editor;
        public string ApproveDate;
    }

    [Serializable]
    public class ChangeNodeInfoDef
    {
        public string NodeName;  //当前树上的节点
        public Boolean IsModel; //是否是Model
    }

    [Serializable]
    public class RefreshMoBomInfoDef
    {
        public string Model;
        public List<MoBomNodeInfoDef> MoBomItem = new List<MoBomNodeInfoDef>();
    }

    [Serializable]
    public class MoBomNodeInfoDef
    {
        public string MO;   
        public string Qty;
        public string PrintQty;
    }

   
}