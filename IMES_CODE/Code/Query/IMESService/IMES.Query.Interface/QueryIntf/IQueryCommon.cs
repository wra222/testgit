using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace IMES.Query.Interface.QueryIntf
{
    public interface IQueryCommon
    {
       // DataTable GetLine(IList<string> lstProcess, string customer, bool IsWithoutShift);
        DataTable GetStationDescr(string station, string DBConnection);
        DataTable GetStation(List<string> stationType, string DBConnection);
        DataTable GetStationName(List<string> station, string DBConnection);
        DataTable GetLine(IList<string> lstProcess, string customer, bool IsWithoutShift, string DBConnection);
        DataTable GetDNList(string DBConnection, DateTime fromDate, DateTime toDate);
        DataTable GetDefect(List<string> type,string DBConnection);
        DataTable GetDefectInfo(List<string> type, string customer, string DBConnection);
        DataTable GetSysSetting(List<string> Name, string DBConnection);
        DataTable GetFixtureID(string DBconnection);
        DataTable GetOP(string DBconnection);
        DataTable GetFamily(string DBconnection, DateTime fromDate, DateTime toDate);
        void UpdateSysSetting(string Name, string Value, string DBConnection);
        DataTable GetWithdrawTest(DateTime shipDate, string model, string status, DataTable dt, string DBConnection);
          DataTable GetSMTLine(string DBConnection);
        DataTable GetSMTRefrshTimeAndStationByLine(string DBConnection,string line);

    }
    
   


    /// <summary>
    /// Defect接口
    /// </summary>
    public interface IDefect
    {
        /// <summary>
        /// 取得Defect列表
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>Defect信息列表</returns>
        IList<DefectInfo> GetDefectList(string type);

        /// <summary>
        /// 取得Defect信息
        /// </summary>
        /// <param name="defectId">Defect标识</param>
        /// <returns>Defect信息</returns>
        DefectInfo GetDefectInfo(string defectId);

        /// <summary>
        /// 根据type,customer获取对应的Defect信息列表
        /// </summary>
        /// <param name="type">type</param>
        /// <param name="customer">customer</param>
        /// <returns>Defect信息列表</returns>
        IList<DefectInfo> GetDefectInfoByTypeAndCustomer(string type, string customer);
    }

    /// <summary>
    /// Defect信息
    /// </summary>
    [Serializable]
    public struct DefectInfo
    {
        /// <summary>
        /// Defect标识
        /// </summary>
        public string id;
        /// <summary>
        /// 友好名字(没有时实现请等同为id)
        /// </summary>
        public string friendlyName;
        /// <summary>
        /// Defect描述
        /// </summary>
        public string description;
    }

    /// <summary>
    /// Cause接口
    /// </summary>
    public interface ICause
    {
        /// <summary>
        /// 取得Cause信息列表
        /// </summary>
        /// <param name="customer">customer</param>
        /// <returns>Cause信息列表</returns>
        IList<CauseInfo> GetCauseList(string customerId,string stage);
    }

    /// <summary>
    /// Cause信息
    /// </summary>
    [Serializable]
    public struct CauseInfo
    {
        /// <summary>
        /// Cause标识
        /// </summary>
        public string id;
        /// <summary>
        /// 友好名字(没有时实现请等同为id)
        /// </summary>
        public string friendlyName;

        /// <summary>
        /// CauseInfo 构造函数
        /// </summary>
        /// <param name="id">Cause ID</param>
        /// <param name="friendlyName">Cause友好名字</param>
        public CauseInfo(string id, string friendlyName)
        {
            this.id = id;
            this.friendlyName = friendlyName;
        }
    }


    /// <summary>
    /// PdLine value object
    /// </summary>
    [Serializable]
    public struct PdLineInfo
    {
        /// <summary>
        /// PdLine Identifier
        /// </summary>
        public string id;

        /// <summary>
        /// PdLine Friendly Name
        /// </summary>
        public string friendlyName;
    }

    /// <summary>
    /// DefectInfo濬倰
    /// </summary>
    public static class DefectInfoType
    {
        public const string Component = "Component";
        public const string Obligation = "Obligation";
        public const string Cause = "Cause";
        public const string MajorPart = "MajorPart";
        public const string Distribution = "Distribution";
        public const string Responsibility = "Responsibility";
        public const string M4 = "4M";
        public const string TrackingStatus = "TrackingStatus";
        public const string Cover = "Cover";
        public const string Uncover = "Uncover";
        public const string Mark = "Mark";
    }
 [Serializable]
    public struct SMTDashBoardLineInfo
    {
        /// <summary>
        /// PdLine Identifier
        /// </summary>
        public string id;

        /// <summary>
        /// PdLine Friendly Name
        /// </summary>
        public string friendlyName;


        /// <summary>
        /// Line refreshTime
        /// </summary>
        public int refreshTime;


        /// <summary>
        /// 此线别抓取的站点
        /// </summary>
        public string station;

    }
}
