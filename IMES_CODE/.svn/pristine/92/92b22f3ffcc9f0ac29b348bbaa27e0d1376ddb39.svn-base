using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;


namespace IMES.Query.Interface.QueryIntf
{
    public interface IFA_ProductInfo       
    {       
        ProdutData ProductInfo(string Connection, string ID, out List<NextStation> NextStationList);
        DataTable GetProductHistory(string Connection, string ID);
        DataTable GetProductHistory(string Connection, string ID, string CUSTSN);
        DataTable GetProductRepair(string Connection, string ID);
        DataTable GetProductInfo(string Connection, string ID);
        DataTable GetProductPart(string Connection, string ID);
        DataTable GetProductUnpack(string Connection, string ID);
        DataTable GetProductChange(string Connection, string ID);
        DataTable GetProductITCND(string connection, string ID);
        DataTable GetProductCRPart(string Connection, string ID);
        DataTable GetProductCRLog(string Connection, string ID);
        //DataTable GetStation();
        //DataTable GetMultiProductInfo(IList<string> CustSNList);
        //void UpdateProdStatus(IList<string> CustSNList, string Station, int Status, string Editor);
    }
    [Serializable]
    public class ProdutData
    {
        public string ProductID;
        public string Model;
        public string Family;
        public string ECR;
        public string MO;
        public string UnitWeight;
        public string CustomSN;
        public string MBSN;
        public string MBPartNo;
        public string MAC;
        public string CartonSN;
        public string PalletNo;
        public string DeliveryNo;
        public string Station;
        public string StationDescr;
        public string Line;
        public int Status;
        public int TestFailCount;
        public string WHLocation; //庫位
        public string ShipDate;

        public DateTime Udt;
    }

    [Serializable]
    public class NextStation
    {
        public string Station;
        public string Description;
    }
}
