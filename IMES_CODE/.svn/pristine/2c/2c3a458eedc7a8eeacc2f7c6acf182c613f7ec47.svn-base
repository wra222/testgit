// created by itc205033

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Station.Interface.CommonIntf;
using IMES.DataModel;

namespace IMES.Station.Interface.StationIntf
{
    /// <summary>
    /// SMT Mo 不同于成制由SAP 系统Download 的Mo。半制根据产销排程表，建立SMT Mo。
    /// </summary>
    public interface IGenSMTMO
    {
        /// <summary>
        /// 1.1	UC-PCA-GSM-01 Generate SMT MO
        /// 根据产销排程生成相关机型Mo
        /// 1.	实现后续通过Mo管控MB S/N。
        /// 2.	实现生产订单追踪。
        /// 
        /// 异常情况：
        /// 1. 检查参数，报告错误情况；
        /// 2. 如果产生的Mo已经在数据库中存在，则报告错误：“MO NO is duplicate, please re-scan.”
        /// </summary>
        /// <param name="_111_PN">111阶Part Number</param>
        /// <param name="quantity">Product数量</param>
        /// <param name="isMassProduction">是否为量产</param>
        /// <param name="remark">remark</param>
        /// <param name="editor">operator</param>
        /// <param name="stationId">stationId</param>
        /// <param name="customerId">customerId</param>
        string GenerateSMTMO(string _111_PN,
            int quantity,
            string IsMassProduction,
            string remark,
            string editor, string stationId, string customerId);

        /// <summary>
        /// 1.2	UC-PCA-GSM-02 Query
        /// 查询并显示当天产生的，尚未列印完MB Label 的SMT MO。
        /// </summary>
        /// <param name="editor">operator</param>
        /// <param name="stationId">stationId</param>
        /// <param name="customerId">customerId</param>
        /// <returns>SMT MO列表</returns>
        IList<SMTMOInfo> Query(string editor, string stationId, string customerId);

        /// <summary>
        /// 1.3	UC-PCA-GSM-03 Delete
        /// 删除用户选择的SMT Mo
        /// 
        /// 异常情况：
        /// 1. 如果欲删除的Mo，已经开始列印MB Label，则需要报告错误：“The Mo has combined MBNo or SBNO or VBNO, Can't delete!”
        /// </summary>
        /// <param name="SMT_MOs">待删除的SMT MO列表</param>
        /// <param name="editor">operator</param>
        /// <param name="stationId">stationId</param>
        /// <param name="customerId">customerId</param>
        void Delete(IList<string> SMT_MOs, string editor, string stationId, string customerId);

        /// <summary>
        /// 1.4	UC-PCA-GSM-04 Report
        /// 查询当天产生的，尚未列印完MB Label 的SMT Mo
        /// </summary>
        /// <param name="editor">operator</param>
        /// <param name="stationId">stationId</param>
        /// <param name="customerId">customerId</param>
        /// <returns>SMTMO结构列表</returns>
        IList<SMTMOInfo> Report(string editor, string stationId, string customerId);


        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="sessionKey"></param>
        void Cancel(string sessionKey);
        /// <summary>
        ///确定是是否要生成MO
        ///
        void CreateSMTMO(string key, bool isCreated);
    }
}
