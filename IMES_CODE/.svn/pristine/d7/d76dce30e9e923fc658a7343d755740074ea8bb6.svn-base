using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace UPH.Interface
{
    public interface IDinner
    {
        DataTable GetAllDinnerTime();  /// 獲取整張表信息

        List<string> GetAllProcess(); // 獲取Process信息

        DataTable GetAllDinnerProcess(string process);  /// 根據Process查詢

        DataTable GetAllDinnerProcessClass(string process, string Class, string line);  /// 根據Process、Class查詢
                                                                                        /// 
        List<string> GetAllLine(string process);  ///根據Process查Line

        DataTable GetAllDinnerLine(string process, string line);  /// 获取Process、Line查詢

        void AddDinnerTimeInfo(DinnerTimeInfo item);  /// 新增資料
        void AddDinnerLogInfo(DinnerLogInfo item);  /// 新增AddLog記錄

        void UpdateDinnerTime(int id, string begintime, string endtime, string remark, string editor);  /// 更改資料
        DataTable DelDinnerTime(int id);  /// 刪除資料
        void ADDDinnerLog(int id, string remark, string editor);  /// 新增記錄
    }

    /// <summary>
    /// 前台使用
    /// </summary>
    [Serializable]
    public class DinnerTimeInfo
    {
        public int ID;
        public string Process;
        public string Type;
        public string Class;
        public string PdLine;
        public string BeginTime;
        public string EndTime;
        public string Remark;
        public string Editor;
        public DateTime Cdt;
        public DateTime Udt;
    }

    [Serializable]
    public class DinnerLogInfo
    {
        public int ID;
        public string Process;
        public string Type;
        public string Class;
        public string PdLine;
        public string BeginTime;
        public string EndTime;
        public string Remark;
        public string Editor;
        public DateTime Cdt;
        public DateTime Udt;
    }
}
