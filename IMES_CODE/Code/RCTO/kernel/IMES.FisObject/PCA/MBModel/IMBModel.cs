using IMES.Infrastructure.FisObjectBase;

namespace IMES.FisObject.PCA.MBModel
{
    /// <summary>
    /// MBModel对象接口
    /// </summary>
    public interface IMBModel : IAggregateRoot
    {
        /// <summary>
        /// 主板型号零件号。
        /// </summary>
        string Pn { get; }

        /// <summary>
        /// 主板型号代码。
        /// </summary>
        string Mbcode { get; }

        /// <summary>
        /// Mdl
        /// </summary>
        string Mdl { get; }

        /// <summary>
        /// 主板类型：MB/SB/VB
        /// </summary>
        string Type { get; }

        /// <summary>
        /// 是否为连扳
        /// </summary>
        bool IsCompositeBoard { get; }

        /// <summary>
        /// Whether VB or not
        /// </summary>
        bool IsVB { get; }
    }
}