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
    /// 主板数据对象接口。
    /// 通过一层接口封装，使我们可以在数据对象层次完成延迟操作、缓存操作和部分提取等功能而无需变更调用代码和方法。
    /// </summary>
    public interface IMB : IAggregateRoot, ICheckObject, IRepairTarget, IPartOwner
    {
        /// <summary>
        /// 主板序号。字符串类型。
        /// 主板序号用于唯一标识一块主板。
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
        /// 制造订单号。字符串类型。
        /// 制造订单号用于唯一标识一个制造订单。一个制造订单确定生产多少块某一型号的主板。
        /// </summary>
        string SMTMO { get; }

        /// <summary>
        /// 主板型号。字符串类型。
        /// 主板型号用于唯一标识一种主板。
        /// </summary> 
        new string Model { get; set; }

        /// <summary>
        /// 主板型号。字符串类型。
        /// 主板型号用于唯一标识一种主板。
        /// </summary> 
        string PCBModelID { get; set; }

        /// <summary>
        /// 日期码。字符串类型。
        /// 主板的日期码。
        /// </summary>
        string DateCode { get; set; }

        /// <summary>
        /// 媒体存取控制码。字符串类型。
        /// 媒体存取控制码俗称MAC地址，用于在以太网络上唯一标识一台通信设备物理地址。
        /// </summary>
        string MAC { get; set; }

        /// <summary>
        /// 通用唯一标识符。字符串类型。
        /// 主板的全球唯一标识符唯一标识一块主板。
        /// </summary>
        string UUID { get; set; }

        /// <summary>
        /// ???
        /// </summary>
        string ECR { get; set; }

        /// <summary>
        /// IEC版本号。字符串类型。
        /// IEC定义的主板版本号。
        /// </summary>
        string IECVER { get; set; }

        /// <summary>
        /// 客户版本号。字符串类型。
        /// 客户定义的主板版本号。
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
        /// 记录更新日期码。日期类型。
        /// 记录该条数据的更新日期和时间。
        /// </summary>
        DateTime Udt { get; }

        /// <summary>
        /// 记录创建日期码。日期类型。
        /// 记录该条数据的创建日期和时间。
        /// </summary>
        DateTime Cdt { get; }

        /// <summary>
        /// 主板生产状态标识。枚举类型。
        /// 标识主板当前的生产状态。
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
        /// Product所属机型对象
        /// </summary>
        MBModel.MBModel ModelObj { get; }

        /// <summary>
        /// 主板1397阶model
        /// </summary>
        string MB1397 { get; set; }

        /// <summary>
        /// 主板1397阶Model对象
        /// </summary>
        Model Model1397Obj { get;  }

        /// <summary>
        /// 1397阶对应的cusomer, 只有主板存在对应1397阶时才有值
        /// </summary>
        string Customer { get; }

        /// <summary>
        /// 下一生产/测试站。对象类型。
        /// 返回下一个生产/测试站对象。
        /// (暂时无用)
        /// </summary>
        //IMES.FisObject.Common.Station.Station NextStation { get; }

        /// <summary>
        /// 确定是否在FA阶段。布尔类型。
        /// 返回主板当前是否在FA阶段。
        /// </summary>
        //bool IsInFAPhrase { get; }

        /// <summary>
        /// 主板切割状态。布尔类型。
        /// 返回主板是否已做过连扳切割。
        /// </summary>
        bool HasBeenCut { get; }

        /// <summary>
        /// 是否是Trial Run主板
        /// </summary>
        bool IsTrialRun { get; }

        /// <summary>
        /// 是否是VB
        /// </summary>
        bool IsVB { get; }

        /// <summary>
        /// 是否是RCTO
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
        /// 添加日志。
        /// </summary>
        /// <param name="log">日志对象</param>
        void AddLog(MBLog log);

        /// <summary>
        /// Get Latest Fail Log
        /// </summary>
        /// <returns></returns>
        MBLog GetLatestFailLog();

        /// <summary>
        /// 连板产生指定数量的子板
        /// </summary>
        /// <param name="qty">子板数量</param>
        void GenerateChildMB(int qty);

        /// <summary>
        /// 设置MTA标记。
        /// </summary>
        void SetMTAMark();

        /// <summary>
        /// 复位MTA标记。
        /// </summary>
        void ResetMTAMark();

        /// <summary>
        /// 绑定CPU供应商信息。
        /// </summary>
        /// <param name="vendor">CPU供应商标识(字符串)</param>
        void BindCPUVendor(string vendor);

        /// <summary>
        /// 获取指定station的log
        /// </summary>
        /// <param name="station">station</param>
        /// <param name="isPass">isPass</param>
        /// <returns>MBLog</returns>
        MBLog GetLogByStation(string station, bool isPass);

        /// <summary>
        /// 增加主板的TestLogDefect
        /// </summary>
        /// <param name="testLogId">testLogId</param>
        /// <param name="defect">Test log defect</param>
        void AddTestLogDefect(int testLogId, TestLogDefect defect);

        /// <summary>
        /// 删除主板的TestLogDefect
        /// </summary>
        /// <param name="testLogId">testLogId</param>
        /// <param name="testLogDefectId">testLogDefectId</param>
        void RemoveTestLogDefect(int testLogId, int testLogDefectId);

        /// <summary>
        /// 删除主板的TestLogDefect
        /// </summary>
        /// <param name="testLogId">testLogId</param>
        /// <param name="defect">defect</param>
        void UpdateTestLogDefect(int testLogId, TestLogDefect defect);

        /// <summary>
        /// 设置制程控制状态值
        /// </summary>
        /// <param name="name">属性名</param>
        /// <param name="value">属性值</param>
        /// <param name="editor">Editor</param>
        /// <param name="station">Station</param>
        /// <param name="descr">Description</param>
        /// <remarks>
        /// 若指定属性已存在则覆盖原属性值同时记录PCBAttributeLog,否则新增属性
        /// </remarks>
        void SetAttributeValue(string name, string value, string editor, string station, string descr);

        /// <summary>
        /// 获取指定属性值
        /// </summary>
        /// <param name="name">属性名</param>
        /// <returns>返回属性值,指定属性若不存在则返回null</returns>
        string GetAttributeValue(string name);

        /// <summary>
        /// 增加主板属性
        /// </summary>
        /// <param name="info">主板属性</param>
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
        /// 131 顶腹 PCBModelID 
        /// </summary>
        IPart Part { get; }
    }
}