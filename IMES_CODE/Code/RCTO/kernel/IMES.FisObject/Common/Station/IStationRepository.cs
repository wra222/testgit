using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectRepositoryFramework;
//
using IMES.DataModel;
using System.Data;
using IMES.Infrastructure.UnitOfWork;

namespace IMES.FisObject.Common.Station
{
    ///<summary>
    ///</summary>
    public interface IStationRepository : IRepository<IStation>
    {
        #region . For CommonIntf  .

        /// <summary>
        /// 根据StationType获得Station列表
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        IList<IStation> GetStationList(StationType type);

        /// <summary>
        /// 取得FA测试站信息列表
        /// </summary>
        /// <returns>测试站信息列表</returns>
        IList<TestStationInfo> GetFATestStationList();

        /// <summary>
        /// 取得SA测试站信息列表
        /// </summary>
        /// <returns>测试站信息列表</returns>
        IList<TestStationInfo> GetSATestStationList();

        /// <summary>
        /// Station.Descr where Descr like ‘%Test %’ and Stage_Station.StageID=’FA’
        /// 如与上面GetTestStationList(string type)重复请告知207006
        /// </summary>
        /// <returns></returns>
        IList<TestStationInfo> GetTestStationListFor023();
        #endregion

        /// <summary>
        /// 获得Station的描述
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        string TransToDesc(string code);

        /// <summary>
        /// 24) 新增 
        /// SELECT [Station],[Name],[Descr] 
        /// FROM [Station] where rtrim([StationType])=@type
        /// order by Station
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        DataTable GetLightNoStationList(string type);

        /// <summary>
        /// SELECT Descr FROM Station WHERE StationType = 'PAKKitting' ORDER BY Descr
        /// </summary>
        /// <param name="stationType"></param>
        /// <returns></returns>
        IList<IStation> GetStationListFromStation(string stationType);

        /// <summary>
        /// 根据条件删除，实体对象，给哪个赋值即使用哪个作条件，条件之间的关系是AND
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        IList<DefectCodeStationInfo> GetDefectCodeStationList(DefectCodeStationInfo condition);

        IList<DefectCodeStationInfo> GetDefectCodeStationList(DefectCodeStationInfo eqCondition, DefectCodeStationInfo isNullCondition);

        /// <summary>
        /// select a.* , pres.Name as preName,curs.Name as curName ,nxts.Name as nxtName,dc.Descr as dfDescr
        /// from [DefectCode_Station] as a
        /// join Station as pres on pres.Station = a.PRE_STN
        /// join Station as curs on curs.Station = a.CRT_STN
        /// join Station as nxts on nxts.Station = a.NXT_STN
        /// join DefectCode as dc on dc.Defect = a.Defect
        /// order by a.PRE_STN,a.CRT_STN,a.Defect
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        IList<DefectCodeStationInfo> GetDefectCodeStationList2(DefectCodeStationInfo condition);

        /// <summary>
        /// 存在返回true
        /// SQL: select * from station where name=[name]
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        bool CheckStationExistByName(string name);

        /// <summary>
        /// SQL: select * from station where name=[name]
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        IList<Station> GetStationItemByName(string name);

        /// <summary>
        /// SQL: select * from station where stationID=[sataion]
        /// </summary>
        /// <param name="staion"></param>
        /// <returns></returns>
        IList<Station> GetStationItemByStationID(string staion);

        /// <summary>
        /// 取得Station的方法：Select Station, Name from Station where StationType not like‘SA%’order by Station
        /// </summary>
        /// <param name="exceptStationTypePrefix"></param>
        /// <returns></returns>
        IList<StationInfo> GetStationInfoListByNotLikeStationType(string exceptStationTypePrefix);

        /// <summary>
        /// StationCheck表的查接口
        /// (按Station、Line、CheckItem Type栏位排序)
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        IList<StationCheckInfo> GetStationCheckInfoList(StationCheckInfo condition);

        /// <summary>
        /// StationCheck表的增接口
        /// </summary>
        /// <param name="item"></param>
        void InsertStationCheckInfo(StationCheckInfo item);

        /// <summary>
        /// StationCheck表的改接口
        /// </summary>
        /// <param name="setValue"></param>
        /// <param name="condition"></param>
        void UpdateStationCheckInfo(StationCheckInfo setValue, StationCheckInfo condition);

        /// <summary>
        /// StationCheck表的删接口
        /// </summary>
        /// <param name="condition"></param>
        void DeleteStationCheckInfo(StationCheckInfo condition);

        /// <summary>
        /// Select Station ,...,Name from Station order by Station
        /// </summary>
        /// <returns></returns>
        IList<IStation> GetStationOrderByStation();

        #region . Defered .

        void InsertStationCheckInfoDefered(IUnitOfWork uow, StationCheckInfo item);

        void UpdateStationCheckInfoDefered(IUnitOfWork uow, StationCheckInfo setValue, StationCheckInfo condition);

        void DeleteStationCheckInfoDefered(IUnitOfWork uow, StationCheckInfo condition);

        #endregion

        #region For Maintain

        /// <summary>
        /// 27)SELECT [Station]
        /// FROM [IMES_GetData_Datamaintain].[dbo].[Station]
        /// ORDER BY [Station]
        /// </summary>
        /// <returns></returns>
        DataTable GetStationList();

        /// <summary>
        /// SELECT [Station]
        ///       ,[StationType]
        ///       ,[OperationObject]
        ///       ,[Descr]
        ///       ,[Editor]
        ///       ,[Cdt]
        ///       ,[Udt]
        ///   FROM [IMES_GetData_Datamaintain].[dbo].[Station]
        /// order by [Station]
        /// 需要按照Code栏位进行排序.
        /// </summary>
        /// <returns></returns>
        DataTable GetStationInfoList();

        /// <summary>
        /// DELETE FROM [Station]
        ///       WHERE Station='station'
        /// </summary>
        /// <param name="station"></param>
        void DeleteStation(string station);

        /// <summary>
        /// IF EXISTS(
        /// SELECT [Station]     
        ///   FROM [Station]
        /// where Station ='station'
        /// )
        /// set @return='True'
        /// ELSE
        /// set @return='False'
        /// </summary>
        /// <param name="station"></param>
        /// <returns></returns>
        bool IsExistStation(string station);

        /// <summary>
        /// 新增Station
        /// </summary>
        /// <param name="item"></param>
        void AddStation(Station item);

        /// <summary>
        /// 修改Station
        /// </summary>
        /// <param name="item"></param>
        /// <param name="oldStationId"></param>
        void UpdateStation(Station item, string oldStationId);

        void InsertDefectCodeStationInfo(DefectCodeStationInfo item);

        void UpdateDefectCodeStationInfo(DefectCodeStationInfo setValue, DefectCodeStationInfo condition);

        void DeleteDefectCodeStationInfo(DefectCodeStationInfo condition);

        #region Defered

        void DeleteStationDefered(IUnitOfWork uow, string station);

        void AddStationDefered(IUnitOfWork uow, Station item);

        void UpdateStationDefered(IUnitOfWork uow, Station item, string oldStationId);

        void InsertDefectCodeStationInfoDefered(IUnitOfWork uow, DefectCodeStationInfo item);

        void UpdateDefectCodeStationInfoDefered(IUnitOfWork uow, DefectCodeStationInfo setValue, DefectCodeStationInfo condition);

        void DeleteDefectCodeStationInfoDefered(IUnitOfWork uow, DefectCodeStationInfo condition);

        #endregion

        #endregion

        #region  for DefectCode_Station add New Major part
        IList<DefectCodeStationInfo> GetDefectStationByCurStation(string curStation);
        IList<DefectCodeStationInfo> GetDefectStationByPreStation(string preStation);
        IList<DefectCodeStationInfo> GetDefectStation(string preStation, string curStation);
        bool CheckDefectStationUnique(string preStation, string curStation, string majorPart, string cause, string defect);
        IList<DefectCodeNextStationInfo> GetNextStationFromDefectStation(string preStation, string curStation, string majorPart, string cause, string defect);
        IList<string> GetCurStationFromDefectStation();
        IList<string> GetPreStationFromDefectStation();

        //void InsertDefectStationInfo(DefectCodeStationExInfo item);
        //void UpdateDefectStationInfo(DefectCodeStationExInfo item);
        void DeleteDefectStationInfo(int id);

        bool CheckDefectStationUnique(string preStation, string curStation, string majorPart, string cause, string defect, string family);
        IList<DefectCodeNextStationInfo> GetNextStationFromDefectStation(string preStation, string curStation, string majorPart, string cause, string defect, string family, string model);
        
        #endregion

        #region for StationAttr table
        IList<StationAttrDef> GetStationAttr(string station);
        String GetStationAttrValue(string station, string attrName);
        IList<string> GetStationByAttrNameValue(string attrName, string attrValue);

        void AddStationAttr(StationAttrDef attr);
        void UpdateStationAttr(StationAttrDef attr);
        void DeleteStationAttr(string station, string attrName);

        #endregion
    }
}
