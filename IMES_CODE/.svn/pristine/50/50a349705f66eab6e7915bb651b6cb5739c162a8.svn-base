using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace IMES.Station.Interface.QueryInf
{
    public interface IFAQuery
    {
        ProdutData ProductInfo(String ID, out List<NextStation> NextStationList);
        DataTable GetProductHistory(string ID);
        DataTable GetStation();
        DataTable GetMultiProductInfo(IList<string> CustSNList);
        void UpdateProdStatus(IList<string> CustSNList, string Station, int Status, string Editor);

    }

    [Serializable]
    public class ProdutData 
    {
        public string ProductID;
        public string Model;
        public string Family;
        public string ECR;
        public string MO;
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
        public DateTime Udt;
    }
    
}
