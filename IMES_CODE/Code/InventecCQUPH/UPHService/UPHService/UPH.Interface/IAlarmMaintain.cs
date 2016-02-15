using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace UPH.Interface
{
    public interface IAlarmMaintain
    {
        /// <summary>
        /// 获取线别
        /// </summary>
        /// <returns></returns>
        List<string> GetAlarmline(string Process);
        List<string> GetAlarmProcess();
        //List<string> GetAlarmAllLine();
        /// <summary>
        /// 获取所有记录
        /// </summary>
        /// <returns></returns>
        //IList<AlarmInfo> GetAllAlarmInfo();
        /// <summary>
        /// 根据条件获取
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        //IList<AlarmInfo> GetAlarmInfoList(DateTime from, DateTime to);

        /// <summary>
        /// 增加1笔
        /// </summary>
        /// <param name="item"></param>
        void AddAlarmInfo(AlarmInfo item);
        void AddAlarmlog(AlarmInfoLog item);

        /// <summary>
        /// 删掉 
        /// </summary>
        /// <param name="astType"></param>
        /// <param name="astCode"></param>
        void DelAlarmInfo(AlarmInfo item);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="item"></param>
        /// <param name="astType"></param>
        /// <param name="astCode"></param>
        void UpdateAlarmInfo(AlarmInfo item);

        /// <summary>
        /// check 重复
        /// </summary>
        /// <param name="astType"></param>
        /// <param name="astCode"></param>
        //bool CheckDuplicateData(AlarmInfo item);
        DataTable GetAlarm(string Process);
        DataTable GetAlarmC(string Process, string Class);
        DataTable GetAlarmCALL(string Class);
        DataTable GetAlarmPd(string PdLine);
        DataTable GetAlarmALL();

        DataTable DelAlarm(int id);
        DataTable DelAlarmLog(int id);
        DataTable UpdateAlarm_ID(int id, string BeginTime, string EndTime, string Status, string Remark, string Editor, DateTime Udt);
        DataTable UpdateAlarmLog_ID(int id);



    }
    /// <summary>
    /// 前台使用
    /// </summary>
    [Serializable]
    public class AlarmInfo
    {

        public int ID;
        public string process;
        public string Class;
        public string PdLine;
        public string BeginTime;
        public string EndTime;
        public string Status;
        public string Remark;
        public string Editor;
        public DateTime Cdt;
        public DateTime Udt;
    }

    [Serializable]
    public class AlarmInfoLog
    {

        public int ID;
        public string process;
        public string Class;
        public string PdLine;
        public string BeginTime;
        public string EndTime;
        public string Status;
        public string Remark;
        public string Editor;
        public DateTime Cdt;
    }
}
