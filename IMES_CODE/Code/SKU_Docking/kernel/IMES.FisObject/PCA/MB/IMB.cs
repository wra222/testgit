using System;
using System.Collections.Generic;
using IMES.FisObject.Common.CheckItem;
using IMES.FisObject.Common.Model;
using IMES.FisObject.Common.Part;
using IMES.FisObject.Common.Repair;
using IMES.FisObject.Common.TestLog;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure.FisObjectBase;

namespace IMES.FisObject.PCA.MB
{
    /// <summary>
    /// �������ݶ���ӿڡ�
    /// ͨ��һ��ӿڷ�װ��ʹ���ǿ��������ݶ���������ӳٲ�������������Ͳ�����ȡ�ȹ��ܶ����������ô���ͷ�����
    /// </summary>
    public interface IMB : IAggregateRoot, ICheckObject, IRepairTarget, IPartOwner
    {
        /// <summary>
        /// ������š��ַ������͡�
        /// �����������Ψһ��ʶһ�����塣
        /// </summary>
        string Sn { get; }

        /// <summary>
        /// Same table Column name
        /// </summary>
        string PCBNo { get; }

        /// <summary>
        /// Custom Sn
        /// </summary>
        string CustSn { get; set; }

        /// <summary>
        /// Same PCB Table column Name
        /// </summary>
        string CUSTSN { get; }

        /// <summary>
        /// ���충���š��ַ������͡�
        /// ���충��������Ψһ��ʶһ�����충����һ�����충��ȷ���������ٿ�ĳһ�ͺŵ����塣
        /// </summary>
        string SMTMO { get; }

        /// <summary>
        /// �����ͺš��ַ������͡�
        /// �����ͺ�����Ψһ��ʶһ�����塣
        /// </summary> 
        new string Model { get; set; }

        /// <summary>
        /// �����ͺš��ַ������͡�
        /// �����ͺ�����Ψһ��ʶһ�����塣
        /// </summary> 
        string PCBModelID { get; set; }

        /// <summary>
        /// �����롣�ַ������͡�
        /// ����������롣
        /// </summary>
        string DateCode { get; set; }

        /// <summary>
        /// ý���ȡ�����롣�ַ������͡�
        /// ý���ȡ�������׳�MAC��ַ����������̫������Ψһ��ʶһ̨ͨ���豸������ַ��
        /// </summary>
        string MAC { get; set; }

        /// <summary>
        /// ͨ��Ψһ��ʶ�����ַ������͡�
        /// �����ȫ��Ψһ��ʶ��Ψһ��ʶһ�����塣
        /// </summary>
        string UUID { get; set; }

        /// <summary>
        /// ???
        /// </summary>
        string ECR { get; set; }

        /// <summary>
        /// IEC�汾�š��ַ������͡�
        /// IEC���������汾�š�
        /// </summary>
        string IECVER { get; set; }

        /// <summary>
        /// �ͻ��汾�š��ַ������͡�
        /// �ͻ����������汾�š�
        /// </summary>
        string CUSTVER { get; set; }

        string CVSN { get; set; }

        string State { get; set; }

        string ShipMode { get; set; }

        decimal CartonWeight { get; set; }
        decimal UnitWeight { get; set; }
        string PizzaID { get; set; }
        string CartonSN { get; set; }
        string DeliveryNo { get; set; }
        string PalletNo { get; set; }
        string QCStatus { get; set; }
        string SkuModel { get; set; }

        /// <summary>
        /// ��¼���������롣�������͡�
        /// ��¼�������ݵĸ������ں�ʱ�䡣
        /// </summary>
        DateTime Udt { get; }

        /// <summary>
        /// ��¼���������롣�������͡�
        /// ��¼�������ݵĴ������ں�ʱ�䡣
        /// </summary>
        DateTime Cdt { get; }

        /// <summary>
        /// ��������״̬��ʶ��ö�����͡�
        /// ��ʶ���嵱ǰ������״̬��
        /// </summary>
        MBStatus MBStatus { get; set; }

        IList<IProductPart> MBParts { get; }
        IList<MBInfo> MBInfos { get; }
        IList<Repair> Repairs { get; }
        IList<MBLog> MBLogs { get; }
        IList<TestLog> TestLogs { get; }
        IList<ProductAttribute> PCBAttributes { get; }
        IList<ProductAttributeLog> PCBAttributeLogs { get; }

        /// <summary>
        /// Product�������Ͷ���
        /// </summary>
        MBModel.MBModel ModelObj { get; }

        /// <summary>
        /// ����1397��model
        /// </summary>
        string MB1397 { get; set; }

        /// <summary>
        /// ����1397��Model����
        /// </summary>
        Model Model1397Obj { get;  }

        /// <summary>
        /// 1397�׶�Ӧ��cusomer, ֻ��������ڶ�Ӧ1397��ʱ����ֵ
        /// </summary>
        string Customer { get; }

        /// <summary>
        /// ��һ����/����վ���������͡�
        /// ������һ������/����վ����
        /// (��ʱ����)
        /// </summary>
        //IMES.FisObject.Common.Station.Station NextStation { get; }

        /// <summary>
        /// ȷ���Ƿ���FA�׶Ρ��������͡�
        /// �������嵱ǰ�Ƿ���FA�׶Ρ�
        /// </summary>
        //bool IsInFAPhrase { get; }

        /// <summary>
        /// �����и�״̬���������͡�
        /// ���������Ƿ������������и
        /// </summary>
        bool HasBeenCut { get; }

        /// <summary>
        /// �Ƿ���Trial Run����
        /// </summary>
        bool IsTrialRun { get; }

        /// <summary>
        /// �Ƿ���VB
        /// </summary>
        bool IsVB { get; }

        /// <summary>
        /// �Ƿ���RCTO
        /// </summary>
        bool IsRCTO { get; }

        /// <summary>
        /// mb rpt repair history
        /// </summary>
        IList<MBRptRepair> MBRptRepairs { get; }

        /// <summary>
        /// Family
        /// </summary>
        string Family { get; }

        /// <summary>
        /// mbcode
        /// </summary>
        String MBCode { get; }

        /// <summary>
        /// ������־��
        /// </summary>
        /// <param name="log">��־����</param>
        void AddLog(MBLog log);

        /// <summary>
        /// Get Latest Fail Log
        /// </summary>
        /// <returns></returns>
        MBLog GetLatestFailLog();

        /// <summary>
        /// �������ָ���������Ӱ�
        /// </summary>
        /// <param name="qty">�Ӱ�����</param>
        void GenerateChildMB(int qty);

        /// <summary>
        /// ����MTA��ǡ�
        /// </summary>
        void SetMTAMark();

        /// <summary>
        /// ��λMTA��ǡ�
        /// </summary>
        void ResetMTAMark();

        /// <summary>
        /// ��CPU��Ӧ����Ϣ��
        /// </summary>
        /// <param name="vendor">CPU��Ӧ�̱�ʶ(�ַ���)</param>
        void BindCPUVendor(string vendor);

        /// <summary>
        /// ��ȡָ��station��log
        /// </summary>
        /// <param name="station">station</param>
        /// <param name="isPass">isPass</param>
        /// <returns>MBLog</returns>
        MBLog GetLogByStation(string station, bool isPass);

        /// <summary>
        /// ���������TestLogDefect
        /// </summary>
        /// <param name="testLogId">testLogId</param>
        /// <param name="defect">Test log defect</param>
        void AddTestLogDefect(int testLogId, TestLogDefect defect);

        /// <summary>
        /// ɾ�������TestLogDefect
        /// </summary>
        /// <param name="testLogId">testLogId</param>
        /// <param name="testLogDefectId">testLogDefectId</param>
        void RemoveTestLogDefect(int testLogId, int testLogDefectId);

        /// <summary>
        /// ɾ�������TestLogDefect
        /// </summary>
        /// <param name="testLogId">testLogId</param>
        /// <param name="defect">defect</param>
        void UpdateTestLogDefect(int testLogId, TestLogDefect defect);

        /// <summary>
        /// �����Ƴ̿���״ֵ̬
        /// </summary>
        /// <param name="name">������</param>
        /// <param name="value">����ֵ</param>
        /// <param name="editor">Editor</param>
        /// <param name="station">Station</param>
        /// <param name="descr">Description</param>
        /// <remarks>
        /// ��ָ�������Ѵ����򸲸�ԭ����ֵͬʱ��¼PCBAttributeLog,������������
        /// </remarks>
        void SetAttributeValue(string name, string value, string editor, string station, string descr);

        /// <summary>
        /// ��ȡָ������ֵ
        /// </summary>
        /// <param name="name">������</param>
        /// <returns>��������ֵ,ָ���������������򷵻�null</returns>
        string GetAttributeValue(string name);

        /// <summary>
        /// ������������
        /// </summary>
        /// <param name="info">��������</param>
        void AddMBInfo(MBInfo info);

        /// <summary>
        /// Add rpt repair record
        /// </summary>
        /// <param name="rptRepair"></param>
        void AddRptRepair(MBRptRepair rptRepair);
        
        /// <summary>
        /// Set a new PCB serial number.
        /// </summary>
        /// <param name="newSn"></param>
        void SetSn(string newSn);

        /// <summary>
        /// get from PCBInfo , PCBAttr
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        string GetAttribute(string name);
        /// <summary>
        /// 131 ���Ƹ� PCBModelID 
        /// </summary>
        IPart Part { get; }
    }
}