using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Station.Interface.CommonIntf;
using IMES.DataModel;
using System.Collections;

namespace IMES.Station.Interface.StationIntf
{
    /// <summary>
    /// 根据生产排程生成的Mo列印PrdId label，并贴附在Travel Card上
    /// 目标：建立Mo与PrdId的对应关系，实现整机生产流程的管控
    /// </summary>
    public interface IProIdPrint
    {
        /// <summary>
        /// 根据产线生产安排，打印travel card
        /// B.	在输入时需要根据MO进行管控，Travel card上会同时打印ProdId，开始online作业
        /// 目的：打印travel card，作流线管控使用
        /// </summary>
        /// <param name="pdLine">Pd Line</param>
        /// <param name="mo">Product MO</param>
        /// <param name="qty">打印数量</param>
        /// <param name="ecr">ECR</param>
        /// <param name="month">月</param>
        /// <param name="editor">operator</param>
        /// <returns>Print Items</returns>
        ArrayList  PrintProId(
            string pdLine,
            string mo,
            int qty,
            string ecr,
            int month,
            string pCode,
            string editor, string stationId, string customerId, IList<PrintItem> printItems);

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="sessionKey"></param>
        void Cancel(string sessionKey);
    }

    
}
