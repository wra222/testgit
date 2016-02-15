using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;

namespace IMES.Docking.Interface.DockingIntf
{
    /// <summary>
    /// Virtual MO
    /// 本站实现的功能：
    ///     1.	Add Qty of Virtual MO
    ///     2.	Add New Virtual MO
    /// </summary> 
    public interface IVirtualMoForDocking
    {
        /// <summary>
        /// Get MO by Model（含Virtual Mo和真实Mo）
        /// </summary>
        /// <param name="model">Model</param>
        /// <param name="editor">operator</param>
        /// <param name="stationId">stationId</param>
        /// <param name="customerId">customerId</param>
        /// <returns>MOInfoList查询结果</returns>
        IList<MOInfo> GetVirtualMOByModel(
            string model,
            string editor, string stationId, string customerId);

        /// <summary>
        /// Get MO （For Query）
        /// </summary>
        /// <param name="mo">mo</param>
        /// <param name="model">Model</param>
        /// <param name="family">family</param>
        /// <param name="editor">editor</param>
        /// <param name="stationId">stationId</param>
        /// <param name="customerId">customerId</param>
        /// <returns>MOInfoList查询结果</returns>
        IList<MOInfo> GetVirtualForQuery(
            string mo,
            string model,
            string family,
            string editor, string stationId, string customerId);


        /// <summary>
        /// 为指定的Model 创建Virtual Mo
        /// </summary>
        /// <param name="model">Model</param>
        /// <param name="qty">Qty</param>
        /// <param name="pCode">pCode</param>
        /// <param name="startDate">startDate</param>
        /// <param name="editor">editor</param>
        /// <param name="pdLine">pdLine</param>
        /// <param name="stationId">stationId</param>
        /// <param name="customerId">customerId</param>
        void CreateNewVirtualMo(
            string model,
            int qty,
            string pCode,
            string startDate,
            string editor,string pdLine, string stationId, string customerId);

        /// <summary>
        /// 检查用户输入的Model在Model表中是否存在
        /// </summary>
        /// <param name="model">Model</param>
        void checkModel(string model);

        /// <summary>
        /// 检查用户输入的Model在是否属于该Family
        /// </summary>
        /// <param name="model">Model</param>
        void checkModelinFamily(string family, string model);


        // <summary>
        /// DownloadMO
        /// </summary>
        /// <returns>_Schema.SqlHelper.ConnectionString_GetData</returns>
        string GetDataConnectionString();

        // <summary>
        /// Auto Download MO(调用客户提供的SP)
        /// SP: op_ExecuteMoUploadJOB(string jobname)
        /// </summary>
        /// <returns></returns>
        void AutoDownloadMO_SP(string jobname);

    
        /// <summary>
        /// 取得Product的Family信息列表,Orderby Family
        /// </summary>
        /// <param name="customer">customer</param>
        /// <returns>Family信息列表</returns>
        IList<FamilyInfo> FindFamiliesByCustomer(string customer);

        /// <summary>
        /// 取得Server当前时间
        /// </summary>
        /// <returns>DateTime</returns>
        DateTime GetCurDate();


        IList<MOInfo> GetMOList(string model);

        void DeleteMo(string mo, string editor, string station, string customer);

        IList<MOInfo> GetVirtualForQuery2(string mo, string model, string family, DateTime startTime, DateTime endTime);

    }
}
