//created by itc206070

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using IMES.Station.Interface.StationIntf;


using IMES.DataModel;

using System.Data.SqlClient;
using System.Data;
using System.Collections;


namespace IMES.Station.Interface.StationIntf
{
    /// <summary>
    /// [PAK AoiOfflineKbCheck] 实现如下功能：
    /// 处理Product手动确认不良的业务
    /// </summary>
    public interface IAoiOfflineKbCheck 
    {
        /// <summary>
        /// 输入Product Id相关信息并处理
        /// </summary>
        /// <param name="pdLine">Pd Line</param>
        /// <param name="custsn">Cust Sn</param>
        /// <param name="editor">Editor</param>
        /// <param name="stationId">Station</param>
        /// <param name="customer">Customer</param>
        /// <returns>prestation</returns>
        ArrayList InputCustSn(
            string pdLine,
            string custsn,
            string editor, string stationId, string customer);

        /// <summary>
        /// 不良品信息。
        /// </summary>
        /// <param name="custsn">Cust Sn</param>
        /// <param name="defectList">Defect IList</param>
        void Save(string custsn,IList<string>defectList, string reason);
      
     
        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="sessionKey"></param>
        void Cancel(string sessionKey);

    }
}
