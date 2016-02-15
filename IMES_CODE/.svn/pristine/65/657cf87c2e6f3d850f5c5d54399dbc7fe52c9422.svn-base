using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMES.Station.Interface.StationIntf
{
    /// <summary>
    /// 刷入需要进行Rework Output 的笔记本的Product Id，确认该笔记本重新进入指定的生产流程
    /// </summary>
    public interface IReworkOutput
    {
        /// <summary>
        /// 输入ProductId并检查
        /// </summary>
        /// <param name="pdLine">Pd Line</param>
        /// <param name="reworkCode">Rework Code</param>
        /// <param name="prodId">Product Id</param>
        /// <param name="editor">operator</param>
        void InputProdId(
            string pdLine,
            string reworkCode,
            string prodId,
            string editor, string stationId, string customerId);

        /// <summary>
        /// Close Rework Code
        /// </summary>
        /// <param name="pdLine">Pd Line</param>
        /// <param name="reworkCode">Rework Code</param>
        /// <param name="editor">operator</param>
        void CloseReworkCode(
            string pdLine,
            string reworkCode,
            string editor, string stationId, string customerId);
    }
}
