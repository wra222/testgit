using System.Text;
using IMES.DataModel;
using System.Data;
using System.Collections.Generic;
using System;

namespace IMES.Maintain.Interface.MaintainIntf
{
    [Serializable]
    public class BSamLocaionInfo
    {
        public string LocationId;
        public string Model ;
        public int Qty;
        public int RemainQty;
        public int FullQty;
        public int FullCartonQty;
        public string HoldInput;
        public string HoldOutput;
        public string Editor;
        public DateTime Cdt;
        public DateTime Udt;
    }

    public interface IBSamLocation
    {

        IList<BSamLocaionInfo> GetBSamLocation(BSamLocationQueryType type, string model);

        void UpdateHoldInLocation(IList<string> Ids, bool isHold, string editor);

        void UpdateHoldOutLocation(IList<string> Ids, bool isHold, string editor);

        IList<string> GetAllBSamModel();
    }
}
