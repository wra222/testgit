using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;


namespace IMES.Query.Interface.QueryIntf
{
    public interface IPAK_Common
    {
        DataTable GetByCartonSN(string Connection, string CartonSN);
        DataTable GetSnByPalletNo(string Connection, string palletNo);
        DataTable GetDtPallet(string Connection,string palletNo, string status);
        DataTable GetDtPallet(string Connection,string palletNo,DateTime fromDate,DateTime toDate,string status);
        DataTable GetPLTLog(string Connection,string palletNo);
        DataTable GetPalletTypeByDetail(string Connection, string dnNo, DateTime shipDate);
        DataTable GetPalletTypeBySummary(string Connection, string dnNo, DateTime shipDate);
        DataTable GetFamilyInfo(string Connection, string FamilyName,string ModelName, string InfoName);
        DataTable QueryIndiaMPRLabel(string Connection, string sn);
        DataTable GetDnDataByPalletNo(string Connection, string palletNo);
        DataTable GetProductProgress(string Connection, DateTime fromDate, DateTime toDate,string prdType);
        DataTable CheckWeightStation(string Connection, string sn);
        DataTable GetCoaStatusLine(string Connection);
        DataTable ReadinessReport(string Connection, string dn,DateTime shipDate,string itemType);
        DataTable GetDashBoardData(string Connection,out int totalPlanQty);
        DataTable GetDashBoardData_Detail(string Connection, DateTime fromDate,DateTime toDate,string line);
        DataTable GetLineQty(string Connection);
        
        void UpdateLineQty(string Connection,string Line,int Qty);
        DataTable GetPalletReqQty(string Connection);
    }
    
}
