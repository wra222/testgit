using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;
using System.Collections;

namespace IMES.Station.Interface.StationIntf
{
    public interface ICheckAst
    {
        /// <summary>
        /// 获取Product的Model和BOM信息
        /// </summary>
        /// <param name="input">ProductId/CustSN</param>
        /// <param name="line">Product Line</param>
        /// <param name="editor">Operator</param>
        /// <param name="station">Station</param>
        /// <param name="customer">Customer</param>
        /// <param name="model">(out)Model</param>
        ArrayList InputProductIDorCustSN(string input, string line, string editor, string station, string customer, bool bQuery, string iamge1ATtype, string iamage2attype, out string prodID, out string model);
        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="sesKey">SessionKey(ProdId)</param>
        void Cancel(string sesKey);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <param name="checkValue"></param>
        /// <returns></returns>
        MatchedPartOrCheckItem TryPartMatchCheck(string sessionKey, string checkValue);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sesKey"></param>
        void Save(string sesKey);
    }
}
