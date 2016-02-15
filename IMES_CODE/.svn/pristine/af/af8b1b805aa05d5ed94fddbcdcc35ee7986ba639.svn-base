using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace UPH.Interface
{
    public interface IUPH_Family
    {
        DataTable GetUPHFamily();
        IList<UPHInfo> GetProductUPHInfoList(UPHInfo item);
        void AddProductUPHInfo(UPHInfo item);
        void DelProductUPHInfo(UPHInfo item);
        void UpdateProductUPHInfo(UPHInfo item);
        bool CheckDuplicateData(UPHInfo item);

        void AddUPHLog(UPHInfo item);
        DataTable GetAlarmALL();
        DataTable GetUPHA(string Process);
        DataTable GetUPHH(string Process, string Family);
        DataTable InsertUPH(string Process, string Family, string Special, string Editor);
        DataTable GetUPHZ(string Process, string Family, string Special);
        DataTable Del();


    }
    [Serializable]
    public class UPHInfo
    {

        public int ID;
        public string Process;
        public int Attend_normal;
        public string Family;
        public string ST;
        public int NormalUPH;
        public string Cycle;
        public string Remark;
        public string Special;
        public string Editor;
        public DateTime Cdt;
        public DateTime Udt;
    }

}
