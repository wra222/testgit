using System;
using System.Collections.Generic;
using IMES.DataModel.Rework;

namespace IMES.Station.Interface.StationIntf
{
    public interface IRework
    {
        /// <summary>
        /// COMMON - Query rework with condition set
        /// </summary>
        /// <param name="conditionList"></param>
        /// <returns></returns>
        IList<Rework> QueryResult(IList<String> conditionList);

        /// <summary>
        /// COMMON - Get rework list
        /// </summary>
        /// <returns></returns>
        IList<Rework> GetReworkList(); // ??? condition

        /// <summary>
        /// UC01 - User 按照某种规则查询到相关的unit，作为此次rework的机器
        /// </summary>
        /// <param name="resultList"></param>
        /// <param name="editor"></param>
        /// <param name="stationId"></param>
        /// <param name="customerId"></param>
        void SaveResult(IList<Rework> resultList,
            string editor, string stationId, string customerId);  // UC01

        /// <summary>
        /// UC02 - 显示所有Rework，以及每个rework对应的unit
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        IList<IUnit> GetReworkUnits(Rework r);      // UC02

        /// <summary>
        /// UC03 - User为选中的Rework设置Process
        /// </summary>
        /// <param name="r"></param>
        /// <param name="process"></param>
        /// <param name="editor"></param>
        /// <param name="stationId"></param>
        /// <param name="customerId"></param>
        void SetReworkProcess(Rework r, string process,
            string editor, string stationId, string customerId); // UC03

        /// <summary>
        /// UC04 - User为选中的Rework设置需要解绑的Part Type
        /// </summary>
        /// <param name="r"></param>
        /// <param name="type"></param>
        /// <param name="editor"></param>
        /// <param name="stationId"></param>
        /// <param name="customerId"></param>
        void SetReleaseType(Rework r, string type,
            string editor, string stationId, string customerId);      // UC04

        /// <summary>
        /// UC05 - User为选中的Rework作提交处理，以mail形式通知相关的Manager
        /// </summary>
        /// <param name="r"></param>
        /// <param name="editor"></param>
        /// <param name="stationId"></param>
        /// <param name="customerId"></param>
        void SubmitRework(Rework r /*, Attachement a*/,
            string editor, string stationId, string customerId); // UC05

        /// <summary>
        /// UC06 - User收到Manager的mail后，确认此次Rework开始
        /// </summary>
        /// <param name="r"></param>
        /// <param name="editor"></param>
        /// <param name="stationId"></param>
        /// <param name="customerId"></param>
        void ConfirmRework(Rework r,
            string editor, string stationId, string customerId);               // UC06

        /// <summary>
        /// UC07 - 删除还没有Confirm的Rework
        /// </summary>
        /// <param name="r"></param>
        /// <param name="editor"></param>
        /// <param name="stationId"></param>
        /// <param name="customerId"></param>
        void DeleteRework(Rework r,
            string editor, string stationId, string customerId);                // UC07

        /// <summary>
        /// UC08 - Rework机器按照rework process跑流程
        /// </summary>
        /// <param name="r"></param>
        /// <param name="editor"></param>
        /// <param name="stationId"></param>
        /// <param name="customerId"></param>
        void FollowReworkProcess(Rework r,
            string editor, string stationId, string customerId);         // UC08
    }
}
