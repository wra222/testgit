// created by itc205033

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;

namespace IMES.Station.Interface.StationIntf
{
    /// <summary>
    /// [PCA Test Station] 实现如下功能：
    /// 记录Function测试结果：如有不良,还需记录不良信息
    /// </summary>
    public interface IPCATestStation
    {
        /// <summary>
        /// 输入MB_SNo, 然后卡站，最后返回Model。
        /// </summary>
        /// <param name="pdLine">Pd Line</param>
        /// <param name="testStation">Test Station</param>
        /// <param name="MB_SNo">MB SNo</param>
        /// <param name="editor">operator</param>
        /// <returns>返回111阶号</returns>
        IList<string> InputMBSNo(
            string pdLine,
            string testStation,
            string MB_SNo,
            string editor, string customerId, out string AllowPass, out string DefectStation);

        /// <summary>
        /// InputMBSNoForLot
        /// </summary>
        /// <param name="pdLine"></param>
        /// <param name="testStation"></param>
        /// <param name="MB_SNo"></param>
        /// <param name="editor"></param>
        /// <param name="customerId"></param>
        /// <param name="AllowPass"></param>
        /// <param name="DefectStation"></param>
        /// <returns></returns>
        IList<string> InputMBSNoForLot(
            string pdLine,
            string testStation,
            string MB_SNo,
            string editor, string customerId, out string AllowPass, out string DefectStation);
        /// <summary>
        /// InputMBSNoTrans
        /// </summary>
        /// <param name="pdLine"></param>
        /// <param name="testStation"></param>
        /// <param name="MB_SNo"></param>
        /// <param name="editor"></param>
        /// <param name="customerId"></param>
        /// <param name="AllowPass"></param>
        /// <param name="DefectStation"></param>
        /// <returns></returns>
        IList<string> InputMBSNoTrans(
            string pdLine, 
            string testStation, 
            string MB_SNo, 
            string editor, string customerId, out string AllowPass, out string DefectStation);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pdLine"></param>
        /// <param name="testStation"></param>
        /// <param name="MB_SNo"></param>
        /// <param name="editor"></param>
        /// <param name="customerId"></param>
        /// <returns></returns>
        string InputMBSNo(
            string pdLine,
            string testStation,
            string MB_SNo,
            string editor, string customerId);
       
        /// <summary>
        /// 1.1	UC-PCA-PTS-01 PCA Test Station
        /// 记录Function测试结果:如有不良,还需记录不良信息.
        /// </summary>
        /// <param name="MB_SNo">MB SNO</param>
        /// <param name="defectList">Defect IList</param>
        void InputDefectCodeList(
            string MB_SNo,
            IList<string> defectList);
        /// <summary>
        /// InputDefectCodeListForLot
        /// </summary>
        /// <param name="MB_SNo"></param>
        /// <param name="defectList"></param>
        void InputDefectCodeListForLot(
            string MB_SNo,
            IList<string> defectList,
            bool FruCheck);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="MB_SNo"></param>
        /// <param name="defectList"></param>
        void InputDefectCodeListTrans(
            string MB_SNo, 
            IList<string> defectList);
        
        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="sessionKey"></param>
        void Cancel(string sessionKey);
        /// <summary>
        /// GetLotInfoLst
        /// </summary>
        /// <param name="pdLine"></param>
        /// <returns></returns>
        IList<LotInfo> GetLotInfoLst(string pdLine);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="SelLotNoList"></param>
        /// <param name="pdline"></param>
        /// <returns></returns>
        IList<LotInfo> UpdateSelectLotStatus(IList<string> SelLotNoList, string pdline);

    }
}
