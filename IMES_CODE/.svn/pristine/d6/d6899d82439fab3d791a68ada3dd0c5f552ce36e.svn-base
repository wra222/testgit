// created by itc207024 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using IMES.DataModel;

namespace IMES.Maintain.Interface.MaintainIntf
{
    public interface IPdLineStation
    {
        /// <summary>
        /// 根据Line检索Line_Station
        /// </summary>
        /// <returns></returns>
        DataTable GetPdLineStationListByLine(string line);

        /// <summary>
        /// 取得全部的Stage 
        /// </summary>
        /// <returns></returns>
        IList<string> GetAllStage();

        /// <summary>
        ///取得PdLine 
        /// </summary>
        /// <returns></returns>
        IList<string> GetLineByCustAndStage(string Cust, string stage);

        /// <summary>
        /// 删除line和Station的关系
        /// </summary>
        /// <returns></returns>
        void DeleteLineStationByID(int id);

        /// <summary>
        /// 判断line和Station的关系是否已经存在
        /// </summary>
        /// <returns></returns>
        bool IFLineStationIsExists(string line, string station);

        /// <summary>
        /// 添加LineStation,返回新纪录的ID 
        /// </summary>
        /// <returns></returns>
        int AddLineStation(LineStation lineStation);


        /// <summary>
        /// 更新LineStation纪录 
        /// </summary>
        /// <returns></returns>
        void UpdateLineStation(LineStation lineStation);

        /// <summary>
        /// 取得station的列表
        /// </summary>
        /// <returns></returns>
        IList<SelectInfoDef> GetStationList();
    }
}
