using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace UPH.Interface
{
    public interface IAlarmMailAddress
    {
       List<string> GetLine(string Process);
       DataTable GetLine1();

       List<string> GetMailProcess();
       DataTable GetAlarmALL();
       DataTable GetMail(string Dept, string Process, string PdLine);
       DataTable GetMail2(string PdLine, string Process);
       DataTable UpdateMail(string Editor, string Remark, string PdLine, string MailAddress);
       DataTable InsertMail(string Editor, string Remark, string PdLine, string MailAddress,string Process,string Dept);
       DataTable DELETEMail(string Editor, string PdLine, string Process);
       DataTable GetLineMail(string PdLine);


    }
    [Serializable]
    public class MailInfo
    {

        public string Dept;
        public string Process;
        public string PdLine;
        public string MailAddress;
        public string Remark;
        public string Editor;
        public DateTime Cdt;
        public DateTime Udt;
    }
}
