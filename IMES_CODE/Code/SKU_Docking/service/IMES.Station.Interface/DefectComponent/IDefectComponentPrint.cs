using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;

namespace IMES.Station.Interface.StationIntf
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDefectComponentPrint
    {
        IList<string> GetVendorList();

        IList<string> GetReturnLineList();

        ArrayList GetDefectComponentInfo(string vendor, string customer, string station, string user);

        ArrayList GetDefectComponentDetailInfo(string guid, string vendor, string family, string iecpn, string defectcode);

        ArrayList Save(string guid, string returnLine, int totalQty, List<PrintItem> printItems);

        void Cancel(string sn);


        IList<DefectComponentPrintGV_Batch> GetDefectComponentBatch(string vendor, string printDate);

        IList<DefectComponentPrintGV1> GetDefectComponent_RePrint(string batchID);

        IList<DefectComponentPrintGV2> GetDefectComponentDetailInfo_RePrint(string batchID, string vendor, string family, string iecpn, string defectcode);

        ArrayList RePrint(string batchID, string customer, string station, string user, List<PrintItem> printItems);
    }

    [Serializable]
    public class DefectComponentPrintGV1
    {
        public string ReturnType;
        public string Family;
        public string IECPn;
        public string PartType;
        public string Vendor;
        public int NPQty;
        public string DefectCode;
        public string DefectDesc;
    }

    [Serializable]
    public class DefectComponentPrintGV2
    {
        public string Vendor;
        public string Family;
        public string PartNo;
        public string PartType;
        public string IECPn;
        public string PartSN;
        public string Defect;
    }

    [Serializable]
    public class DefectComponentPrintGV_Batch
    {
        public string BatchID;
        public DateTime PrintDate;
        public string Status;
        public string ReturnLine;
        public int TotalQty;
    }
}
