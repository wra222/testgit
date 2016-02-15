using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using IMES.DataModel;

namespace IMES.Station.Interface.StationIntf
{
    /// <summary>
    /// 将流程卡和Kitting Box结合，放入，一起投入kitting生产线
    /// 目标：建立PrdId和Box ID的对应关系，以实现通过kitting系统分拣Key parts
    /// </summary>
    public interface IOfflineLabelPrint
    {
        /// <summary>
        /// get_offline_label_list.
        /// </summary>
        /// <returns></returns>
        IList<OfflineLableSettingDef> Get_offline_lable_list();

        /// <summary>
        /// 输入PdLine和ProdId并检查
        /// </summary>
        /// <param name="pdLine">Pd Line</param>
        /// <param name="prodId">Product Id</param>
        /// <param name="editor">operator</param>
        ArrayList InputProdId(
            string pdLine,
            string prodId,
            string editor,
            string stationId,
            string customerId);

        /// <summary>
        /// 输入BoxId并绑定

        /// </summary>
        /// <param name="prodId">Product Id</param>
        /// <param name="boxId"></param>
        void InputBoxId(
            string prodId,
            string boxId);

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="sessionKey"></param>
        void Cancel(string sessionKey);
    }
}
