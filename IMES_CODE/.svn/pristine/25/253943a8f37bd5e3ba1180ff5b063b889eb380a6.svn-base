using System.Text;
using IMES.DataModel;
using System.Data;
using System.Collections.Generic;
using System;



namespace IMES.Maintain.Interface.MaintainIntf
{
    [Serializable]
    public class PalletTypeInfo
    {
        public int ID;
        public string ShipWay;
        public string RegId;
        public string PalletType;
		public string OceanType;
        public string StdFullQty;
        public int MinQty;
        public int MaxQty;
        public string PalletCode;
        public decimal Weight;
        public string InPltWeight;
        public string ChepPallet;
        public string CheckCode;
        public string Editor;
        public int PalletLayer;
        public DateTime Cdt;
        public DateTime Udt;
    }

    public interface IPalletTypeforICC
    {
        IList<string> GetShipWay();

        IList<string> GetRegId();

        IList<PalletTypeInfo> GetPalletType(string shipWay, string regId);

        IList<PalletTypeInfo> CheckExistRangeQty(string shipWay, string regId, string stdPltFullQty, int maxQty, int minQty);

        IList<PalletTypeInfo> CheckExistRangeQty(string shipWay, string regId, string stdPltFullQty, int palletLayer, string oceanType, int maxQty, int minQty);

        void Remove(PalletTypeInfo item);

        void Update(PalletTypeInfo item);

        void Add(PalletTypeInfo item);


    }
}
