using System.Collections.Generic;
using IMES.FisObject.Common.Part;

namespace IMES.FisObject.Common.Repair
{
    ///<summary>
    /// Repair��Ŀ�����ӿ�
    ///</summary>
    public interface IRepairTarget
    {
        /// <summary>
        /// RepairTarget����
        /// </summary>
        string RepairTargetModel { get; }

        /// <summary>
        /// �Ƿ����ά��
        /// </summary>
        bool IsFirstRepair { get; }

        /// <summary>
        /// ���µĲ���Fail log
        /// </summary>
        TestLog.TestLog LatestFailTestLog { get;  }

        /// <summary>
        /// ���µĲ���log
        /// </summary>
        TestLog.TestLog LatestTestLog { get; }

        /// <summary>
        /// ȡ�ò�����־���ϡ�ͨ�����ڲ��һ����ض���־��Ϣ��
        /// </summary>
        /// <returns>���ز�����־�����б�</returns>
        IList<TestLog.TestLog> GetTestLog();

        /// <summary>
        /// ��Ӳ��Լ�¼
        /// </summary>
        /// <param name="testLog">���Լ�¼</param>
        void AddTestLog(TestLog.TestLog testLog);

        /// <summary>
        /// �����������һ�����ά�޼�¼��
        /// </summary>
        /// <param name="rep">ά�޼�¼����</param>
        void AddRepair(Repair rep);

        /// <summary>
        /// Ϊָ��Repair����һ��RepairDefect
        /// </summary>
        /// <param name="repairId">ָ��Repair��Id</param>
        /// <param name="defect"></param>
        void AddRepairDefect(int repairId, RepairDefect defect);

        /// <summary>
        /// Ϊָ��Repairɾ��һ��RepairDefect
        /// </summary>
        /// <param name="repairId">ָ��Repair��Id</param>
        /// <param name="repairDefectId">ָ��RepairDefect��Id</param>
        void RemoveRepairDefect(int repairId, int repairDefectId);

        /// <summary>
        /// Ϊָ��Repair����һ��RepairDefect
        /// </summary>
        /// <param name="repairId">ָ��Repair��Id</param>
        /// <param name="defect">ָ��RepairDefect</param>
        void UpdateRepairDefect(int repairId, RepairDefect defect);

        /// <summary>
        /// ȡ����������ά�޼�¼��
        /// </summary>
        /// <returns>ά�޶��󼯺�</returns>
        IList<Repair> GetRepair();

        /// <summary>
        /// ȡ�����嵱ǰά�޼�¼��
        /// </summary>
        /// <returns>��ǰά�޶���</returns>
        Repair GetCurrentRepair();

        /// <summary>
        /// ��ȡָ��Site+Component��ά����ʷ
        /// </summary>
        /// <param name="site">site</param>
        /// <param name="component">component</param>
        /// <returns>ά����ʷ</returns>
        IList<RepairDefect> GetRepairDefectBySiteComponent(string site, string component);

        /// <summary>
        /// complteһ��ά��
        /// </summary>
        void CompleteRepair(string line, string station, string editor);

        /// <summary>
        /// 
        /// </summary>
        //void GenerateRepairByTestLog();

        /// <summary>
        /// ��ָ��partID��Ӧ��Part�滻
        /// </summary>
        /// <param name="partID">ָ��partID</param>
        /// <param name="newPart">��Part</param>
        void ChangePart(int partID, IProductPart newPart);


        /// <summary>
        /// Ϊָ��Repairɾ��һ��RepairDefect
        /// </summary>
        /// <param name="repairId">ָ��Repair��Id</param>
        void RemoveRepair(int repairId);

        /// <summary>
        /// delete all Repair/RepairDefectInfo
        /// </summary>
        void RemoveAllRepair();
    }
}