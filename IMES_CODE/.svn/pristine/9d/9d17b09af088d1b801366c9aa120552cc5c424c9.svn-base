using System.Collections.Generic;
using IMES.Station.Interface.CommonIntf;
using IMES.DataModel;
using System.Collections;

namespace IMES.Station.Interface.StationIntf
{
    
    public interface ICombineCarton_CR
    {
        /// <summary>
        /// 第一次输入ProdId
        /// </summary>
        ArrayList inputProductFirst(string pdLine, string input, string Station, string editor, string customerId);

        /// <summary>
        /// 第二次输入ProdId
        /// </summary>
        ArrayList inputProductOther(string input, string FirstProductID);

        ArrayList Reprint(string cartonsn, string reason, string line, string editor, string station, string customer, string pCode, IList<PrintItem> printItems);

        IList<PrintItem> Save(string FirstProductID,IList<PrintItem> printItemLst, out string cartonsn);

        void ClearPart(string sessionKey);

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="sessionKey"></param>
        void Cancel(string sessionKey);

        IList<ConstValueTypeInfo> GetQtySelect();
   }
}
