using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

namespace IMES.LD
{
    /// <summary>
    /// 雷雕回传的信息：SN、PN、Warranty、MN 
    /// Status>0 =>success 
    /// Status<0 => fail
    /// </summary>
    public class PrintResponse
    {
        public string SN { get; set; }
        public string PN { get; set; }
        public string Warranty { get; set; }
        public string MN { get; set; }
        public string ErrorMsg { get; set; }
        public int Status { get; set; }

        public override string ToString()
        {
            return string.Format("SN:{0} PN:{1} Warranty:{2} MN:{3} ErrorMsg:{4} Status:{5}", SN,PN,Warranty,MN,ErrorMsg,Status.ToString()) ;
        }
    }

    /// <summary>
    /// EoLA回传的信息
    /// </summary>
    public class ModelQty
    {
        public int Qty { get; set; }
        public DateTime Date { get; set; }       
    }

    /// <summary>
    /// Dashboard SumaryInputOutputQty
    /// </summary>
    public class SumInputOutputQty
    {
        public DateTime Period_Date { get; set; }
        public int TargetInput { get; set; }
        public int InputQty { get; set; }
        public int NonInput { get; set; }
        public int TargetOutput { get; set; }
        public int OutputQty { get; set; }
        public int NonOutput { get; set; }
        public DateTime Exec_Time { get; set; }
              
    }

    public class IACInOutQty
    {
        public string Period_Date { get; set; }
        public string Site { get; set; }
        public int InputQty { get; set; }
        public int OutputQty { get; set; }

    }

    public class DailyOutIACQty
    {
        public string NPeriod_Date { get; set; }
        public int TotalQty { get; set; }

    }

    public class DailyOutputQty
    {
        public string NPeriod_Date { get; set; }
        public int TotalQty { get; set; }
        public int AttendanceQty { get; set; }

    }

    public class GrandTTLOutQty
    {
        public string GPeriod_Date { get; set; }
        public int GTotalQty { get; set; }
    }

    public class OutSAIn
    {
        public DateTime Period_Date { get; set; }
        public string Station { get; set; }
        public int OutQty { get; set; }
        public int WIPQty { get; set; }
        public decimal OKRate { get; set; }
    }

    public class OutSAOut
    {
        public DateTime Period_Date { get; set; }
        public string Station { get; set; }
        public int OutQty { get; set; }
        public int WIPQty { get; set; }
        public decimal OKRate { get; set; }
    }

    public class OutFAIn
    {
        public DateTime Period_Date { get; set; }
        public string Station { get; set; }
        public int OutQty { get; set; }
        public int WIPQty { get; set; }
    }

    public class OutFAOut
    {
        public DateTime Period_Date { get; set; }
        public string Station { get; set; }
        public int OutQty { get; set; }
        public int WIPQty { get; set; }
        public decimal OKRate { get; set; }
    }

    public class OutPAKIn
    {
        public DateTime Period_Date { get; set; }
        public string Station { get; set; }
        public int OutQty { get; set; }
        public int WIPQty { get; set; }
        public decimal OKRate { get; set; }
    }

    public class OutPAKOut
    {
        public DateTime Period_Date { get; set; }
        public string Station { get; set; }
        public int OutQty { get; set; }
        public int WIPQty { get; set; }
        public decimal OKRate { get; set; }
    }

    public class TmpPeriod_Date
    {
        public string Period_Date { get; set; }
    }

    public class IACPeriod_Date
    {
        public DateTime Period_Date { get; set; }
    }
    public class OutSAInLine
    {
        public DateTime Period_Date { get; set; }
        public string Station { get; set; }
        public string PdLine { get; set; } 
        public int OutQty { get; set; }
        public int WIPQty { get; set; }
    }

    public class OutSAOutLine
    {
        public DateTime Period_Date { get; set; }
        public string Station { get; set; }
        public string PdLine { get; set; }
        public int OutQty { get; set; }
        public int WIPQty { get; set; }
    }

    public class OutFAInLine
    {
        public DateTime Period_Date { get; set; }
        public string Station { get; set; }
        public string PdLine { get; set; }
        public int OutQty { get; set; }
        public int WIPQty { get; set; }
    }

    public class OutFAOutLine
    {
        public DateTime Period_Date { get; set; }
        public string Station { get; set; }
        public string PdLine { get; set; }
        public int OutQty { get; set; }
        public int WIPQty { get; set; }
    }

    public class OutPAKInLine
    {
        public DateTime Period_Date { get; set; }
        public string Station { get; set; }
        public string PdLine { get; set; }
        public int OutQty { get; set; }
        public int WIPQty { get; set; }
    }

    public class OutPAKOutLine
    {
        public DateTime Period_Date { get; set; }
        public string Station { get; set; }
        public string PdLine { get; set; }
        public int OutQty { get; set; }
        public int WIPQty { get; set; }
    }

    public class HRAttendanceQty
    {
        public DateTime Period_Date { get; set; }
        public int AttendanceQty { get; set; }
        public string WorkCategory { get; set; }
    }

    public class HRAttendQty
    {
        public DateTime Period_Date { get; set; }
        public int AttendanceQty { get; set; }
        public string WorkCategory { get; set; }
        public string Site { get; set; }
    }


    public class SAInOutStageLine
    {
        public DateTime Period_Date { get; set; }
        public string NPeriod_Date { get; set; }
        public string Stage { get; set; }
        public string PdLine { get; set; }
        public int InQty { get; set; }
        public int OutQty { get; set; }
    }

    public class SAWIPStage
    {
        public DateTime Period_Date { get; set; }
        public string NPeriod_Date { get; set; }
        public string Stage { get; set; }
        public int WIPQty { get; set; }
    }

    public class FAInOutStageLine
    {
        public DateTime Period_Date { get; set; }
        public string NPeriod_Date { get; set; }
        public string Stage { get; set; }
        public string PdLine { get; set; }
        public int InQty { get; set; }
        public int OutQty { get; set; }
    }

    public class FAWIPStage
    {
        public DateTime Period_Date { get; set; }
        public string NPeriod_Date { get; set; }
        public string Stage { get; set; }
        public int WIPQty { get; set; }
    }

    public class PAKInOutStageLine
    {
        public DateTime Period_Date { get; set; }
        public string NPeriod_Date { get; set; }
        public string Stage { get; set; }
        public string PdLine { get; set; }
        public int InQty { get; set; }
        public int OutQty { get; set; }
    }

    public class PAKWIPStage
    {
        public DateTime Period_Date { get; set; }
        public string NPeriod_Date { get; set; }
        public string Stage { get; set; }
        public int WIPQty { get; set; }
    }

    public class MonthlyQty
    {
        public DateTime Period_Date { get; set; }
        public string NPeriod_Date { get; set; }
        public int TTLQty { get; set; }
    }


}