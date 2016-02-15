using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;
using System.Collections;
namespace IMES.Station.Interface.StationIntf
{
    /// <summary>
    /// 刷入Image D/L完成的整机。
    /// 目的：Check Image D/L是否Pass
    /// </summary>
    public interface IIMAGEOutput
    {
       
        string  Save(string prodId,string version);
        
        /// <summary>
        /// 輸入Customer SN 取得Product ID
        /// </summary>
        /// <param name="pdLine"></param>
        /// <param name="prodId"></param>
        /// <param name="editor"></param>
        /// <param name="stationId"></param>
        /// <param name="customerId"></param>

        ArrayList InputCustsn(
         string custsn,
         string editor, string stationId, string customerId);
        
        
        /// <summary>
        /// 输入ProductId和相关信息, 然后卡站, Check D/L Pass
        /// </summary>
        /// <param name="pdLine">Pd Line</param>
        /// <param name="prodId">Product Id</param>
        /// <param name="editor">operator</param>
        void InputProdId(
            string pdLine,
            string prodId,
            string editor, string stationId, string customerId);
     

        /// <summary>
        /// 得到当前线别在7:50之前的各model的pass数量，以show modal窗口形式显示
        /// 得到pass数量：
        /// 从ProductLog获取，条件：
        /// A.	Status=1
        /// B.	StationID=67(Image Output站)
        /// C.	Cdt范围：若当前时间大于7:50，则Cdt>当天日期的7:50；否则Cdt在前一天日期的7:50和当天日期的7:50之间
        /// </summary>
        /// <param name="pdLine">pdLine</param>
        /// <param name="editor">Operator</param>
        /// <returns>Model, Qty pair</returns>
        IList<ModelPassQty> Query(
            string pdLine,
            string editor, string stationId, string customerId);

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="sessionKey"></param>
        void Cancel(string sessionKey);
        ArrayList GetProdidLineVersion(string custsn, string editor, string stationId, string customerId);
        void SaveDownloadImgResult(string prodId, bool IsSuccessDownload, IList<string> defectList);
    }

}
