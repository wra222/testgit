using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;

namespace IMES.Maintain.Interface.MaintainIntf
{
    public interface IDefectStation
    {
        /// <summary>
        /// 查询Defect Station(DefectCode_Station)
        /// </summary>
        /// <returns></returns>
        IList<DefectCodeStationInfo> GetDefectList();

        bool CheckFamily(string family);

        bool CheckModel(string model);

        /// <summary>
        /// 添加DefectStationInfo
        /// </summary>
        /// <param name="defectStation"></param>
        /// <returns>返回被添加数据的ID</returns>      
        string AddDefectStation(DefectCodeStationInfo defectStation);

        /// <summary>
        /// 删除选中的DefectStation
        /// </summary>
        /// <param name="id"></param>
        void DeleteDefectStation(int id);

        /// <summary>
        /// 更新DefectStation
        /// </summary>
        /// <param name="newItem"></param>
        void UpdateDefectStation(DefectCodeStationInfo stationItem);

        /// <summary>
        /// 获取defect Code列表
        /// </summary>
        /// <returns></returns>
        IList<IMES.DataModel.DefectInfo> GetDefectCodeList();

        /// <summary>
        /// 获取Station列表
        /// </summary>
        /// <returns></returns>
        IList<StationMaintainInfo> GetStationList();

        IList<string> GetPreStationFromDefectStation();

        IList<List<string>> GetMajorPartList(string type, string customer);

        IList<DefectCodeStationInfo> GetDefectStationByPreStation(string preStation);

        bool CheckDefectStationUnique(string preStation, string curStation, string majorPart, string cause, string defect);

    }
}
