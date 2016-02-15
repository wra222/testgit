/*
 * INVENTEC corporation ?2011 all rights reserved. 
 * Description:Interface for QC Repair Add/Edit Page
 * UI:CI-MES12-SPEC-FA-UI QC Repair.docx –2011/10/19 
 * UC:CI-MES12-SPEC-FA-UC QC Repair.docx –2011/10/19            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
* 2011-11-14   Jessica Liu           (Reference Ebook SourceCode) Create
* Known issues:
* TODO：
* ITC-1360-0434, Jessica Liu, 2012-2-28
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;

namespace IMES.Docking.Interface.DockingIntf
{
    /// <summary>
    /// 功能：
    /// 1、查询并显示unit 当前维修记录
    /// 2、修改指定的unit 维修记录
    /// 3、增加unit 维修记录
    /// </summary>
    public interface IOQCRepairForDocking
    {
        /// <summary>
        /// 输入Product Id和相关信息, 
        /// 初次进入Repair 的时候，会基于Test Log 生成Repair Record
        /// </summary>
        /// <param name="pdLine">Pd Line</param>
        /// <param name="prodId">Product Id</param>
        /// <param name="editor">operator</param>
        /// <returns>Repair Logs</returns>
        IList<RepairInfo> InputProdId(
            string pdLine,
            string prodId,
            string editor, string stationId, string customer);

        /// <summary>
        /// Edit Repair logs
        /// </summary>
        /// <param name="prodId">Product Id</param>
        /// <param name="rll">改变的Repair log</param>
        void Edit(
            string prodId,
            RepairInfo rll);

        /// <summary>
        /// Add Repair logs
        /// </summary>
        /// <param name="prodId">Product Id</param>
        /// <param name="rll">新增的Repair Log</param>
        void Add(
            string prodId,
            RepairInfo rll);

        /// <summary>
        /// Delete Repair logs
        /// </summary>
        /// <param name="prodId">Product Id</param>
        /// <param name="rll">删除的Repair logs</param>
        void Delete(
            string prodId,
            RepairInfo rll);

        /* 2012-7-5, 新需求
        /// <summary>
        /// 完成维修并保存
        /// </summary>
        /// <param name="prodId">Product Id</param>
        void Save(
            string prodId);
        */
        /* 2012-8-1, Jessica Liu, 需求变更：增加Return Station 计算逻辑, 修改解DN条件
        /// <summary>
        /// 完成维修并保存，返回excel打印信息
        /// </summary>
        /// <param name="prodId">Product Id</param>
        /// <returns>excel里7个参数的值</returns>
        List<string> Save(
            string prodId);
        */
        /// <summary>
        /// 完成维修并保存，返回excel打印信息
        /// </summary>
        /// <param name="prodId">Product Id</param>
        /// <param name="returnStation">return Station</param>
        /// <param name="returnStationText">return Station Text</param>
        /// <returns>excel里7个参数的值</returns>
        //public List<string> Save(string prodId)
        List<string> Save(string prodId, string returnStation, string returnStationText);


        /// <summary>
        /// 取得OQC Product Repair列表
        /// </summary>
        /// <param name="ProdId">Product标识</param>
        /// <param name="status">status</param>
        /// <param name="defectCodeType">defectCode Type</param>
        /// <returns>Product Repair列表</returns>
        //ITC-1360-0434, Jessica Liu, 2012-2-28
        IList<RepairInfo> GetOQCProdRepairList(string ProdId, int status, string defectCodeType);

        /// <summary>
        /// Cancel
        /// </summary>
        void Cancel(string prodId);

        //2012-8-1, Jessica Liu, 需求变更：增加Return Station 计算逻辑, 修改解DN条件
        /// <summary>
        /// GetReturnStation
        /// </summary>
        /// <param name="code"></param>
        /// <param name="preStn"></param>
        /// <param name="station"></param>
        /// <param name="cause"></param>
        /// <returns></returns>
        IList<DefectCodeStationInfo> GetReturnStation(int id, string code, string preStn, string station, string cause);

        //2012-8-1, Jessica Liu, 需求变更：增加Return Station 计算逻辑, 修改解DN条件
        /// <summary>
        /// GetReturnStationList
        /// </summary>
        /// <param name="prodid"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        IList<StationInfo> GetReturnStationList(string prodid, int status);
    }
}
