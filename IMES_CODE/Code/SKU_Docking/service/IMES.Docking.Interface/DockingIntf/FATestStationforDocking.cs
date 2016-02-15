using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;

//namespace IMES.Station.Interface.StationIntf
namespace IMES.Docking.Interface.DockingIntf
{
    /// <summary>
    /// 记录后段测试结果：良品，记录pass信息；不良品，还需记录不良信息。
    /// 目的：记录后段测试结果
    /// </summary>
    public interface IFATestStation
    {
        /// <summary>
        /// 输入ProductId和相关信息, 然后卡站
        /// </summary>
        /// <param name="pdLine">Pd Line</param>
        /// <param name="testStation">Test station</param>
        /// <param name="prodId">Product Id</param>
        /// <param name="editor">operator</param>
        /// <param name="customerId">operator</param>
        //string InputProdId(
        IList<string>InputProdId(
            string pdLine,
            string testStation,
            string prodId,
            string editor, string customerId);

        string InputCustsn(
           out string pdLine,
            string testStation,
            string custsn,
            string editor, string customerId, out string defectStation, out bool isAllowPass, out string LastStation);
        /// <summary>
        /// 输入Defect Items然后保存 
        /// </summary>
        /// <param name="prodId">Product Id</param>
        /// <param name="defectList">Defect IList</param>

        IList<PrintItem> InputDefectCodeList(
            string prodId,
            IList<string> defectList,string thisTestTool, bool isNeedPrint2D, IList<PrintItem> printItems, out string qcMethod);

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="sessionKey"></param>
        void Cancel(string sessionKey);
    }
}
