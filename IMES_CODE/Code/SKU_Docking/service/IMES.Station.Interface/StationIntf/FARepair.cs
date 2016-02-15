using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Station.Interface.CommonIntf;
using IMES.DataModel;

namespace IMES.Station.Interface.StationIntf
{
    /// <summary>
    /// FA Repair
    /// </summary>
    public interface IFARepair
    {
        /// <summary>
        /// 输入Product Id和相关信息
        /// </summary>
        /// <param name="pdLine">Pd Line</param>
        /// <param name="prodId">Product Id</param>
        /// <param name="editor">operator</param>
        /// <returns>Repair Logs</returns>
        IList<RepairInfo> InputProdId(
            string pdline,
            string prodId,
            string editor, string station, string customer);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="preStn"></param>
        /// <returns></returns>
        //IList<DefectCodeStationInfo> GetReturnStation(int id, string code, string preStn, string cause);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="prodid"></param>
        /// <returns></returns>
        //IList<DefectCodeStationInfo> GetReturnStationForAdd(string code, string cause, string prodid);
            

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prodid"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        IList<StationInfo> GetReturnStationList(string prodid, int status);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prodid"></param>
        /// <returns></returns>
        string GetProductMac(string prodid);

        /// <summary>
        /// Edit Repair logs
        /// </summary>
        /// <param name="prodId">Product Id</param>
        /// <param name="macValue">MAC</param>
        /// <param name="rll">改变的Repair log</param>
        void Edit(string prodId, string macValue, RepairInfo rll, string repairStation);

        /// <summary>
        /// Add Repair logs
        /// </summary>
        /// <param name="prodId">Product Id</param>
        /// <param name="macValue">MAC</param>
        /// <param name="rll">新增的Repair Log</param>
        void Add(string prodId, string macValue, RepairInfo rll, string repairStation);

        /// <summary>
        /// Delete Repair logs
        /// </summary>
        /// <param name="prodId">Product Id</param>
        /// <param name="rll">删除的Repair log</param>
        void Delete(
            string prodId,
            RepairInfo rll);

        /// <summary>
        /// 完成维修并保存
        /// </summary>
        /// <param name="prodId">Product Id</param>
        ArrayList Save(
            string prodId,
            string testStation, string type, out string setMsg);

        /// <summary>
        /// Cancel
        /// </summary>
        void WFCancel(string prodId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prodid"></param>
        /// <returns></returns>
        int CheckReturnProduct(string prodid);

        /// <summary>
        ///  抓NextStation By MajorPart, Cause, Defectcode
        /// </summary>
        /// <param name="majorPart"></param>
        /// <param name="cause"></param>
        /// <param name="code"></param>
        /// <param name="prodid"></param>
        /// <returns></returns>
        string GetReturnStationByAll(string majorPart, string cause, string defectcode, string prodid);

        /// <summary>
        ///  	若Marjor Part = ‘CRLCM‘，則帶出Faulty Part Sno
        /// </summary>
        /// <param name="ProductID"></param>
        /// <returns></returns>
        string GetFaultyPartSN(string ProductID);

		ArrayList Print(string defectId, string prodId, RepairInfo defect, IList<PrintItem> printItems, string line, string editor, string station, string customer);
		
		ArrayList SaveFaultPartSno(string defectId, string prodId, RepairInfo defect, string line, string editor, string station, string customer);
		
    }
}
