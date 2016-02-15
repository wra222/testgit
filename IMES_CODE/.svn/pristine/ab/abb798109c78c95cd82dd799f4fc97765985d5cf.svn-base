using System.Collections.Generic;
using IMES.FisObject.Common.Part;

namespace IMES.FisObject.Common.Repair
{
    ///<summary>
    /// Repair的目标对象接口
    ///</summary>
    public interface IRepairTarget
    {
        /// <summary>
        /// RepairTarget机型
        /// </summary>
        string RepairTargetModel { get; }

        /// <summary>
        /// 是否初次维修
        /// </summary>
        bool IsFirstRepair { get; }

        /// <summary>
        /// 最新的测试Fail log
        /// </summary>
        TestLog.TestLog LatestFailTestLog { get;  }

        /// <summary>
        /// 最新的测试log
        /// </summary>
        TestLog.TestLog LatestTestLog { get; }

        /// <summary>
        /// 取得测试日志集合。通常用于查找或检查特定日志信息。
        /// </summary>
        /// <returns>返回测试日志对象列表。</returns>
        IList<TestLog.TestLog> GetTestLog();

        /// <summary>
        /// 添加测试记录
        /// </summary>
        /// <param name="testLog">测试记录</param>
        void AddTestLog(TestLog.TestLog testLog);

        /// <summary>
        /// 向主板中添加一条添加维修记录。
        /// </summary>
        /// <param name="rep">维修记录对象</param>
        void AddRepair(Repair rep);

        /// <summary>
        /// 为指定Repair增加一个RepairDefect
        /// </summary>
        /// <param name="repairId">指定Repair的Id</param>
        /// <param name="defect"></param>
        void AddRepairDefect(int repairId, RepairDefect defect);

        /// <summary>
        /// 为指定Repair删除一个RepairDefect
        /// </summary>
        /// <param name="repairId">指定Repair的Id</param>
        /// <param name="repairDefectId">指定RepairDefect的Id</param>
        void RemoveRepairDefect(int repairId, int repairDefectId);

        /// <summary>
        /// 为指定Repair更新一个RepairDefect
        /// </summary>
        /// <param name="repairId">指定Repair的Id</param>
        /// <param name="defect">指定RepairDefect</param>
        void UpdateRepairDefect(int repairId, RepairDefect defect);

        /// <summary>
        /// 取得主板所有维修记录。
        /// </summary>
        /// <returns>维修对象集合</returns>
        IList<Repair> GetRepair();

        /// <summary>
        /// 取得主板当前维修记录。
        /// </summary>
        /// <returns>当前维修对象</returns>
        Repair GetCurrentRepair();

        /// <summary>
        /// 获取指定Site+Component的维修历史
        /// </summary>
        /// <param name="site">site</param>
        /// <param name="component">component</param>
        /// <returns>维修历史</returns>
        IList<RepairDefect> GetRepairDefectBySiteComponent(string site, string component);

        /// <summary>
        /// complte一次维修
        /// </summary>
        void CompleteRepair(string line, string station, string editor);

        /// <summary>
        /// 
        /// </summary>
        //void GenerateRepairByTestLog();

        /// <summary>
        /// 将指定partID对应的Part替换
        /// </summary>
        /// <param name="partID">指定partID</param>
        /// <param name="newPart">新Part</param>
        void ChangePart(int partID, IProductPart newPart);


        /// <summary>
        /// 为指定Repair删除一个RepairDefect
        /// </summary>
        /// <param name="repairId">指定Repair的Id</param>
        void RemoveRepair(int repairId);

        /// <summary>
        /// delete all Repair/RepairDefectInfo
        /// </summary>
        void RemoveAllRepair();
    }
}