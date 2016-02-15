// created by itc207024 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;

namespace IMES.Maintain.Interface.MaintainIntf
{
    public interface IRunInTimeControl
    {
        //从表RunInTimeControl取得Family列表 
        IList<string> GetFamilyListFromRunInTimeControl();

        //根据Type取得RunInTimeControl列表
        IList<RunInTimeControlInfoMaintain> GetRunInTimeControlListByType(string type);

        //根据CPQSNO取得RunInTimeControl列表
        IList<RunInTimeControlInfoMaintain> GetRunInTimeControlListByCPQSNO(string type);

        //根据Type和Code取得RunInTimeControl 
        RunInTimeControlInfoMaintain GetRunInTimeControlByTypeAndCode(string type, string code);

        //更新RunInTimeControl,返回ID (对象带出)
        //int UpdateRunInTimeControlByTypeAndCode(RunInTimeControlInfoMaintain runInTimeControl);

        //记录log信息
        //void InsertRunInTimeControlLog(string type, string code);

        //新增RunInTimeControl纪录，返回ID (对象带出)
        int InsertRunInTimeControl(RunInTimeControlInfoMaintain runInTimeControl);

        //删除RunInTimeControl纪录
        //void DeleteRunInTimeControl(string type, string code);

        //判断Product是否存在
        bool IfProductIsExists(string custSN);

        //判断Family是否存在
        bool IfFamilyIsExists(string family);

        //根据type、code和teststation修改数据
        //void UpdateRunInTimeControlByTypeCodeAndTestStation(RunInTimeControlInfoMaintain runInTimeControl, string type, string code, string testStation);

        //根据type、code和teststation获取runintimecontrol数据
        RunInTimeControlInfoMaintain getRunintimeControlByCodeTypeAndTestStation(string type, string code, string testStation);

        //根据type，code和teststation删除runintimcontrol数据
        //void DeleteRunInTimeControlByTypeCodeAndTeststation(string type, string code, string teststation);

        void DeleteRunInTimeControlById(string idStr);

        void UpdateRunInTimeControlById(RunInTimeControlInfoMaintain runInTimeControlInfoMaintain);
    }
}
